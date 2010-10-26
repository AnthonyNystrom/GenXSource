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
#endregion

namespace Geotools.Graph.Index
{
	/// <summary>
	/// An EdgeSetIntersector computes all the intersections between the
	/// edges in the set.  It adds the computed intersections to each edge
	/// they are found on.  It may be used in two scenarios:
	/// <list type="bullet">
	/// <item><term>determining the internal intersections between a single set of edges.</term><description>Your Description</description></item>
	/// <item><term>determining the mutual intersections between two different sets of edges</term><description>Your Description</description></item>
	/// </list>
	/// It uses a {@link SegmentIntersector} to compute the intersections between
	/// segments and to record statistics about what kinds of intersections were found.
	/// </summary>
	internal abstract class EdgeSetIntersector
	{
		//ArrayList _edges0 = null;  inherited class SimpleMCSweepLineIntersector does not use these either..
		//ArrayList _edges1 = null;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeSetIntersector class.
		/// </summary>
		public EdgeSetIntersector()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Computes all self-intersections between edges in a set of edges.
		/// </summary>
		/// <param name="edges"></param>
		/// <param name="si"></param>
		abstract public void ComputeIntersections(ArrayList edges, SegmentIntersector si);

		/// <summary>
		/// Computes all mutual intersections between two sets of edges.
		/// </summary>
		/// <param name="edges0"></param>
		/// <param name="edges1"></param>
		/// <param name="si"></param>
		abstract public void ComputeIntersections(ArrayList edges0, ArrayList edges1, SegmentIntersector si);

		#endregion

	}
}
