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
	public class Vector2f:Tuple2f
	{
		/*
		* $Log: Vector2f.java,v $
		* Revision 1.1  2001/10/13 04:11:14  arty
		* Rest of class contribs by Kenji Hiranabe
		*
		* Revision 1.9  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.9  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.8  1999/03/04  09:16:33  hiranabe
		* small bug fix and copyright change
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
		
		
		/// <summary> Constructs and initializes a Vector2f from the specified xy coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		public Vector2f(float x, float y):base(x, y)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2f from the specified array.</summary>
		/// <param name="v">the array of length 2 containing xy in order
		/// </param>
		public Vector2f(float[] v):base(v)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2f from the specified Vector2f.</summary>
		/// <param name="v1">the Vector2f containing the initialization x y data
		/// </param>
		public Vector2f(Vector2f v1):base(v1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2f from the specified Vector2d.</summary>
		/// <param name="v1">the Vector2f containing the initialization x y data
		/// </param>
		public Vector2f(Vector2d v1):base(v1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2f from the specified Tuple2f.</summary>
		/// <param name="t1">the Tuple2f containing the initialization x y data
		/// </param>
		public Vector2f(Tuple2f t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2f from the specified Tuple2d.</summary>
		/// <param name="t1">the Tuple2d containing the initialization x y data
		/// </param>
		public Vector2f(Tuple2d t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector2f to (0,0).</summary>
		public Vector2f():base()
		{
		}
		
		/// <summary> Computes the dot product of the this vector and vector v1.</summary>
		/// <param name="v1">the other vector
		/// </param>
		public float dot(Vector2f v1)
		{
			return x * v1.x + y * v1.y;
		}
		
		/// <summary> Returns the length of this vector.</summary>
		/// <returns> the length of this vector
		/// </returns>
		public float length()
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Sqrt(x * x + y * y);
		}
		
		/// <summary> Returns the squared length of this vector.</summary>
		/// <returns> the squared length of this vector
		/// </returns>
		public float lengthSquared()
		{
			return x * x + y * y;
		}
		
		/// <summary> Normalizes this vector in place.</summary>
		public void  normalize()
		{
			double d = length();
			
			// zero-div may occur.
			x = (float) (x / d);
			y = (float) (y / d);
		}
		
		/// <summary> Sets the value of this vector to the normalization of vector v1.</summary>
		/// <param name="v1">the un-normalized vector
		/// </param>
		public void  normalize(Vector2f v1)
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
		public float angle(Vector2f v1)
		{
			// stabler than acos
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Abs(System.Math.Atan2(x * v1.y - y * v1.x, dot(v1)));
		}
	}
}