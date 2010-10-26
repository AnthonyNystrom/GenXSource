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
using Geotools.Graph;
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Relate
{
	/// <summary>
	/// Summary description for RelateNode.
	/// </summary>
	internal class RelateNode : Node
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the RelateNode class.
		/// </summary>
		public RelateNode( Coordinate coordinate, EdgeEndStar edges ) : base( coordinate, edges )
		{	
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		/// <summary>
		/// Update the IM with the contribution for this component.
		/// A component only contributes if it has a labelling for both parent geometries
		/// </summary>
		/// <param name="im"></param>
		protected override void ComputeIM( IntersectionMatrix im )
		{
			im.SetAtLeastIfValid( _label.GetLocation( 0 ), _label.GetLocation( 1 ), 0 );
		} // protected override void ComputeIM( IntersectionMatrix im )

		/// <summary>
		/// Update the IM with the contribution for the EdgeEnds incident on this node.
		/// </summary>
		/// <param name="im"></param>
		public void UpdateIMFromEdges( IntersectionMatrix im )
		{
			((EdgeEndBundleStar) _edges).UpdateIM( im );
		} // void UpdateIMFromEdges( IntersectionMatrix im )
		#endregion

	} // public class RelateNode : Node
}
