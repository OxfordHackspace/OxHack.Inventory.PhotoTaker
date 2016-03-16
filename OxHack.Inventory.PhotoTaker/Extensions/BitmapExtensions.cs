using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace OxHack.Inventory.PhotoTaker.Extensions
{
	public static class BitmapExtensions
	{
		[DllImport("gdi32")]
		private static extern int DeleteObject(IntPtr o);

		public static BitmapSource ToBitmapSource(this Bitmap bitmap)
		{
			using (var source = bitmap)
			{
				IntPtr ptr = source.GetHbitmap();

				BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap(
					ptr,
					IntPtr.Zero,
					Int32Rect.Empty,
					System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

				BitmapExtensions.DeleteObject(ptr);

				return result;
			}
		}
	}
}
