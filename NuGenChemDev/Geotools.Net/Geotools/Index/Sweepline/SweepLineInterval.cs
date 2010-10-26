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

namespace Geotools.Index.Sweepline
{
	/// <summary>
	/// Summary description for SweepLineInterval.
	/// </summary>
	internal class SweepLineInterval
	{
		private double _min;
		private double _max;
		private object _item;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the SweepLineInterval class.
		/// </summary>
		public SweepLineInterval( double min, double max ) : this( min, max, null )
		{
		} // public SweepLineInterval( double min, double max )

		public SweepLineInterval( double min, double max, object item )
		{
			_min = min < max ? min : max;
			_max = max > min ? max : min;
			_item = item;
		}
		#endregion

		#region Properties
		public double Min 
		{ 
			get
			{
				return _min; 
			}
		}
		public double Max
		{
			get
			{
				return _max;  
			}
		}
		public object Item 
		{
			get
			{
				return _item;
			}
		}

		#endregion

		#region Methods
		#endregion

	}
}
