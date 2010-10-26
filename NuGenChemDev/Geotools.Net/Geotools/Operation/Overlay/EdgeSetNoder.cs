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
using System.Diagnostics;
using Geotools.Algorithms;
using Geotools.Graph;
using Geotools.Graph.Index;
#endregion

namespace Geotools.Operation.Overlay
{
	/// <summary>
	/// Summary description for EdgeSetNoder.
	/// </summary>
	internal class EdgeSetNoder
	{
		private LineIntersector _li;
		private ArrayList _inputEdges = new ArrayList();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeSetNoder class.
		/// </summary>
		public EdgeSetNoder( LineIntersector li )
		{
			_li = li;
		} // public EdgeSetNoder( LineIntersector li )
		#endregion

		#region Properties
		#endregion

		#region Methods
		/// <summary>
		/// Addes a ranges of edges.
		/// </summary>
		/// <param name="edges">Array of edges to add.</param>
		public void AddEdges(ArrayList edges)
		{
			_inputEdges.AddRange( edges );
		} // public void AddEdges(ArrayList edges)

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ArrayList GetNodedEdges()
		{
			EdgeSetIntersector esi = new SimpleMCSweepLineIntersector();
			SegmentIntersector si = new SegmentIntersector( _li, true, false );
			esi.ComputeIntersections( _inputEdges, si );
			//Trace.WriteLine("has proper int = " + si.hasProperIntersection());

			ArrayList splitEdges = new ArrayList();
			foreach (object obj in _inputEdges ) 
			{
				Edge e = (Edge) obj;
				e.EdgeIntersectionList.AddSplitEdges( splitEdges );
			}
			return splitEdges;
		} // public ArrayList GetNodedEdges()
		#endregion

	} // public class EdgeSetNoder
}
