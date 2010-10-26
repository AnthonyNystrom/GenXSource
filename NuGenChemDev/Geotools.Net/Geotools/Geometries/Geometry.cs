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

#region Using statements
using System;
using System.Collections;
using Geotools.Algorithms;
using Geotools.IO;
using Geotools.Operation;
using Geotools.Operation.Overlay;
using Geotools.Operation.Buffer;
using Geotools.Operation.Relate;
using Geotools.Operation.Valid;
using Geotools.Operation.Distance;
using Geotools.Utilities;
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A set of points. 
	/// </summary>
	/// <remarks>
	/// <para>The spatial relationship predicates (like disjoint) 
	/// are based on the Dimensionally Extended Nine-Intersection Model 
	/// (DE-9IM). For a description of the DE-9IM, see the OpenGIS Simple 
	/// Features Specification (SFS) for SQL</para>
	/// <para>A Precision Model object is a member of every Geometry object</para>. 
	/// 
	/// <para>The SFS specifies that objects of each Geometry subclass may be empty. 
	/// It is sometimes necessary to construct a generic empty object of class Geometry 
	/// (e.g. if the exact type of the Geometry to be returned is not known). The SFS 
	/// does not define a specific class or object to represent a generic empty Geometry. 
	/// JTS uses the convention that an empty GeometryCollection will be returned.</para>
	/// 
	/// <para><B>Binary Predicates</B></para>
	/// 
	/// <para>The binary predicates can be completely specified in terms of an IntersectionMatrix 
	/// pattern. In fact, their implementation is simply a call to relate with the appropriate pattern.</para>
	/// 
	/// <para>It is important to note that binary predicates are topological operations rather 
	/// than pointwise operations. Even for apparently straightforward predicates such as equals 
	/// topoloty, it is easy to find cases where a pointwise comparison does not produce the same 
	/// result as a topological comparison. (for instance: A and B are MultiPoints with the same 
	/// point repeated different numbers of times; A is a LineString with two collinear line 
	/// segments and B is a single line segment with the same start and endpoints). The algorithm 
	/// used for the relate method is a topology-based algorithm which produces a topologically 
	/// correct result.</para>
	/// 
	/// <para>As in the SFS, the term P is used to refer to 0-dimensional Geometrys (Point and 
	/// MultiPoint), L to 1-dimensional Geometrys ( LineString, and MultiLineString ), and A to 
	/// 2-dimensional Geometrys (Polygon and MultiPolygon). The dimension of a GeometryCollection 
	/// is equal to the maximum dimension of its components.</para>
	/// 
	/// <para>In the SFS some binary predicates are stated to be undefined for some combinations of 
	/// dimensions (e.g. touches is undefined for P /P ). In the interests of simplifying the API, 
	/// combinations of argument Geometrys which are not in the domain of a predicate will return 
	/// false (e.g. touches(Point, Point) => false).</para>
	/// 
	/// <para>If either argument to a predicate is an empty Geometry the predicate will return false.</para>
	/// 
	/// <para><B>Set-Theoretic Methods</B></para>
	/// 
	/// <para>For certain inputs, the difference and symDifference methods may compute non-closed 
	/// sets. This can happen when the arguments overlap and have different dimensions. Since JTS 
	/// Geometry objects can represent only closed sets, the spatial analysis methods are specified 
	/// to return the closure of the point-set-theoretic result.</para>
	/// </remarks>
	public abstract class Geometry : IGeometry, IComparable
	{	
		protected static CGAlgorithms _cgAlgorithms =  new RobustCGAlgorithms();
		protected PrecisionModel _precisionModel;
		protected int _SRID;
		protected Envelope _envelope;
		protected GeometryFactory _geometryFactory;
		protected string[] _sortedClasses = new string[]{	"Point",
														  "MultiPoint",
														  "LineString",
														  "LinearRing",
														  "MultiLineString",
														  "Polygon",
														  "MultiPolygon",
														  "GeometryCollection" };
		InternalGeometryComponentFilter _geometryChangedFilter = new InternalGeometryComponentFilter();

		#region Internal class
		private class InternalGeometryComponentFilter : IGeometryComponentFilter
		{
			public void Filter(Geometry geom)
			{
				geom.GeometryChangedAction();
			}
		}
		#endregion

		#region Constructors
		///<summary>
		///Initializes aGeometry.  
		///</summary>
		///<param name="precisionModel">The specification of the grid of allowable points  for this Geometry.</param>
		///<param name="SRID">The ID of the Spatial Reference System used by this Geometry.</param>
		internal Geometry(PrecisionModel precisionModel, int SRID) 
		{
			this._precisionModel = precisionModel;
			this._SRID = SRID;
			this._geometryFactory = new GeometryFactory(_precisionModel, _SRID);
		}
		#endregion

		#region Properties

		/// <summary>
		/// Algorithms for computational geometry //do we want to do this...
		/// </summary>
		public CGAlgorithms CGAlgorithms
		{
			get
			{
				if ( _cgAlgorithms == null )
				{
					_cgAlgorithms = new RobustCGAlgorithms();
				}
				return _cgAlgorithms;
			}
		}
		#endregion

		/// <summary>
		/// The specification of the grid of allowable points for this Geometry.
		/// </summary>
		public PrecisionModel GetPrecisionModel()
		{
			return _precisionModel;
		}

		public PrecisionModel PrecisionModel
		{
			get
			{
				return _precisionModel;
			}
		}

		///<summary>
		///  The ID of the Spatial Reference System used by this Geometry.
		///</summary>
		public int GetSRID()
		{
			return _SRID;
		}

		/// <summary>
		/// Sets the ID of the Spatial Reference System used by the Geometry.
		/// </summary>
		/// <param name="SRID">The SRID to set to this geometry.</param>
		public void SetSRID(int SRID) 
		{
			_SRID = SRID;
		}
	
		///<summary>
		///  Returns this Geometrys bounding box.
		///</summary>
		///<remarks>If this Geometry  is the empty geometry, returns an empty Point.
		///  If the Geometry  is a point, returns a non-empty Point. Otherwise, returns a  Polygon whose 
		///  points are (minx, miny), (maxx, miny), (maxx,  maxy), (minx, maxy), (minx, miny).</remarks>
		///<returns>
		///Returns an empty Point (for empty Geometrys), a  Point (for Points) or a Polygon  
		///(in all other cases)
		///</returns>
		public virtual Geometry GetEnvelope() 
		{
			return GeometryFactory.ToGeometry( GetEnvelopeInternal(), _precisionModel, _SRID );
		} // public Geometry Envelope()

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public abstract string GetGeometryType();

		///<summary>
		/// Returns true if the array contains any non-empty Geometrys.  
		///</summary>
		///<param name="array">An Array of geometries.</param>
		///<returns>
		///Returns true if any of the geometries in the list's IsEmpty method return false.
		///</returns>
		protected static bool HasNonEmptyElements(IEnumerable array) 
		{
			Geometry geometry;
			foreach(object obj in array)
			{
				geometry = obj as Geometry;
				if ( geometry != null && !geometry.IsEmpty() ) 
				{
					return true;
				}
			}
			return false;
		}

		///<summary>
		///  Returns true if the array contains any null elements.  
		///</summary>
		///<param name="array">An array to validate.</param>
		///<returns>Returns true if any of arrays elements are null</returns>
		protected static bool HasNullElements(IEnumerable array) 
		{
			if(array != null)
			{
				foreach(object obj in array)
				{
					if (obj==null)
					{
						return true;
					}
				}
			}
			return false;
		}

		///<summary>
		///  Flips the positions of the elements in the array so that the last is first.  
		///</summary>
		///<param name="coordinates">The array of coordinates to rearrange.</param>
		protected static void ReversePointOrder(Coordinates coordinates) 
		{
			coordinates.Reverse();
		}

		///<summary>
		/// Returns the minimum coordinate, using the usual lexicographic comparison.  
		///</summary>
		///<param name="coordinates">The array to search for the minimum coordinate.</param>
		///<returns> the minimum coordinate in the array, found using CompareTo method.  Returns
		///null if coordinates is null or empty.</returns>
		protected static Coordinate MinCoordinate(ArrayList coordinates) 
		{
			Coordinate coord = null;
			if ( coordinates != null && coordinates.Count > 0 )
			{
				CoordinateCompare coordCompare = new CoordinateCompare();
				coordinates.Sort( coordCompare );
				coord =  (Coordinate) coordinates[0];
			}
			return coord;
		}

		///<summary>
		/// Shifts the positions of the coordinates until firstCoordinate is first.  
		///</summary>
		///<param name="coordinates">The array to rearrange.</param>
		///<param name="firstCoordinate">The coordinate to make first.</param>
		protected static void Scroll(ArrayList coordinates, Coordinate firstCoordinate) 
		{
			int index = coordinates.IndexOf( firstCoordinate );
			if ( index > -1)
			{
				Coordinate[] newCoordinates = new Coordinate[ coordinates.Count ];
				coordinates.CopyTo( index, newCoordinates, 0, coordinates.Count - index );	// copies from index to end
				coordinates.CopyTo( 0, newCoordinates, coordinates.Count - index, index );		// copies from 0 to index
				coordinates.Clear();  // now clear array to refill with scrolled array.
				coordinates.AddRange( newCoordinates ); // add newCoordinates to coordinates array
			}
			else
			{
				throw new ArgumentException("firstCoordinate not found in ArrayList", "firstCoordinate");
			}
		}

		///<summary>
		/// Returns the index of coordinate in coordinates.  
		///</summary>
		///<remarks>The first position is 0; the second, 1; etc.</remarks>
		///<param name="coordinate">The Coordinate to search in the array.</param>
		///<param name="coordinates">The array to search </param>
		///<returns>Returns the position of coordinate, or -1 if it is  not found</returns>
		protected static int IndexOf(Coordinate coordinate, ArrayList coordinates)	//TODO: I don't think this is needed because IndexOf for ArrayList will do this.
		{
			for (int i = 0; i < coordinates.Count; i++) 
			{
				if (coordinate.Equals(coordinates[i])) 
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Returns a vertex of this Geometry.
		/// </summary>
		/// <returns>Returns a Coordinate which is a vertex of this Geometry.  Returns null if this Geometry is empty.</returns>
		public abstract Coordinate GetCoordinate();

		///<summary>
		///  Returns this Geometry's internal vertices.
		///</summary>
		///<remarks>If you modify the coordinates in this array, be sure to call
		///  GeometryChanged afterwards.  The Geometries contained by composite Geometries must be Geometry's;
		///  that is, they must implement get Coordinates.</remarks>
		///<returns>Returns the vertices of this Geometry</returns>
		public abstract Coordinates GetCoordinates() ;

		/// <summary>
		/// Returns this Geometry's external vertices (points).
		/// </summary>
		/// <remarks>These point are based on the output of precisionModel.ToExternal.</remarks>
		/// <returns>Returns the external vertices of this Geometry.</returns>
		public virtual Coordinates GetCoordinatesInternal()
		{
			Coordinates externalCoordinates = new Coordinates();
			Coordinates internalCoordinates = GetCoordinates();
			for ( int i=0; i < internalCoordinates.Count; i++ )
			{
				externalCoordinates.Add( _precisionModel.ToExternal( internalCoordinates[i] ) );			// creates a new Coordinate() in the process.
			}
			return externalCoordinates;
		} //public virtual Coordinates GetCoordinatesInternal()
	
		///<summary>
		///  Returns the count of this Geometrys vertices.
		///</summary>
		///<remarks>The Geometry  s contained by composite
		///  Geometrys must be  Geometry's; that is, they must implement get NumPoints.</remarks>
		///<returns>Returns the number of vertices in this Geometry</returns>
		public abstract int GetNumPoints();

		///<summary>
		///  Returns false if the Geometry not simple.  
		///</summary>
		///<remarks>Subclasses provide their own definition of "simple". 
		///  If  this Geometry is empty, returns true. In general, the OpenGIS specifications of simplicity 
		///  seem to follow the  following rule: A Geometry is simple iff the only self-intersections
		///  are at  boundary points and for all empty Geometrys, isSimple = true.
		///</remarks>
		///<returns>
		/// Returns true if this Geometry has any points of  self-tangency, self-intersection or other
		/// anomalous points
		///</returns>
		public abstract bool IsSimple();

		///<summary>
		///  Returns false if the Geometry is invlaid.  
		///</summary>
		///<remarks>Subclasses provide their own definition of "valid". 
		///  If this Geometry is empty, returns true.</remarks>
		///<returns>Returns true if this Geometry is valid.</returns>
		public virtual bool IsValid()
		{
			IsValidOp isValidOp = new IsValidOp(this);
			return isValidOp.IsValid();
		}

		///<summary>
		///  Returns string containing error message if IsValid() returns false or OK if no error exists.  
		///</summary>
		///<returns>Returns string corresponding to error message if IsValid fails.</returns>
		public virtual string IsValidErrorMessage()
		{
			TopologyValidationError er = GetValidOpError();
			string errorMessage = "OK";
			if ( er != null )
				errorMessage = er.ToString();

			return errorMessage;
		}

		///<summary>
		///  Returns sTopologyValidationError object available if IsValid() returns false or null if no error exists.  
		///</summary>
		///<returns>Returns string corresponding to error message if IsValid fails.</returns>
		public virtual TopologyValidationError GetValidOpError()
		{
			IsValidOp isValidOp = new IsValidOp(this);
			return isValidOp.GetValidationError();
		}

		///<summary>
		///  Returns whether or not the set of points in this Geometry is  empty.  
		///</summary>
		///<returns>Returns true if this Geometry equals the empty  geometry</returns>
		public abstract bool IsEmpty();

		/// <summary>
		/// Returns the minimum distance between this Geometry and the Geometry g.
		/// </summary>
		/// <param name="g">The Geometry from which to compute the distance.</param>
		/// <returns>Returns the minimum distance between this Geometry and the Geometry g.</returns>
		public double Distance(Geometry g)
		{
			return DistanceOp.Distance(this, g);
		}

		/// <summary>
		/// Returns the area of this Geometry.  Areal Geometrys have a non-zero area.  They override this function to
		/// compute the area, others return 0.0.
		/// </summary>
		/// <returns>Returns the area of the Geometry.</returns>
		public virtual double GetArea()
		{
			return 0.0;
		}

		/// <summary>
		/// Returns the length of this Geometry.
		/// </summary>
		/// <remarks>Linear geometries return their length.  Areal geometries return their
		///		perimeter.  They override this function to compute the Length.  Others return 0.0.
		/// </remarks>
		/// <returns>Returns the length of this Geometry.</returns>
		public virtual double GetLength()
		{
			return 0.0;
		}

		///<summary>
		///  Returns the dimension of this Geometry.  
		///</summary>
		///<returns>Returns the dimension of the class implementing this interface, whether  or not this 
		/// object is the empty geometry</returns>
		public abstract int GetDimension();

		///<summary>
		///  Returns the boundary, or the empty geometry if this Geometry  is empty.  
		///</summary>
		///<remarks>For a discussion
		///  of this function, see the OpenGIS Simple Features Specification. As stated in SFS 
		///  Section 2.1.13.1, "the boundary  of a Geometry is a set of Geometries of the next lower dimension."
		///  </remarks>
		///<returns>Returns the closure of the combinatorial boundary of this Geometry</returns>
		public abstract Geometry GetBoundary();

		///<summary>
		///  Returns the dimension of this Geometrys inherent boundary.  
		///</summary>
		///<returns>
		/// Returns the dimension of the boundary of the class implementing this interface, 
		/// whether or not this object is the empty geometry. Returns Dimension.FALSE if the boundary
		/// is the empty geometry.
		/// </returns>
		public abstract int GetBoundaryDimension();	

		///<summary>
		///  Returns the minimum and maximum x and y values in this Geometry  , or a null Envelope if this
		///  Geometry is empty.  
		///</summary>
		///<returns>Returns this Geometry's bounding box.  If the Geometry is empty, Envelope is null will return null.</returns>
		public virtual Envelope GetEnvelopeInternal() 
		{
			if ( _envelope == null ) 
			{
				_envelope = ComputeEnvelopeInternal();
			}
			return _envelope;
		}

		/// <summary>
		/// Notifies this Geometry that it's Coordinates have been changed by an external party
		/// (using a CoordinateFilter, for example).
		/// </summary>
		/// <remarks>The Geometry will flush and/or update any
		/// information it has chached (such as it's envelope).</remarks>
		public void GeometryChanged() 
		{
			/*if (this is GeometryCollection)
			{
				GeometryChangedAction();
			}
			else if (this is Polygon)
			{
				GeometryChangedAction();
			}*/
			Apply( _geometryChangedFilter );
		}

		/// <summary>
		/// Notifies this Geometry that its Coordinates have been changed by an external party.
		/// </summary>
		/// <remarks>
		/// When GeometryChanged is called, this method will be called for this Geometry and it's component Geometries.
		/// </remarks>
		protected void GeometryChangedAction() 
		{
			_envelope = null;
		}


		///<summary>
		///  Returns true if the DE-9IM intersection matrix for the two Geometrys is FF*FF****.  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>Returns true if the two Geometrys are  disjoint, false otherwise.</returns>
		public virtual bool Disjoint(Geometry geometry) 
		{
			return Relate(geometry).IsDisjoint();
		}

		///<summary>
		///  Returns true if the DE-9IM intersection matrix for the two
		///  Geometrys is FT*******, F**T***** or F***T****.  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>Returns true if the two Geometrys touch;  Returns false if both Geometrys are points.</returns>
		public virtual bool Touches(Geometry geometry) 
		{
			return Relate(geometry).IsTouches( GetDimension(), geometry.GetDimension() );
		}

		///<summary>
		/// Determines if this geometry intersects the input geometry.  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>Returns true if the two Geometrys intersect.</returns>
		public virtual bool Intersects(Geometry geometry) 
		{
			return Relate(geometry).IsIntersects();
		}

		///<summary>
		///  Returns true if the DE-9IM intersection matrix for the two Geometrys is: T*T****** for a point
		///  and a curve, a point and an area or a line  and an area, 0******** for two curves.  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>Returns true if the two Geometrys cross.  For this function to return true, the Geometry's
		/// must be a point and a curve; a point and a surface; two curves; or a  curve and a surface.</returns>
		public virtual bool Crosses(Geometry geometry) 
		{
			return Relate(geometry).IsCrosses( GetDimension(), geometry.GetDimension() );
		}

		///<summary>
		/// Returns true if the DE-9IM intersection matrix for the two  Geometrys is T*F**F***.  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>Returns true if this Geometry is within other geometry.</returns>
		public virtual bool Within(Geometry geometry) 
		{
			return Relate(geometry).IsWithin();
		}

		///<summary>
		/// Returns true if this geometry contains the input geometry.
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>Returns true if this Geometry contains other.</returns>
		public virtual bool Contains(Geometry geometry) 
		{
			return Relate(geometry).IsContains();
		}

		///<summary>
		///  Returns true if the DE-9IM intersection matrix for the two  Geometrys is: &lt;br&gt;
		///		T*T***T** (for two points or two surfaces) &lt;br&gt;
		///		1*T***T** (for two curves)  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>
		/// Returns true if the two Geometrys overlap.  For this function to return true, 
		/// the Geometry  s must be two points, two curves or two surfaces.
		///</returns>
		public virtual bool Overlaps(Geometry geometry) 
		{
			return Relate(geometry).IsOverlaps( GetDimension(), geometry.GetDimension() );
		}
	


		///<summary>
		///  Returns true if the elements in the DE-9IM intersection matrix for the two Geometrys
		///  match the elements in intersectionPattern.
		///</summary>
		///<remarks>
		///	These may be:  
		///	<list type="bullet">
		///		<item><term>0</term></item>
		///		<item><term>1</term></item>
		///		<item><term>2</term></item>
		///		<item><term>T ( = 0, 1 or 2)</term></item>
		///		<item><term>F ( = -1)</term></item>
		///		<item><term>* ( = -1, 0, 1 or 2)</term></item>
		///	</list>
		///	 For more information on the DE-9IM, see the OpenGIS Simple Features Specification.  
		///</remarks>
		///<param name="geometry">The Geometry with which to compare  this Geometry.</param>
		///<param name="intersectionPattern">The pattern against which to check the intersection matrix for
		/// the two Geometrys.</param>
		///<returns>
		///Returns true if the DE-9IM intersection matrix for the two Geometrys 
		///match intersectionPattern.
		///</returns>
		public virtual bool Relate( Geometry geometry, string intersectionPattern ) 
		{
			return Relate(geometry).Matches( intersectionPattern );
		}

		///<summary>
		/// Returns the DE-9IM intersection matrix for the two Geometrys.  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>
		///Returns a matrix describing the intersections of the interiors,  boundaries and 
		///exteriors of the two Geometrys
		///</returns>
		public virtual IntersectionMatrix Relate(Geometry geometry) 
		{
			CheckNotGeometryCollection(this);
			CheckNotGeometryCollection(geometry);
			CheckEqualSRID(geometry);
			CheckEqualPrecisionModel(geometry);
			return RelateOp.Relate(this, geometry);
		}

		///<summary>
		/// Returns true if the DE-9IM intersection matrix for the two Geometrys is T*F**FFF*.  
		///</summary>
		///<remarks>EqualsTopology is a topological relationship, and does not imply that the Geometries have
		///the same points or even that they are of the same class.  This more restrictive form of equality
		///is implemented in the Equals method.  Two geometries are topologically equal if and only iff their interiors
		///intersect and no part of the interior or boundary of one geomtry intersects the exterior of the other.
		///</remarks>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<returns>Returns true if the two Geometrys are equal.</returns>
		public virtual bool EqualsTopology(Geometry geometry)
		{
			return Relate( geometry ).IsEquals( GetDimension(), geometry.GetDimension() );
		} // public virtual bool Equals(Geometry geometry)

		/// <summary>
		/// Converts to the string representation of this object.
		/// </summary>
		/// <returns>Returns the string representation of this object.</returns>
		public override string ToString() 
		{
			return ToText();
		}

		///<summary>
		///  Returns the Well-known Text representation of this Geometry.
		///</summary>
		///<remarks>
		///	For a definition of the Well-known Text format, see the OpenGIS Simple Features Specification.
		///</remarks>
		///<returns>Returns the Well-known Text representation of this Geometry</returns>
		public virtual string ToText() 
		{
			GeometryWKTWriter writer = new GeometryWKTWriter();
			return writer.WriteFormatted(this);
		}

		
		///<summary>
		/// Returns a buffer region around this Geometry having the given width.
		/// The buffer of a Geometry is the Minkowski sum of the Geometry with
		/// a disc of radius distance.  
		///</summary>
		///<param name="distance">The width of the buffer, interpreted according to the  PrecisionModel of the
		///Geometry.</param>
		///<returns>
		///Returns all points whose distance from this Geometry are less than or equal to distance.
		///</returns>
		public virtual Geometry Buffer(double distance) 
		{
			return BufferOp.GetBuffer(this, distance);
		}

		///<summary>
		///  Returns the smallest convex Polygon that contains all the  points in the Geometry.
		/// </summary>
		/// <remarks>This obviously applies only to Geometry's which contain 3 or more points; 
		/// the results for degenerate cases are specified as follows:
		///	<list type="table">
		///		<listheader><term>Number of Points in argument Geometry</term><description>Geometry class of result</description></listheader>
		///			<item><term>0</term><description>Empty GeometryCollection</description></item>
		///			<item><term>1</term><description>Point</description></item>
		///			<item><term>2</term><description>LineString</description></item>
		///			<item><term>3 or more</term><description>Polygon</description></item>
		///	</list>
		///	</remarks>
		///<returns>Returns the minimum-area convex polygon containing this Geometry's points.</returns>
		public virtual Geometry ConvexHull() 
		{
			return (new ConvexHull(_cgAlgorithms)).GetConvexHull(this);
		}

		///<summary>
		/// Returns a Geometry representing the points shared by this Geometry and other.  
		///</summary>
		///<param name="geometry">The Geometry with which to compute the intersection.</param>
		///<returns>Returns the points common to the two Geometrys.</returns>
		public virtual Geometry Intersection(Geometry geometry) 
		{
			CheckNotGeometryCollection(this);
			CheckNotGeometryCollection(geometry);
			CheckEqualSRID(geometry);
			CheckEqualPrecisionModel(geometry);
			return OverlayOp.Overlay(this, geometry, OverlayOp.Intersection)  ;
		}

		///<summary>
		/// Returns a Geometry representing all the points in this Geometry and other.  
		///</summary>
		///<param name="geometry"> the Geometry with which to compute the union </param>
		///<returns>Returns a set combining the points of this Geometry and the points of other geometry.</returns>
		public virtual Geometry Union(Geometry geometry) 
		{
			CheckNotGeometryCollection(this);
			CheckNotGeometryCollection(geometry);
			CheckEqualSRID(geometry);
			CheckEqualPrecisionModel(geometry);
			return OverlayOp.Overlay(this, geometry, OverlayOp.Union);
		}

		///<summary>
		/// Returns a Geometry representing the points making up this Geometry that do not make up the other geometry. 
		///</summary>
		///<remarks>This method returns the closure of the resultant Geometry.</remarks>
		///<param name="geometry">The Geometry with which to compute the difference.</param>
		///<returns>Return the point set difference of this Geometry with other geometry.</returns>
		public virtual Geometry Difference(Geometry geometry) 
		{
			CheckNotGeometryCollection(this);
			CheckNotGeometryCollection(geometry);
			CheckEqualSRID(geometry);
			CheckEqualPrecisionModel(geometry);
			return OverlayOp.Overlay(this, geometry, OverlayOp.Difference);
		}

		///<summary>
		/// Returns a set combining the points in this Geometry not in other, and the points in other not
		/// in this Geometry.
		///</summary>
		///<remarks>This method returns the closure of the resultant Geometry.</remarks>
		///<param name="geometry"> the Geometry with which to compute the symmetric difference.</param>
		///<returns>Returns the point set symmetric difference of this Geometry with other geometry.</returns>
		public virtual Geometry SymDifference(Geometry geometry) 
		{
			CheckNotGeometryCollection(this);
			CheckNotGeometryCollection(geometry);
			CheckEqualSRID(geometry);
			CheckEqualPrecisionModel(geometry);
			return OverlayOp.Overlay(this, geometry, OverlayOp.SymDifference);
		}

		///<summary>
		///  Performs an operation with or on this Geometry's coordinates.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry's coordinates</param>
		public abstract void Apply(ICoordinateFilter filter);

		///<summary>
		///  Performs an operation with or on this Geometry and it's subelement Geometrys (if any).
		///  Only GeometryCollections and subclasses have subelement Geometrys.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry (and it's children, if it is a 
		///GeometryCollection).</param>
		public abstract void Apply(IGeometryFilter filter);

		/// <summary>
		/// Performs an operation with or on this Geometry and its component Geometries.  Only GeometryCollection and
		/// Polygons have component Geometryes; for Polygons they are the LinearRings of the shell and holes.
		/// </summary>
		/// <param name="filter">The filter to apply to this Geometry.</param>
		public abstract void Apply(IGeometryComponentFilter filter);

		/// <summary>
		/// Creates a copy of this object.
		/// </summary>
		/// <returns>Returns an exact copy of this object.</returns>
		public abstract Geometry Clone(); 

		///<summary>
		/// Converts this Geometry to normal form (or canonical form).
		///</summary>
		///<remarks>Normal form is a unique representation
		/// for Geometry's. It can be used to test whether two Geometrys are equal in a way that is 
		/// independent of the ordering of the coordinates within them. Normal form equality is a stronger 
		/// condition than topological equality, but weaker than pointwise equality. The definitions for 
		/// normal form use the standard lexicographical ordering for coordinates. Sorted in order of 
		/// coordinates means the obvious extension of this ordering to sequences of coordinates.
		///</remarks>
		public abstract void Normalize();

		///<summary>
		/// Returns whether this Geometry is greater than, equal to, or less than another Geometry. If their 
		/// classes are different, they are compared using the following  ordering:  
		/// <list type="bullet">
		///		<item><term>Point (lowest)</term></item>
		///		<item><term>MultiPoint</term></item>
		///		<item><term>LineString</term></item>
		///		<item><term>LinearRing</term></item>
		///		<item><term>MultiLineString</term></item>
		///		<item><term>Polygon</term></item>
		///		<item><term>MultiPolygon</term></item>
		///		<item><term>GeometryCollection (highest)</term></item>
		///	</list>
		///	If the two Geometrys have the same class, their first elements are compared. 
		///	If those are the same, the second elements are compared, etc.  
		///</summary>
		///<param name="obj">A Geometry with which to compare to this Geometry.</param>
		///<returns>
		///	Returns a positive number, 0, or a negative number, depending on whether this object
		/// is greater than, equal to, or less than the other geometry.
		///</returns>
		public virtual int CompareTo(object obj) 
		{
			Geometry geometry = obj as Geometry;
			if ( geometry != null )
			{
				if ( GetClassSortIndex() != geometry.GetClassSortIndex() ) 
				{
					return GetClassSortIndex() - geometry.GetClassSortIndex();
				}
				if ( IsEmpty() && geometry.IsEmpty() ) 
				{
					return 0;
				}
				if ( IsEmpty() ) 
				{
					return -1;
				}
				if ( geometry.IsEmpty() ) 
				{
					return 1;
				}
				return CompareToSameClass( geometry );
			}
			else
			{
				throw new ArgumentException("object geometry is not of type Geometry");
			}
		}

		///<summary>
		/// Returns whether the two Geometrys are equal, from the point of view of the equalsExact method. 
		///</summary>
		///<remarks>Called by equalsExact. In general, two Geometry classes are considered to be "equivalent" only
		/// if they are the same class. An exception is LineString, which is considered to be equivalent
		/// to its subclasses.</remarks>
		///<param name="geometry"> the Geometry with which to compare this Geometry  for equality </param>
		///<returns>Returns true if the classes of the two Geometries are considered to be equal by the
		/// equalsExact method.</returns>
		protected  virtual bool IsEquivalentClass(Geometry geometry) 
		{
			return this.GetType().Name.Equals( geometry.GetType().Name );
		}

		///<summary>
		/// Throws an exception if geometry's class is GeometryCollection.
		/// (Its subclasses do not trigger an exception).  
		///</summary>
		///<param name="geometry">The Geometry to check.</param>
		///<exception cref="ArgumentException">Throws an exception if geometry is a GeometryCollection
		/// but not one of its subclasses</exception>
		protected virtual void CheckNotGeometryCollection(Geometry geometry) 
		{
			//Don't use is because we want to allow subclasses e.g. multipolygon etc...
			string name=geometry.GetType().FullName;
			if (geometry.GetType().FullName=="Geotools.Geometries.GeometryCollection")
			{
				throw new ArgumentException("This method does not support GeometryCollection arguments");
			}
		}

		///<summary>
		/// Throws an exception if the spatial reference IDs differ  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<exception cref="ArgumentException">If the two Geometrys have *different SRIDs</exception>
		protected virtual void CheckEqualSRID(Geometry geometry) 
		{
			if ( _SRID != geometry.GetSRID() ) 
			{
				throw new ArgumentException("Expected SRIDs to be equal, but they were not");
			}
		}

		///<summary>
		/// Throws an exception if the PrecisionModels differ.  
		///</summary>
		///<param name="geometry">The Geometry with which to compare this Geometry.</param>
		///<exception cref="ArgumentException">If the two Geometrys have *unequal PrecisionModels</exception>
		protected virtual void CheckEqualPrecisionModel(Geometry geometry) 
		{
			if ( !_precisionModel.Equals( geometry.GetPrecisionModel() ) ) 
			{
				throw new ArgumentException("Expected precision models to be equal, but they were not");
			}
		}

		///<summary>
		/// Returns the minimum and maximum x and y value1s in this Geometry, or a null Envelope if this Geometry
		/// is empty.
		///</summary>
		///<remarks>Unlike getEnvelopeInternal, this method calculates the Envelope each time it is called; 
		/// getEnvelopeInternal caches the result of this method.</remarks>
		///<returns>Returns this Geometrys bounding box; if the Geometry is empty, Envelope.IsNull will 
		///return true</returns>
		protected abstract Envelope ComputeEnvelopeInternal();

		///<summary>
		/// Returns whether this Geometry is greater than, equal to, or less than another Geometry having 
		/// the same class.  
		///</summary>
		///<param name="obj">A Geometry having the same class as this Geometry.</param>
		///<returns>Returns a positive number, 0, or a negative number, depending on whether this object is 
		/// greater than, equal to, or less than obj.</returns>
		public abstract int CompareToSameClass(object obj);

		///<summary>
		/// Returns the first non-zero result of CompareTo encountered as  the two Collections are iterated 
		/// over. If, by the time one of  the iterations is complete, no non-zero result has been 
		/// encountered,  returns 0 if the other iteration is also complete. If b  completes before a, 
		/// a positive number is returned; if a  before b, a negative number.  
		///</summary>
		///<param name="a"> a Collection of Comparables </param>
		///<param name="b"> a Collection of Comparables </param>
		///<returns> the first non-zero compareTo result, if any;  otherwise, zero</returns>
		protected virtual int Compare(ArrayList a, ArrayList b)		// todo change to coordinates
		{
			Coordinate coordA = new Coordinate();
			Coordinate coordB = new Coordinate();
			int compare = 0;
			if(a.Count == b.Count)
			{
				for(int i = 0; i < a.Count; i++)
				{
					coordA = a[i] as Coordinate;
					coordB = b[i] as Coordinate;
					if((coordA != null) && (coordB != null))
					{
						compare =  coordA.CompareTo(coordB);
						if(compare != 0)
						{
							return compare;
						}
					}
					else if((coordA == null) && (coordB == null))
					{
						return 0;
					}
					else if(coordA == null)
					{
						return -1;
					}
					else if(coordB == null)
					{
						return 1;
					}
				}
				return 0;
			}
			if (a.Count > b.Count) 
			{
				return 1;
			}
			else
			{
				return -1;
			}
		}

		private int GetClassSortIndex() 
		{
			bool found = false;
			int index = -1;
			for (int i = 0; i < _sortedClasses.Length && !found; i++) 
			{
				if ( _sortedClasses[i] == this.GetGeometryType() ) 
				{
					index = i;
					found = true;
				}
			}
			if ( !found )
			{ 
				// should never reach here.
				throw new InvalidOperationException("Invalid object type. Object does not support GetClassSortIndex.");
			}
			return index;
		} // private int GetClassSortIndex()

		/// <summary>
		/// Retrieve the new ID of the Spatial Reference System used by the Geometry.
		/// </summary>
		/// <param name="coordinateTransform">The coordinate transformatioin.</param>
		/// <returns>An integer containing the new SRID.</returns>
		public int GetNewSRID(ICoordinateTransformation  coordinateTransform)
		{
			int sourceSRID = int.Parse(coordinateTransform.SourceCS.AuthorityCode);
			int targetSRID = int.Parse(coordinateTransform.TargetCS.AuthorityCode);
		
			int newSRID =-1;
			if (this.GetSRID()==sourceSRID)
			{
				newSRID = targetSRID;
			}
			else if (this.GetSRID()==targetSRID)
			{
				newSRID = sourceSRID;
			}
			else
			{
				throw new InvalidOperationException(String.Format("Current SRID of {0} is not recognized.",this.GetSRID() ));
			}
			return newSRID;
		}

		/// <summary>
		/// Projects a geometry using the given transformation.
		/// </summary>
		/// <param name="coordinateTransform">The transformation to use.</param>
		/// <returns>The geometry projected.</returns>
		public virtual Geometry Project(ICoordinateTransformation  coordinateTransform)
		{
			throw new InvalidOperationException("This method is abstract - but got an error message when unittesting if I made this abstract.");
			/*
			 * could not make this abstract - otherwise go an error.
			   [nunit] Running Geotools.UnitTests.AllTests
				[nunit] ERROR: ProjectTest.Test1:\projects\ogis v. 1.0\4 construction\Geotools.unittests\geometres\projecttest.cs(56): Could not load type Geotools.Geometries.Gometry from assembly Geotools, Version=0.0.0.0, Culture=neutral,PublicKeyToken=null.
				[nunit] 1 tests: FAILURES: 0 ERRORS: 1
			*/
		}

		/// <summary>
		/// Gets the extents of the Geometry.
		/// </summary>
		/// <param name="minX">The minimum X value.</param>
		/// <param name="minY">The minimum Y value.</param>
		/// <param name="maxX">The maximum X value.</param>
		/// <param name="maxY">The maximum Y value.</param>
		public virtual void Extent2D(out double minX, out double minY, out double maxX, out double maxY)
		{
			//If the member variable is null get the Envelope
			if(_envelope == null)
			{
				GetEnvelopeInternal();
			}
			//if the envelope is null get all zeros
			if(!_envelope.IsNull())
			{
				minX = _envelope.MinX;
				minY = _envelope.MinY;
				maxX = _envelope.MaxX;
				maxY = _envelope.MaxY;
			}
			else
			{
				minX = 0;
				minY = 0;
				maxX = 0;
				maxY = 0;
			}
		}
	}


} // namespace Geotools.Geometry