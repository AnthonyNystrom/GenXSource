/* -----------------------------------------------
 * POINT.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Globalization;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines the x- and y- coordinates of a point. 
	/// </summary>
	public struct POINT
	{
		#region Declarations.Fields

		/// <summary>
		/// Specifies the point's x-coordinate.
		/// </summary>
		public int x;

		/// <summary>
		/// Specifies the point's y-coordinate.
		/// </summary>
		public int y;

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
			if (obj is POINT)
			{
				POINT p = (POINT)obj;

				if (this.x == p.x)
				{
					return this.y == p.y;
				}
			}

			if (obj is Point)
			{
				Point p = (Point)obj;

				if (this.x == p.X)
				{
					return this.y == p.Y;
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
			return this.x ^ this.y;
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
			return string.Format("{X={0},Y={1}}", this.x, this.y);
		}

		#endregion

		#region Methods.Operators

		/// <summary>
		/// </summary>
		public static implicit operator POINT(Point p)
		{
			return new POINT(p.X, p.Y);
		}

		/// <summary>
		/// </summary>
		public static implicit operator Point(POINT p)
		{
			return new Point(p.x, p.y);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="POINT"/> structure.
		/// </summary>
		public POINT(int px, int py)
		{
			this.x = px;
			this.y = py;
		}

		#endregion
	}
}
