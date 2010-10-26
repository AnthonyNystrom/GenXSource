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
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// Summary description for EdgeList.
	/// </summary>
	internal class EdgeList : System.Collections.ArrayList
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeList class.
		/// </summary>
		public EdgeList() : base()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Insert an edge unless it is already in the list.
		/// </summary>
		/// <param name="e"></param>
		public void Insert(Edge e)
		{
			Add(e);
		}

		/// <summary>
		/// If the edge e is already in the list, return its index.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>return  index, if e is already in the list, -1 otherwise</returns>
		public int FindEdgeIndex(Edge e)
		{
			for (int i = 0; i < Count; i++) 
			{
				if ( ((Edge)this[i]).Equals( e ) ) 
				{
					return i;
				}
			}
			return -1;
		}// public int FindEdgeIndex(Edge e)


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("MULTILINESTRING ( ");
			//for (int j = 0; j < size(); j++) 
			int j=0;
			foreach(object objEdge in this )
			{
				Edge e = (Edge) objEdge;
				if (j > 0)
				{
					sb.Append(",");
				}
				sb.Append("(");
				Coordinates pts = e.Coordinates;
				int i=0;
				//for (int i = 0; i < pts.length; i++) 
				foreach(Coordinate coord in pts)
				{
					if (i > 0)
					{
						sb.Append(",");
					}
					sb.Append( coord.ToString() );
					i++;
				}
				sb.Append(")");
				j++;
			}
			sb.Append(")  ");
			return sb.ToString();
		}
		#endregion

	}
}
