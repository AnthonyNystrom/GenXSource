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
	
	/// <summary> A generic 3 element tuple that is represented by
	/// double precision floating point x,y and z coordinates.
	/// </summary>
	/// <version>  specification 1.1, implementation $Revision: 1.10 $, $Date: 1999/10/05 07:03:50 $
	/// </version>
	/// <author>  Kenji hiranabe
	/// </author>
	[Serializable]
	public abstract class Tuple3d
	{
		/*
		* $Log: Tuple3d.java,v $
		* Revision 1.10  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.10  1999/10/05  07:03:50  hiranabe
		* copyright change
		*
		* Revision 1.9  1999/03/04  09:16:33  hiranabe
		* small bug fix and copyright change
		*
		* Revision 1.8  1998/10/14  00:49:10  hiranabe
		* API1.1 Beta02
		*
		* Revision 1.7  1998/07/27  04:28:13  hiranabe
		* API1.1Alpha01 ->API1.1Alpha03
		*
		* Revision 1.6  1998/04/17  10:30:46  hiranabe
		* null check for equals
		*
		* Revision 1.5  1998/04/09  08:18:15  hiranabe
		* minor comment change
		*
		* Revision 1.4  1998/04/09  07:05:18  hiranabe
		* API 1.1
		*
		* Revision 1.3  1998/01/06  00:22:00  hiranabe
		* typo of scale method
		*
		* Revision 1.2  1998/01/05  06:29:31  hiranabe
		* copyright 98
		*
		* Revision 1.1  1997/11/26  03:00:44  hiranabe
		* Initial revision
		*
		*/
		
		
		/// <summary> The x coordinate.</summary>
		public double x;
		
		/// <summary> The y coordinate.</summary>
		public double y;
		
		/// <summary> The z coordinate.</summary>
		public double z;
		
		/// <summary> Constructs and initializes a Tuple3d from the specified xyz coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		public Tuple3d(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		
		/// <summary> Constructs and initializes a Tuple3d from the specified array.</summary>
		/// <param name="t">the array of length 3 containing xyz in order
		/// </param>
		public Tuple3d(double[] t)
		{
			// ArrayIndexOutOfBounds is thrown if t.length < 3
			this.x = t[0];
			this.y = t[1];
			this.z = t[2];
		}
		
		/// <summary> Constructs and initializes a Tuple3d from the specified Tuple3d.</summary>
		/// <param name="t1">the Tuple3d containing the initialization x y z data
		/// </param>
		public Tuple3d(Tuple3d t1)
		{
			x = t1.x;
			y = t1.y;
			z = t1.z;
		}
		
		/// <summary> Constructs and initializes a Tuple3d from the specified Tuple3f.</summary>
		/// <param name="t1">the Tuple3f containing the initialization x y z data
		/// </param>
		public Tuple3d(Tuple3f t1)
		{
			x = t1.x;
			y = t1.y;
			z = t1.z;
		}
		
		/// <summary> Constructs and initializes a Tuple3d to (0,0,0).</summary>
		public Tuple3d()
		{
			x = 0.0f;
			y = 0.0f;
			z = 0.0f;
		}
		
		/// <summary> Sets the value of this tuple to the specified xyz coordinates.</summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		public void  set_Renamed(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		
		/// <summary> Sets the value of this tuple from the 3 values specified in the array.</summary>
		/// <param name="t">the array of length 3 containing xyz in order
		/// </param>
		public void  set_Renamed(double[] t)
		{
			// ArrayIndexOutOfBounds is thrown if t.length < 3
			x = t[0];
			y = t[1];
			z = t[2];
		}
		
		/// <summary> Sets the value of this tuple to the value of the Tuple3d argument.</summary>
		/// <param name="t1">the tuple to be copied
		/// </param>
		public void  set_Renamed(Tuple3d t1)
		{
			x = t1.x;
			y = t1.y;
			z = t1.z;
		}
		
		/// <summary> Sets the value of this tuple to the value of the Tuple3f argument.</summary>
		/// <param name="t1">the tuple to be copied
		/// </param>
		public void  set_Renamed(Tuple3f t1)
		{
			x = t1.x;
			y = t1.y;
			z = t1.z;
		}
		
		/// <summary> Copies the value of the elements of this tuple into the array t[]. </summary>
		/// <param name="t">the array that will contain the values of the vector
		/// </param>
		public void  get_Renamed(double[] t)
		{
			// ArrayIndexOutOfBounds is thrown if t.length < 3
			t[0] = x;
			t[1] = y;
			t[2] = z;
		}
		
		/// <summary> Gets the value of this tuple and copies the values into the Tuple3d.</summary>
		/// <param name="t">Tuple3d object into which that values of this object are copied
		/// </param>
		public void  get_Renamed(Tuple3d t)
		{
			t.x = x;
			t.y = y;
			t.z = z;
		}
		
		/// <summary> Sets the value of this tuple to the vector sum of tuples t1 and t2.</summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="t2">the second tuple
		/// </param>
		public void  add(Tuple3d t1, Tuple3d t2)
		{
			x = t1.x + t2.x;
			y = t1.y + t2.y;
			z = t1.z + t2.z;
		}
		
		/// <summary> Sets the value of this tuple to the vector sum of itself and tuple t1.</summary>
		/// <param name="t1"> the other tuple
		/// </param>
		public void  add(Tuple3d t1)
		{
			x += t1.x;
			y += t1.y;
			z += t1.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the vector difference of tuple t1 and t2 (this = t1 - t2).</summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="t2">the second tuple
		/// </param>
		public void  sub(Tuple3d t1, Tuple3d t2)
		{
			x = t1.x - t2.x;
			y = t1.y - t2.y;
			z = t1.z - t2.z;
		}
		
		/// <summary> Sets the value of this tuple to the vector difference of itself and tuple t1 (this = this - t1).</summary>
		/// <param name="t1">the other tuple
		/// </param>
		public void  sub(Tuple3d t1)
		{
			x -= t1.x;
			y -= t1.y;
			z -= t1.z;
		}
		
		/// <summary> Sets the value of this tuple to the negation of tuple t1. </summary>
		/// <param name="t1">the source vector
		/// </param>
		public void  negate(Tuple3d t1)
		{
			x = - t1.x;
			y = - t1.y;
			z = - t1.z;
		}
		
		/// <summary> Negates the value of this vector in place.</summary>
		public void  negate()
		{
			x = - x;
			y = - y;
			z = - z;
		}
		
		
		/// <summary> Sets the value of this tuple to the scalar multiplication of tuple t1.</summary>
		/// <param name="s">the scalar value
		/// </param>
		/// <param name="t1">the source tuple
		/// </param>
		public void  scale(double s, Tuple3d t1)
		{
			x = s * t1.x;
			y = s * t1.y;
			z = s * t1.z;
		}
		
		/// <summary> Sets the value of this tuple to the scalar multiplication of itself.</summary>
		/// <param name="s">the scalar value
		/// </param>
		public void  scale(double s)
		{
			x *= s;
			y *= s;
			z *= s;
		}
		
		/// <summary> Sets the value of this tuple to the scalar multiplication of tuple t1 and then
		/// adds tuple t2 (this = s*t1 + t2).
		/// </summary>
		/// <param name="s">the scalar value
		/// </param>
		/// <param name="t1">the tuple to be multipled
		/// </param>
		/// <param name="t2">the tuple to be added
		/// </param>
		public void  scaleAdd(double s, Tuple3d t1, Tuple3d t2)
		{
			x = s * t1.x + t2.x;
			y = s * t1.y + t2.y;
			z = s * t1.z + t2.z;
		}
		
		/// <summary> Sets the value of this tuple to the scalar multiplication of itself and then
		/// adds tuple t1 (this = s*this + t1).
		/// </summary>
		/// <param name="s">the scalar value
		/// </param>
		/// <param name="t1">the tuple to be added
		/// </param>
		public void  scaleAdd(double s, Tuple3d t1)
		{
			x = s * x + t1.x;
			y = s * y + t1.y;
			z = s * z + t1.z;
		}
		
		/// <summary> Returns a hash number based on the data values in this object. 
		/// Two different Tuple3d objects with identical data  values
		/// (ie, returns true for equals(Tuple3d) ) will return the same hash number.
		/// Two vectors with different data members may return the same hash value,
		/// although this is not likely.
		/// </summary>
        //public override int GetHashCode()
        //{
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    long xbits = Double.doubleToLongBits(x);
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    long ybits = Double.doubleToLongBits(y);
        //    //UPGRADE_ISSUE: Method 'java.lang.Double.doubleToLongBits' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangDoubledoubleToLongBits_double'"
        //    long zbits = Double.doubleToLongBits(z);
        //    return (int) (xbits ^ (xbits >> 32) ^ ybits ^ (ybits >> 32) ^ zbits ^ (zbits >> 32));
        //}
		
		/// <summary> Returns true if all of the data members of Tuple3d t1 are equal to the corresponding
		/// data members in this
		/// </summary>
		/// <param name="t1">the vector with which the comparison is made.
		/// </param>
		public bool equals(Tuple3d t1)
		{
			return t1 != null && x == t1.x && y == t1.y && z == t1.z;
		}
		
		/// <summary> Returns true if the L-infinite distance between this tuple and tuple t1 is
		/// less than or equal to the epsilon parameter, otherwise returns false. The L-infinite
		/// distance is equal to MAX[abs(x1-x2), abs(y1-y2)].
		/// </summary>
		/// <param name="t1">the tuple to be compared to this tuple
		/// </param>
		/// <param name="epsilon">the threshold value
		/// </param>
		public virtual bool epsilonEquals(Tuple3d t1, double epsilon)
		{
			return (System.Math.Abs(t1.x - this.x) <= epsilon) && (System.Math.Abs(t1.y - this.y) <= epsilon) && (System.Math.Abs(t1.z - this.z) <= epsilon);
		}
		
		/// <summary> Returns a string that contains the values of this Tuple3d. The form is (x,y,z).</summary>
		/// <returns> the String representation
		/// </returns>
		public override System.String ToString()
		{
			return "(" + x + ", " + y + ", " + z + ")";
		}
		
		/// <summary> Sets each component of the tuple parameter to its absolute value and
		/// places the modified values into this tuple.
		/// </summary>
		/// <param name="t">the source tuple, which will not be modified
		/// </param>
		public void  absolute(Tuple3d t)
		{
			set_Renamed(t);
			absolute();
		}
		
		/// <summary> Sets each component of this tuple to its absolute value.</summary>
		public void  absolute()
		{
			if (x < 0.0)
				x = - x;
			if (y < 0.0)
				y = - y;
			if (z < 0.0)
				z = - z;
		}
		
		/// <summary> Clamps this tuple to the range [low, high].</summary>
		/// <param name="min">the lowest value in this tuple after clamping
		/// </param>
		/// <param name="max">the highest value in this tuple after clamping
		/// </param>
		/// <deprecated> As of Java3D API 1.1 Beta02
		/// </deprecated>
		public void  clamp(float min, float max)
		{
			// why float ?
			clampMin(min);
			clampMax(max);
		}
		
		/// <summary> Clamps the tuple parameter to the range [low, high] and places the values
		/// into this tuple.
		/// </summary>
		/// <param name="min">the lowest value in the tuple after clamping
		/// </param>
		/// <param name="max">the highest value in the tuple after clamping
		/// </param>
		/// <param name="t">the source tuple, which will not be modified
		/// </param>
		/// <deprecated> As of Java3D API 1.1 Beta02
		/// </deprecated>
		public void  clamp(float min, float max, Tuple3d t)
		{
			// why float ?
			set_Renamed(t);
			clamp(min, max);
		}
		
		/// <summary> Clamps the minimum value of this tuple to the min parameter.</summary>
		/// <param name="min">the lowest value in this tuple after clamping
		/// </param>
		/// <deprecated> As of Java3D API 1.1 Beta02
		/// </deprecated>
		public void  clampMin(float min)
		{
			// why float ?
			if (x < min)
				x = min;
			if (y < min)
				y = min;
			if (z < min)
				z = min;
		}
		
		/// <summary> Clamps the minimum value of the tuple parameter to the min parameter
		/// and places the values into this tuple.
		/// </summary>
		/// <param name="min">the lowest value in the tuple after clamping
		/// </param>
		/// <parm>  t the source tuple, which will not be modified </parm>
		/// <deprecated> As of Java3D API 1.1 Beta02
		/// </deprecated>
		public void  clampMin(float min, Tuple3d t)
		{
			// why float ?
			set_Renamed(t);
			clampMin(min);
		}
		
		/// <summary> Clamps the maximum value of the tuple parameter to the max parameter and
		/// places the values into this tuple.
		/// </summary>
		/// <param name="max">the highest value in the tuple after clamping
		/// </param>
		/// <param name="t">the source tuple, which will not be modified
		/// </param>
		/// <deprecated> As of Java3D API 1.1 Beta02
		/// </deprecated>
		public void  clampMax(float max, Tuple3d t)
		{
			// why float ?
			set_Renamed(t);
			clampMax(max);
		}
		
		
		/// <summary> Clamps the maximum value of this tuple to the max parameter.</summary>
		/// <param name="max">the highest value in the tuple after clamping
		/// </param>
		/// <deprecated> As of Java3D API 1.1 Beta02
		/// </deprecated>
		public void  clampMax(float max)
		{
			// why float ?
			if (x > max)
				x = max;
			if (y > max)
				y = max;
			if (z > max)
				z = max;
		}
		
		/// <summary> Clamps this tuple to the range [low, high].</summary>
		/// <param name="min">the lowest value in this tuple after clamping
		/// </param>
		/// <param name="max">the highest value in this tuple after clamping
		/// </param>
		public void  clamp(double min, double max)
		{
			clampMin(min);
			clampMax(max);
		}
		
		/// <summary> Clamps the tuple parameter to the range [low, high] and places the values
		/// into this tuple.
		/// </summary>
		/// <param name="min">the lowest value in the tuple after clamping
		/// </param>
		/// <param name="max">the highest value in the tuple after clamping
		/// </param>
		/// <param name="t">the source tuple, which will not be modified
		/// </param>
		public void  clamp(double min, double max, Tuple3d t)
		{
			set_Renamed(t);
			clamp(min, max);
		}
		
		/// <summary> Clamps the minimum value of this tuple to the min parameter.</summary>
		/// <param name="min">the lowest value in this tuple after clamping
		/// </param>
		public void  clampMin(double min)
		{
			if (x < min)
				x = min;
			if (y < min)
				y = min;
			if (z < min)
				z = min;
		}
		
		/// <summary> Clamps the minimum value of the tuple parameter to the min parameter
		/// and places the values into this tuple.
		/// </summary>
		/// <param name="min">the lowest value in the tuple after clamping
		/// </param>
		/// <parm>  t the source tuple, which will not be modified </parm>
		public void  clampMin(double min, Tuple3d t)
		{
			set_Renamed(t);
			clampMin(min);
		}
		
		/// <summary> Clamps the maximum value of the tuple parameter to the max parameter and
		/// places the values into this tuple.
		/// </summary>
		/// <param name="max">the highest value in the tuple after clamping
		/// </param>
		/// <param name="t">the source tuple, which will not be modified
		/// </param>
		public void  clampMax(double max, Tuple3d t)
		{
			set_Renamed(t);
			clampMax(max);
		}
		
		
		/// <summary> Clamps the maximum value of this tuple to the max parameter.</summary>
		/// <param name="max">the highest value in the tuple after clamping
		/// </param>
		public void  clampMax(double max)
		{
			if (x > max)
				x = max;
			if (y > max)
				y = max;
			if (z > max)
				z = max;
		}
		
		/// <summary> Linearly interpolates between tuples t1 and t2 and places the
		/// result into this tuple: this = (1-alpha)*t1 + alpha*t2.
		/// </summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="t2">the second tuple
		/// </param>
		/// <param name="alpha">the alpha interpolation parameter
		/// </param>
		/// <deprecated> As of Java3D API 1.1 Beta02
		/// </deprecated>
		public void  interpolate(Tuple3d t1, Tuple3d t2, float alpha)
		{
			// why float ?
			set_Renamed(t1);
			interpolate(t2, alpha);
		}
		
		
		/// <summary> Linearly interpolates between this tuple and tuple t1 and places the
		/// result into this tuple: this = (1-alpha)*this + alpha*t1.
		/// </summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="alpha">the alpha interpolation parameter
		/// </param>
		/// <deprecated> As of Java3D API 1.1 Beta02
		/// </deprecated>
		public void  interpolate(Tuple3d t1, float alpha)
		{
			// why float ?
			float beta = 1 - alpha;
			x = beta * x + alpha * t1.x;
			y = beta * y + alpha * t1.y;
			z = beta * z + alpha * t1.z;
		}
		
		/// <summary> Linearly interpolates between tuples t1 and t2 and places the
		/// result into this tuple: this = (1-alpha)*t1 + alpha*t2.
		/// </summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="t2">the second tuple
		/// </param>
		/// <param name="alpha">the alpha interpolation parameter
		/// </param>
		public void  interpolate(Tuple3d t1, Tuple3d t2, double alpha)
		{
			set_Renamed(t1);
			interpolate(t2, alpha);
		}
		
		
		/// <summary> Linearly interpolates between this tuple and tuple t1 and places the
		/// result into this tuple: this = (1-alpha)*this + alpha*t1.
		/// </summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="alpha">the alpha interpolation parameter
		/// </param>
		public void  interpolate(Tuple3d t1, double alpha)
		{
			double beta = 1 - alpha;
			x = beta * x + alpha * t1.x;
			y = beta * y + alpha * t1.y;
			z = beta * z + alpha * t1.z;
		}
	}
}