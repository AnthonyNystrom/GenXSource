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
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Valid
{
	/// <summary>
	/// Implements the appropriate checks for repeated points
	/// (consecutive identical coordinates) as defined in the
	/// JTS spec.
	/// </summary>
	internal class RepeatedPointTester
	{
		// save the repeated coord found (if any)
		private Coordinate _repeatedCoord;


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the RepeatedPointTester class.
		/// </summary>
		public RepeatedPointTester()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		public Coordinate GetCoordinate()
		{
			return _repeatedCoord; 
		}

		public bool HasRepeatedPoint(Geometry g)
		{
			
			if ( g.IsEmpty() ) return false;
			if (g is Point)                   return false;
			else if (g is MultiPoint)         return false;
				// LineString also handles LinearRings
			else if (g is LineString)         
			{
				return HasRepeatedPoint(((LineString) g).GetCoordinates() );
			}
			else if (g is Polygon)            return HasRepeatedPoint((Polygon) g);
			else if (g is GeometryCollection) return HasRepeatedPoint((GeometryCollection) g);
			else  throw new NotSupportedException(g.GetType().Name);
			
		}

		public bool HasRepeatedPoint(Coordinates coord)
		{
			
			for (int i = 1; i < coord.Count; i++) 
			{
				if (coord[i - 1].Equals(coord[i]) ) 
				{
					_repeatedCoord = coord[i];
					return true;
				}
			}
			return false;
		}
		private bool HasRepeatedPoint(Polygon p)
		{
			LinearRing exteriorRing1;
			exteriorRing1 = (LinearRing)p.GetExteriorRing();	
			if (HasRepeatedPoint(exteriorRing1.GetCoordinates() ))
			{
				return true;
			}
			for (int i = 0; i < p.GetNumInteriorRing(); i++) 
			{
				LinearRing interiorRing = p.GetInteriorRingN(i);
				if (HasRepeatedPoint( (interiorRing).GetCoordinates() )) return true;
			}
			return false;
		}
		private bool HasRepeatedPoint(GeometryCollection gc)
		{	
			for (int i = 0; i < gc.GetNumGeometries(); i++) 
			{

				Geometry g = (Geometry)gc.GetGeometryN(i);
				if (HasRepeatedPoint(g)) return true;
			}
			return false;
		}

		#endregion

	}
}
