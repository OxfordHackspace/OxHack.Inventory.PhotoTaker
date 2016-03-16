using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;
using OxHack.Inventory.PhotoTaker.Events;
using OxHack.Inventory.PhotoTaker.Extensions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace OxHack.Inventory.PhotoTaker.ViewModels
{
	public class CapturePhotoViewModel : BindableBase
	{
		private readonly string pathToImages;
		private readonly Dispatcher dispatcher;
		private readonly IEventAggregator eventAggregator;

		private BitmapSource previewImage;
		private Capture captureDevice;
		private int selectedIndex;
		private BlockingCollection<Bitmap> frameQueue;
		private Task captureDeviceWorker;
		private Task previewUpdateWorker;
		private bool isStarted;

		public CapturePhotoViewModel(String pathToImages, Dispatcher dispatcher, IEventAggregator eventAggregator)
		{
			this.pathToImages = pathToImages;
			this.dispatcher = dispatcher;
			this.eventAggregator = eventAggregator;

			this.AvailableCameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).ToList();
			this.SelectedIndex = 0;

			this.TakePictureCommand = new DelegateCommand(this.TakePicture);


			this.frameQueue = new BlockingCollection<Bitmap>();

			this.StartPreviewUpdateWorker();
		}

		private void TakePicture()
		{
			var jpeg = this.captureDevice.QueryFrame().ToImage<Bgr, byte>().ToJpegData(100);

			FileInfo targetFile;
			do
			{
				targetFile = new FileInfo(
					Path.Combine(this.pathToImages, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()))
					+ ".jpg");
				
			}
			while (targetFile.Exists);

			using (var stream = targetFile.Create())
			{
				stream.Write(jpeg, 0, jpeg.Length);
			}

			if (File.Exists(targetFile.FullName))
			{
				this.eventAggregator.GetEvent<PictureTaken>().Publish(targetFile);
			}
		}

		public DelegateCommand TakePictureCommand
		{
			get;
			private set;
		}

		private void StartPreviewUpdateWorker()
		{
			this.previewUpdateWorker = Task.Run(() =>
			{
				while (!this.frameQueue.IsAddingCompleted)
				{
					try
					{
						var frame = this.frameQueue.Take();
						this.dispatcher.Invoke(() =>
						{
							try
							{
								var previewImage = frame.ToBitmapSource();
								this.PreviewImage = previewImage;
							}
							catch
							{
								this.StopCamera();
							}
						});
					}
					catch
					{
						// nothing
					}
				}
			});
		}

		internal void TearDown()
		{
			this.frameQueue.CompleteAdding();
			this.StopCamera();
		}

		private void StartCamera()
		{
			this.captureDevice.Start();
			this.isStarted = true;

			this.captureDeviceWorker = Task.Run(() =>
			{
				while (this.isStarted)
				{
					try
					{
						this.frameQueue.Add(this.captureDevice.QueryFrame().ToImage<Bgr, byte>().ToBitmap());
						Thread.Sleep(TimeSpan.FromSeconds(1.0 / 22));
					}
					catch
					{
						// nothing
					}
				}
			});
		}

		private void StopCamera()
		{
			this.isStarted = false;
			this.captureDevice?.Stop();
			this.captureDevice?.Dispose();
		}

		public IEnumerable<DsDevice> AvailableCameras
		{
			get;
			private set;
		}

		public BitmapSource PreviewImage
		{
			get
			{
				return this.previewImage;
			}
			private set
			{
				base.SetProperty(ref this.previewImage, value);
			}
		}

		public int SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				base.SetProperty(ref this.selectedIndex, value);

				this.StopCamera();
				this.captureDevice = new Capture(this.SelectedIndex);
				this.StartCamera();
			}
		}
	}
}
