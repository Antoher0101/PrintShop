using PrintShop.core;
using PrintShop.models;
using PrintShop.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrintShop
{
    /// <summary>
    /// Логика взаимодействия для EmployeeStat.xaml
    /// </summary>
    public partial class EmployeeStat : Window
    {
        ApplicationContext Context;
        DbRepository<Employee> employeeRepository { get; set; }
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();


        public EmployeeStat(ApplicationContext context)
        {
            Context = context;
            employeeRepository = new DbRepository<Employee>(Context);
            InitializeComponent();
            this.DataContext = this;
            RefreshEmployees();
            TotalsDataGrid.ItemsSource = Employees;

        }
        private void RefreshEmployees()
        {
            Employees.Clear();
            foreach (Employee item in employeeRepository.Items)
            {
                Employees.Add(item);
            }
        }
    }
}
