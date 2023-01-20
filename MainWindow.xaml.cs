﻿using Microsoft.Win32;

using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CardFilePBX
{
	public partial class MainWindow : Window
	{
		private DataListApplication dl;
		private AbonentInfo InfoWindow;
		public MainWindow()
		{
			InitializeComponent();

			dl = new DataListApplication();
			this.DataContext = dl;
			// Подключение БД и инициализация таблицы
			dl.UpdateTable();
			ConnectionDB(this, new RoutedEventArgs());
		}

		private void AddAbonent(object sender, RoutedEventArgs e)
		{
			TabMenu.SelectedIndex = 0;
			Random r = new Random();
			bool valid = true;

			var firstName = NameAddBox.Text;
			var lastName = LastNameAddBox.Text;
			var patronymic = PatronymicAddBox.Text;
			var phoneNumber = PhoneNumberAddBox.Text;
			var tariff = TariffAddBox.SelectedIndex;

			// Восстановление стиля
			NameAddBox.Style = (Style)Application.Current.Resources["TextBox"];
			LastNameAddBox.Style = (Style)Application.Current.Resources["TextBox"]; ;
			PhoneNumberAddBox.Style = (Style)Application.Current.Resources["TextBox"];
			TariffAddBox.Style = (Style)Application.Current.Resources["ComboBox"];

			// Валидация ввода
			if (string.IsNullOrEmpty(firstName))
			{
				NameAddBox.Style = (Style)Application.Current.Resources["RedTextBox"];
				valid = false;
			}
			if (string.IsNullOrEmpty(lastName))
			{
				LastNameAddBox.Style = (Style)Application.Current.Resources["RedTextBox"];
				valid = false;
			}
			if (string.IsNullOrEmpty(patronymic))
			{
				patronymic = "-";
			}
			Regex ex = new Regex(@"^(\+\d)(\(\d{3}\)\d{3})(\-)(\d{2}\-\d{2})$");
			if (!ex.IsMatch(phoneNumber))
			{
				PhoneNumberAddBox.Style = (Style)Application.Current.Resources["RedTextBox"];
				valid = false;
			}
			if (tariff < 0)
			{
				TariffAddBox.Style = (Style)Application.Current.Resources["RedComboBox"];
				valid = false;
			}

			if (valid)
			{
				dl.AddAbonent(firstName, lastName, patronymic, phoneNumber, tariff.ToString(), DataListApplication.GetTotalCallTime(r), DataListApplication.GetTotalCallTime(r));
				dl.UpdateTable();
				NameAddBox.Text = string.Empty;
				LastNameAddBox.Text = string.Empty;
				PatronymicAddBox.Text = string.Empty;
				PhoneNumberAddBox.Text = string.Empty;
				TariffAddBox.SelectedIndex = -1;
			}
		}

		private void QuestionMark_Click(object sender, RoutedEventArgs e)
		{
			var about = new AboutWindow();
			about.Owner = this;
			about.Show();
		}
		private void RefreshBtn_Click(object sender, RoutedEventArgs e)
		{
			dl.UpdateTable();
		}

		// Наименование и ширина столбцов в таблице
		private void AbonentsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			var col = e.Column.Header.ToString();
			e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
			switch (col)
			{
				case "Id":
					e.Column.Header = "ИН";
					e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
					break;
				case "Name":
					e.Column.Header = "Имя";
					break;
				case "LastName":
					e.Column.Header = "Фамилия";
					break;
				case "Patronymic":
					e.Column.Header = "Отчество";
					break;
				case "PhoneNumber":
					e.Column.Header = "Номер телефона";
					break;
				case "Tariff":
					e.Column.Header = "Тариф";
					break;
				default:
					e.Column.Visibility = Visibility.Collapsed;
					break;
			}
		}
		private void AbonentViewChanged(object sender, EventArgs e)
		{
			var data = AbonentsDataGrid.CurrentCell.Item as DataRowView;
			if (data != null)
			{
				var curr = dl.SelectById((int)data.Row[0]);
				dl.AbonentView = curr;
				AbonentCard.IsSelected = true;
			}
		}
		private void ConnectionDB(object sender, RoutedEventArgs args)
		{
			// Проверка доступности БД
			if (dl.CheckDB())
			{
				string dbName = Path.GetFileNameWithoutExtension(dl.connectionString);
				dl.State = ConnectionState.Open;
				if (sender != this)
				{
					MessageBox.Show($"Успешное подключение к базе данных {dbName}.", "Подключение установлено", MessageBoxButton.OK);
				}
				return;
			}
			else dl.State = ConnectionState.Broken;
			if(!dl.FirstStart) 
				MessageBox.Show("Невозможно подключиться к базе данных", "Подключение не установлено", MessageBoxButton.OK);
		}
		private void SaveDB(object sender, RoutedEventArgs e)
		{
			dl.WriteDatabase();
		}
		private void TextPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex(@"[0-9]");
			if (regex.Match(e.Text).Success)
			{
				e.Handled = true;
			}
		}
		private void PhoneNumberBox_GotFocus(object sender, RoutedEventArgs e)
		{
			((Xceed.Wpf.Toolkit.MaskedTextBox)sender).CaretIndex = 3;
		}

		private async void EditButton_Click(object sender, RoutedEventArgs e)
		{
			TabMenu.SelectedIndex = 2;
			if (dl.AbonentView is null)
			{
				await Task.Run(() =>
				{
					NullSelectionPopup.Dispatcher.Invoke(() => NullSelectionPopup.IsOpen = true);
					while (dl.AbonentView is null)
					{
					}
					NullSelectionPopup.Dispatcher.Invoke(() => NullSelectionPopup.ClearValue(Popup.IsOpenProperty));
					dl.IsNoEditing = false;
				});
			}
			else dl.IsNoEditing = false;
		}
		private void ConfirmEditing(object sender, RoutedEventArgs e)
		{
			dl.AbonentView.Tariff = dl.TariffConverter(TariffChangeComboBox.SelectedIndex.ToString());
			var dialogResult = MessageBox.Show("Вы действительно изменить данные?", "Изменение данных абонента", MessageBoxButton.YesNo);
			if (dialogResult == MessageBoxResult.Yes)
			{
				dl.EditAbonentData();
			}
			dl.IsNoEditing = true;
			dl.AbonentView = null;
			dl.UpdateTable();
		}

		private async void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			TabMenu.SelectedIndex = 2;
			if (!dl.IsNoEditing) {
				dl.IsNoEditing = true;
				dl.AbonentView = null;
				dl.UpdateTable();
				return;
			}
			if (dl.AbonentView is null)
			{
				await Task.Run(() =>
				{
					NullSelectionPopup.Dispatcher.Invoke(() => NullSelectionPopup.IsOpen = true);
					while (dl.AbonentView is null)
					{
					}
					NullSelectionPopup.Dispatcher.Invoke(() => NullSelectionPopup.ClearValue(Popup.IsOpenProperty));
				});
			}
			var dialogResult = MessageBox.Show("Вы действительно хотите удалить абонента из базы данных?",
					"Удаление из базы данных", MessageBoxButton.YesNo);
				if (dialogResult == MessageBoxResult.Yes)
				{
					dl.DeleteAbonent(dl.AbonentView.Id);
					dl.AbonentView = null;
				}
				dl.UpdateTable();
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			dl.UpdateTable();
			string query = "";
			if (SearchNameBox.Text != "")
			{
				query = "Name LIKE '%" + SearchNameBox.Text.Trim() + "%' AND ";
			}
			if (SearchLastNameBox.Text != "")
			{
				query += "LastName LIKE '%" + SearchLastNameBox.Text.Trim() + "%' AND ";
			}
			if (SearchPatronymicBox.Text != "")
			{
				query += "Patronymic LIKE '%" + SearchPatronymicBox.Text.Trim() + "%' AND ";
			}
			if (SearchPhoneNumberBox.IsMaskFull)
			{
				query += "PhoneNumber LIKE '%" + SearchPhoneNumberBox.Text.Trim() + "%'";
			}
			if (query.EndsWith("AND "))
			{
				query = query.Remove(query.Length - 4);
			}
			dl.SearchAbonent(query);
		}

		private void SetConnectionDB(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			if (openFileDialog1.ShowDialog() != true)
				return;
			dl.SetConnectionString(openFileDialog1.FileName);
			dl.UpdateTable();
		}

		private void CreateDB(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog()
			{
				DefaultExt = "*.db",
				AddExtension = true,
				Filter = "Data Base File(*.db)|*.db",
				InitialDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\database")),
			};
			if (dialog.ShowDialog() == true)
			{
				dl.CreateDbFile(dialog.FileName);
			}
		}

		private void GetAbonentInfo(object sender, RoutedEventArgs e)
		{
			if (InfoWindow is null)
			{
				InfoWindow = new AbonentInfo();
				InfoWindow.SetAbonent(dl.AbonentView);

				InfoWindow.Closed += (o, args) => InfoWindow = null;
				InfoWindow.Owner = this;
				InfoWindow.Show();
			}
			else
			{
				InfoWindow.AddInfoCard(dl.AbonentView);
			}
		}

		private void ExitProgram(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void AllInfo(object sender, RoutedEventArgs e)
		{
			var list = dl.GetAbonentList();
			if (list is null || list.Count == 0)
			{
				MessageBox.Show("В базе данных отсутсвует информация об абонентах.", "Информация отсутствует", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			InfoWindow = new AbonentInfo();
			InfoWindow.SetAbonent(list);
			InfoWindow.Closed += (o, args) => InfoWindow = null;
			InfoWindow.Owner = this;
			InfoWindow.Show();
		}

		private void ChangePopupLocation(object sender, MouseEventArgs e)
		{
			var mousePosition = e.GetPosition(this.window);
			this.NullSelectionPopup.HorizontalOffset = mousePosition.X + 15;
			this.NullSelectionPopup.VerticalOffset = mousePosition.Y + 15;
		}
	}
}