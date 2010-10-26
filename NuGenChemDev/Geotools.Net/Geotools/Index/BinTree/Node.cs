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
#endregion

namespace Geotools.Index.BinTree
{
	/// <summary>
	/// Summary description for Node.
	/// </summary>
	internal class Node : NodeBase
	{
		private Interval _interval;
		private double _centre;
		private int _level;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Node class.
		/// </summary>
		public Node( Interval interval, int level )
		{
			_interval = interval;
			_level = level;
			_centre = ( interval.Min + interval.Max ) / 2;
		}
		#endregion

		#region Properties
		public Interval Interval
		{
			get
			{
				return _interval;
			}
		}
		public int Level
		{
			get
			{
				return _level;
			}
		}
		public double Centre
		{
			get
			{
				return _centre;
			}
		}		
		#endregion

		#region Methods
		protected override bool IsSearchMatch( Interval itemInterval )
		{
			return itemInterval.Overlaps( _interval );
		}

		/// <summary>
		/// Returns the subnode containing the envelope.  Creates the node if it does not already exist.
		/// </summary>
		/// <param name="searchInterval"></param>
		/// <returns></returns>
		public Node GetNode( Interval searchInterval )
		{
			int subnodeIndex = GetSubnodeIndex( searchInterval, _centre );

			// if index is -1 searchEnv is not contained in a subnode
			if ( subnodeIndex != -1 ) 
			{
				// create the node if it does not exist
				Node node = GetSubnode( subnodeIndex );
				// recursively search the found/created node
				return node.GetNode( searchInterval );
			}
			else 
			{
				return this;
			}
		}

		/// <summary>
		/// Returns the smallest existing node containing the envelope.
		/// </summary>
		/// <param name="searchInterval"></param>
		/// <returns></returns>
		public NodeBase Find( Interval searchInterval )
		{
			int subnodeIndex = GetSubnodeIndex( searchInterval, _centre );
			if ( subnodeIndex == -1 )
			{
				return this;
			}

			if ( _subnode[subnodeIndex] != null ) 
			{
				// query lies in subnode, so search it
				Node node = _subnode[subnodeIndex];
				return node.Find( searchInterval );
			}
			// no existing subnode, so return this one anyway
			return this;
		}

		void Insert( Node node )
		{
			Debug.Assert( _interval == null || _interval.Contains( node.Interval ) );
			int index = GetSubnodeIndex( node.Interval, _centre );
			if ( node.Level == _level - 1 ) 
			{
				_subnode[index] = node;
			}
			else 
			{
				// the node is not a direct child, so make a new child node to contain it
				// and recursively insert the node
				Node childNode = CreateSubnode( index );
				childNode.Insert( node );
				_subnode[index] = childNode;
			}
		}

		/// <summary>
		/// Get the subnode for the index.  If it doesn't exist, create it.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private Node GetSubnode( int index )
		{
			if ( _subnode[index] == null ) 
			{
				_subnode[index] = CreateSubnode( index );
			}
			return _subnode[index];
		}

		private Node CreateSubnode( int index )
		{
			// create a new subnode in the appropriate interval

			double min = 0.0;
			double max = 0.0;

			switch (index) 
			{
				case 0:
					min = _interval.Min;
					max = _centre;
					break;
				case 1:
					min = _centre;
					max = _interval.Max;
					break;
			}

			Interval subInt = new Interval( min, max );
			Node node = new Node (subInt, _level - 1);
			return node;
		}

		#endregion

		#region Static Methods
		public static Node CreateNode( Interval itemInterval )
		{
			Key key = new Key( itemInterval );

			//System.out.println("input: " + env + "  binaryEnv: " + key.getEnvelope());
			Node node = new Node( key.Interval, key.Level );
			return node;
		}

		public static Node CreateExpanded( Node node, Interval addInterval )
		{
			Interval expandInt = new Interval( addInterval );
			if ( node != null ) expandInt.ExpandToInclude( node.Interval );

			Node largerNode = CreateNode( expandInt );
			if ( node != null ) largerNode.Insert( node );
			return largerNode;
		}
		#endregion



	}
}
