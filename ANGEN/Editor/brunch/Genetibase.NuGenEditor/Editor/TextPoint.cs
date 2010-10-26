/* -----------------------------------------------
 * TextPoint.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor
{
	/// <summary>
	/// </summary>
	public class TextPoint
	{
		#region Properties.Public

		/// <summary>
		/// </summary>
		public Int32 Line
		{
			get
			{
				this.Update();
				return _textBuffer.GetLineNumberFromPosition(_position);
			}
		}

		/// <summary>
		/// </summary>
		public Int32 LineOffset
		{
			get
			{
				this.Update();
				return _textBuffer.GetLineOffsetFromPosition(_position);
			}
		}

		private Int32 _position;

		/// <summary>
		/// </summary>
		public Int32 Position
		{
			get
			{
				this.Update();
				return _position;
			}
		}

		private TextBuffer _textBuffer;

		/// <summary>
		/// </summary>
		public TextBuffer TextBuffer
		{
			get
			{
				return _textBuffer;
			}
		}

		private TrackingMode _trackingMode;

		/// <summary>
		/// </summary>
		public TrackingMode TrackingMode
		{
			get
			{
				return _trackingMode;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		public override Boolean Equals(Object obj)
		{
			TextPoint point = obj as TextPoint;
			if (point == null)
			{
				return false;
			}
			return (point.Position == this.Position);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override String ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "({0},{1})", new Object[] { this.Line, this.LineOffset });
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		public Int32 MapPositionTo(ITextVersion version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			if (!_textBuffer.VersionBelongsToBuffer(version))
			{
				throw new ArgumentException("The specified version doesn't belong to this text buffer.");
			}
			if (TextBuffer.CompareVersions(version, _version) < 0)
			{
				return this.GetPositionBackwardsInTime(version);
			}
			return this.GetPositionForwardsInTime(version);
		}

		#endregion

		#region Methods.Public.Static

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="p1"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="p2"/> is <see langword="null"/>.</para>
		/// </exception>
		public static Int32 Compare(TextPoint p1, TextPoint p2)
		{
			if (p1 == null)
			{
				throw new ArgumentNullException("p1");
			}
			if (p2 == null)
			{
				throw new ArgumentNullException("p2");
			}
			return (p1.Position - p2.Position);
		}


		#endregion

		#region Methods.Private

		private Int32 GetPositionBackwardsInTime(ITextVersion version)
		{
			Stack<TextChange> stack = new Stack<TextChange>();
			while (version != _version)
			{
				stack.Push(version.Change);
				version = version.Next;
			}
			Int32 num = _position;
			while (stack.Count > 0)
			{
				TextChange change = stack.Pop();
				if (change.Position < num)
				{
					num -= change.Delta;
				}
			}
			return num;
		}

		private Int32 GetPositionForwardsInTime(ITextVersion version)
		{
			while (_version != version)
			{
				if ((_version.Change.Position < _position) || ((_trackingMode == TrackingMode.Positive) && (_version.Change.Position == _position)))
				{
					if (_version.Change.OldEnd < _position)
					{
						_position += _version.Change.Delta;
					}
					else if (_trackingMode == TrackingMode.Negative)
					{
						_position = _version.Change.Position;
					}
					else
					{
						_position = _version.Change.NewEnd;
					}
				}
				_version = _version.Next;
			}
			return _position;
		}

		private void Initialize(TextBuffer buffer, ITextVersion bufferVersion, Int32 position, TrackingMode trackingMode)
		{
			_version = bufferVersion;
			_textBuffer = buffer;
			_position = position;
			_trackingMode = trackingMode;
		}

		private void Update()
		{
			this.GetPositionForwardsInTime(_textBuffer.Version);
		}

		#endregion

		#region Operators

		/// <summary>
		/// </summary>
		public static Boolean operator ==(TextPoint p1, TextPoint p2)
		{
			if (Object.ReferenceEquals(p1, p2))
			{
				return true;
			}
			if (!Object.Equals(p1, null) && !Object.Equals(p2, null))
			{
				return (p1.Position == p2.Position);
			}
			return false;
		}

		/// <summary>
		/// </summary>
		public static Boolean operator >(TextPoint p1, TextPoint p2)
		{
			if (p1 == null)
			{
				throw new ArgumentNullException("p1");
			}
			if (p2 == null)
			{
				throw new ArgumentNullException("p2");
			}
			return (p1.Position > p2.Position);
		}

		/// <summary>
		/// </summary>
		public static Boolean operator >=(TextPoint p1, TextPoint p2)
		{
			if (p1 == null)
			{
				throw new ArgumentNullException("p1");
			}
			if (p2 == null)
			{
				throw new ArgumentNullException("p2");
			}
			return (p1.Position >= p2.Position);
		}

		/// <summary>
		/// </summary>
		public static implicit operator Int32(TextPoint point)
		{
			if (point == null)
			{
				throw new ArgumentNullException("point");
			}
			return point.Position;
		}

		/// <summary>
		/// </summary>
		public static Boolean operator !=(TextPoint p1, TextPoint p2)
		{
			if (Object.ReferenceEquals(p1, p2))
			{
				return false;
			}
			if (!Object.Equals(p1, null) && !Object.Equals(p2, null))
			{
				return (p1.Position != p2.Position);
			}
			return true;
		}

		/// <summary>
		/// </summary>
		public static Boolean operator <(TextPoint p1, TextPoint p2)
		{
			if (p1 == null)
			{
				throw new ArgumentNullException("p1");
			}
			if (p2 == null)
			{
				throw new ArgumentNullException("p2");
			}
			return (p1.Position < p2.Position);
		}

		/// <summary>
		/// </summary>
		public static Boolean operator <=(TextPoint p1, TextPoint p2)
		{
			if (p1 == null)
			{
				throw new ArgumentNullException("p1");
			}
			if (p2 == null)
			{
				throw new ArgumentNullException("p2");
			}
			return (p1.Position <= p2.Position);
		}

		#endregion

		private ITextVersion _version;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextPoint"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="position"/> is less than zero or greater than buffer.Length.</para>
		/// </exception>
		public TextPoint(TextBuffer buffer, Int32 position)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if ((position < 0) || (position > buffer.Length))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this.Initialize(buffer, buffer.Version, position, TrackingMode.Positive);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextPoint"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="position"/> is less than zero or greater than buffer.Length.</para>
		/// </exception>
		public TextPoint(TextBuffer buffer, Int32 position, TrackingMode trackingMode)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if ((position < 0) || (position > buffer.Length))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this.Initialize(buffer, buffer.Version, position, trackingMode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextPoint"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="line"/> is less than zero or greater than or equal to buffer.LineCount.</para>
		/// -or-
		/// <para><paramref name="offset"/> is less than zero or greater than buffer.GetLengthOfLineFromLineNumber(line).</para>
		/// </exception>
		public TextPoint(TextBuffer buffer, Int32 line, Int32 offset)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if ((line < 0) || (line >= buffer.LineCount))
			{
				throw new ArgumentOutOfRangeException("line");
			}
			if ((offset < 0) || (offset > buffer.GetLengthOfLineFromLineNumber(line)))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			Int32 position = buffer.GetStartOfLineFromLineNumber(line) + offset;
			this.Initialize(buffer, buffer.Version, position, TrackingMode.Positive);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextPoint"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="version"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="position"/> is less than zero or greater than buffer.GetLengthOfVersion(version).</para>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The specified <paramref name="version"/> doesn't belong to the specified <paramref name="buffer"/>.
		/// </exception>
		public TextPoint(TextBuffer buffer, ITextVersion version, Int32 position, TrackingMode trackingMode)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			if (!buffer.VersionBelongsToBuffer(version))
			{
				throw new ArgumentException("The specified version doesn't belong to the specified buffer.");
			}
			if ((position < 0) || (position > buffer.GetLengthOfVersion(version)))
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this.Initialize(buffer, version, position, trackingMode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextPoint"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="version"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="line"/> is less than zero or greater than or equal to buffer.GetLengthOfLineFromLineNumber(line).</para>
		/// -or-
		/// <para><paramref name="offset"/> is less than zero or greater than buffer.GetLengthOfLineFromLineNumber(line).</para>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The specified <paramref name="version"/> doesn't belong to the specified <paramref name="buffer"/>.
		/// </exception>
		public TextPoint(TextBuffer buffer, Int32 line, Int32 offset, TrackingMode trackingMode)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if ((line < 0) || (line >= buffer.LineCount))
			{
				throw new ArgumentOutOfRangeException("line");
			}
			if ((offset < 0) || (offset > buffer.GetLengthOfLineFromLineNumber(line)))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			Int32 position = buffer.GetStartOfLineFromLineNumber(line) + offset;
			this.Initialize(buffer, buffer.Version, position, trackingMode);
		}
	}
}
