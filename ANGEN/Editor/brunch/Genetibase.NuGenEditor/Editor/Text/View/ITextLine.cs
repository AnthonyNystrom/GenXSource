/* -----------------------------------------------
 * ITextLine.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;
using System.Collections.ObjectModel;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public interface ITextLine
	{
		/// <summary>
		/// </summary>
		TextBounds GetCharacterBounds(Int32 characterIndex);
		
		/// <summary>
		/// </summary>
		ReadOnlyCollection<TextBounds> GetTextBounds(Span span);

		/// <summary>
		/// </summary>
		ICaretPosition MoveCaretToLocation(Double horizontalDistance);

		/// <summary>
		/// </summary>
		Double Baseline
		{
			get;
		}
		
		/// <summary>
		/// </summary>
		Double Extent
		{
			get;
		}
		
		/// <summary>
		/// </summary>
		Double Height
		{
			get;
		}
		
		/// <summary>
		/// </summary>
		Double HorizontalOffset
		{
			get;
		}

		/// <summary>
		/// </summary>
		Span LineSpan
		{
			get;
		}
		
		/// <summary>
		/// </summary>
		Int32 NewlineLength
		{
			get;
		}
		
		/// <summary>
		/// </summary>
		Double OverhangAfter
		{
			get;
		}
		
		/// <summary>
		/// </summary>
		Double OverhangLeading
		{
			get;
		}
		
		/// <summary>
		/// </summary>
		Double OverhangTrailing
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
		Double Width
		{
			get;
		}
	}
}
