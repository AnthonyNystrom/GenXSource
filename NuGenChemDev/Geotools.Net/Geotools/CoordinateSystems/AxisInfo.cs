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

namespace Geotools.CoordinateReferenceSystems
{
	/// <summary>
	/// Some commonly used axis information. 
	/// </summary>
	public class AxisInfo : IAxisInfo
	{
		private string _name="";
		private AxisOrientation _orientation;
		/// <summary>
		/// Default axis info for <var>x</var> values. Increasing ordinates values go East. This 
		/// is usually used with projected coordinate systems.
		/// </summary>
		public static IAxisInfo X
		{
			get
			{
				return new AxisInfo("x",AxisOrientation.East);
			}
		}

		/// <summary> 
		/// Default axis info for <var>y</var> values. Increasing ordinates values go North. This
		/// is usually used with projected coordinate systems.
		/// </summary> 
		public static  IAxisInfo Y 
		{
			get
			{
				return new AxisInfo("y",AxisOrientation.North);
			}
		}


		/// <summary>
		/// Default axis info for longitudes. Increasing ordinates values go East.
		/// This is usually used with geographic coordinate systems.
		/// </summary>
		public static  IAxisInfo Longitude
		{
			get
			{
				return new AxisInfo("Longitude",AxisOrientation.East);
			}
		}

		/// <summary>
		///  Default axis info for latitudes.Increasing ordinates values go North.
		///  This is usually used with geographic coordinate systems.
		/// </summary>
		public static IAxisInfo Latitude
		{
			get
			{
				return new AxisInfo("Latitude",AxisOrientation.North);
			}
		}


		/// <summary>
		/// The default axis for altitude values. Increasing ordinates values go up.
		/// </summary>
		public static  IAxisInfo Altitude
		{
			get
			{
				return new AxisInfo("Altitude",AxisOrientation.Up);
			}
		}

		/// <summary>
		/// Initializes a new instance of the AxisInfo class with a value for the RadiansPerUnit property.
		/// </summary>
		/// <param name="name">The name of the new axis.</param>
		/// <param name="orientation">The orietation of the axis.</param>
		public AxisInfo(string name, AxisOrientation orientation)
		{
			_name = name;
			_orientation = orientation;
		}

		/// <summary>
		/// Gets the name of the axis.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}
		/// <summary>
		/// Returns the orientation of the axis.
		/// </summary>
		public AxisOrientation Orientation
		{
			get
			{
				return _orientation;
			}
		}

	}
}
