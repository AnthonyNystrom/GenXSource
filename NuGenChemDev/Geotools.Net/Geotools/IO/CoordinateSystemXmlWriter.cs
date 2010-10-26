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
using System.Xml;
using Geotools.CoordinateReferenceSystems;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Writes a coordinate system object out as XML.
	/// </summary>
	public class CoordinateSystemXmlWriter
	{
		#region Public Static Methods

		/// <summary>
		/// Converts a given coordinate system object to a XML string.
		/// </summary>
		/// <param name="obj">The coordinate system object to convert.</param>
		/// <returns>A string containing WKT.</returns>
		public static string Write(object obj)
		{
			TextWriter textWriter = new StringWriter();
			XmlTextWriter xmlWriter = new XmlTextWriter(textWriter);
			xmlWriter.Formatting = Formatting.Indented;
			// important to use double quotes here - otherwise the XML compare fails.
			xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\"");
			Write(obj, xmlWriter);
			xmlWriter.Close();
			textWriter.Close();
			return textWriter.ToString();
		}
		/// <summary>
		/// Converts a given coordinate system object to a IndentedTextWriter.
		/// </summary>
		/// <param name="obj">The coordinate system to convert.</param>
		/// <param name="writer"></param>
		/// <remarks>
		/// <list type="bullet">
		/// <listheader><term>Items</term><description>Descriptions</description></listheader>
		/// <item><term>ICoordinateSystem</term><description>Your Description</description></item>
		/// <item><term>IDatum</term><description>Your Description</description></item>
		/// <item><term>IEllipsoid</term><description>Your Description</description></item>
		/// <item><term>IAxisInfo</term><description>Your Description</description></item>
		/// <item><term>IWGS84ConversionInfo</term><description>Your Description</description></item>
		/// <item><term>IUnit</term><description>Your Description</description></item>
		/// <item><term>IPrimeMeridian</term><description>Your Description</description></item>
		/// <item><term>ICompoundCoordinateSystem</term><description>Your Description</description></item>
		/// <item><term>IGeographicCoordinateSystem</term><description>Your Description</description></item>
		/// <item><term>IProjectedCoordinateSystem</term><description>Your Description</description></item>
		/// <item><term>IVerticalCoordinateSystem</term><description>Your Description</description></item>
		/// </list>
		/// </remarks>
		public static void Write(object obj, XmlTextWriter writer)
		{
			
			if (obj is ICoordinateSystem)
			{
				WriteCoordinateSystem(obj as ICoordinateSystem, writer);
			}
			else if (obj is IDatum)
			{
				WriteDatum(obj as IDatum, writer);
			}
			else if (obj is IEllipsoid)
			{
				WriteEllipsoid(obj as IEllipsoid, writer);
			}
			else if (obj is IAxisInfo)
			{
				IAxisInfo info = (IAxisInfo)obj;
				WriteAxis(info, writer);
			}
			else if (obj is WGS84ConversionInfo)
			{
				WGS84ConversionInfo info = (WGS84ConversionInfo)obj;
				WriteWGS84ConversionInfo(info, writer);
			}
			else if (obj is IUnit)
			{
				WriteUnit(obj as IUnit, writer);
			}
			else if (obj is IPrimeMeridian)
			{
				WritePrimeMeridian(obj as IPrimeMeridian, writer);
			}
			else if (obj is IProjection)
			{
				WriteProjection(obj as IProjection, writer);
			}
			else
			{
				throw new NotImplementedException(String.Format("Cannot convert {0} to XML.",obj.GetType().FullName));
			}
			
		}
		#endregion

		#region Private Static Methods
		private static void WriteCoordinateSystem(ICoordinateSystem coordinateSystem, XmlTextWriter writer)
		{
			
			if (coordinateSystem is ICompoundCoordinateSystem)
			{
				WriteCompoundCoordinateSystem(coordinateSystem as ICompoundCoordinateSystem, writer);		
			}
			else if (coordinateSystem is IGeographicCoordinateSystem)
			{
				WriteGeographicCoordinateSystem( coordinateSystem as IGeographicCoordinateSystem, writer);
			}
			else if (coordinateSystem is IProjectedCoordinateSystem)
			{
				WriteProjectedCoordinateSystem( coordinateSystem as IProjectedCoordinateSystem, writer);
			}
			else if (coordinateSystem is ILocalCoordinateSystem)
			{
				WriteLocalCoordinateSystem( coordinateSystem as ILocalCoordinateSystem, writer);
			}
			else if (coordinateSystem is IFittedCoordinateSystem)
			{
				WriteFittedCoordinateSystem( coordinateSystem as IFittedCoordinateSystem, writer);
			}
			else if (coordinateSystem is IGeocentricCoordinateSystem)
			{
				WriteGeocentricCoordinateSystem(coordinateSystem as IGeocentricCoordinateSystem, writer);
			}
			else if (coordinateSystem is IVerticalCoordinateSystem)
			{
				WriteVerticalCoordinateSystem(coordinateSystem as IVerticalCoordinateSystem, writer);
			}
			else if (coordinateSystem is IHorizontalCoordinateSystem)
			{
				WriteHorizontalCoordinateSystem( coordinateSystem as IHorizontalCoordinateSystem, writer);
			}
			else
			{
				throw new InvalidOperationException("this coordinate system is recongized");
			}

			//writer.WriteEndElement();
		}
		private static void WriteUnit(IUnit unit, XmlTextWriter writer)
		{
			if (unit is IAngularUnit)
			{
				WriteAngularUnit(unit as IAngularUnit, writer);
			}
			else if (unit is ILinearUnit)
			{
				WriteLinearUnit(unit as ILinearUnit, writer);
			}
			else
			{
				throw new InvalidOperationException("this unit is not recognized");
			}
		}

		private static void WriteCSInfo(IInfo info, XmlTextWriter writer)
		{
			//string t1=writer.BaseStream.ToString();
			writer.WriteStartElement("CS_Info");
			if (info.AuthorityCode.Trim()!="")
			{
				writer.WriteAttributeString("AuthorityCode",info.AuthorityCode);
			}
			if (info.Abbreviation!=null && info.Abbreviation.Trim()!="")
			{
				writer.WriteAttributeString("Abbreviation",info.Abbreviation);
			}
			if (info.Authority.Trim()!="")
			{
				writer.WriteAttributeString("Authority",info.Authority);
			}
			if (info.Name.Trim()!="")
			{
				writer.WriteAttributeString("Name",info.Name);
			}
			//string t2=writer.BaseStream.ToString();
			writer.WriteEndElement();
			//string t3=writer.BaseStream.ToString();
		}
		#region Units
		
		/// <summary>
		/// Writes an angular unit to the given IndentedTextWriter object.
		/// </summary>
		/// <param name="angularUnit">The angular unit to write.</param>
		/// <param name="writer">The IndentedTextWriter to write to. </param>
		private static void WriteAngularUnit(IAngularUnit angularUnit, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_AngularUnit");
			writer.WriteAttributeString("RadiansPerUnit",angularUnit.RadiansPerUnit.ToString());
			WriteCSInfo(angularUnit,writer);
			writer.WriteEndElement();
		}
		private static void WriteLinearUnit(ILinearUnit linearUnit, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_LinearUnit");
			writer.WriteAttributeString("MetersPerUnit",linearUnit.MetersPerUnit.ToString());
			WriteCSInfo(linearUnit,writer);
			writer.WriteEndElement();
		}
		#endregion
		

		private static void WriteCompoundCoordinateSystem(ICompoundCoordinateSystem compoundCoordinateSystem, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_CoordinateSystem");
			writer.WriteAttributeString("Dimension",compoundCoordinateSystem.Dimension.ToString() );

			writer.WriteStartElement("CS_CompoundCoordinateSystem");

			
			WriteCSInfo(compoundCoordinateSystem, writer);
			for(int i=0;i<compoundCoordinateSystem.Dimension;i++)
			{
				WriteAxis(compoundCoordinateSystem.GetAxis(i), writer);
			}
			
			writer.WriteStartElement("CS_CoordinateSystem");
			writer.WriteAttributeString("Dimension",compoundCoordinateSystem.HeadCS.Dimension.ToString() );
			WriteCoordinateSystem(compoundCoordinateSystem.HeadCS, writer);
			writer.WriteEndElement();
			
			writer.WriteStartElement("CS_CoordinateSystem");
			writer.WriteAttributeString("Dimension",compoundCoordinateSystem.TailCS.Dimension.ToString() );
			WriteCoordinateSystem(compoundCoordinateSystem.TailCS, writer);
			writer.WriteEndElement();

			writer.WriteEndElement();
			writer.WriteEndElement();
		}
		
		#region Coordinate Systems
		
		private static void WriteGeographicCoordinateSystem(IGeographicCoordinateSystem geographicCoordinateSystem, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_GeographicCoordinateSystem");
			WriteCSInfo(geographicCoordinateSystem, writer);
			for(int i=0;i<geographicCoordinateSystem.Dimension;i++)
			{
				WriteAxis(geographicCoordinateSystem.GetAxis(i), writer);
			}
			WriteHorizontalDatum(geographicCoordinateSystem.HorizontalDatum, writer);
			WriteAngularUnit(geographicCoordinateSystem.AngularUnit, writer);
			WritePrimeMeridian(geographicCoordinateSystem.PrimeMeridian, writer);
			writer.WriteEndElement();
		}
		private static void WriteProjectedCoordinateSystem(IProjectedCoordinateSystem projectedCoordinateSystem, XmlTextWriter writer)
		{
			
			writer.WriteStartElement("CS_ProjectedCoordinateSystem");
			WriteCSInfo(projectedCoordinateSystem, writer);
			for(int i=0;i<projectedCoordinateSystem.Dimension;i++)
			{
				WriteAxis(projectedCoordinateSystem.GetAxis(i), writer);
			}
			WriteCoordinateSystem(projectedCoordinateSystem.GeographicCoordinateSystem, writer);
			WriteLinearUnit(projectedCoordinateSystem.LinearUnit, writer );
			WriteProjection(projectedCoordinateSystem.Projection, writer);
			writer.WriteEndElement();
		}
		#endregion

		#region Datums
		private static void WriteDatum(IDatum datum, XmlTextWriter writer)
		{
			if (datum is IVerticalDatum)
			{
				WriteVerticalDatum(datum as IVerticalDatum, writer);
			}
			else if (datum is IHorizontalDatum)
			{
				WriteHorizontalDatum(datum as IHorizontalDatum, writer);
			}
			else
			{
				throw new NotImplementedException("This datum is not supported.");
			}
		}
		private static void WriteHorizontalDatum(IHorizontalDatum horizontalDatum, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_HorizontalDatum");
			writer.WriteAttributeString("DatumType",DatumTypeAsCode(horizontalDatum.DatumType));
			WriteCSInfo(horizontalDatum, writer);
			WriteEllipsoid(horizontalDatum.Ellipsoid, writer);
			WriteWGS84ConversionInfo(horizontalDatum.WGS84Parameters, writer);
			writer.WriteEndElement();
		}
		#endregion

		private static void WriteEllipsoid(IEllipsoid ellipsoid, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_Ellipsoid");
			writer.WriteAttributeString("SemiMajorAxis",ellipsoid.SemiMajorAxis.ToString());
			writer.WriteAttributeString("SemiMinorAxis",ellipsoid.SemiMinorAxis.ToString());
			writer.WriteAttributeString("InverseFlattening",ellipsoid.InverseFlattening.ToString());
			if (ellipsoid.IsIvfDefinitive())
			{
				writer.WriteAttributeString("IvfDefinitive","1");
			}
			else
			{
				writer.WriteAttributeString("CS_vfDefinitive","1");
			}
			WriteCSInfo(ellipsoid, writer);
			WriteUnit(ellipsoid.AxisUnit,writer);
			writer.WriteEndElement();
		}
		private static void WriteAxis(IAxisInfo axis, XmlTextWriter writer)
		{
			string axisOrientation="";
			switch(axis.Orientation)
			{
				case AxisOrientation.Down:
					axisOrientation="DOWN";
					break;
				case AxisOrientation.East:
					axisOrientation="EAST";
					break;
				case AxisOrientation.North:
					axisOrientation="NORTH";
					break;
				case AxisOrientation.Other:
					axisOrientation="OTHER";
					break;
				case AxisOrientation.South:
					axisOrientation="SOUTH";
					break;
				case AxisOrientation.Up:
					axisOrientation="UP";
					break;
				case AxisOrientation.West:
					axisOrientation="WEST";
					break;
				default:
					throw new InvalidOperationException("This enum should not exist");
			}
			//writer.WriteLine(String.Format("AXIS[\"{0}\",\"{1}\"],", axis.Name, axisOrientation));	
			writer.WriteStartElement("CS_AxisInfo");
			writer.WriteAttributeString("Name",axis.Name);
			writer.WriteAttributeString("Orientation",axisOrientation);
			writer.WriteEndElement();
		} 

		private static void WriteWGS84ConversionInfo(WGS84ConversionInfo conversionInfo,  XmlTextWriter writer)
		{	 
			writer.WriteStartElement("CS_WGS84ConversionInfo");
			writer.WriteAttributeString("Dx",conversionInfo.Dx.ToString());
			writer.WriteAttributeString("Dy",conversionInfo.Dy.ToString());
			writer.WriteAttributeString("Dz",conversionInfo.Dz.ToString());
			writer.WriteAttributeString("Ex",conversionInfo.Ex.ToString());
			writer.WriteAttributeString("Ey",conversionInfo.Ey.ToString());
			writer.WriteAttributeString("Ez",conversionInfo.Ex.ToString());
			writer.WriteAttributeString("Ppm",conversionInfo.Ppm.ToString());
			writer.WriteEndElement();	
		}
		
		

		private static void WritePrimeMeridian(IPrimeMeridian primeMeridian, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_PrimeMeridian");
			writer.WriteAttributeString("Longitude",primeMeridian.Longitude.ToString())	;
			WriteCSInfo(primeMeridian, writer);
			WriteAngularUnit(primeMeridian.AngularUnit, writer);
			writer.WriteEndElement();
		}
		
		
		private static void WriteProjection(IProjection projection, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_Projection");
			writer.WriteAttributeString("ClassName",projection.ClassName);
			WriteCSInfo(projection, writer);
			for (int i=0;i<projection.NumParameters; i++)
			{
				string paramName = projection.GetParameter(i).Name;
				double paramValue = projection.GetParameter(i).Value;
				writer.WriteStartElement("CS_ProjectionParameter");
				writer.WriteAttributeString("Name",paramName);
				writer.WriteAttributeString("Value",paramValue.ToString());
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
		private static void WriteVerticalCoordinateSystem(IVerticalCoordinateSystem verticalCoordinateSystem, XmlTextWriter writer)
		{
			
			writer.WriteStartElement("CS_VerticalCoordinateSystem");
			//writer.WriteAttributeString("Dimension",verticalCoordinateSystem.Dimension.ToString());
			WriteCSInfo( verticalCoordinateSystem, writer);
			for(int i=0;i<verticalCoordinateSystem.Dimension;i++)
			{
				WriteAxis(verticalCoordinateSystem.GetAxis(i), writer);
			}
			WriteVerticalDatum( verticalCoordinateSystem.VerticalDatum, writer);
			WriteLinearUnit( verticalCoordinateSystem.VerticalUnit, writer);
			writer.WriteEndElement();
		}
		
		private static void WriteVerticalDatum(IVerticalDatum verticalDatum, XmlTextWriter writer)
		{
			writer.WriteStartElement("CS_VerticalDatum");
			writer.WriteAttributeString("DatumType",DatumTypeAsCode(verticalDatum.DatumType));
			WriteCSInfo(verticalDatum, writer);
			writer.WriteEndElement();
		}

		public static string DatumTypeAsCode(DatumType datumtype)
		{
			string datumCode = Enum.Format(typeof(DatumType),datumtype, "d");
			return datumCode;
		}
		#endregion

		#region  Not Implemented.
		public static void WriteFittedCoordinateSystem(IFittedCoordinateSystem fiitedCoordinateSystem, XmlTextWriter writer)
		{
			throw new NotImplementedException();
		}
		public static void WriteGeocentricCoordinateSystem(IGeocentricCoordinateSystem geocentricCoordinateSystem, XmlTextWriter writer)
		{
			throw new NotImplementedException();
		}
		public static void WriteHorizontalCoordinateSystem(IHorizontalCoordinateSystem horizontalCoordinateSystem, XmlTextWriter writer)
		{
			throw new NotImplementedException();
		}
		private static void WriteLocalCoordinateSystem(ILocalCoordinateSystem localCoordinateSystem, XmlTextWriter writer)
		{
			throw new NotImplementedException();
		}
		private static void WriteLocalDatum(LocalDatum localDatum, XmlTextWriter writer)
		{
			throw new NotImplementedException();
		}
		#endregion

	}
}
