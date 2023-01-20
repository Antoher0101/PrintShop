using System.Windows;

namespace PrintShop
{
	public partial class AboutWindow : Window
	{
		public string Version { get; set; } = PrintShop.Properties.Settings.Default.Version;
		public AboutWindow()
		{
			this.DataContext = this;
			InitializeComponent();
		}
	}
}
