using OxHack.Inventory.Data.Csv.CsvHelperConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxHack.Inventory.Data.Models
{
	public class CsvItem
	{
		public int Id
		{
			get;
			set;
		}

		public String Name
		{
			get;
			set;
		}

		public String Manufacturer
		{
			get;
			set;
		}

		public String Model
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public String Category
		{
			get;
			set;
		}

		public String Spec
		{
			get;
			set;
		}

		public String Appearance
		{
			get;
			set;
		}

		public String AssignedLocation
		{
			get;
			set;
		}

		public String CurrentLocation
		{
			get;
			set;
		}

		public bool IsLoan
		{
			get;
			set;
		}

		public String Origin
		{
			get;
			set;
		}

		public String AdditionalInformation
		{
			get;
			set;
		}

		[TypeConverter(typeof(PhotoListConverter))]
		public IEnumerable<String> Photos
		{
			get;
			set;
		}
	}
}
