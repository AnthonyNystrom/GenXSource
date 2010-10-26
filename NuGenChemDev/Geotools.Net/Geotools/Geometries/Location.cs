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
	/// Summary description for Location.
	/// </summary>
	internal class Location
	{
		
		/// <summary>
		/// DE-9IM row index of the interior of the first geometry and column index of
		/// the interior of the second geometry. Location value for the interior of a
		/// geometry.
		/// </summary>
		public static int Interior
		{
			get
			{
				return 0;
			}
		}
				
	
		/// <summary>
		/// DE-9IM row index of the boundary of the first geometry and column index of
		/// the boundary of the second geometry. Location value for the boundary of a
		/// geometry.
		/// </summary>
		public static int Boundary
		{
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// DE-9IM row index of the exterior of the first geometry and column index of 
		/// the exterior of the second geometry. Location value for the exterior of a 
		/// geometry. 
		/// </summary>
		public static int Exterior 
		{
			get
			{
				return 2;
			}
		}

		/// <summary>
		/// Used for uninitialized location values.
		/// </summary>
		public static int Null 
		{
			get
			{
				return -1;
			}	
		}


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Location class.
		/// </summary>
		public Location()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion


		#region Static Methods
		/// <summary>
		/// Converts the location value to a location symbol, for example, EXTERIOR => 'e'.
		/// </summary>
		/// <param name="locationValue">The  locationValue  either EXTERIOR(2), BOUNDARY(1), INTERIOR(0) or NULL(-1)</param>
		/// <returns> either 'e', 'b', 'i' or '-'</returns>
		public static char ToLocationSymbol(int locationValue) 
		{
			switch (locationValue) 
			{
				case 2://Location.Exterior:
					return 'e';
				case 1://Location.Boundary:
					return 'b';
				case 0://Location.Interior:
					return 'i';
				case -1://Location.Null:
					return '-';
			}
			throw new ArgumentOutOfRangeException("Unknown location value: " + locationValue);
		}
		#endregion

	}
}
