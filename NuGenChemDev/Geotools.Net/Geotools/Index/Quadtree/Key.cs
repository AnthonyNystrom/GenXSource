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

namespace Geotools.Index.Quadtree
{
	/// <summary>
	/// A Key is a unique identifier for a node in a quadtree.
	/// It contains a lower-left point and a level number. The level number
	/// is the power of two for the size of the node envelope.
	/// </summary>
	internal class Key 
	{
		// the fields which make up the key
		private Coordinate _pt = new Coordinate();
		private int _level = 0;
		// auxiliary data which is derived from the key for use in computation
		private Envelope _env = null;

		#region Constructors
		public Key(Envelope itemEnv)
		{
			ComputeKey( itemEnv );
		}
		#endregion

		#region Properties
		public Coordinate Point
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
		public Envelope Envelope
		{
			get
			{ 
				return _env; 
			}
		}
		#endregion

		#region Methods
		public Coordinate GetCentre()
		{
			return new Coordinate(
				( _env.MinX + _env.MaxX ) / 2,
				( _env.MinY + _env.MaxY ) / 2
				);
		}
		/// <summary>
		/// Return a square envelope containing the argument envelope,
		/// whose extent is a power of two and which is based at a power of 2.
		/// </summary>
		/// <param name="itemEnv"></param>
		public void ComputeKey(Envelope itemEnv)
		{
			_level = ComputeQuadLevel(itemEnv);
			_env = new Envelope();
			ComputeKey( _level, itemEnv );
			// MD - would be nice to have a non-iterative form of this algorithm
			while ( !_env.Contains( itemEnv ) ) 
			{
				_level += 1;
				ComputeKey( _level, itemEnv );
			}
		}

		private void ComputeKey( int level, Envelope itemEnv )
		{
			double quadSize = DoubleBits.PowerOf2(level);
			//double quadSize = pow2.power(level);
			_pt.X = Math.Floor(itemEnv.MinX / quadSize) * quadSize;
			_pt.Y = Math.Floor(itemEnv.MinY / quadSize) * quadSize;
			_env.Initialize( _pt.X, _pt.X + quadSize, _pt.Y, _pt.Y + quadSize );
		}
		#endregion

		#region Static Methods
		public static int ComputeQuadLevel( Envelope env )
		{
			double dx = env.Width;
			double dy = env.Height;
			double dMax = dx > dy ? dx : dy;
			int level = DoubleBits.Exponent( dMax ) + 1;
			return level;
		}
		#endregion
	}
}
