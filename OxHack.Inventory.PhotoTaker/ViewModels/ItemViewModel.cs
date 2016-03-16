using OxHack.Inventory.Data.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Prism.Events;
using OxHack.Inventory.PhotoTaker.Events;
using OxHack.Inventory.Data.Csv.Repositories;

namespace OxHack.Inventory.PhotoTaker.ViewModels
{
	public class ItemViewModel : BindableBase
	{
		private Item model;
		private readonly string pathToImages;
		private ItemRepository itemRepository;

		public ItemViewModel(string pathToImages, ItemRepository itemRepository, IEventAggregator eventAggregator)
		{
			this.pathToImages = pathToImages;
			this.itemRepository = itemRepository;

			eventAggregator.GetEvent<PictureTaken>().Subscribe(this.HandlePictureTaken, ThreadOption.UIThread);

			this.model = new Item();

			this.RemovePictureCommand = new DelegateCommand<string>(this.HandlePictureRemoved);
		}

		private async void HandlePictureRemoved(String image)
		{
			await this.itemRepository.RemoveItemPhotoAsync(this.model.Id, Path.GetFileName(image));

			var updatedModel = await this.itemRepository.GetByIdAsync(this.model.Id);

			this.Load(updatedModel);
		}

		private async void HandlePictureTaken(FileInfo pictureFile)
		{
			if (this.model != null && this.model.Id != default(int))
			{
				await this.itemRepository.AddItemPhotoAsync(this.model.Id, pictureFile.Name);

				var updatedModel = await this.itemRepository.GetByIdAsync(this.model.Id);

				this.Load(updatedModel);
			}
		}

		public void Load(Item item)
		{
			if (item != null)
			{
				this.model = item;
				this.Photos = new ObservableCollection<string>(this.model.Photos.Select(filename => Path.Combine(this.pathToImages, filename)) ?? Enumerable.Empty<String>());
				base.OnPropertyChanged(() => this.Id);
				base.OnPropertyChanged(() => this.AssignedLocation);
				base.OnPropertyChanged(() => this.Name);
				base.OnPropertyChanged(() => this.Photos);
				base.OnPropertyChanged(() => this.Appearance);
			}
		}

		public DelegateCommand<String> RemovePictureCommand
		{
			get;
			private set;
		}

		public int Id
		{
			get
			{
				return this.model.Id;
			}
		}

		public String AssignedLocation
		{
			get
			{
				return this.model.AssignedLocation;
			}
		}

		public String Name
		{
			get
			{
				return this.model.Name;
			}
		}

		public ObservableCollection<String> Photos
		{
			get;
			private set;
		}

		public String Appearance
		{
			get
			{
				return this.model.Appearance;
			}
		}
	}
}
