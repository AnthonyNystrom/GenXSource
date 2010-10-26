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

#region Using

using System;
using System.IO;
using Geotools.CoordinateReferenceSystems;
using Geotools.CoordinateTransformations;
using Geotools.Utilities;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Creates an object based on the supplied Well Known Text (WKT).
	/// </summary>
	public class CoordinateSystemWktReader
	{
		/// <summary>
		/// Creates the appropriate object given a string containing XML.
		/// </summary>
		/// <param name="wkt">String containing XML.</param>
		/// <returns>Object representation of the XML.</returns>
		/// <exception cref="ParseException">If a token is not recognised.</exception>
		public static object Create(string wkt)
		{
			object returnObject = null;
			StringReader reader = new StringReader(wkt);
			WktStreamTokenizer tokenizer = new WktStreamTokenizer(reader);
			tokenizer.NextToken();
			string objectName=tokenizer.GetStringValue();
			switch (objectName)
			{
				case "UNIT":
					IUnit unit = ReadUnit(tokenizer);
					returnObject = unit;
					break;
				case "VERT_DATUM":
					IVerticalDatum verticalDatum = ReadVerticalDatum(tokenizer);
					returnObject = verticalDatum;
					break;
				case "SPHEROID":
					IEllipsoid ellipsoid = ReadEllipsoid(tokenizer);
					returnObject = ellipsoid;
					break;
				case "TOWGS84":
					WGS84ConversionInfo wgsInfo = ReadWGS84ConversionInfo(tokenizer);
					returnObject = wgsInfo;
					break;
				case "DATUM":
					IHorizontalDatum horizontalDatum = ReadHorizontalDatum(tokenizer);
					returnObject = horizontalDatum;
					break;
				case "PRIMEM":
					IPrimeMeridian primeMeridian = ReadPrimeMeridian(tokenizer);
					returnObject = primeMeridian;
					break;
				case "VERT_CS":
					IVerticalCoordinateSystem verticalCS = ReadVerticalCoordinateSystem(tokenizer);
					returnObject = verticalCS;
					break;
				case "GEOGCS":
					IGeographicCoordinateSystem geographicCS = ReadGeographicCoordinateSystem(tokenizer);
					returnObject = geographicCS;
					break;
				case "PROJCS":
					IProjectedCoordinateSystem projectedCS = ReadProjectedCoordinateSystem(tokenizer);
					returnObject = projectedCS;
					break;
				case "COMPD_CS":
					ICompoundCoordinateSystem compoundCS = ReadCompoundCoordinateSystem(tokenizer);
					returnObject = compoundCS;
					break;
				case "GEOCCS":
				case "FITTED_CS":
				case "LOCAL_CS":
					throw new NotSupportedException(String.Format("{0} is not implemented.",objectName));
				default:
					throw new ParseException(String.Format("'{0'} is not recongnized.",objectName));

			}
			reader.Close();
			return returnObject;
		}

		
		#region Methods
		/// <summary>
		/// Returns a IUnit given a piece of WKT.
		/// </summary>
		/// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
		/// <returns>An object that implements the IUnit interface.</returns>
		private static IUnit ReadUnit(WktStreamTokenizer tokenizer)
		{
			//UNIT["degree",0.01745329251994433,AUTHORITY["EPSG","9102"]]
			IUnit unit=null;
			tokenizer.ReadToken("[");
			string unitName=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.NextToken();
			double unitsPerUnit = tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			tokenizer.ReadToken("]");
			switch (unitName)
			{
					// take into account the different spellings of the word meter/metre.
				case "meter":
				case "metre":
					unit = new LinearUnit(unitsPerUnit,"",authority,authorityCode,unitName,"","");
					break;
				case "degree":
				case "radian":
					unit = new AngularUnit(unitsPerUnit,"",authority,authorityCode,unitName,"","");
					break;
				default:
					throw new NotImplementedException(String.Format("{0} is not recognized a unit of measure.",unitName));
			}
			return unit;
		}

		private static ICoordinateSystem ReadCoordinateSystem(string coordinateSystem, WktStreamTokenizer tokenizer)
		{
			ICoordinateSystem returnCS=null;
			switch (coordinateSystem)
			{
				case "VERT_CS":
					IVerticalCoordinateSystem verticalCS = ReadVerticalCoordinateSystem(tokenizer);
					returnCS = verticalCS;
					break;
				case "GEOGCS":
					IGeographicCoordinateSystem geographicCS = ReadGeographicCoordinateSystem(tokenizer);
					returnCS = geographicCS;
					break;
				case "PROJCS":
					IProjectedCoordinateSystem projectedCS = ReadProjectedCoordinateSystem(tokenizer);
					returnCS = projectedCS;
					break;
				case "COMPD_CS":
					ICompoundCoordinateSystem compoundCS = ReadCompoundCoordinateSystem(tokenizer);
					returnCS = compoundCS;
					break;
				case "GEOCCS":
				case "FITTED_CS":
				case "LOCAL_CS":
					throw new InvalidOperationException(String.Format("{0} coordinate system is not recongized.",coordinateSystem));
			}
			return returnCS;
		}
		


		private static WGS84ConversionInfo ReadWGS84ConversionInfo(WktStreamTokenizer tokenizer)
		{
			//TOWGS84[0,0,0,0,0,0,0]
			tokenizer.ReadToken("[");
			WGS84ConversionInfo info = new WGS84ConversionInfo();
			tokenizer.NextToken();
			info.Dx=tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");

			tokenizer.NextToken();
			info.Dy=tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");

			tokenizer.NextToken();
			info.Dz=tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");

			tokenizer.NextToken();
			info.Ex=tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");

			tokenizer.NextToken();
			info.Ey=tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");

			tokenizer.NextToken();
			info.Ez=tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");

			tokenizer.NextToken();
			info.Ppm=tokenizer.GetNumericValue();

			tokenizer.ReadToken("]");
			return info;
		}
		private static ICompoundCoordinateSystem ReadCompoundCoordinateSystem(WktStreamTokenizer tokenizer)
		{
			/*
			COMPD_CS[
			"OSGB36 / British National Grid + ODN",
			PROJCS[]
			VERT_CS[]
			AUTHORITY["EPSG","7405"]
			]*/

			//TODO add a ReadCoordinateSystem - that determines the correct coordinate system to 
			//read. Right now this hard coded for a projected and a vertical coord sys - so the UK
			//national grid works.
			tokenizer.ReadToken("[");
			string name=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.NextToken();
			string headCSCode =  tokenizer.GetStringValue();
			ICoordinateSystem headCS = ReadCoordinateSystem(headCSCode,tokenizer);
			tokenizer.ReadToken(",");
			tokenizer.NextToken();
			string tailCSCode =  tokenizer.GetStringValue();
			ICoordinateSystem tailCS = ReadCoordinateSystem(tailCSCode,tokenizer);
			tokenizer.ReadToken(",");
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			tokenizer.ReadToken("]");
			ICompoundCoordinateSystem compoundCS = new CompoundCoordinateSystem(headCS,tailCS,"",authority,authorityCode,name,"",""); 
			return compoundCS;
			
		}
		
		
		private static IEllipsoid ReadEllipsoid(WktStreamTokenizer tokenizer)
		{
			//SPHEROID["Airy 1830",6377563.396,299.3249646,AUTHORITY["EPSG","7001"]]
			tokenizer.ReadToken("[");
			string name=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.NextToken();
			double majorAxis = tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");
			tokenizer.NextToken();
			double e = tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");

			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			tokenizer.ReadToken("]");
			IEllipsoid ellipsoid = new Ellipsoid(majorAxis,0.0,e,true,LinearUnit.Meters,"",authority,authorityCode,name,"","");
			return ellipsoid;
		}

		private static IProjection ReadProjection(WktStreamTokenizer tokenizer)
		{
			//tokenizer.NextToken();// PROJECTION
			tokenizer.ReadToken("PROJECTION");
			tokenizer.ReadToken("[");//[
			string projectionName=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken("]");//]
			tokenizer.ReadToken(",");//,
			tokenizer.ReadToken("PARAMETER");
			ParameterList paramList = new ParameterList();
			while (tokenizer.GetStringValue()=="PARAMETER")
			{
				tokenizer.ReadToken("[");
				string paramName = tokenizer.ReadDoubleQuotedWord();
				tokenizer.ReadToken(",");
				tokenizer.NextToken();
				double paramValue = tokenizer.GetNumericValue();
				tokenizer.ReadToken("]");
				tokenizer.ReadToken(",");
				paramList.Add(paramName,paramValue);
				tokenizer.NextToken();
			}
			
			ProjectionParameter[] paramArray = new ProjectionParameter[paramList.Count];
			int i=0;
			foreach(string key in paramList.Keys)
			{
				ProjectionParameter param= new ProjectionParameter();
				param.Name=key;
				param.Value=(double)paramList[key];
				paramArray[i]=param;
				i++;
			}
			string authority="";
			string authorityCode=""; 
			IProjection projection = new Projection(projectionName, paramArray,"", "",authority, authorityCode);
			return projection;
		}
		private static IProjectedCoordinateSystem ReadProjectedCoordinateSystem(WktStreamTokenizer tokenizer)
		{
				/*			PROJCS[
					"OSGB 1936 / British National Grid",
					GEOGCS[
						"OSGB 1936",
						DATUM[...]
						PRIMEM[...]
						AXIS["Geodetic latitude","NORTH"]
						AXIS["Geodetic longitude","EAST"]
						AUTHORITY["EPSG","4277"]
					],
					PROJECTION["Transverse Mercator"],
					PARAMETER["latitude_of_natural_origin",49],
					PARAMETER["longitude_of_natural_origin",-2],
					PARAMETER["scale_factor_at_natural_origin",0.999601272],
					PARAMETER["false_easting",400000],
					PARAMETER["false_northing",-100000],
					AXIS["Easting","EAST"],
					AXIS["Northing","NORTH"],
					AUTHORITY["EPSG","27700"]
				]
				*/
			tokenizer.ReadToken("[");
			string name=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.ReadToken("GEOGCS");
			IGeographicCoordinateSystem geographicCS = ReadGeographicCoordinateSystem(tokenizer);
			tokenizer.ReadToken(",");
			IProjection projection = ReadProjection(tokenizer);
			IAxisInfo axis0 = ReadAxisInfo(tokenizer);
			tokenizer.ReadToken(",");
			tokenizer.ReadToken("AXIS");
			IAxisInfo axis1 = ReadAxisInfo(tokenizer);
			tokenizer.ReadToken(",");
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			tokenizer.ReadToken("]");
			IAxisInfo[] axisArray = new IAxisInfo[2];
			axisArray[0]=axis0;
			axisArray[1]=axis1;
			ILinearUnit linearUnit = LinearUnit.Meters;
			IProjectedCoordinateSystem projectedCS = new ProjectedCoordinateSystem(null,axisArray,geographicCS,linearUnit, projection,"",authority,authorityCode,name,"","");
			return projectedCS;
		}
		private static IGeographicCoordinateSystem ReadGeographicCoordinateSystem(WktStreamTokenizer tokenizer)
		{
			/*
			GEOGCS["OSGB 1936",
			DATUM["OSGB 1936",SPHEROID["Airy 1830",6377563.396,299.3249646,AUTHORITY["EPSG","7001"]]TOWGS84[0,0,0,0,0,0,0],AUTHORITY["EPSG","6277"]]
			PRIMEM["Greenwich",0,AUTHORITY["EPSG","8901"]]
			AXIS["Geodetic latitude","NORTH"]
			AXIS["Geodetic longitude","EAST"]
			AUTHORITY["EPSG","4277"]
			]
			*/
			
			tokenizer.ReadToken("[");
			string name=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.ReadToken("DATUM");
			IHorizontalDatum horizontalDatum = ReadHorizontalDatum(tokenizer);
			tokenizer.ReadToken("PRIMEM");
			IPrimeMeridian primeMeridian = ReadPrimeMeridian(tokenizer);
			tokenizer.ReadToken("AXIS");
			IAxisInfo axis0 = ReadAxisInfo(tokenizer);
			tokenizer.ReadToken(",");
			tokenizer.ReadToken("AXIS");
			IAxisInfo axis1 = ReadAxisInfo(tokenizer);
			tokenizer.ReadToken(",");
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			tokenizer.ReadToken("]");
			// ?? assume angular unit is degrees.
			IAngularUnit angularUnit = new AngularUnit(180/Math.PI);
			IGeographicCoordinateSystem geographicCS = new GeographicCoordinateSystem(angularUnit, horizontalDatum,
					primeMeridian,axis0, axis1,"",authority,authorityCode,name,"","");
			return geographicCS;
		}

		private static IAxisInfo ReadAxisInfo(WktStreamTokenizer tokenizer)
		{
			//AXIS["Geodetic longitude","EAST"]
			tokenizer.ReadToken("[");
			string name=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			string orientationString = tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken("]");
			AxisOrientation orientation =(AxisOrientation) Enum.Parse(typeof(AxisOrientation),orientationString,true);
			IAxisInfo axis = new AxisInfo(name, orientation);
			return axis;
		}
	
		private static IHorizontalDatum ReadHorizontalDatum(WktStreamTokenizer tokenizer)
		{
			//DATUM["OSGB 1936",SPHEROID["Airy 1830",6377563.396,299.3249646,AUTHORITY["EPSG","7001"]]TOWGS84[0,0,0,0,0,0,0],AUTHORITY["EPSG","6277"]]
		  
			tokenizer.ReadToken("[");
			string name=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.ReadToken("SPHEROID");
			IEllipsoid ellipsoid = ReadEllipsoid(tokenizer);
			tokenizer.ReadToken("TOWGS84");
			WGS84ConversionInfo wgsInfo = ReadWGS84ConversionInfo(tokenizer);
			tokenizer.ReadToken(",");
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			// make an assumption about the datum type.
			DatumType datumType = DatumType.IHD_Geocentric;
			IHorizontalDatum horizontalDatum = new HorizontalDatum(name,datumType,ellipsoid, wgsInfo,"",authority,authorityCode,"","");
			tokenizer.ReadToken("]");
			return horizontalDatum;
		}
		
		private static IPrimeMeridian ReadPrimeMeridian(WktStreamTokenizer tokenizer)
		{
			//PRIMEM["Greenwich",0,AUTHORITY["EPSG","8901"]]
			tokenizer.ReadToken("[");
			string name=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.NextToken();
			double longitude = tokenizer.GetNumericValue();
			tokenizer.ReadToken(",");
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			// make an assumption about the Angular units - degrees.
			IPrimeMeridian primeMeridian = new PrimeMeridian(name,new AngularUnit(180/Math.PI),longitude,"",authority,authorityCode,"","");
			tokenizer.ReadToken("]");
			return primeMeridian;
		}
		
		
		private static IVerticalCoordinateSystem ReadVerticalCoordinateSystem(WktStreamTokenizer tokenizer)
		{
			/*
			VERT_CS["Newlyn",
			VERT_DATUM["Ordnance Datum Newlyn",2005,AUTHORITY["EPSG","5101"]]
			UNIT["metre",1,AUTHORITY["EPSG","9001"]]
			AUTHORITY["EPSG","5701"]
			*/
			tokenizer.ReadToken("[");
			string name=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.ReadToken("VERT_DATUM");
			IVerticalDatum verticalDatum = ReadVerticalDatum(tokenizer);
			tokenizer.ReadToken("UNIT");
			IUnit unit = ReadUnit(tokenizer);
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			tokenizer.ReadToken("]");

			IVerticalCoordinateSystem verticalCS = new VerticalCoordinateSystem(name,verticalDatum,"",authority,authorityCode,"","");
			return verticalCS;
		}
		private static IVerticalDatum  ReadVerticalDatum(WktStreamTokenizer tokenizer)
		{
			//VERT_DATUM["Ordnance Datum Newlyn",2005,AUTHORITY["5101","EPSG"]]
			tokenizer.ReadToken("[");
			string datumName=tokenizer.ReadDoubleQuotedWord();
			tokenizer.ReadToken(",");
			tokenizer.NextToken();
			string datumTypeNumber = tokenizer.GetStringValue();
			tokenizer.ReadToken(",");
			string authority="";
			string authorityCode=""; 
			tokenizer.ReadAuthority(ref authority, ref authorityCode);
			DatumType datumType = (DatumType)Enum.Parse(typeof(DatumType),datumTypeNumber);
			IVerticalDatum verticalDatum = new VerticalDatum(datumType,"",authorityCode,authority,datumName,"","");
			tokenizer.ReadToken("]");
			return verticalDatum;
		}
		#endregion

		#region NotImplemented
		// since the related objects have not been implemented 
		private static IFittedCoordinateSystem ReadFittedCoordinateSystem(WktStreamTokenizer tokenizer)
		{
			throw new NotImplementedException("IFittedCoordinateSystem is not implemented.");
		}
		private static IGeocentricCoordinateSystem ReadGeocentricCoordinateSystem(WktStreamTokenizer tokenizer)
		{
			throw new NotImplementedException("IGeocentricCoordinateSystem is not implemented");
		}
		private static IHorizontalCoordinateSystem ReadHorizontalCoordinateSystem(WktStreamTokenizer tokenizer)
		{
			throw new NotImplementedException("IGeocentricCoordinateSystem is not implemented.");
		}
		private LocalCoordinateSystem ReadLocalCoordinateSystem(WktStreamTokenizer tokenizer)
		{
			throw new NotImplementedException();
		}
		private LocalDatum ReadLocalDatum(WktStreamTokenizer tokenizer)
		{
			throw new NotImplementedException();
		}
		
		#endregion
	}
}
