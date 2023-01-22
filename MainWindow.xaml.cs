using Microsoft.EntityFrameworkCore;
using PrintShop.core;
using PrintShop.models;
using PrintShop.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PrintShop
{
    public partial class MainWindow : Window
    {
        ApplicationContext db { get; set; } = new ApplicationContext();

        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<TotalService> TotalServices { get; set; } = new ObservableCollection<TotalService>();
        public ObservableCollection<DiscountInfo> DiscountInfos { get; set; } = new ObservableCollection<DiscountInfo>();
        

        DbRepository<Client> clientsRepository { get; set; }
        DbRepository<Discount> discountRepository { get; set; }
        DbRepository<DiscountInfo> discountInfoRepository { get; set; }
        DbRepository<Employee> employeeRepository { get; set; }
        DbRepository<TotalService> totalServiceRepository { get; set; }

        Window serviceAddingWindow;
        public MainWindow()
        {
            clientsRepository = new DbRepository<Client>(db);
            discountRepository = new DbRepository<Discount>(db);
            discountInfoRepository = new DbRepository<DiscountInfo>(db);
            employeeRepository = new DbRepository<Employee>(db);
            totalServiceRepository = new DbRepository<TotalService>(db);
            RefreshCollections();

            InitializeComponent();
            this.DataContext = this;

            initDataGridView();

            ClientAddBox.ItemsSource = Clients;
            EmployeeAddBox.ItemsSource = Employees;
            DiscountAddBox.ItemsSource = DiscountInfos;
        }

        private void window_closing(object sender, EventArgs e)
        {
            RefreshCollections();
        }

        private void ExitProgram(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RefreshCollections()
        {
            Clients.Clear();
            Employees.Clear();
            TotalServices.Clear();
            foreach (Client item in clientsRepository.Items)
            {
                Clients.Add(item);
            }
            foreach (Employee item in employeeRepository.Items)
            {
                Employees.Add(item);
            }
            foreach (TotalService item in totalServiceRepository.Items)
            {
                TotalServices.Add(item);
            }
        }

        private void initDataGridView()
        {
            ServiceDataGrid.ItemsSource = TotalServices;
        }

        private void AddService(object sender, RoutedEventArgs e)
        {
            TabMenu.SelectedIndex = 0;
            bool valid = true;

            var clientBox = ClientAddBox.SelectedIndex;
            var employeeBox = EmployeeAddBox.SelectedIndex;

            // Восстановление стиля
            ClientAddBox.Style = (Style)Application.Current.Resources["ComboBox"];
            EmployeeAddBox.Style = (Style)Application.Current.Resources["ComboBox"];

            // Валидация ввода
       
            if (clientBox < 0)
            {
                ClientAddBox.Style = (Style)Application.Current.Resources["RedComboBox"];
                valid = false;
            }
            if (employeeBox < 0)
            {
                EmployeeAddBox.Style = (Style)Application.Current.Resources["RedComboBox"];
                valid = false;
            }

            if (valid)
            {
                Client client = ClientAddBox.SelectedItem as Client;
                Employee employee = EmployeeAddBox.SelectedItem as Employee;

                TotalService totalService = totalServiceRepository.Add(new TotalService() { 
                    Client = client, 
                    Employee = employee,
                    Date = DateTime.Now.ToShortDateString(),
                    Discount = discountRepository.Add(new Discount()
                    {
                        Client= client,
                        DiscountInfo = DiscountAddBox.SelectedItem as DiscountInfo
                    })
                    
                });

                serviceAddingWindow = new ServiceAdding(totalService, db);
                serviceAddingWindow.Closed += new EventHandler(window_closing);

                serviceAddingWindow.Owner = this;
                serviceAddingWindow.ShowDialog();
            }
        }

        private void AddClient(object sender, RoutedEventArgs e)
        {
            TabMenu.SelectedIndex = 1;
            bool valid = true;

            var firstName = NameAddBox.Text;
            var lastName = LastNameAddBox.Text;
            var patronymic = MiddleNameAddBox.Text;
            var phoneNumber = PhoneNumberAddBox.Text;

            // Восстановление стиля
            NameAddBox.Style = (Style)Application.Current.Resources["TextBox"];
            LastNameAddBox.Style = (Style)Application.Current.Resources["TextBox"]; ;
            PhoneNumberAddBox.Style = (Style)Application.Current.Resources["TextBox"];

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
            if (string.IsNullOrEmpty(phoneNumber))
            {
                PhoneNumberAddBox.Style = (Style)Application.Current.Resources["RedTextBox"];
                valid = false;
            }
            if (valid)
            {
                
            }
        }

        private void AddServiceInfo(object sender, RoutedEventArgs e)
        {
            TabMenu.SelectedIndex = 3;
            bool valid = true;

            var name = ServiceName.Text;
            var price = ServicePrice.Text;
            var format = ServiceFormat.Text;
            var type = ServiceType.Text;
            var paper = ServicePaper.Text;

            // Восстановление стиля
            ServiceName.Style = (Style)Application.Current.Resources["TextBox"];
            ServicePrice.Style = (Style)Application.Current.Resources["TextBox"]; ;
            ServiceFormat.Style = (Style)Application.Current.Resources["TextBox"];
            ServiceType.Style = (Style)Application.Current.Resources["TextBox"];
            ServicePaper.Style = (Style)Application.Current.Resources["TextBox"]; ;

            // Валидация ввода
            if (string.IsNullOrEmpty(name))
            {
                ServiceName.Style = (Style)Application.Current.Resources["RedTextBox"];
                valid = false;
            }
            if (string.IsNullOrEmpty(price))
            {
                ServicePrice.Style = (Style)Application.Current.Resources["RedTextBox"];
                valid = false;
            }
            if (string.IsNullOrEmpty(format))
            {
                ServiceFormat.Style = (Style)Application.Current.Resources["RedTextBox"];
                valid = false;
            }
            if (string.IsNullOrEmpty(type))
            {
                ServiceType.Style = (Style)Application.Current.Resources["RedTextBox"];
                valid = false;
            }
            if (string.IsNullOrEmpty(paper))
            {
                ServicePaper.Style = (Style)Application.Current.Resources["RedTextBox"];
                valid = false;
            }
            if (valid)
            {

            }
        }

        private void ClientAddBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DiscountInfos.Clear();
            if (ClientAddBox.SelectedItem!= null)
            {
                var client = ClientAddBox.SelectedItem as Client;
                foreach (var item in client.Discounts)
                {
                    DiscountInfos.Add(item.DiscountInfo);
                }
            }
        }
    }
}
