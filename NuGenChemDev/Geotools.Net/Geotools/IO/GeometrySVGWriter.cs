 /*  Copyright (C) 2002 Urban Science Applications, Inc. 
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
using System.Xml;
using Geotools.Utilities;
using Geotools.Geometries;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// The SVG Writer outputs the textual representation of a Geometry object.
	/// </summary>
	/// <remarks>
	/// <para>The GeometrySVGWriter will output coordinates rounded to the precision model. No more than 
	/// the maximum number of necessary decimal places will be output.</para>
	/// </remarks>
	public class GeometrySVGWriter
	{

		private int _radius =2;
		private string _cssClass="";
		private string _style="";
		private string _formatterString="";
		private string _namespace="";
		private PrecisionModel _precisionModel;
		private bool _useRelative = true;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the GeometrySVGWriter class.
		/// </summary>
		public GeometrySVGWriter(PrecisionModel precisionModel): this(precisionModel,"",5)
		{

		}
		public GeometrySVGWriter(PrecisionModel precisionModel, string xmlNamespace, int decimalPlaces)
		{
			_formatterString = CreateFormatter(decimalPlaces);
			_namespace = xmlNamespace;
			_precisionModel = precisionModel;
		}

		

		#endregion

		#region Properties
		/// <summary>
		/// Radius of circle objects written for Point objects.
		/// </summary>
		public int Radius
		{
			set
			{
				_radius = value;
				if ( _radius <= 0 )
					_radius = 1;
			}
			get
			{
				return _radius;
			}
		}

		/// <summary>
		/// If set true (the default) graphics paths use relative coordinates.
		/// </summary>
		public bool RelativeCoordinates
		{
			get
			{
				return _useRelative;
			}
			set
			{
				_useRelative = value;
			}
		}
		#endregion

		#region Methods

		/*/// <summary>
		///	 Creates the DecimalFormat used to write doubles
		///  with a sufficient number of decimal places.
		/// </summary>
		/// <param name="precisionModel">The precision model used to determine the number of
		/// decimal places to write.</param>
		/// <returns>Returns a DecimalFormat object that writes doubles without scientific notation.</returns>
		private static string CreateFormatter(PrecisionModel precisionModel) 
		{
			// the default number of decimal places is 16, which is sufficient
			// to accomodate the maximum precision of a double.
			int decimalPlaces = 16;
			if (! precisionModel.IsFloating()) 
			{
				decimalPlaces = 1 + (int) Math.Ceiling(Math.Log(precisionModel.Scale) / Math.Log(10));
			}
			return ("#" + (decimalPlaces > 0 ? "." : "") + StringOfChar('#', decimalPlaces));
		}*/

		private static string CreateFormatter(int decimalPlaces) 
		{
			return "{0:F"+ decimalPlaces+"}";
		}


		/// <summary>
		/// Returns a string of repeated characters.
		/// </summary>
		/// <param name="ch">The character to repeat.</param>
		/// <param name="count">The number of times to repeat the character.</param>
		/// <returns>Returns a string of repeated characters.</returns>
		private static string StringOfChar(char ch, int count) 
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
		/// <param name="CssClass">The class attribute for the SVG element.</param>
		/// <param name="style"></param>
		/// <returns>A &lt;Geometry Tagged Text&gt; string (see the OpenGIS Simple
		///  Features Specification)</returns>
		public string Write(IGeometry geometry,string CssClass, string style)
		{
			_cssClass=CssClass;
			_style=style;
			StringWriter sw = new StringWriter();
			Geometry geometryObj = (Geometry)geometry;
			AppendGeometryTaggedText(geometry, sw);
			_cssClass="";
			_style="";
			return sw.ToString();
		}

		/// <summary>
		/// Converts a Geometry to its Well-known Text representation.
		/// </summary>
		/// <param name="geometry">A geometry to process.</param>
		/// <param name="writer">Stream to write out the geometry's text representation.</param>
		/// <param name="CssClass">The value to class attribute should have.</param>
		/// <param name="style">The value to put into the style attribute.</param>
		/// <remarks>
		/// Geometry is written to the output stream as &lt;Gemoetry Tagged Text&gt; string (see the OpenGIS
		/// Simple Features Specification).
		/// </remarks>
		public void Write(IGeometry geometry, TextWriter writer,string CssClass, string style)
		{
			_cssClass=CssClass;
			_style=style;
			Geometry geometryObj = (Geometry)geometry;
			AppendGeometryTaggedText(geometry, writer);
			_cssClass="";
			_style="";

		}

		/// <summary>
		/// Converts a Geometry to &lt;Geometry Tagged Text &gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="geometry">The Geometry to process.</param>
		/// <param name="writer">The output stream to Append to.</param>
		protected void AppendGeometryTaggedText(IGeometry geometry, TextWriter writer)
		{
			
			if (geometry is Point) 
			{
				Point point = (Point) geometry;
				AppendPointTaggedText(point.GetCoordinate(), writer, _precisionModel);
			}
			else if (geometry is LineString) 
			{
				AppendLineStringTaggedText((LineString) geometry,  writer);
			}
			else if (geometry is Polygon) 
			{
				AppendPolygonTaggedText((Polygon) geometry,  writer);
			}
			else if (geometry is MultiPoint) 
			{
				AppendMultiPointTaggedText((MultiPoint) geometry,  writer);
			}
			else if (geometry is MultiLineString) 
			{
				AppendMultiLineStringTaggedText((MultiLineString) geometry,  writer);
			}
			else if (geometry is MultiPolygon) 
			{
				AppendMultiPolygonTaggedText((MultiPolygon) geometry,  writer);
			}
			else if (geometry is GeometryCollection) 
			{
				AppendGeometryCollectionTaggedText((GeometryCollection) geometry,  writer);
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
		/// <param name="writer">the output writer to Append to</param>
		/// <param name="precisionModel">the PrecisionModel to use to convert
		///  from a precise coordinate to an external coordinate</param>
		protected void AppendPointTaggedText(Coordinate coordinate,  TextWriter writer, 	PrecisionModel precisionModel)
		{
			Coordinate externalCoordinate = new Coordinate();
			precisionModel.ToExternal(coordinate, externalCoordinate);
			if (this._cssClass!="")
			{
				writer.WriteLine(String.Format("<{5}circle class=\"{0}\" style=\"{1}\" cx=\"{2}\" cy=\"{3}\" r=\"{4}\"/>",_cssClass,_style,WriteNumber(externalCoordinate.X),WriteNumber(externalCoordinate.Y),_radius,_namespace));			
			}
			else
			{
				writer.WriteLine(String.Format("<{4}circle  style=\"{0}\" cx=\"{1}\" cy=\"{2}\" r=\"{3}\"/>",_style,WriteNumber(externalCoordinate.X),WriteNumber(externalCoordinate.Y),_radius,_namespace));			
			}
		}

		protected void AppendPath(TextWriter writer)
		{
			if (this._cssClass=="")
			{
				writer.WriteLine(String.Format("<{1}path  style=\"{0}\" d=\"",_style,_namespace));
			}
			else
			{
				writer.WriteLine(String.Format("<{2}path class=\"{0}\" style=\"{1}\" d=\"",_cssClass,_style,_namespace));
			}
		}
		protected void AppendEndPath(TextWriter writer)
		{
			writer.Write("\"/>");
		}
		/// <summary>
		/// Converts a LineString to SVG path element. 
		/// </summary>
		/// <param name="lineString">The LineString to process.</param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendLineStringTaggedText(LineString lineString, TextWriter writer)
		{
			AppendPath(writer);
			AppendLineStringText(lineString,  writer);
			AppendEndPath(writer);	
		}

		/// <summary>
		///  Converts a Polygon to &lt;Polygon Tagged Text&gt; format,
		///  then Appends it to the writer.
		/// </summary>
		/// <param name="polygon">Th Polygon to process.</param>
		/// <param name="writer">The stream writer to Append to.</param>
		protected void AppendPolygonTaggedText(Polygon polygon,  TextWriter writer)
		{
			AppendPath(writer);
			AppendPolygonText(polygon, writer);
			AppendEndPath(writer);	
		}

		/// <summary>
		/// Converts a MultiPoint to &lt;MultiPoint Tagged Text&gt;
		/// format, then Appends it to the writer.
		/// </summary>
		/// <param name="multipoint">The MultiPoint to process.</param>
		/// <param name="writer">The output writer to Append to.</param>
		protected void AppendMultiPointTaggedText(MultiPoint multipoint, TextWriter writer)
		{
			//writer.Write("<g>");
			AppendMultiPointText(multipoint,  writer);
			//writer.Write("</g>");
		}

		/// <summary>
		/// Converts a MultiLineString to &lt;MultiLineString Tagged
		/// Text&gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="multiLineString">The MultiLineString to process</param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendMultiLineStringTaggedText( MultiLineString multiLineString, TextWriter writer )
		{
			AppendPath(writer);
			AppendMultiLineStringText(multiLineString, writer);
			AppendEndPath(writer);	
		}

		/// <summary>
		/// Converts a MultiPolygon to &lt;MultiPolygon Tagged
		/// Text&gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="multiPolygon">The MultiPolygon to process</param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendMultiPolygonTaggedText(MultiPolygon multiPolygon,  TextWriter writer)
		{
			AppendPath(writer);
			AppendMultiPolygonText(multiPolygon,  writer);
			AppendEndPath(writer);	
		}

		/// <summary>
		/// Converts a GeometryCollection to &lt;GeometryCollection Tagged
		/// Text&gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="geometryCollection">The GeometryCollection to process</param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendGeometryCollectionTaggedText(GeometryCollection geometryCollection, TextWriter writer)
		{
			writer.Write("<g>");
			AppendGeometryCollectionText(geometryCollection,  writer);
			writer.Write("</g>");
		}

		/// <summary>
		/// Converts a Coordinate to &lt;Point&gt; format, then Appends
		/// it to the writer. 
		/// </summary>
		/// <param name="coordinate">The Coordinate to process.</param>
		/// <param name="writer">The output writer to Append to.</param>
		/// <param name="precisionModel">The PrecisionModel to use to convert
		/// from a precise coordinate to an external coordinate</param>
		protected void AppendCoordinate(Coordinate coordinate, TextWriter writer, PrecisionModel precisionModel)
		{
			//throw new NotFiniteNumberException();
			Coordinate externalCoordinate = new Coordinate();
			precisionModel.ToExternal(coordinate, externalCoordinate);
			writer.Write(" " + WriteNumber(externalCoordinate.X) + " " + WriteNumber(externalCoordinate.Y));
			//writer.Write(WriteNumber(coordinate.X) + " " + WriteNumber(coordinate.Y));
			
		}

		/// <summary>
		/// Converts a double to a string, not in scientific notation.
		/// </summary>
		/// <param name="d">The double to convert.</param>
		/// <returns>The double as a string, not in scientific notation.</returns>
		protected string WriteNumber(double d) 
		{
			if (d!=0.0)
			{
				return String.Format(_formatterString, d);
			}
			
			return "0";
		}

		public void AppendLineStringText(LineString lineString, TextWriter writer)
		{
			if (_useRelative)
			{
				AppendLineStringTextRelative(lineString, writer);
			}
			else
			{
				AppendLineStringTextAbsolute(lineString, writer);
			}
		}
		/// <summary>
		/// Converts a LineString to &lt;LineString Text&gt; format, then
		/// Appends it to the writer.
		/// </summary>
		/// <param name="lineString">The LineString to process.</param>
		/// <param name="writer">The output stream to Append to.</param>
		public void AppendLineStringTextRelative(LineString lineString, TextWriter writer)
		{
			
			if (lineString.IsEmpty()) 
			{
				//writer.Write("EMPTY");
			}
			else 
			{
				writer.Write(" M ");
				double currentX = lineString.GetCoordinateN(0).X; 
				double currentY = lineString.GetCoordinateN(0).Y;
				double x=0;
				double y=0;
				AppendCoordinate(lineString.GetCoordinateN(0), writer, _precisionModel);
				Coordinate relativeCoordinate = new Coordinate();
				writer.Write(" l ");
				for (int i = 1; i < lineString.GetNumPoints(); i++) 
				{
					x = lineString.GetCoordinateN(i).X;
					y = lineString.GetCoordinateN(i).Y;
					relativeCoordinate.X=  x- currentX;
					relativeCoordinate.Y=  y - currentY;
					AppendCoordinate(relativeCoordinate, writer, _precisionModel);
					currentX = x;
					currentY = y;	
					if (i%5==4)
					{
						writer.WriteLine();
					}
				}
				
			}
		}

		protected void AppendLineStringTextAbsolute(LineString lineString, TextWriter writer)
		{
			
			if (lineString.IsEmpty()) 
			{
				//writer.Write("EMPTY");
			}
			else 
			{
				writer.Write(" M ");
				AppendCoordinate(lineString.GetCoordinateN(0), writer, _precisionModel);
				for (int i = 1; i < lineString.GetNumPoints(); i++) 
				{
					writer.Write(" L ");
					AppendCoordinate(lineString.GetCoordinateN(i), writer, _precisionModel);
					if (i%5==4)
					{
						writer.WriteLine();
					}
				}
				
			}
		}

		/// <summary>
		/// Converts a Polygon to &lt;Polygon Text&gt; format, then
		/// Appends it to the writer.
		/// </summary>
		/// <param name="polygon">The Polygon to process.</param>
		/// <param name="writer"></param>
		protected void AppendPolygonText(Polygon polygon,  TextWriter writer)
		{
			
			if (polygon.IsEmpty()) 
			{
				//writer.Write("EMPTY");
			}
			else 
			{
				AppendLineStringText(polygon.Shell,writer);
				for (int i = 0; i < polygon.GetNumInteriorRing(); i++) 
				{
					AppendLineStringText(polygon.Holes[i],  writer);
				}
				writer.Write(" Z ");
			}	
		
		}

		/// <summary>
		/// Converts a MultiPoint to &lt;MultiPoint Text&gt; format, then
		/// Appends it to the writer.
		/// </summary>
		/// <param name="multiPoint">The MultiPoint to process.</param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendMultiPointText(MultiPoint multiPoint,  TextWriter writer)
		{
			if (multiPoint.IsEmpty()) 
			{
				//writer.Write("EMPTY");
			}
			else 
			{
				for (int i = 0; i < multiPoint.GetNumGeometries(); i++) 
				{
					AppendPointTaggedText( multiPoint.GetCoordinate(i), writer, _precisionModel);
				}
			}
		}

		/// <summary>
		/// Converts a MultiLineString to &lt;MultiLineString Text&gt;
		/// format, then Appends it to the writer.
		/// </summary>
		/// <param name="multiLineString">The MultiLineString to process.</param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendMultiLineStringText(MultiLineString multiLineString, TextWriter writer)
		{
			
			if (multiLineString.IsEmpty()) 
			{
				writer.Write("EMPTY");
			}
			else 
			{
				for (int i = 0; i < multiLineString.GetNumGeometries(); i++) 
				{
					if (i > 0) 
					{
						writer.Write(", ");
					}
					//AppendLineStringText((LineString) multiLineString.GetGeometryN(i), level2, doIndent, writer);
					AppendLineStringText((LineString) multiLineString.GetGeometryN(i), writer);
				}
				//writer.Write(")");
			}
		}

		/// <summary>
		/// Converts a MultiPolygon to &lt;MultiPolygon Text&gt; format, then Appends to it to the writer.
		/// </summary>
		/// <param name="multiPolygon">The MultiPolygon to process.</param>
		/// <param name="writer">The output stream to Append to.</param>
		protected void AppendMultiPolygonText(MultiPolygon multiPolygon, TextWriter writer)
		{
			
			if (multiPolygon.IsEmpty()) 
			{
				writer.Write("EMPTY");
			}
			else 
			{
				//writer.Write("M");
				for (int i = 0; i < multiPolygon.GetNumGeometries(); i++) 
				{
					/*if (i > 0 && (i<multiPolygon.GetNumGeometries()-1) ) 
					{
						writer.Write(", ");
					}*/
					AppendPolygonText((Polygon) multiPolygon.GetGeometryN(i), writer);
				}
				//writer.Write("Z");
			}
			
			
		}

		/// <summary>
		/// Converts a GeometryCollection to &lt;GeometryCollection Text &gt; format, then Appends it to the writer.
		/// </summary>
		/// <param name="geometryCollection">The GeometryCollection to process.</param>
		/// <param name="writer">The output stream writer to Append to.</param>
		protected void AppendGeometryCollectionText(GeometryCollection geometryCollection, TextWriter writer)
		{	
			if (geometryCollection.IsEmpty()) 
			{
			}
			else 
			{
				for (int i = 0; i < geometryCollection.GetNumGeometries(); i++) 
				{
					AppendGeometryTaggedText(geometryCollection.GetGeometryN(i),  writer);
				}
			}			
		}
		#endregion

	}
}
