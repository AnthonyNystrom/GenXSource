using System;
using System.IO;
using System.Drawing;
using Genetibase.NuGenMediImage.Utility;

namespace Genetibase.NuGenMediImage.Handlers
{	
	public class InterImage : RAWImage
	{
		private string dataFileName = string.Empty;					

		public void LoadImage( string Path )
		{
			StreamReader input = new StreamReader( File.Open( Path, FileMode.Open, FileAccess.Read ) );
			
			string x = input.ReadLine();
			header += x + Environment.NewLine;

			x = x.ToUpper();
			
			if (!( x.IndexOf("!INTERFILE") >= 0 ))
				throw new ApplicationException("Not an Interfile");
        			

			while(  ( x = input.ReadLine() ) != null )
			{
				header += x+ Environment.NewLine;

				if( x.ToUpper().IndexOf("!END OF INTERFILE") >= 0 )
					break;

				ParseString( x );		
			}

			input.Close();

			BinaryReader reader = new BinaryReader( File.Open( Path, FileMode.Open, FileAccess.Read ));
			reader.BaseStream.Seek(0, SeekOrigin.Begin);
			_headerData = reader.ReadBytes( (int)reader.BaseStream.Length );

			reader.Close();


            string imgFileName = Util.GetFileNameWOExt(Path);
			_headerName = imgFileName;

            string path = Util.StripFileNameExt(Path);
			string newFile = path + dataFileName ;
						
			base.LoadImage( newFile, bitsPerPixel, 0, width, height, nImages, RAWImage.Format.Interleaved);

		}

		private void ParseString( string text )
		{
			int idx = text.IndexOf(":=");

			string name = text.Substring( 0, idx ).Trim().ToUpper();
			string Value = text.Substring( idx + 2).Trim();

			switch( name )
			{
				case "!MATRIX SIZE (X)":
				case "!MATRIX SIZE [1]":
					width = int.Parse( Value );
					break;

				case "!MATRIX SIZE (Y)":
				case "!MATRIX SIZE [2]":
					height = int.Parse( Value );
					break;

				case "!TOTAL NUMBER OF IMAGES":
				case "NUMBER OF IMAGES/WINDOW":
				case "NUMBER OF IMAGES/ENERGY WINDOW":
					if( nImages != 0 )
					{
						nImages = int.Parse( Value );
					}
					break;

				case "!NAME OF DATA FILE":
					dataFileName = Value;
					_imageName = dataFileName;
					break;

				case "!NUMBER FORMAT":
					break;

				case "!NUMBER OF BYTES PER PIXEL":
					bitsPerPixel = 8 * int.Parse( Value );
					break;
			}			
		}
	}
}

