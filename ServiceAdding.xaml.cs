using PrintShop.core;
using PrintShop.models;
using PrintShop.Repository;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PrintShop
{
    /// <summary>
    /// Логика взаимодействия для ServiceAdding.xaml
    /// </summary>
    public partial class ServiceAdding : Window
    {
        TotalService Total { get; set; }
        ApplicationContext Context { get; set; }
        DbRepository<ServiceInfo> ServiceInfoRepository { get; set; }
        ObservableCollection<ServiceInfo> ServicesInfo { get; set; } = new ObservableCollection<ServiceInfo>();
        DbRepository<Service> ServiceRepository { get; set; }
        ObservableCollection<Service> Services { get; set; } = new ObservableCollection<Service>();

        public ServiceAdding(TotalService totalService, ApplicationContext context)
        {
            Total = totalService;
            Context = context;
            InitializeComponent();

            ServiceInfoRepository = new DbRepository<ServiceInfo>(Context);
            ServiceRepository = new DbRepository<Service>(Context);


            RefreshCollections();

            ServiceInfoAddBox.ItemsSource = ServicesInfo;
            ServiceDataGrid.ItemsSource = Services;

            ClientTextBox.Text = totalService.Client.ToString();
            EmployeeTextBox.Text = totalService.Employee.ToString();
        }
        private void RefreshCollections()
        {
            ServicesInfo.Clear();
            Services.Clear();
            foreach (ServiceInfo item in ServiceInfoRepository.Items)
            {
                ServicesInfo.Add(item);
            }
            foreach (Service item in ServiceRepository.Items)
            {
                if (item.IdTotalService == Total.Id)
                    Services.Add(item);
            }
        }

        private void AddService(object sender, RoutedEventArgs e)
        {
            bool valid = true;

            var serviceInfo = ServiceInfoAddBox.SelectedIndex;
            var countBox = CountBox.Text;

            // Восстановление стиля
            ServiceInfoAddBox.Style = (Style)Application.Current.Resources["ComboBox"];
            CountBox.Style = (Style)Application.Current.Resources["TextBox"];

            // Валидация ввода

            if (serviceInfo < 0)
            {
                ServiceInfoAddBox.Style = (Style)Application.Current.Resources["RedComboBox"];
                valid = false;
            }
            if (string.IsNullOrEmpty(countBox))
            {
                CountBox.Style = (Style)Application.Current.Resources["RedTextBox"];
                valid = false;
            }

            if (valid)
            {
                Service s = new Service()
                {
                    Count = int.Parse(CountBox.Text),
                    IdTotalService = Total.Id,
                    IdService = (ServiceInfoAddBox.SelectedItem as ServiceInfo).Id
                };
                ServiceRepository.Add(s);
                CountBox.Text = string.Empty;
                RefreshCollections();
            }
        }

        private void CountBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex _regex = new Regex("[^0-9.-]+");

            if(_regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
