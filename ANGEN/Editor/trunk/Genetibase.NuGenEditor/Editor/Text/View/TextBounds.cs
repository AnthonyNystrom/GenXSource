/* -----------------------------------------------
 * TextBounds.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public struct TextBounds
	{
		#region Properties

		private Double _left;

		/// <summary>
		/// </summary>
		public Double Left
		{
			get
			{
				return _left;
			}
		}

		private Double _top;

		/// <summary>
		/// </summary>
		public Double Top
		{
			get
			{
				return _top;
			}
		}

		private Double _width;

		/// <summary>
		/// </summary>
		public Double Width
		{
			get
			{
				return _width;
			}
		}

		private Double _height;

		/// <summary>
		/// </summary>
		public Double Height
		{
			get
			{
				return _height;
			}
		}

		/// <summary>
		/// </summary>
		public Double Right
		{
			get
			{
				return (_left + _width);
			}
		}

		/// <summary>
		/// </summary>
		public Double Bottom
		{
			get
			{
				return (_top + _height);
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> containing a fully qualified type name.
		/// </returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "[{0},{1},{2},{3}]", new Object[] { this.Left, this.Top, this.Right, this.Bottom });
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override Int32 GetHashCode()
		{
			return (Int32)((this.Left + this.Top) * (this.Right + this.Bottom));
		}

		/// <summary>
		/// Indicates whether this instance and a specified Object are equal.
		/// </summary>
		/// <param name="obj">Another Object to compare to.</param>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj)
		{
			if ((obj != null) && (obj is TextBounds))
			{
				TextBounds bounds = (TextBounds)obj;
				if (((bounds.Left == this.Left) && (bounds.Width == this.Width)) && (bounds.Top == this.Top))
				{
					return (bounds.Height == this.Height);
				}
			}
			return false;
		}

		#endregion

		#region Operators

		/// <summary>
		/// </summary>
		public static Boolean operator ==(TextBounds bounds1, TextBounds bounds2)
		{
			if (((bounds1.Left == bounds2.Left) && (bounds1.Top == bounds2.Top)) && (bounds1.Width == bounds2.Width))
			{
				return (bounds1.Height == bounds2.Height);
			}
			return false;
		}

		/// <summary>
		/// </summary>
		public static Boolean operator !=(TextBounds bounds1, TextBounds bounds2)
		{
			return (bounds1 != bounds2);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="TextBounds"/> class.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para>
		///		<paramref name="left"/> is not a number.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="top"/> is not a number.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="width"/> is not a number.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="height"/> is not a number.
		/// </para>
		/// </exception>
		public TextBounds(Double left, Double top, Double width, Double height)
		{
			if (Double.IsNaN(left))
			{
				throw new ArgumentOutOfRangeException("left");
			}

			if (Double.IsNaN(top))
			{
				throw new ArgumentOutOfRangeException("top");
			}
			
			if (Double.IsNaN(width))
			{
				throw new ArgumentOutOfRangeException("width");
			}
			
			if (Double.IsNaN(height))
			{
				throw new ArgumentOutOfRangeException("height");
			}
			
			_left = left;
			_top = top;
			_width = width;
			_height = height;
		}
	}
}
