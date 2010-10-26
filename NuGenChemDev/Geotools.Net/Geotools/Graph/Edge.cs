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
using System.Text;
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph.Index;
#endregion

namespace Geotools.Graph 
{
	/// <summary>
	/// Summary description for Edge.
	/// </summary>
	internal class Edge : GraphComponent
	{
		Coordinates _pts;
		EdgeIntersectionList _eiList = null;
		private string _name;
		private MonotoneChainEdge _monotoneChainEdge;
		private bool _isIsolated = true;
		private Depth _depth = new Depth();
		private int _depthDelta = 0;   // the change in area depth from the R to L side of this edge

		#region Constructors
		/// <summary>
		/// Constructs a new edge using the coordinates collection and supplied label object.
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="label"></param>
		public Edge(Coordinates pts, Label label)
		{
			_eiList = new EdgeIntersectionList( this );
			_pts = pts;
			_label = label;		// inherited member variable.
		}

		/// <summary>
		/// Constructs a new edge using the coordinates collection.
		/// </summary>
		/// <param name="pts"></param>
		public Edge( Coordinates pts ) : this( pts, null )
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// NumPoints returns the number of points in the coordinates collection.  Same as Count.
		/// </summary>
		public int NumPoints
		{
			get
			{
				return _pts.Count;
			}
		}

		/// <summary>
		/// Count returns the number of points in the coordinates collection.  Same as NumPoints.
		/// </summary>
		public int Count
		{
			get
			{
				return _pts.Count;
			}
		}
		
		/// <summary>
		/// Returns the Coordinates collection.
		/// </summary>
		public Coordinates Coordinates
		{
			get
			{
				return _pts;  
			}
		}

		/// <summary>
		/// Returns the Depth object.
		/// </summary>
		public Depth Depth
		{
			get
			{
				return _depth;
			}
		}

		/// <summary>
		/// Returns the changes in depth.
		/// </summary>
		public int DepthDelta
		{
			get
			{
				return _depthDelta;  
			}
			set
			{
				_depthDelta = value;
			}
		}

		/// <summary>
		/// Gets/Sets the name for this edge.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		/// <summary>
		/// Returns the Maximum segment index.
		/// </summary>
		public int MaximumSegmentIndex
		{
			get
			{
				return _pts.Count - 1;
			}
		}

		/// <summary>
		/// Returns the EdgeIntersectionList object.
		/// </summary>
		public EdgeIntersectionList EdgeIntersectionList
		{
			get
			{
				return _eiList;
			}
		}

		/// <summary>
		/// Returns true if Coordinates represents a closed geometry.
		/// </summary>
		public bool IsClosed
		{
			get
			{
				return _pts[0].Equals( _pts[ _pts.Count - 1]);		// true if first point is equal to last point in collection.
			}
		}
	

	
		#endregion

		#region Static Methods
		/// <summary>
		/// Updates an IM from the label for an edge. Handles edges from both L and A geometrys.
		/// </summary>
		/// <param name="label"></param>
		/// <param name="im"></param>
		public static void UpdateIM(Label label, IntersectionMatrix im)
		{

			im.SetAtLeastIfValid( label.GetLocation( 0, Position.On ), label.GetLocation( 1, Position.On ), 1);
			if ( label.IsArea() ) 
			{
				im.SetAtLeastIfValid( label.GetLocation( 0, Position.Left ),  label.GetLocation( 1, Position.Left ),   2);
				im.SetAtLeastIfValid( label.GetLocation( 0, Position.Right ), label.GetLocation( 1, Position.Right ),  2);
			}
		} // public static void UpdateIM(Label label, IntersectionMatrix im)
		#endregion

		#region Methods
		/// <summary>
		///  An Edge is collapsed if it is an Area edge and it consists of two segments which are equal and opposite (eg a zero-width V).
		/// </summary>
		/// <returns></returns>
		public bool IsCollapsed()
		{
			if ( !_label.IsArea() )
			{
				return false;
			}
			if ( _pts.Count != 3 )
			{
				return false;
			}
			if ( _pts[0].Equals( _pts[2] ) )
			{
				return true;
			}
			return false;
		}	

		/// <summary>
		/// Returns the MonotoneChainEdge object.
		/// </summary>
		public MonotoneChainEdge GetMonotoneChainEdge()
		{
				if (_monotoneChainEdge == null)
				{
					_monotoneChainEdge = new MonotoneChainEdge( this );
				}
				return _monotoneChainEdge;
		}

		/// <summary>
		/// Returns the collapsed edge.
		/// </summary>
		/// <returns>Returns the collapsed edge.</returns>
		public Edge GetCollapsedEdge()
		{
			Coordinates newPts = new Coordinates();
			newPts.Add( _pts[0] );
			newPts.Add( _pts[1] );
			Edge newEdge = new Edge( newPts, Label.ToLineLabel( _label ) );
			return newEdge;
		} // public Edge GetCollapsedEdge()

		/// <summary>
		/// Returns the Coordinate at index.
		/// </summary>
		/// <param name="index">The index into the Coordinates collection.</param>
		/// <returns>Returns a coordinate at index.</returns>
		public Coordinate GetCoordinate(int index)
		{
			return _pts[index];
		} // public Coordinate GetCoordinate(int index)

		/// <summary>
		/// Returns a coordinate in this component (or null, if there are none).
		/// </summary>
		/// <returns>Returns a coordinate in this edge.</returns>
		public override Coordinate GetCoordinate()
		{
			if ( _pts.Count > 0 )
			{
				return _pts[0];
			}
			return null;
		}	// public override Coordinate GetCoordinate()
	
		
		
		/// <summary>
		/// Adds EdgeIntersections for one or both intersections found for a segment of an edge to the edge intersection list.
		/// </summary>
		/// <param name="li"></param>
		/// <param name="segmentIndex"></param>
		/// <param name="geomIndex"></param>
		public void AddIntersections( LineIntersector li, int segmentIndex, int geomIndex )
		{
			for (int i = 0; i < li.GetIntersectionNum(); i++) 
			{
				AddIntersection( li, segmentIndex, geomIndex, i );
			}
		} // public void AddIntersections(LineIntersector li, int segmentIndex, int geomIndex)

		/// <summary>
		/// Add an EdgeIntersection for intersection intIndex.
		/// </summary>
		/// <remarks>
		/// An intersection that falls exactly on a vertex of the edge is normalized
		/// to use the higher of the two possible segmentIndexes
		/// </remarks>
		/// <param name="li"></param>
		/// <param name="segmentIndex"></param>
		/// <param name="geomIndex"></param>
		/// <param name="intIndex"></param>
		public void AddIntersection(LineIntersector li, int segmentIndex, int geomIndex, int intIndex)
		{
			Coordinate intPt = new Coordinate( li.GetIntersection( intIndex ) );
			int normalizedSegmentIndex = segmentIndex;
			double dist = li.GetEdgeDistance( geomIndex, intIndex );
			//Trace.WriteLine("edge intpt: " + intPt + " dist: " + dist);
			// normalize the intersection point location
			int nextSegIndex = normalizedSegmentIndex + 1;
			if ( nextSegIndex < _pts.Count ) 
			{
				Coordinate nextPt = _pts[nextSegIndex];
				//Trace.WriteLine("next pt: " + nextPt);
				if ( intPt.Equals( nextPt ) ) 
				{
					//Trace.WriteLine("normalized distance");
					normalizedSegmentIndex = nextSegIndex;
					dist = 0.0;
				}
			}
			//Add the intersection point to edge intersection list.
			EdgeIntersection ei = _eiList.Add( intPt, normalizedSegmentIndex, dist );
			//Trace.WriteLine( ei.ToString() );
		} // public void AddIntersection(LineIntersector li, int segmentIndex, int geomIndex, int intIndex)

		/// <summary>
		/// Update the IM with the contribution for this component. A component only contributes if it has a labelling for both parent geometries.
		/// </summary>
		/// <param name="im"></param>
		protected override void ComputeIM(IntersectionMatrix im)
		{
			UpdateIM( _label, im );
		} // protected override void ComputeIM(IntersectionMatrix im)

		/// <summary>
		/// Returns true if and only if the coordinates of this object are the same or the reverse of the
		/// coordinates of obj.
		/// </summary>
		/// <param name="obj">The object to compare to this object.  Must be of type Edge.</param>
		/// <returns>Returns true if and only if the coordinates of this object are the same or the reverse of the
		/// coordinates of obj.</returns>
		public override bool Equals(Object obj)
		{
			Edge e = obj as Edge;	
			if ( e == null )
			{
				return false;
			}
			if ( _pts.Count != e.Coordinates.Count ) 
			{
				return false;
			}

			bool isEqualForward = true;
			bool isEqualReverse = true;
			int iRev = _pts.Count;
			for ( int i = 0; i < _pts.Count; i++) 
			{
				if ( !_pts[i].Equals2D( e.Coordinates[i] ) ) 
				{
					isEqualForward = false;
				}
				if ( !_pts[i].Equals2D( e.Coordinates[--iRev] ) ) 
				{
					isEqualReverse = false;
				}
				if ( !isEqualForward && !isEqualReverse )
				{
					return false;
				}
			}
			return true;
		} // public override bool Equals(Object obj)

		public override int GetHashCode()
		{
			return base.GetHashCode();
		} // public override int GetHashCode()

		/// <summary>
		/// Returns true if the coordinate sequences of the Edges are identical.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public bool IsPointwiseEqual(Edge e)
		{
			if ( _pts.Count != e.Coordinates.Count ) return false;

			for ( int i = 0; i < _pts.Count; i++ ) 
			{
				if ( !_pts[i].Equals2D( e.Coordinates[i] ) ) 
				{
					return false;
				}
			}
			return true;
		} // public bool IsPointwiseEqual(Edge e)

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( "edge " + _name + ": " );
			sb.Append( "LINESTRING (" );
			for (int i = 0; i < _pts.Count; i++) 
			{
				if (i > 0) 
				{
					sb.Append(",");
				}
				sb.Append(_pts[i].X + " " + _pts[i].Y );
			}
			sb.Append( ")  " + _label + " " + _depthDelta );
			return sb.ToString();
		} // public override string ToString()

		public string ToStringReverse()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("edge " + _name + ": ");
			for (int i = _pts.Count - 1; i >= 0; i--) 
			{
				sb.Append( _pts[i] + " ");
			}
			sb.Append("");
			return sb.ToString();
		} // public string ToStringReverse()


		/// <summary>
		/// 
		/// </summary>
		public override bool IsIsolated()
		{
			return _isIsolated;
		}

		/// <summary>
		/// Sets the isIsolated member variable.
		/// </summary>
		/// <param name="isIsolated"></param>
		public void SetIsIsolated( bool isIsolated )
		{
			_isIsolated = isIsolated;
		}

		#endregion

	}
}
