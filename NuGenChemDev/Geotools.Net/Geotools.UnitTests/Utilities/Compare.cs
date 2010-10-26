#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Utilities/Compare.cs,v 1.2 2003/01/02 20:24:48 awcoats Exp $
 * $Log: Compare.cs,v $
 * Revision 1.2  2003/01/02 20:24:48  awcoats
 * *** empty log message ***
 *
 * 
 * 4     12/27/02 10:42a Awcoats
 * Added BinaryCompare()
 * 
 * 3     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 2     10/08/02 12:15p Awcoats
 * 
 * 1     9/19/02 9:39a Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using System.IO;
#endregion

namespace Geotools.UnitTests.Utilities
{
	/// <summary>
	/// Summary description for FileCompare.
	/// </summary>
	public class Compare
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the FileCompare class.
		/// </summary>
		public static bool CompareAgainstString(string filename, string compareString)
		{
			StreamReader tr = new StreamReader(filename);
			string filestring=  tr.ReadToEnd();
			tr.Close();
			string s1= filestring.Trim();
			string s2= compareString.Trim();
			bool same = filestring.TrimEnd()==compareString.TrimEnd();
			return same;
		}

		/// <summary>
		/// Compares strings ignoring spaces and case.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool WktStrings(string a, string b)
		{
			string a1=a.Replace(" ","").ToUpper();
			string b1=b.Replace(" ","").ToUpper();
			a1=a1.Replace("\n","");
			b1=b1.Replace("\n","");
			a1=a1.Replace("\t","");
			b1=b1.Replace("\t","");
			return a1==b1;
		}

		public static int BinaryCompare(string filename1, string filename2)
		{
			FileStream stream1 = new FileStream(filename1, FileMode.Open);
			FileStream stream2 = new FileStream(filename2, FileMode.Open);

			//todo check files are the same length before even starting.
			long length1 = stream1.Length;
			long length2 = stream2.Length;

			if (length1 != length2)
			{
				Console.WriteLine(String.Format("File 1: {0} bytes" , length1) );
				Console.WriteLine(String.Format("File 2: {0} bytes" , length2) );
			}

			// this should make things a litte quicker
			BufferedStream bf1 = new BufferedStream(stream1);
			BufferedStream bf2 = new BufferedStream(stream2);

			
			BinaryReader reader1 = new BinaryReader(bf1);
			BinaryReader reader2 = new BinaryReader(bf2);
			int index1=0;
			int index2=0;
			bool readMore = (index1<stream1.Length) && (index2<stream2.Length);
			
			int iDiffCount=0;
			while (readMore)
			{
				byte a = reader1.ReadByte();
				byte b = reader2.ReadByte();
				if (a!=b)
				{
					iDiffCount++;
					if (iDiffCount<100) 
					{
						Console.WriteLine(String.Format("Difference at {0} a:=0x{1:x} b=0x{2:x}",index1,a,b));
					}
				}
				index1++;
				index2++;

				readMore = (index1<stream1.Length) && (index2<stream2.Length);

				
				if (iDiffCount>100)
				{
					Console.WriteLine("More than 100 differences.");
					break;
				}
			}
			reader1.Close();
			reader2.Close();
			return iDiffCount;
		}
		#endregion
	}
}
