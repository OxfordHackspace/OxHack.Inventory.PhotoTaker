using OxHack.Inventory.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxHack.Inventory.Data.Models;
using Prism.Events;
using OxHack.Inventory.Data.Events;

namespace OxHack.Inventory.Data.Csv.Repositories
{
	public class ItemRepository : IItemRepository
	{
		private readonly IEventAggregator eventAggregator;
		private CsvDataSource dataSource;
		private List<Item> data;
		private bool isInitialized;

		internal ItemRepository(IEventAggregator eventAggregator, CsvDataSource dataSource)
		{
			this.eventAggregator = eventAggregator;
			this.dataSource = dataSource;

			this.isInitialized = false;
		}

		private void InitializeIfNeeded()
		{
			if (!this.isInitialized)
			{
				this.data = dataSource.ReadAll().ToList();
				this.isInitialized = true;
			}
		}

		public async Task<IReadOnlyCollection<Item>> GetAllItemsAsync()
		{
			this.InitializeIfNeeded();
			return await Task.FromResult(this.data.ToList().AsReadOnly());
		}

		public async Task<Item> GetByIdAsync(int id)
		{
			this.InitializeIfNeeded();
			return await Task.FromResult(this.data.SingleOrDefault(item => item.Id == id));
		}

		public async Task AddItemPhotoAsync(int itemId, string photoFilename)
		{
			this.InitializeIfNeeded();

			if (String.IsNullOrWhiteSpace(photoFilename))
			{
				throw new ArgumentException("photoFilename");
			}

			var matchingItem = this.data.SingleOrDefault(item => item.Id == itemId);

			if (matchingItem != null)
			{
				var photoList = matchingItem.Photos.ToList();
				photoList.Add(photoFilename);

				matchingItem.Photos = photoList;

				this.dataSource.WriteAll(this.data);
				this.eventAggregator.GetEvent<ItemUpdated>().Publish(matchingItem);
			}
		}

		public async Task RemoveItemPhotoAsync(int itemId, string photoFilename)
		{
			this.InitializeIfNeeded();

			if (String.IsNullOrWhiteSpace(photoFilename))
			{
				throw new ArgumentException("photoFilename");
			}

			var matchingItem = this.data.SingleOrDefault(item => item.Id == itemId);

			bool photoRemoved = false;
			if (matchingItem != null)
			{
				var photoList = matchingItem.Photos.ToList();
				photoRemoved = photoList.Remove(photoFilename);

				matchingItem.Photos = photoList;
			}

			if (photoRemoved)
			{
				this.dataSource.WriteAll(this.data);
				this.eventAggregator.GetEvent<ItemUpdated>().Publish(matchingItem);
			}
		}
	}
}
