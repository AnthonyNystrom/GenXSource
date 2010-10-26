/* -----------------------------------------------
 * SpaceNegotiation.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Genetibase.Windows.Controls.Editor
{
	/// <summary>
	/// </summary>
	public struct SpaceNegotiation
	{
		#region Properties.Public

		private TextPoint _textPosition;

		/// <summary>
		/// </summary>
		public TextPoint TextPosition
		{
			get
			{
				return _textPosition;
			}
		}

		private Size _size;

		/// <summary>
		/// </summary>
		public Size Size
		{
			get
			{
				return _size;
			}
		}

		#endregion

		#region Methods.Public.Overridden

		/// <summary>
		/// Indicates whether this instance and a specified Object are equal.
		/// </summary>
		/// <param name="obj">Another Object to compare to.</param>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj)
		{
			if ((obj != null) && (obj is SpaceNegotiation))
			{
				SpaceNegotiation negotiation = (SpaceNegotiation)obj;
				if (negotiation.Size == this.Size)
				{
					return (negotiation.TextPosition == this.TextPosition);
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override Int32 GetHashCode()
		{
			return this.TextPosition.GetHashCode();
		}

		#endregion

		#region Operators

		/// <summary>
		/// </summary>
		public static Boolean operator ==(SpaceNegotiation negotiation1, SpaceNegotiation negotiation2)
		{
			return negotiation1.Equals(negotiation2);
		}

		/// <summary>
		/// </summary>
		public static Boolean operator !=(SpaceNegotiation negotiation1, SpaceNegotiation negotiation2)
		{
			return !negotiation1.Equals(negotiation2);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="SpaceNegotiation"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="textPosition"/> is <see langword="null"/>.
		/// </exception>
		public SpaceNegotiation(TextPoint textPosition, Size size)
		{
			if (textPosition == null)
			{
				throw new ArgumentNullException("textPosition");
			}

			_textPosition = textPosition;
			_size = size;
		}
	}
}
