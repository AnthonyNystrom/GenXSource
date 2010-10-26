/* -----------------------------------------------
 * Span.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Windows.Controls.Data.Text
{
	/// <summary>
	/// </summary>
	public class Span
	{
		#region Properties

		/*
		 * End
		 */

		/// <summary>
		/// </summary>
		public Int32 End
		{
			get
			{
				return this.Start + this.Length;
			}
		}

		/*
		 * IsEmpty
		 */

		/// <summary>
		/// </summary>
		public Boolean IsEmpty
		{
			get
			{
				return this.Length == 0;
			}
		}

		/*
		 * Length
		 */

		private Int32 _length;

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="value"/> is less than zero or greater than 2147483646.
		/// </exception>
		public virtual Int32 Length
		{
			get
			{
				return _length;
			}
			protected set
			{
				if ((value < 0) || (value > 0x7ffffffe))
				{
					throw new ArgumentOutOfRangeException("value");
				}

				_length = value;
			}
		}

		/*
		 * Start
		 */

		private Int32 _start;

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="value"/> is less than zero.
		/// </exception>
		public virtual Int32 Start
		{
			get
			{
				return _start;
			}
			protected set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				_start = value;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * Contains
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="span"/> is <see langword="null"/>.
		/// </exception>
		public Boolean Contains(Span span)
		{
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}

			if (span.Start >= this.Start)
			{
				return (span.End <= this.End);
			}

			return false;
		}

		/// <summary>
		/// </summary>
		public Boolean Contains(Int32 position)
		{
			if (position >= this.Start)
			{
				return (position < this.End);
			}

			return false;
		}

		/*
		 * Intersection
		 */

		/// <summary>
		/// </summary>
		public Span Intersection(Span span)
		{
			if (this.Intersects(span))
			{
				Int32 start = Math.Max(this.Start, span.Start);
				Int32 num2 = Math.Min(this.End, span.End);
				return new Span(start, num2 - start);
			}

			return null;
		}

		/*
		 * Intersects
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="span"/> is <see langword="null"/>.</para>
		/// </exception>
		public virtual Boolean Intersects(Span span)
		{
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}

			if (this == span)
			{
				return true;
			}

			if (span.Start < this.End)
			{
				return (span.End > this.Start);
			}

			return false;
		}

		#endregion

		#region Methods.Public.Overridden

		/*
		 * Equals
		 */

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj)
		{
			Span span = obj as Span;

			if ((span != null) && (span.Start == this.Start))
			{
				return (span.Length == this.Length);
			}

			return false;
		}

		/*
		 * GetHashCode
		 */

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override Int32 GetHashCode()
		{
			return (this.Length * this.Start);
		}

		/*
		 * ToString
		 */

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "[{0}..{1})", new Object[] { _start, _start + _length });
		}

		#endregion

		#region Operators

		/// <summary>
		/// </summary>
		public static Boolean operator ==(Span span1, Span span2)
		{
			if (!Object.ReferenceEquals(span1, null))
			{
				return span1.Equals(span2);
			}
			return Object.ReferenceEquals(span1, span2);
		}

		/// <summary>
		/// </summary>
		public static Boolean operator !=(Span span1, Span span2)
		{
			return (span1 != span2);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Span"/> class.
		/// </summary>
		protected Span()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Span"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="span"/> is <see langword="null"/>.
		/// </exception>
		public Span(Span span)
		{
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}

			_start = span._start;
			_length = span._length;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Span"/> class.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para>
		///		<paramref name="start"/> is less than zero.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="length"/> is less than zero or greater than 2147483646.
		/// </para>
		/// </exception>
		public Span(Int32 start, Int32 length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start");
			}

			if ((length < 0) || (length > 0x7ffffffe))
			{
				throw new ArgumentOutOfRangeException("length");
			}

			_start = start;
			_length = length;
		}
	}
}
