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
using System.Diagnostics;
using System.Collections;
using System.Text;
using Geotools.Algorithms;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// A EdgeEndStar is an ordered list of EdgeEnds around a node.
	/// They are maintained in CCW order (starting with the positive x-axis) around the node
	/// for efficient lookup and topology building.
	/// </summary>
	internal abstract class EdgeEndStar : System.Collections.IEnumerable
	{
		/// <summary>
		/// A map which maintains the edges in sorted order around the node.  Uses the IComparable interface of the
		/// keys which are added to the list to maintain its sorted order.
		/// </summary>
		protected SortedList _edgeMap = new SortedList();

		/// <summary>
		/// A list of all outgoing edges in the result, in CCW order.
		/// </summary>
		protected ArrayList _edgeList;

		/// <summary>
		/// The location of the point for this star in Geometry i areas.
		/// </summary>
		private int[] _ptInAreaLocation = { Location.Null, Location.Null };


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeEndStart class.
		/// </summary>
		public EdgeEndStar()
		{
		}
		#endregion

		#region Properties
		public int Degree
		{
			get
			{

				return _edgeMap.Count;
			}
		}
		#endregion

		#region Methods
		
	
		/// <summary>
		/// Insert a EdgeEnd into this EdgeEndStar.
		/// </summary>
		/// <param name="e">The edge to be inserted.</param>
		abstract public void Insert(EdgeEnd e);

		/// <summary>
		/// Insert an EdgeEnd into the map, and clear the edgeList cache,
		/// since the list of edges has now changed
		/// </summary>
		/// <param name="e"></param>
		/// <param name="obj"></param>
		protected void InsertEdgeEnd(EdgeEnd e, Object obj)  // what's object?
		{
									// uses IComparable interface to maintain sorted order.
			_edgeMap[e]= obj;	// first element is the key, second is values.  automatically entered in sorted.
			_edgeList = null;		// edge list has changed - clear the cache
		}


		/// <summary>
		/// Returns the coordinate for the node this star is based at.
		/// </summary>
		/// <returns>Return the coordinate for the node this star is based at.</returns>
		public Coordinate GetCoordinate()
		{
			Edges();
			if ( _edgeList.Count <= 0 ) return null;
			EdgeEnd e = (EdgeEnd)_edgeList[0];
			return e.Coordinate;
		} // public Coordinate GetCoordinate()
		
		/// <summary>
		/// This method retrieves the sorted list's values as an arraylist for speedier retrieval.
		/// </summary>
		/// <returns>Returns an arraylist of the sorted EdgeEnds.</returns>
		public ArrayList Edges()
		{
			if (_edgeList == null) 
			{
				_edgeList = new ArrayList( _edgeMap.GetValueList() );		// values are sorted based on keys array
			}
			return _edgeList;
		}

		/// <summary>
		/// Returns the next EdgeEnd following the EdgeEnd ee.  EdgeEnds are stored in ClockWise order.
		/// </summary>
		/// <param name="ee">Returns the next EdgeEnd which follows this EdgeEnd.</param>
		/// <returns>Returns the next EdgeEnd following the EdgeEnd ee.</returns>
		public EdgeEnd GetNextCW(EdgeEnd ee)
		{
			Edges();
			int i = _edgeList.IndexOf( ee );  // had to implement EdgeEnd.Equals for this  to work.
			int iNextCW = i - 1;
			if (i == 0)
			{
				iNextCW = _edgeList.Count - 1;
			}
			return (EdgeEnd) _edgeList[iNextCW];
		} // public EdgeEnd GetNextCW(EdgeEnd ee)


		/// <summary>
		/// 
		/// </summary>
		/// <param name="geom"></param>
		public virtual void ComputeLabelling( GeometryGraph[] geom )
		{
			
			ComputeEdgeEndLabels();
			// Propagate side labels  around the edges in the star
			// for each parent Geometry
			Trace.WriteLine( ToString() );
			PropagateSideLabels(0);
			Trace.WriteLine( ToString() );
			PropagateSideLabels(1);
			Trace.WriteLine( ToString() );

			/**
			 * If there are edges that still have null labels for a geometry
			 * this must be because there are no area edges for that geometry incident on this node.
			 * In this case, to label the edge for that geometry we must test whether the
			 * edge is in the interior of the geometry.
			 * To do this it suffices to determine whether the node for the edge is in the interior of an area.
			 * If so, the edge has location INTERIOR for the geometry.
			 * In all other cases (e.g. the node is on a line, on a point, or not on the geometry at all) the edge
			 * has the location EXTERIOR for the geometry.
			 * 
			 * Note that the edge cannot be on the BOUNDARY of the geometry, since then
			 * there would have been a parallel edge from the Geometry at this node also labelled BOUNDARY
			 * and this edge would have been labelled in the previous step.
			 * 
			 * This code causes a problem when dimensional collapses are present, since it may try and
			 * determine the location of a node where a dimensional collapse has occurred.
			 * The point should be considered to be on the EXTERIOR
			 * of the polygon, but locate() will return INTERIOR, since it is passed
			 * the original Geometry, not the collapsed version.
			 *
			 * If there are incident edges which are Line edges labelled BOUNDARY,
			 * then they must be edges resulting from dimensional collapses.
			 * In this case the other edges can be labelled EXTERIOR for this Geometry.
			 *
			 * MD 8/11/01 - NOT TRUE!  The collapsed edges may in fact be in the interior of the Geometry,
			 * which means the other edges should be labelled INTERIOR for this Geometry.
			 * Not sure how solve this...  Possibly labelling needs to be split into several phases:
			 * area label propagation, symLabel merging, then finally null label resolution.
			 */
			
			bool[] hasDimensionalCollapseEdge = new bool[]{ false, false };
			foreach ( EdgeEnd e in Edges() ) 
			{
				Label label = e.Label;

				for (int geomi = 0; geomi < 2; geomi++) 
				{
					if ( label.IsLine( geomi ) && label.GetLocation( geomi ) == Location.Boundary )
					{
						hasDimensionalCollapseEdge[geomi] = true;
					}
				} // for (int geomi = 0; geomi < 2; geomi++)
			} // foreach ( EdgeEnd e in edges )

			//Trace.WriteLine( ToString() );
			foreach ( EdgeEnd e in Edges() ) 
			{
				Label label = e.Label;
				//Trace.WriteLine( label.ToString() );
				for (int geomi = 0; geomi < 2; geomi++) 
				{
					if ( label.IsAnyNull( geomi ) )
					{
						int loc = Location.Null;
						if ( hasDimensionalCollapseEdge[geomi] ) 
						{
							loc = Location.Exterior;
						}
						else 
						{
							Coordinate p = e.Coordinate;
							loc = GetLocation( geomi, p, geom );
						}
						label.SetAllLocationsIfNull( geomi, loc );
					}
				}
				//Tract.WriteLin( e.ToString() );
			}

			//Trace.WriteLine( ToString() );

		} // public void ComputeLabelling( GeometryGraph[] geom )


		private void ComputeEdgeEndLabels()
		{
			// Compute edge label for each EdgeEnd
			foreach( EdgeEnd ee in Edges() ) 
			{
				ee.ComputeLabel();
			}
		}

		int GetLocation(int geomIndex, Coordinate p, GeometryGraph[] geom)
		{
			// compute location only on demand
			if ( _ptInAreaLocation[ geomIndex ] == Location.Null ) 
			{
				_ptInAreaLocation[ geomIndex ] = SimplePointInAreaLocator.Locate( p, geom[geomIndex].Geometry );
			}
			return _ptInAreaLocation[geomIndex];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsAreaLabelsConsistent()
		{
			ComputeEdgeEndLabels();
			return CheckAreaLabelsConsistent( 0 );
		}

		private bool CheckAreaLabelsConsistent(int geomIndex)
		{
			// Since edges are stored in CCW order around the node,
			// As we move around the ring we move from the right to the left side of the edge
			ArrayList edges = Edges();
			// if no edges, trivially consistent
			if ( edges.Count <= 0 )
			{
				return true;
			}

			// initialize startLoc to location of last L side (if any)
			int lastEdgeIndex = edges.Count - 1;
			Label startLabel = ((EdgeEnd)edges[lastEdgeIndex]).Label;
			int startLoc = startLabel.GetLocation( geomIndex, Position.Left );
			if ( startLoc == Location.Null )
			{
				throw new InvalidOperationException("Found an unlabelled area edge.");
			}

			int currLoc = startLoc;
			edges = Edges();
			foreach ( object obj in edges ) 
			{
				EdgeEnd e = (EdgeEnd)obj;

				Label label = e.Label;
				// we assume that we are only checking a area
				if ( !label.IsArea( geomIndex ) )
				{
					throw new InvalidOperationException("Label is a non-area edge.");
				}

				int leftLoc   = label.GetLocation( geomIndex, Position.Left );
				int rightLoc  = label.GetLocation( geomIndex, Position.Right );

				Trace.WriteLine(leftLoc + " " + rightLoc);
				Trace.WriteLine( ToString() );

				// check that edge is really a boundary between inside and outside!
				if (leftLoc == rightLoc) 
				{
					return false;
				}
				// check side location conflict
				
				if (rightLoc != currLoc) 
				{
					//Trace.WriteLine( ToString() );
					return false;
				}
				currLoc = leftLoc;
			}
			return true;

		} // private bool CheckAreaLabelsConsistent(int geomIndex)


		void PropagateSideLabels( int geomIndex )
		{

			// Since edges are stored in CCW order around the node,
			// As we move around the ring we move from the right to the left side of the edge
			int startLoc = Location.Null;

			// initialize loc to location of last L side (if any)
			foreach ( EdgeEnd e in Edges() ) 
			{
				Label label = e.Label;
				if ( label.IsArea( geomIndex ) && label.GetLocation( geomIndex, Position.Left ) != Location.Null )
				{
					startLoc = label.GetLocation( geomIndex, Position.Left );
				}
			}

			// no labelled sides found, so no labels to propagate
			if ( startLoc == Location.Null ) return;

			int currLoc = startLoc;
			foreach ( EdgeEnd e in Edges() ) 
			{
				Label label = e.Label;
				// set null ON values to be in current location
				if ( label.GetLocation( geomIndex, Position.On ) == Location.Null )
				{
					label.SetLocation( geomIndex, Position.On, currLoc);
				}

				// set side labels (if any)
				if ( label.IsArea( geomIndex ) ) 
				{
					int leftLoc   = label.GetLocation( geomIndex, Position.Left );
					int rightLoc  = label.GetLocation( geomIndex, Position.Right );

					// if there is a right location, that is the next location to propagate
					if ( rightLoc != Location.Null ) 
					{
						string locStr = "(at " + e.Coordinate.ToString() + ")";
						//Debug.Print( rightLoc != currLoc, this );
						if ( rightLoc != currLoc )
						{
							throw new TopologyException( String.Format("Side location conflict " , locStr ));
						}
						if ( leftLoc == Location.Null )
						{
							throw new TopologyException( String.Format("Found single null side " , locStr ));
						
						}
						currLoc = leftLoc;
					}
					else 
					{
						/** RHS is null - LHS must be null too.
						 *  This must be an edge from the other geometry, which has no location
						 *  labelling for this geometry.  This edge must lie wholly inside or outside
						 *  the other geometry (which is determined by the current location).
						 *  Assign both sides to be the current location.
						 */
			
						if ( label.GetLocation( geomIndex, Position.Left ) != Location.Null )
						{
							throw new TopologyException( "Found single null side." );
						}
						label.SetLocation( geomIndex, Position.Right, currLoc );
						label.SetLocation( geomIndex, Position.Left, currLoc );
					}
				} // if ( label.IsArea( geomIndex ) ) 

			} // foreach ( object obj in edges )

		} // void PropagateSideLabels( int geomIndex )


		/// <summary>
		/// Searchs the Edges list for the supplied EdgeEnd and returns the index into the list if found.  If not found,
		/// returns -1.
		/// </summary>
		/// <param name="eSearch">The EdgeEnd to find in the Edges list.</param>
		/// <returns>Returns the index for the supplied EdgeEnd or -1 if not found.</returns>
		public int FindIndex(EdgeEnd eSearch)
		{
			Edges();
			for( int i = 0; i < _edgeList.Count; i++ ) 
			{
				EdgeEnd e = (EdgeEnd)_edgeList[i];
				if ( e == eSearch  ) 
				{
					return i;
				}
			} // for( int i = 0; i < Edges().Count; i++ )
			return -1;
		} // public int FindIndex(EdgeEnd eSearch)

		/// <summary>
		/// Returns the string representation for this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( "EdgeEndStar:   " + GetCoordinate() );
			ArrayList edges = Edges();
			foreach( object edgeend in edges )
			{
				EdgeEnd e = edgeend as EdgeEnd;
				if ( e != null )
				{
					sb.Append( e.ToString() );
				}
			} // foreach( object edgeend in edges )
			return sb.ToString();
		} // public override string ToString()
		#endregion

		#region IEnumerable implementation

		/// <summary>
		/// Iterator access to the ordered list of edges is optimized by
		/// copying the map collection to a list.  (This assumes that
		///  once an iterator is requested, it is likely that insertion into
		///  the map is complete).
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return Edges().GetEnumerator();
		}

		#endregion

	} // public abstract class EdgeEndStar : System.Collections.IEnumerable
}
