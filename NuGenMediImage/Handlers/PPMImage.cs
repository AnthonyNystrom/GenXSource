using System;
using System.IO;
using System.Drawing;
using Genetibase.NuGenMediImage.Utility;

namespace Genetibase.NuGenMediImage.Handlers
{	
	public class PPMImage : ImageBase
	{
		private BinaryReader reader = null;
	
        //public  int width = -1;
        //public  int height = -1;
		private int bitshift;
		private int bits;

		private int MaxColor = 0;

		public Bitmap LoadImage( string filePath )
		{
			Stream stream = File.Open( filePath, FileMode.Open );			
			reader = new BinaryReader( stream );
			Bitmap bitmap = getImage();
			
			reader.BaseStream.Seek(0, SeekOrigin.Begin );
			_imageData = reader.ReadBytes( (int)reader.BaseStream.Length );

			reader.Close();
			return bitmap;
		}

		public override ImageArray Images
		{
			get
			{
				return _images;
			}
		}

		public Bitmap getImage()
		{
			try
			{
				ReadHeader();
			}
			catch(Exception)
			{
				throw new IOException("Open PPM Exception - Couldn't read header!");
			}

			Bitmap bitmap = new Bitmap( width, height );
            FastBitmap fb = new FastBitmap(bitmap);
            fb.LockBitmap();
			
			int x;
			int y;

			try
			{
				for(y = 0; y < height; y++ )				
				{
					for(x = 0; x < width; x++)
					{
                        unsafe
                        {
                            PixelData* pd = fb[x, y];
                            pd->red = (byte)ReadByte();
                            pd->green = (byte)ReadByte();
                            pd->blue = (byte)ReadByte();
                        }
						//bitmap.SetPixel( x, y, Color.FromArgb( makeRgb( ReadByte(), ReadByte(), ReadByte() ) ) ) ;
					}
				}

			}
			catch(Exception)
			{
				throw new IOException("Open PPM Exception - Couldn't read image data!");
			}

            fb.UnlockBitmap();			
			
			_images.Clear();
			_images.Add(bitmap);

			return bitmap;
		}

		private int makeRgb(int i, int j, int k)
		{
			return (int) (0xff000000 | (uint) i << 16 | (uint) j << 8 | (uint)k);
		}

		void ReadHeader()
		{
			char c = (char)ReadByte();
			char c1 = (char)ReadByte();
			if(c != 'P')
				throw new IOException("not a PPM file");
			if(c1 != '6')
			{
				throw new IOException("not a PPM file");
			} 
			else
			{
				width = ReadInt();
				height = ReadInt();
				MaxColor = ReadInt();
				return;
			}
		}

		private int ReadByte( )
		{	
			int i = 0;
			
			if( MaxColor <= 255 )
				i= reader.ReadByte();
			else
				i = reader.ReadInt16();

			if(i == -1)
				throw new EndOfStreamException();
			else
				return i;
		}

		private bool ReadBit()
		{
			if(bitshift == -1)
			{
				bits = ReadByte();
				bitshift = 7;
			}
			bool flag = (bits >> bitshift & 1) != 0;
			bitshift--;
			return flag;
		}

		private char ReadChar()
		{
			char c = (char)ReadByte();
			if(c == '#')
				do
					c = (char)ReadByte();
				while(c != '\n' && c != '\r');
			return c;
		}

		private char ReadNonwhiteChar()
		{
			char c;
			do
				c = ReadChar();
			while(c == ' ' || c == '\t' || c == '\n' || c == '\r');
			return c;
		}

		private int ReadInt()
		{
			char c = ReadNonwhiteChar();
			if(c < '0' || c > '9')
				throw new IOException("Invalid integer when reading PPM image file.");
			int i = 0;
			do
			{
				i = (i * 10 + c) - 48;
				c = ReadChar();
			} while(c >= '0' && c <= '9');
			return i;
		}

		public PPMImage()
		{
			bitshift = -1;
		} 

	}
}