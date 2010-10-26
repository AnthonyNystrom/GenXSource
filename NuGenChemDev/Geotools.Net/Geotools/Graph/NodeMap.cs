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
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// Summary description for NodeMap.
	/// </summary>
	internal class NodeMap  : System.Collections.IEnumerable
	{
		SortedList _nodeMap = new SortedList();
		NodeFactory _nodeFact;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the NodeMap class.
		/// </summary>
		public NodeMap(NodeFactory nodeFact) 
		{
			_nodeFact = nodeFact;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the number of nodes in the sorted list (nodemap).
		/// </summary>
		public int Count
		{
			get
			{
				return _nodeMap.Count;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public SortedList NodeList
		{
			get
			{
				return _nodeMap;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		public Node this[int index]
		{
			get
			{
				if ( index >= 0 && index < Count )
				{
					return (Node)_nodeMap[index];
				}
				else
				{
					throw new IndexOutOfRangeException( "NodeMap index out of range: " + index.ToString() );
				}
			}
		} // public Node this[int index]

		/// <summary>
		/// Factory function - subclasses can override to create their own types of nodes.
		/// </summary>
		/// <param name="coord"></param>
		/// <returns></returns>
		protected Node CreateNode(Coordinate coord)
		{
			return new Node(coord,null);
		}


		/// <summary>
		/// This method expects that a node has a coordinate value.
		/// </summary>
		/// <param name="coord"></param>
		/// <returns></returns>
		public Node AddNode(Coordinate coord)
		{
			Node node = null;
			if ( !_nodeMap.ContainsKey( coord ) ) 
			{
				node = _nodeFact.CreateNode(coord);
				_nodeMap.Add( coord, node );
			}
			else
			{
				node = (Node)_nodeMap[coord];
			}
			return node;
		} // public Node AddNode(Coordinate coord)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public Node AddNode(Node n)
		{
			if ( !_nodeMap.ContainsKey( n.Coordinate ) ) 
			{
				_nodeMap.Add( n.Coordinate, n );
				return n;
			}
			else
			{
				Node node = (Node)_nodeMap[n.Coordinate];
				node.MergeLabel(n);
				return node;
			}
		} // public Node AddNode(Node n)

		/// <summary>
		/// Adds a node for the start point of this EdgeEnd (if one does not already exist in this map).
		/// Adds the EdgeEnd to the (possibly new) node.
		/// </summary>
		/// <param name="e"></param>
		public void Add(EdgeEnd e)
		{
			Coordinate p = e.Coordinate;
			Node n = AddNode( p );
			n.Add( e );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coord"></param>
		/// <returns>return the node if found; null otherwise</returns>
		public Node Find(Coordinate coord)
		{    
			return (Node) _nodeMap[ coord ];
		} // public Node Find(Coordinate coord)

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return _nodeMap.GetEnumerator();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <returns></returns>
		public ArrayList GetBoundaryNodes(int geomIndex)
		{
			ArrayList bdyNodes = new ArrayList();

			foreach( DictionaryEntry entry in _nodeMap )
			{
				Node node = (Node) entry.Value;
				if ( node.Label.GetLocation(geomIndex) == Location.Boundary )
				{
					bdyNodes.Add( node );
				}
			} // foreach( object objNode in _nodeMap )

			return bdyNodes;
		} // public ArrayList GetBoundaryNodes(int geomIndex)	

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach( DictionaryEntry entry in _nodeMap )
			{
				Node node = (Node) entry.Value;
				sb.Append( node.ToString() );
			}
			return sb.ToString();
		} // public override string ToString()

		#endregion

	} // public class NodeMap  : System.Collections.IEnumerable
}
