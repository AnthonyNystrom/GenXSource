/*
Copyright (C) 1997,1998,1999
Kenji Hiranabe, Eiwa System Management, Inc.

This program is free software.
Implemented by Kenji Hiranabe(hiranabe@esm.co.jp),
conforming to the Java(TM) 3D API specification by Sun Microsystems.

Permission to use, copy, modify, distribute and sell this software
and its documentation for any purpose is hereby granted without fee,
provided that the above copyright notice appear in all copies and
that both that copyright notice and this permission notice appear
in supporting documentation. Kenji Hiranabe and Eiwa System Management,Inc.
makes no representations about the suitability of this software for any
purpose.  It is provided "AS IS" with NO WARRANTY.*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A 2 element point that is represented by single precision
	/// floating point x,y coordinates.
	/// </summary>
	/// <version>  specification 1.1, implementation $Revision: 1.2 $, $Date: 2001/10/13 04:09:46 $
	/// </version>
	/// <author>  Kenji hiranabe
	/// </author>
	[Serializable]
	public class Point2f:Tuple2f
	{
		/*
		* $Log: Point2f.java,v $
		* Revision 1.2  2001/10/13 04:09:46  arty
		* checkins for the walk demo.
		* Includes classes written in SML.
		*
		* Revision 1.10  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.10  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.9  1999/03/04  09:16:33  hiranabe
		* small bug fix and copyright change
		*
		* Revision 1.8  1998/10/16  00:10:11  hiranabe
		* distanceSquared bug(thanks > nhv@laserplot.com)
		*
		* Revision 1.7  1998/10/14  00:49:10  hiranabe
		* API1.1 Beta02
		*
		* Revision 1.6  1998/07/27  04:28:13  hiranabe
		* API1.1Alpha01 ->API1.1Alpha03
		*
		* Revision 1.5  1998/04/10  04:52:14  hiranabe
		* API1.0 -> API1.1 (added constructors, methods)
		*
		* Revision 1.4  1998/04/09  08:18:15  hiranabe
		* minor comment change
		*
		* Revision 1.3  1998/04/09  07:05:18  hiranabe
		* API 1.1
		*
		* Revision 1.2  1998/01/05  06:29:31  hiranabe
		* copyright 98
		*
		* Revision 1.1  1997/11/26  03:00:44  hiranabe
		* Initial revision
		*
		*/
		
		
		/// <summary> Constructs and initializes a Point2f from the specified xy coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		public Point2f(float x, float y):base(x, y)
		{
		}
		
		/// <summary> Constructs and initializes a Point2f from the specified array.</summary>
		/// <param name="p">the array of length 2 containing xy in order
		/// </param>
		public Point2f(float[] p):base(p)
		{
		}
		
		/// <summary> Constructs and initializes a Point2f from the specified Point2f.</summary>
		/// <param name="p1">the Point2f containing the initialization x y data
		/// </param>
		public Point2f(Point2f p1):base(p1)
		{
		}
		
		/// <summary> Constructs and initializes a Point2f from the specified Point2d.</summary>
		/// <param name="p1">the Point2d containing the initialization x y data
		/// </param>
		public Point2f(Point2d p1):base(p1)
		{
		}
		
		/// <summary> Constructs and initializes a Point2f from the specified Tuple2f.</summary>
		/// <param name="t1">the Tuple2f containing the initialization x y data
		/// </param>
		public Point2f(Tuple2f t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Point2f from the specified Tuple2d.</summary>
		/// <param name="t1">the Tuple2d containing the initialization x y data
		/// </param>
		public Point2f(Tuple2d t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Point2f to (0,0).</summary>
		public Point2f()
		{
			// super(); called implicitly.
		}
		
		/// <summary> Computes the square of the distance between this point and point p1.</summary>
		/// <param name="p1">the other point
		/// </param>
		public float distanceSquared(Point2f p1)
		{
			double dx = x - p1.x;
			double dy = y - p1.y;
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) (dx * dx + dy * dy);
		}
		
		/// <summary> Computes the distance between this point and point p1.</summary>
		/// <param name="p1">the other point
		/// </param>
		public float distance(Point2f p1)
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Sqrt(distanceSquared(p1));
		}
		
		/// <summary> Computes the L-1 (Manhattan) distance between this point and point p1.
		/// The L-1 distance is equal to abs(x1-x2) + abs(y1-y2).
		/// </summary>
		/// <param name="p1">the other point
		/// </param>
		public float distanceL1(Point2f p1)
		{
			return System.Math.Abs(x - p1.x) + System.Math.Abs(y - p1.y);
		}
		
		/// <summary> Computes the L-infinite distance between this point and point p1.
		/// The L-infinite distance is equal to MAX[abs(x1-x2), abs(y1-y2)].
		/// </summary>
		/// <param name="p1">the other point
		/// </param>
		public float distanceLinf(Point2f p1)
		{
			return System.Math.Max(System.Math.Abs(x - p1.x), System.Math.Abs(y - p1.y));
		}
	}
}