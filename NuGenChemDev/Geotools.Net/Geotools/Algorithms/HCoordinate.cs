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

#region Using Statements
using System;
using Geotools.Geometries;
#endregion

namespace Geotools.Algorithms
{
	/// <summary>
	/// Summary description for HCoordinate.
	/// </summary>
	internal class HCoordinate
	{

		/// <summary>
		/// Computes the (approximate) intersection point between two line segments
		///  using homogeneous coordinates.
		/// </summary>
		/// <remarks>Note that this algorithm is
		/// not numerically stable; i.e. it can produce intersection points which
		/// lie outside the envelope of the line segments themselves.  In order
		/// to increase the precision of the calculation input points should be normalized
		/// before passing them to this routine.
		/// </remarks>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="q1"></param>
		/// <param name="q2"></param>
		/// <returns></returns>
		public static Coordinate Intersection(
			Coordinate p1, Coordinate p2,
			Coordinate q1, Coordinate q2)
		{
			HCoordinate l1 = new HCoordinate(new HCoordinate(p1), new HCoordinate(p2));
			HCoordinate l2 = new HCoordinate(new HCoordinate(q1), new HCoordinate(q2));
			HCoordinate intHCoord = new HCoordinate(l1, l2);
			Coordinate intPt = intHCoord.GetCoordinate();
			return intPt;
		}

		private double _x;
		private double _y;
		private double _w;

		#region Constructor

		/// <summary>
		/// Constructs HCoordinate with x=0.0, y=0.0, and w=1.0.
		/// </summary>
		public HCoordinate()
		{
			_x = 0.0;
			_y = 0.0;
			_w = 1.0;
		}

		/// <summary>
		/// Constructs HCoordinate with x, y, and w.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		public HCoordinate(double x, double y, double w) 
		{
			_x = x;
			_y = y;
			_w = w;
		}

		/// <summary>
		/// Constructs HCoordinate with x=p.X, y=p.Y, and w=1.0.
		/// </summary>
		/// <param name="p"></param>
		public HCoordinate(Coordinate p) 
		{
			_x = p.X;
			_y = p.Y;
			_w = 1.0;
		}

		/// <summary>
		/// Constructs HCoordinate based on p1 and p2.
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		public HCoordinate(HCoordinate p1, HCoordinate p2) 
		{
			_x = p1.Y*p2.W - p2.Y*p1.W;
			_y = p2.X*p1.W - p1.X*p2.W;
			_w = p1.X*p2.Y - p2.X*p1.Y;
		}

		#endregion	

		#region Properties

		/// <summary>
		/// Property for internal x value.
		/// </summary>
		public double X
		{
			get
			{
				return _x;
			}

		}

		/// <summary>
		/// Property for internal y value.
		/// </summary>
		public double Y
		{
			get
			{
				return _y;
			}
		}

		/// <summary>
		/// Property for internal w value.
		/// </summary>
		public double W
		{
			get
			{
				return _w;
			}
		}

		#endregion

		#region Methods
	

		/// <summary>
		/// Returns the new calculated X value.
		/// </summary>
		/// <returns></returns>
		public double GetX()
		{
			double a = _x/_w;
			if ( Double.IsNaN(a) || Double.IsInfinity(a) ) 
			{
				throw new NotRepresentableException("Unable to calculate x value in HCoordinate");
			}			
			return a;
		}

		/// <summary>
		/// Returns the new calculated Y value.
		/// </summary>
		/// <returns></returns>
		public double GetY()
		{
			double a = _y/_w;

			if  ( Double.IsNaN(a) || Double.IsInfinity(a) ) 
			{
				throw new NotRepresentableException("Unable to calculate y value in HCoordinate");
			}
			return a;
		}

		/// <summary>
		/// Returns a Coordinate object based on calculated x and y values.
		/// </summary>
		/// <returns></returns>
		public Coordinate GetCoordinate()
		{
			return new Coordinate( GetX(), GetY() );
		}

		#endregion


	}
}
