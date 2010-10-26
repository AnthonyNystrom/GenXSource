/* -----------------------------------------------
 * SIZE.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Specifies the width and height of a rectangle.
	/// </summary>
	public struct SIZE
	{
		#region Methods.Public.Overridden

		/*
		 * Equals
		 */

		/// <summary>
		/// Indicates whether this instance and a specified Object are equal.
		/// </summary>
		/// <param name="obj">Another Object to compare to.</param>
		/// <returns>
		/// true if obj and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj)
		{
			if (obj is SIZE)
			{
				SIZE s = (SIZE)obj;

				if (this.cx == s.cx)
				{
					return this.cy == s.cy;
				}
			}

			if (obj is Size)
			{

				Size s = (Size)obj;

				if (this.cx == s.Width)
				{
					return this.cy == s.Height;
				}
			}

			return false;
		}

		/*
		 * GetHashCode
		 */

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override Int32 GetHashCode()
		{
			return this.cx ^ this.cy;
		}

		/*
		 * ToString
		 */

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> containing a fully qualified type name.
		/// </returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{Width={0},Height={1}}", this.cx, this.cy);
		}

		#endregion

		#region Methods.Operators

		/// <summary>
		/// </summary>
		public static implicit operator SIZE(Size size)
		{
			return new SIZE(size.Width, size.Height);
		}

		/// <summary>
		/// </summary>
		public static implicit operator Size(SIZE size)
		{
			return new Size(size.cx, size.cy);
		}

		/// <summary>
		/// </summary>
		public static Boolean operator ==(SIZE left, SIZE right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// </summary>
		public static Boolean operator !=(SIZE left, SIZE right)
		{
			return !left.Equals(right);
		}

		#endregion

		/// <summary>
		/// Specifies the rectangle's width.
		/// </summary>
		public Int32 cx;

		/// <summary>
		/// Specifies the rectangle's height.
		/// </summary>
		public Int32 cy;

		/// <summary>
		/// Initializes a new instance of the <see cref="SIZE"/> structure.
		/// </summary>
		/// <param name="cx">Specifies the rectangle's width.</param>
		/// <param name="cy">Specifies the rectangle's height.</param>
		public SIZE(Int32 cx, Int32 cy)
		{
			this.cx = cx;
			this.cy = cy;
		}
	}
}
