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
using Geotools.Graph;
using Geotools.Operation;
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A collection of linestrings.
	/// </summary>
	public class MultiLineString : GeometryCollection, IMultiLineString
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the MultiLineString class.
		/// </summary>
		/// <param name="lineStrings">
		/// The LineStrings for this MultiLineString, or null or an 
		/// empty array to create the empty MultiLineString.  Elements may be empty 
		/// LineStrings, but not nulls.
		/// </param>
		/// <param name="precisionModel">
		/// The specification of the grid of allowable points for this MultiLineString.
		/// </param>
		/// <param name="SRID">
		/// The ID of the Spatial Reference System used by this MultiLineString.
		/// </param>
		internal MultiLineString(LineString[] lineStrings, PrecisionModel precisionModel, int SRID)
			: base(lineStrings, precisionModel, SRID)
		{
			if(lineStrings == null)
			{
				lineStrings = new LineString[]{};
			}
			if(HasNullElements(lineStrings))
			{
				throw new ArgumentException("MultiLineStrings cannot have nulls.");
			}
			_geometries = lineStrings;
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets the linestring for this multilinestring.
		/// </summary>
		public LineString[] GetLineStrings
		{
			get
			{
				return _geometries as LineString[];
			}
		}
		
		#endregion

		#region Methods

		/// <summary>
		/// Returns the dimension of this multilinestring (always 1).
		/// </summary>
		/// <returns>1 the dimension of a multilinestring is always 1.</returns>
		public override int GetDimension()
		{
			return 1;
		}

		///<summary>
		///  Returns the dimension of this Geometrys inherent boundary.  
		///</summary>
		///<returns>
		/// Returns the dimension of the boundary of the class implementing this interface, 
		/// whether or not this object is the empty geometry. Returns Dimension.False if the boundary
		/// is the empty geometry.
		/// </returns>
		public override int GetBoundaryDimension()
		{
			if ( IsClosed() )
			{
				return Geotools.Geometries.Dimension.False;
			}
			return 0;
		}

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public override string GetGeometryType()
		{
				return "MultiLineString";
		}

		/// <summary>
		/// Determines if this multilinestring is closed.
		/// </summary>
		/// <returns>True if all the linestrings in the multilinestring are closed.</returns>
		public bool IsClosed()
		{
				if ( IsEmpty() )
				{
					return false;
				}
				for(int i = 0; i < _geometries.Length; i++)
				{
					LineString ls = (LineString)_geometries[i];
					if( !ls.IsClosed() )
					{
						return false;
					}
				}
				return true;
		}

		/// <summary>
		/// Determines if this mls is simple.
		/// </summary>
		/// <returns>True if all the linestrings in the multilinestring are simple.</returns>
		public override bool IsSimple()
		{
			return (new IsSimpleOp()).IsSimple(this);
		}

		///<summary>
		///  Returns the boundary, or the empty geometry if this Geometry is empty.
		///</summary>
		///<remarks>For a discussion
		///  of this function, see the OpenGIS Simple Features Specification. As stated in SFS 
		///  Section 2.1.13.1, "the boundary  of a Geometry is a set of Geometries of the next lower dimension."</remarks>
		///<returns>Returns the closure of the combinatorial boundary of this Geometry.</returns>
		public override Geometry GetBoundary()
		{
			if ( IsEmpty() ) 
			{
				return _geometryFactory.CreateGeometryCollection( null );
			}
			GeometryGraph g = new GeometryGraph(0, this);
			Coordinates pts = g.GetBoundaryPoints();
			return _geometryFactory.CreateMultiPoint(pts);
		}


		/// <summary>
		/// Creates an exact copy of this MulitLineString.
		/// </summary>
		/// <returns>A new MultiLineString containing a copy of the MultiLineString cloned.</returns>
		public override Geometry Clone()
		{
			return _geometryFactory.CreateMultiLineString( (LineString[])_geometries.Clone() );
		}
		
		/// <summary>
		/// Projects a multi line string.
		/// </summary>
		/// <param name="coordinateTransform">The coordinate transformation to use.</param>
		/// <returns>A projected multi line string.</returns>
		public override Geometry Project(ICoordinateTransformation  coordinateTransform)
		{
			if (coordinateTransform==null)
			{
				throw new ArgumentNullException("coordinateTransform");
			}
			if (!(coordinateTransform.MathTransform is Geotools.CoordinateTransformations.MapProjection))
			{
				throw new ArgumentException("coordinateTransform must be a MapProjection.");
			}

			
			LineString[] projectedlines = new LineString[_geometries.Length];
			IGeometry projectedline;
			for(int i=0; i<_geometries.Length; i++)
			{				
				projectedline = _geometries[i].Project(coordinateTransform);
				projectedlines[i] = (LineString)projectedline;				
			}
			return _geometryFactory.CreateMultiLineString(projectedlines);
		}
		#endregion
	}
}
