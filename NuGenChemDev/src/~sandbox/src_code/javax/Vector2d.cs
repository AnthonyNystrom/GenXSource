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
	
	/// <summary> A 2 element vector that is represented by single precision
	/// floating point x,y coordinates.
	/// </summary>
	/// <version>  specification 1.1, implementation $Revision: 1.1 $, $Date: 2001/10/13 04:11:14 $
	/// </version>
	/// <author>  Kenji hiranabe
	/// </author>
	[Serializable]
	public class Vector2d:Tuple2d
	{
		/*
		* $Log: Vector2d.java,v $
		* Revision 1.1  2001/10/13 04:11:14  arty
		* Rest of class contribs by Kenji Hiranabe
		*
		* Revision 1.4  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.4  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.3  1999/03/04  09:16:33  hiranabe
		* small bug fix and copyright change
		*
		* Revision 1.2  1998/10/14  00:49:10  hiranabe
		* API1.1 Beta02
		*
		# Revision 1.1  1998/07/27  04:28:13  hiranabe
		# API1.1Alpha01 ->API1.1Alpha03
		#
		*
		*/
		
		/// <summary> Constructs and initializes a Vector2d from the specified xy coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		public Vector2d(double x, double y):base(x, y)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2d from the specified array.</summary>
		/// <param name="v">the array of length 2 containing xy in order
		/// </param>
		public Vector2d(double[] v):base(v)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2d from the specified Vector2d.</summary>
		/// <param name="v1">the Vector2d containing the initialization x y data
		/// </param>
		public Vector2d(Vector2d v1):base(v1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2d from the specified Vector2f.</summary>
		/// <param name="v1">the Vector2f containing the initialization x y data
		/// </param>
		public Vector2d(Vector2f v1):base(v1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2d from the specified Tuple2d.</summary>
		/// <param name="t1">the Tuple2d containing the initialization x y data
		/// </param>
		public Vector2d(Tuple2d t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2d from the specified Tuple2f.</summary>
		/// <param name="t1">the Tuple2f containing the initialization x y data
		/// </param>
		public Vector2d(Tuple2f t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2d to (0,0).</summary>
		public Vector2d():base()
		{
		}
		
		/// <summary> Computes the dot product of the this vector and vector v1.</summary>
		/// <param name="v1">the other vector
		/// </param>
		public double dot(Vector2d v1)
		{
			return x * v1.x + y * v1.y;
		}
		
		/// <summary> Returns the length of this vector.</summary>
		/// <returns> the length of this vector
		/// </returns>
		public double length()
		{
			return System.Math.Sqrt(x * x + y * y);
		}
		
		/// <summary> Returns the squared length of this vector.</summary>
		/// <returns> the squared length of this vector
		/// </returns>
		public double lengthSquared()
		{
			return x * x + y * y;
		}
		
		/// <summary> Normalizes this vector in place.</summary>
		public void  normalize()
		{
			double d = length();
			
			// zero-div may occur.
			x /= d;
			y /= d;
		}
		
		/// <summary> Sets the value of this vector to the normalization of vector v1.</summary>
		/// <param name="v1">the un-normalized vector
		/// </param>
		public void  normalize(Vector2d v1)
		{
			set_Renamed(v1);
			normalize();
		}
		
		/// <summary> Returns the angle in radians between this vector and
		/// the vector parameter; the return value is constrained to the
		/// range [0,PI].
		/// </summary>
		/// <param name="v1"> the other vector
		/// </param>
		/// <returns> the angle in radians in the range [0,PI]
		/// </returns>
		public double angle(Vector2d v1)
		{
			// stabler than acos
			return System.Math.Abs(System.Math.Atan2(x * v1.y - y * v1.x, dot(v1)));
		}
	}
}