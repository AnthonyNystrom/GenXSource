using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ComparisonGreyScaleImage
{
    public unsafe class BitmapGrey
	{
		Bitmap bitmap;
		int width;
		BitmapData bitmapData = null;
		Byte* pBase = null;

		public BitmapGrey(Bitmap bitmap)
		{
			this.bitmap = bitmap;
		}

		public void Save(string filename)
		{
			bitmap.Save(filename, ImageFormat.Jpeg);
		}

		public void Dispose()
		{
			bitmap.Dispose();
		}

		public Bitmap Bitmap
		{
			get{return(bitmap);}
		}

		public Point PixelSize
		{
			get
			{
				GraphicsUnit unit = GraphicsUnit.Pixel;
				RectangleF bounds = bitmap.GetBounds(ref unit);
				return new Point((int) bounds.Width, (int) bounds.Height);
			}
		}

		public void MakeGrey()
		{
			Point size = PixelSize;
			for (int x = 0; x < size.X; x++)
			{
				for (int y = 0; y < size.Y; y++)
				{
					Color c = bitmap.GetPixel(x, y);
					int value = (c.R + c.G + c.B) / 3;
					bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
				}
			}
		}

		public void MakeGreyUnsafe()
		{
			Point size = PixelSize;
			LockBitmap();
			for (int x = 0; x < size.X; x++)
			{
				for (int y = 0; y < size.Y; y++)
				{
					PixelData* pPixel = PixelAt(x, y);
					int value = (pPixel->red + pPixel->green + pPixel->blue) / 3;
					pPixel->red = (byte) value;
					pPixel->green = (byte) value;
					pPixel->blue = (byte) value;
				}
			}
			UnlockBitmap();
		}

		public void MakeGreyUnsafeFaster()
		{
			Point size = PixelSize;
			LockBitmap();
			for (int y = 0; y < size.Y; y++)
			{
				PixelData* pPixel = PixelAt(0, y);
				for (int x = 0; x < size.X; x++)
				{
					byte value = (byte) ((pPixel->red + pPixel->green + pPixel->blue) / 3);
					pPixel->red =  value;
					pPixel->green = value;
					pPixel->blue = value;
					pPixel++;
				}
			}
			UnlockBitmap();
		}

        /// <summary>
        /// Convert a bitmap from 24bpp to 8bpp 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bitmap GrayScale8bpp(Bitmap b)
        {
            Bitmap bitmap8 = new Bitmap(b.Width, b.Height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bitmap8.Palette;
            for (int i = 0; i < 256; i++){palette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);}
            bitmap8.Palette = palette;
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData8 = bitmap8.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            int stride = bmData.Stride;
            int stride1 = bmData8.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan1 = bmData8.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p1 = (byte*)(void*)Scan1;

                int nOffset = stride - b.Width * 3;
                int nOffset1 = stride1 - bitmap8.Width;

                byte red, green, blue;
                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        red = p[0];
                        green = p[1];
                        blue = p[2];
                        p1[0] = (byte)(.299 * red + .587 * green + .114 * blue);
                        p += 3;
                        p1 += 1;
                    }
                    p += nOffset;
                    p1 += nOffset1;
                }
            }

            b.UnlockBits(bmData);
            bitmap8.UnlockBits(bmData8);
            return bitmap8;
        }



		public void LockBitmap()
		{
			GraphicsUnit unit = GraphicsUnit.Pixel;
			RectangleF boundsF = bitmap.GetBounds(ref unit);
			Rectangle bounds = new Rectangle((int) boundsF.X,
				(int) boundsF.Y,
				(int) boundsF.Width,
				(int) boundsF.Height);

			// Figure out the number of bytes in a row
			// This is rounded up to be a multiple of 4
			// bytes, since a scan line in an image must always be a multiple of 4 bytes
			// in length. 
			width = (int) boundsF.Width * sizeof(PixelData);
			if (width % 4 != 0)
			{
				width = 4 * (width / 4 + 1);
			}

			bitmapData = 
				bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			pBase = (Byte*) bitmapData.Scan0.ToPointer();
		}

		public PixelData* PixelAt(int x, int y)
		{
			return (PixelData*) (pBase + y * width + x * sizeof(PixelData));
		}

		public void UnlockBitmap()
		{
			bitmap.UnlockBits(bitmapData);
			bitmapData = null;
			pBase = null;
		}

        public struct PixelData
        {
            public byte blue;
            public byte green;
            public byte red;
        }
	}
}
