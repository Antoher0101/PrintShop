using Microsoft.EntityFrameworkCore;
using PrintShop.core;
using PrintShop.models;
using PrintShop.Repository;
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
            DbRepository<Client> clientsRepository = new DbRepository<Client>(db);
            DbRepository<Discount> discountRepository = new DbRepository<Discount>(db);
            DbRepository<DiscountInfo> discountInfoRepository = new DbRepository<DiscountInfo>(db);

            Console.WriteLine(clientsRepository.GetAll());
        }
    }
}
