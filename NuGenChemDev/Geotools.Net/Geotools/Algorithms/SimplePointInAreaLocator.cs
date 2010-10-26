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

namespace Geotools.Algorithms
{
	/// <summary>
	/// Summary description for SimplePointInAreaLocator.
	/// </summary>
	internal class SimplePointInAreaLocator
	{
		private static CGAlgorithms _cga = new RobustCGAlgorithms();

		#region Constructors
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		///		Locate is the main location function.  It handles both single-element
		///		and multi-element Geometries.  The algorithm for multi-element Geometries
		///		is more complex, since it has to take into account the boundaryDetermination rule
		/// </summary>
		/// <param name="p"></param>
		/// <param name="geom"></param>
		/// <returns></returns>
		public static int Locate(Coordinate p, Geometry geom)
		{

			if ( geom.IsEmpty() ) return Location.Exterior;

			if ( ContainsPoint( p, geom ) )
			{
				return Location.Interior;
			}
			return Location.Exterior;
		} // public static int Locate(Coordinate p, Geometry geom)

		private static bool ContainsPoint(Coordinate p, Geometry geom)
		{
			if( geom is Polygon )
			{
				return ContainsPointInPolygon( p, (Polygon)geom );
			}
			else if( geom is GeometryCollection )
			{
				GeometryCollection gc = geom as GeometryCollection;
				foreach( Geometry g2 in gc )
				{
					if ( g2 != geom )			// TODO:  what is this line trying to do???
					{
						if ( ContainsPoint( p, g2 ) )
						{
							return true;
						}
					}
				} // foreach( Geometry g2 in geom )


			} // else if( geom is GeometryCollection )

			return false;

		} // private static bool ContainsPoint(Coordinate p, Geometry geom)



		private static bool ContainsPointInPolygon(Coordinate p, Polygon poly)
		{

			if ( poly.IsEmpty() ) return false;

			LinearRing shell = (LinearRing) poly.GetExteriorRing();
			if ( ! _cga.IsPointInRing( p, shell.GetCoordinates() ) )
			{
				return false;
			}

			// now test if the point lies in or on the holes
			for (int i = 0; i < poly.GetNumInteriorRing(); i++) 
			{
				LinearRing lr = poly.GetInteriorRingN( i );
				if ( _cga.IsPointInRing( p, lr.GetCoordinates() ) )
				{
					return false;
				}
			}
			return true;
		} // private static bool ContainsPointInPolygon(Coordinate p, Polygon poly)


		#endregion

	}
}
