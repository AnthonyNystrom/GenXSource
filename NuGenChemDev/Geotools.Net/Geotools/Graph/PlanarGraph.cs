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
using System.IO;
using System.Text;
using Geotools.Geometries;
using Geotools.Algorithms;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// The computation of the IntersectionMatrix relies on the use of a structure
	/// called a "topology graph".
	/// </summary>
	/// <remarks>
	/// <para>The topology graph contains nodes and edges corresponding to the nodes and line segments of a Geometry. Each
	/// node and edge in the graph is labeled with its topological location relative to
	/// the source geometry.</para>
	/// 
	/// <para>Note that there is no requirement that points of self-intersection be a vertex.
	/// Thus to obtain a correct topology graph, Geometrys must be
	/// self-noded before constructing their graphs.</para>
	/// 
	/// <para>Two fundamental operations are supported by topology graphs:
	/// 
	/// 1) Computing the intersections between all the edges and nodes of a single graph
	/// 2) Computing the intersections between the edges and nodes of two different graphs</para>
	/// </remarks>
	internal class PlanarGraph : System.Collections.IEnumerable 
	{
		public static CGAlgorithms _cga = new RobustCGAlgorithms();
		public static LineIntersector _li = new RobustLineIntersector();
		protected ArrayList _edges        = new ArrayList();
		protected NodeMap _nodes;
		protected ArrayList _edgeEndList  = new ArrayList();

		#region Properties

		/// <summary>
		/// Returns the NodeMap of Nodes.
		/// </summary>
		public NodeMap Nodes
		{
			get
			{
				return _nodes;
			}
		}

		/// <summary>
		/// Returns the list of EdgeEnds.
		/// </summary>
		public ArrayList EdgeEnds 
		{
			get
			{
				return _edgeEndList;
			}
		}

		/// <summary>
		/// Returns the list of Edges.
		/// </summary>
		public ArrayList Edges
		{
			get
			{
				return _edges;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the PlanarGraph class.
		/// </summary>
		public PlanarGraph( NodeFactory nodeFact ) 
		{
			_nodes = new NodeMap( nodeFact );
		}

		/// <summary>
		/// Constructs a new instance of PlanarGraph.
		/// </summary>
		public PlanarGraph() 
		{
			_nodes = new NodeMap( new NodeFactory() );
		}
		#endregion

		#region Static methods

		/// <summary>
		/// For nodes in the Collection, link the DirectedEdges at the node that are in the result.
		/// </summary>
		/// <remarks>
		/// This allows clients to link only a subset of nodes in the graph, for
		///  efficiency (because they know that only a subset is of interest).
		/// </remarks>
		/// <param name="nodes"></param>
		public static void LinkResultDirectedEdges(ArrayList nodes)
		{
			foreach ( object obj in nodes ) 
			{
				Node node = (Node) obj;
				((DirectedEdgeStar) node.Edges).LinkResultDirectedEdges();
			}
		} // public static void LinkResultDirectedEdges(ArrayList nodes)
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
			return _edges.GetEnumerator();
		}

		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="coord"></param>
		/// <returns></returns>
		public bool IsBoundaryNode( int geomIndex, Coordinate coord )
		{
			Node node = _nodes.Find( coord );
			if ( node == null )
			{
				return false;
			}

			Label label = node.Label;
			if ( label != null && label.GetLocation( geomIndex ) == Location.Boundary )
			{
				return true;
			}
			return false;
		} // public bool IsBoundaryNode( int geomIndex, Coordinate coord )

		/// <summary>
		/// Inserts an Edge into the Edges list.
		/// </summary>
		/// <param name="e"></param>
		protected void InsertEdge( Edge e )
		{
			_edges.Add( e );
		}

		/// <summary>
		/// Adds and EdgeEnd into the EdgeEndList.
		/// </summary>
		/// <param name="e"></param>
		public void Add( EdgeEnd e )
		{
			_nodes.Add( e );
			_edgeEndList.Add( e );
		}

		/// <summary>
		/// Adds a node to the node list.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public Node AddNode( Node node )
		{
			return _nodes.AddNode( node );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coord"></param>
		/// <returns></returns>
		public Node AddNode( Coordinate coord )
		{
			return _nodes.AddNode( coord );
		}
 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coord"></param>
		/// <returns>return the node if found; null otherwise.</returns>
		public Node Find( Coordinate coord )
		{
			return _nodes.Find( coord );
		}

		/// <summary>
		/// Add a set of edges to the graph.  For each edge two DirectedEdges
		/// will be created.  DirectedEdges are NOT linked by this method.
		/// </summary>
		/// <param name="edgesToAdd"></param>
		public void AddEdges( ArrayList edgesToAdd )
		{
			// create all the nodes for the edges
			foreach( object objectEdge in edgesToAdd )
			{
				Edge e = (Edge) objectEdge;
				_edges.Add( e );

				DirectedEdge de1 = new DirectedEdge( e, true );
				DirectedEdge de2 = new DirectedEdge( e, false );

				de1.Sym = de2;
				de2.Sym = de1;

				Add( de1 );
				Add( de2 );
			} // foreach( object objectEdge in edgesToAdd )
		} // public void AddEdges( ArrayList edgesToAdd )

		/// <summary>
		///  Link the DirectedEdges at the nodes of the graph.
		///  This allows clients to link only a subset of nodes in the graph, for
		///  efficiency (because they know that only a subset is of interest).
		/// </summary>
		public void LinkResultDirectedEdges()
		{
			foreach( DictionaryEntry objectNode in _nodes )
			{
				Node node = (Node) objectNode.Value;
				( (DirectedEdgeStar) node.Edges).LinkResultDirectedEdges();
			} // foreach( object objectNode in _nodes )
		} // public void LinkResultDirectedEdges()


		/// <summary>
		/// Link the DirectedEdges at the nodes of the graph.
		/// This allows clients to link only a subset of nodes in the graph, for
		/// efficiency (because they know that only a subset is of interest).
		/// </summary>
		public void LinkAllDirectedEdges()
		{
			foreach( DictionaryEntry objectNode in _nodes )
			{
				Node node = (Node) objectNode.Value;
				((DirectedEdgeStar) node.Edges).LinkAllDirectedEdges();
			} // foreach( object objectNode in _nodes )
		} // public void LinkAllDirectedEdges()


		/// <summary>
		/// Returns the EdgeEnd which has edge e as its base edge
		/// (MD 18 Feb 2002 - this should return a pair of edges)
		/// </summary>
		/// <param name="e"></param>
		/// <returns>return the edge, if found. Null if the edge was not found</returns>
		public EdgeEnd FindEdgeEnd( Edge e )
		{
			foreach( object objectEdgeEnd in EdgeEnds )
			{
				EdgeEnd ee = (EdgeEnd) objectEdgeEnd;
				if ( ee.Edge == e )
				{
					return ee;
				}
			}
			return null;
		} // public EdgeEnd FindEdgeEnd( Edge e )
	
		/// <summary>
		/// Returns the edge whose first two coordinates are p0 and p1.
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <returns>Return the edge, if found. null if the edge was not found.</returns>
		public Edge FindEdge( Coordinate p0, Coordinate p1 )
		{
			foreach( Edge e in _edges )
			{
				Coordinates eCoord = e.Coordinates;
				if ( p0.Equals( eCoord[0] ) && p1.Equals( eCoord[1] ) )
				{
					return e;
				}
			}
			return null;
		} // public Edge FindEdge( Coordinate p0, Coordinate p1 )


		/// <summary>
		/// Returns the edge which starts at p0 and whose first segment is
		/// parallel to p1
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <returns>Return the edge, if found. Return null if the edge was not found.</returns>
		public Edge FindEdgeInSameDirection( Coordinate p0, Coordinate p1 )
		{
			foreach(Edge e in _edges)
			{
				Coordinates eCoord = e.Coordinates;
				if ( MatchInSameDirection( p0, p1, eCoord[0], eCoord[1]) )
				{
					return e;
				}

				if ( MatchInSameDirection( p0, p1, eCoord[eCoord.Count - 1], eCoord[eCoord.Count - 2]) )
				{
					return e;
				}
			}
			return null;
		} // public Edge FindEdgeInSameDirection( Coordinate p0, Coordinate p1 )


		/// <summary>
		/// The coordinate pairs match if they define line segments lying in the same direction.
		/// </summary>
		/// <remarks>
		/// E.g. the segments are parallel and in the same quadrant (as opposed to parallel and opposite!).
		/// </remarks>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <param name="ep0"></param>
		/// <param name="ep1"></param>
		/// <returns></returns>
		private bool MatchInSameDirection( Coordinate p0, Coordinate p1, Coordinate ep0, Coordinate ep1 )
		{
			if ( !p0.Equals( ep0 ) )
			{
				return false;
			}

			if ( _cga.ComputeOrientation( p0, p1, ep1 ) == CGAlgorithms.COLLINEAR
			     && Quadrant.QuadrantLocation( p0, p1 ) == Quadrant.QuadrantLocation( ep0, ep1 ) )
			{

				return true;
			}
			return false;
		} // private bool MatchInSameDirection( Coordinate p0, Coordinate p1, Coordinate ep0, Coordinate ep1 )

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach(Edge e in _edges )
			{
				sb.Append("edge " + i + ":");
				sb.Append( e.ToString() );
				sb.Append( e.EdgeIntersectionList.ToString() );
				i++;
			}
			return sb.ToString();
		} // public override string ToString()

		#endregion

	}
}
