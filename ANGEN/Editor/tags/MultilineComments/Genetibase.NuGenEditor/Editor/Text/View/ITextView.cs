/* -----------------------------------------------
 * ITextView.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public interface ITextView : IPropertyOwner
	{
		/// <summary>
		/// </summary>
		event EventHandler<TextViewLayoutChangedEventArgs> LayoutChanged;
		/// <summary>
		/// </summary>
		event EventHandler<TextViewInputEventArgs> TextViewInputEvent;
		/// <summary>
		/// </summary>
		event EventHandler ViewportHeightChanged;
		/// <summary>
		/// </summary>
		event EventHandler ViewportHorizontalOffsetChanged;
		/// <summary>
		/// </summary>
		event EventHandler ViewportVerticalOffsetChanged;
		/// <summary>
		/// </summary>
		event EventHandler ViewportWidthChanged;

		/// <summary>
		/// </summary>
		void DisplayLine(Int32 lineNumber, Double lineOffset, ViewRelativePosition relativeTo);
		/// <summary>
		/// </summary>
		Span GetTextElementSpan(Int32 position);
		/// <summary>
		/// </summary>
		void Invalidate();
		/// <summary>
		/// </summary>
		Boolean ScrollViewportHorizontally(Double pixelsToScroll);
		/// <summary>
		/// </summary>
		Boolean ScrollViewportVertically(Double pixelsToScroll);
		/// <summary>
		/// </summary>
		Boolean ScrollViewportVertically(Int32 visualLinesToScroll);

		/// <summary>
		/// </summary>
		ITextCaret Caret
		{
			get;
		}
		/// <summary>
		/// </summary>
		ITextSelection Selection
		{
			get;
		}
		/// <summary>
		/// </summary>
		Int32 TabSize
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		TextBuffer TextBuffer
		{
			get;
		}
		/// <summary>
		/// </summary>
		IList<ITextLine> TextLines
		{
			get;
		}
		/// <summary>
		/// </summary>
		Double TotalContentHeight
		{
			get;
		}
		/// <summary>
		/// </summary>
		Double TotalContentWidth
		{
			get;
		}
		/// <summary>
		/// </summary>
		Double ViewportHeight
		{
			get;
		}
		/// <summary>
		/// </summary>
		Double ViewportHorizontalOffset
		{
			get;
		}
		/// <summary>
		/// </summary>
		Double ViewportMargin
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		Double ViewportVerticalOffset
		{
			get;
		}
		/// <summary>
		/// </summary>
		Double ViewportWidth
		{
			get;
		}
	}
}
