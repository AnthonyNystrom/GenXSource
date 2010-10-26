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

namespace Geotools.Graph
{
	/// <summary>
	/// A Depth object records the topological depth of the sides of an Edge for up to two Geometries.
	/// </summary>
	internal class Depth
	{
		private const int NULL= -1;
		private int[,] _depth = new int[2,3];

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Depth class.
		/// </summary>
		public Depth() 
		{
			// initialize depth array to a sentinel value
			for (int i = 0; i < 2; i++) 
			{
				for (int j = 0; j < 3; j++) 
				{
					_depth[i,j] = NULL;
				}
			}
		}
		#endregion

		#region Static Methods
		public static int DepthAtLocation(int location)
		{
			if ( location == Location.Exterior ) return 0;
			if ( location == Location.Interior ) return 1;
			return NULL;
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="posIndex"></param>
		/// <returns></returns>
		public int GetDepth(int geomIndex, int posIndex)
		{
			return _depth[geomIndex,posIndex];
		}

		/// <summary>
		/// Sets the depth at geomIndex, posIndex to depthValue.
		/// </summary>
		/// <param name="geomIndex">The geometry index.</param>
		/// <param name="posIndex"></param>
		/// <param name="depthValue"></param>
		public void SetDepth(int geomIndex, int posIndex, int depthValue)
		{
			_depth[geomIndex,posIndex] = depthValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="posIndex"></param>
		/// <returns></returns>
		public int GetLocation(int geomIndex, int posIndex)
		{
			if ( _depth[geomIndex,posIndex] <= 0 )
			{
				return Location.Exterior;
			}

			return Location.Interior;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="posIndex"></param>
		/// <param name="location"></param>
		public void Add(int geomIndex, int posIndex, int location)
		{
			if ( location == Location.Interior )
			{
				_depth[geomIndex,posIndex]++;
			}
		}

		/// <summary>
		/// A Depth object is null (has never been initialized) if all depths are null.
		/// </summary>
		/// <returns></returns>
		public bool IsNull()
		{
			for (int i = 0; i < 2; i++) 
			{
				for (int j = 0; j < 3; j++) 
				{
					if (_depth[i,j] != NULL)
					{
						return false;
					}
				}
			}
			return true;	
		} // public bool IsNull()


		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <returns></returns>
		public bool IsNull(int geomIndex)
		{
			return _depth[geomIndex,1] == NULL;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="posIndex"></param>
		/// <returns></returns>
		public bool IsNull(int geomIndex, int posIndex)
		{
			return _depth[geomIndex,posIndex] == NULL;	
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lbl"></param>
		public void Add(Label lbl)
		{
			for (int i = 0; i < 2; i++) 
			{
				for (int j = 1; j < 3; j++) 
				{
					int loc = lbl.GetLocation( i, j );
					if ( loc == Location.Exterior || loc == Location.Interior ) 
					{
						// initialize depth if it is null, otherwise add this location value
						if (IsNull(i, j)) 
						{
							_depth[i,j] = DepthAtLocation(loc);
						}
						else
							_depth[i,j] += DepthAtLocation(loc);
					}
				}
			} // for (int i = 0; i < 2; i++)
		} // public void Add(Label lbl)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <returns></returns>
		public int GetDelta(int geomIndex)
		{
			return _depth[geomIndex,Position.Right] - _depth[geomIndex,Position.Left];
		}

		/// <summary>
		/// Normalize the depths for each geometry.
		/// </summary>
		/// <remarks>
		/// A normalized depth has depth values in the set { 0, 1 }.
		/// Normalizing the depths involves reducing the depths by the same amount so that at least
		/// one of them is 0.  If the remaining value is > 0, it is set to 1.
		/// </remarks>
		public void Normalize()
		{
			for (int i = 0; i < 2; i++) 
			{
				if ( !IsNull(i) ) 
				{
					int minDepth = _depth[i,1];
					if ( _depth[i,2] < minDepth )
					{
						minDepth = _depth[i,2];
					}

					if (minDepth < 0) minDepth = 0;
					for ( int j = 1; j < 3; j++ ) 
					{
						int newValue = 0;
						if (_depth[i,j] > minDepth)
						{
							newValue = 1;
						}
						_depth[i,j] = newValue;
					} // for ( int j = 1; j < 3; j++ )
				} // if ( !IsNull(i) )
			} // for (int i = 0; i < 2; i++)
		} // public void Normalize()


		public override string ToString()
		{
			return
				"A: " + _depth[0,1] + "," + _depth[0,2]
				+ " B: " + _depth[1,1] + "," + _depth[1,2];
		}
		#endregion

	}
}
