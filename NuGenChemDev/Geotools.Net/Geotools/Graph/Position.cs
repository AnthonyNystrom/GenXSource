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

namespace Geotools.Graph
{
	/// <summary>
	/// Summary description for Position.
	/// </summary>
	internal class Position
	{
		public static int On      = 0;
		public static int Left    = 1;
		public static int Right   = 2;

		#region Static methods
		/// <summary>
		/// Returns the opposite of the supplied position.
		/// </summary>
		/// <param name="position">The position from which the opposite will be computed.</param>
		/// <returns>Returns the opposite position of supplied position.</returns>
		public static int Opposite(int position)
		{
			if (position == Left) return Right;
			if (position == Right) return Left;
			return position;
		}
		#endregion
	}
}
