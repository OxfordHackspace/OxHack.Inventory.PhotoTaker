using OxHack.Inventory.Data.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxHack.Inventory.Data.Events
{
	public class ItemUpdated : PubSubEvent<Item>
	{
	}
}
