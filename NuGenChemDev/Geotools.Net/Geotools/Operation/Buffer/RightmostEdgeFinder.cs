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
using Geotools.Graph;
using Geotools.Geometries;
using Geotools.Algorithms;
#endregion

namespace Geotools.Operation.Buffer
{
	/// <summary>
	/// A RightmostEdgeFinder find the DirectedEdge in a list which has the highest coordinate,
	/// and which is oriented L to R at that point. (I.e. the right side is on the RHS of the edge.)
	/// </summary>
	internal class RightmostEdgeFinder
	{

		private static CGAlgorithms _cga;

		//private Coordinate extremeCoord;
		private int _minIndex = -1;
		private Coordinate _minCoord = null;
		private DirectedEdge _minDe = null;
		private DirectedEdge _orientedDe = null;

		#region Constructors

		/// <summary>
		/// A RightmostEdgeFinder finds the DirectedEdge with the rightmost coordinate.
		/// The DirectedEdge returned is guranteed to have the R of the world on its RHS.
		/// </summary>
		/// <param name="cga"></param>
		public RightmostEdgeFinder(CGAlgorithms cga)
		{
			_cga = cga;
		}

		#endregion

		#region Properties
		#endregion

		#region Methods

		public DirectedEdge GetEdge()  
		{    
			return _orientedDe;

		}

		public Coordinate GetCoordinate()  
		{    
			return _minCoord;  
		}

		public void FindEdge(ArrayList dirEdgeList)
		{
			 // Check all forward DirectedEdges only.  This is still general,
			 // because each edge has a forward DirectedEdge.

			
			//for (Iterator i = dirEdgeList.iterator(); i.hasNext();) 
			foreach(object obj in dirEdgeList)
			{
				DirectedEdge de = (DirectedEdge)obj;
				if (! de.IsForward)
					continue;
				CheckForRightmostCoordinate(de);
			}

			 // If the rightmost point is a node, we need to identify which of
			 // the incident edges is rightmost.
			//Assert.isTrue(minCoord.equals(minDe.getCoordinate()) || minIndex != 0, "inconsistency in rightmost processing");
			if (_minIndex == 0 ) 
			{
				FindRightmostEdgeAtNode();
			}
			else 
			{
				FindRightmostEdgeAtVertex();
			}

			 // now check that the extreme side is the R side.
			 // If not, use the sym instead.
			_orientedDe = _minDe;
			int rightmostSide = GetRightmostSide(_minDe, _minIndex);
			if (rightmostSide == Position.Left) 
			{
				_orientedDe = _minDe.Sym;
			}
		}

		private void FindRightmostEdgeAtNode()
		{
			
			Node node = _minDe.Node;
			DirectedEdgeStar star = (DirectedEdgeStar) node.Edges;
			_minDe = star.GetRightmostEdge();
			// the DirectedEdge returned by the previous call is not
			// necessarily in the forward direction. Use the sym edge if it isn't.
			if (! _minDe.IsForward) 
			{
				_minDe = _minDe.Sym;
				_minIndex = _minDe.Edge.Coordinates.Count - 1;
			}
			
		}

		private void FindRightmostEdgeAtVertex()
		{
			 // The rightmost point is an interior vertex, so it has a segment on either side of it.
			 // If these segments are both above or below the rightmost point, we need to
			 // determine their relative orientation to decide which is rightmost.

			
			Coordinates pts = _minDe.Edge.Coordinates;
			if (!(_minIndex > 0 && _minIndex < pts.Count))
			{
				throw new InvalidOperationException("Rightmost point expected to be interior vertex of edge.");
			}
			Coordinate pPrev = pts[_minIndex - 1];
			Coordinate pNext = pts[_minIndex + 1];
			int orientation = _cga.ComputeOrientation(_minCoord, pNext, pPrev);
			bool usePrev = false;
			// both segments are below min point
			if (pPrev.Y < _minCoord.Y && pNext.Y < _minCoord.Y
				&& orientation == CGAlgorithms.COUNTERCLOCKWISE) 
			{
				usePrev = true;
			}
			else if (pPrev.Y > _minCoord.Y && pNext.Y > _minCoord.Y
				&& orientation == CGAlgorithms.CLOCKWISE) 
			{
				usePrev = true;
			}
			// if both segments are on the same side, do nothing - either is safe
			// to select as a rightmost segment
			if (usePrev) 
			{
				_minIndex = _minIndex - 1;
			}
			
		}

		private void CheckForRightmostCoordinate(DirectedEdge de)
		{
			
			Coordinates coord = de.Edge.Coordinates;
			// only check vertices which are the starting point of a non-horizontal segment
			for (int i = 0; i < coord.Count - 1; i++) 
			{
				if (coord[i].Y != coord[i + 1].Y)  
				{ // non-horizontal
					if (_minCoord == null || coord[i].X > _minCoord.X ) 
					{
						_minDe = de;
						_minIndex = i;
						_minCoord = coord[i];
					}
				}
			}
		}

		private int GetRightmostSide(DirectedEdge de, int index)
		{
			
			int side = GetRightmostSideOfSegment(de, index);
			if (side < 0)
				side = GetRightmostSideOfSegment(de, index - 1);
			if (side < 0)
				// reaching here can indicate that segment is horizontal
				//Assert.shouldNeverReachHere("problem with finding rightmost side of segment");
				throw new InvalidOperationException("problem with finding rightmost side of segment");
			return side;
		}

		private int GetRightmostSideOfSegment(DirectedEdge de, int i)
		{
			
			Edge e = de.Edge;
			Coordinates coord = e.Coordinates;

			if (i < 0 || i + 1 >= coord.Count) return -1;
			if (coord[i].Y == coord[i + 1].Y) return -1;    // indicates edge is parallel to x-axis

			int pos = Position.Left;
			if (coord[i].Y < coord[i + 1].Y) pos = Position.Right;
			return pos;
		}

		#endregion

	}
}
