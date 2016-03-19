using Prism.Events;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace OxHack.Inventory.Data.Csv.Repositories
{
	public class ItemRepositoryFactory
	{
		private readonly IEventAggregator eventAggregator;

		public ItemRepositoryFactory(IEventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;
		}

		public ItemRepository CreateInstance()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "CSV Files (*.csv)|*.csv";
			dialog.RestoreDirectory = true;
			dialog.Multiselect = false;

			var assmeblyFile = new FileInfo(Assembly.GetEntryAssembly().Location);
			var file = new FileInfo(Path.Combine(assmeblyFile.DirectoryName, "..\\Files", "_Latest.csv"));

			if (Keyboard.IsKeyDown(Key.LeftCtrl) || !file.Exists)
			{
				if (dialog.ShowDialog() != DialogResult.OK)
				{
					throw new InvalidOperationException("The user must select a CSV file.");
				}

				file = new FileInfo(dialog.FileName);
			}

			return new ItemRepository(this.eventAggregator, new CsvDataSource(file));
		}
	}
}
