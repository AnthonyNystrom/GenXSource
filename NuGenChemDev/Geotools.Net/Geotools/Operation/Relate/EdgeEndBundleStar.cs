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
using System.Collections;
using Geotools.Graph;
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Relate
{
	/// <summary>
	/// Summary description for EdgeEndBundleStar.
	/// </summary>
	internal class EdgeEndBundleStar : EdgeEndStar
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeEndBundleStar class.
		/// </summary>
		public EdgeEndBundleStar()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Insert an EdgeEnd in order in the list.  If there is an existing EdgeStubBundle which is parallel,
		/// the EdgeEnd is added to the bundle.  Otherwise, a new EdgeEndBundle is created to contain the EdgeEnd.
		/// </summary>
		/// <param name="e"></param>
		public override void Insert( EdgeEnd e )
		{
			EdgeEndBundle eb = (EdgeEndBundle) _edgeMap[ e ];
			if (eb == null) 
			{
				eb = new EdgeEndBundle( e );
				InsertEdgeEnd( e, eb );
			}
			else 
			{
				eb.Insert( e );
			}
		} // public override void Insert( EdgeEnd e )


		/// <summary>
		/// Update the IM with the contribution for the EdgeStubs around the node.
		/// </summary>
		/// <param name="im"></param>
		public void UpdateIM( IntersectionMatrix im )
		{
			ArrayList edges = Edges();
			foreach ( object obj in edges ) 
			{
				EdgeEndBundle esb = (EdgeEndBundle) obj;
				esb.UpdateIM( im );
			}
		} // void UpdateIM( IntersectionMatrix im )

		#endregion

	}
}
