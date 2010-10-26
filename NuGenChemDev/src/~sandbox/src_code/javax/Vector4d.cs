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
	
	/// <summary> A 4 element vector that is represented by double precision floating point
	/// x,y,z,w coordinates. 
	/// </summary>
	/// <version>  specification 1.1, implementation $Revision: 1.1 $, $Date: 2001/10/13 04:11:14 $
	/// </version>
	/// <author>  Kenji hiranabe
	/// </author>
	[Serializable]
	public class Vector4d:Tuple4d
	{
		/*
		* $Log: Vector4d.java,v $
		* Revision 1.1  2001/10/13 04:11:14  arty
		* Rest of class contribs by Kenji Hiranabe
		*
		* Revision 1.9  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.9  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.8  1999/10/05  04:56:47  hiranabe
		* Java3D 1.2 support
		* Vector4d(Tuple3d t1) constructor
		*  set(Tuple3d t1)
		*
		* Revision 1.7  1999/03/04  09:16:33  hiranabe
		* small bug fix and copyright change
		*
		* Revision 1.6  1998/10/14  00:49:10  hiranabe
		* API1.1 Beta02
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
		
		
		/// <summary> Constructs and initializes a Vector4d from the specified xyzw coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		/// <param name="w">the w coordinate
		/// </param>
		public Vector4d(double x, double y, double z, double w):base(x, y, z, w)
		{
		}
		
		/// <summary> Constructs and initializes a Vector4d from the specified array of length 4.</summary>
		/// <param name="v">the array of length 4 containing xyzw in order
		/// </param>
		public Vector4d(double[] v):base(v)
		{
		}
		
		/// <summary> Constructs and initializes a Vector4d from the specified Vector4d.</summary>
		/// <param name="v1">the Vector4d containing the initialization x y z w data
		/// </param>
		public Vector4d(Vector4f v1):base(v1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector4d from the specified Vector4d.</summary>
		/// <param name="v1">the Vector4d containing the initialization x y z w data
		/// </param>
		public Vector4d(Vector4d v1):base(v1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector4d from the specified Tuple4d.</summary>
		/// <param name="t1">the Tuple4d containing the initialization x y z w data
		/// </param>
		public Vector4d(Tuple4d t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector4d from the specified Tuple4f.</summary>
		/// <param name="t1">the Tuple4f containing the initialization x y z w data
		/// </param>
		public Vector4d(Tuple4f t1):base(t1)
		{
		}
		
		/// <summary> Constructs and initializes a Vector4d to (0,0,0,0).</summary>
		public Vector4d()
		{
			// super(); called implicitly
		}
		
		/// <summary> Constructs and initializes a Vector4d from the specified Tuple3d.
		/// The x,y,z  components of this point are set to the corresponding
		/// components
		/// of tuple t1. The w component of this point is set to 0.
		/// 
		/// </summary>
		/// <param name="t1">the tuple to be copied
		/// </param>
		/// <since> Java3D 1.2
		/// </since>
		public Vector4d(Tuple3d t1):base(t1.x, t1.y, t1.z, 0)
		{
		}
		
		/// <summary> Sets the x,y,z components of this point to the corresponding
		/// components of tuple t1. The w component of this point is set to 1.
		/// 
		/// </summary>
		/// <param name="t1">the tuple to be copied
		/// </param>
		/// <since> Java3D 1.2
		/// </since>
		public void  set_Renamed(Tuple3d t1)
		{
			set_Renamed(t1.x, t1.y, t1.z, 0);
		}
		
		/// <summary> Returns the squared length of this vector.</summary>
		/// <returns> the squared length of this vector
		/// </returns>
		public double lengthSquared()
		{
			return x * x + y * y + z * z + w * w;
		}
		
		/// <summary> Returns the length of this vector.</summary>
		/// <returns> the length of this vector
		/// </returns>
		public double length()
		{
			return System.Math.Sqrt(lengthSquared());
		}
		
		/// <summary> Computes the dot product of the this vector and vector v1.</summary>
		/// <param name="v1">the other vector
		/// </param>
		/// <returns> the dot product of this vector and vector v1
		/// </returns>
		public double dot(Vector4d v1)
		{
			return x * v1.x + y * v1.y + z * v1.z + w * v1.w;
		}
		
		/// <summary> Sets the value of this vector to the normalization of vector v1.</summary>
		/// <param name="v1">the un-normalized vector
		/// </param>
		public void  normalize(Vector4d v1)
		{
			set_Renamed(v1);
			normalize();
		}
		
		/// <summary> Normalizes this vector in place.</summary>
		public void  normalize()
		{
			double d = length();
			
			// zero-div may occur.
			x /= d;
			y /= d;
			z /= d;
			w /= d;
		}
		
		/// <summary> Returns the (4-space) angle in radians between this vector and
		/// the vector parameter; the return value is constrained to the
		/// range [0,PI].
		/// </summary>
		/// <param name="v1"> the other vector
		/// </param>
		/// <returns> the angle in radians in the range [0,PI]
		/// </returns>
		public double angle(Vector4d v1)
		{
			// zero div may occur.
			double d = dot(v1);
			double v1_length = v1.length();
			double v_length = length();
			
			// numerically, domain error may occur
			return (double) System.Math.Acos(d / v1_length / v_length);
		}
	}
}