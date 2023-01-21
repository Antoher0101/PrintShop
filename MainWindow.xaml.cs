using PrintShop.core;
using PrintShop.models;
using System;
using System.Linq;
using System.Windows;
namespace PrintShop
{
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {
            InitializeComponent();
            foreach (var item in db.Clients.AsEnumerable())
            {
                Console.WriteLine(item);
            }
        }
    }
}
