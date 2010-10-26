using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Genetibase.NuGenMediImage.Utility;
using System.Drawing.Drawing2D;

namespace Genetibase.NuGenMediImage
{
	/// <summary>
	/// Summary description for Filters.
	/// </summary>
	public class Filters
	{
		public class ConvMatrix
		{
			public int TopLeft = 0, TopMid = 0, TopRight = 0;
			public int MidLeft = 0, Pixel = 1, MidRight = 0;
			public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
			public int Factor = 1;
			public int Offset = 0;
			public void SetAll(int nVal)
			{
				TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
			}
		}

		public static void Brightness(Bitmap b, Single nBrightness)
		{	
			ColorMatrix brightnessMatrix = new ColorMatrix(new Single[][]
										{
											new Single[] {1, 0, 0, 0, 0}, 
											new Single[] {0, 1, 0, 0, 0}, 
											new Single[] {0, 0, 1, 0, 0}, 
											new Single[] {0, 0, 0, 1, 0}, 
											new Single[] {nBrightness, nBrightness, nBrightness, 0, 1}});

			ApplyColorMatrix(b, ref brightnessMatrix );			
		}

		public static void Contrast(Bitmap b, Single nContrast)
		{	
			ColorMatrix contrastMatrix = new ColorMatrix(new Single[][]
										{
											new Single[] {nContrast, 0, 0, 0, 0}, 
											new Single[] {0, nContrast, 0, 0, 0}, 
											new Single[] {0, 0, nContrast, 0, 0}, 
											new Single[] {0, 0, 0, 1, 0}, 
											new Single[] {0.0001F, 0.0001F, 0.0001F, 0, 1}});

			ApplyColorMatrix(b, ref contrastMatrix );			
		}

		private static void ApplyColorMatrix(Bitmap b , ref ColorMatrix matrix)
		{
		
			ImageAttributes ImgAtt = new ImageAttributes();
			//Bitmap bmpMatrix=new Bitmap(b.Width, b.Height);
			Graphics grMatrix = Graphics.FromImage(b);
																	
			ImgAtt.SetColorMatrix(matrix);

			grMatrix.DrawImage(b, new Rectangle(0, 0, b.Width, b.Height), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, ImgAtt);
		
			//b = bmpMatrix;

			grMatrix.Dispose();
			ImgAtt.Dispose();
		}
		
		public static void Smooth(Bitmap b, int nWeight)
		{
            ConvMatrix m = new ConvMatrix();
			m.SetAll(1);
			m.Pixel = nWeight;
			m.Factor = nWeight + 8;

			Conv3x3(b, m);
		}

		public static Bitmap Brightness2(Bitmap b, int nBrightness)
		{
			if (nBrightness < -255 || nBrightness > 255)
				throw new ArgumentException("Brightness can not be less then -255 or greater then 255");

			if(nBrightness == 0)
				return b;

			Bitmap bSrc = (Bitmap)b.Clone();

			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			int nVal = 0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - bSrc.Width*3;
				int nWidth = bSrc.Width * 3;

				for(int y=0;y < bSrc.Height;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						nVal = (int) (p[0] + nBrightness);
		
						if (nVal < 0) nVal = 0;
						if (nVal > 255) nVal = 255;

						p[0] = (byte)nVal;

						++p;
					}
					p += nOffset;
				}
			}

			bSrc.UnlockBits(bmData);

			return bSrc;
		}

		public static Bitmap Contrast(Bitmap b, int nContrast)
		{
			Bitmap bSrc = (Bitmap)b.Clone();

			if (nContrast < -100) return bSrc;
			if (nContrast >  100) return bSrc;

			double pixel = 0, contrast = (100.0+nContrast)/100.0;

			contrast *= contrast;

			int red, green, blue;
			
			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - bSrc.Width*3;

				for(int y=0;y<bSrc.Height;++y)
				{
					for(int x=0; x < bSrc.Width; ++x )
					{
						blue = p[0];
						green = p[1];
						red = p[2];
				
						pixel = red/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[2] = (byte) pixel;

						pixel = green/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[1] = (byte) pixel;

						pixel = blue/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[0] = (byte) pixel;					

						p += 3;
					}
					p += nOffset;
				}
			}

			bSrc.UnlockBits(bmData);

			return bSrc;
		}

		private static Bitmap Conv3x3(Bitmap b, ConvMatrix m)
		{
			// Avoid divide by zero errors
			if ( m.Factor == 0 ) return b;

			Bitmap bSrc = (Bitmap)b.Clone(); 

			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			int stride2 = stride * 2;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr SrcScan0 = bmSrc.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * pSrc = (byte *)(void *)SrcScan0;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width - 2;
				int nHeight = b.Height - 2;

				int nPixel;

				for(int y=0;y < nHeight;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						nPixel = ( ( ( (pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
							(pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
							(pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;

						p[5 + stride]= (byte)nPixel;

						nPixel = ( ( ( (pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
							(pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
							(pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;
							
						p[4 + stride] = (byte)nPixel;

						nPixel = ( ( ( (pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
							(pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
							(pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;

						p[3 + stride] = (byte)nPixel;

						p += 3;
						pSrc += 3;
					}
					p += nOffset;
					pSrc += nOffset;
				}
			}

			b.UnlockBits(bmData);
			bSrc.UnlockBits(bmSrc);	
	
			bSrc.Dispose();
			bSrc = null;

			return b;
		}

        public static Bitmap Resize(Bitmap imgPhoto, float nPercent)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                     PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
		public static void ReadAndApplyLUT( Stream file , Bitmap b)
		{
			BinaryReader reader = new BinaryReader( file );
			
			int []R = new int[256];
			int []G = new int[256];			
			int []B = new int[256];

			byte []LUT = reader.ReadBytes( 768 );
           
			// Read the look up table info
			for(int i = 0; i < 256; i++ )
			{				
				R[i] = LUT[i];				
				G[i] = LUT[i + 256];
				B[i] = LUT[i + 512];
			}
			// Close the reader
			reader.Close();

            FastBitmap fb = new FastBitmap(b);
			//Bitmap newBitmap = b;
			//Color color;
			//Color newColor;
            fb.LockBitmap();

            unsafe
            {
                // Apply the look up table
                for (int i = 0; i < fb.Bitmap.Width; i++)
                {
                    for (int j = 0; j < fb.Bitmap.Height; j++)
                    {
                        PixelData* pd = fb[i, j];
                        //color = newBitmap.GetPixel(i,j);
                        //newColor = Color.FromArgb( R[ color.R ], G[ color.G ], B[ color.B ] );
                        //newBitmap.SetPixel(i,j, newColor );
                        pd->red = (byte)R[pd->red];
                        pd->green = (byte)G[pd->green];
                        pd->blue = (byte)B[pd->blue];                        
                    }
                }
            }
           
            fb.UnlockBitmap();

            R = null;
            G = null;
            B = null;
            LUT = null;
		}

        public static void iRotate(Bitmap b, RotateFlipType rotate)
        {
            b.RotateFlip(rotate);            
        }	

		public static Bitmap Rotate(Bitmap b,RotateFlipType rotate)
		{
			Bitmap src = (Bitmap)b.Clone();

			src.RotateFlip(rotate);
			return src;
		}

		public static Bitmap Flip(Bitmap b,RotateFlipType flip)
		{
			Bitmap src = (Bitmap)b.Clone();

			src.RotateFlip(flip);
			return src;
		}
	
		public static Bitmap Emboss(Bitmap b, int Offset)
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(-1);
			m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 0;
			m.Pixel = 4;
			m.Offset = Offset;

			return  Conv3x3(b, m);
		}	

	}
}
