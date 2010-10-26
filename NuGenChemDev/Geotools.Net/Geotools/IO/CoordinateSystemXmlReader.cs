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
using System.Collections;
using System.Xml;
using System.IO;
using Geotools.CoordinateReferenceSystems;
 
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Creates an object based on the supplied XML.
	/// </summary>
	public class CoordinateSystemXmlReader
	{
		#region Public static methods
		/// <summary>
		/// Creates the appropriate object given a string containing XML.
		/// </summary>
		/// <param name="xml">String containing XML.</param>
		/// <returns>Object representation of the XML.</returns>
		/// <exception cref="ParseException">If a token is not recognised.</exception>
		public static object Create(string xml)
		{
			if (xml==null)
			{
				throw new ArgumentNullException("xml");
			}
			object returnObject = null;
			StringReader textReader = new StringReader(xml);
			XmlTextReader reader = new XmlTextReader(textReader);
			reader.WhitespaceHandling=WhitespaceHandling.None;
			
			returnObject = Create(reader);
			return returnObject;
		}
		public static object Create(XmlTextReader reader)
		{
			if (reader==null)
			{
				throw new ArgumentNullException("reader");
			}
			// we don't want to handle whitespace
			reader.WhitespaceHandling=WhitespaceHandling.None;
			object returnObject = null;
			reader.Read();
			

			// skip declarions and comments.
			while (reader.NodeType != XmlNodeType.Element)
			{
				reader.Read();
			}
		
			if (reader.NodeType==XmlNodeType.Element)
			{
				switch(reader.Name)
				{
					case "CS_LinearUnit":
						ILinearUnit linearUnit = ReadLinearUnit( reader );
						returnObject = linearUnit;
						break;
					case "CS_AngularUnit":
						IAngularUnit angularUnit = ReadAngularUnit( reader );
						returnObject = angularUnit;
						break;
					case "CS_VerticalDatum":
						IVerticalDatum verticalDatum = ReadVerticalDatum(reader);
						returnObject = verticalDatum;
						break;
					case "CS_Ellipsoid":
						IEllipsoid ellipsoid = ReadEllipsoid(reader);
						returnObject = ellipsoid;
						break;
					case "CS_WGS84ConversionInfo":
						WGS84ConversionInfo wgsInfo = ReadWGS84ConversionInfo(reader);
						returnObject = wgsInfo;
						break;
					case "CS_HorizontalDatum":
						IHorizontalDatum horizontalDatum = ReadHorizontalDatum(reader);
						returnObject = horizontalDatum;
						break;
					case "CS_PrimeMeridian":
						IPrimeMeridian primeMeridian = ReadPrimeMeridian(reader);
						returnObject = primeMeridian;
						break;
					case "CS_VerticalCoordinateSystem":
						IVerticalCoordinateSystem verticalCS = ReadVerticalCoordinateSystem(reader);
						returnObject = verticalCS;
						break;
					case "CS_GeographicCoordinateSystem":
						IGeographicCoordinateSystem geographicCS = ReadGeographicCoordinateSystem(reader);
						returnObject = geographicCS;
						break;
					case "CS_ProjectedCoordinateSystem":
						IProjectedCoordinateSystem projectedCS = ReadProjectedCoordinateSystem(reader);
						returnObject = projectedCS;
						break;
					case "CS_CompoundCoordinateSystem":
						ICompoundCoordinateSystem compoundCS = ReadCompoundCoordinateSystem(reader);
						returnObject = compoundCS;
						break;
					case "CS_Projection":
						IProjection projection = ReadProjection(reader);
						returnObject = projection;
						break;
					case "CS_CoordinateSystem":
						// must be a compound coord sys since all other coord system should have been 
						// taken care of by now.
						reader.Read();
						ICoordinateSystem coordinateSystem = ReadCompoundCoordinateSystem( reader );
						reader.Read();
						returnObject = coordinateSystem;
						break;
					case "CS_GEOCCS":
					case "CS_FITTED_CS":
					case "CS_LOCAL_CS":
						throw new NotSupportedException(String.Format("{0} is not implemented.",reader.Name));
					default:
						throw new ParseException(String.Format("Element type {0} was is not understoon.",reader.Name));

				}
			}
			
			return returnObject;
			
		}
		private static ICoordinateSystem ReadCoordinateSystem( XmlTextReader reader)
		{
			//reader.Read();
			ICoordinateSystem returnCS=null;
			switch (reader.Name)
			{
				case "CS_VerticalCoordinateSystem":
					IVerticalCoordinateSystem verticalCS = ReadVerticalCoordinateSystem(reader);
					returnCS = verticalCS;
					break;
				case "CS_GeographicCoordinateSystem":
					IGeographicCoordinateSystem geographicCS = ReadGeographicCoordinateSystem(reader);
					returnCS = geographicCS;
					break;
				case "CS_ProjectedCoordinateSystem":
					IProjectedCoordinateSystem projectedCS = ReadProjectedCoordinateSystem(reader);
					returnCS = projectedCS;
					break;
				case "CS_CompoundCoordinateSystem":
					ICompoundCoordinateSystem compoundCS = ReadCompoundCoordinateSystem(reader);
					returnCS = compoundCS;
					break;
				case "GEOCCS":
				case "FITTED_CS":
				case "LOCAL_CS":
					throw new InvalidOperationException(String.Format("{0} coordinate system is not recongized.",reader.Name));
				//default:
				//	throw new ParseException(String.Format("Coordinate System {0} was not understoon.",reader.Name));

			}
			//reader.Read();
			return returnCS;
		}
		

		public static void ReadInfo(XmlTextReader reader, ref string  authority, ref string  authorityCode, ref string  abbreviation, ref string name)
		{
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_Info"))
			{
				throw new ParseException(String.Format("Expected a IInfo but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}	
			authority = reader.GetAttribute("Authority");
			authorityCode = reader.GetAttribute("AuthorityCode");
			abbreviation = reader.GetAttribute("Abbreviation");
			name = reader.GetAttribute("Name");

			//GetAttribute - returns null if the attribute is not found, so ensure
			//string is empty.
			if (name==null)
			{
				name="";
			}
			if (abbreviation==null)
			{
				abbreviation="";
			}
			if (authorityCode==null)
			{
				authorityCode="";
			}
			if (authority==null)
			{
				authority="";
			}
			reader.Read();
		}

		public static ILinearUnit ReadLinearUnit(XmlTextReader reader)
		{
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_LinearUnit"))
			{
				throw new ParseException(String.Format("Expected a CS_LinearUnit but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}

			double metersPerUnit = XmlConvert.ToDouble(reader.GetAttribute("MetersPerUnit"));
			string authority="",authorityCode="",abbreviation="",name="";
			/*while (reader.NodeType != XmlNodeType.EndElement)
			{
				XmlNodeType nodetype = reader.NodeType;
				string nodeValue = reader.Value;
				string nodeName = reader.Name;
				reader.Read();
			}
		
			while (reader.NodeType != XmlNodeType.Element)
			{
				reader.Read();
			}*/
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			reader.Read();
			LinearUnit linearUnit = new LinearUnit(metersPerUnit,"",authority,authorityCode,name,"",abbreviation);
			return linearUnit;
		}
		public static IAngularUnit ReadAngularUnit(XmlTextReader reader)
		{
			if (!(reader.NodeType==XmlNodeType.Element && reader.Name=="CS_AngularUnit"))
			{
				throw new ParseException(String.Format("Expected a ILinearUnit but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
			double radiansPerUnit = XmlConvert.ToDouble(reader.GetAttribute("RadiansPerUnit"));
			string authority="",authorityCode="",abbreviation="",name="";
			
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			reader.Read();
			AngularUnit AngularUnit = new AngularUnit(radiansPerUnit,"",authority,authorityCode,name,"",abbreviation);
			return AngularUnit;
		}
		#endregion



		private static WGS84ConversionInfo ReadWGS84ConversionInfo(XmlTextReader reader)
		{
			double dx = XmlConvert.ToDouble(reader.GetAttribute("Dx"));
			double dy = XmlConvert.ToDouble(reader.GetAttribute("Dy"));
			double dz = XmlConvert.ToDouble(reader.GetAttribute("Dz"));
			double ex = XmlConvert.ToDouble(reader.GetAttribute("Ex"));
			double ey = XmlConvert.ToDouble(reader.GetAttribute("Ey"));
			double ez = XmlConvert.ToDouble(reader.GetAttribute("Ez"));
			double ppm = XmlConvert.ToDouble(reader.GetAttribute("Ppm"));
			WGS84ConversionInfo wgs84Info = new WGS84ConversionInfo();
			wgs84Info.Dx=dx;
			wgs84Info.Dy=dy;
			wgs84Info.Dz=dz;
			wgs84Info.Ex=ex;
			wgs84Info.Ey=ey;
			wgs84Info.Ez=ez;
			wgs84Info.Ppm=ppm;

			reader.Read();
			return wgs84Info;

		}
		private static ICompoundCoordinateSystem ReadCompoundCoordinateSystem(XmlTextReader reader)
		{
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_CompoundCoordinateSystem"))
			{
				throw new ParseException(String.Format("Expected a ICompoundCoordinateSystem but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
			string authority="",authorityCode="",abbreviation="",name="";
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);

			while (reader.NodeType==XmlNodeType.Element && reader.Name=="CS_AxisInfo")
			{
				//IAxisInfo axis = ReadAxisInfo( reader );
				//list.Add(axis);
				reader.Read();
			}
			reader.Read();
			ICoordinateSystem headCS = ReadCoordinateSystem( reader );
			reader.Read();
			reader.Read();
			reader.Read();
			ICoordinateSystem tailCS = ReadCoordinateSystem( reader );
			reader.Read();
			CompoundCoordinateSystem compoundCS = new CompoundCoordinateSystem(headCS,tailCS,"",authority, authorityCode, name, "", abbreviation);
			return compoundCS;
			
		}
		
		
		private static IEllipsoid ReadEllipsoid(XmlTextReader reader)
		{
			/*
			 * <IEllipsoid SemiMajorAxis="6377563.396" SemiMinorAxis="6356256.90923729" InverseFlattening="299.3249646" IvfDefinitive="1">
                            <IInfo AuthorityCode="7001" Authority="EPSG" Name="Airy 1830"/>
                            <ILinearUnit MetersPerUnit="1">
                                <IInfo AuthorityCode="9001" Abbreviation="m" Authority="EPSG" Name="metre"/>
                            </ILinearUnit>
                        </IEllipsoid>
						*/
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_Ellipsoid"))
			{
				throw new ParseException(String.Format("Expected a CS_Ellipsoid but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
			double semiMajor = XmlConvert.ToDouble(reader.GetAttribute("SemiMajorAxis"));
			double semiMinor = XmlConvert.ToDouble(reader.GetAttribute("SemiMinorAxis"));
			double inverseFlattening = XmlConvert.ToDouble(reader.GetAttribute("InverseFlattening"));
			bool ivfDefinitive=false;
			if (reader.GetAttribute("IvfDefinitive")=="1")
			{
				ivfDefinitive=true;
			}
			string authority="",authorityCode="",abbreviation="",name="";
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			//reader.Read();
			ILinearUnit linearUnit = ReadLinearUnit( reader );
			reader.Read();
			Ellipsoid ellipsoid = new Ellipsoid(semiMajor, semiMinor, inverseFlattening,ivfDefinitive,
												linearUnit,"",authority,authorityCode,name,"",abbreviation);
			return ellipsoid;
		}

		private static ProjectionParameter ReadProjectionParameter(XmlTextReader reader)
		{
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_ProjectionParameter"))
			{
				throw new ParseException(String.Format("Expected a IProjectionParameter but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
			string name = reader.GetAttribute("Name");
			string val = reader.GetAttribute("Value");
			ProjectionParameter param = new ProjectionParameter();
			param.Name=name;
			param.Value=Double.Parse( val );
			return param;
		}

		private static IProjection ReadProjection(XmlTextReader reader)
		{
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_Projection"))
			{
				throw new ParseException(String.Format("Expected a IProjection but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
			string className=reader.GetAttribute("ClassName");
			reader.Read();
			string authority="",authorityCode="",abbreviation="",name="";
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			ArrayList list = new ArrayList();
			while (reader.NodeType==XmlNodeType.Element && reader.Name=="CS_ProjectionParameter")
			{
				ProjectionParameter param = ReadProjectionParameter( reader );
				list.Add(param);
				reader.Read();
			}
			ProjectionParameter[] projectionParams = new ProjectionParameter[list.Count];
			projectionParams = (ProjectionParameter[])list.ToArray(typeof(ProjectionParameter));
			
			Projection projection = new Projection(name,projectionParams,className,"",authority,authorityCode);
			return projection;
			
		}
		private static IProjectedCoordinateSystem ReadProjectedCoordinateSystem(XmlTextReader reader)
		{
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_ProjectedCoordinateSystem"))
			{
				throw new ParseException(String.Format("Expected a IProjectedCoordinateSystem but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
			string authority="",authorityCode="",abbreviation="",name="";
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			ArrayList list = new ArrayList();
			while (reader.NodeType==XmlNodeType.Element && reader.Name=="CS_AxisInfo")
			{
				IAxisInfo axis = ReadAxisInfo( reader );
				list.Add(axis);
				reader.Read();
			}
			IAxisInfo[] axisInfos = new IAxisInfo[list.Count];
			axisInfos = (IAxisInfo[])list.ToArray(typeof(IAxisInfo));
			IGeographicCoordinateSystem geographicCoordinateSystem = ReadGeographicCoordinateSystem( reader );
			ILinearUnit linearUnit = ReadLinearUnit( reader );
			IProjection projection = ReadProjection( reader );
			reader.Read();
			//IPrimeMeridian primeMeridian = null;
			IHorizontalDatum horizontalDatum = null;
			ProjectedCoordinateSystem projectedCS = new ProjectedCoordinateSystem(horizontalDatum,
				axisInfos,geographicCoordinateSystem,linearUnit, projection,"",authority,authorityCode,
				name,"",abbreviation);
			return projectedCS;
		}
		private static IGeographicCoordinateSystem ReadGeographicCoordinateSystem(XmlTextReader reader)
		{
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_GeographicCoordinateSystem"))
			{
				throw new ParseException(String.Format("Expected a IGeographicCoordinateSystem but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
			string authority="",authorityCode="",abbreviation="",name="";
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			//reader.Read();
			ArrayList list = new ArrayList();
			while (reader.NodeType==XmlNodeType.Element && reader.Name=="CS_AxisInfo")
			{
				IAxisInfo axis = ReadAxisInfo( reader );
				list.Add(axis);
				reader.Read();
			}
			IAxisInfo[] axisInfos = new IAxisInfo[list.Count];
			axisInfos = (IAxisInfo[])list.ToArray(typeof(IAxisInfo));
			IHorizontalDatum horizontalDatum = ReadHorizontalDatum( reader );
			//reader.Read();
			IAngularUnit angularUnit = ReadAngularUnit( reader );
			//reader.Read();
			IPrimeMeridian primeMeridian = ReadPrimeMeridian( reader );
		
			reader.Read();
			IGeographicCoordinateSystem geographicCoordinateSystem = new GeographicCoordinateSystem(angularUnit,horizontalDatum,
				primeMeridian, axisInfos[0], axisInfos[1],"",
				authority,authorityCode,name,"",abbreviation);
			return geographicCoordinateSystem;
		}

		private static IAxisInfo ReadAxisInfo(XmlTextReader reader)
		{
			 //<IAxisInfo Name="Up" Orientation="UP"/>

			if (!(reader.NodeType==XmlNodeType.Element ||  reader.Name=="CS_Info"))
			{
				throw new ParseException(String.Format("Expected a IInfo but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
			
			string orientationString = reader.GetAttribute("Orientation");
			AxisOrientation orientation =(AxisOrientation) Enum.Parse(typeof(AxisOrientation),orientationString,true);
			IAxisInfo axis = new AxisInfo(reader.GetAttribute("Name"),orientation);
			return axis;
		}
	
		private static IHorizontalDatum ReadHorizontalDatum(XmlTextReader reader)
		{
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_HorizontalDatum"))
			{
				throw new ParseException(String.Format("Expected a IHorizontalDatum but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}
		
			/* <IHorizontalDatum DatumType="1001">
				<IInfo AuthorityCode="6277" Authority="EPSG" Name="OSGB_1936"/>
				<IEllipsoid SemiMajorAxis="6377563.396" SemiMinorAxis="6356256.90923729" InverseFlattening="299.3249646" IvfDefinitive="1">
					<IInfo AuthorityCode="7001" Authority="EPSG" Name="Airy 1830"/>
					<ILinearUnit MetersPerUnit="1">
						<IInfo AuthorityCode="9001" Abbreviation="m" Authority="EPSG" Name="metre"/>
					</ILinearUnit>
				</IEllipsoid>
				<IWGS84ConversionInfo Dx="375" Dy="-111" Dz="431" Ex="0" Ey="0" Ez="0" Ppm="0"/>
			</IHorizontalDatum>
					*/
			string datumTypeString = reader.GetAttribute("DatumType");
			DatumType datumType = (DatumType)Enum.Parse(typeof(DatumType),datumTypeString,true);
			string authority="",authorityCode="",abbreviation="",name="";
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			IEllipsoid ellipsoid = ReadEllipsoid( reader );
			WGS84ConversionInfo wgs84info = ReadWGS84ConversionInfo( reader );
			reader.Read();
			HorizontalDatum horizontalDatum = new HorizontalDatum(name,datumType,ellipsoid, wgs84info,"",authority,authorityCode,"",abbreviation);
			return horizontalDatum;
		}
		
		private static IPrimeMeridian ReadPrimeMeridian(XmlTextReader reader)
		{
			/*
			 * <?xml version="1.0"?>
<IPrimeMeridian Longitude="0">
						<IInfo AuthorityCode="8901" Authority="EPSG" Name="Greenwich"/>
						<IAngularUnit RadiansPerUnit="1.74532925199433E-02">
							<IInfo AuthorityCode="9110" Authority="EPSG" Name="DDD.MMSSsss"/>
						</IAngularUnit>
					</IPrimeMeridian>
					*/
			double longitude = XmlConvert.ToDouble(reader.GetAttribute("Longitude"));
			string authority="",authorityCode="",abbreviation="",name="";
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			IAngularUnit angularUnit = ReadAngularUnit( reader );
			reader.Read();
			IPrimeMeridian primeMeridian = new PrimeMeridian(name, angularUnit,longitude,"",authority,authorityCode,"",abbreviation);
					
			return primeMeridian;
		}
		
		
		private static IVerticalCoordinateSystem ReadVerticalCoordinateSystem(XmlTextReader reader)
		{
			/*
			 * <?xml version="1.0"?>
				<IVerticalCoordinateSystem>
                <IInfo AuthorityCode="5701" Abbreviation="ODN" Authority="EPSG" Name="Newlyn"/>
                <IAxisInfo Name="Up" Orientation="UP"/>
                <IVerticalDatum DatumType="2005">
                    <IInfo AuthorityCode="5101" Abbreviation="ODN" Authority="EPSG" Name="Ordnance Datum Newlyn"/>
                </IVerticalDatum>
                <ILinearUnit MetersPerUnit="1">
                    <IInfo AuthorityCode="9001" Abbreviation="m" Authority="EPSG" Name="metre"/>
                </ILinearUnit>
            </IVerticalCoordinateSystem>
			*/
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_VerticalCoordinateSystem"))
			{
				throw new ParseException(String.Format("Expected a IVerticalCoordinateSystem but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}	
			string authority="",authorityCode="",abbreviation="",name="";
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			//reader.Read();
			ArrayList list = new ArrayList();
			while (reader.NodeType==XmlNodeType.Element && reader.Name=="CS_AxisInfo")
			{
				IAxisInfo axis = ReadAxisInfo( reader );
				list.Add(axis);
				reader.Read();
			}
			IAxisInfo[] axisInfos = new IAxisInfo[list.Count];
			axisInfos = (IAxisInfo[])list.ToArray(typeof(IAxisInfo));
			IVerticalDatum verticalDatum = ReadVerticalDatum( reader );
			ILinearUnit linearUnit = ReadLinearUnit( reader );
			reader.Read();
			reader.Read();
			VerticalCoordinateSystem verticalCoordinateSystem = new VerticalCoordinateSystem(name, verticalDatum, axisInfos[0],linearUnit, "", authority, authorityCode,"",abbreviation);
			return verticalCoordinateSystem;
		}
		private static IVerticalDatum  ReadVerticalDatum(XmlTextReader reader)
		{
		/*
		 *		<?xml version="1.0"?>
				<IVerticalDatum DatumType="2005">
                    <IInfo AuthorityCode="5101" Abbreviation="ODN" Authority="EPSG" Name="Ordnance Datum Newlyn"/>
                </IVerticalDatum>
		 */
			if (!(reader.NodeType==XmlNodeType.Element &&  reader.Name=="CS_VerticalDatum"))
			{
				throw new ParseException(String.Format("Expected a IVerticalDatum but got a {0} at line {1} col {2}",reader.Name,reader.LineNumber,reader.LinePosition));
			}

			string datumTypeString = reader.GetAttribute("DatumType");
			DatumType datumType = (DatumType)Enum.Parse(typeof(DatumType),datumTypeString,true);
			string authority="",authorityCode="",abbreviation="",name="";
			reader.Read();
			ReadInfo(reader, ref authority,ref authorityCode, ref abbreviation, ref name);
			reader.Read();
			IVerticalDatum verticalDatum = new VerticalDatum(datumType, "",authorityCode,authority,name, "",abbreviation);
			return verticalDatum;
		}


		#region NotImplemented
		// since the related objects have not been implemented 
	
		private LocalCoordinateSystem ReadLocalCoordinateSystem(XmlTextReader reader)
		{
			throw new NotImplementedException();
		}
			private LocalDatum ReadLocalDatum(XmlTextReader reader)
		{
			throw new NotImplementedException();
		}
		private static IFittedCoordinateSystem ReadFittedCoordinateSystem(XmlTextReader reader)
		{
			throw new NotImplementedException("CS_FittedCoordinateSystem is not implemented.");
		}
		private static IGeocentricCoordinateSystem ReadGeocentricCoordinateSystem(XmlTextReader reader)
		{
			throw new NotImplementedException("CS_GeocentricCoordinateSystem is not implemented");
		}
		private static IHorizontalCoordinateSystem ReadHorizontalCoordinateSystem(XmlTextReader reader)
		{
			throw new NotImplementedException("CS_GeocentricCoordinateSystem is not implemented.");
		}
		#endregion
	
	

	}
}
