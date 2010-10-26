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
using Geotools.Geometries;
#endregion

namespace Geotools.Utilities
{
	/// <summary>
	/// Creates a new list of unique coordinates.
	/// </summary>
	public class UniqueCoordinateArrayFilter : ICoordinateFilter
	{
		ArrayList _treeSet = new ArrayList();

		// not need for this - just use the hashtable to store everything.
		//ArrayList _list= new ArrayList();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the UniqueCoordinateArrayFilter class.
		/// </summary>
		public UniqueCoordinateArrayFilter()
		{
		}
		#endregion


		#region Methods
		/// <summary>
		/// Returns the unique Coordinates.
		/// </summary>
		/// <returns>Return the Coordinates collected by this CoordinateArrayFilter.</returns>
		public Coordinates GetCoordinates() 
		{
			// copy costruct a new list of coordinates.
			Coordinates coordinates = new Coordinates();
			foreach(object obj in _treeSet)
			{
				coordinates.Add(obj);
			}
			return coordinates;
		}

		/// <summary>
		/// Creates a list of unique coordinates.
		/// </summary>
		/// <param name="coord"></param>
		public void Filter(Coordinate coord) 
		{
			if (!_treeSet.Contains(coord)) 
			{
				_treeSet.Add(coord);
			}
		}
		#endregion

	}
}
