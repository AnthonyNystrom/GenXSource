/* -----------------------------------------------
 * VersionedTextSpan.cs
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
	public class VersionedTextSpan
	{
		private TextBuffer _buffer;
		private Int32 _length;
		private Int32 _start;
		private SpanTrackingMode _trackingMode;
		private ITextVersion _version;

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionedTextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="span"/> is <see langword="null"/>.</para>
		/// </exception>
		public VersionedTextSpan(TextBuffer buffer, Span span)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}
			this.Construct(buffer, buffer.Version, span.Start, span.Length, SpanTrackingMode.EdgeExclusive);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionedTextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="span"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="version"/> is <see langword="null"/>.</para>
		/// </exception>
		public VersionedTextSpan(TextBuffer buffer, ITextVersion version, Span span)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			if (version == null)
			{
				throw new ArgumentNullException("version");
			}

			if (span == null)
			{
				throw new ArgumentNullException("span");
			}

			this.Construct(buffer, buffer.Version, span.Start, span.Length, SpanTrackingMode.EdgeExclusive);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionedTextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="span"/> is <see langword="null"/>.</para>
		/// </exception>
		public VersionedTextSpan(TextBuffer buffer, Span span, SpanTrackingMode trackingMode)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			if (span == null)
			{
				throw new ArgumentNullException("span");
			}

			this.Construct(buffer, buffer.Version, span.Start, span.Length, trackingMode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionedTextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="buffer"/> is <see langword="null"/>.</para></exception>
		public VersionedTextSpan(TextBuffer buffer, Int32 start, Int32 length)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			this.Construct(buffer, buffer.Version, start, length, SpanTrackingMode.EdgeExclusive);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionedTextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="span"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="version"/> is <see langword="null"/>.</para>
		/// </exception>
		public VersionedTextSpan(TextBuffer buffer, ITextVersion version, Span span, SpanTrackingMode trackingMode)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			if (span == null)
			{
				throw new ArgumentNullException("span");
			}
			this.Construct(buffer, buffer.Version, span.Start, span.Length, trackingMode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionedTextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="version"/> is <see langword="null"/>.</para>
		/// </exception>
		public VersionedTextSpan(TextBuffer buffer, ITextVersion version, Int32 start, Int32 length)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}

			if (version == null)
			{
				throw new ArgumentNullException("version");
			}

			this.Construct(buffer, version, start, length, SpanTrackingMode.EdgeExclusive);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionedTextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// </exception>
		public VersionedTextSpan(TextBuffer buffer, Int32 start, Int32 length, SpanTrackingMode trackingMode)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.Construct(buffer, buffer.Version, start, length, trackingMode);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VersionedTextSpan"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="buffer"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="version"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <para>The specified <paramref name="version"/> does not belong to the specified <paramref name="buffer"/>.</para>
		/// </exception>
		public VersionedTextSpan(TextBuffer buffer, ITextVersion version, Int32 start, Int32 length, SpanTrackingMode trackingMode)
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
				throw new ArgumentException("The specified TextVersion does not belong to the specified TextBuffer.");
			}
			this.Construct(buffer, version, start, length, trackingMode);
		}

		private void Construct(TextBuffer buffer, ITextVersion version, Int32 start, Int32 length, SpanTrackingMode trackingMode)
		{
			Int32 lengthOfVersion = buffer.GetLengthOfVersion(version);
			if ((start < 0) || (start > lengthOfVersion))
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if ((length < 0) || ((start + length) > lengthOfVersion))
			{
				throw new ArgumentOutOfRangeException("length");
			}
			_buffer = buffer;
			_version = version;
			_start = start;
			_length = length;
			_trackingMode = trackingMode;
		}

		/// <summary>
		/// </summary>
		public Span Span()
		{
			return this.Span(this.TextBuffer.Version);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetVersion"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		The specified <paramref name="version"/> does not belong to the specified <paramref name="buffer"/>.
		/// </para>
		/// </exception>
		public Span Span(ITextVersion targetVersion)
		{
			if (targetVersion == null)
			{
				throw new ArgumentNullException("targetVersion");
			}

			if (!_buffer.VersionBelongsToBuffer(targetVersion))
			{
				throw new ArgumentException("The specified TextVersion does not belong to the specified TextBuffer");
			}

			ITextVersion next = _version;
			Int32 num = TextBuffer.CompareVersions(next, targetVersion);
			Int32 position = _start;
			Int32 num3 = _start + _length;
			if (num < 0)
			{
				while (next != targetVersion)
				{
					if (_trackingMode == SpanTrackingMode.EdgeExclusive)
					{
						position = TextSpan.UpdatePositiveTrackingPoint(next.Change, position);
						num3 = TextSpan.UpdateNegativeTrackingPoint(next.Change, num3);
					}
					else
					{
						position = TextSpan.UpdateNegativeTrackingPoint(next.Change, position);
						num3 = TextSpan.UpdatePositiveTrackingPoint(next.Change, num3);
					}
					next = next.Next;
				}
			}
			else if (num > 0)
			{
				Stack<TextChange> stack = new Stack<TextChange>(1);
				for (ITextVersion version2 = targetVersion; version2 != next; version2 = version2.Next)
				{
					stack.Push(version2.Change);
				}
				while (stack.Count > 0)
				{
					TextChange change = stack.Pop();
					if (_trackingMode == SpanTrackingMode.EdgeExclusive)
					{
						if (change.Position <= position)
						{
							position -= change.Delta;
						}
						if (change.Position < num3)
						{
							num3 -= change.Delta;
						}
					}
					else
					{
						if (change.Position < position)
						{
							position -= change.Delta;
						}
						if (change.Position <= num3)
						{
							num3 -= change.Delta;
						}
					}
				}
			}
			if (num3 < position)
			{
				num3 = position;
			}
			return new Span(position, num3 - position);
		}

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

		/// <summary>
		/// </summary>
		public TextBuffer TextBuffer
		{
			get
			{
				return _buffer;
			}
		}

		/// <summary>
		/// </summary>
		public SpanTrackingMode TrackingMode
		{
			get
			{
				return _trackingMode;
			}
		}

		/// <summary>
		/// </summary>
		public ITextVersion Version
		{
			get
			{
				return _version;
			}
		}
	}
}
