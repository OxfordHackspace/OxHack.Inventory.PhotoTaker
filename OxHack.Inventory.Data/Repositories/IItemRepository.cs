﻿using OxHack.Inventory.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxHack.Inventory.Data.Repositories
{
	public interface IItemRepository
	{
		Task<IReadOnlyCollection<Item>> GetAllItemsAsync();

		Task<Item> GetByIdAsync(int id);

		Task RemoveItemPhotoAsync(int itemId, string photoFilename);

		Task AddItemPhotoAsync(int itemId, string photoFilename);
	}
}
