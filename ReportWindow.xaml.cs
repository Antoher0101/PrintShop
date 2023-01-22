using ClosedXML.Excel;
using PrintShop.core;
using PrintShop.models;
using PrintShop.Repository;
using System;
using System.Linq;
using System.Windows;

namespace PrintShop
{
    public partial class ReportWindow : Window
    {
        public string StartDateF { get; set; } = DateTime.Now.AddMonths(-1).ToString("dd-MM-yyyy");
        public string EndDateF { get; set; } = DateTime.Now.ToString("dd-MM-yyyy");

        ApplicationContext Context;
        DbRepository<Service> serviceRepository;

        public ReportWindow(ApplicationContext context)
        {
            Context = context;
            serviceRepository = new DbRepository<Service>(Context);
            InitializeComponent();

            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var services = Context.Services.ToList().Where(s => (DateTime.Parse(s.TotalService.Date) >= DateTime.Parse(StartDateF)
            && DateTime.Parse(s.TotalService.Date) <= DateTime.Parse(EndDateF)));

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Отчет");

                //Заголовки у столбцов
                worksheet.Cell("A" + 1).Value = "Услуга";
                worksheet.Cell("B" + 1).Value = "Количество услуг";
                worksheet.Cell("C" + 1).Value = "Дата";
                int row = 2;

                foreach (var data in services)
                {
                    worksheet.Cell("A" + row).Value = data.ServiceInfo.ToString();
                    worksheet.Cell("B" + row).Value = data.Count;
                    worksheet.Cell("C" + row).Value = data.TotalService.Date;
                    row++;
                }

                worksheet.Columns().AdjustToContents(); //ширина столбца по содержимому

                workbook.SaveAs($"Отчет от {DateTime.Now.ToShortDateString()}.xlsx");
            }
            catch (Exception)
            {

            }
            finally
            {
                Close();
            }
            
        }
    }
}
