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
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A set of points. The spatial relationship predicates (like disjoint) are based on the Dimensionally 
	/// Extended Nine-Intersection Model (DE-9IM). For a description of the DE-9IM, see the <A
	///  HREF="http://www.opengis.org/techno/specs.htm">OpenGIS Simple Features Specification for SQL</A> 
	/// </summary>
	/// <remarks>
	/// A Precision Model object is a member of every Geometry object.
	///
	/// <para>
	/// The SFS specifies that objects of each Geometry subclass may be empty. It is
	/// sometimes necessary to construct a generic empty object of class Geometry
	/// (e.g. if the exact type of the Geometry to be returned is not known). The
	/// SFS does not define a specific class or object to represent a generic
	/// empty Geometry. JTS uses the convention that an empty GeometryCollection
	/// will be returned.
	/// <H3> Binary Predicates </H3>
	/// The binary predicates can be completely specified in terms of an IntersectionMatrix pattern.
	/// In fact, their implementation is simply a call to relate with the appropriate pattern.
	///	</para>
	///	
	///	<para>
	///	 It is important to note that binary predicates are topological operations
	///	 rather than pointwise operations. Even for apparently straightforward
	///	 predicates such as equals topoloty, it is easy to find cases where a
	///	 pointwise comparison does not produce the same result as a topological
	///	 comparison. (for instance: A and B are MultiPoints with the same point 
	///	 repeated different numbers of times; A is a LineString with two collinear line
	///	 segments and B is a single line segment with the same start and endpoints). 
	///	 The algorithm used for the relate method is a topology-based algorithm which
	///	 produces a topologically correct result.</para>
	///	
	///	<para>
	///	 As in the SFS, the term P is used to refer to 0-dimensional Geometrys (Point and MultiPoint), 
	///	 L to 1-dimensional Geometrys ( LineString, and MultiLineString ), and 
	///	 A to 2-dimensional Geometrys (Polygon and MultiPolygon). The dimension of a GeometryCollection
	///	 is equal to the maximum dimension of its components.</para>
	///	
	///	<para>
	///	 In the SFS some binary predicates are stated to be undefined for some
	///	 combinations of dimensions (e.g. touches is undefined for P /P ). In the interests
	///	 of simplifying the API, combinations of argument Geometrys which are not in the
	///	 domain of a predicate will return false (e.g. touches(Point, Point) => false). </para>
	///	 
	///	 <para>
	///	  If either argument to a predicate is an empty Geometry the
	///  predicate will return false. 
	///  <H3>Set-Theoretic Methods</H3> 
	///  For certain inputs, the difference and symDifference methods may compute non-closed sets. 
	///  This can happen when the arguments overlap and have different dimensions. Since JTS Geometry
	///  objects can represent only closed sets, the spatial analysis methods are
	///  specified to return the closure of the point-set-theoretic result.</para>
	/// </remarks>
	public interface IGeometry
	{
		/// <summary>
		/// Returns the ID of the Spatial Reference System used by the Geometry.
		/// </summary>
		/// <remarks>JTS supports Spatial Reference System information in the simple way
		///  defined in the SFS. A Spatial Reference System ID (SRID) is present in
		///  each Geometry object. Geometry provides basic accessor operations for this field, 
		///  but no others. The SRID is represented as an integer.</remarks>
		/// <returns>Returns the ID of the coordinate space in which the geometry is defined.</returns>
		int GetSRID();

		/// <summary>
		/// Returns the type name of this object. 
		/// </summary>
		/// <returns>Returns the type name of this Geometry.</returns>
		string GetGeometryType();

		/// <summary>
		/// Returns the PrecisionModel used by the Geometry.
		/// </summary>
		/// <returns>Returns the PrecisionModel used by the Geometry.</returns>
		PrecisionModel PrecisionModel{get;}

		/// <summary>
		/// Returns the minimum and maximum x and y values in this Geometry, or a null envelope
		/// if this geometry is empty.
		/// </summary>
		/// <returns>Returns an Envelope object with this Geometry's bounding box; if the Geometry
		/// is empty, Envelope.IsNull will return true.</returns>
		Envelope GetEnvelopeInternal() ;

		/// <summary>
		/// Returns this Geometrys bounding box.
		/// </summary>
		/// <remarks>If this Geometry is the empty geometry, returns an empty Point.
		/// If the Geometry is a point, returns a non-empty Point.  Otherwise, returns a Polygon whose point are
		/// (minx, miny), (maxx, miny), (maxx, maxy), (minx, maxy), (minx, Miny).</remarks>
		/// <returns>Returns an empty Point for empty geometries, a Point for Points, or a Polygon in all other cases.</returns>
		Geometry GetEnvelope();	

		/// <summary>
		/// Returns whether or not the set of points in this Geometry is empty.
		/// </summary>
		/// <returns>Return true if this Geometry equals the empty geometry.</returns>
		bool IsEmpty();

		/// <summary>
		///  Returns false if the Geometry has any anomalous points.
		/// </summary>
		/// <remarks>
		///	  Subinterfaces can refine this definition of "simple" in their comments.
		///	  If this Geometry is empty, returns true.
		///
		///  In general, the SFS specifications of simplicity seem to follow the
		///	  following rule:
		///		A Geometry is simple iff the only self-intersections are at boundary points.
		///	 
		///  For all empty Geometrys, isSimple = true
		/// </remarks>
		/// <returns>Returns true if this Geometry has any points of
		///  self-tangency, self-intersection or other anomalous points</returns>
		bool IsSimple();

		/// <summary>
		/// Returns the boundary, or the empty geometry if this Geometry is empty.
		/// </summary>
		/// <remarks>For
		/// discussion of this function, see the OpenGIS Simple Features Specification.
		/// As stated in SFS Section 2.1.13.1 "the boundary of a Geometry is a set of
		/// Geometries of the next lower dimension."</remarks>
		/// <returns>Returns the closure of the combinatorial boundary of this geometry.</returns>
		Geometry GetBoundary();

		/// <summary>
		/// Returns the inherent dimension of this Geometry.
		/// </summary>
		/// <returns>Returns the dimension of the class implementing this interface, whether
		/// or not this object is the empty geometry.</returns>
		int GetDimension();

		/// <summary>
		/// Returns true if the DE-9IM intersection matrix for the two Geometries is
		/// "T*F**FFF*".
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns true if the two Geometry are Topologically equal.</returns>
		bool EqualsTopology(Geometry other);

		/// <summary>
		/// Returns true if the DE-9IM intersection matrix for the two geometries is "FF*FF****".
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns true if the two Geometrys are disjoint.</returns>
		bool Disjoint(Geometry other);

		/// <summary>
		/// Returns true if disjoint returns false.
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns true if the two geometries intersect.</returns>
		bool Intersects(Geometry other);

		/// <summary>
		/// Returns true if the DE-9IM intersection matrix for the two
		///  Geometrys is FT*******, F**T***** or F***T****.
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns true if the two Geometrys touch;
		/// Returns false if both Geometrys are points.</returns>
		bool Touches(Geometry other);


		
	
		/// <summary>
		/// Returns true if the DE-9IM intersection matrix for the two
		/// Geometrys is
		/// <list type="bullet">
		/// <item><term>T*T****** for a point and a curve, a point and an area or a line and an area.</term><description>for a point and a curve, a point and an area or a line and an area</description></item>
		/// <item><term>0******** for two curves.</term><description></description></item>
		/// </list>
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns true if the two Geometrys cross.
		/// For this function to return true, the Geometrys must be a point and a curve;
		/// a point and a surface; two curves; or a curve and a surface.</returns>
		bool Crosses(Geometry other);

		
		/// <summary>
		/// Returns true if the DE-9IM intersection matrix for the two
		/// Geometrys is T*F**F***.
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns true if this Geometry is within other.</returns>
		bool Within(Geometry other);

		/// <summary>
		/// Returns true if the other geometry is within this.
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns true if this Geometry contains other.</returns>
		bool Contains(Geometry other);

		/// <summary>
		/// Returns true if the DE-9IM intersection matrix for the two geometries is:
		/// <list type="bullet">
		/// <item><term>T*T***T** for two points or two surfaces</term><description></description></item>
		/// <item><term>1*T***T** for two curves</term><description></description></item>
		/// </list>
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns true if the two Geometrys overlap. For this function to return true, the Geometrys
		///  must be two points, two curves or two surfaces.</returns>
		bool Overlaps(Geometry other);

		/// <summary>
		///   Returns true if the elements in the DE-9IM intersection
		///  matrix for the two Geometrys match the elements in intersectionPattern,
		///	  which may be:
		///	<list type="bullet">
		///		<item><term>0</term></item>
		///		<item><term>1</term></item>
		///		<item><term>2</term></item>
		///		<item><term>T ( = 0, 1 or 2)</term></item>
		///		<item><term>F ( = -1)</term></item>
		///		<item><term>* ( = -1, 0, 1 or 2)</term></item>
		///	</list>
		///  For more information on the DE-9IM, see the OpenGIS Simple Features Specification.
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <param name="intersectionPattern">The intersectionPattern the pattern against which to check the
		///  intersection matrix for the two Geometrys.</param>
		/// <returns>Returns true if the DE-9IM intersection matrix for the two Geometries match 
		/// intersectionPattern.</returns>
		bool Relate(Geometry other, string intersectionPattern);

		/// <summary>
		/// Returns the DE-9IM intersection matrix for the two Geometries.
		/// </summary>
		/// <param name="other">The other geometry with which to compare to this geometry.</param>
		/// <returns>Returns a matrix describing the intersection of the interiors, boundaries, and exteriors of
		/// the two geometries.</returns>
		IntersectionMatrix Relate(Geometry other);

		/// <summary>
		/// Returns a buffer region around this geometry having the given width.
		/// </summary>
		/// <param name="distance">The distance the width of the buffer, interpreted according to the
		///  PrecisionModel of the Geometry.</param>
		/// <returns>Returns all points whose distance from this Geometry are less than or equal to distance.</returns>
		Geometry Buffer(double distance);

		/// <summary>
		/// Returns the smallest convex Polygon that contains all the points in the Geometry. 
		/// </summary>
		/// <remarks>This obviously applies only to Geometrys which contain 3 or more points; 
		///	 the results for degenerate cases are specified as follows:
		///	 <list type="table">
		///		<listheader><term>Number of Points in argument Geometry</term><description>Geometry class of result</description></listheader>
		///		<item><term>0</term><description>Empty GeometryCollection</description></item>
		///		<item><term>1</term><description>Point</description></item>
		///		<item><term>2</term><description>LineString</description></item>
		///		<item><term>3 or more</term><description>Polygon</description></item>
		///	</list>
		///	</remarks>
		/// <returns>Returns the minimum-area convex polygon containing this Geometry's points.</returns>
		Geometry ConvexHull();

		/// <summary>
		/// Returns a Geometry representing the points shared by this Geometry and other.
		/// </summary>
		/// <param name="other">The other Geometry with which to compute the intersection.</param>
		/// <returns>Returns the points common to th two Geometries.</returns>
		Geometry Intersection(Geometry other);

		/// <summary>
		/// Returns a Geometry representing all the points in this Geometry and other.
		/// </summary>
		/// <param name="other">The other Geometry with which to compute the union.</param>
		/// <returns>Returns a set combining the points of this Geometry and the points of other.</returns>
		Geometry Union(Geometry other);

		/// <summary>
		/// Returns a Geometry representing the points making up this Geometry that do not make up other.
		/// This method returns the closure of the resultant Geometry.
		/// </summary>
		/// <param name="other">The other geometry with which to comput the difference.</param>
		/// <returns>Returns the point set difference of this Geometry with other.</returns>
		Geometry Difference(Geometry other);

		/// <summary>
		/// Returns a set combining the points in this Geometry not in other, and the points in other not
		/// in this geometry.  This method returns the closure of the resultant geometry.
		/// </summary>
		/// <param name="other">The other geometry with which to compute the symmetric difference.</param>
		/// <returns>Returns the point set symmetric difference of this geometry with other.</returns>
		Geometry SymDifference(Geometry other);

		/// <summary>
		/// Returns the well-known text representation of this geometry.  For a definition of the well-known text format,
		/// see the OpenGIS Simple Features Specification.
		/// </summary>
		/// <returns>Returns the well-known text representation of this geometry.</returns>
		string ToText();
	}
}
