/*
 *  Copyright (C) 2002 Urban Science Applications, Inc.
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
using Geotools.CoordinateReferenceSystems;
#endregion

namespace Geotools.Positioning
{
	/// <summary>
	/// Summary description for Longitude.
	/// </summary>
	internal class Latitude : AngularUnit
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Longitude class.
		/// </summary>
		public Latitude(double radiansPerUnit) : base(radiansPerUnit)
		{
		}
		#endregion

		
		/// <summary>
		/// Minimum legal value for latitude (-90°).
		/// </summary>
		public static double MinimumValue
		{
			get
			{
				return -90.0;
			}
		}

		/// <summary>
		/// Maximum legal value for latitude (+90°).
		/// </summary>
		public static double MaximumValue
		{
			get
			{
				return +90;
			}
		}

	}
}
