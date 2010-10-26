/* -----------------------------------------------
 * TextSpan.cs
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
	public class TextSpan : Span
	{
		#region Properties.Public

		/// <summary>
		/// </summary>
		public String Text
		{
			get
			{
				return this.TextBuffer.GetText(this.Start, this.Length);
			}
		}

		/// <summary>
		/// </summary>
		public TextBuffer TextBuffer
		{
			get;
			private set;
		}

		/// <summary>
		/// </summary>
		public SpanTrackingMode TrackingMode
		{
			get;
			private set;
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * Length
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		/// <exception cref="ArgumentOutOfRangeException">
		/// 	<paramref name="value"/> is less than zero or greater than 2147483646.
		/// </exception>
		public override Int32 Length
		{
			get
			{
				this.Update();
				return base.Length;
			}
		}

		/*
		 * Start
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		/// <exception cref="ArgumentOutOfRangeException">
		/// 	<paramref name="value"/> is less than zero.
		/// </exception>
		public override Int32 Start
		{
			get
			{
				this.Update();
				return base.Start;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		public Boolean Intersects(TextSpan span)
		{
			if (span.TextBuffer == this.TextBuffer)
			{
				return base.Intersects(span);
			}

			return false;
		}

		#endregion

		#region Methods.Private

		private void Initialize(TextBuffer textBuffer, SpanTrackingMode trackingMode)
		{
			if (textBuffer == null)
			{
				throw new ArgumentNullException("textBuffer");
			}

			if (base.Start > textBuffer.Length)
			{
				throw new ArgumentOutOfRangeException("start");
			}

			if ((base.Start + base.Length) > textBuffer.Length)
			{
				throw new ArgumentOutOfRangeException("length");
			}

			this.TextBuffer = textBuffer;
			_version = textBuffer.Version;
			this.TrackingMode = trackingMode;
		}

		private void Update()
		{
			Int32 position = base.Start + base.Length;

			while (_version != this.TextBuffer.Version)
			{
				if (this.TrackingMode == SpanTrackingMode.EdgeExclusive)
				{
					base.Start = UpdatePositiveTrackingPoint(_version.Change, base.Start);
					position = UpdateNegativeTrackingPoint(_version.Change, position);
				}
				else
				{
					base.Start = UpdateNegativeTrackingPoint(_version.Change, base.Start);
					position = UpdatePositiveTrackingPoint(_version.Change, position);
				}

				_version = _version.Next;
			}

			Int32 num2 = position - base.Start;
			base.Length = (num2 < 0) ? 0 : num2;
		}

		#endregion

		#region Methods.Internal.Static

		internal static Int32 UpdateNegativeTrackingPoint(TextChange change, Int32 position)
		{
			if (change.Position < position)
			{
				if (change.OldEnd < position)
				{
					position += change.Delta;
					return position;
				}

				position = change.Position;
			}

			return position;
		}

		internal static Int32 UpdatePositiveTrackingPoint(TextChange change, Int32 position)
		{
			if (change.Position <= position)
			{
				if (change.OldEnd <= position)
				{
					position += change.Delta;
					return position;
				}

				position = change.NewEnd;
			}

			return position;
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
			TextSpan span = obj as TextSpan;

			if ((span != null) && (((span.TextBuffer == this.TextBuffer) && (span.Start == this.Start)) && (span.Length == this.Length)))
			{
				return (span.TrackingMode == this.TrackingMode);
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
			return String.Format(CultureInfo.InvariantCulture, "{2}[{0}..{1})", new Object[] { base.Start, base.End, (_version != this.TextBuffer.Version) ? "*" : "" });
		}

		#endregion

		private ITextVersion _version;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="span"/> is <see langword="null"/>.</exception>
		public TextSpan(TextSpan span)
			: base(span)
		{
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}
			this.Initialize(span.TextBuffer, span.TrackingMode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="span"/> is <see langword="null"/>.</exception>
		public TextSpan(TextBuffer textBuffer, Span span)
			: this(textBuffer, span, SpanTrackingMode.EdgeExclusive)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="span"/> is <see langword="null"/>.</exception>
		public TextSpan(TextBuffer textBuffer, Span span, SpanTrackingMode trackingMode)
			: base(span)
		{
			this.Initialize(textBuffer, trackingMode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextSpan"/> class.
		/// </summary>
		public TextSpan(TextBuffer textBuffer, Int32 start, Int32 length)
			: this(textBuffer, start, length, SpanTrackingMode.EdgeExclusive)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextSpan"/> class.
		/// </summary>
		public TextSpan(TextBuffer textBuffer, Int32 start, Int32 length, SpanTrackingMode trackingMode)
			: base(start, length)
		{
			this.Initialize(textBuffer, trackingMode);
		}
	}
}
