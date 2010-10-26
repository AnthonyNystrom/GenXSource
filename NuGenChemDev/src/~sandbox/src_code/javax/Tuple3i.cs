/*
* $RCSfile: Tuple3i.java,v $
*
* Copyright (c) 2006 Sun Microsystems, Inc. All rights reserved.
*
* Use is subject to license terms.
*
* $Revision: 1.5 $
* $Date: 2006/09/29 18:05:22 $
* $State: Exp $
*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A 3-element tuple represented by signed integer x,y,z
	/// coordinates.
	/// 
	/// </summary>
	/// <since> vecmath 1.2
	/// </since>
	[Serializable]
	public abstract class Tuple3i : System.ICloneable
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Get the <i>x</i> coordinate.
		/// 
		/// </summary>
		/// <returns>  the <i>x</i> coordinate.
		/// 
		/// </returns>
		/// <since> vecmath 1.5
		/// </since>
		/// <summary> Set the <i>x</i> coordinate.
		/// 
		/// </summary>
		/// <param name="x"> value to <i>x</i> coordinate.
		/// 
		/// </param>
		/// <since> vecmath 1.5
		/// </since>
		virtual public int X
		{
			get
			{
				return x;
			}
			
			set
			{
				this.x = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Get the <i>y</i> coordinate.
		/// 
		/// </summary>
		/// <returns>  the <i>y</i> coordinate.
		/// 
		/// </returns>
		/// <since> vecmath 1.5
		/// </since>
		/// <summary> Set the <i>y</i> coordinate.
		/// 
		/// </summary>
		/// <param name="y">value to <i>y</i> coordinate.
		/// 
		/// </param>
		/// <since> vecmath 1.5
		/// </since>
		virtual public int Y
		{
			get
			{
				return y;
			}
			
			set
			{
				this.y = value;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Get the <i>z</i> coordinate.
		/// 
		/// </summary>
		/// <returns> the <i>z</i> coordinate.
		/// </returns>
		/// <since> vecmath 1.5
		/// </since>
		/// <summary> Set the <i>z</i> coordinate.
		/// 
		/// </summary>
		/// <param name="z">value to <i>z</i> coordinate.
		/// 
		/// </param>
		/// <since> vecmath 1.5
		/// </since>
		virtual public int Z
		{
			get
			{
				return z;
			}
			
			set
			{
				this.z = value;
			}
			
		}
		
		internal const long serialVersionUID = - 732740491767276200L;
		
		/// <summary> The x coordinate.</summary>
		public int x;
		
		/// <summary> The y coordinate.</summary>
		public int y;
		
		/// <summary> The z coordinate.</summary>
		public int z;
		
		
		/// <summary> Constructs and initializes a Tuple3i from the specified
		/// x, y, and z coordinates.
		/// </summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		public Tuple3i(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		
		
		/// <summary> Constructs and initializes a Tuple3i from the array of length 3.</summary>
		/// <param name="t">the array of length 3 containing x, y, and z in order.
		/// </param>
		public Tuple3i(int[] t)
		{
			this.x = t[0];
			this.y = t[1];
			this.z = t[2];
		}
		
		
		/// <summary> Constructs and initializes a Tuple3i from the specified Tuple3i.</summary>
		/// <param name="t1">the Tuple3i containing the initialization x, y, and z
		/// data.
		/// </param>
		public Tuple3i(Tuple3i t1)
		{
			this.x = t1.x;
			this.y = t1.y;
			this.z = t1.z;
		}
		
		
		/// <summary> Constructs and initializes a Tuple3i to (0,0,0).</summary>
		public Tuple3i()
		{
			this.x = 0;
			this.y = 0;
			this.z = 0;
		}
		
		
		/// <summary> Sets the value of this tuple to the specified x, y, and z
		/// coordinates.
		/// </summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		/// <param name="z">the z coordinate
		/// </param>
		public void  set_Renamed(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		
		
		/// <summary> Sets the value of this tuple to the specified coordinates in the
		/// array of length 3.
		/// </summary>
		/// <param name="t">the array of length 3 containing x, y, and z in order.
		/// </param>
		public void  set_Renamed(int[] t)
		{
			this.x = t[0];
			this.y = t[1];
			this.z = t[2];
		}
		
		
		/// <summary> Sets the value of this tuple to the value of tuple t1.</summary>
		/// <param name="t1">the tuple to be copied
		/// </param>
		public void  set_Renamed(Tuple3i t1)
		{
			this.x = t1.x;
			this.y = t1.y;
			this.z = t1.z;
		}
		
		
		/// <summary> Copies the values of this tuple into the array t.</summary>
		/// <param name="t">is the array
		/// </param>
		public void  get_Renamed(int[] t)
		{
			t[0] = this.x;
			t[1] = this.y;
			t[2] = this.z;
		}
		
		
		/// <summary> Copies the values of this tuple into the tuple t.</summary>
		/// <param name="t">is the target tuple
		/// </param>
		public void  get_Renamed(Tuple3i t)
		{
			t.x = this.x;
			t.y = this.y;
			t.z = this.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the sum of tuples t1 and t2.</summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="t2">the second tuple
		/// </param>
		public void  add(Tuple3i t1, Tuple3i t2)
		{
			this.x = t1.x + t2.x;
			this.y = t1.y + t2.y;
			this.z = t1.z + t2.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the sum of itself and t1.</summary>
		/// <param name="t1">the other tuple
		/// </param>
		public void  add(Tuple3i t1)
		{
			this.x += t1.x;
			this.y += t1.y;
			this.z += t1.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the difference
		/// of tuples t1 and t2 (this = t1 - t2).
		/// </summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="t2">the second tuple
		/// </param>
		public void  sub(Tuple3i t1, Tuple3i t2)
		{
			this.x = t1.x - t2.x;
			this.y = t1.y - t2.y;
			this.z = t1.z - t2.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the difference
		/// of itself and t1 (this = this - t1).
		/// </summary>
		/// <param name="t1">the other tuple
		/// </param>
		public void  sub(Tuple3i t1)
		{
			this.x -= t1.x;
			this.y -= t1.y;
			this.z -= t1.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the negation of tuple t1.</summary>
		/// <param name="t1">the source tuple
		/// </param>
		public void  negate(Tuple3i t1)
		{
			this.x = - t1.x;
			this.y = - t1.y;
			this.z = - t1.z;
		}
		
		
		/// <summary> Negates the value of this tuple in place.</summary>
		public void  negate()
		{
			this.x = - this.x;
			this.y = - this.y;
			this.z = - this.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the scalar multiplication
		/// of tuple t1.
		/// </summary>
		/// <param name="s">the scalar value
		/// </param>
		/// <param name="t1">the source tuple
		/// </param>
		public void  scale(int s, Tuple3i t1)
		{
			this.x = s * t1.x;
			this.y = s * t1.y;
			this.z = s * t1.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the scalar multiplication
		/// of the scale factor with this.
		/// </summary>
		/// <param name="s">the scalar value
		/// </param>
		public void  scale(int s)
		{
			this.x *= s;
			this.y *= s;
			this.z *= s;
		}
		
		
		/// <summary> Sets the value of this tuple to the scalar multiplication
		/// of tuple t1 plus tuple t2 (this = s*t1 + t2).
		/// </summary>
		/// <param name="s">the scalar value
		/// </param>
		/// <param name="t1">the tuple to be multipled
		/// </param>
		/// <param name="t2">the tuple to be added
		/// </param>
		public void  scaleAdd(int s, Tuple3i t1, Tuple3i t2)
		{
			this.x = s * t1.x + t2.x;
			this.y = s * t1.y + t2.y;
			this.z = s * t1.z + t2.z;
		}
		
		
		/// <summary> Sets the value of this tuple to the scalar multiplication
		/// of itself and then adds tuple t1 (this = s*this + t1).
		/// </summary>
		/// <param name="s">the scalar value
		/// </param>
		/// <param name="t1">the tuple to be added
		/// </param>
		public void  scaleAdd(int s, Tuple3i t1)
		{
			this.x = s * this.x + t1.x;
			this.y = s * this.y + t1.y;
			this.z = s * this.z + t1.z;
		}
		
		
		/// <summary> Returns a string that contains the values of this Tuple3i.
		/// The form is (x,y,z).
		/// </summary>
		/// <returns> the String representation
		/// </returns>
		public override System.String ToString()
		{
			return "(" + this.x + ", " + this.y + ", " + this.z + ")";
		}
		
		
		/// <summary> Returns true if the Object t1 is of type Tuple3i and all of the
		/// data members of t1 are equal to the corresponding data members in
		/// this Tuple3i.
		/// </summary>
		/// <param name="t1"> the object with which the comparison is made
		/// </param>
		public  override bool Equals(System.Object t1)
		{
			try
			{
				Tuple3i t2 = (Tuple3i) t1;
				return (this.x == t2.x && this.y == t2.y && this.z == t2.z);
			}
			catch (System.NullReferenceException e2)
			{
				return false;
			}
			catch (System.InvalidCastException e1)
			{
				return false;
			}
		}
		
		
		/// <summary> Returns a hash code value based on the data values in this
		/// object.  Two different Tuple3i objects with identical data values
		/// (i.e., Tuple3i.equals returns true) will return the same hash
		/// code value.  Two objects with different data members may return the
		/// same hash value, although this is not likely.
		/// </summary>
		/// <returns> the integer hash code value
		/// </returns>
		public override int GetHashCode()
		{
			long bits = 1L;
			bits = 31L * bits + (long) x;
			bits = 31L * bits + (long) y;
			bits = 31L * bits + (long) z;
			return (int) (bits ^ (bits >> 32));
		}
		
		
		/// <summary>  Clamps the tuple parameter to the range [low, high] and
		/// places the values into this tuple.
		/// </summary>
		/// <param name="min">  the lowest value in the tuple after clamping
		/// </param>
		/// <param name="max"> the highest value in the tuple after clamping
		/// </param>
		/// <param name="t">  the source tuple, which will not be modified
		/// </param>
		public void  clamp(int min, int max, Tuple3i t)
		{
			if (t.x > max)
			{
				x = max;
			}
			else if (t.x < min)
			{
				x = min;
			}
			else
			{
				x = t.x;
			}
			
			if (t.y > max)
			{
				y = max;
			}
			else if (t.y < min)
			{
				y = min;
			}
			else
			{
				y = t.y;
			}
			
			if (t.z > max)
			{
				z = max;
			}
			else if (t.z < min)
			{
				z = min;
			}
			else
			{
				z = t.z;
			}
		}
		
		
		/// <summary>  Clamps the minimum value of the tuple parameter to the min
		/// parameter and places the values into this tuple.
		/// </summary>
		/// <param name="min">  the lowest value in the tuple after clamping
		/// </param>
		/// <param name="t">  the source tuple, which will not be modified
		/// </param>
		public void  clampMin(int min, Tuple3i t)
		{
			if (t.x < min)
			{
				x = min;
			}
			else
			{
				x = t.x;
			}
			
			if (t.y < min)
			{
				y = min;
			}
			else
			{
				y = t.y;
			}
			
			if (t.z < min)
			{
				z = min;
			}
			else
			{
				z = t.z;
			}
		}
		
		
		/// <summary>  Clamps the maximum value of the tuple parameter to the max
		/// parameter and places the values into this tuple.
		/// </summary>
		/// <param name="max">  the highest value in the tuple after clamping
		/// </param>
		/// <param name="t">  the source tuple, which will not be modified
		/// </param>
		public void  clampMax(int max, Tuple3i t)
		{
			if (t.x > max)
			{
				x = max;
			}
			else
			{
				x = t.x;
			}
			
			if (t.y > max)
			{
				y = max;
			}
			else
			{
				y = t.y;
			}
			
			if (t.z > max)
			{
				z = max;
			}
			else
			{
				z = t.z;
			}
		}
		
		
		/// <summary>  Sets each component of the tuple parameter to its absolute
		/// value and places the modified values into this tuple.
		/// </summary>
		/// <param name="t">  the source tuple, which will not be modified
		/// </param>
		public void  absolute(Tuple3i t)
		{
			x = System.Math.Abs(t.x);
			y = System.Math.Abs(t.y);
			z = System.Math.Abs(t.z);
		}
		
		
		/// <summary>  Clamps this tuple to the range [low, high].</summary>
		/// <param name="min"> the lowest value in this tuple after clamping
		/// </param>
		/// <param name="max"> the highest value in this tuple after clamping
		/// </param>
		public void  clamp(int min, int max)
		{
			if (x > max)
			{
				x = max;
			}
			else if (x < min)
			{
				x = min;
			}
			
			if (y > max)
			{
				y = max;
			}
			else if (y < min)
			{
				y = min;
			}
			
			if (z > max)
			{
				z = max;
			}
			else if (z < min)
			{
				z = min;
			}
		}
		
		
		/// <summary>  Clamps the minimum value of this tuple to the min parameter.</summary>
		/// <param name="min">  the lowest value in this tuple after clamping
		/// </param>
		public void  clampMin(int min)
		{
			if (x < min)
				x = min;
			
			if (y < min)
				y = min;
			
			if (z < min)
				z = min;
		}
		
		
		/// <summary>  Clamps the maximum value of this tuple to the max parameter.</summary>
		/// <param name="max">  the highest value in the tuple after clamping
		/// </param>
		public void  clampMax(int max)
		{
			if (x > max)
				x = max;
			
			if (y > max)
				y = max;
			
			if (z > max)
				z = max;
		}
		
		
		/// <summary>  Sets each component of this tuple to its absolute value.</summary>
		public void  absolute()
		{
			x = System.Math.Abs(x);
			y = System.Math.Abs(y);
			z = System.Math.Abs(z);
		}
		
		/// <summary> Creates a new object of the same class as this object.
		/// 
		/// </summary>
		/// <returns> a clone of this instance.
		/// </returns>
		/// <exception cref="OutOfMemoryError">if there is not enough memory.
		/// </exception>
		/// <seealso cref="java.lang.Cloneable">
		/// </seealso>
		/// <since> vecmath 1.3
		/// </since>
		public virtual System.Object Clone()
		{
			// Since there are no arrays we can just use Object.clone()
			try
			{
				return base.MemberwiseClone();
			}
			//UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				// this shouldn't happen, since we are Cloneable
				throw new System.ApplicationException();
			}
		}
	}
}