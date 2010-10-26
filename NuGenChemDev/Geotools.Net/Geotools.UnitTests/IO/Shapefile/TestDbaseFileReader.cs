#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/Shapefile/TestDbaseFileReader.cs,v 1.1 2003/01/02 20:37:30 awcoats Exp $
 * $Log: TestDbaseFileReader.cs,v $
 * Revision 1.1  2003/01/02 20:37:30  awcoats
 * *** empty log message ***
 *
 * 
 * 3     12/27/02 1:09p Awcoats
 * nunit 1.0 to 2.0
 * 
 * 2     12/09/02 11:56a Awcoats
 * 
 * 1     11/27/02 10:28a Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using System.Collections;
using Geotools.IO;
using NUnit.Framework;
#endregion

namespace Geotools.UnitTests.Shapefile
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.Shapefile.TestDbaseFileReader class
	/// </summary>
	public class TestDbaseFileReader 
	{
		

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
			
			try
			{
				DbaseFileReader dbfReader = new DbaseFileReader(null);
				
			}
			catch (ArgumentNullException)
			{
			}
		}

		/// <summary>
		/// Test getting and setting the properties
		/// </summary>
		public void Test_TestProperties() 
		{
			DbaseFileReader dbfReader = new DbaseFileReader(Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\statepop.dbf");
			DbaseFileHeader dbfHeader = dbfReader.GetHeader();
			Assertion.AssertEquals("Dbase header: Num records", 49, dbfHeader.NumRecords);
			Assertion.AssertEquals("Dbase header: Num fields", 252, dbfHeader.NumFields);
			
			Assertion.AssertEquals("Field 0: name", "STATE_NAME", dbfHeader.Fields[0].Name);
			Assertion.AssertEquals("Field 0: name", 'C', dbfHeader.Fields[0].DbaseType);
			Assertion.AssertEquals("Field 0: name", typeof(string), dbfHeader.Fields[0].Type);
			Assertion.AssertEquals("Field 0: name", 25, dbfHeader.Fields[0].Length);

			Assertion.AssertEquals("Field 251: name", "SAMP_POP", dbfHeader.Fields[251].Name);
			Assertion.AssertEquals("Field 251: name", 'N', dbfHeader.Fields[251].DbaseType);
			Assertion.AssertEquals("Field 251: name", typeof(double), dbfHeader.Fields[251].Type);
			Assertion.AssertEquals("Field 251: name", 19, dbfHeader.Fields[251].Length);

			// note alaska and hawaii are missing - hence 48 states not 50.
			int i=0;
			foreach(ArrayList columnValues in dbfReader)
			{
				if (i==0)
				{
					Assertion.AssertEquals("Row1: STATE_NAME:","Illinois", columnValues[0]);
					Assertion.AssertEquals("Row1: STATE_FIPS:","17", columnValues[1].ToString());
					Assertion.AssertEquals("Row1: SAMP_POP", 1747776.0, columnValues[251]);
				}
				if (i==48)
				{
					Assertion.AssertEquals("Row1: STATE_NAME:","Washington", columnValues[0]);
					Assertion.AssertEquals("Row1: STATE_FIPS:","53", columnValues[1].ToString());
					Assertion.AssertEquals("Row1: SAMP_POP", 736744.0, columnValues[251]);
				}
				i++;
			}
			Assertion.AssertEquals("48 Records",48,i-1);

			DbaseFileReader dbfReader2 = new DbaseFileReader(Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\statepop.dbf");
			
			i=0;
			foreach(ArrayList columnValues in dbfReader2)
			{
				if (i==0)
				{
					Assertion.AssertEquals("Row1: STATE_NAME:","Illinois", columnValues[0]);
					Assertion.AssertEquals("Row1: STATE_FIPS:","17", columnValues[1].ToString());
					Assertion.AssertEquals("Row1: SAMP_POP", 1747776.0, columnValues[251]);
				}
				if (i==48)
				{
					Assertion.AssertEquals("Row1: STATE_NAME:","Washington", columnValues[0]);
					Assertion.AssertEquals("Row1: STATE_FIPS:","53", columnValues[1].ToString());
					Assertion.AssertEquals("Row1: SAMP_POP", 736744.0, columnValues[251]);
				}
				i++;
			}
			Assertion.AssertEquals("48 Records",48,i-1);
		}
	}
}

