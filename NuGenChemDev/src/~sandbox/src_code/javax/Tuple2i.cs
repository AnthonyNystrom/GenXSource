/*
* $RCSfile: Tuple2i.java,v $
*
* Copyright (c) 2006 Sun Microsystems, Inc. All rights reserved.
*
* Use is subject to license terms.
*
* $Revision: 1.4 $
* $Date: 2006/09/29 18:05:22 $
* $State: Exp $
*/
using System;
namespace javax.vecmath
{
	
	/// <summary> A 2-element tuple represented by signed integer x,y
	/// coordinates.
	/// 
	/// </summary>
	/// <since> vecmath 1.4
	/// </since>
	[Serializable]
	public abstract class Tuple2i : System.ICloneable
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Get the <i>x</i> coordinate.
		/// 
		/// </summary>
		/// <returns> the x coordinate.
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
		
		internal const long serialVersionUID = - 3555701650170169638L;
		
		/// <summary> The x coordinate.</summary>
		public int x;
		
		/// <summary> The y coordinate.</summary>
		public int y;
		
		
		/// <summary> Constructs and initializes a Tuple2i from the specified
		/// x and y coordinates.
		/// </summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		public Tuple2i(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		
		
		/// <summary> Constructs and initializes a Tuple2i from the array of length 2.</summary>
		/// <param name="t">the array of length 2 containing x and y in order.
		/// </param>
		public Tuple2i(int[] t)
		{
			this.x = t[0];
			this.y = t[1];
		}
		
		
		/// <summary> Constructs and initializes a Tuple2i from the specified Tuple2i.</summary>
		/// <param name="t1">the Tuple2i containing the initialization x and y
		/// data.
		/// </param>
		public Tuple2i(Tuple2i t1)
		{
			this.x = t1.x;
			this.y = t1.y;
		}
		
		
		/// <summary> Constructs and initializes a Tuple2i to (0,0).</summary>
		public Tuple2i()
		{
			this.x = 0;
			this.y = 0;
		}
		
		
		/// <summary> Sets the value of this tuple to the specified x and y
		/// coordinates.
		/// </summary>
		/// <param name="x">the x coordinate
		/// </param>
		/// <param name="y">the y coordinate
		/// </param>
		public void  set_Renamed(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		
		
		/// <summary> Sets the value of this tuple to the specified coordinates in the
		/// array of length 2.
		/// </summary>
		/// <param name="t">the array of length 2 containing x and y in order.
		/// </param>
		public void  set_Renamed(int[] t)
		{
			this.x = t[0];
			this.y = t[1];
		}
		
		
		/// <summary> Sets the value of this tuple to the value of tuple t1.</summary>
		/// <param name="t1">the tuple to be copied
		/// </param>
		public void  set_Renamed(Tuple2i t1)
		{
			this.x = t1.x;
			this.y = t1.y;
		}
		
		
		/// <summary> Copies the values of this tuple into the array t.</summary>
		/// <param name="t">is the array
		/// </param>
		public void  get_Renamed(int[] t)
		{
			t[0] = this.x;
			t[1] = this.y;
		}
		
		
		/// <summary> Copies the values of this tuple into the tuple t.</summary>
		/// <param name="t">is the target tuple
		/// </param>
		public void  get_Renamed(Tuple2i t)
		{
			t.x = this.x;
			t.y = this.y;
		}
		
		
		/// <summary> Sets the value of this tuple to the sum of tuples t1 and t2.</summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="t2">the second tuple
		/// </param>
		public void  add(Tuple2i t1, Tuple2i t2)
		{
			this.x = t1.x + t2.x;
			this.y = t1.y + t2.y;
		}
		
		
		/// <summary> Sets the value of this tuple to the sum of itself and t1.</summary>
		/// <param name="t1">the other tuple
		/// </param>
		public void  add(Tuple2i t1)
		{
			this.x += t1.x;
			this.y += t1.y;
		}
		
		
		/// <summary> Sets the value of this tuple to the difference
		/// of tuples t1 and t2 (this = t1 - t2).
		/// </summary>
		/// <param name="t1">the first tuple
		/// </param>
		/// <param name="t2">the second tuple
		/// </param>
		public void  sub(Tuple2i t1, Tuple2i t2)
		{
			this.x = t1.x - t2.x;
			this.y = t1.y - t2.y;
		}
		
		
		/// <summary> Sets the value of this tuple to the difference
		/// of itself and t1 (this = this - t1).
		/// </summary>
		/// <param name="t1">the other tuple
		/// </param>
		public void  sub(Tuple2i t1)
		{
			this.x -= t1.x;
			this.y -= t1.y;
		}
		
		
		/// <summary> Sets the value of this tuple to the negation of tuple t1.</summary>
		/// <param name="t1">the source tuple
		/// </param>
		public void  negate(Tuple2i t1)
		{
			this.x = - t1.x;
			this.y = - t1.y;
		}
		
		
		/// <summary> Negates the value of this tuple in place.</summary>
		public void  negate()
		{
			this.x = - this.x;
			this.y = - this.y;
		}
		
		
		/// <summary> Sets the value of this tuple to the scalar multiplication
		/// of tuple t1.
		/// </summary>
		/// <param name="s">the scalar value
		/// </param>
		/// <param name="t1">the source tuple
		/// </param>
		public void  scale(int s, Tuple2i t1)
		{
			this.x = s * t1.x;
			this.y = s * t1.y;
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
		public void  scaleAdd(int s, Tuple2i t1, Tuple2i t2)
		{
			this.x = s * t1.x + t2.x;
			this.y = s * t1.y + t2.y;
		}
		
		
		/// <summary> Sets the value of this tuple to the scalar multiplication
		/// of itself and then adds tuple t1 (this = s*this + t1).
		/// </summary>
		/// <param name="s">the scalar value
		/// </param>
		/// <param name="t1">the tuple to be added
		/// </param>
		public void  scaleAdd(int s, Tuple2i t1)
		{
			this.x = s * this.x + t1.x;
			this.y = s * this.y + t1.y;
		}
		
		
		/// <summary> Returns a string that contains the values of this Tuple2i.
		/// The form is (x,y).
		/// </summary>
		/// <returns> the String representation
		/// </returns>
		public override System.String ToString()
		{
			return "(" + this.x + ", " + this.y + ")";
		}
		
		
		/// <summary> Returns true if the Object t1 is of type Tuple2i and all of the
		/// data members of t1 are equal to the corresponding data members in
		/// this Tuple2i.
		/// </summary>
		/// <param name="t1"> the object with which the comparison is made
		/// </param>
		public  override bool Equals(System.Object t1)
		{
			try
			{
				Tuple2i t2 = (Tuple2i) t1;
				return (this.x == t2.x && this.y == t2.y);
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
		/// object.  Two different Tuple2i objects with identical data values
		/// (i.e., Tuple2i.equals returns true) will return the same hash
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
		public void  clamp(int min, int max, Tuple2i t)
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
		}
		
		
		/// <summary>  Clamps the minimum value of the tuple parameter to the min
		/// parameter and places the values into this tuple.
		/// </summary>
		/// <param name="min">  the lowest value in the tuple after clamping
		/// </param>
		/// <param name="t">  the source tuple, which will not be modified
		/// </param>
		public void  clampMin(int min, Tuple2i t)
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
		}
		
		
		/// <summary>  Clamps the maximum value of the tuple parameter to the max
		/// parameter and places the values into this tuple.
		/// </summary>
		/// <param name="max">  the highest value in the tuple after clamping
		/// </param>
		/// <param name="t">  the source tuple, which will not be modified
		/// </param>
		public void  clampMax(int max, Tuple2i t)
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
		}
		
		
		/// <summary>  Sets each component of the tuple parameter to its absolute
		/// value and places the modified values into this tuple.
		/// </summary>
		/// <param name="t">  the source tuple, which will not be modified
		/// </param>
		public void  absolute(Tuple2i t)
		{
			x = System.Math.Abs(t.x);
			y = System.Math.Abs(t.y);
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
		}
		
		
		/// <summary>  Sets each component of this tuple to its absolute value.</summary>
		public void  absolute()
		{
			x = System.Math.Abs(x);
			y = System.Math.Abs(y);
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