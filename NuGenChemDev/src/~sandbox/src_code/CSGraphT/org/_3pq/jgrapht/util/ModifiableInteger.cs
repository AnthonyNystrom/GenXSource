/* ==========================================
* JGraphT : a free Java graph-theory library
* ==========================================
*
* Project Info:  http://jgrapht.sourceforge.net/
* Project Lead:  Barak Naveh (http://sourceforge.net/users/barak_naveh)
*
* (C) Copyright 2003-2004, by Barak Naveh and Contributors.
*
* This library is free software; you can redistribute it and/or modify it
* under the terms of the GNU Lesser General Public License as published by
* the Free Software Foundation; either version 2.1 of the License, or
* (at your option) any later version.
*
* This library is distributed in the hope that it will be useful, but
* WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
* or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
* License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this library; if not, write to the Free Software Foundation, Inc.,
* 59 Temple Place, Suite 330, Boston, MA 02111-1307, USA.
*/
/* ----------------------
* ModifiableInteger.java
* ----------------------
*
* (C) Copyright 2002-2004, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: ModifiableInteger.java,v 1.3 2005/07/17 05:40:49 perfecthash Exp $
*
* Changes
* -------
* 2004-05-27 : Initial version (BN);
*
*/
using System;
namespace org._3pq.jgrapht.util
{
	
	/// <summary> The <code>ModifiableInteger</code> class wraps a value of the primitive type
	/// <code>int</code> in an object, similarly to {@link java.lang.Integer}. An
	/// object of type <code>ModifiableInteger</code> contains a single field whose
	/// type is <code>int</code>.
	/// 
	/// <p>
	/// Unlike <code>java.lang.Integer</code>, the int value which the
	/// ModifiableInteger represents can be modified. It becomes useful when used
	/// together with the collection framework. For example, if you want to have a
	/// {@link java.util.List} of counters. You could use <code>Integer</code> but
	/// that would have became wasteful and inefficient if you frequently had to
	/// update the counters.
	/// </p>
	/// 
	/// <p>
	/// WARNING: Because instances of this class are mutable, great care must be
	/// exercised if used as keys of a {@link java.util.Map} or as values in a
	/// {@link java.util.Set} in a manner that affects equals comparisons while the
	/// instances are keys in the map (or values in the set). For more see
	/// documentation of <code>Map</code> and <code>Set</code>.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> May 27, 2004
	/// </since>
	//UPGRADE_TODO: Parent class 'java.lang.Number' was replaced with 'System.ValueType'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1077'"
	[Serializable]
	public class ModifiableInteger:System.ValueType, System.IComparable
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the value of this object, similarly to {@link #intValue()}. This
		/// getter is NOT redundant. It is used for serialization by
		/// java.beans.XMLEncoder.
		/// 
		/// </summary>
		/// <returns> the value.
		/// </returns>
		/// <summary> Sets a new value for this modifiable integer.
		/// 
		/// </summary>
		/// <param name="value">the new value to set.
		/// </param>
		virtual public int Value
		{
			get
			{
				return this.value_Renamed;
			}
			
			set
			{
				this.value_Renamed = value;
			}
			
		}
		private const long serialVersionUID = 3618698612851422261L;
		
		/// <summary>The int value represented by this <code>ModifiableInteger</code>. </summary>
		public int value_Renamed;
		
		/// <summary> <b>!!! DON'T USE - Use the {@link #ModifiableInteger(int)} constructor
		/// instead !!!</b>
		/// 
		/// <p>
		/// This constructor is for the use of java.beans.XMLDecoder
		/// deserialization. The constructor is marked as 'deprecated' to indicate
		/// to the programmer against using it by mistake.
		/// </p>
		/// 
		/// </summary>
		/// <deprecated> not really deprecated, just marked so to avoid mistaken use.
		/// </deprecated>
		public ModifiableInteger()
		{
		}
		
		
		/// <summary> Constructs a newly allocated <code>ModifiableInteger</code> object that
		/// represents the specified <code>int</code> value.
		/// 
		/// </summary>
		/// <param name="value">the value to be represented by the
		/// <code>ModifiableInteger</code> object.
		/// </param>
		public ModifiableInteger(int value_Renamed)
		{
			this.value_Renamed = value_Renamed;
		}
		
		
		/// <summary> Compares two <code>ModifiableInteger</code> objects numerically.
		/// 
		/// </summary>
		/// <param name="anotherInteger">the <code>ModifiableInteger</code> to be compared.
		/// 
		/// </param>
		/// <returns> the value <code>0</code> if this <code>ModifiableInteger</code>
		/// is equal to the argument <code>ModifiableInteger</code>; a
		/// value less than <code>0</code> if this
		/// <code>ModifiableInteger</code> is numerically less than the
		/// argument <code>ModifiableInteger</code>; and a value greater
		/// than <code>0</code> if this <code>ModifiableInteger</code> is
		/// numerically greater than the argument
		/// <code>ModifiableInteger</code> (signed comparison).
		/// </returns>
		public virtual int compareTo(ModifiableInteger anotherInteger)
		{
			int thisVal = this.value_Renamed;
			int anotherVal = anotherInteger.value_Renamed;
			
			return thisVal < anotherVal?- 1:(thisVal == anotherVal?0:1);
		}
		
		
		/// <summary> Compares this <code>ModifiableInteger</code> object to another object.
		/// If the object is an <code>ModifiableInteger</code>, this function
		/// behaves like <code>compareTo(Integer)</code>.  Otherwise, it throws a
		/// <code>ClassCastException</code> (as <code>ModifiableInteger</code>
		/// objects are only comparable to other <code>ModifiableInteger</code>
		/// objects).
		/// 
		/// </summary>
		/// <param name="o">the <code>Object</code> to be compared.
		/// 
		/// </param>
		/// <returns> the value <code>0</code> if the argument is a
		/// <code>ModifiableInteger</code> numerically equal to this
		/// <code>ModifiableInteger</code>; a value less than
		/// <code>0</code>  if the argument is a
		/// <code>ModifiableInteger</code> numerically  greater than this
		/// <code>ModifiableInteger</code>; and a value  greater than
		/// <code>0</code> if the argument is a
		/// <code>ModifiableInteger</code> numerically less than this
		/// <code>ModifiableInteger</code>.
		/// 
		/// </returns>
		/// <seealso cref="java.lang.Comparable.compareTo(java.lang.Object)">
		/// </seealso>
		public virtual int CompareTo(System.Object o)
		{
			return compareTo((ModifiableInteger) o);
		}
		
		
		/// <seealso cref="Number.doubleValue()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.lang.Number.doubleValue' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public double doubleValue()
		{
			return this.value_Renamed;
		}
		
		
		/// <summary> Compares this object to the specified object.  The result is
		/// <code>true</code> if and only if the argument is not <code>null</code>
		/// and is an <code>ModifiableInteger</code> object that contains the same
		/// <code>int</code> value as this object.
		/// 
		/// </summary>
		/// <param name="o">the object to compare with.
		/// 
		/// </param>
		/// <returns> <code>true</code> if the objects are the same;
		/// <code>false</code> otherwise.
		/// </returns>
		public  override bool Equals(System.Object o)
		{
			if (o is ModifiableInteger)
			{
				return this.value_Renamed == ((ModifiableInteger) o).value_Renamed;
			}
			
			return false;
		}
		
		
		/// <seealso cref="Number.floatValue()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.lang.Number.floatValue' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public float floatValue()
		{
			return this.value_Renamed;
		}
		
		
		/// <summary> Returns a hash code for this <code>ModifiableInteger</code>.
		/// 
		/// </summary>
		/// <returns> a hash code value for this object, equal to the  primitive
		/// <code>int</code> value represented by this
		/// <code>ModifiableInteger</code> object.
		/// </returns>
		public override int GetHashCode()
		{
			return this.value_Renamed;
		}
		
		
		/// <seealso cref="Number.intValue()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.lang.Number.intValue' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public int intValue()
		{
			return this.value_Renamed;
		}
		
		
		/// <seealso cref="Number.longValue()">
		/// </seealso>
		//UPGRADE_NOTE: The equivalent of method 'java.lang.Number.longValue' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public long longValue()
		{
			return this.value_Renamed;
		}
		
		
		/// <summary> Returns an <code>Integer</code> object representing this
		/// <code>ModifiableInteger</code>'s value.
		/// 
		/// </summary>
		/// <returns> an <code>Integer</code> representation of the value of this
		/// object.
		/// </returns>
		public virtual System.Int32 toInteger()
		{
			return (System.Int32) this.value_Renamed;
		}
		
		
		/// <summary> Returns a <code>String</code> object representing this
		/// <code>ModifiableInteger</code>'s value. The value is converted to
		/// signed decimal representation and returned as a string, exactly as if
		/// the integer value were given as an argument to the {@link
		/// java.lang.Integer#toString(int)} method.
		/// 
		/// </summary>
		/// <returns> a string representation of the value of this object in
		/// base&nbsp;10.
		/// </returns>
		public override System.String ToString()
		{
			return System.Convert.ToString(this.value_Renamed);
		}
	}
}