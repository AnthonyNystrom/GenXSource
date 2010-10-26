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
using Geotools.Geometries;
using Geotools.Algorithms;
#endregion

namespace Geotools.Graph
{
	/// <summary> Models the end of an edge incident on a node.
	/// EdgeEnds have a direction
	/// determined by the direction of the ray from the initial
	/// point to the next point.
	/// EdgeEnds are comparable under the ordering
	/// "a has a greater angle with the x-axis than b".
	/// This ordering is used to sort EdgeEnds around a node.
	/// </summary>
	internal class EdgeEnd : IComparable
	{
		protected static CGAlgorithms _cga = new RobustCGAlgorithms();

		protected Edge _edge;  // the parent edge of this edge end
		protected Label _label;

		private Node _node;          // the node this edge end originates at
		private Coordinate _p0, _p1;  // points of initial line segment
		private double _dx, _dy;      // the direction vector for this edge from its starting point
		private int _quadrant;

		#region Constructors
		/// <summary>
		/// Constructs a new instance of the EdgeEnd class.
		/// </summary>
		protected EdgeEnd(Edge edge)
		{
			_edge = edge;
		}

		/// <summary>
		/// Constructs a new instance of the EdgeEnd class.
		/// </summary>
		/// <param name="edge"></param>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		public EdgeEnd(Edge edge, Coordinate p0, Coordinate p1) : this( edge, p0, p1, null)
		{
		}

		public EdgeEnd(Edge edge, Coordinate p0, Coordinate p1, Label label) : this( edge ) 
		{
			Initialize(p0, p1);
			_label = label;
		}

		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public Edge Edge
		{
			get
			{ 
				return _edge;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Label Label
		{ 
			get 
			{ 
				return _label;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Coordinate Coordinate 
		{ 
			get 
			{
				return _p0; 
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Coordinate DirectedCoordinate 
		{ 
			get
			{ 
				return _p1;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int QuadrantLocation
		{ 
			get
			{ 
				return _quadrant; 
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public double Dx 
		{ 
			get
			{ 
				return _dx; 
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public double Dy
		{
			get
			{ 
				return _dy; 
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Node Node
		{
			get
			{
				return _node;
			}
			set
			{
				_node = value;
			}

		}
		
		#endregion
		
		#region Implementation of IComparable
		public int CompareTo(object obj)
		{
			EdgeEnd e = (EdgeEnd) obj;
			return CompareDirection(e);
		}
		#endregion

		#region Methods
		protected void Initialize( Coordinate p0, Coordinate p1 )
		{
			_p0 = p0;
			_p1 = p1;
			_dx = p1.X - p0.X;
			_dy = p1.Y - p0.Y;
			_quadrant = Quadrant.QuadrantLocation(_dx, _dy);
			if(_dx == 0 && _dy == 0)
			{
				throw new InvalidOperationException("EdgeEnd with identical endpoints found.");
			}
		}


		/// <summary>
		///  Implements the total order relation.
		/// </summary>
		/// <remarks>
		/// <para>Implements the total order relation: a has a greater angle with the positive x-axis than b.</para>
		/// <para>
		/// Using the obvious algorithm of simply computing the angle is not robust,
		/// since the angle calculation is obviously susceptible to roundoff.
		/// A robust algorithm is: - first compare the quadrant.  If the quadrants
		/// are different, it it trivial to determine which vector is "greater".
		/// - if the vectors lie in the same quadrant, the computeOrientation function
		///  can be used to decide the relative orientation of the vectors.
		/// 
		/// </para> 
		/// 
		/// 
		/// </remarks>
		/// <param name="e"></param>
		/// <returns></returns>
		public int CompareDirection(EdgeEnd e)
		{
			if ( _dx == e.Dx && _dy == e.Dy)
			{
				return 0;
			}
			// if the rays are in different quadrants, determining the ordering is trivial
			if (_quadrant > e.QuadrantLocation) return 1;
			if (_quadrant < e.QuadrantLocation) return -1;
			// vectors are in the same quadrant - check relative orientation of direction vectors
			// this is > e if it is CCW of e
			return _cga.ComputeOrientation( e.Coordinate, e.DirectedCoordinate, _p1);
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void ComputeLabel()
		{
			// subclasses should override this if they are using labels
		}

		/// <summary>
		/// Returns a string representation of this object.  Mostly used in debugging of this library.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			double angle = Math.Atan2( _dy, _dx );    
			string name = GetType().Name;
			return name + ": " + _p0 + " - " + _p1 + " " + _quadrant + ":" + angle + "   " + _label.ToString() ;

		}
		#endregion

	} // public class EdgeEnd : IComparable
}
