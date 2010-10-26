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

namespace Geotools.Index.Quadtree
{
	internal class Node: NodeBase
	{

		private Envelope _env;
		private Coordinate _centre;
		private int _level;

		#region Constructors
		public Node( Envelope env, int level )
		{
			//this.parent = parent;
			this._env = env;
			this._level = level;
			_centre = new Coordinate();
			_centre.X = ( env.MinX + env.MaxX ) / 2;
			_centre.Y = ( env.MinY + env.MaxY ) / 2;
		}
		#endregion

		#region Properties
        public Envelope Envelope
		{
			get
			{
				return _env;
			}
		}
		public int Level
		{
			get
			{
				return _level;
			}
		}
		#endregion

		#region Methods

		protected override bool IsSearchMatch(Envelope searchEnv)
		{
			return _env.Intersects( searchEnv );
		}

		/// <summary>
		/// Returns the subquad containing the envelope.  Creates the subquad if it does not already exist.
		/// </summary>
		/// <param name="searchEnv"></param>
		/// <returns></returns>
		public Node GetNode( Envelope searchEnv )
		{
			int subnodeIndex = GetSubnodeIndex( searchEnv, _centre);
			// if subquadIndex is -1 searchEnv is not contained in a subquad
			if (subnodeIndex != -1) 
			{
				// create the quad if it does not exist
				Node node = GetSubnode( subnodeIndex );
				// recursively search the found/created quad
				return node.GetNode( searchEnv );
			}
			else 
			{
				return this;
			}
		}

		/// <summary>
		/// Returns the smallest existing node containing the envelope.
		/// </summary>
		/// <param name="searchEnv"></param>
		/// <returns></returns>
		public NodeBase Find(Envelope searchEnv)
		{
			int subnodeIndex = GetSubnodeIndex( searchEnv, _centre );
			if ( subnodeIndex == -1 )
			{
				return this;
			}
			if ( _subnode[subnodeIndex] != null ) 
			{
				// query lies in subquad, so search it
				Node node = _subnode[subnodeIndex];
				return node.Find( searchEnv );
			}
			// no existing subquad, so return this one anyway
			return this;
		}

		void InsertNode( Node node )
		{
			//Assert.isTrue(env == null || env.contains(node.env));
			if ( !( _env == null || _env.Contains( node.Envelope ) ) )
			{
				throw new InvalidOperationException();
			}
			//System.out.println(env);
			//System.out.println(quad.env);
			int index = GetSubnodeIndex( node.Envelope, _centre );
			//System.out.println(index);
			if ( node.Level== _level - 1) 
			{
				_subnode[index] = node;
				//System.out.println("inserted");
			}
			else 
			{
				// the quad is not a direct child, so make a new child quad to contain it
				// and recursively insert the quad
				Node childNode = CreateSubnode( index );
				childNode.InsertNode(node);
				_subnode[index] = childNode;
			}
		}

		/**
		 * get the subquad for the index.
		 * If it doesn't exist, create it
		 */
		private Node GetSubnode( int index )
		{
			if ( _subnode[index] == null ) 
			{
				_subnode[index] = CreateSubnode(index);
			}
			return _subnode[index];
		}

		private Node CreateSubnode( int index )
		{
			// create a new subquad in the appropriate quadrant

			double minx = 0.0;
			double maxx = 0.0;
			double miny = 0.0;
			double maxy = 0.0;

			switch (index) 
			{
				case 0:
					minx = _env.MinX;
					maxx = _centre.X;
					miny = _env.MinY;
					maxy = _centre.Y;
					break;
				case 1:
					minx = _centre.X;
					maxx = _env.MaxX;
					miny = _env.MinY;
					maxy = _centre.Y;
					break;
				case 2:
					minx = _env.MinX;
					maxx = _centre.X;
					miny = _centre.Y;
					maxy = _env.MaxY;
					break;
				case 3:
					minx = _centre.X;
					maxx = _env.MaxX;
					miny = _centre.Y;
					maxy = _env.MaxY;
					break;
			}
			Envelope sqEnv = new Envelope( minx, maxx, miny, maxy );
			Node node = new Node( sqEnv, _level - 1 );
			return node;
		}
		#endregion

		#region Static Methods
		public static Node CreateNode( Envelope env )
		{
			Key key = new Key( env );
			Node node = new Node( key.Envelope, key.Level );
			return node;
		}

		public static Node CreateExpanded( Node node, Envelope addEnv )
		{
			Envelope expandEnv = new Envelope(addEnv);
			if (node != null) expandEnv.ExpandToInclude(node.Envelope);

			Node largerNode = CreateNode(expandEnv);
			if (node != null) largerNode.InsertNode(node);
			return largerNode;
		}
		#endregion
		 
	}
}