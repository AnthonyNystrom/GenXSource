/* -----------------------------------------------
 * SIZE.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Specifies the width and height of a rectangle.
	/// </summary>
	public struct SIZE
	{
		#region Declarations.Fields

		/// <summary>
		/// Specifies the rectangle's width.
		/// </summary>
		public int cx;

		/// <summary>
		/// Specifies the rectangle's height.
		/// </summary>
		public int cy;

		#endregion

		#region Methods.Public.Overridden

		/*
		 * Equals
		 */

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>
		/// true if obj and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
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
		public override int GetHashCode()
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
		public override string ToString()
		{
			return string.Format("{Width={0},Height={1}}", this.cx, this.cy);
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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SIZE"/> structure.
		/// </summary>
		/// <param name="cx">Specifies the rectangle's width.</param>
		/// <param name="cy">Specifies the rectangle's height.</param>
		public SIZE(int cx, int cy)
		{
			this.cx = cx;
			this.cy = cy;
		}

		#endregion
	}
}
