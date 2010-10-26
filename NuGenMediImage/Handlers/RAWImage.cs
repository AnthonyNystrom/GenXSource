using System;
using System.Drawing;
using System.IO;
using Genetibase.NuGenMediImage.Utility;

namespace Genetibase.NuGenMediImage.Handlers
{	
	public class RAWImage : ImageBase
	{
		private int offset = 0;		

		public int Offset
		{
			get
			{
				return offset;
			}
			set
			{
				if( value > 0 )
					offset = value;
			}
		}
		

		public enum Format
		{
			Interleaved,Planar
		}
		

		public enum ColorOrder
		{
			RGB, BGR
		}


		public Format format = Format.Interleaved;

		public ColorOrder colorOrder = ColorOrder.RGB;
		
		public void LoadImage(String fileName,int BitsPerPixel, int Offset, int Width, int Height, int NumberOfImages , Format format)
		{
			bitsPerPixel = BitsPerPixel;
			offset = Offset;
			width = Width;
			height = Height;
			nImages = NumberOfImages;
			this.format = format;

			if( format == Format.Interleaved || bitsPerPixel < 24 )
				HandleInterleaved( fileName );
			else if( format == Format.Planar )// Planar is possible only if bits Per Pixel
											  // is equal to 24
				HandlePlanar( fileName );			
		}

		private Color ReadColor(BinaryReader reader)
		{
			if( bitsPerPixel == 24 )
			{
				int r = reader.ReadByte();
				int g = reader.ReadByte();
				int b = reader.ReadByte();
			
				/*if(this.colorOrder == ColorOrder.BGR )
					return Color.FromArgb(b,g,r);
				else*/
				return Color.FromArgb(r,g,b);
			}
			else if( bitsPerPixel == 16 )
			{
				byte b1 = reader.ReadByte();
				byte b2 = reader.ReadByte();
				int argb = b1 << 8;
				argb = argb | b2;

				return Color.FromArgb( argb );
			}
			else if( bitsPerPixel == 8 )
			{
				return Color.FromArgb( reader.ReadByte() );
			}
			else if( bitsPerPixel == 20  )
			{
				return Color.Red;
			}
			else if( bitsPerPixel == 12  )
			{
				return Color.Red;
			}

			// Return white by default
			// This should never be reached
			return Color.White;
		}

		private int ReadPixel(BinaryReader reader)
		{
			return reader.ReadByte();			
		}

		private void HandleInterleaved(String fileName)
		{
			BinaryReader reader = new BinaryReader(	
				File.Open( fileName, FileMode.Open ) );

			for( int k = 0; k <  nImages; k++ )
			{
				Bitmap retBitmap = new Bitmap( width, height );
                FastBitmap fb = new FastBitmap(retBitmap);				
				//Skip the offset
				if( offset > 0 )
					reader.ReadBytes( offset );

                fb.LockBitmap();

				for( int j=0; j < height; j++ )
				{
					for( int i=0; i < width; i++ )
					{
                        unsafe
                        {
                            Color c = ReadColor(reader);
                            PixelData* pd = fb[i, j];
                            pd->red = c.R;
                            pd->green = c.G;
                            pd->blue = c.B;
                        }
						//retBitmap.SetPixel( i,j, ReadColor( reader ) );
					}
				}
                fb.UnlockBitmap();

				Images.Add( retBitmap );
			}

			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			_imageData = reader.ReadBytes( (int)reader.BaseStream.Length );
						
			reader.Close();			
			
		}

		private void HandlePlanar(String fileName)
		{
			BinaryReader reader = new BinaryReader(	
				File.Open( fileName, FileMode.Open ) );

			for( int k = 0; k <  nImages; k++ )
			{
				Bitmap retBitmap = new Bitmap( width, height );
                FastBitmap fb = new FastBitmap(retBitmap);
					
				//Skip the offset
				if( offset > 0 )
					reader.ReadBytes( offset );

				int totalPixels = width * height;
				int []r = new int[ totalPixels ];
				int []g = new int[ totalPixels ];
				int []b = new int[ totalPixels ];

				// Read all R colors
				for( int i=0; i < totalPixels; i++ )
				{
					r[i] = ReadPixel( reader );
				}	

				// Read all G colors
				for( int i=0; i < totalPixels; i++ )
				{
					g[i] = ReadPixel( reader );
				}	

				// Read all B colors
				for( int i=0; i < totalPixels; i++ )
				{
					b[i] = ReadPixel( reader );
				}
	
				if( colorOrder == ColorOrder.BGR )
				{
					int []temp = r;
					r = b;
					b = temp;
				}

				int index = 0;

                fb.LockBitmap();

				for( int j=0; j < height; j++ )
				{
					for( int i=0; i < width; i++ )
					{
                        unsafe
                        {
                            PixelData* pd = fb[i, j];
                            pd->red = (byte)r[index];
                            pd->green = (byte)g[index];
                            pd->blue = (byte)b[index];
                        }

						//retBitmap.SetPixel( i,j, Color.FromArgb(r[index], g[index], b[index] ) );
						index ++;
					}
				}
                fb.UnlockBitmap();
				Images.Add( retBitmap );
			}

			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			_imageData = reader.ReadBytes( (int)reader.BaseStream.Length );

			reader.Close();
			
		}
	}
}
