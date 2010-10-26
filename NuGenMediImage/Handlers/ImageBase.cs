using System;
using Genetibase.NuGenMediImage.Utility;

namespace Genetibase.NuGenMediImage.Handlers
{
	/// <summary>
	/// Summary description for ImageBase.
	/// </summary>
	public class ImageBase
	{
		protected int width = -1;

		protected int height = -1;

		protected int nImages = 1;

		protected int bitsPerPixel = 24;

		protected string header = "";

		protected ImageArray _images = new ImageArray();

		protected byte []_headerData = null;
		protected byte []_imageData = null;

		protected string _headerName = string.Empty;
		protected string _imageName = string.Empty;

		public byte[] HeaderData
		{
			get
			{
				return _headerData;
			}
		}

		public byte[] ImageData
		{
			get
			{
				return _imageData;
			}
		}

		public string HeaderName
		{
			get
			{
				return _headerName;
			}
		}

		public string ImageName
		{
			get
			{
				return _imageName;
			}
		}

		public virtual ImageArray Images
		{
			get
			{
				return _images;
			}
		}
		
		public virtual string Header
		{
			get
			{
				return header;
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
		}

		public int NumberOfImages
		{
			get
			{
				return nImages;
			}
		}

		public int BitsPerPixel
		{
			get
			{
				return bitsPerPixel;
			}
		}
	}
}
