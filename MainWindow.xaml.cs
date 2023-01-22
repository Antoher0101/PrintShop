using Microsoft.EntityFrameworkCore;
using PrintShop.core;
using PrintShop.models;
using PrintShop.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrintShop
{
    public partial class MainWindow : Window
    {
        ApplicationContext db { get; set; } = new ApplicationContext();

        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<TotalService> TotalServices { get; set; } = new ObservableCollection<TotalService>();
        public ObservableCollection<DiscountInfo> DiscountInfos { get; set; } = new ObservableCollection<DiscountInfo>();
        public ObservableCollection<ServiceInfo> ServiceInfos { get; set; } = new ObservableCollection<ServiceInfo>();
        public ObservableCollection<Discount> Discounts { get; set; } = new ObservableCollection<Discount>();


        DbRepository<Client> clientsRepository { get; set; }
        DbRepository<Discount> discountRepository { get; set; }
        DbRepository<DiscountInfo> discountInfoRepository { get; set; }
        DbRepository<Employee> employeeRepository { get; set; }
        DbRepository<TotalService> totalServiceRepository { get; set; }
        DbRepository<ServiceInfo> serviceInfoRepository { get; set; }

        Window serviceAddingWindow;
        Window reportWindoow;
        public MainWindow()
        {
            clientsRepository = new DbRepository<Client>(db);
            discountRepository = new DbRepository<Discount>(db);
            discountInfoRepository = new DbRepository<DiscountInfo>(db);
            employeeRepository = new DbRepository<Employee>(db);
            totalServiceRepository = new DbRepository<TotalService>(db);
            serviceInfoRepository = new DbRepository<ServiceInfo>(db);
            RefreshCollections();

            InitializeComponent();
            this.DataContext = this;

            initDataGridView();

            ClientAddBox.ItemsSource = Clients;
            EmployeeAddBox.ItemsSource = Employees;
            DiscountAddBox.ItemsSource = Discounts;
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
            Employees.Clear();
            TotalServices.Clear();
            foreach (Employee item in employeeRepository.Items)
            {
                Employees.Add(item);
            }
            foreach (TotalService item in totalServiceRepository.Items)
            {
                TotalServices.Add(item);
            }
            RefreshDiscountInfos();
            RefreshClientCollection();
            RefreshServiceInfoCollection();
        }

        private void RefreshDiscountInfos()
        {
            DiscountInfos.Clear();
            foreach (DiscountInfo item in discountInfoRepository.Items)
            {
                DiscountInfos.Add(item);
            }
        }

        private void RefreshClientCollection()
        {
            Clients.Clear();
            foreach (Client item in clientsRepository.Items)
            {
                Clients.Add(item);
            }
        }

        private void RefreshServiceInfoCollection()
        {
            ServiceInfos.Clear();
            foreach (ServiceInfo item in serviceInfoRepository.Items)
            {
                ServiceInfos.Add(item);
            }
        }

        private void initDataGridView()
        {
            ServiceDataGrid.ItemsSource = TotalServices;
            ServiceInfoDataGrid.ItemsSource = ServiceInfos;
            ClientsDataGrid.ItemsSource = Clients;
        }

        private void AddService(object sender, RoutedEventArgs e)
        {
            TabMenu.SelectedIndex = 0;
            bool valid = true;

            var clientBox = ClientAddBox.SelectedIndex;
            var employeeBox = EmployeeAddBox.SelectedIndex;
            var discount = DiscountAddBox.SelectedIndex;

            // Восстановление стиля
            ClientAddBox.Style = (Style)Application.Current.Resources["ComboBox"];
            EmployeeAddBox.Style = (Style)Application.Current.Resources["ComboBox"];
            DiscountAddBox.Style = (Style)Application.Current.Resources["ComboBox"];

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
            if (discount < 0)
            {
                DiscountAddBox.Style = (Style)Application.Current.Resources["RedComboBox"];
                valid = false;
            }

            if (valid)
            {
                Client client = ClientAddBox.SelectedItem as Client;
                Employee employee = EmployeeAddBox.SelectedItem as Employee;

                TotalService totalService = totalServiceRepository.Add(new TotalService()
                {
                    Client = client,
                    Employee = employee,
                    Date = DateTime.Now.ToShortDateString(),
                    Discount = DiscountAddBox.SelectedItem as Discount,

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
                Client client = new Client()
                {
                    Name = firstName,
                    LastName = lastName,
                    MiddleName = patronymic,
                    Phone = phoneNumber,
                };
                if (clientsRepository.Add(client) != null)
                {
                    discountRepository.Add(new Discount()
                    {
                        IdClient = client.Id,
                        IdDiscount = 1
                    });

                    RefreshClientCollection();
                    RefreshDiscountInfos();
                    NameAddBox.Text = string.Empty;
                    LastNameAddBox.Text = string.Empty;
                    MiddleNameAddBox.Text = string.Empty;
                    PhoneNumberAddBox.Text = string.Empty;
                }
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
                ServiceInfo serviceinfo = new ServiceInfo()
                {
                    Name = name,
                    Price = double.Parse(price),
                    Format = format,
                    Type = type,
                    Paper = paper,
                };
                if (serviceInfoRepository.Add(serviceinfo) != null)
                {
                    RefreshServiceInfoCollection();
                    ServiceName.Text = string.Empty;
                    ServicePrice.Text = string.Empty;
                    ServiceFormat.Text = string.Empty;
                    ServiceType.Text = string.Empty;
                    ServicePaper.Text = string.Empty;
                }
            }
        }

        private void ClientAddBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Discounts.Clear();
            if (ClientAddBox.SelectedItem != null)
            {
                var client = ClientAddBox.SelectedItem as Client;
                foreach (var item in client.Discounts)
                {
                    Discounts.Add(item);
                }
            }
        }

        private void GenerateReport(object sender, RoutedEventArgs e)
        {
            reportWindoow = new ReportWindow(db);

            reportWindoow.Owner = this;
            reportWindoow.ShowDialog();
        }

        private void ServiceEditTab(object sender, SelectionChangedEventArgs e)
        {
            if (ServicesTab.IsSelected)
            {
                ServiceDataGrid.Visibility = Visibility.Collapsed;
                ClientsDataGrid.Visibility = Visibility.Collapsed;
                ServiceInfoDataGrid.Visibility = Visibility.Visible;

            }
            else if (NewClientsTab.IsSelected)
            {
                ClientsDataGrid.Visibility = Visibility.Visible;
                ServiceDataGrid.Visibility = Visibility.Collapsed;
                ServiceInfoDataGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ServiceDataGrid.Visibility = Visibility.Visible;
                ServiceInfoDataGrid.Visibility = Visibility.Collapsed;
                ClientsDataGrid.Visibility = Visibility.Collapsed;

            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var totalServices = db.TotalServices.AsEnumerable();
            TotalServices.Clear();
            if (SearchNameBox.Text != "")
            {
                totalServices = totalServices.Where(s => s.Client.Name.Contains(SearchNameBox.Text));
            }
            if (SearchLastNameBox.Text != "")
            {
                totalServices = totalServices.Where(s => s.Client.LastName.Contains(SearchLastNameBox.Text));
            }
            if (SearchPhoneNumberBox.Text != "")
            {
                totalServices = totalServices.Where(s => s.Client.Phone.Contains(SearchPhoneNumberBox.Text));
            }
            foreach (var item in totalServices)
            {
                TotalServices.Add(item);
            }
        }
    }
}
