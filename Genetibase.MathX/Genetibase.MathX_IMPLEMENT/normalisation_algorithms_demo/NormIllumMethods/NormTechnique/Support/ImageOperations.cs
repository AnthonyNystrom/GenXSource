using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace NormIllumMethods.Techniques
{
    public unsafe abstract class ImageOperations
    {
        public unsafe abstract Bitmap Apply(Bitmap bitmap);

        Bitmap bitmap;
		int width;
		BitmapData bitmapData = null;
		Byte* pBase = null;

		public void Dispose()
		{
			bitmap.Dispose();
		}

		public Bitmap Bitmap
		{
			get{return(bitmap);}
            set{bitmap = value;}
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

		public void LockBitmap()
		{
			GraphicsUnit unit = GraphicsUnit.Pixel;
			RectangleF boundsF = bitmap.GetBounds(ref unit);
			Rectangle bounds = new Rectangle((int) boundsF.X,(int) boundsF.Y,(int) boundsF.Width,(int) boundsF.Height);

			// Figure out the number of bytes in a row
			// This is rounded up to be a multiple of 4
			// bytes, since a scan line in an image must always be a multiple of 4 bytes
			// in length. 
			width = (int) boundsF.Width * sizeof(PixelData);
			if (width % 4 != 0)
			{
				width = 4 * (width / 4 + 1);
			}

			bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
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

        #region Operations
        /// <summary>
        /// Compute the normalisation operation of image using the min and max previously calculated  
        /// </summary>
        /// <param name="img"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Bitmap Normalise(double[,] img,double min,double max) 
        {
            int nfil = img.GetLength(0);
            int ncol = img.GetLength(1);
            Bitmap  = new Bitmap(nfil,ncol,PixelFormat.Format8bppIndexed);
            
            ColorPalette palette = Bitmap.Palette;
            for (int i = 0; i < 256; i++)
                palette.Entries[i] = Color.FromArgb(i, i, i);
            Bitmap.Palette = palette;

            LockBitmap();
            double k = 255 / (max - min);
            for (int y = 0; y < ncol ; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < nfil ; x++)
                {
                    try
                    {
                        pPixel->gray = Convert.ToByte(k * (img[x, y] - min));
                        pPixel++;
                    }
                    catch(Exception ex)
                    {
                        int tt = 7;
                    }
                }
            }
            UnlockBitmap();
            return Bitmap;
        }

        /// <summary>
        /// Compute the normalisation operation of image
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public Bitmap Normalise(double[,] img)
        {
            int nfil = img.GetLength(0);
            int ncol = img.GetLength(1);
            double min = 0, max = 0;
            bool first = true;
            
            //Computing min and max
            for (int j = 0; j < ncol; j++)
            {
                for (int i = 0; i < nfil; i++)
                {
                    if (first) { min = max = img[i, j]; first = false; }
                    else
                    {
                        if (img[i, j] < min) min = img[i, j];
                        else
                            if (img[i, j] > max) max = img[i, j];
                    }
                }
            }

            Bitmap = new Bitmap(nfil, ncol, PixelFormat.Format8bppIndexed);
            //Setting color palette
            ColorPalette palette = Bitmap.Palette;
            for (int i = 0; i < 256; i++)
                palette.Entries[i] = Color.FromArgb(i, i, i);
            Bitmap.Palette = palette;

            //Normalising
            LockBitmap();
            double k = 255 / (max - min);
            for (int y = 0; y < ncol ; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < nfil ; x++)
                {
                    pPixel->gray = Convert.ToByte(k * (img[x , y ] - min));
                    pPixel++;
                }
            }
            UnlockBitmap();
            return Bitmap;
        }

        /// <summary>
        /// Compute the sum operation to the image with a factor
        /// </summary>
        /// <param name="img"></param>
        /// <param name="destiny"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public double[,] SumBitmap(Bitmap img, double[,] destiny, double factor)
        {
            int nfil = destiny.GetLength(0);
            int ncol = destiny.GetLength(1);
            Bitmap = img;

            LockBitmap();
            for (int y = 0; y < ncol ; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < nfil ; x++)
                {
                    destiny[x,y] +=  factor*pPixel->gray ;
                    pPixel++;
                }
            }
            UnlockBitmap();
            return destiny;
        }

        /// <summary>
        /// Compute the division operation to the image with a array of values
        /// </summary>
        /// <param name="img"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public double[,] DivBitmap(Bitmap img, double[,] divider)
        {
            int nfil = divider.GetLength(0);
            int ncol = divider.GetLength(1);
            double[,] result = new double[nfil,ncol];
            Bitmap = img;
            LockBitmap();
            for (int y = 0; y < ncol; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < nfil; x++)
                {
                    result[x, y] += (double)pPixel->gray/(divider[x,y] + 1);
                    pPixel++;
                }
            }
            UnlockBitmap();
            return result;
        }

        /// <summary>
        /// Compute the natural logarithm operation to the image
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public double[,] LogBitmap(Bitmap img)
        {
            Bitmap = img;
            int nfil = img.Width;
            int ncol = img.Height;
            double[,] src = new double[nfil,ncol];
            LockBitmap();
            for (int y = 0; y < ncol; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < nfil; x++)
                {
                    src[x, y] = Math.Log((double)pPixel->gray + 1.0, Math.E);           
                    pPixel++;
                }
            }

            UnlockBitmap();
            return src;
        }

        /// <summary>
        /// Compute the exponential operation to the image
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public double[,] ExpBitmap(Bitmap img)
        {
            Bitmap = img;
            int nfil = img.Width;
            int ncol = img.Height;
            double[,] src = new double[nfil, ncol];
            LockBitmap();
            for (int y = 0; y < ncol; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < nfil; x++)
                {
                    src[x, y] = Math.Pow(Math.E,(double)pPixel->gray) - 1;
                    pPixel++;
                }
            }

            UnlockBitmap();
            return src;
        }

        /// <summary>
        /// Compute the exponential operation to the image
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public double[,] ExpBitmap(double[,] img)
        {
            int nfil = img.GetLength(0);
            int ncol = img.GetLength(1);
            double[,] result = new double[nfil, ncol];
            for (int y = 0; y < ncol; y++)
            {
                for (int x = 0; x < nfil; x++)
                {
                    result[x, y] = Math.Pow(Math.E, img[x,y]) - 1;
                }
            }
            return result;
        }

        /// <summary>
        /// Compute the difference operation between two images
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public double[,] DifArray(double[,] a, double[,] b)
        {
            int nfil = a.GetLength(0);
            int ncol = a.GetLength(1);
            double[,] result = new double[nfil, ncol];
            
            for (int y = 0; y < ncol; y++)
            {
                for (int x = 0; x < nfil; x++)
                {
                    result[x, y] = a[x,y] - b[x,y];
                }
            }
            return result;
        }

        /// <summary>
        /// Create an image from Bitmap
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public Bitmap ArrayToBitmap(double[,] src)
        {
            int nfil = src.GetLength(0);
            int ncol = src.GetLength(1);
            Bitmap = new Bitmap(nfil,ncol,PixelFormat.Format8bppIndexed);
            
            ColorPalette palette = Bitmap.Palette;
            for (int i = 0; i < 256; i++)
                palette.Entries[i] = Color.FromArgb(i, i, i);
            Bitmap.Palette = palette;

            LockBitmap();
            for (int y = 0; y < ncol; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < nfil; x++)
                {
                    pPixel->gray = Convert.ToByte(src[x, y]);
                    pPixel++;
                }
            }
            UnlockBitmap();
            return Bitmap;
        }

        /// <summary>
        /// Create an array from Bitmap
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public double[,] BitmapToArray(Bitmap src)
        {
            int nfil = src.Width;
            int ncol = src.Height;
            double[,] result = new double[nfil,ncol];
            Bitmap = src;

            LockBitmap();
            for (int y = 0; y < ncol; y++)
            {
                PixelData* pPixel = PixelAt(0, y);
                for (int x = 0; x < nfil; x++)
                {
                    result[x,y] =  pPixel->gray;
                    pPixel++;
                }
            }
            UnlockBitmap();
            return result;
        }
        
        #endregion
    }

    public struct PixelData
    {
        public byte gray;
    }
}
