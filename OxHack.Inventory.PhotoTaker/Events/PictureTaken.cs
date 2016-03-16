using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxHack.Inventory.PhotoTaker.Events
{
	public class PictureTaken : PubSubEvent<FileInfo>
	{
	}
}
