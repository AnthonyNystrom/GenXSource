#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/Shapefile/TestShapefileDataReader.cs,v 1.1 2003/01/02 20:37:30 awcoats Exp $
 * $Log: TestShapefileDataReader.cs,v $
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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Geotools.Geometries;
using Geotools.IO;
using NUnit.Framework;
using Geotools.UnitTests.Utilities;
#endregion

namespace Geotools.UnitTests.IO.Shapefile
{
	/// <summary>
	/// Summary description for TestShapefileDataReader.
	/// </summary>
	[TestFixture]
	public class TestShapefileDataReader
	{
		

		#region Methods
	

		public void Test_TestConstructor() 
		{
			PrecisionModel pm = new PrecisionModel(100000,0,0);
			GeometryFactory geometryFactory = new GeometryFactory(pm,-1);

			string filename= Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\statepop";
			ShapefileDataReader shpDataReader = Geotools.IO.Shapefile.CreateDataReader(filename, geometryFactory);

			int i=0;
			//for( i=0;i<shpDataReader.FieldCount;i++)
			//{
			//	Console.WriteLine(i+" "+shpDataReader.GetName(i) +" " +shpDataReader.GetFieldType(i));
			//}
			
			Assertion.AssertEquals("Dbase header: Num records", 49, shpDataReader.RecordCount);
			Assertion.AssertEquals("Dbase header: Num fields", 253, shpDataReader.FieldCount);
			
			Assertion.AssertEquals("Field 1: name", "STATE_NAME", shpDataReader.GetName(1));
			Assertion.AssertEquals("Field 1: name", typeof(string), shpDataReader.GetFieldType(1));

			Assertion.AssertEquals("Field 252: name", "SAMP_POP", shpDataReader.GetName(252));
			Assertion.AssertEquals("Field 252: name", typeof(double), shpDataReader.GetFieldType(252));


			// note alaska and hawaii are missing - hence 48 states not 50.
			i=0;
			foreach(object columnValues in shpDataReader)
			{
				if (columnValues==null)
				{
					Assertion.Fail("columnValues should not be null.");
				}
				if (i==0)
				{
					Assertion.AssertEquals("Row1: STATE_NAME:","Illinois", shpDataReader.GetString(1));
					Assertion.AssertEquals("Row1: STATE_FIPS:","17", shpDataReader.GetValue(2));
					Assertion.AssertEquals("Row1: SAMP_POP", 1747776.0, shpDataReader.GetDouble(252));
				}
				if (i==48)
				{
					Assertion.AssertEquals("Row1: STATE_NAME:","Washington", shpDataReader.GetString(1));
					Assertion.AssertEquals("Row1: STATE_FIPS:","53", shpDataReader.GetValue(2));
					Assertion.AssertEquals("Row1: SAMP_POP", 736744.0, shpDataReader.GetDouble(252));
				}
				i++;
			}
			Assertion.AssertEquals("49 Records",49,i);

			// try opening the file again, to make sure file is not locked from previous reader.
			ShapefileDataReader shpDataReader2 = Geotools.IO.Shapefile.CreateDataReader(filename, geometryFactory);
			i=0;
			while (shpDataReader2.Read())
			{
				if (i==0)
				{
					Assertion.AssertEquals("Row1: STATE_NAME:","Illinois", shpDataReader2.GetString(1));
					Assertion.AssertEquals("Row1: STATE_FIPS:","17", shpDataReader2.GetValue(2));
					Assertion.AssertEquals("Row1: SAMP_POP", 1747776.0, shpDataReader2.GetDouble(252));
				}
				if (i==48)
				{
					Assertion.AssertEquals("Row1: STATE_NAME:","Washington", shpDataReader2.GetString(1));
					Assertion.AssertEquals("Row1: STATE_FIPS:","53", shpDataReader2.GetValue(2));
					Assertion.AssertEquals("Row1: SAMP_POP", 736744.0, shpDataReader2.GetDouble(252));
				}
				i++;
			}
			Assertion.AssertEquals("49 Records",49,i);
		}

		public void Test_TestGets() 
		{
			PrecisionModel pm = new PrecisionModel(1,0,0);
			GeometryFactory geometryFactory = new GeometryFactory(pm,-1);

			string filename= Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\statepop";
			ShapefileDataReader shpDataReader = Geotools.IO.Shapefile.CreateDataReader(filename, geometryFactory);


			// note alaska and hawaii are missing - hence 48 states not 50.
			
			// tests GetValues().
			object[] values = new object[shpDataReader.FieldCount];
			foreach(object columnValues in shpDataReader)
			{
				if (columnValues==null)
				{
					Assertion.Fail("columnValues should have data.");
				}
				// get  values using GetValue()
				for(int i = 0; i < shpDataReader.FieldCount; i++)
				{
					values[i] = shpDataReader.GetValue(i);
				}

				// get values using GetValues()
				object[] values2=new object[shpDataReader.FieldCount];
				shpDataReader.GetValues(values2);

				// ensure they are both the same.
				for(int i = 0; i < shpDataReader.FieldCount; i++)
				{
					Assertion.AssertEquals("Values "+i, values[i], values2[i]);
				}
			}		
		}

		public void Test_TestRead() 
		{
			PrecisionModel pm = new PrecisionModel(100000,0,0);
			GeometryFactory geometryFactory = new GeometryFactory(pm,-1);

			//DataSets can be enumerated thru, or you can use Read(). This one tests the read;
			string filename= Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\statepop";
			ShapefileDataReader shpDataReader = Geotools.IO.Shapefile.CreateDataReader(filename, geometryFactory);

			int i=0;
			while (shpDataReader.Read())
			{
				i++;
			}
			Assertion.AssertEquals("Read using Read()",49,i);
		}

		public void Test_DataGrid()
		{
			PrecisionModel pm = new PrecisionModel(100000,0,0);
			GeometryFactory geometryFactory = new GeometryFactory(pm,-1);

			string filename= Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\statepop";
			ShapefileDataReader shpDataReader = Geotools.IO.Shapefile.CreateDataReader(filename, geometryFactory);

			// make sure the datagrid gets the column headings.
			DataGrid grid = new DataGrid();
			grid.DataSource = shpDataReader;
			grid.DataBind();

			TextWriter tempWriter = new StringWriter();
			grid.RenderControl(new HtmlTextWriter(tempWriter));
			string html = tempWriter.ToString();
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\ExpectedDataGridDataReader.txt",html);
			Assertion.AssertEquals("Datagrid properties",true,same);
		}
		#endregion
	}
}
