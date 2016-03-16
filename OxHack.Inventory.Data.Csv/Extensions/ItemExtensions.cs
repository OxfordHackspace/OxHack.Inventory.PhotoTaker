using OxHack.Inventory.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxHack.Inventory.Data.Csv.Extensions
{
	internal static class ItemExtensions
	{
		public static Item ToImmutableItem(this CsvItem source)
		{
			return
				new Item()
				{
					Id = source.Id,
					Name = source.Name,
					Manufacturer = source.Manufacturer,
					Model = source.Model,
					Quantity = source.Quantity,
					Category = source.Category,
					Spec = source.Spec,
					Appearance = source.Appearance,
					AssignedLocation = source.AssignedLocation,
					CurrentLocation = source.CurrentLocation,
					IsLoan = source.IsLoan,
					Origin = source.Origin,
					AdditionalInformation = source.AdditionalInformation,
					Photos = source.Photos.ToList()
				};
		}

		public static CsvItem ToCsvItem(this Item source)
		{
			return
				new CsvItem()
				{
					Id = source.Id,
					Name = source.Name,
					Manufacturer = source.Manufacturer,
					Model = source.Model,
					Quantity = source.Quantity,
					Category = source.Category,
					Spec = source.Spec,
					Appearance = source.Appearance,
					AssignedLocation = source.AssignedLocation,
					CurrentLocation = source.CurrentLocation,
					IsLoan = source.IsLoan,
					Origin = source.Origin,
					AdditionalInformation = source.AdditionalInformation,
					Photos = source.Photos.ToList()
				};
		}
	}
}
