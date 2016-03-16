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

			FileInfo file;

			if (Keyboard.IsKeyDown(Key.LeftCtrl))
			{
				var assmeblyFile = new FileInfo(Assembly.GetEntryAssembly().Location);
				file = new FileInfo(Path.Combine(assmeblyFile.DirectoryName, "Files", "Original.csv"));
			}
			else
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
