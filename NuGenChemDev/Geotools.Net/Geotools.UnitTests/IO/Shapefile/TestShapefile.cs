#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/Shapefile/TestShapefile.cs,v 1.1 2003/01/02 20:37:30 awcoats Exp $
 * $Log: TestShapefile.cs,v $
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
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using NUnit.Framework;
using Geotools.Geometries;
using Geotools.IO;
using Geotools.UnitTests.Utilities;
#endregion

namespace Geotools.UnitTests.IO.Shapefile
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.IO.Shapefile.TestShapefile class
	/// </summary>
	public class TestShapefile 
	{
		

		/// <summary>
		/// Test getting and setting the properties
		/// </summary>
		public void Test_MultipleRead() 
		{
			PrecisionModel pm = new PrecisionModel(1,0,0);
			GeometryFactory geometryFactory = new GeometryFactory(pm,-1);

			string filename= Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\statepop.shp";
			
			// tests two readers reading the file as the same time.
			Geotools.IO.ShapefileReader shpFile = new Geotools.IO.ShapefileReader(filename, geometryFactory);
			Geotools.IO.ShapefileReader shpFile2 = new Geotools.IO.ShapefileReader(filename, geometryFactory);
			foreach(object row in shpFile)
			{
				Assertion.AssertNotNull(row);
				foreach(object row2 in shpFile2)
				{
					Assertion.AssertNotNull(row2);
				}
			}	
		}

		public void Test_CreateDataTable()
		{
			PrecisionModel pm = new PrecisionModel(100,0,0);
			GeometryFactory geometryFactory = new GeometryFactory(pm,-1);

			string filename= Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\statepop";
			DataTable table = Geotools.IO.Shapefile.CreateDataTable(filename, "State", geometryFactory);
			DataSet ds = new DataSet();
			ds.Tables.Add(table);

			// make sure the datagrid gets the column headings.
			DataGrid grid = new DataGrid();
			grid.DataSource = ds;
			grid.DataMember="State";
			grid.DataBind();

			TextWriter tempWriter = new StringWriter();
			grid.RenderControl(new HtmlTextWriter(tempWriter));
			string html = tempWriter.ToString();
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\ExpectedDataGridDataReader.txt",html);
			Assertion.AssertEquals("Datagrid properties",true,same);
		}
	}
}

