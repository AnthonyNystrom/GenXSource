/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */

#region using
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Diagnostics;
using Geotools.Geometries;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// This class is used to read and write ESRI Shapefiles.
	/// </summary>
	public class Shapefile
	{
		private static BooleanSwitch _tracingSwitch = new BooleanSwitch("Shapefile", "Shapefile reader");

		internal static int ShapefileId = 9994;
		internal static int Version = 1000;

		/// <summary>
		/// Use for debugging.
		/// </summary>
		internal static BooleanSwitch TraceSwitch
		{
			get
			{
				return _tracingSwitch;
			}
		}
		/// <summary>
		/// Given a geomtry object, returns the equilivent shape file type.
		/// </summary>
		/// <param name="geom">A Geometry object.</param>
		/// <returns>The equilivent for the geometry object.</returns>
		public static ShapeType GetShapeType(Geometry geom) 
		{
			if (geom is Point) 
			{
				return ShapeType.Point;
			}

			if (geom is Polygon) 
			{
				return ShapeType.Polygon;
			}

			if (geom is MultiPolygon) 
			{
				return ShapeType.Polygon;
			}

			if (geom is LineString) 
			{
				return ShapeType.Arc;
			}

			if (geom is MultiLineString) 
			{
				return ShapeType.Arc;
			}
			return ShapeType.Undefined;
		}

		/// <summary>
		/// Returns the appropriate class to convert a shaperecord to an OGIS geometry given the type of shape.
		/// </summary>
		/// <param name="type">The shapefile type.</param>
		/// <returns>An instance of the appropriate handler to convert the shape record to a Geometry object.</returns>
		public static ShapeHandler GetShapeHandler(ShapeType type) 
		{
			switch (type) 
			{
				case ShapeType.Point:
					return new PointHandler();

				case ShapeType.Polygon:
					return new PolygonHandler();

				case ShapeType.Arc:
					return new MultiLineHandler();
			}
			return null;
		}

		/// <summary>
		/// Returns an ShapefileDataReader representing the data in a shapefile.
		/// </summary>
		/// <param name="filename">The filename (minus the . and extension) to read.</param>
		/// <param name="geometryFactory">The geometry factory to use when creating the objects.</param>
		/// <returns>An ShapefileDataReader representing the data in the shape file.</returns>
		public static ShapefileDataReader CreateDataReader(string filename, GeometryFactory geometryFactory)
		{
			if (filename==null)
			{
				throw new ArgumentNullException("filename");
			}
			if (geometryFactory==null)
			{
				throw new ArgumentNullException("geometryFactory");
			}
			ShapefileDataReader shpDataReader= new ShapefileDataReader(filename,geometryFactory);
			return shpDataReader;
		}

		/// <summary>
		/// Creates a DataTable representing the information in a shape file.
		/// </summary>
		/// <param name="filename">The filename (minus the . and extension) to read.</param>
		/// <param name="tableName">The name to give to the table.</param>
		/// <param name="geometryFactory">The geometry factory to use when creating the objects.</param>
		/// <returns>DataTable representing the data </returns>
		public static DataTable CreateDataTable(string filename, string tableName, GeometryFactory geometryFactory)
		{
			if (filename==null)
			{
				throw new ArgumentNullException("filename");
			}
			if (tableName==null)
			{
				throw new ArgumentNullException("tableName");
			}
			if (geometryFactory==null)
			{
				throw new ArgumentNullException("geometryFactory");
			}

			ShapefileDataReader shpfileDataReader= new ShapefileDataReader(filename, geometryFactory);
			DataTable table = new DataTable(tableName);
		
			// use ICustomTypeDescriptor to get the properies/ fields. This way we can get the 
			// length of the dbase char fields. Because the dbase char field is translated into a string
			// property, we lost the length of the field. We need to know the length of the
			// field when creating the table in the database.

			IEnumerator enumerator = shpfileDataReader.GetEnumerator();
			bool moreRecords = enumerator.MoveNext();
			ICustomTypeDescriptor typeDescriptor  = (ICustomTypeDescriptor)enumerator.Current;
			foreach(PropertyDescriptor property in typeDescriptor.GetProperties())
			{
				ColumnStructure column = (ColumnStructure)property;
				Type fieldType = column.PropertyType;
				DataColumn datacolumn = new DataColumn(column.Name, fieldType);
				if (fieldType== typeof(string))
				{
					// use MaxLength to pass the length of the field in the dbase file
					datacolumn.MaxLength=column.Length;
				}
				table.Columns.Add( datacolumn );
			}

			// add the rows - need a do-while loop because we read one row in order to determine the fields
			int iRecordCount=0;
			object[] values = new object[shpfileDataReader.FieldCount];
			do
			{
				iRecordCount++;
				shpfileDataReader.GetValues(values);
				table.Rows.Add(values);
				moreRecords = enumerator.MoveNext();
			} while (moreRecords);

			//Debug.Assert(shpfileDataReader.RecordCount != iRecordCount," Records in DataReader did not match property.");
			return table;
		}

		

		/// <summary>
		/// Imports a shapefile into a dababase table.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method assumes a table has already been crated in the database.
		/// </para>
		/// <para>Calling this method does not close the connection that is passed in.</para>
		/// </remarks>
		/// <param name="filename"></param>
		/// <param name="connectionstring"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public static int ImportShapefile(string filename, string connectionstring, string tableName)
		{
			int rowsAdded =-1;
			SqlConnection connection = new SqlConnection(connectionstring); 
		
			PrecisionModel pm = new PrecisionModel();
			GeometryFactory geometryFactory = new GeometryFactory(pm,-1);


			DataTable shpDataTable = Geotools.IO.Shapefile.CreateDataTable(filename, tableName, geometryFactory);
			string createTableSql = CreateDbTable(shpDataTable, true);
		
			SqlCommand createTableCommand = new SqlCommand(createTableSql, connection);
			connection.Open();

			
			createTableCommand.ExecuteNonQuery();

			string sqlSelect = String.Format("select * from {0}", tableName);
			SqlDataAdapter selectCommand = new SqlDataAdapter(sqlSelect, connection);
				
			// use a data adaptor - saves donig the inserts ourselves
			SqlDataAdapter dataAdapter = new SqlDataAdapter();
			dataAdapter.SelectCommand = new SqlCommand(sqlSelect, connection);
			SqlCommandBuilder custCB = new SqlCommandBuilder(dataAdapter);
			DataSet ds = new DataSet();

			// fill dataset
			dataAdapter.Fill(ds, shpDataTable.TableName);
	
			// copy rows from our datatable to the empty table in the DataSet
			int i=0;
			foreach(DataRow row in shpDataTable.Rows)
			{
				DataRow newRow = ds.Tables[0].NewRow();
				newRow.ItemArray=row.ItemArray;
				//gotcha! - new row still needs to be added to the table.
				//NewRow() just creates a new row with the same schema as the table. It does
				//not add it to the table.
				ds.Tables[0].Rows.Add(newRow); 
				i++;
			}

			// update all the rows in batch
			rowsAdded = dataAdapter.Update(ds, shpDataTable.TableName);
			int iRows = shpDataTable.Rows.Count;
			Debug.Assert(rowsAdded != iRows, String.Format("{0} of {1] rows were added to the database.",rowsAdded, shpDataTable.Rows.Count));
		
			//connection.Close();
			//connection.Dispose();
		
			return rowsAdded;
		}


		private static string CreateDbTable(DataTable table, bool deleteExisting)
		{
			StringBuilder sb = new StringBuilder();
			if (deleteExisting)
			{
				sb.AppendFormat("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)\n",table.TableName);
				sb.AppendFormat("drop table [dbo].[{0}]\n",table.TableName);
			}

			sb.AppendFormat("CREATE TABLE [dbo].[{0}] ( \n",table.TableName);
			for (int i=0; i < table.Columns.Count; i++)
			{
				string type = GetDbType(table.Columns[i].DataType, table.Columns[i].MaxLength );
				string columnName = table.Columns[i].ColumnName;
				if (columnName=="PRIMARY")
				{
					columnName="DBF_PRIMARY";
					Debug.Assert(false, "Shp2Db: Column PRIMARY renamed to PRIMARY.");
					Trace.WriteLine("Shp2Db: Column PRIMARY renamed to PRIMARY.");
				}
				sb.AppendFormat("[{0}] {1} ", columnName, type );
				
				// the unique id column cannot be null
				if (i==1)
				{
					sb.Append(" NOT NULL ");
				}
				if (i+1 != table.Columns.Count)
				{
					sb.Append(",\n");
				}
			}
			sb.Append(")\n");

			// the DataSet update stuff requires a unique column - so give it the row colum that we added
			//sb.AppendFormat("ALTER TABLE [dbo].[{0}] WITH NOCHECK ADD CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED ([{1}])  ON [PRIMARY]\n",table.TableName, table.Columns[1].ColumnName);
			return sb.ToString();
		}

		
		
		private static string GetDbType(Type type, int length)
		{
			if (type==typeof(double))
			{
				return "real";
			}
			else if (type==typeof(float))
			{
				return "float";
			}
			else if (type==typeof(string))
			{
				return String.Format("nvarchar({0}) ", length);
			}
			else if (type==typeof(byte[]))
			{
				return "image";
			}
			else if (type==typeof(int))
			{
				return "int";
			}
			else if (type==typeof(char[]))
			{
				return String.Format("nvarchar({0}) ", length);
			}
			throw new NotSupportedException("Need to add the SQL type for "+type.Name);
		}
	}
}