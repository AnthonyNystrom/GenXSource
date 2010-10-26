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
	
	/// <summary> A 3 element vector that is represented by single precision floating point
	/// x,y,z coordinates. If this value represents a normal, then it should be
	/// normalized.
	/// </summary>
	/// <version>  specification 1.1, implementation $Revision: 1.3 $, $Date: 2001/10/13 04:09:46 $
	/// </version>
	/// <author>  Kenji hiranabe
	/// </author>
	[Serializable]
	public class Vector3f:Tuple3f
	{
		/*
		* $Log: Vector3f.java,v $
		* Revision 1.3  2001/10/13 04:09:46  arty
		* checkins for the walk demo.
		* Includes classes written in SML.
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
		* Revision 1.6  1998/04/10  04:52:14  hiranabe
		* API1.0 -> API1.1 (added constructors, methods)
		*
		* Revision 1.5  1998/04/09  08:18:15  hiranabe
		* minor comment change
		*
		* Revision 1.4  1998/04/09  07:05:18  hiranabe
		* API 1.1
		*
		* Revision 1.3  1998/01/05  06:29:31  hiranabe
		* copyright 98
		*
		* Revision 1.2  1997/12/28  23:40:13  hiranabe
		* normalize(Vector3d) -> normalize(Vector3f) suggested by leonvs@iaehv.nl
		*
		* Revision 1.1  1997/11/26  03:00:44  hiranabe
		* Initial revision
		*
		*/
		
		
		/// <summary> Constructs and initializes a Vector3f from the specified xyz coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		public Vector3f(float x, float y, float z):base(x, y, z)
		{
		}
		
		/// <summary> Constructs and initializes a Vector3f from the specified array of length 3.</summary>
		/// <param name="v">the array of length 3 containing xyz in order
		/// </param>
		public Vector3f(float[] v):base(v)
		{
		}
		
		/// <summary> Constructs and initializes a Vector3f from the specified Vector3f.</summary>
		/// <param name="v1">the Vector3f containing the initialization x y z data
		/// </param>
		public Vector3f(Vector3f v1):base(v1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector3f from the specified Vector3d.</summary>
		/// <param name="v1">the Vector3d containing the initialization x y z data
		/// </param>
		public Vector3f(Vector3d v1):base(v1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector3f from the specified Tuple3d.</summary>
		/// <param name="t1">the Tuple3d containing the initialization x y z data
		/// </param>
		public Vector3f(Tuple3d t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector3f from the specified Tuple3f.</summary>
		/// <param name="t1">the Tuple3f containing the initialization x y z data
		/// </param>
		public Vector3f(Tuple3f t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector3f to (0,0,0).</summary>
		public Vector3f():base()
		{
		}
		
		/// <summary> Returns the squared length of this vector.</summary>
		/// <returns> the squared length of this vector
		/// </returns>
		public float lengthSquared()
		{
			return x * x + y * y + z * z;
		}
		
		/// <summary> Returns the length of this vector.</summary>
		/// <returns> the length of this vector
		/// </returns>
		public float length()
		{
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Sqrt(lengthSquared());
		}
		
		/// <summary> Sets this vector to be the vector cross product of vectors v1 and v2.</summary>
		/// <param name="v1">the first vector
		/// </param>
		/// <param name="v2">the second vector
		/// </param>
		public void  cross(Vector3f v1, Vector3f v2)
		{
			// store in tmp once for aliasing-safty
			// i.e. safe when a.cross(a, b)
			set_Renamed(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
		}
		
		
		/// <summary> Computes the dot product of the this vector and vector v1.</summary>
		/// <param name="v1">the other vector
		/// </param>
		public float dot(Vector3f v1)
		{
			return x * v1.x + y * v1.y + z * v1.z;
		}
		
		/// <summary> Sets the value of this vector to the normalization of vector v1.</summary>
		/// <param name="v1">the un-normalized vector
		/// </param>
		public void  normalize(Vector3f v1)
		{
			set_Renamed(v1);
			normalize();
		}
		
		/// <summary> Normalizes this vector in place.</summary>
		public void  normalize()
		{
			double d = length();
			
			// zero-div may occur.
			x = (float) (x / d);
			y = (float) (y / d);
			z = (float) (z / d);
		}
		
		/// <summary> Returns the angle in radians between this vector and
		/// the vector parameter; the return value is constrained to the
		/// range [0,PI].
		/// </summary>
		/// <param name="v1"> the other vector
		/// </param>
		/// <returns> the angle in radians in the range [0,PI]
		/// </returns>
		public float angle(Vector3f v1)
		{
			// return (double)Math.acos(dot(v1)/v1.length()/v.length());
			// Numerically, near 0 and PI are very bad condition for acos.
			// In 3-space, |atan2(sin,cos)| is much stable.
			
			double xx = y * v1.z - z * v1.y;
			double yy = z * v1.x - x * v1.z;
			double zz = x * v1.y - y * v1.x;
			double cross = System.Math.Sqrt(xx * xx + yy * yy + zz * zz);
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			return (float) System.Math.Abs(System.Math.Atan2(cross, dot(v1)));
		}
	}
}