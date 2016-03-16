using OxHack.Inventory.Data.Csv.Repositories;
using OxHack.Inventory.PhotoTaker.ViewModels;
using Prism.Events;
using System.Windows;

namespace OxHack.Inventory.PhotoTaker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

			var eventAggregator = new EventAggregator();

			this.DataContext = 
				new MainViewModel(
					new ItemRepositoryFactory(eventAggregator).CreateInstance(), 
					this.Dispatcher,
					eventAggregator);

			this.Loaded += (s, e) =>
			{
				(this.DataContext as MainViewModel)?.LoadItems();
				this.Activate();
			};
		}
	}
}
