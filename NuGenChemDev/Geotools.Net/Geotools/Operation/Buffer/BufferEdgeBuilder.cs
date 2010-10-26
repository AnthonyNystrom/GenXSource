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


namespace Geotools.Operation.Buffer
{
	/// <summary>
	/// BufferEdgeBuilder creates all the "rough" edges in the buffer for a Geometry.
	/// Rough edges need to be noded together and polygonized to form the final buffer polygon.
	/// </summary>
	internal class BufferEdgeBuilder
	{
		private CGAlgorithms _cga;
		private double _distance;

		//private BufferMultiLineBuilder lineBuilder;
		private BufferLineBuilder _lineBuilder;
		private ArrayList _edgeList = new ArrayList();
		
		
		
		public BufferEdgeBuilder(CGAlgorithms cga, LineIntersector li, double distance, bool makePrecise, int quadrantSegments)
		{
			
			this._cga = cga;
			this._distance = distance;
			//lineBuilder = new BufferMultiLineBuilder(cga, li);
			_lineBuilder = new BufferLineBuilder(_cga, li, makePrecise,quadrantSegments);
			
		}
		#region Properties
		#endregion

		#region Methods

		

		public ArrayList GetEdges(Geometry geom)
		{
			
			Add(geom);
			return _edgeList;
		}

		private void AddEdges(ArrayList lineList, int leftLoc, int rightLoc)
		{
			
			//for (Iterator i = lineList.iterator(); i.hasNext(); ) 
			foreach(object o in lineList)
			{
				// may need to make this Coordinates????
				//Coordinate[] coords = (Coordinate[])o;
				Coordinates coords = (Coordinates)o;
				AddEdge(coords, leftLoc, rightLoc);
			}
			
		}

		/// <summary>
		/// Creates an edge for a coordinate list which is a ring of a buffer, and adds it to the list of buffer
		/// edges.  The ring may be oriented in either direction.  If the ring is oriented CW, the locations will
		/// be: Left: Location.EXTERIOR  Right: Location.INTERIOR
		/// </summary>
		/// <param name="coord"></param>
		/// <param name="leftLoc"></param>
		/// <param name="rightLoc"></param>
		private void AddEdge(Coordinates coord, int leftLoc, int rightLoc)
		{
	
			// don't add null buffers!
			if (coord.Count < 2) return;
			// add the edge for a coordinate list which is a ring of a buffer
			Edge e = new Edge(coord,
				new Label(0, Location.Boundary, leftLoc, rightLoc));
			_edgeList.Add(e);
			
		}


		private void Add(Geometry g)
		{
			
			if ( g.IsEmpty() ) return;

			if (g is Polygon)                 AddPolygon((Polygon) g);
			// LineString also handles LinearRings
			else if (g is LineString)         AddLineString((LineString) g);
			else if (g is Point)              AddPoint((Point) g);
			else if (g is MultiPoint)         AddCollection((MultiPoint) g);
			else if (g is MultiLineString)    AddCollection((MultiLineString) g);
			else if (g is MultiPolygon)       AddCollection((MultiPolygon) g);
			else if (g is GeometryCollection) AddCollection((GeometryCollection) g);
			else throw new NotSupportedException(g.GetType().Name);
			
			
		}

		private void AddCollection(GeometryCollection gc)
		{
			
			for (int i = 0; i < gc.GetNumGeometries(); i++) 
			{
				Geometry g = (Geometry)gc.GetGeometryN(i);
				Add(g);
			}
			
		}

		/// <summary>
		/// Add a Point to the graph
		/// </summary>
		/// <param name="p"></param>
		private void AddPoint(Point p)
		{
			
			if (_distance <= 0.0) return;
			Coordinates coord = p.GetCoordinates();
			ArrayList lineList = _lineBuilder.GetLineBuffer(coord, _distance);
			AddEdges(lineList, Location.Exterior, Location.Interior);
			
		}


		/// <summary>
		/// Add a LineString to the graph
		/// </summary>
		/// <param name="line"></param>
		private void AddLineString(LineString line)
		{
			if (_distance <= 0.0) return;
			Coordinates coords = Coordinates.RemoveRepeatedPoints(line.GetCoordinates());
		
			ArrayList lineList = _lineBuilder.GetLineBuffer(coords, _distance);
			AddEdges(lineList, Location.Exterior, Location.Interior);
			
		}

		/// <summary>
		/// Add a Polygon to the graph.
		/// </summary>
		/// <param name="p">The polygon to add.</param>
		private void AddPolygon(Polygon p)
		{
			
			double lineDistance = _distance;
			int side = Position.Left;
			if (_distance < 0.0) 
			{
				lineDistance = -_distance;
				side = Position.Right;
			}
			int holeSide = (side == Position.Left) ? Position.Right : Position.Left;
			AddPolygonRing(
				(LinearRing) p.GetExteriorRing(),
				lineDistance,
				side,
				Location.Exterior,
				Location.Interior);

			for (int i = 0; i < p.GetNumInteriorRing(); i++) 
			{
				// Holes are topologically labelled opposite to the shell, since
				// the interior of the polygon lies on their opposite side
				// (on the left, if the hole is oriented CCW)
				LinearRing  interiorRing = p.GetInteriorRingN( i );
				AddPolygonRing(
					interiorRing,
					lineDistance,
					Position.Opposite(side),
					Location.Interior,
					Location.Exterior);
			}
			
		}

		/// <summary>
		/// The side and left and right topological location arguments assume that the ring is oriented CW.
		/// If the ring is in the opposite orientation, the left and right locations must be interchanged
		/// and the side flipped.
		/// </summary>
		/// <param name="lr">lr the LinearRing around which to create the buffer</param>
		/// <param name="distance">distance the distance at which to create the buffer</param>
		/// <param name="side">side the side of the ring on which to construct the buffer line</param>
		/// <param name="cwLeftLoc">cwLeftLoc the location on the L side of the ring (if it is CW)</param>
		/// <param name="cwRightLoc">cwRightLoc the location on the R side of the ring (if it is CW)</param>
		private void AddPolygonRing(LinearRing lr, double distance, int side, int cwLeftLoc, int cwRightLoc)
		{
			
			Coordinates coord = Coordinates.RemoveRepeatedPoints(lr.GetCoordinates());
			
			int leftLoc  = cwLeftLoc;
			int rightLoc = cwRightLoc;
			if (_cga.IsCCW(coord)) 
			{
				leftLoc = cwRightLoc;
				rightLoc = cwLeftLoc;
				side = Position.Opposite(side);
			}
			ArrayList lineList = _lineBuilder.GetRingBuffer(coord, side, distance);
			AddEdges(lineList, leftLoc, rightLoc);
			

			/*  This section was already commented out in the java code.  Leave it commented out...
			Edge e = new Edge(coord,
								new Label(0, Location.BOUNDARY, left, right));

			insertEdge(e);
			// insert the endpoint as a node, to mark that it is on the boundary
			insertPoint(argIndex, coord[0], Location.BOUNDARY);
			*/
		}
		
		#endregion

	}
}
