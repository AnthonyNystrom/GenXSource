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
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph;
#endregion

namespace Geotools.Operation.Overlay
{
	/// <summary>
	/// Summary description for PointBuilder.
	/// </summary>
	internal class PointBuilder
	{
		private OverlayOp _op;
		private GeometryFactory _geometryFactory;
		private PointLocator _ptLocator;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the PointBuilder class.
		/// </summary>
		public PointBuilder( OverlayOp op, GeometryFactory geometryFactory, PointLocator ptLocator ) 
		{
			_op = op;
			_geometryFactory = geometryFactory;
			_ptLocator = ptLocator;
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="opCode"></param>
		/// <returns>A list of the Points in the result of the specified overlay operation</returns>
		public ArrayList Build( int opCode )
		{
			ArrayList nodeList = CollectNodes( opCode );
			ArrayList resultPointList = SimplifyPoints( nodeList );
			return resultPointList;
		} // public ArrayList Build( int opCode )

		private ArrayList CollectNodes( int opCode )
		{
			ArrayList resultNodeList = new ArrayList();
			// add nodes from edge intersections which have not already been included in the result
			foreach ( DictionaryEntry obj in _op.Graph.Nodes )
			{
				Node n = (Node) obj.Value;
				if ( !n.IsInResult ) 
				{
					Label label = n.Label;
					if ( OverlayOp.IsResultOfOp( label, opCode ) ) 
					{
						resultNodeList.Add( n );
					}
				} // if ( !n.IsInResult )
			} // foreach ( object obj in _op.Graph.Nodes )
			return resultNodeList;
		} // private ArrayList CollectNodes( int opCode )

		/// <summary>
		/// This method simplifies the resultant Geometry by finding and eliminating
		/// "covered" points.
		/// A point is covered if it is contained in another element Geometry
		/// with higher dimension (e.g. a point might be contained in a polygon,
		/// in which case the point can be eliminated from the resultant).
		/// </summary>
		/// <param name="resultNodeList"></param>
		/// <returns></returns>
		private ArrayList SimplifyPoints( ArrayList resultNodeList )
		{
			ArrayList nonCoveredPointList = new ArrayList();
			foreach ( object obj in resultNodeList )
			{
				Node n = (Node) obj;
				Coordinate coord = n.GetCoordinate();
				if ( !_op.IsCoveredByLA( coord ) )
				{
					Point pt = _geometryFactory.CreatePoint( coord );
					nonCoveredPointList.Add( pt );
				} // if ( !_op.IsCoveredByLA( coord ) )
			} // foreach ( object obj in resultNodeList )
			return nonCoveredPointList;
		} // private ArrayList SimplifyPoints( ArrayList resultNodeList )
		#endregion

	} // public class PointBuilder
}
