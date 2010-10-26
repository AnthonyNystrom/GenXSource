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
#endregion

namespace Geotools.IO
{
	//Types 2,4,6,7 and 9 were undefined at time of writing

	/// <summary>
	/// The values used in the .shp file to represent the type of shape.
	/// </summary>
	public enum ShapeType
	{
		/// <summary>
		/// The null shape type.
		/// </summary>
		Null=0,
		/// <summary>
		/// The point shape type.
		/// </summary>
		Point=1,
		/// <summary>
		/// The polyline/ arc shape type.
		/// </summary>
		Arc=3,
		/// <summary>
		/// The polygon shape type.
		/// </summary>
		Polygon=5,
		/// <summary>
		/// The multi point shape type.
		/// </summary>
		MultiPoint=8,
		//ArcM=23,
		/// <summary>
		/// Shapetype is undefined.
		/// </summary>
		Undefined=-1
	}
}
