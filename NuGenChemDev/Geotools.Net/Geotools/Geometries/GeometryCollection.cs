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
using System.Text;
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A Geometry that is a collection of one or more Geometries.
	/// </summary>
	public class GeometryCollection :Geometry, IGeometryCollection, IEnumerable
	{
		#region Constructors

		//A private member variable to hold the geometry array
		protected Geometry[] _geometries;

		/// <summary>
		/// Initializes a GeometryCollection.
		/// </summary>
		/// <param name="geometries">
		/// The Geometries for this GeometryCollection, or null or an 
		/// empty array to create the empty geometry.  Elements may be empty 
		/// Geometries, but not nulls.
		/// </param>
		/// <param name="precisionModel">
		/// The specification of the grid of allowable points for this GeometryCollection.
		/// </param>
		/// <param name="SRID">
		/// The ID of the Spatial Reference System used by this GeometryCollection.
		/// </param>
		public GeometryCollection(Geometry[] geometries, PrecisionModel precisionModel, int SRID) : base(precisionModel,SRID) 
		{
			if(geometries == null) 
			{
				geometries = new Geometry[]{};
			}
			if(HasNullElements(geometries)) 
			{
				throw new ArgumentException("Geometries must not contain null elements.");
			}
			this._geometries = geometries;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the number of geometries in this geometrycollection
		/// </summary>
		/// <returns>An int containing the number of geometries</returns>
		public int Count
		{
			get
			{
				if(_geometries != null)
				{
					return _geometries.Length;
				}
				return 0;
			}
		}

		/// <summary>
		/// Represents the GeometryCollection object entry at the specified index.
		/// </summary>
		public Geometry this [int index]
		{
			get
			{
				return (Geometry)_geometries[index];
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Retrieves the coordinates for this geometry.
		/// </summary>
		/// <returns>A Coordinate.</returns>
		public override Coordinate GetCoordinate() 
		{
			if ( IsEmpty() ) return null;
			return _geometries[0].GetCoordinate();
		}

		///<summary>
		///  Returns this Geometry's vertices.
		///</summary>
		///<remarks>Do not modify the array, as it may be the actual array stored
		///  by this Geometry.  The Geometries contained by composite Geometries must be Geometry's;
		///  that is, they must implement get Coordinates.  </remarks>
		///<returns>Returns the vertices of this Geometry</returns>
		public override Coordinates GetCoordinates()
		{
			Coordinates coordinates = new Coordinates();
			if(_geometries != null)
			{
				foreach(Geometry geom in _geometries)
				{
					Coordinates childCoords = geom.GetCoordinates();
					foreach(Coordinate coord in childCoords)
					{
						coordinates.Add(coord);
					}
				}
			}
			return coordinates;
		}

		/// <summary>
		/// Determines if this geometrycollection is empty
		/// </summary>
		/// <returns>True if empty else false.</returns>
		public override bool IsEmpty()
		{
			if( _geometries==null )
			{
				return true;
			}
			foreach(Geometry geom in _geometries)
			{
				if( !geom.IsEmpty() )
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns the largest dimension in the collection.
		/// </summary>
		/// <remarks>E.g. a surface = 2 , a point = 0.</remarks>
		/// <returns>An integer containing the largest dimension in the collection.</returns>
		public override int GetDimension()
		{
			int dimension = Geometries.Dimension.False;
			if(_geometries != null)
			{
				foreach(Geometry geom in _geometries)
				{
					dimension = Math.Max(dimension, geom.GetDimension() );
				}
			}
			return dimension;
		}

		///<summary>
		///  Returns the dimension of this Geometry's inherent boundary.  
		///</summary>
		///<returns>
		/// Returns the dimension of the boundary of the class implementing this interface, 
		/// whether or not this object is the empty geometry. Returns Dimension.FALSE if the boundary
		/// is the empty geometry. 
		/// </returns>
		public override int GetBoundaryDimension() 
		{
			int dimension = Geometries.Dimension.False;
			if(_geometries != null)
			{
				foreach(Geometry geom in _geometries)
				{
					dimension = Math.Max(dimension, geom.GetBoundaryDimension());
				}
			}
			return dimension;
		}

		/// <summary>
		/// Gets the number of geometries in this geometrycollection
		/// </summary>
		/// <returns>An int containing the number of geometries</returns>
		public virtual int GetNumGeometries()
		{
			if(_geometries == null)
			{
				return 0;
			}
			return _geometries.Length;
		}

		/// <summary>
		/// Returns the nth geometry in the collection.
		/// </summary>
		/// <param name="n">The index into the collection from which to get the geometry.</param>
		/// <returns>Returns the geometry at the nth index.</returns>
		public Geometry GetGeometryN(int n)
		{
			return _geometries[n];
		}

		/// <summary>
		/// Gets the total number of points in this collection
		/// </summary>
		/// <returns>An integer containing the number of points in this collection.</returns>
		public override int GetNumPoints()
		{
			int numPoints = 0;
			if( _geometries != null )
			{
				foreach(Geometry geom in _geometries)
				{
					numPoints += geom.GetNumPoints();
				}
			}
			return numPoints;
		}

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public override string GetGeometryType()
		{
			return "GeometryCollection";
		}

		/// <summary>
		/// Determines if this collection is simple
		/// </summary>
		/// <returns>True if all the geometries in this collection are simple.</returns>
		public override bool IsSimple()
		{
			CheckNotGeometryCollection(this);
			throw new NotImplementedException("This operation is not valid on geometry collections");
		}

		///<summary>
		///  Returns the boundary, or the empty geometry if this Geometry is empty.  
		///</summary>
		///<remarks>For a discussion
		///  of this function, see the OpenGIS Simple Features Specification. As stated in SFS 
		///  Section 2.1.13.1, "the boundary  of a Geometry is a set of Geometries of the next lower dimension."
		/// </remarks>
		///<returns>Returns the closure of the combinatorial boundary of this Geometry</returns>
		public override Geometry GetBoundary()
		{
			CheckNotGeometryCollection(this);
			throw new NotSupportedException();
		}
	
		///<summary>
		/// Returns true if the two Geometrys have the same class and if the data which they store
		/// internally are equal.
		///</summary>
		///<remarks>This method is stricter equality than equals. If this and the other 
		/// Geometrys are composites and any children are not Geometrys, returns  false.
		/// </remarks>
		///<param name="obj">The Geometry with which to compare this Geometry.</param>
		///<returns>
		/// Returns true if this and the other Geometry are of the same class and have equal 
		/// internal data.
		///</returns>
		public override bool Equals(object obj)
		{
			Geometry geometry = obj as Geometry;
			if ( geometry != null )
			{

				if( !IsEquivalentClass(geometry) )
				{
					return false;
				}
				GeometryCollection otherCollection = geometry as GeometryCollection;
				if( otherCollection != null )
				{
					if (_geometries.Length != otherCollection.Count )
					{
						return false;
					}
					int count = 0;
					foreach( Geometry geom in _geometries )
					{
						if ( (otherCollection[count] as Geometry) == null )
						{
							return false;
						}
						if(!(geom.Equals((Geometry)otherCollection._geometries[count])))
						{
							return false;
						}
						count++;
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns a unique integer for this object.
		/// </summary>
		/// <remarks>Used with hash tables.</remarks>
		/// <returns>An integer containing the hash code.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		///<summary>
		///  Performs an operation with or on this Geometry's coordinates.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry's coordinates</param>
		public override void Apply(ICoordinateFilter filter)
		{
			if(filter == null)
			{
				throw new ArgumentException("Filters cannot be null");
			}
			foreach(Geometry geom in _geometries)
			{
				geom.Apply(filter);
			}
		}

		///<summary>
		///  Performs an operation with or on this Geometry and it's children.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry (and it's children, if it is a 
		///GeometryCollection).</param>
		public override void Apply(IGeometryFilter filter)
		{
			if(filter == null)
			{
				throw new ArgumentException("Filters cannot be null");
			}
			filter.Filter( this );
			foreach(Geometry geom in _geometries)
			{
				geom.Apply(filter);
			}
		}

		/// <summary>
		/// Performs an operation with or on this Geometry and it's children.
		/// </summary>
		/// <param name="filter">The filter to apply to this Geometry (and it's children, if it is a 
		///GeometryCollection).</param>
		public override void Apply(IGeometryComponentFilter filter)
		{
			if(filter == null)
			{
				throw new ArgumentException("Filters cannot be null");
			}
			filter.Filter( this ); // will cause a recusive loop????
			foreach( Geometry geom in _geometries )
			{
				geom.Apply( filter );
			}
		}

		/// <summary>
		/// Creates an exact copy of this geometrycollection
		/// </summary>
		/// <returns>A new geometrycollection</returns>
		public override Geometry Clone()
		{
			// create a new array of geometrie with clones of existing geometries.
			Geometry[] garray = new Geometry[ _geometries.Length ];
			for ( int i = 0; i < _geometries.Length; i++ )
			{
				garray[i] = (Geometry)_geometries[i].Clone();
			}
			// create geometrycollection with cloned geometries.
			return _geometryFactory.CreateGeometryCollection(garray);
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
			for (int i = 0; i < _geometries.Length; i++) 
			{
				_geometries[i].Normalize();
			}
			Array.Sort( _geometries );
		}

		///<summary>
		/// Returns the minimum and maximum x and y value1s in this Geometry, or a null Envelope if this Geometry
		/// is empty.
		///</summary>
		///<remarks>Unlike getEnvelopeInternal, this method calculates the Envelope each time it is called; 
		/// getEnvelopeInternal caches the result of this method.</remarks>
		///<returns>
		///	Returns this Geometrys bounding box; if the Geometry is empty, Envelope.IsNull will return true
		///</returns>
		protected override Envelope ComputeEnvelopeInternal()
		{
			Envelope env = new Envelope();
			if(_geometries != null)
			{
				foreach(Geometry geom in _geometries)
				{
					env.ExpandToInclude(geom.GetEnvelopeInternal());
				}
			}
			return env;
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
			// create new array of coordinates to sort.
			ArrayList theseElements = new ArrayList();
			theseElements.AddRange( GetCoordinates() );
			theseElements.Sort();

			// get other's elements in arraylist and sort.
			Geometry other = obj as Geometry;
			ArrayList otherElements = new ArrayList();
			otherElements.AddRange( other.GetCoordinates() );
			otherElements.Sort();

			return Compare(theseElements, otherElements);
		} // public override int CompareToSameClass(object obj)
	
		/// <summary>
		/// Gets the enumerator for this class.
		/// </summary>
		/// <returns>Returns an IEnumerator.</returns>
		public IEnumerator GetEnumerator()
		{
			return _geometries.GetEnumerator();
		}

		/// <summary>
		/// Returns a string representation of this object. Holes are excluded.
		/// </summary>
		/// <returns>A string containing each of the geometries in the collection seperated by a ;.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( this.GetGeometryType() );
			foreach( Geometry geometry in _geometries )
			{
				sb.Append(";");
				sb.Append( geometry.ToString() );
			}
			return sb.ToString();
		} // public override string ToString()

		/// <summary>
		/// Projects each item in a geometry collection.
		/// </summary>
		/// <param name="coordinateTransform">The transformation to use.</param>
		/// <returns>The resulting projected geometry.</returns>
		public override Geometry Project(ICoordinateTransformation  coordinateTransform)
		{
			if (coordinateTransform==null)
			{
				throw new ArgumentNullException("coordinateTransform");
			}
			if (!(coordinateTransform.MathTransform is Geotools.CoordinateTransformations.MapProjection))
			{
				throw new ArgumentException("CoordinateTransform must be a MapProjection.");
			}

			Geometry[] geometryArray = new Geometry[_geometries.Length];
			Geometry geometry = null;
			Geometry projectedGeometry = null;
			for (int i=0;i<_geometries.Length; i++)
			{
				geometry = (Geometry)_geometries.GetValue(i);
				projectedGeometry = (Geometry)geometry.Project( coordinateTransform );
				geometryArray[i] = projectedGeometry;
			}
			
			return _geometryFactory.CreateGeometryCollection(geometryArray);
		}

		/// <summary>
		/// Returns the area of this GeometryCollection.
		/// </summary>
		/// <returns>Returns the area of all geometries.</returns>
		public override double GetArea()
		{
			double area = 0.0;
			for (int i = 0; i < _geometries.Length; i++) 
			{
				area += _geometries[i].GetArea();
			}
			return area;
		}

		/// <summary>
		/// Returns the Length of all geometries in this GeometryCollection.
		/// </summary>
		/// <returns>Returns the Length of all geometries in this GeometryCollection.</returns>
		public override double GetLength()
		{
			double sum = 0.0;
			for (int i = 0; i < _geometries.Length; i++) 
			{
				sum += ((LineString) _geometries[i]).GetLength();
			}
			return sum;
		}

		#endregion

	}
}
