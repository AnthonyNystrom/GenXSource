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
using Geotools.CoordinateTransformations;
using Geotools.Algorithms;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A polygon is a collection containing a least one linear ring for the 
	/// shell of the polygon and 0-n linear rings for the interior holes.
	/// </summary>
	public class Polygon : Geometry, IPolygon
	{
		/// <summary>
		/// The exterior boundary, or null if this Polygon is the empty geometry.
		/// </summary>
		protected LinearRing _shell = null;
		/// <summary>
		/// The interior boundaries, if any.
		/// </summary>
		protected LinearRing[] _holes;

		#region Constructors

		/// <summary>
		/// Initializes a Polygon with the given exterior boundary.  The shell and holes 
		/// must conform to the assertions specified in the OpenGIS Simple Features
		/// Specification for SQL.
		/// </summary>
		/// <param name="shell">
		///	The outer boundary of the new Polygon, or null or an empty LinearRing if the empty
		///	geometry is to be created. Must be oriented clockwise.
		/// </param>
		/// <param name="precisionModel">
		/// The specification of the grid of allowable points for this Polygon.
		/// </param>
		/// <param name="SRID">
		/// The ID of the Spatial Reference System used by this Polygon.
		/// </param>
		internal Polygon(LinearRing shell, PrecisionModel precisionModel, int SRID) 
			: this( shell, new LinearRing[]{}, precisionModel, SRID)
		{	
		}

		/// <summary>
		/// Initializes a Polygon with the given exterior boundary.  The shell and holes 
		/// must conform to the assertions specified in the OpenGIS Simple Features
		/// Specification for SQL.
		/// </summary>
		/// <param name="shell">
		///	The outer boundary of the new Polygon, or null or an empty LinearRing if the empty
		///	geometry is to be created. Must be oriented clockwise.
		/// </param>
		/// <param name="holes">
		/// The inner boundaries of the new Polygon, or null or empty LinearRings if the empty 
		/// geometry is to be created. Each must be oriented counterclockwise.
		/// </param>
		/// <param name="precisionModel">
		/// The specification of the grid of allowable points for this Polygon.
		/// </param>
		/// <param name="SRID">
		/// The ID of the Spatial Reference System used by this Polygon.
		/// </param>
		internal Polygon(LinearRing shell, LinearRing[] holes, PrecisionModel precisionModel, int SRID) : base(precisionModel, SRID)
		{
			if (shell == null) 
			{
				shell = _geometryFactory.CreateLinearRing(null);
			}
			if (holes == null) 
			{
				holes = new LinearRing[]{};
			}
			//OPTIMIZE: - if fully optimzed, don't bother doing this test.
			if (HasNullElements(holes)) 
			{
				throw new ArgumentNullException("holes must not contain null elements");
			}
			if (shell.IsEmpty() && HasNonEmptyElements(holes)) 
			{
				throw new ArgumentException("shell is empty but holes are not");
			}
			_shell = shell;
			_holes = holes;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the shell of this polygon.
		/// </summary>
		public LinearRing Shell
		{
			get
			{
				return _shell;
			}
		}

		/// <summary>
		/// Gets the interior holes of this polygon.
		/// </summary>
		public LinearRing[] Holes
		{
			get
			{
				return _holes;
			}
		}
		
		#endregion

		#region Private Methods
		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the coordinates for the shell of this polygon.
		/// </summary>
		/// <returns>The coordinates for the shell of the polygon.</returns>
		public override Coordinate GetCoordinate() 
		{
			return _shell.GetCoordinate();
		}

		///<summary>
		///  Returns this Geometry's vertices. Do not modify  the array, as it may be the actual array stored
		///  by this Geometry.  The Geometries contained by composite Geometries must be Geometry's;
		///  that is, they must implement get Coordinates.  
		///</summary>
		///<returns>Returns the vertices of this Geometry.</returns>
		public override Coordinates GetCoordinates()
		{
			Coordinates coords = new Coordinates();
			if( IsEmpty() )
			{
				return coords;
			}
			coords.AddRange( _shell.GetCoordinates() );
			
			if(_holes != null)
			{
				for(int i = 0; i < _holes.Length; i++)
				{
					coords.AddRange( _holes[i].GetCoordinates() );
				}
			}
			return coords;
		}

		/// <summary>
		/// Returns the number of points in this polygon.
		/// </summary>
		/// <returns>The number of points in the polygon 
		/// (both the shell and interior rings if they exist).</returns>
		public override int GetNumPoints()
		{
			int count = _shell.GetNumPoints();
			if(_holes != null)
			{
				for(int i = 0; i < _holes.Length; i++)
				{
					count += _holes[i].GetNumPoints();
				}
			}
			return count;
		}

		/// <summary>
		/// Gets the dimension of this polygon. (The dimension of a polygon is always 2).
		/// </summary>
		/// <returns>2 the dimension of a polygon is always 2.</returns>
		public override int GetDimension()
		{
			return 2;
		}

		///<summary>
		///  Returns the dimension of this Geometrys inherent boundary.  
		///</summary>
		///<returns>
		/// Returns the dimension of the boundary of the class implementing this interface, 
		/// whether or not this object is the empty geometry. Returns Dimension.FALSE if the boundary
		/// is the empty geometry.
		/// </returns>
		public override int GetBoundaryDimension()
		{
			return 1;
		}

		/// <summary>
		/// Determines if this polygon is empty.
		/// </summary>
		/// <returns>True if the polygon is empty.</returns>
		public override bool IsEmpty()
		{
			return _shell.IsEmpty();
		}

		/// <summary>
		/// Determines if this polygon is simple (always true.)
		/// </summary>
		/// <returns>True.</returns>
		public override bool IsSimple()
		{
			return true;
		}
		/// <summary>
		/// Gets the exterior ring for this polygon.
		/// </summary>
		/// <returns>A linearring for the exterior of this polygon.</returns>
		public LinearRing GetExteriorRing()
		{
			return _shell;
		}

		/// <summary>
		/// Gets the number of interior rings in this polygon.
		/// </summary>
		/// <returns>The number of interior rings.</returns>
		public int GetNumInteriorRing()
		{
			return _holes.Length;
		}

		/// <summary>
		/// Gets the interior ring from the array at index n.
		/// </summary>
		/// <param name="n">The index at which to retrieve the interior ring.</param>
		/// <returns>The interior ring at index n.</returns>
		public LinearRing GetInteriorRingN(int n) 
		{
			if(_holes.Length != 0)
			{
				return (LinearRing)_holes[n];
			}
			return null;
		}

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public override string GetGeometryType()
		{
				return "Polygon";
		}

		///<summary>
		///  Returns the boundary, or the empty geometry if this Geometry  is empty.
		/// </summary>
		/// <remarks>For a discussion of this function, see the OpenGIS Simple Features Specification. As stated in SFS 
		///  Section 2.1.13.1, "the boundary  of a Geometry is a set of Geometries of the next lower dimension."  
		/// </remarks> 
		///<returns>Returns the closure of the combinatorial boundary of this Geometry.</returns>
		public override Geometry GetBoundary() 
		{
			if ( IsEmpty() ) 
			{
				return _geometryFactory.CreateGeometryCollection(null);
			}
			LinearRing[] rings = new LinearRing[_holes.Length + 1];
			rings[0] = _shell;
			for (int i = 0; i < _holes.Length; i++) 
			{
				rings[i + 1] = _holes[i];
			}
			return _geometryFactory.CreateMultiLineString(rings);
		}

		///<summary>
		/// Returns the minimum and maximum x and y value1s in this Geometry, or a null Envelope if this Geometry
		/// is empty.
		///</summary>
		///<remarks>Unlike getEnvelopeInternal, this method calculates the Envelope each time it is called; 
		/// getEnvelopeInternal caches the result of this method.</remarks>
		///<returns>
		///	Returns this Geometrys bounding box; if the Geometry is empty, Envelope.IsNull will return true.
		///</returns>
		protected override Envelope ComputeEnvelopeInternal()
		{
			return _shell.GetEnvelopeInternal();
		}

		///<summary>
		/// Returns true if the two Geometrys have the same class and if the data which they store
		/// internally are equal. This method is stricter equality than equals. If this and the other 
		/// Geometrys are composites and any children are not Geometrys, returns  false.  
		///</summary>
		///<param name="obj">The Geometry with which to compare this Geometry.</param>
		///<returns>
		/// Returns true if this and the other Geometry are of the same class and have equal 
		/// internal data.
		///</returns>
		public override bool Equals( object obj )
		{
			Geometry geometry = obj as Geometry;
			if ( !IsEquivalentClass( geometry ) )
			{
				return false;
			}
			Polygon otherPoly = geometry as Polygon;
			if ( otherPoly != null )
			{
				if ( !_shell.Equals( otherPoly.Shell ) )
				{
					return false;
				}
				if ( _holes != null && otherPoly.Holes != null )
				{
					if ( _holes.Length != otherPoly.Holes.Length )
					{
						return false;
					}
					for(int i = 0; i < _holes.Length; i++)
					{
						if ( !_holes[i].Equals( otherPoly.Holes[i] )  )
						{
							return false;
						}
					}
					return true;
				}
				if( _holes == null && otherPoly.Holes == null )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the hash code for this object.
		/// </summary>
		/// <returns>The hash code for this polygon.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		///<summary>
		///  Performs an operation with or on this Geometry's coordinates.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry's coordinates.</param>
		public override void Apply(ICoordinateFilter filter)
		{
			if (filter==null)
			{
				throw new ArgumentNullException("filter");
			}
			_shell.Apply(filter);
			for(int i = 0; i < _holes.Length; i++)
			{
				_holes[i].Apply(filter);
			}
		}

		///<summary>
		///  Performs an operation with or on this Geometry and it's children.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry (and it's children, if it is a 
		///GeometryCollection).</param>
		public override void Apply(IGeometryFilter filter)
		{
			if (filter==null)
			{
				throw new ArgumentNullException("filter");
			}
			filter.Filter( this );
		}

		/// <summary>
		/// Performs an operation with or on this Geometry's components.
		/// </summary>
		/// <param name="filter">The filter to apply to this Geometry's components.</param>
		public override void Apply(IGeometryComponentFilter filter )
		{
			if (filter==null)
			{
				throw new ArgumentNullException("filter");
			}
			filter.Filter( this );
			_shell.Apply( filter );
			for ( int i = 0; i < _holes.Length; i++ )
			{
				_holes[i].Apply( filter );
			}
		}

		/// <summary>
		/// Creates an exact copy of this Polygon.
		/// </summary>
		/// <returns>A geometry containing an exact copy of the original polygon.</returns>
		public override Geometry Clone()
		{
			//create deep clone of the holes
			LinearRing[] holesCopy = new LinearRing[_holes.Length];
			for(int i=0; i<_holes.Length; i++)
			{
				holesCopy[i]= (LinearRing)_holes[i].Clone();
			}
			return _geometryFactory.CreatePolygon((LinearRing)_shell.Clone(), holesCopy);
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
		public override Geometry ConvexHull()
		{
			return _shell.ConvexHull();
		}

		///<summary>
		/// Returns whether this Geometry is greater than, equal to, or less than another Geometry having 
		/// the same class.  
		///</summary>
		///<param name="obj">A Geometry having the same class as this Geometry.</param>
		///<returns>Returns a positive number, 0, or a negative number, depending on whether this object is 
		/// greater than, equal to, or less than obj.</returns>
		public override int CompareToSameClass(object obj)
		{
			LinearRing thisShell = _shell;
			LinearRing otherShell = ((Polygon) obj)._shell;
			return thisShell.CompareToSameClass(otherShell);
		}

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
		public override void Normalize() 
		{
			Normalize( _shell, true );
			for (int i = 0; i < _holes.Length; i++) 
			{
				Normalize( _holes[i], false );
			}
			Array.Sort( _holes );
		}

		///<summary>
		/// Converts a linearring to be sure it is clockwise.
		///</summary>
		///<remarks>Normal form is a unique representation
		/// for Geometry's. It can be used to test whether two Geometrys are equal in a way that is 
		/// independent of the ordering of the coordinates within them. Normal form equality is a stronger 
		/// condition than topological equality, but weaker than pointwise equality. The definitions for 
		/// normal form use the standard lexicographical ordering for coordinates. Sorted in order of 
		/// coordinates means the obvious extension of this ordering to sequences of coordinates.
		///</remarks>
		private void Normalize( LinearRing ring, bool clockwise ) 
		{
			if ( ring.IsEmpty() ) 
			{
				return;
			}
			Coordinates uniqueCoordinates = new Coordinates();
			for ( int i=0; i < ring.GetNumPoints()-1; i++ )
			{
				uniqueCoordinates.Add( ring.GetCoordinateN( i ) );		// copy all but last one into uniquecoordinates
			}
			Coordinate minCoordinate = MinCoordinate( ring.GetCoordinates() );
			Scroll( uniqueCoordinates, minCoordinate );
			Coordinates ringCoordinates = ring.GetCoordinates();
			ringCoordinates.Clear();
			ringCoordinates.AddRange( uniqueCoordinates );
			ringCoordinates.Add( uniqueCoordinates[0].Clone() );		// add back in the closing point.
			if ( _cgAlgorithms.IsCCW( ringCoordinates ) == clockwise )
			{
				ReversePointOrder( ringCoordinates );
			}
		}
		
		/// <summary>
		/// Retrieves the centroid of this polygon.
		/// </summary>
		///<remarks>
		/// <para>Will return a point inside or on the polygon boundary. It will first use the 
		/// average of all the points. If the point falls outside the polygon, the
		/// point returned point will be the nearest polygon boundary point from the average point.
		/// </para>
		/// <para>This point gets cached.</para>
		///</remarks>
		/// <returns>Throws an exception.</returns>
		public IPoint Centroid()
		{
			throw new NotImplementedException("See /// remarks for details on logic.");
		}

		/// <summary>
		/// Projects a geometry using the given transformation. 
		/// </summary>
		/// <param name="coordinateTransform">The transformation to use.</param>
		/// <returns>A projected line string object.</returns>
		public override Geometry Project(ICoordinateTransformation  coordinateTransform)
		{
			LinearRing projectedShell = (LinearRing)this._shell.Project(coordinateTransform);
			LinearRing[] projectedHoles = new LinearRing[_holes.Length];
			for(int i=0;i<_holes.Length; i++)
			{
				projectedHoles[i]=(LinearRing)_holes[i].Project(coordinateTransform);
			}
			return _geometryFactory.CreatePolygon(projectedShell,projectedHoles);
		}

		/// <summary>
		/// Returns the area of this polygon.
		/// </summary>
		/// <returns>Returns the area of this polygon.</returns>
		public override double GetArea()
		{
			double area = 0.0;
			area += Math.Abs( CGAlgorithms.SignedArea( _shell.GetCoordinates()) );
			for (int i = 0; i < _holes.Length; i++) 
			{
				area -= Math.Abs( CGAlgorithms.SignedArea( _holes[i].GetCoordinates()) );
			}
			return area;
		}

		/// <summary>
		/// Returns the perimeter of this polygon.
		/// </summary>
		/// <returns>Returns the perimeter of this polygon.</returns>
		public override double GetLength()
		{
			double len = 0.0;
			len += _shell.GetLength();
			for (int i = 0; i < _holes.Length; i++) 
			{
				len += _holes[i].GetLength();
			}
			return len;
		}


		/// <summary>
		/// Returns a string representation of this object. Holes are excluded.
		/// </summary>
		/// <returns>A string representation of the shell of the polygon.</returns>
		public override string ToString()
		{
			return this.GetGeometryType() + ":" + _shell.ToString();
		}

		#endregion
		
	}
}
