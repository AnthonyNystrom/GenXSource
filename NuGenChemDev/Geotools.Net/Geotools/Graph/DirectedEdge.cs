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
	/// Summary description for DirectedEdge.
	/// </summary>
	internal class DirectedEdge : EdgeEnd
	{
		protected bool _isForward;
		private bool _isInResult = false;
		private bool _isVisited = false;

		private DirectedEdge _sym; // the symmetric edge
		private DirectedEdge _next;  // the next edge in the edge ring for the polygon containing this edge
		private DirectedEdge _nextMin;  // the next edge in the MinimalEdgeRing that contains this edge
		private EdgeRing _edgeRing;  // the EdgeRing that this edge is part of
		private EdgeRing _minEdgeRing;  // the MinimalEdgeRing that this edge is part of

		/**
		* The depth of each side (position) of this edge.
		* The 0 element of the array is never used.
		*/
		private int[] _depth = { 0, -999, -999 };

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the DirectedEdge class.
		/// </summary>
		public DirectedEdge( Edge edge, bool isForward) :  base( edge )
		{
			_isForward = isForward;
			
			if ( _isForward ) 
			{
				Initialize( _edge.Coordinates[0], _edge.Coordinates[1] );
			}
			else 
			{
				int n = _edge.Count - 1;
				Initialize( _edge.Coordinates[n], _edge.Coordinates[n-1] );
			}
			ComputeDirectedLabel();
		} // public DirectedEdge( Edge edge, bool isForward) :  base( edge )
		#endregion

		#region Static Methods
		/// <summary>
		/// Computes the factor for the change in depth when moving from one location to another.
		/// E.g. if crossing from the INTERIOR to the EXTERIOR the depth decreases, so the factor is -1
		/// </summary>
		/// <param name="currLocation"></param>
		/// <param name="nextLocation"></param>
		/// <returns></returns>
		public static int DepthFactor(int currLocation, int nextLocation)
		{
			if ( currLocation == Location.Exterior && nextLocation == Location.Interior )
			{
				return 1;
			}
			else if (currLocation == Location.Interior && nextLocation == Location.Exterior)
			{
				return -1;
			}
			return 0;
		} // public static int DepthFactor(int currLocation, int nextLocation)
		#endregion

		#region Properties
		
		/// <summary>
		/// 
		/// </summary>
		public bool InResult
		{
			get
			{
				return _isInResult;
			}
			set
			{
				_isInResult = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool Visited 
		{
			get
			{
				return _isVisited;
			}
			set
			{
				_isVisited = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public EdgeRing EdgeRing
		{
			get
			{
				return _edgeRing;
			}
			set
			{
				_edgeRing = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public EdgeRing MinEdgeRing
		{
			get
			{
				return _minEdgeRing;
			}
			set
			{
				_minEdgeRing = value;
			}
		}
		
		
		/// <summary>
		/// Each Edge gives rise to a pair of symmetric DirectedEdges, in opposite directions.
		/// </summary>
		/// <returns>The DirectedEdge for the same Edge but in the opposite direction</returns>
		public DirectedEdge Sym 
		{
			get
			{
				return _sym;
			}
			set
			{
				_sym = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsForward
		{
			get
			{
				return _isForward;
			}
			set
			{
				_isForward = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public DirectedEdge Next
		{
			get
			{
				return _next;
			}
			set
			{
				_next = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public DirectedEdge NextMin
		{
			get
			{
				return _nextMin;
			}
			set
			{
				_nextMin = value;
			}
		}

		/// <summary>
		/// This edge is a line edge if:
		/// &lt;ul&gt;
		/// &lt;li&gt;at least one of the labels is a line label.
		/// &lt;li&gt;any labels which are not line labels have all Locations = Exterior
		/// &lt;/ul&gt;
		/// </summary>
		public bool IsLineEdge
		{
			get
			{
				bool isLine = _label.IsLine( 0 ) || _label.IsLine( 1 );
				bool isExteriorIfArea0 = !_label.IsArea(0) || _label.AllPositionsEqual( 0, Location.Exterior );
				bool isExteriorIfArea1 = !_label.IsArea(1) || _label.AllPositionsEqual( 1, Location.Exterior );
				return isLine && isExteriorIfArea0 && isExteriorIfArea1;
			}
		} // public bool IsLineEdge

		/// <summary>
		/// This is an interior Area Edge if:
		/// &lt;ul&gt;
		/// &lt;li&gt;its label is an Area label for both Geometries.
		/// &lt;li&gt;and for each Geometry both sides are in the interior.
		/// &lt;/ul&gt;
		/// Returns true is this is an interior Area edge.
		/// </summary>
		public bool IsInteriorAreaEdge
		{
			get
			{
				bool isInteriorAreaEdge = true;
				for (int i = 0; i < 2; i++) 
				{
					if (! ( _label.IsArea( i )
						 && _label.GetLocation(i, Position.Left ) == Location.Interior
						 && _label.GetLocation(i, Position.Right) == Location.Interior ) ) 
					{
						isInteriorAreaEdge = false;
					}
				} // for (int i = 0; i < 2; i++)
				
				return isInteriorAreaEdge;
			}
		} // public bool IsInteriorAreaEdge
		#endregion

		#region Methods
		
		/// <summary>
		/// Returns the depth of the position.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public int GetDepth(int position)
		{
			return _depth[position]; 

		}

		/// <summary>
		/// Sets the depth of the position.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="depth"></param>
		public void SetDepth(int position, int depth)
		{
			_depth[position] = depth; 
		}

		/// <summary>
		/// SetVisitedEdge marks both DirectedEdges attached to a given Edge.
		/// </summary>
		/// <remarks>
		/// This is used for edges corresponding to lines, which will only
		/// appear oriented in a single direction in the result.
		/// </remarks>
		/// <param name="isVisited"></param>
		public void SetVisitedEdge(bool isVisited)
		{
			Visited = isVisited;
			_sym.Visited = isVisited;
		}

		/// <summary>
		/// Compute the label in the appropriate orientation for this DirEdge.
		/// </summary>
		private void ComputeDirectedLabel()
		{
			_label = new Label( _edge.Label );
			if (!IsForward )
			{
				_label.Flip();
			}
		} // private void ComputeDirectedLabel()

		/// <summary>
		/// Set both edge depths.  One depth for a given side is provided.  The other is
		/// computed depending on the Location transition and the depthDelta of the edge.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="depth"></param>
		public void SetEdgeDepths(int position, int depth)
		{
			int depthDelta = _edge.DepthDelta;
			int loc = _label.GetLocation( 0, position );
			int oppositePos = Position.Opposite( position );
			int oppositeLoc = _label.GetLocation( 0, oppositePos );
			int delta = Math.Abs( depthDelta ) * DirectedEdge.DepthFactor(loc, oppositeLoc);
			//TESTINGint delta = depthDelta * DirectedEdge.depthFactor(loc, oppositeLoc);
			int oppositeDepth = depth + delta;
			SetDepth( position, depth );
			SetDepth( oppositePos, oppositeDepth );
		} // public void SetEdgeDepths(int position, int depth)
		#endregion

	}
}
