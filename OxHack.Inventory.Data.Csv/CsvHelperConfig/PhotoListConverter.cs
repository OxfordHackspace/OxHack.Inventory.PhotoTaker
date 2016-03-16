using CsvHelper.TypeConversion;
using OxHack.Inventory.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OxHack.Inventory.Data.Csv.CsvHelperConfig
{
	internal class PhotoListConverter : ITypeConverter
	{
		public bool CanConvertFrom(Type type)
		{
			return PhotoListConverter.IsValidConversionType(type);
		}

		public bool CanConvertTo(Type type)
		{
			return PhotoListConverter.IsValidConversionType(type);
		}

		private static bool IsValidConversionType(Type type)
		{
			return type == typeof(String) || type == typeof(IEnumerable<String>);
		}

		public object ConvertFromString(TypeConverterOptions options, string text)
		{
			var result =
				text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(item => item.Trim())
					.ToList();

			return result;
		}

		public string ConvertToString(TypeConverterOptions options, object value)
		{
			IEnumerable<String> photoList = value as IEnumerable<String>;

			var result = String.Join(";", photoList ?? Enumerable.Empty<String>());

			return result;
		}
	}
}