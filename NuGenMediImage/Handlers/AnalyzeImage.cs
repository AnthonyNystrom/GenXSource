using System;
using System.Collections;
using System.IO;
using System.Drawing;
using Genetibase.NuGenMediImage.Utility;

namespace Genetibase.NuGenMediImage.Handlers
{
	/// <summary>
	/// Summary description for AnalyzeImage.
	/// </summary>
	public class AnalyzeImage : RAWImage
	{
		public void LoadImage( string Path )
		{
			int i=0;
			int bitsPerPixel;
			int width;
			int height;
			int datatype;
			int nImages;
			int offset;

			BinaryReader input = new BinaryReader( File.Open( Path, FileMode.Open, FileAccess.Read ) );
        
			input.ReadInt32 (); 				// sizeof_hdr
			for (i=0; i<10; i++) input.ReadByte();		// data_type
			for (i=0; i<18; i++) input.ReadByte(); 		// db_name 
			input.ReadInt32 (); 				// extents 
			input.ReadInt16 (); 			// session_error
			input.ReadByte ();				// regular 
			input.ReadByte (); 				// hkey_un0 

			// image_dimension

			short endian = input.ReadInt16();		// dim[0] 
			/*if ((endian < 0) || (endian > 15)) 
			{
					littleEndian = true;
				fi.intelByteOrder = true; }  */

			width = input.ReadInt16();		// dim[1] 
			height = input.ReadInt16();		// dim[2] 
			nImages = input.ReadInt16();		// dim[3] 
			input.ReadInt16();				// dim[4] 
			for (i=0; i<3; i++) input.ReadInt16();	// dim[5-7] 

			byte[] units = new byte[4];
			input.Read( units, 0, 4); 			// vox_units 

			//fi.unit = new String (units, 0, 4); 

			for (i=0; i<8; i++) input.ReadByte();		// cal_units[8] 
			input.ReadInt16 ();				// unused1
			datatype = input.ReadInt16 ();		// datatype 
			bitsPerPixel = input.ReadInt16 ();		// bitpix
			input.ReadInt16 ();				// dim_un0
			input.ReadInt32();				// pixdim[0] 
			/*pixelWidth = (double)*/ input.ReadInt32();	// pixdim[1] 
			/*pixelHeight = (double)*/ input.ReadInt32(); // pixdim[2] 
			/*pixelDepth = (double)*/ input.ReadInt32(); 	// pixdim[3] 
			for (i=0; i<4; i++) input.ReadInt32();	// pixdim[4-7]  
			offset = input.ReadInt32();		// vox_offset

			input.ReadInt32();				// roi_scale 
			input.ReadInt32();				// funused1 
			input.ReadInt32();				// funused2 
			input.ReadInt32();				// cal_max 
			input.ReadInt32();				// cal_min 
			input.ReadInt32();				// compressed
			input.ReadInt32();				// verified  
			//   ImageStatistics s = imp.getStatistics();
			input.ReadInt32();	//(int) s.max		// glmax 
			input.ReadInt32();	//(int) s.min		// glmin 

			// data_history 

			for (i=0; i<80; i++) input.ReadByte();		// descrip  
			for (i=0; i<24; i++) input.ReadByte();		// aux_file 
			input.ReadByte();				// orient 
			for (i=0; i<10; i++) input.ReadByte();		// originator 
			for (i=0; i<10; i++) input.ReadByte();		// generated 
			for (i=0; i<10; i++) input.ReadByte();		// scannum 
			for (i=0; i<10; i++) input.ReadByte();		// patient_id  
			for (i=0; i<10; i++) input.ReadByte();		// exp_date 
			for (i=0; i<10; i++) input.ReadByte();		// exp_time  
			for (i=0; i<3; i++) input.ReadByte();		// hist_un0
			input.ReadInt32 ();				// views 
			input.ReadInt32 ();				// vols_added 
			input.ReadInt32 ();				// start_field  
			input.ReadInt32 ();				// field_skip
			input.ReadInt32 ();				// omax  
			input.ReadInt32 ();				// omin 
			input.ReadInt32 ();				// smax  
			input.ReadInt32 ();				// smin 

	
			input.BaseStream.Seek(0, SeekOrigin.Begin);
			_headerData = input.ReadBytes( (int)input.BaseStream.Length );

			input.Close();

            string imgFileName = Util.GetFileNameWOExt(Path);
			
			_headerName = imgFileName;

            string path = Util.StripFileNameExt(Path);
			string newFile = path + imgFileName + "." + "img";

			_imageName = imgFileName + "." + "img";
			
			header += "Analyze Format" + Environment.NewLine;
			header += "XYZ dim: " + width + "/" + height + "/" + nImages  + Environment.NewLine;;
			header += "Bits per pixel: " + bitsPerPixel + Environment.NewLine;

			base.LoadImage( newFile, bitsPerPixel, offset, width, height, nImages, RAWImage.Format.Interleaved);			
		}
	}
}
