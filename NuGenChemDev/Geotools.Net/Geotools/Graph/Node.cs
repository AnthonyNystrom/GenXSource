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
using System.IO;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// Summary description for Node.
	/// </summary>
	internal class Node : GraphComponent
	{
		protected Coordinate _coord; // only non-null if this node is precise
		protected EdgeEndStar _edges;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Node class.
		/// </summary>
		public Node(Coordinate coord, EdgeEndStar edges)
		{
			_coord = coord;
			_edges = edges;
			_label = new Label( 0, Location.Null );
		}
		#endregion

		#region Properties
		public Coordinate Coordinate 
		{
			get
			{
				return _coord;
			}
		}
		public EdgeEndStar Edges
		{
			get
			{
				return _edges;
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Basic nodes do not compute IMs.
		/// </summary>
		/// <param name="im"></param>
		protected override void ComputeIM( IntersectionMatrix im )
		{
		}

		/// <summary>
		/// Add the edge to the list of edges at this node.
		/// </summary>
		/// <param name="e"></param>
		public void Add( EdgeEnd e )
		{
			// Assert: start pt of e is equal to node point
			_edges.Insert( e );
			e.Node = this;
		} // public void Add(EdgeEnd e)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"></param>
		public void MergeLabel( Node n )
		{
			MergeLabel( n.Label );
		}

		public override bool IsIsolated()
		{
			return (_label.GetGeometryCount() == 1);
		}
	
		/// <summary>
		/// To merge labels for two nodes, the merged location for each LabelElement is computed.
		/// The location for the corresponding node LabelElement is set to the result, as long as the location is non-null.
		/// </summary>
		/// <param name="label2"></param>
		public void MergeLabel( Label label2 )
		{
			for (int i = 0; i < 2; i++) 
			{
				int loc = ComputeMergedLocation(label2, i);
				int thisLoc = _label.GetLocation(i);
				if ( thisLoc == Location.Null )
				{
					_label.SetLocation( i, loc );
				}
			} // for (int i = 0; i < 2; i++)

		} // public void MergeLabel(Label label2)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="argIndex"></param>
		/// <param name="onLocation"></param>
		public void SetLabel( int argIndex, int onLocation )
		{
			if ( _label == null ) 
			{
				_label = new Label( argIndex, onLocation );
			}
			else
			{
				_label.SetLocation( argIndex, onLocation );
			}
		} // public void SetLabel(int argIndex, int onLocation)


		/// <summary>
		/// Updates the label of a node to BOUNDARY, obeying the mod-2 boundaryDetermination rule.
		/// </summary>
		/// <param name="argIndex"></param>
		public void SetLabelBoundary( int argIndex )
		{
			// determine the current location for the point (if any)
			int loc = Location.Null;
			if ( _label != null )
			{
				loc = _label.GetLocation( argIndex );
			}

			// flip the loc
			int newLoc;
			switch ( loc ) 
			{
				case 1:		// Location.Boundary
					newLoc = Location.Interior; 
					break;
				case 0:			// Location.Interior
					newLoc = Location.Boundary;
					break;
				default:
					newLoc = Location.Boundary;
					break;
			}
			_label.SetLocation( argIndex, newLoc );

		} // public void SetLabelBoundary( int argIndex )

		/// <summary>
		/// The location for a given eltIndex for a node will be one
		/// of { null, INTERIOR, BOUNDARY }.
		/// A node may be on both the boundary and the interior of a geometry;
		/// in this case, the rule is that the node is considered to be in the boundary.
		/// The merged location is the maximum of the two input values.
		/// </summary>
		/// <param name="label2"></param>
		/// <param name="eltIndex"></param>
		/// <returns></returns>
		int ComputeMergedLocation(Label label2, int eltIndex)
		{
			int loc = Location.Null;
			loc = _label.GetLocation( eltIndex );
			if ( !label2.IsNull( eltIndex ) ) 
			{
				int nLoc = label2.GetLocation( eltIndex );
				if (loc != Location.Boundary)
				{
					loc = nLoc;
				}
			}
			return loc;

		} // int ComputeMergedLocation(Label label2, int eltIndex)

		/// <summary>
		/// Returns the Coordinate for this node.
		/// </summary>
		/// <returns></returns>
		public override Coordinate GetCoordinate()
		{
			return _coord;
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "node:" + _coord.ToString() +  "IsIsolated:" + IsIsolated().ToString() + " Edges:" + _edges.ToString();
		}
		#endregion

	} // public class Node : GraphComponent
}
