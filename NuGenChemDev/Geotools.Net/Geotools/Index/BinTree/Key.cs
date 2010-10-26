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
using Geotools.Index.Quadtree;
#endregion

namespace Geotools.Index.BinTree
{
	/// <summary>
	/// Summary description for Key.
	/// </summary>
	internal class Key
	{
		// the fields which make up the key
		private double _pt = 0.0;
		private int _level = 0;
		// auxiliary data which is derived from the key for use in computation
		private Interval _interval;



		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Key class.
		/// </summary>
		public Key( Interval interval )
		{
			ComputeKey( interval );
		}
		#endregion

		#region Properties
		public double Point
		{
			get
			{
				return _pt;
			}
		}
		public int Level
		{
			get
			{
				return _level;
			}
		}
		public Interval Interval
		{
			get
			{
				return _interval;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Return a square envelope containing the argement envelope, whose extent is a power of two and
		/// which is based at a power of 2.
		/// </summary>
		/// <param name="itemInterval"></param>
		public void ComputeKey( Interval itemInterval )
		{
			_level = Key.ComputeLevel( itemInterval );
			_interval = new Interval();
			ComputeInterval( _level, itemInterval );

			// MD - would be nice to have a non-iterative form of this algorithm
			while ( !_interval.Contains( itemInterval ) ) 
			{
				_level += 1;
				ComputeInterval( _level, itemInterval );
			}
		}

		private void ComputeInterval( int level, Interval itemInterval )
		{
			double size = DoubleBits.PowerOf2( level );
			//double size = pow2.power(level);
			_pt = Math.Floor( itemInterval.Min / size) * size;
			_interval.Initialize( _pt, _pt + size );
		}
		#endregion

		#region Static Methods
		public static int ComputeLevel( Interval interval )
		{
			double dx = interval.GetWidth();
			//int level = BinaryPower.exponent(dx) + 1;
			int level = DoubleBits.Exponent(dx) + 1;
			return level;
		}

		#endregion



	}
}
