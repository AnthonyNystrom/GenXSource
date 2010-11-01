/* -----------------------------------------------
 * NuGenControlPaint.Text.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Reflection;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * CreateStringFormat
		 */

		/// <summary>
		/// Builds <see cref="T:System.Drawing.StringFormat"/> for the specified <see cref="T:System.Windows.Forms.Control"/>.
		/// </summary>
		/// <param name="ctrl">Specifies the <see cref="T:System.Windows.Forms.Control"/> to build
		/// <see cref="T:System.Drawing.StringFormat"/> for.</param>
		/// <param name="textAlign">Specifies text alignment on the drawing surface.</param>
		/// <param name="showEllipsis">Specifies the value indicating whether to show ellipsis if the whole text
		/// cannot be displayed.</param>
		/// <param name="useMnemonic">Specifies the value indicating whether an ampersand (&amp;) is included in the
		/// text.</param>
		/// <returns></returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="ctrl"/> is <see langword="null"/>.</exception>
		public static StringFormat CreateStringFormat(
			Control ctrl,
			ContentAlignment textAlign,
			bool showEllipsis,
			bool useMnemonic
			)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			StringFormat stringFormat = NuGenControlPaint.ContentAlignmentToStringFormat(textAlign);

			if (ctrl.RightToLeft == RightToLeft.Yes)
			{
				stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			}

			if (showEllipsis)
			{
				stringFormat.Trimming = StringTrimming.EllipsisCharacter;
				stringFormat.FormatFlags |= StringFormatFlags.LineLimit;
			}

			if (!useMnemonic)
			{
				stringFormat.HotkeyPrefix = HotkeyPrefix.None;
			}
			else if ((bool)NuGenInvoker.GetProperty(ctrl, "ShowKeyboardCues"))
			{
				stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
			}
			else
			{
				stringFormat.HotkeyPrefix = HotkeyPrefix.Hide;
			}

			if (ctrl.AutoSize)
			{
				stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			}

			return stringFormat;
		}

		/*
		 * CreateTextFormat
		 */

		/// <summary>
		/// Builds <see cref="T:System.Windows.Forms.TextFormatFlags"/> for the specified
		/// <see cref="T:System.Windows.Forms.Control"/>.
		/// </summary>
		/// <param name="ctrl">Specifies the <see cref="T:System.Windows.Forms.Control"/> to build
		/// <see cref="T:System.Windows.Forms.TextFormatFlags"/> for.</param>
		/// <param name="textAlign">Specifies text alignment on the drawing surface.</param>
		/// <param name="showEllipsis">Specifies the value indicating whether to show ellipsis if the whole text
		/// cannot be displayed.</param>
		/// <param name="useMnemonic">Specifies the value indicating whether an ampersand (&amp;) is included in the
		/// text.</param>
		/// <returns></returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="ctrl"/> is <see langword="null"/>.</exception>
		public static TextFormatFlags CreateTextFormat(
			Control ctrl,
			ContentAlignment textAlign,
			bool showEllipsis,
			bool useMnemonic
			)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			TextFormatFlags formatFlags = TextFormatFlags.Default;

			/* RightToLeft */

			if (ctrl.RightToLeft == RightToLeft.Yes)
			{
				formatFlags |= TextFormatFlags.RightToLeft;
			}

			/* Alignment */

			if ((textAlign & NuGenControlPaint.AnyRight) != (ContentAlignment)0)
			{
				formatFlags |= TextFormatFlags.Right;
			}
			else if ((textAlign & NuGenControlPaint.AnyCenter) != (ContentAlignment)0)
			{
				formatFlags |= TextFormatFlags.HorizontalCenter;
			}
			else
			{
				formatFlags |= TextFormatFlags.Left;
			}

			/* Line alignment */

			if ((textAlign & NuGenControlPaint.AnyBottom) != (ContentAlignment)0)
			{
				formatFlags |= TextFormatFlags.Bottom;
			}
			else if ((textAlign & NuGenControlPaint.AnyMiddle) != (ContentAlignment)0)
			{
				formatFlags |= TextFormatFlags.VerticalCenter;
			}
			else
			{
				formatFlags |= TextFormatFlags.Top;
			}

			/* Other */

			if (showEllipsis)
			{
				formatFlags |= TextFormatFlags.EndEllipsis;
			}

			if (ctrl.AutoSize)
			{
				formatFlags |= TextFormatFlags.SingleLine;
			}

			return formatFlags;
		}

		/*
		 * StringFormatForAlignment
		 */

		/// <summary>
		/// Converts <see cref="T:System.Drawing.ContentAlignment"/> to <see cref="T:System.Drawing.StringFormat"/>.
		/// </summary>
		public static StringFormat ContentAlignmentToStringFormat(ContentAlignment alignmentToConvert)
		{
			StringFormat stringFormat = new StringFormat();

			stringFormat.Alignment = NuGenControlPaint.TranslateAlignment(alignmentToConvert);
			stringFormat.LineAlignment = NuGenControlPaint.TranslateLineAlignment(alignmentToConvert);

			return stringFormat;
		}

		/*
		 * TranslateAlignment
		 */

		/// <summary>
		/// Converts the specified <see cref="T:System.Drawing.ContentAlignment"/> to
		/// <see cref="T:System.Drawing.StringAlignment"/>.
		/// </summary>
		public static StringAlignment TranslateAlignment(ContentAlignment alignmentToConvert)
		{
			if ((alignmentToConvert & NuGenControlPaint.AnyRight) != (ContentAlignment)0)
			{
				return StringAlignment.Far;
			}

			if ((alignmentToConvert & NuGenControlPaint.AnyCenter) != ((ContentAlignment)0))
			{
				return StringAlignment.Center;
			}

			return StringAlignment.Near;
		}

		/*
		 * TranslateLineAlignment
		 */

		/// <summary>
		/// Converts the specified <see cref="T:System.Drawing.ContentAlignment"/> that represents
		/// line alignment to <see cref="T:System.Drawing.StringAlignment"/>.
		/// </summary>
		public static StringAlignment TranslateLineAlignment(ContentAlignment alignmentToConvert)
		{
			if ((alignmentToConvert & NuGenControlPaint.AnyBottom) != ((ContentAlignment)0))
			{
				return StringAlignment.Far;
			}

			if ((alignmentToConvert & NuGenControlPaint.AnyMiddle) != ((ContentAlignment)0))
			{
				return StringAlignment.Center;
			}

			return StringAlignment.Near;
		}
	}
}
