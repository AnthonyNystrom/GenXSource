/* -----------------------------------------------
 * ITextCaret.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public interface ITextCaret
	{
		/// <summary>
		/// </summary>
		event EventHandler<CaretPositionChangedEventArgs> PositionChanged;

		/// <summary>
		/// </summary>
		Double HorizontalOffset
		{
			get;
		}

		/// <summary>
		/// </summary>
		Boolean OverwriteMode
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		ICaretPosition Position
		{
			get;
		}

		/// <summary>
		/// </summary>
		Double PreferredHorizontalPosition
		{
			get;
		}

		/// <summary>
		/// </summary>
		Double PreferredVerticalPosition
		{
			get;
		}

		/// <summary>
		/// </summary>
		Double VerticalOffset
		{
			get;
		}

		/// <summary>
		/// </summary>
		void CaptureHorizontalPosition();

		/// <summary>
		/// </summary>
		void CaptureVerticalPosition();

		/// <summary>
		/// </summary>
		void EnsureVisible();

		/// <summary>
		/// </summary>
		void EnsureVisible(Double verticalPadding, ViewRelativePosition relativeTo);

		/// <summary>
		/// </summary>
		ICaretPosition MoveTo(Int32 characterIndex);

		/// <summary>
		/// </summary>
		ICaretPosition MoveTo(Int32 characterIndex, CaretPlacement caretPlacement);

		/// <summary>
		/// </summary>
		ICaretPosition MoveToNextCaretPosition();

		/// <summary>
		/// </summary>
		ICaretPosition MoveToPreviousCaretPosition();
	}
}
