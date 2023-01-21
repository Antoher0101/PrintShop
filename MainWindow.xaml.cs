using PrintShop.core;
using System.Windows;
namespace PrintShop
{
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext("Data Source=./db.db");
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
