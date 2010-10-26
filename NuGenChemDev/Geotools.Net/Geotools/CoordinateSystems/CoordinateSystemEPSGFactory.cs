/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. (translated from Java Topology Suite, 
 *  Copyright 2001 Vivid Solutions)
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

#region Using
using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using Geotools.CoordinateTransformations;
using Geotools.Utilities;
#endregion


namespace Geotools.CoordinateReferenceSystems
{
	/// <summary>
	/// A factory class that creates objects using codes defined by the <a href="http://www.epsg.org/">EPSG</a>.
	/// </summary>
	/// <remarks>
	/// EPSG, through its geodesy working group, maintains and publishes a data set of parameters for coordinate system and coordinate transformation description. The data is supported through formulae given in Guidance Note number 7. The EPSG Geodetic Parameters have been included as reference data in the GeoTIFF data exchange specifications, in the Iris21 data model and in Epicentre (the POSC data model). 
	/// </remarks>
	public class CoordinateSystemEPSGFactory : ICoordinateSystemAuthorityFactory
	{
		#region Static methods
		/// <summary>
		/// Uses the default database.
		/// </summary>
		/// <remarks>
		/// The default database must be placed in the working directory. An error occures if the file
		/// cannot be found. The name of the database must be 'EPSG_v61.mcb'.
		/// </remarks>
		/// <returns>Returns an instance of CoordinateSystemEPSGFactory.</returns>
		public static CoordinateSystemEPSGFactory UseDefaultDatabase()
		{
			string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=EPSG_v61.mdb";
			OleDbConnection connection = new OleDbConnection(connectionString);
			CoordinateSystemEPSGFactory factory = new CoordinateSystemEPSGFactory(connection);
			return factory;
		}
		#endregion

		#region Constructors
		private IDbConnection _databaseConnection;
		/// <summary>
		/// Initializes a new instance of the CoordinateSystemEPSGFactory class.
		/// </summary>
		public CoordinateSystemEPSGFactory(IDbConnection databaseConnection)
		{
			if (databaseConnection==null)
			{
				throw new ArgumentNullException("databaseConnection");
			}
			_databaseConnection = databaseConnection;
		}
		#endregion

		#region Implementation of ICoordinateSystemAuthorityFactory

		/// <summary>
		/// Returns a GeographicCoordinateSystem object from a code.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>An object that implements the IGeographicCoordinateSystem interface.</returns>
		public IGeographicCoordinateSystem CreateGeographicCoordinateSystem(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =	"SELECT COORD_REF_SYS_NAME, COORD_REF_SYS_CODE, AREA_OF_USE_CODE, "+
								"	COORD_REF_SYS_KIND, DATUM_CODE, COORD_SYS_CODE, "+
								"	SOURCE_GEOGCRS_CODE, PROJECTION_CONV_CODE, CMPD_VERTCRS_CODE, CRS_SCOPE, CMPD_HORIZCRS_CODE, REMARKS, DATA_SOURCE "+
								"FROM  [Coordinate Reference System] "+
								"WHERE COORD_REF_SYS_CODE = {0}";

			
			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("Geographic Coordinate System with a code {0} not found in the CRS table in the EPSG database.",code));
			};

			string coordSysCode = reader["COORD_SYS_CODE"].ToString().ToLower();
			string coordSysName = reader["COORD_REF_SYS_NAME"].ToString();
			string name = reader["COORD_REF_SYS_NAME"].ToString();
			string horizontalDatumCode = reader["DATUM_CODE"].ToString();
			string coordRefKind = reader["COORD_REF_SYS_KIND"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString(); // should always be EPSG??
			string remarks = reader["REMARKS"].ToString();
			
			if (coordRefKind.ToLower() != "geographic 2d")
			{
				throw new ArgumentException(String.Format("CRS code {0} is not a geographic coordinate system but a {1}.",code,coordRefKind));
			}

			Database.CheckOneRow(reader,code,"Geographic CRC code");

			string primeMeridianCode = "";
			IPrimeMeridian primeMeridian = null;
			IHorizontalDatum horizontalDatum= null;
			if (horizontalDatumCode=="")
			{
				horizontalDatum = HorizontalDatum.WGS84;//this.CreateHorizontalDatum( horizontalDatumCode );
				primeMeridianCode = this.CreatePrimeMeridianCodeFromDatum(horizontalDatumCode);
				primeMeridian  = this.CreatePrimeMeridian( primeMeridianCode );
			}
			else
			{
				horizontalDatum = this.CreateHorizontalDatum( horizontalDatumCode );
				primeMeridianCode = this.CreatePrimeMeridianCodeFromDatum(horizontalDatumCode);
				primeMeridian  = this.CreatePrimeMeridian( primeMeridianCode );
			}

			// we get the information for the axis 
			IAxisInfo[] axisInfos = GetAxisInfo(coordSysCode);
			IAngularUnit angularUnit = new AngularUnit(1);
			
			
			IAxisInfo axisInfo1 = axisInfos[0];
			IAxisInfo axisInfo2 = axisInfos[1];
			IGeographicCoordinateSystem geographicCoordSys = new GeographicCoordinateSystem(angularUnit, horizontalDatum, primeMeridian, axisInfo1, axisInfo2,remarks,datasource,code,name,"","");
			return geographicCoordSys;
			
		}

		/// <summary>
		/// Creates a 3D coordinate system from a code.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>An object that implements the ICompoundCoordinateSystem interface.</returns>
		public ICompoundCoordinateSystem CreateCompoundCoordinateSystem(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =	"SELECT COORD_REF_SYS_NAME, COORD_REF_SYS_CODE, AREA_OF_USE_CODE, "+
				"	COORD_REF_SYS_KIND, DATUM_CODE, COORD_SYS_CODE, "+
				"	SOURCE_GEOGCRS_CODE, PROJECTION_CONV_CODE, CMPD_VERTCRS_CODE, CRS_SCOPE, CMPD_HORIZCRS_CODE, DATA_SOURCE, REMARKS "+
				"FROM  [Coordinate Reference System] "+
				"WHERE COORD_REF_SYS_CODE = {0}";

			
			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("Geographic Coordinate System with a code {0} not found in the CRS table in the EPSG database.",code));
			}

			
			string coordSysCode = reader["COORD_SYS_CODE"].ToString().ToLower();
			string coordSysName = reader["COORD_REF_SYS_NAME"].ToString();
			string name = reader["COORD_REF_SYS_NAME"].ToString();
			string verticalCRSCode = reader["CMPD_VERTCRS_CODE"].ToString();
			string horizontalCRSCode = reader["CMPD_HORIZCRS_CODE"].ToString();
			string coordRefKind = reader["COORD_REF_SYS_KIND"].ToString();
			string remarks = reader["REMARKS"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString(); // should always be EPSG??


			Database.CheckOneRow(reader,code,"Copound CRS code");
			if (coordRefKind.ToLower() != "compound")
			{
				throw new ArgumentException(String.Format("CRS code {0} is not a projected coordinate system but a {1}.",code,coordRefKind));
			}
		
			ICoordinateSystem   headCRS = this.CreateCoordinateSystem( horizontalCRSCode);
			ICoordinateSystem   tailCRS = this.CreateCoordinateSystem( verticalCRSCode  );									

			ICompoundCoordinateSystem compoundCRS = new CompoundCoordinateSystem(headCRS, tailCRS, remarks, datasource, code, name,"","");
			return compoundCRS;	
		}
	
		
		/// <summary>
		/// Returns a LinearUnit object from a code.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>An object that implements the ILinearUnit interface.</returns>
		public ILinearUnit CreateLinearUnit(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =   "SELECT UNIT_OF_MEAS_NAME, "+
				"   UNIT_OF_MEAS_TYPE, TARGET_UOM_CODE, "+
				"   FACTOR_B, FACTOR_C, " +
				"	REMARKS, INFORMATION_SOURCE, DATA_SOURCE "+
				"FROM [Unit of Measure]"+
				"WHERE UOM_CODE={0}";

			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			ILinearUnit linearUnit = null;
			bool recordFound = reader.Read();
			if (!recordFound)
			{
				throw new ArgumentOutOfRangeException(String.Format("Linear unit with a code of '{0}' was not found in the database.",code));
			}
		
			
			string unitOfMeasureType = reader["UNIT_OF_MEAS_TYPE"].ToString();
			if (unitOfMeasureType.ToLower()!="length")
			{
				throw new ArgumentException(String.Format("Requested unit ({0}) is not a linear unit.",unitOfMeasureType));
			}	
			double metersPerUnit = (double)reader["FACTOR_B"];
			double factor = (double)reader["FACTOR_C"];
			string remarks = reader["REMARKS"].ToString();
			string name = reader["UNIT_OF_MEAS_NAME"].ToString();
			linearUnit = new LinearUnit(metersPerUnit * factor ,remarks,"EPSG",code,name,"","");

			Database.CheckOneRow(reader, code, "Linear Unit");

			return linearUnit;
		}

		/// <summary>
		/// Gets the Geoid code from a WKT name. In the OGC definition of WKT horizontal datums, the geoid is referenced by a quoted string, which is used as a key value.  This method converts the key value string into a code recognized by this authority.
		/// </summary>
		/// <param name="wkt">WKT text name.</param>
		/// <returns>String containing the Geoid.</returns>
		public string GeoidFromWKTName(string wkt)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new vertical coordinate system object from a code.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>An object that implements the IVerticalCoordinateSystem interface.</returns>
		public IVerticalCoordinateSystem CreateVerticalCoordinateSystem(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =	"SELECT COORD_REF_SYS_NAME, COORD_REF_SYS_CODE, AREA_OF_USE_CODE, "+
				"	COORD_REF_SYS_KIND, DATUM_CODE, COORD_SYS_CODE, "+
				"	SOURCE_GEOGCRS_CODE, PROJECTION_CONV_CODE, CMPD_VERTCRS_CODE, CRS_SCOPE, CMPD_HORIZCRS_CODE, DATA_SOURCE, REMARKS "+
				"FROM  [Coordinate Reference System] "+
				"WHERE COORD_REF_SYS_CODE = {0}";

			
			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("Geographic Coordinate System with a code {0} not found in the CRS table in the EPSG database.",code));
			}

			
			string coordSysCode = reader["COORD_SYS_CODE"].ToString().ToLower();
			string coordSysName = reader["COORD_REF_SYS_NAME"].ToString();
			string name = reader["COORD_REF_SYS_NAME"].ToString();
			string verticalDatumCode = reader["DATUM_CODE"].ToString();
			string coordRefKind = reader["COORD_REF_SYS_KIND"].ToString();
			string remarks = reader["REMARKS"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString(); // should always be EPSG??


			Database.CheckOneRow(reader,code,"Coordinate Reference System");
			if (coordRefKind.ToLower() != "vertical")
			{
				throw new ArgumentException(String.Format("CRS code {0} is not a projected coordinate system but a {1}.",code,coordRefKind));
			}
			
			IVerticalDatum verticalDatum = this.CreateVerticalDatum(verticalDatumCode);
			VerticalCoordinateSystem vrs = new VerticalCoordinateSystem(coordSysName, verticalDatum,remarks,datasource,code,"","");
			return vrs;
		}

		/// <summary>
		/// Creates a projected coordinate system using the given code.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>A IProjectedCoordinateSystem object.</returns>
		public IProjectedCoordinateSystem CreateProjectedCoordinateSystem(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =	"SELECT COORD_REF_SYS_NAME, COORD_REF_SYS_CODE, AREA_OF_USE_CODE, "+
				"	COORD_REF_SYS_KIND, DATUM_CODE, COORD_SYS_CODE, "+
				"	SOURCE_GEOGCRS_CODE, PROJECTION_CONV_CODE, CMPD_VERTCRS_CODE, CRS_SCOPE, CMPD_HORIZCRS_CODE, DATA_SOURCE, REMARKS "+
				"FROM  [Coordinate Reference System] "+
				"WHERE COORD_REF_SYS_CODE = {0}";

			
			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("Geographic Coordinate System with a code {0} not found in the CRS table in the EPSG database.",code));
			}

			
			string coordSysCode = reader["COORD_SYS_CODE"].ToString().ToLower();
			string coordSysName = reader["COORD_REF_SYS_NAME"].ToString();
			string name = reader["COORD_REF_SYS_NAME"].ToString();
			string horizontalDatumCode = reader["DATUM_CODE"].ToString();
			string geographicCRSCode = reader["SOURCE_GEOGCRS_CODE"].ToString();
			string projectionCode = reader["PROJECTION_CONV_CODE"].ToString();
			string coordRefKind = reader["COORD_REF_SYS_KIND"].ToString();
			string remarks = reader["REMARKS"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString(); // should always be EPSG??


			Database.CheckOneRow(reader,code,"Geographic CRC code");
			if (coordRefKind.ToLower() != "projected")
			{
				throw new ArgumentException(String.Format("CRS code {0} is not a projected coordinate system but a {1}.",code,coordRefKind));
			}

			string primeMeridianCode = "";
			IPrimeMeridian primeMeridian = null;
			IHorizontalDatum horizontalDatum= null;
			if (horizontalDatumCode!="")
			{
				horizontalDatum = HorizontalDatum.WGS84;//this.CreateHorizontalDatum( horizontalDatumCode );
				primeMeridianCode = this.CreatePrimeMeridianCodeFromDatum(horizontalDatumCode);
				primeMeridian  = this.CreatePrimeMeridian( primeMeridianCode );
			}

			// we get the information for the axis 
			IAxisInfo[] axisInfos = GetAxisInfo(coordSysCode);
			
			ICoordinateTransformationAuthorityFactory factory = new CoordinateTransformationEPSGFactory(_databaseConnection);

			ICoordinateTransformation mathtransform = factory.CreateFromCoordinateSystemCodes(geographicCRSCode,"");
			string methodOperation = this.GetMethodOperationCodeFromProjectionCode( projectionCode );
			IProjection projection = this.CreateProjection(methodOperation, projectionCode);
			IGeographicCoordinateSystem geographicCoordSystem = this.CreateGeographicCoordinateSystem( geographicCRSCode );
			ILinearUnit linearUnit = LinearUnit.Meters;
			IProjectedCoordinateSystem projectedCoordSys = new ProjectedCoordinateSystem(horizontalDatum, axisInfos,geographicCoordSystem, linearUnit, projection, remarks,datasource,code,coordSysName,"","");
													
			return projectedCoordSys;
		}
		


		/// <summary>
		/// Returns an AngularUnit object from a code.
		/// </summary>
		/// <remarks>
		/// Some common angular units and their codes are described in the table below.
		/// <list type="table">
		/// <listheader><term>EPSG Code</term><description>Descriptions</description></listheader>
		/// <item><term>9101</term><description>Radian</description></item>
		/// <item><term>9102</term><description>Degree</description></item>
		/// <item><term>9103</term><description>Arc-minute</description></item>
		/// <item><term>9104</term><description>Arc-second</description></item>
		/// </list>
		/// </remarks>
		/// <param name="code">The EPSG code.</param>
		/// <returns>An object that implements the IAngularUnit interface.</returns>
		public IAngularUnit CreateAngularUnit(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =   "SELECT UNIT_OF_MEAS_NAME, "+
				"   UNIT_OF_MEAS_TYPE, TARGET_UOM_CODE, "+
				"   FACTOR_B, FACTOR_C, " +
				"	REMARKS, INFORMATION_SOURCE, DATA_SOURCE "+
				"FROM [Unit of Measure]"+
				"WHERE UOM_CODE={0}";

			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			IAngularUnit angularUnit = null;
			bool recordFound = reader.Read();
			if (!recordFound)
			{
				throw new ArgumentOutOfRangeException(String.Format("Angular unit with a code of '{0}' was not found in the database.",code));
			}

	
			string unitOfMeasureType = reader["UNIT_OF_MEAS_TYPE"].ToString();
			if (unitOfMeasureType.ToLower()!="angle")
			{
				throw new ArgumentException(String.Format("Requested unit ({0}) is not a angular unit.",unitOfMeasureType));
			}	
			string remarks = reader["REMARKS"].ToString();
			string name = reader["UNIT_OF_MEAS_NAME"].ToString();
			string targetUOMcode = reader["TARGET_UOM_CODE"].ToString();
			
			if (!reader.IsDBNull(3)) //TODO: 4th item in the select statement. Should realy determine this using GetOrdinal()
			{
				double factorB = (double)reader["FACTOR_B"];
				double factorC = (double)reader["FACTOR_C"];
				double radiansPerUnit = factorB/ factorC;
				Database.CheckOneRow(reader, code, "Angular Unit");
				angularUnit = new AngularUnit(radiansPerUnit, remarks, "EPSG",code,name,"","");
			}
			else
			{
				// some units have a null for the Factor B - so must then try using the other UOM code.
				Database.CheckOneRow(reader, code, "Angular Unit");
				angularUnit = this.CreateAngularUnit(targetUOMcode);
			}
			return angularUnit;
		}

		/// <summary>
		/// Creates a horizontal datum from a code.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>An object that implements the IHorizontalDatum interface.</returns>
		public IHorizontalDatum CreateHorizontalDatum(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			
			string sqlQuery =	"SELECT DATUM_NAME, DATUM_CODE, DATUM_TYPE, ORIGIN_DESCRIPTION, "+ 
				"REALIZATION_EPOCH, ELLIPSOID_CODE, PRIME_MERIDIAN_CODE, "+
				"AREA_OF_USE_CODE, DATUM_SCOPE, REMARKS, DATA_SOURCE, INFORMATION_SOURCE "+
				"FROM  Datum "+
				"WHERE DATUM_CODE={0}";

			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("Could not find datum {0}.",code));
			};
			string datumtype= reader["DATUM_TYPE"].ToString();			
			string ellipsoidCode = reader["ELLIPSOID_CODE"].ToString();
			string primeMeridianCode = reader["PRIME_MERIDIAN_CODE"].ToString();
			string name = reader["DATUM_NAME"].ToString();
			string remarks = reader["REMARKS"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString();// should always be EPSG?

			Database.CheckOneRow(reader, code, "Horizontal Datum");


			//TODO: need to populate wgsConversionInfo with the right parameters. 
			WGS84ConversionInfo wgsConversionInfo = new WGS84ConversionInfo();
			IEllipsoid ellipsoid = this.CreateEllipsoid( ellipsoidCode );
			IHorizontalDatum horizontalDatum = new HorizontalDatum(name, DatumType.IHD_Geocentric, ellipsoid, wgsConversionInfo,remarks,datasource,code,"","");
			return horizontalDatum;
		}

		/// <summary>
		/// Returns descriptive text about this factory.
		/// </summary>
		/// <remarks>This method has not been implemented.</remarks>
		/// <param name="code"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public string DescriptionText(string code)
		{
			//return "EPSG Coordinate System factory.";
			throw new NotImplementedException();
		}


		/// <summary>
		/// Create a vertical datum given a code.
		/// </summary>
		/// <param name="code">The EPSG code of the datum to create.</param>
		/// <returns>An object that implements the IVerticalDatum interface.</returns>
		public IVerticalDatum CreateVerticalDatum(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			
			string sqlQuery =	"SELECT DATUM_NAME, DATUM_CODE, DATUM_TYPE, ORIGIN_DESCRIPTION, "+ 
				"REALIZATION_EPOCH, ELLIPSOID_CODE, PRIME_MERIDIAN_CODE, "+
				"AREA_OF_USE_CODE, DATUM_SCOPE, REMARKS, DATA_SOURCE, INFORMATION_SOURCE "+
				"FROM  Datum "+
				"WHERE DATUM_CODE={0}";

			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("Could not find datum {0}.",code));
			};
			string datumtype= reader["DATUM_TYPE"].ToString();			
			string name = reader["DATUM_NAME"].ToString();
			string remarks = reader["REMARKS"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString();// should always be EPSG?

			Database.CheckOneRow(reader, code, "Vertical Datum");

			if (datumtype.ToLower()!="vertical")
			{
				throw new ArgumentException(String.Format("Requested datum ({0}) is not a vertical datum.", code));
			}
			IVerticalDatum verticalDatum = new VerticalDatum(DatumType.IVD_GeoidModelDerived, remarks, code,"EPSG",name,"","");
			return verticalDatum;
		}

		/// <summary>
		///	Gets the Geoid code from a WKT name. 	
		/// </summary>
		/// <remarks>In the OGC definition of WKT horizontal datums, the geoid is referenced by a quoted string, which is used as a key value.  This method converts the key value string into a code recognized by this authority.
		/// </remarks>
		/// <param name="wkt">The WKT name.</param>
		/// <returns>String with Geoid code.</returns>
		public string WktGeoidName(string wkt)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Creates a prime meridian given a code.
		/// </summary>
		/// <param name="code">The EPSG code of the prime meridian.</param>
		/// <returns>An object that implements the IPrimeMeridian interface.</returns>
		public IPrimeMeridian CreatePrimeMeridian(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =
				"SELECT PRIME_MERIDIAN_CODE, PRIME_MERIDIAN_NAME,GREENWICH_LONGITUDE,"+
				"	UOM_CODE,REMARKS,INFORMATION_SOURCE,DATA_SOURCE "+
				"FROM  [Prime Meridian]"+
				"WHERE PRIME_MERIDIAN_CODE={0}";

			sqlQuery = String.Format(sqlQuery, code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			IPrimeMeridian primeMeridian = null;
			bool recordFound = reader.Read();
			if (!recordFound)
			{
				throw new ArgumentOutOfRangeException(String.Format("Prime meridian with a code of '{0}' was not found in the database.",code));
			}
		
			double degreesFromGreenwich = (double)reader["GREENWICH_LONGITUDE"];
			string remarks = reader["REMARKS"].ToString();
			string name = reader["PRIME_MERIDIAN_NAME"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString();
			string unitsOfMeasure = reader["UOM_CODE"].ToString();

			Database.CheckOneRow(reader, code, "Prime meridian");
			IAngularUnit degrees = this.CreateAngularUnit(unitsOfMeasure);
			primeMeridian = new PrimeMeridian(name, degrees, degreesFromGreenwich,remarks,datasource,code,"","");
			return primeMeridian;
		}


		/// <summary>
		/// Creates an ellipsoid given a code.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>An object that implements the IEllipsoid interface.</returns>
		public IEllipsoid CreateEllipsoid(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =	"SELECT "+
				"	ELLIPSOID_CODE, ELLIPSOID_NAME, SEMI_MAJOR_AXIS, UOM_CODE, "+
				"	SEMI_MINOR_AXIS, INV_FLATTENING, ELLIPSOID_SHAPE, DATA_SOURCE,  "+
				"	REMARKS "+
				"FROM Ellipsoid "+
				"WHERE ELLIPSOID_CODE={0}";

			sqlQuery = String.Format(sqlQuery, code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			IEllipsoid ellipsoid = null;
			bool recordFound = reader.Read();
			if (!recordFound)
			{
				throw new ArgumentOutOfRangeException(String.Format("Ellipsoid with a code of '{0}' was not found in the database.",code));
			}

			int semiMinorAxisIndex = reader.GetOrdinal("SEMI_MINOR_AXIS");
			int inverseFlatteningIndex = reader.GetOrdinal("INV_FLATTENING");
			string ellipsoidName = reader["ELLIPSOID_NAME"].ToString();
			double semiMajorAxis = (double)reader["SEMI_MAJOR_AXIS"];
			string unitsOfMearureCode = reader["UOM_CODE"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString();
			bool ellipsoidShape = (bool)reader["ELLIPSOID_SHAPE"];
			string remarks = reader["REMARKS"].ToString();
			
			if (reader.IsDBNull(semiMinorAxisIndex))
			{
				double inverseFlattening = (double)reader["INV_FLATTENING"];
				Database.CheckOneRow(reader, code, "Ellipsoid");
				ILinearUnit linearUnit = CreateLinearUnit(unitsOfMearureCode);
				ellipsoid = new Ellipsoid(semiMajorAxis,0.0,inverseFlattening,true,linearUnit, remarks,datasource,code,ellipsoidName,"","");
			} 
			else if  (reader.IsDBNull(inverseFlatteningIndex))
			{
				double semiMinorAxis = (double)reader["SEMI_MINOR_AXIS"];
				Database.CheckOneRow(reader, code, "Ellipsoid");
				ILinearUnit linearUnit = CreateLinearUnit(unitsOfMearureCode);
				ellipsoid = new Ellipsoid(semiMajorAxis,semiMinorAxis, 0.0,false, linearUnit, remarks,datasource,code,ellipsoidName,"","");
			}
			return ellipsoid;
		}

		/// <summary>
		/// Creates a horizontal coordinate system given a code.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>An object that implements the IHorizontalCoordinateSystem interface.</returns>
		public IHorizontalCoordinateSystem CreateHorizontalCoordinateSystem(string code)
		{

			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =	"SELECT COORD_REF_SYS_NAME, COORD_REF_SYS_CODE, AREA_OF_USE_CODE, "+
				"	COORD_REF_SYS_KIND, DATUM_CODE, COORD_SYS_CODE, "+
				"	SOURCE_GEOGCRS_CODE, PROJECTION_CONV_CODE, CMPD_VERTCRS_CODE, CRS_SCOPE, CMPD_HORIZCRS_CODE, DATA_SOURCE, REMARKS "+
				"FROM  [Coordinate Reference System] "+
				"WHERE COORD_REF_SYS_CODE = {0}";

			
			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("Geographic Coordinate System with a code {0} not found in the CRS table in the EPSG database.",code));
			}

			
			string coordSysCode = reader["COORD_SYS_CODE"].ToString().ToLower();
			string coordSysName = reader["COORD_REF_SYS_NAME"].ToString();
			string name = reader["COORD_REF_SYS_NAME"].ToString();
			string datumCode = reader["DATUM_CODE"].ToString();
			string coordRefKind = reader["COORD_REF_SYS_KIND"].ToString();
			string remarks = reader["REMARKS"].ToString();
			string datasource = reader["DATA_SOURCE"].ToString(); // should always be EPSG??


			Database.CheckOneRow(reader,code,"Coordinate Reference System");
			if (coordRefKind.ToLower() != "horizontal")
			{
				throw new ArgumentException(String.Format("CRS code {0} is not a horizontal coordinate system but a {1}.",code,coordRefKind));
			}
			IAxisInfo[] axisInfos = GetAxisInfo(coordSysCode);
			IHorizontalDatum horizontalDatum = this.CreateHorizontalDatum(datumCode);
			HorizontalCoordinateSystem vrs = new HorizontalCoordinateSystem(horizontalDatum, axisInfos,remarks,datasource,code,name,"","");
			return vrs;
		}

		/// <summary>
		/// Gets the authorith which is ESPG.
		/// </summary>
		public string Authority
		{
			get
			{
				return "EPSG";
			}
		}
		#endregion

		#region Properties
		#endregion

		#region Public Helper methods
	
		/// <summary>
		/// Creates a coordinate system.
		/// </summary>
		/// <remarks>
		/// Creates the right kind of coordinate system for the given code. The available coordinate systems are
		/// <list type="bullet">
		/// <item><term>geographic 2d</term><description>Your Description</description></item>
		/// <item><term>projected</term><description>Your Description</description></item>
		/// <item><term>vertical</term><description>Your Description</description></item>
		/// <item><term>horizontal</term><description>Your Description</description></item>
		/// </list>
		/// </remarks>
		/// <param name="code">The EPSG code of the coordinate system.</param>
		/// <returns>An object that implements the ICoordinateSystem interface. </returns>
		public ICoordinateSystem CreateCoordinateSystem(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =	"SELECT COORD_REF_SYS_KIND "+
								"FROM  [Coordinate Reference System] "+
								"WHERE COORD_REF_SYS_CODE = {0}";
			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("Coordinate System with a code {0} not found in the CRS table in the EPSG database.",code));
			}

			string coordRefKind = reader["COORD_REF_SYS_KIND"].ToString().ToLower();
			Database.CheckOneRow(reader, code, "COORD_REF_SYS_CODE");

			ICoordinateSystem coordinateSystem= null;
			switch (coordRefKind)
			{
				case "geographic 2d":
					coordinateSystem = this.CreateGeographicCoordinateSystem(code);
					break;
				case "projected":
					coordinateSystem = this.CreateProjectedCoordinateSystem(code);
					break;
				case "vertical":
					coordinateSystem = this.CreateVerticalCoordinateSystem(code);
					break;
				case "horizontal":
					coordinateSystem = this.CreateHorizontalCoordinateSystem(code);
					break;
				default:
					throw new ArgumentException(String.Format("Coordinate system '{0}' (code='{1}') is not supported.",coordRefKind,code)); 
			}
			return coordinateSystem;

		}
		

		/// <summary>
		/// Returns the coordinate system type.
		/// </summary>
		/// <param name="code">The EPSG code for the coordinate system.</param>
		/// <returns>String with the coordinate system type.</returns>
		public string GetCoordinateSystemType(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery =	"select COORD_REF_SYS_KIND " +
				"from [Coordinate Reference System] "+
				"where COORD_REF_SYS_CODE = {0}";

			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);

			reader.Read();
			string coordSysType = reader["COORD_REF_SYS_KIND"].ToString();

			Database.CheckOneRow(reader, code, "GetCoordinateSystemType");

			return coordSysType;
		}

		
		/// <summary>
		/// Returns an array containing axis information.
		/// </summary>
		/// <param name="code">The EPSG code.</param>
		/// <returns>IAxisInfo[] containing axis information.</returns>
		public IAxisInfo[] GetAxisInfo(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			string sqlQuery = 
				  "select COORD_AXIS_NAME,"
				+ "		COORD_AXIS_ORIENTATION "
				+ "from [Coordinate Axis] AS ca,"
				+ "		[Coordinate Axis Name] AS can"
				+ "		where COORD_SYS_CODE = {0}"
				+ "		and ca.COORD_AXIS_NAME_CODE=can.COORD_AXIS_NAME_CODE "
				+ "order by ca.ORDER";
			/* the order by clause is odd - there is no column called 'ORDER' in the Coordinate_Axis table.
			 * there is a column called 'Axix Order'. Heh, this seems to work, so leave this in. There was
			 * some comments in the SeaGIS Java code, but they were in Frnech.
			 */

			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);

			int i=0;
			ArrayList axisList = new ArrayList();
			while (reader.Read())
			{
				IAxisInfo axisInfo = new AxisInfo(reader["COORD_AXIS_NAME"].ToString(),GetOrientation( reader["COORD_AXIS_ORIENTATION"].ToString()));
				axisList.Add(axisInfo);
				i++;
			}
			reader.Close();
			if (i==0)
			{
				throw new ArgumentException(String.Format("Could not find axis with a code of {0}",code));
			}
			IAxisInfo[] infolist = (IAxisInfo[])axisList.ToArray(typeof(IAxisInfo));
			return infolist;
		}

		/// <summary>
		/// Helper function to turn an string into an enumeration.
		/// </summary>
		/// <param name="code">The string representation of the axis orientation.</param>
		/// <returns>IAxisOrientationEnum enumation.</returns>
		/// <exception cref="NotSupportedException">If the code is not recognized.</exception>
		public AxisOrientation GetOrientation(string code)
		{
			if (code==null)
			{
				throw new ArgumentNullException("code");
			}
			AxisOrientation orientation = AxisOrientation.Other;
			switch (code.ToLower())
			{
				case "north": 
					orientation = AxisOrientation.North;
					break;
				case "south":
					orientation = AxisOrientation.South;
					break;
				case "east":
					orientation = AxisOrientation.East;
					break;
				case "west":
					orientation = AxisOrientation.West;
					break;
				case "up":
					orientation = AxisOrientation.Up;
					break;
				case "down":
					orientation = AxisOrientation.Down;
					break;
				default:
					throw new NotSupportedException(String.Format("The axis orientation '{0}' is not supported.",code));
			}
			return orientation;
		}

		/// <summary>
		/// Gets information about the parameters for a projection.
		/// </summary>
		/// <param name="projectionConversionCode">The projection code.</param>
		/// <param name="coordOperationMethod">The coordniate operation code.</param>
		/// <returns>IProjectionParameter[] containing information about the parameters.</returns>
		private ProjectionParameter[] GetProjectionParameterInfo(string projectionConversionCode, string coordOperationMethod)
		{
			ParameterList paramsList = GetParameters(projectionConversionCode);//e, coordOperationMethod);
			ProjectionParameter[] projectionParams = new ProjectionParameter[paramsList.Count];
			int i=0;
			foreach(string key in paramsList.Keys)
			{
				ProjectionParameter param = new ProjectionParameter(key,paramsList.GetDouble(key));
				projectionParams[i] = param;
				i++;
			}
			return projectionParams;
		}
		/// <summary>
		/// Gets projection parameters information.
		/// </summary>
		/// <param name="projectionConversionCode">The projeciton conversion code.</param>
		/// <returns>ParameterList with details about the projection parameters.</returns>
		public ParameterList GetParameters(string projectionConversionCode)//, string coordOperationMethod)
		{		
			string sqlQuery="SELECT param.PARAMETER_NAME, paramValue.PARAMETER_VALUE, paramUsage.Param_Sign_Reversal  "+
							"FROM  [Coordinate_Operation Parameter Usage] AS paramUsage, "+
							"	   [Coordinate_Operation Parameter Value] AS paramValue,  "+
							"	   [Coordinate_Operation Parameter] AS param  "+
							"WHERE "+
							"	paramUsage.COORD_OP_METHOD_CODE = paramValue.COORD_OP_METHOD_CODE AND "+
							"	paramUsage.PARAMETER_CODE = paramValue.PARAMETER_CODE AND "+
							"	paramValue.PARAMETER_CODE = param.PARAMETER_CODE AND "+
							"   (paramValue.COORD_OP_CODE = {0})";
			//sqlQuery = String.Format(sqlQuery,coordOperationMethod, projectionConversionCode);
			sqlQuery = String.Format(sqlQuery, projectionConversionCode);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection, sqlQuery);

			ParameterList parameters = new ParameterList();
			
			while (reader.Read())
			{
				string paramNameLong =	reader[0].ToString();
				string paramNameShort = TranslateParamName(paramNameLong);
				string paramValueString = reader[1].ToString();
				double paramValue = 0.0;
				if (paramValueString!="" && paramValueString!=null)
				{
					paramValue = double.Parse(paramValueString);
				}
				string reverse =reader[2].ToString();

				// for some reason params, all params are held positive, and there is a flag to determine if params
				// are negative.
				if (reverse.ToLower()=="yes")
				{
					paramValue = paramValue * -1.0;
				}
				parameters.Add(paramNameShort, paramValue);

			}
			reader.Close();
			return parameters;
		}

		private static string TranslateParamName(string paramLongName)
		{
			return paramLongName.ToLower().Replace(" ","_");
			/*
			switch (paramLongName)
			{
				case "Latitude of natural origin":
					paramShortName="latitude_of_origin";
					break;
				//case "Longitude of natural origin":
				case "Scale factor at natural origin":
					paramShortName="scale_factor";
					break;
				case "False easting":
					paramShortName="false_easting";
					break;
				case "False northing":
					paramShortName="false_northing";
					break;
				default:
					break;
			}
			return paramShortName;*/
		}

		/// <summary>
		/// Gets the prime meridian code for the specified datum.
		/// </summary>
		/// <param name="code">The ESP code.</param>
		/// <returns>String with the EPSG code for the prime meridian.</returns>
		private string CreatePrimeMeridianCodeFromDatum(string code)
		{
			string sqlQuery="SELECT PRIME_MERIDIAN_CODE "+
							"FROM  Datum "+
							"WHERE DATUM_CODE ={0}";
			sqlQuery = String.Format(sqlQuery,code);
			IDataReader reader = Database.ExecuteQuery(_databaseConnection,sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("A Datum with a code of {0} could not be found.",code));
			}
		
			string primeMeridianCode=reader["PRIME_MERIDIAN_CODE"].ToString();
			Database.CheckOneRow(reader,code,"Datum");
			return primeMeridianCode;
		}
		
		private string GetMethodOperationCodeFromProjectionCode(string code)
		{
			string sqlQuery = "SELECT TOP 1 COORD_OP_CODE, COORD_OP_METHOD_CODE "+
				"FROM [Coordinate_Operation Parameter Value] "+
				"WHERE COORD_OP_CODE={0}";
			sqlQuery = String.Format(sqlQuery,code);

			IDataReader reader = Database.ExecuteQuery(_databaseConnection,sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("A coordinate operation with a code of {0} could not be found.",code));
			}
			string methodOp = reader["COORD_OP_METHOD_CODE"].ToString();
			Database.CheckOneRow(reader,code,"COORD_OP_CODE");
			return methodOp;

		}

		private IProjection CreateProjection(string code, string projectionCode)
		{
			string sqlQuery = "SELECT COORD_OP_METHOD_NAME, REVERSE_OP, REMARKS "+
							"FROM [Coordinate_Operation Method] "+
							"WHERE COORD_OP_METHOD_CODE={0}";
			sqlQuery = String.Format(sqlQuery,code);

			IDataReader reader = Database.ExecuteQuery(_databaseConnection,sqlQuery);
			if (!reader.Read())
			{
				throw new ArgumentException(String.Format("A coordinate operation with a code of {0} could not be found.",code));
			}
			string name = reader["COORD_OP_METHOD_NAME"].ToString();
			string remarks = reader["REMARKS"].ToString();

			Database.CheckOneRow(reader,code,"Datum");
			ProjectionParameter[] projectionParameters = this.GetProjectionParameterInfo(projectionCode,"ignore");
			Projection projection = new Projection(name,projectionParameters,name,remarks,"EPSG",code);
			return projection;
		}
		#endregion

	}
}
