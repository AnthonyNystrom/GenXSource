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

namespace Geotools.Index.Chain
{
	/// <summary>
	/// Summary description for MonotoneChainOverlapAction.
	/// </summary>
	internal class MonotoneChainOverlapAction
	{
		LineSegment _seg1 = new LineSegment();
		LineSegment _seg2 = new LineSegment();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the MonotoneChainOverlapAction class.
		/// </summary>
		public MonotoneChainOverlapAction()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// This function can be overridden if the original chains are needed.
		/// </summary>
		/// <param name="mc1"></param>
		/// <param name="start1"></param>
		/// <param name="mc2"></param>
		/// <param name="start2"></param>
		public void Overlap(MonotoneChain mc1, int start1, MonotoneChain mc2, int start2)
		{
			mc1.GetLineSegment( start1, ref _seg1 );
			mc2.GetLineSegment( start2, ref _seg2 );
			Overlap( _seg1, _seg2 );		// This does nothing.
		} // public void Overlap(MonotoneChain mc1, int start1, MonotoneChain mc2, int start2)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seg1"></param>
		/// <param name="seg2"></param>
		public void Overlap(LineSegment seg1, LineSegment seg2)
		{
		}
		#endregion

	}
}
