using CsvHelper.Configuration;
using OxHack.Inventory.Data.Models;
using System;
using System.Linq;

namespace OxHack.Inventory.Data.Csv.CsvHelperConfig
{
	public class CsvItemClassMap : CsvClassMap<CsvItem>
	{
		public CsvItemClassMap()
		{
			base.Map(item => item.Id).Index(0);
			base.Map(item => item.Name).Index(1);
			base.Map(item => item.Manufacturer).Index(2);
			base.Map(item => item.Model).Index(3);
			base.Map(item => item.Quantity).Default(0).Index(4);
			base.Map(item => item.Category).Index(5);
			base.Map(item => item.Spec).Index(6);
			base.Map(item => item.Appearance).Index(7);
			base.Map(item => item.AssignedLocation).Index(8);
			base.Map(item => item.CurrentLocation).Index(9);
			base.Map(item => item.IsLoan).Default(false).Index(10);
			base.Map(item => item.Origin).Index(11);
			base.Map(item => item.AdditionalInformation).Index(12);
			base.Map(item => item.Photos).Index(13).TypeConverter<PhotoListConverter>();
		}
	}
}
