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
using System.Diagnostics;
using System.Collections;
using System.Text;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// Summary description for DirectedEdgeStar.
	/// </summary>
	internal class DirectedEdgeStar : EdgeEndStar
	{
		// A list of all outgoing edges in the result, in CCW order
		private ArrayList _resultAreaEdgeList;
		private Label _label;
	
		private const int ScanningForIncoming = 1;
		private const int LinkingToOutGoing = 2;


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the DirectedEdgeStar class.
		/// </summary>
		public DirectedEdgeStar()
		{
		}
		#endregion

		#region Properties
		public Label Label 
		{
			get
			{
				return _label;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		///  Insert a directed edge in the list.
		/// </summary>
		/// <param name="ee">The EdgeEnd to be inserted into the list.  Must be of type DirectedEdge.</param>
		public override void Insert(EdgeEnd ee)
		{
			DirectedEdge de = (DirectedEdge) ee;
			InsertEdgeEnd( de, de );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetOutgoingDegree()
		{
			int degree = 0;
			foreach ( DirectedEdge de in Edges() ) 
			{
				if ( de.InResult )
				{
					degree++;
				}
			}
			return degree;
		} // public int GetOutgoingDegree()


		/// <summary>
		/// 
		/// </summary>
		/// <param name="er"></param>
		/// <returns></returns>
		public int GetOutgoingDegree( EdgeRing er )
		{
			int degree = 0;
			foreach ( DirectedEdge de in Edges() ) 
			{
				if ( de.EdgeRing == er)
				{
					degree++;
				}
			}
			return degree;
		} // public int GetOutgoingDegree( EdgeRing er )

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DirectedEdge GetRightmostEdge()
		{
			ArrayList edges = Edges();
			int size = edges.Count;
			if (size < 1) return null;
			DirectedEdge de0 = (DirectedEdge)edges[0];
			if (size == 1) return de0;
			DirectedEdge deLast = (DirectedEdge)edges[size - 1];
			
			int quad0 = de0.QuadrantLocation;
			int quad1 = deLast.QuadrantLocation;
			if (  Quadrant.IsNorthern( quad0 ) && Quadrant.IsNorthern( quad1 )  )
			{
				return de0;
			}
			else if (  !Quadrant.IsNorthern( quad0 ) && !Quadrant.IsNorthern( quad1 )  )
			{
				return deLast;
			}
			else 
			{
				// edges are in different hemispheres - make sure we return one that is non-horizontal
				//Assert.isTrue(de0.getDy() != 0, "should never return horizontal edge!");
				if ( de0.Dy != 0 )
				{
					return de0;
				}
				else if ( deLast.Dy != 0 )
				{
					return deLast;
				}
			}

			throw new InvalidOperationException("found two horizontal edges incident on node");
		} // public DirectedEdge GetRightmostEdge()
	

		/// <summary>
		/// Compute the labelling for all dirEdges in this star, as well
		///  as the overall labelling
		/// </summary>
		/// <param name="geom"></param>
		public override void ComputeLabelling(GeometryGraph[] geom)
		{
			//Trace.WriteLine( ToString() );
			base.ComputeLabelling( geom );

			// determine the overall labelling for this DirectedEdgeStar
			// (i.e. for the node it is based at)
			_label = new Label( Location.Null );
			foreach ( object obj in Edges() ) 
			{
				EdgeEnd ee = (EdgeEnd)obj;
				Edge e = ee.Edge;
				Label eLabel = e.Label;
				for (int i = 0; i < 2; i++) 
				{
					int eLoc = eLabel.GetLocation(i);
					if ( eLoc == Location.Interior || eLoc == Location.Boundary )
					{
						_label.SetLocation( i, Location.Interior );
					}
				}
			} // foreach ( object obj in edges )

			//Trace.WriteLine( ToString() );
		} // public override void ComputeLabelling(GeometryGraph[] geom)

		/// <summary>
		/// For each dirEdge in the star, merge the label from the sym dirEdge into the label.
		/// </summary>
		public void MergeSymLabels()
		{
			foreach ( DirectedEdge de in Edges() ) 
			{
				Label label = de.Label;
				label.Merge( de.Sym.Label );
			}
		} // public void MergeSymLabels()
		

		/// <summary>
		/// Update incomplete dirEdge labels from the labelling for the node.
		/// </summary>
		/// <param name="nodeLabel"></param>
		public void UpdateLabelling(Label nodeLabel)
		{
			foreach ( DirectedEdge de in Edges() ) 
			{
				Label label = de.Label;
				label.SetAllLocationsIfNull( 0, nodeLabel.GetLocation( 0 ) );
				label.SetAllLocationsIfNull( 1, nodeLabel.GetLocation( 1 ) );
			}
		}  // public void UpdateLabelling(Label nodeLabel)


		private ArrayList GetResultAreaEdges()
		{
			//Trace.WriteLine( ToString() );
			if ( _resultAreaEdgeList != null )
			{
				return _resultAreaEdgeList;
			}
			_resultAreaEdgeList = new ArrayList();
			foreach ( DirectedEdge de in Edges() ) 
			{
				if ( de.InResult || de.Sym.InResult )
				{
					_resultAreaEdgeList.Add( de );
				}
			}
			return _resultAreaEdgeList;
		} // private ArrayList GetResultAreaEdges()


		/// <summary>
		/// Traverse the star of DirectedEdges, linking the included edge together.
		/// To link two dirEdges, the next pointer for an incoming dirEdge is set to the next outgoing edge.
		/// 
		/// DirEdges are only  linked. If:
		/// &lt;ul&gt;
		/// &lt;li&gt;They belong to an area (i.e. they have sides)
		/// &lt;li&gt;They are marked as being in the result.
		/// &lt;/ul&gt;
		/// Edges are linked in CCW order (the order they are stored.) This means that rings have
		/// their face on the Right ( in other words, the topological location of the face is given
		/// by the RHS label of the DirectedEdge)
		/// 
		/// PRECONDITION: No pair of dirEdges are both marked as being in the result.
		/// </summary>
		public void LinkResultDirectedEdges()
		{
			// make sure edges are copied to resultAreaEdges list
			GetResultAreaEdges();

			// find first area edge (if any) to start linking at
			DirectedEdge firstOut = null;
			DirectedEdge incoming = null;
			int state = ScanningForIncoming;

			// link edges in CCW order
			for (int i = 0; i < _resultAreaEdgeList.Count; i++) 
			{
				DirectedEdge nextOut = (DirectedEdge) _resultAreaEdgeList[i];
				DirectedEdge nextIn = nextOut.Sym;

				// skip de's that we're not interested in
				if ( nextOut.Label.IsArea() )
				{
					// record first outgoing edge, in order to link the last incoming edge
					if ( firstOut == null && nextOut.InResult )
					{
						firstOut = nextOut;
					}

					// assert: sym.isInResult() == false, since pairs of dirEdges should have been removed already

					switch ( state ) 
					{
						case ScanningForIncoming:
							if ( !nextIn.InResult ) continue;
							incoming = nextIn;
							state = LinkingToOutGoing;
							break;
						case LinkingToOutGoing:
							if ( !nextOut.InResult ) continue;
							incoming.Next = nextOut;
							state = ScanningForIncoming;
							break;
					} // switch ( state )

				} // if ( nextOut.Label.IsArea() )
			} // for (int i = 0; i < _resultAreaEdgeList.Count; i++)

			//Trace.WriteLine( ToString() );

			if ( state == LinkingToOutGoing ) 
			{
				if (firstOut == null)
				{
					throw new TopologyException("No outgoing dirEdge found." );
				}
				if (!firstOut.InResult)
				{
					throw new TopologyException("Unable to link last incoming dirEdge." );
				}
				incoming.Next = firstOut;
			}
		} // public void LinkResultDirectedEdges()


		/// <summary>
		/// 
		/// </summary>
		/// <param name="er"></param>
		public void LinkMinimalDirectedEdges(EdgeRing er)
		{
			// find first area edge (if any) to start linking at
			DirectedEdge firstOut = null;
			DirectedEdge incoming = null;
			int state = ScanningForIncoming;

			// link edges in CW order
			for ( int i = _resultAreaEdgeList.Count - 1; i >= 0; i-- ) 
			{
				DirectedEdge nextOut = (DirectedEdge)_resultAreaEdgeList[i];
				DirectedEdge nextIn = nextOut.Sym;

				// record first outgoing edge, in order to link the last incoming edge
				if ( firstOut == null && nextOut.EdgeRing == er)
				{
					firstOut = nextOut;
				}

				switch ( state ) 
				{
					case ScanningForIncoming:
						if ( nextIn.EdgeRing != er) continue;
						incoming = nextIn;
						state = LinkingToOutGoing;
						break;
					case LinkingToOutGoing:
						if ( nextOut.EdgeRing != er) continue;
						incoming.NextMin = nextOut;
						state = ScanningForIncoming;
						break;
				} // switch ( state )
			} // for ( int i = _resultAreaEdgeList.Count - 1; i >= 0; i-- ) 

			//Trace.WriteLine( ToString() );

			if ( state == LinkingToOutGoing ) 
			{
				Trace.Assert( firstOut != null, "found null for first outgoing dirEdge");
				Trace.Assert( firstOut.EdgeRing == er, "unable to link last incoming dirEdge");
				incoming.NextMin = firstOut;
			} // if ( state == LinkingToOutGoing )

		} // public void LinkMinimalDirectedEdges(EdgeRing er)

		/// <summary>
		/// 
		/// </summary>
		public void LinkAllDirectedEdges()
		{
			// makes sure _edgeList is set properly.
			Edges();

			// find first area edge (if any) to start linking at
			DirectedEdge prevOut = null;
			DirectedEdge firstIn = null;

			// link edges in CW order
			for ( int i = _edgeList.Count - 1; i >= 0; i-- ) 
			{
				DirectedEdge nextOut = (DirectedEdge) _edgeList[i];
				DirectedEdge nextIn = nextOut.Sym;
				if ( firstIn == null )
				{
					firstIn = nextIn;
				}
				if ( prevOut != null )
				{
					nextIn.Next = prevOut;
				}

				// record outgoing edge, in order to link the last incoming edge
				prevOut = nextOut;
			}

			firstIn.Next = prevOut;
			//Trace.WriteLine( ToString() );
		} // public void LinkAllDirectedEdges()

		/// <summary>
		/// Traverse the star edges, maintaing the current location in the result area
		/// at this node (if any).  If any L edges are found in the interior of the result,
		/// mark them as covered.
		/// </summary>
		public void FindCoveredLineEdges()
		{
			//Trace.WriteLine("findCoveredLineEdges");
			//Trace.WriteLine( ToString() );
			// Since edges are stored in CCW order around the node,
			// as we move around the ring we move from the right to the left side of the edge

			
			 // Find first DirectedEdge of result area (if any).
			 // The interior of the result is on the RHS of the edge,
			 // so the start location will be:
			 // - INTERIOR if the edge is outgoing
			 // - EXTERIOR if the edge is incoming			
			int startLoc = Location.Null;
			foreach ( DirectedEdge de in Edges() ) 
			{
				DirectedEdge nextOut  = de;
				DirectedEdge nextIn   = nextOut.Sym;

				if ( !nextOut.IsLineEdge ) 
				{
					if ( nextOut.InResult ) 
					{
						startLoc = Location.Interior;
						break;
					}
					if ( nextIn.InResult ) 
					{
						startLoc = Location.Exterior;
						break;
					}
				}
			} // foreach ( object obj in edges )

			// no A edges found, so can't determine if L edges are covered or not
			if ( startLoc == Location.Null ) return;
			
			
			 // move around ring, keeping track of the current location
			 // (Interior or Exterior) for the result area.
			 // If L edges are found, mark them as covered if they are in the interior
			int currLoc = startLoc;
			foreach ( DirectedEdge de in Edges() ) 
			{
				DirectedEdge nextOut  = de;
				DirectedEdge nextIn   = nextOut.Sym;
				if ( nextOut.IsLineEdge ) 
				{
					nextOut.Edge.SetCovered = (currLoc == Location.Interior);
					//Trace.WriteLine( nextOut.ToString() );
				}
				else 
				{  // edge is an Area edge
					if ( nextOut.InResult )
					{
						currLoc = Location.Exterior;
					}
					if ( nextIn.InResult )
					{
						currLoc = Location.Interior;
					}
				}
			} // foreach ( object obj in edges )
		} // public void FindCoveredLineEdges()

		public void ComputeDepths(DirectedEdge de)
		{
			int edgeIndex = FindIndex(de);
			Label label = de.Label;
			int startDepth = de.GetDepth( Position.Left );

			// compute the depths from this edge up to the end of the edge array
			int nextDepth = ComputeDepths( edgeIndex + 1, _edgeList.Count - 1, startDepth );

			// compute the depths for the initial part of the array
			int lastDepth = ComputeDepths( 0, edgeIndex, nextDepth );

			//Trace.WriteLine( ToString() );
			Trace.Assert( lastDepth == startDepth, "depth mismatch at " + de.Coordinate.ToString() );
		} // public void ComputeDepths(DirectedEdge de)


		private int ComputeDepths( int startIndex, int endIndex, int startDepth )
		{
			int currDepth = startDepth;
			for ( int i = startIndex; i <= endIndex ; i++ ) 
			{
				DirectedEdge nextDe = (DirectedEdge) _edgeList[i];
				Label label = nextDe.Label;
				nextDe.SetEdgeDepths( Position.Right, currDepth );
				currDepth = nextDe.GetDepth( Position.Left );
			}
			return currDepth;
		} // private int ComputeDepths( int startIndex, int endIndex, int startDepth )

		/// <summary>
		/// Returns the string representation of DirectedEdgeStar.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("DirectedEdgeStar: ");
			Coordinate c = GetCoordinate();
			if ( c != null )
			{
				sb.Append( c.ToString() );
			}
			foreach ( DirectedEdge de in Edges() ) 
			{
				sb.Append( de.ToString() );
			} // foreach ( object obj in edges )

			return sb.ToString();
		} // public override string ToString()
		#endregion

	}
}
