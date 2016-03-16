using OxHack.Inventory.Data.Csv.Repositories;
using OxHack.Inventory.Data.Events;
using OxHack.Inventory.Data.Models;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using System.Linq;

namespace OxHack.Inventory.PhotoTaker.ViewModels
{
	class MainViewModel : BindableBase
	{
		private ItemRepository itemRepository;
		private ObservableCollection<Item> items;
		private Item selectItem;

		public MainViewModel(ItemRepository itemRepository, Dispatcher dispatcher, IEventAggregator eventAggregator)
		{
			this.itemRepository = itemRepository;

			var assemblyFileInfo = new FileInfo(Assembly.GetEntryAssembly().FullName);
			var pathToImages = Path.Combine(assemblyFileInfo.DirectoryName, "Files\\Images\\");

			eventAggregator.GetEvent<ItemUpdated>().Subscribe(this.HandleItemUpdated);

			this.CapturePhotoViewModel = new CapturePhotoViewModel(pathToImages, dispatcher, eventAggregator);
			this.SelectedItemViewModel = new ItemViewModel(pathToImages, itemRepository, eventAggregator);
		}

		// hacky crap to refresh datagrid
		private void HandleItemUpdated(Item updatedItem)
		{
			var index = this.Items.IndexOf(updatedItem);
			this.Items.RemoveAt(index);
			this.Items.Insert(index, updatedItem);
			this.SelectedItem = updatedItem;
		}

		public CapturePhotoViewModel CapturePhotoViewModel
		{
			get;
			private set;
		}

		public ObservableCollection<Item> Items
		{
			get
			{
				return this.items;
			}
			private set
			{
				base.SetProperty(ref this.items, value);
			}
		}

		public Item SelectedItem
		{
			get
			{
				return this.selectItem;
			}
			set
			{
				base.SetProperty(ref this.selectItem, value);

				this.SelectedItemViewModel.Load(this.SelectedItem);
				base.OnPropertyChanged(() => this.SelectedItemViewModel);
			}
		}

		public ItemViewModel SelectedItemViewModel
		{
			get;
			private set;
		}

		internal async void LoadItems()
		{
			this.Items = new ObservableCollection<Item>(await this.itemRepository.GetAllItemsAsync());
			this.SelectedItem = this.Items.FirstOrDefault();
		}
	}
}
