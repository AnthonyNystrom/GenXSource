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

namespace Geotools.Index.BinTree
{
	/// <summary>
	/// Summary description for Interval.
	/// </summary>
	internal class Interval
	{
		public double _min, _max;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Interval class.
		/// </summary>
		public Interval()
		{
			_min = 0.0;
			_max = 0.0;
		}

		public Interval(double min, double max)
		{
			Initialize(min, max);
		}

		public Interval(Interval interval)
		{
			Initialize( interval.Min, interval.Max );
		}

		#endregion

		#region Properties
		public double Min
		{
			get
			{
				return _min;
			}
			set
			{
				_min = value;
			}
		}
		public double Max
		{
			get
			{
				return _max;
			}
			set
			{
				_max = value;
			}
		}
		#endregion

		#region Methods
		public void Initialize(double min, double max)
		{
			_min = min;
			_max = max;
			if ( _min > _max ) 
			{
				_min = max;
				_max = min;
			}
		}
		public double GetWidth()
		{ 
			return _max - _min; 
		}

		public void ExpandToInclude( Interval interval )
		{
			if ( interval.Max > _max ) _max = interval.Max;
			if ( interval.Min < _min ) _min = interval.Min;
		}

		public bool Overlaps( Interval interval )
		{
			return Overlaps( interval.Min, interval.Max );
		}

		public bool Overlaps( double min, double max )
		{
			if ( _min > max || _max < min) return false;
			return true;
		}

		public bool Contains( Interval interval )
		{
			return Contains( interval.Min, interval.Max );
		}

		public bool Contains( double min, double max )
		{
			return ( min >= _min && max <= _max );
		}

		public bool Contains( double p )
		{
			return ( p >= _min && p <= _max );
		}

		#endregion
	}
}
