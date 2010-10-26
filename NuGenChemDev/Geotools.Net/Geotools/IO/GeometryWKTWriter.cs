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
using System.IO;
using System.Text;
using Geotools.Utilities;
using Geotools.Geometries;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Outputs the textual representation of a Geometry object.
	/// </summary>
	/// <remarks>
	/// <para>The Well-known Text format is defined in the OpenGIS Simple Features Specification for SQL.</para>
	/// <para>The GeometryWKTWriter will output coordinates rounded to the precision model. No more than 
	/// the maximum number of necessary decimal places will be output.</para>
	/// </remarks>
	public class GeometryWKTWriter
	{
		/// <summary>
		/// Converts doubles to string without scientific notation.
		/// </summary>
		//private DecimalFormat _formatter;
		private bool _isFormatted;
		private int _indent=2;
		private string _formatterString;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the GeometryWKTWriter class.
		/// </summary>
		public GeometryWKTWriter()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		///	 Creates the DecimalFormat used to write doubles
		///  with a sufficient number of decimal places.
		/// </summary>
		/// <param name="precisionModel">The precision model used to determine the number of
		/// decimal places to write.</param>
		/// <returns>Returns a DecimalFormat object that writes doubles without scientific notation.</returns>
		private static string CreateFormatter(PrecisionModel precisionModel) 
		{
			return "r";
			/*
			// the default number of decimal places is 16, which is sufficient
			// to accomodate the maximum precision of a double.
			int decimalPlaces = 16;
			if (! precisionModel.isFloating()) 
			{
				decimalPlaces = 1 + (int) Math.ceil(Math.log(precisionModel.GetScale()) / Math.log(10));
			}
			return new DecimalFormat("#" + (decimalPlaces > 0 ? "." : "")
				+ stringOfChar('#', decimalPlaces));
				
		
			return null;*/
		}


		/// <summary>
		/// Returns a string of repeated characters.
		/// </summary>
		/// <param name="ch">The character to repeat.</param>
		/// <param name="count">The number of times to repeat the character.</param>
		/// <returns>Returns a string of repeated characters.</returns>
		public static string StringOfChar(char ch, int count) 
		{
			
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < count; i++) 
			{
				sb.Append(ch);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Converts a Geometry to its Well-known Text representation.
		/// </summary>
		/// <param name="geometry">A Geometry to write.</param>
		/// <returns>A &lt;Geometry Tagged Text&gt; string (see the OpenGIS Simple
		///  Features Specification)</returns>
		public string Write(IGeometry geometry)
		{
			
			StringWriter sw = new StringWriter();

			WriteFormatted(geometry, false, sw);
			
			return sw.ToString();
		}

		/// <summary>
		/// Converts a Geometry to its Well-known Text representation.
		/// </summary>
		/// <param name="geometry">A geometry to process.</param>
		/// <param name="writer">Stream to write out the geometry's text representation.</param>
		/// <remarks>
		/// Geometry is written to the output stream as &lt;Gemoetry Tagged Text&gt; string (see the OpenGIS
		/// Simple Features Specification).
		/// </remarks>
		public void Write(IGeometry geometry, StringWriter writer)
		{
			WriteFormatted(geometry, false, writer);
		}

		/// <summary>
		/// Same as Write, but with newlines and spaces to make the well-known text more readable.
		/// </summary>
		/// <param name="geometry">The Geometry to process.</param>
		/// <returns>A &lt;Geometry Tagged Text&gt; string (see the OpenGIS Simple
		///  Features Specification), with newlines and spaces</returns>
		public string WriteFormatted(IGeometry geometry)
		{
			
			StringWriter sw = new StringWriter();
			WriteFormatted(geometry, true, sw);
			return sw.ToString();
		}

		/// <summary>
		/// Same as Write, but with newlines and spaces to make the well-known text more readable.
		/// </summary>
		/// <param name="geometry">The Geometry to process.</param>
		/// <param name="writer">Stream to write out the geometry's text representation.</param>
		public void WriteFormatted(IGeometry geometry, StringWriter writer)
		{
			WriteFormatted(geometry, true, writer);
		}


		/// <summary>
		/// Converts a Geometry to its Well-known Text representation.
		/// </summary>
		/// <param name="geometry">A Geometry to process.</param>
		/// <param name="isFormatted"></param>
		/// <param name="writer"></param>
		private void WriteFormatted(IGeometry geometry, bool isFormatted, StringWriter writer)
		{
		
			this._isFormatted = isFormatted;
			Geometry geometryObj = (Geometry)geometry;
			_formatterString = CreateFormatter( geometryObj.PrecisionModel );
			AppendGeometryTaggedText(geometry, 0, writer);

		}


		/// <summary>
		/// Converts a Geometry to &lt;Geometry Tagged Text &gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="geometry">The Geometry to process.</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream to Append to.</param>
		protected void AppendGeometryTaggedText(IGeometry geometry, int level, StringWriter writer)
		{
			
			Indent(level, writer);

			if (geometry is Point) 
			{
				Point point = (Point) geometry;
				AppendPointTaggedText(point.GetCoordinate(), level, writer, point.PrecisionModel );
			}
			else if (geometry is LineString) 
			{
				AppendLineStringTaggedText((LineString) geometry, level, writer);
			}
			else if (geometry is Polygon) 
			{
				AppendPolygonTaggedText((Polygon) geometry, level, writer);
			}
			else if (geometry is MultiPoint) 
			{
				AppendMultiPointTaggedText((MultiPoint) geometry, level, writer);
			}
			else if (geometry is MultiLineString) 
			{
				AppendMultiLineStringTaggedText((MultiLineString) geometry, level, writer);
			}
			else if (geometry is MultiPolygon) 
			{
				AppendMultiPolygonTaggedText((MultiPolygon) geometry, level, writer);
			}
			else if (geometry is GeometryCollection) 
			{
				AppendGeometryCollectionTaggedText((GeometryCollection) geometry, level, writer);
			}
			else 
			{
				throw new NotSupportedException("Unsupported Geometry implementation:"+ geometry.GetType().Name);
			}

		}

		/// <summary>
		/// Converts a Coordinate to &lt;Point Tagged Text&gt; format,
		/// then Appends it to the writer.
		/// </summary>
		/// <param name="coordinate">the <code>Coordinate</code> to process</param>
		/// <param name="level"></param>
		/// <param name="writer">the output writer to Append to</param>
		/// <param name="precisionModel">the PrecisionModel to use to convert
		///  from a precise coordinate to an external coordinate</param>
		protected void AppendPointTaggedText(Coordinate coordinate, int level, StringWriter writer,
											 PrecisionModel precisionModel)
		{
			
			writer.Write("POINT ");
			AppendPointText(coordinate, level, writer, precisionModel);
			
		}

		/// <summary>
		/// Converts a LineString to LineString tagged text format, 
		/// </summary>
		/// <param name="lineString">The LineString to process.</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendLineStringTaggedText(LineString lineString, int level, StringWriter writer)
		{
			
			writer.Write("LINESTRING ");
			AppendLineStringText(lineString, level, false, writer);
			
		}

		/// <summary>
		///  Converts a Polygon to &lt;Polygon Tagged Text&gt; format,
		///  then Appends it to the writer.
		/// </summary>
		/// <param name="polygon">Th Polygon to process.</param>
		/// <param name="level"></param>
		/// <param name="writer">The stream writer to Append to.</param>
		protected void AppendPolygonTaggedText(Polygon polygon, int level, StringWriter writer)
		{
			
			writer.Write("POLYGON ");
			AppendPolygonText(polygon, level, false, writer);
			
		}

		/// <summary>
		/// Converts a MultiPoint to &lt;MultiPoint Tagged Text&gt;
		/// format, then Appends it to the writer.
		/// </summary>
		/// <param name="multipoint">The MultiPoint to process.</param>
		/// <param name="level"></param>
		/// <param name="writer">The output writer to Append to.</param>
		protected void AppendMultiPointTaggedText(MultiPoint multipoint, int level, StringWriter writer)
		{
			
			writer.Write("MULTIPOINT ");
			AppendMultiPointText(multipoint, level, writer);
			
		}

		/// <summary>
		/// Converts a MultiLineString to &lt;MultiLineString Tagged
		/// Text&gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="multiLineString">The MultiLineString to process</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendMultiLineStringTaggedText( MultiLineString multiLineString, int level,
														StringWriter writer )
		{
			
			writer.Write("MULTILINESTRING ");
			AppendMultiLineStringText(multiLineString, level, false, writer);
			
		}

		/// <summary>
		/// Converts a MultiPolygon to &lt;MultiPolygon Tagged
		/// Text&gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="multiPolygon">The MultiPolygon to process</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendMultiPolygonTaggedText(MultiPolygon multiPolygon, int level, StringWriter writer)
		{
			
			writer.Write("MULTIPOLYGON ");
			AppendMultiPolygonText(multiPolygon, level, writer);
			
		}

		/// <summary>
		/// Converts a GeometryCollection to &lt;GeometryCollection Tagged
		/// Text&gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="geometryCollection">The GeometryCollection to process</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendGeometryCollectionTaggedText(GeometryCollection geometryCollection, int level,
														  StringWriter writer)
		{
			
			writer.Write("GEOMETRYCOLLECTION ");
			AppendGeometryCollectionText(geometryCollection, level, writer);
			
		}


		/// <summary>
		/// Converts a Coordinate to Point Text format then Appends it to the writer.
		/// </summary>
		/// <param name="coordinate">The Coordinate to process.</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream writer to Append to.</param>
		/// <param name="precisionModel">The PrecisionModel to use to convert from a precise
		/// coordinate to an external coordinate.</param>
		protected void AppendPointText( Coordinate coordinate, int level, StringWriter writer,
										PrecisionModel precisionModel)
		{
			
			if (coordinate == null) 
			{
				writer.Write("EMPTY");
			}
				else 
			{
				writer.Write("(");
				AppendCoordinate(coordinate, writer, precisionModel);
				writer.Write(")");
			}
			
		}

		/// <summary>
		/// Converts a Coordinate to &lt;Point&gt; format, then Appends
		/// it to the writer. 
		/// </summary>
		/// <param name="coordinate">The Coordinate to process.</param>
		/// <param name="writer">The output writer to Append to.</param>
		/// <param name="precisionModel">The PrecisionModel to use to convert
		/// from a precise coordinate to an external coordinate</param>
		protected void AppendCoordinate(Coordinate coordinate, StringWriter writer, PrecisionModel precisionModel)
		{
			
			Coordinate externalCoordinate = new Coordinate();
			precisionModel.ToExternal(coordinate, externalCoordinate);
			writer.Write(WriteNumber(externalCoordinate.X) + " " + WriteNumber(externalCoordinate.Y));
			
		}

		/// <summary>
		/// Converts a double to a string, not in scientific notation.
		/// </summary>
		/// <param name="d">The double to convert.</param>
		/// <returns>The double as a string, not in scientific notation.</returns>
		protected string WriteNumber(double d) 
		{
			//return formatter.format(d);
			//return "";
			return d.ToString(_formatterString);
		}

		/// <summary>
		/// Converts a LineString to &lt;LineString Text&gt; format, then
		/// Appends it to the writer.
		/// </summary>
		/// <param name="lineString">The LineString to process.</param>
		/// <param name="level"></param>
		/// <param name="doIndent"></param>
		/// <param name="writer">The output stream to Append to.</param>
		protected void AppendLineStringText(LineString lineString, int level, bool doIndent, StringWriter writer)
		{
			
			if ( lineString.IsEmpty() ) 
			{
				writer.Write("EMPTY");
			}
			else 
			{
				if (doIndent)
				{
					Indent(level, writer);
				}
				writer.Write("(");
				for (int i = 0; i < lineString.GetNumPoints(); i++) 
				{
					if (i > 0) 
					{
						writer.Write(", ");
						if (i % 10 == 0) Indent(level + 2, writer);
					}
					//AppendCoordinate(lineString.GetCoordinateN(i), writer, lineString.PrecisionModel);
					AppendCoordinate( lineString.GetCoordinates()[i], writer, lineString.PrecisionModel );
				}
				writer.Write(")");
			}
			
		}

		/// <summary>
		/// Converts a Polygon to &lt;Polygon Text&gt; format, then
		/// Appends it to the writer.
		/// </summary>
		/// <param name="polygon">The Polygon to process.</param>
		/// <param name="level"></param>
		/// <param name="indentFirst"></param>
		/// <param name="writer"></param>
		protected void AppendPolygonText(Polygon polygon, int level, bool indentFirst, StringWriter writer)
		{
			
			if ( polygon.IsEmpty() ) 
			{
				writer.Write("EMPTY");
			}
			else 
			{
				if (indentFirst) Indent(level, writer);
				writer.Write("(");
				AppendLineStringText(polygon.Shell, level, false, writer);
				for (int i = 0; i < polygon.GetNumInteriorRing(); i++) 
				{
					writer.Write(", ");
					AppendLineStringText(polygon.Holes[i], level + 1, true, writer);
				}
				writer.Write(")");
			}		
		}

		/// <summary>
		/// Converts a MultiPoint to &lt;MultiPoint Text&gt; format, then
		/// Appends it to the writer.
		/// </summary>
		/// <param name="multiPoint">The MultiPoint to process.</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendMultiPointText(MultiPoint multiPoint, int level, StringWriter writer)
		{
			
			if  ( multiPoint.IsEmpty() ) 
			{
				writer.Write("EMPTY");
			}
			else 
			{
				writer.Write("(");
				for (int i = 0; i < multiPoint.GetNumGeometries(); i++) 
				{
					if (i > 0) 
					{
						writer.Write(", ");
					}
						AppendCoordinate( multiPoint.GetCoordinates()[i], writer,
						multiPoint.PrecisionModel );
				}
				writer.Write(")");
			}
				
		}

		/// <summary>
		/// Converts a MultiLineString to &lt;MultiLineString Text&gt;
		/// format, then Appends it to the writer.
		/// </summary>
		/// <param name="multiLineString">The MultiLineString to process.</param>
		/// <param name="level"></param>
		/// <param name="indentFirst"></param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendMultiLineStringText(MultiLineString multiLineString, int level, bool indentFirst,
												 StringWriter writer)
		{
			
			if ( multiLineString.IsEmpty() ) 
			{
				writer.Write("EMPTY");
			}
			else 
			{
				int level2 = level;
				bool doIndent = indentFirst;
				writer.Write("(");
				for (int i = 0; i < multiLineString.GetNumGeometries(); i++) 
				{
					if (i > 0) 
					{
						writer.Write(", ");
						level2 = level + 1;
						doIndent = true;
					}
					//AppendLineStringText((LineString) multiLineString.GetGeometryN(i), level2, doIndent, writer);
					AppendLineStringText((LineString) multiLineString.GetGeometryN(i), level2, doIndent, writer);
				}
				writer.Write(")");
			}
			
		}

		/// <summary>
		/// Converts a MultiPolygon to &lt;MultiPolygon Text&gt; format, then Appends to it to the writer.
		/// </summary>
		/// <param name="multiPolygon">The MultiPolygon to process.</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream to Append to.</param>
		protected void AppendMultiPolygonText(MultiPolygon multiPolygon, int level, StringWriter writer)
		{
			
			if ( multiPolygon.IsEmpty() ) 
			{
				writer.Write("EMPTY");
			}
			else 
			{
				int level2 = level;
				bool doIndent = false;
				writer.Write("(");
				for (int i = 0; i < multiPolygon.GetNumGeometries(); i++) 
				{
					if (i > 0) 
					{
						writer.Write(", ");
						level2 = level + 1;
						doIndent = true;
					}
					//AppendPolygonText((Polygon) multiPolygon.GetGeometryN(i), level2, doIndent, writer);
					AppendPolygonText((Polygon) multiPolygon.GetGeometryN(i), level2, doIndent, writer);
				}
				writer.Write(")");
			}
			
		}

		/// <summary>
		/// Converts a GeometryCollection to &lt;GeometryCollection Text &gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="geometryCollection">The GeometryCollection to process.</param>
		/// <param name="level"></param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendGeometryCollectionText(GeometryCollection geometryCollection, int level,
													StringWriter writer)
		{
			
			if ( geometryCollection.IsEmpty() ) 
			{
				writer.Write("EMPTY");
			}
			else 
			{
				int level2 = level;
				writer.Write("(");
				for (int i = 0; i < geometryCollection.GetNumGeometries(); i++) 
				{
					if (i > 0) 
					{
						writer.Write(", ");
						level2 = level + 1;
					}
					//AppendGeometryTaggedText(geometryCollection.GetGeometryN(i), level2, writer);
					AppendGeometryTaggedText(geometryCollection.GetGeometryN(i), level2, writer);
				}
				writer.Write(")");
			}
			
		}

		private void Indent(int level, StringWriter writer)
		{
			
			if (! _isFormatted || level <= 0) return;
			writer.Write("\n");
			writer.Write(StringOfChar(' ', _indent * level));
			
		}

		#endregion

	}
}
