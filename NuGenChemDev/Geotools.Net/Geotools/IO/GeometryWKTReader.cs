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
using System.Collections;
using Geotools.Geometries;
using Geotools.Utilities;
#endregion

namespace Geotools.IO
{
	/// <summary>
	///  Converts a Well-known Text string to a Geometry object.
	/// </summary>
	/// <remarks>The Well-known
	///  <para>Text format is defined in the 
	///  OpenGIS Simple Features Specification for SQL</para>
	///
	///  <para>The Well-Known Text reader (GeometryWKTReader) is designed to allow
	///  extracting Geometry objects from either input streams or
	///  internal strings. This allows it to function as a parser to read Geometry
	///  objects from text blocks embedded in other data formats (e.g. XML).</para>
	///
	///  <para>Note: There is an inconsistency in the SFS. The WKT grammar states
	///  that MultiPoints are represented by MULTIPOINT ( ( x y), (x y) )
	///  , but the examples show MultiPoints as MULTIPOINT ( x y, x y )
	///  . Other implementations follow the latter syntax, so JTS will adopt it as
	///  well.</para>
	///
	///  <para>A GeometryWKTReader is parameterized by a GeometryFactory
	///  , to allow it to create Geometry objects of the appropriate
	///  implementation. In particular, the GeometryFactory will
	///  determine the PrecisionModel and SRID that is used.</para>
	///
	///  <para>The GeometryWKTReader will convert the input numbers to the precise
	///  internal representation.</para>
	/// </remarks> 
	public class GeometryWKTReader
	{
		private GeometryFactory _geometryFactory;
		private PrecisionModel _precisionModel;

		#region Constructors

		/// <summary>
		/// Creates a GeometryWKTReader that creates objects using the given GeometryFactory.
		/// </summary>
		/// <param name="geometryFactory">The factory used to create Geometrys.</param>
		public GeometryWKTReader(GeometryFactory geometryFactory) 
		{
			_geometryFactory = geometryFactory;
			_precisionModel = geometryFactory.PrecisionModel;
		}		
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Converts a Well-known text representation to a Geometry.
		/// </summary>
		/// <param name="wellKnownText">A Geometry tagged text string ( see the OpenGIS Simple Features Specification.</param>
		/// <returns>Returns a Geometry specified by wellKnownText.  Throws an exception if there is a parsing problem.</returns>
		public  Geometry Create(string wellKnownText)
		{
			// throws a parsing exception is there is a problem.
			StringReader reader = new StringReader(wellKnownText);
			return Create(reader);
		}

		/// <summary>
		/// Converts a Well-known Text representation to a Geometry.
		/// </summary>
		/// <param name="reader">A Reader which will return a Geometry Tagged Text
		/// string (see the OpenGIS Simple Features Specification)</param>
		/// <returns>Returns a Geometry read from StreamReader.  An exception will be thrown if there is a
		/// parsing problem.</returns>
		public  Geometry Create(TextReader reader) 
		{
			
			WktStreamTokenizer tokenizer = new WktStreamTokenizer(reader);
	
			return ReadGeometryTaggedText(tokenizer);
		}

		/// <summary>
		/// Returns the next array of Coordinates in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text format.  The
		/// next element returned by the stream should be "(" (the beginning of "(x1 y1, x2 y2, ..., xn yn)" or
		/// "EMPTY".</param>
		/// <returns>The next array of Coordinates in the stream, or an empty array of "EMPTY" is the
		/// next element returned by the stream.</returns>
		private  Coordinates GetCoordinates(WktStreamTokenizer tokenizer) 
		{
			Coordinates coordinates= new Coordinates();
			string nextToken = GetNextEmptyOrOpener(tokenizer);
			if (nextToken=="EMPTY")
			{
				return  coordinates;
			}
			Coordinate externalCoordinate = new Coordinate();
			Coordinate internalCoordinate = new Coordinate();
			externalCoordinate.X = GetNextNumber(tokenizer);
			externalCoordinate.Y = GetNextNumber(tokenizer);
			_precisionModel.ToInternal(externalCoordinate, internalCoordinate);
			coordinates.Add(internalCoordinate);
			nextToken = GetNextCloserOrComma(tokenizer);
			while (nextToken==",")
			{
				externalCoordinate.X = this.GetNextNumber(tokenizer);
				externalCoordinate.Y = this.GetNextNumber(tokenizer);
				internalCoordinate = new Coordinate();
				_precisionModel.ToInternal(externalCoordinate, internalCoordinate);
				coordinates.Add(internalCoordinate);
				nextToken = GetNextCloserOrComma(tokenizer);
			}
			return coordinates;
			//Coordinate[] array = new Coordinate[coordinates.Count];
			//return (Coordinate[]) coordinates.ToArray(typeof(Coordinate));
		}

		/// <summary>
		/// Returns the next number in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known text format.  The next token
		/// must be a number.</param>
		/// <returns>Returns the next number in the stream.</returns>
		/// <remarks>
		/// ParseException is thrown if the next token is not a number.
		/// </remarks>
		private   double GetNextNumber(WktStreamTokenizer tokenizer) 
		{
			tokenizer.NextToken();
			return tokenizer.GetNumericValue();
		}

		/// <summary>
		/// Returns the next "EMPTY" or "(" in the stream as uppercase text.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text
		/// format. The next token must be "EMPTY" or "(".</param>
		/// <returns>the next "EMPTY" or "(" in the stream as uppercase
		/// text.</returns>
		/// <remarks>
		/// ParseException is thrown if the next token is not "EMPTY" or "(".
		/// </remarks>
		private   string GetNextEmptyOrOpener(WktStreamTokenizer tokenizer)
		{
			tokenizer.NextToken();
			string nextWord = tokenizer.GetStringValue();
			if (nextWord=="EMPTY" || nextWord=="(")
			{
				return nextWord;
			}
			throw new ParseException("Expected 'EMPTY' or '(' but encountered '" +	nextWord + "'");

		}

		/// <summary>
		/// Returns the next ")" or "," in the stream.
		/// </summary>
		/// <param name="tokenizer">tokenizer over a stream of text in Well-known Text
		/// format. The next token must be ")" or ",".</param>
		/// <returns>Returns the next ")" or "," in the stream.</returns>
		/// <remarks>
		/// ParseException is thrown if the next token is not ")" or ",".
		/// </remarks>
		private   string GetNextCloserOrComma(WktStreamTokenizer tokenizer)
		{
			tokenizer.NextToken();
			string nextWord = tokenizer.GetStringValue();
			if (nextWord=="," || nextWord==")")
			{
				return nextWord;
			}
			throw new ParseException("Expected ')' or ',' but encountered '" + nextWord	+ "'");
	
		}

		/// <summary>
		/// Returns the next ")" in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text
		/// format. The next token must be ")".</param>
		/// <returns>Returns the next ")" in the stream.</returns>
		/// <remarks>
		/// ParseException is thrown if the next token is not ")".
		/// </remarks>
		private string GetNextCloser(WktStreamTokenizer tokenizer)
		{
			
			string nextWord = GetNextWord(tokenizer);
			if (nextWord==")")
			{
				return nextWord;
			}
			throw new ParseException("Expected ')' but encountered '" + nextWord + "'");
		}

		/// <summary>
		/// Returns the next word in the stream as uppercase text.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text
		/// format. The next token must be a word.</param>
		/// <returns>Returns the next word in the stream as uppercase text.</returns>
		/// <remarks>
		/// ParseException is thrown if the next token is not a word.
		/// </remarks>
		private   string GetNextWord(WktStreamTokenizer tokenizer)
		{
			TokenType type = tokenizer.NextToken();
			string token = tokenizer.GetStringValue();
			if (type==TokenType.Number)
			{
				throw new ParseException("Expected  a number but got "+token);
			}
			else if (type==TokenType.Word)
			{
				return token.ToUpper();
			}
			else if (token=="(")
			{
				return "(";
			}
			else if (token==")")
			{
				return ")";
			}
			else if (token==",")
			{
				return ",";
			}
			
			throw new ParseException("Not a valid symbol in WKT format.");
		}


		/// <summary>
		/// Creates a Geometry using the next token in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text
		/// format. The next tokens must form a &lt;Geometry Tagged Text&gt;.</param>
		/// <returns>Returns a Geometry specified by the next token in the stream.</returns>
		/// <remarks>
		/// ParseException is thrown if the coordinates used to create a Polygon
		/// shell and holes do not form closed linestrings, or if an unexpected
		/// token is encountered.
		/// </remarks>
		private   Geometry ReadGeometryTaggedText(WktStreamTokenizer tokenizer)
		{
			tokenizer.NextToken();
			string type = tokenizer.GetStringValue().ToUpper();
			Geometry geometry= null;
			switch (type)
			{
				case "POINT":
					geometry =  ReadPointText(tokenizer);
					break;
				case "LINESTRING":
					geometry = ReadLineStringText(tokenizer);
					break;	
				case "MULTIPOINT":
					geometry = this.ReadMultiPointText(tokenizer);
					break;	
				case "MULTILINESTRING":
					geometry = this.ReadMultiLineStringText(tokenizer);
					break;
				case "POLYGON":
					geometry = ReadPolygonText(tokenizer);
					break;	
				case "MULTIPOLYGON":
					geometry = ReadMultiPolygonText(tokenizer);
					break;
				case "GEOMETRYCOLLECTION":
					geometry = ReadGeometryCollectionText(tokenizer);
					break;
				default:
					throw new ParseException(String.Format("{0} is not WKT.",type));
			}
			return geometry;
			/*
			 *  String type = getNextWord(tokenizer);
    if (type.equals("POINT")) {
      return readPointText(tokenizer);
    }
    else if (type.equals("LINESTRING")) {
      return readLineStringText(tokenizer);
    }
    else if (type.equals("POLYGON")) {
      return readPolygonText(tokenizer);
    }
    else if (type.equals("MULTIPOINT")) {
      return readMultiPointText(tokenizer);
    }
    else if (type.equals("MULTILINESTRING")) {
      return readMultiLineStringText(tokenizer);
    }
    else if (type.equals("MULTIPOLYGON")) {
      return readMultiPolygonText(tokenizer);
    }
    else if (type.equals("GEOMETRYCOLLECTION")) {
      return readGeometryCollectionText(tokenizer);
    }*/
			
		}

		/// <summary>
		/// Creates a Point using the next token in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text
		/// format. The next tokens must form a &lt;Point Text&gt;.</param>
		/// <returns>Returns a Point specified by the next token in
		/// the stream.</returns>
		/// <remarks>
		/// ParseException is thrown if an unexpected token is encountered.
		/// </remarks>
		private   Point ReadPointText(WktStreamTokenizer tokenizer)
		{
			string nextToken = GetNextEmptyOrOpener(tokenizer);
			if (nextToken=="EMPTY")
			{
				return _geometryFactory.CreatePoint(null);
			}
			
			double x = GetNextNumber(tokenizer);
			double y = GetNextNumber(tokenizer);
			Coordinate externalCoordinate = new Coordinate(x, y);
			Coordinate internalCoordinate = new Coordinate();
			_precisionModel.ToInternal(externalCoordinate, internalCoordinate);
			GetNextCloser(tokenizer);
			return _geometryFactory.CreatePoint(internalCoordinate);
		}

		/// <summary>
		/// Creates a LineString using the next token in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text format.  The next
		/// tokens must form a LineString Text.</param>
		/// <returns>Returns a LineString specified by the next token in the stream.</returns>
		/// <remarks>
		/// ParseException is thrown if an unexpected token is encountered.
		/// </remarks>
		private LineString ReadLineStringText(WktStreamTokenizer tokenizer) 
		{
			return _geometryFactory.CreateLineString(GetCoordinates(tokenizer));
		}

		/// <summary>
		///  Creates a LinearRing using the next token in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text
		///  format. The next tokens must form a &lt;LineString Text&gt;.</param>
		/// <returns>Returns a LinearRing specified by the next token in the stream.</returns>
		/// <remarks>
		/// ParseException is thrown if the coordinates used to create the LinearRing
		/// do not form a closed linestring, or if an unexpected token is encountered.
		/// </remarks>
		private   LinearRing ReadLinearRingText(WktStreamTokenizer tokenizer)
		{
			return _geometryFactory.CreateLinearRing(GetCoordinates(tokenizer));
		}

		/// <summary>
		/// Creates a MultiPoint using the next token in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text
		/// format. The next tokens must form a &lt;MultiPoint Text&gt;.</param>
		/// <returns>Returns a MultiPoint specified by the next token in the stream.</returns>
		/// <remarks>
		/// ParseException is thrown if an unexpected token is encountered
		/// </remarks>
		private   MultiPoint ReadMultiPointText(WktStreamTokenizer tokenizer)
		{
			return _geometryFactory.CreateMultiPoint(GetCoordinates(tokenizer));
		}

/*
		/// <summary>
		/// Creates an array of Points have the given Coordinates.
		/// </summary>
		/// <param name="coordinates">The Coordinates with which to create the Points.</param>
		/// <returns>Returns a Point array created using this GeometryWKTReader's GeometryFactory.</returns>
		private   ArrayList ToPoints(ArrayList coordinates) 
		{
			
			ArrayList points = new ArrayList();
			for (int i = 0; i < coordinates.length; i++) {
			points.add(geometryFactory.createPoint(coordinates[i]));
			}
			return (Point[]) points.ToArray(new Point[]{});
			
			throw new NotImplementedException();
		}
*/
		/// <summary>
		/// Creates a Polygon using the next token in the stream.
		/// </summary>
		/// <param name="tokenizer">Tokenizer over a stream of text in Well-known Text
		///  format. The next tokens must form a &lt;Polygon Text&gt;.</param>
		/// <returns>Returns a Polygon specified by the next token
		///  in the stream</returns>
		///  <remarks>
		///  ParseException is thown if the coordinates used to create the Polygon
		///  shell and holes do not form closed linestrings, or if an unexpected
		///  token is encountered.
		///  </remarks>
		private   Polygon ReadPolygonText(WktStreamTokenizer tokenizer)
		{
			
			string nextToken = GetNextEmptyOrOpener(tokenizer);
			if (nextToken=="EMPTY")
			{
				//return geometryFactory.createPolygon(geometryFactory.createLinearRing(
				//	new Coordinate[]{}), new LinearRing[]{});
				LinearRing linearRing = new LinearRing(new Coordinates(),_precisionModel,_geometryFactory.SRID);
				return new Polygon(linearRing,_precisionModel, _geometryFactory.SRID);
			}
			ArrayList holes = new ArrayList();
			LinearRing shell = ReadLinearRingText(tokenizer);
			nextToken = GetNextCloserOrComma(tokenizer);
			while (nextToken==",")
			{
				LinearRing hole = ReadLinearRingText(tokenizer);
				holes.Add(hole);
				nextToken = GetNextCloserOrComma(tokenizer);
			}
			LinearRing[] array = new LinearRing[holes.Count];
			return _geometryFactory.CreatePolygon(shell, (LinearRing[])holes.ToArray(typeof(LinearRing)));
		}

		/**
		*  Creates a <code>MultiLineString</code> using the next token in the stream.
		*
		*@param  tokenizer        tokenizer over a stream of text in Well-known Text
		*      format. The next tokens must form a &lt;MultiLineString Text&gt;.
		*@return                  a <code>MultiLineString</code> specified by the
		*      next token in the stream
		*@throws  IOException     if an I/O error occurs
		*@throws  ParseException  if an unexpected token was encountered
		*/
		private   MultiLineString ReadMultiLineStringText(WktStreamTokenizer tokenizer) 
		{
			
			string nextToken = GetNextEmptyOrOpener(tokenizer);
			if (nextToken=="EMPTY")
			{
				//return geometryFactory.createMultiLineString(new LineString[]{});
				return new MultiLineString(new LineString[]{},_precisionModel, _geometryFactory.SRID);
			}
			ArrayList lineStrings = new ArrayList();
			LineString lineString = ReadLineStringText(tokenizer);
			lineStrings.Add(lineString);
			nextToken = GetNextCloserOrComma(tokenizer);
			while (nextToken==",")
			{
				lineString = ReadLineStringText(tokenizer);
				lineStrings.Add(lineString);
				nextToken = GetNextCloserOrComma(tokenizer);
			}
			LineString[] array = new LineString[lineStrings.Count];
			return _geometryFactory.CreateMultiLineString((LineString[]) lineStrings.ToArray(typeof(LineString)));
		}

		/**
		*  Creates a <code>MultiPolygon</code> using the next token in the stream.
		*
		*@param  tokenizer        tokenizer over a stream of text in Well-known Text
		*      format. The next tokens must form a &lt;MultiPolygon Text&gt;.
		*@return                  a <code>MultiPolygon</code> specified by the next
		*      token in the stream, or if if the coordinates used to create the
		*      <code>Polygon</code> shells and holes do not form closed linestrings.
		*@throws  IOException     if an I/O error occurs
		*@throws  ParseException  if an unexpected token was encountered
		*/
		private   MultiPolygon ReadMultiPolygonText(WktStreamTokenizer tokenizer) 
		{
			
			string nextToken = GetNextEmptyOrOpener(tokenizer);
			if (nextToken=="EMPTY")
			{
				return _geometryFactory.CreateMultiPolygon(new Polygon[]{});
			}
			ArrayList polygons = new ArrayList();
			Polygon polygon = ReadPolygonText(tokenizer);
			polygons.Add(polygon);
			nextToken = GetNextCloserOrComma(tokenizer);
			while (nextToken==",") {
			polygon = ReadPolygonText(tokenizer);
			polygons.Add(polygon);
			nextToken = GetNextCloserOrComma(tokenizer);
			}
			Polygon[] array = new Polygon[polygons.Count];
			return _geometryFactory.CreateMultiPolygon((Polygon[]) polygons.ToArray(typeof(Polygon)));
		}

		/**
		*  Creates a <code>GeometryCollection</code> using the next token in the
		*  stream.
		*
		*@param  tokenizer        tokenizer over a stream of text in Well-known Text
		*      format. The next tokens must form a &lt;GeometryCollection Text&gt;.
		*@return                  a <code>GeometryCollection</code> specified by the
		*      next token in the stream
		*@throws  ParseException  if the coordinates used to create a <code>Polygon</code>
		*      shell and holes do not form closed linestrings, or if an unexpected
		*      token was encountered
		*@throws  IOException     if an I/O error occurs
		*/
		private   GeometryCollection ReadGeometryCollectionText(WktStreamTokenizer tokenizer) 
		{
			string nextToken = GetNextEmptyOrOpener(tokenizer);
			if (nextToken=="EMPTY")
			{
				return _geometryFactory.CreateGeometryCollection(new Geometry[]{});
			}
			ArrayList geometries = new ArrayList();
			Geometry geometry = ReadGeometryTaggedText(tokenizer);
			geometries.Add(geometry);
			nextToken = GetNextCloserOrComma(tokenizer);
			while (nextToken==",")
			{
				geometry = ReadGeometryTaggedText(tokenizer);
				geometries.Add(geometry);
				nextToken = GetNextCloserOrComma(tokenizer);
			}
			Geometry[] array = new Geometry[geometries.Count];
			return _geometryFactory.CreateGeometryCollection((Geometry[]) geometries.ToArray(typeof(Geometry)));
		}
		
		#endregion

	}
}
