using CsvHelper;
using CsvHelper.Configuration;
using OxHack.Inventory.Data.Csv.CsvHelperConfig;
using OxHack.Inventory.Data.Csv.Extensions;
using OxHack.Inventory.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OxHack.Inventory.Data.Csv.Repositories
{
	internal class CsvDataSource
	{
		private FileInfo sourceFile;

		public CsvDataSource(FileInfo sourceFile)
		{
			this.sourceFile = sourceFile;
		}

		public IEnumerable<Item> ReadAll()
		{
			List<CsvItem> data = new List<CsvItem>();
			var config = CsvDataSource.GetCsvConfig();

			using (var reader = new CsvReader(this.sourceFile.OpenText(), config))
			{
				while (reader.Read())
				{
					try
					{
						data.Add(reader.GetRecord<CsvItem>());
					}
					catch (Exception e)
					{
					}
				}
			}

			return data.Select(item => item.ToImmutableItem()).ToList();
		}

		internal void WriteAll(IEnumerable<Item> data)
		{
			var csvItems = data.Select(item => item.ToCsvItem()).ToList();
			var config = CsvDataSource.GetCsvConfig();

			FileInfo targetFile;

			do
			{
				var targetFilename = DateTime.Now.ToString("s").Replace(':', '_') + ".csv";
				targetFile = new FileInfo(Path.Combine(this.sourceFile.DirectoryName, targetFilename));
			}
			while (targetFile.Exists);

			using (var writer = new CsvWriter(targetFile.CreateText(), config))
			{
				writer.WriteHeader(typeof(CsvItem));
				writer.WriteRecords(csvItems);
			}

			var convenienceFile = new FileInfo(Path.Combine(targetFile.DirectoryName, "_Latest.csv"));

			targetFile.Refresh();
			targetFile.CopyTo(convenienceFile.FullName, overwrite: true);
		}

		private static CsvConfiguration GetCsvConfig()
		{
			var config = new CsvConfiguration();
			config.RegisterClassMap<CsvItemClassMap>();
			return config;
		}
	}
}
