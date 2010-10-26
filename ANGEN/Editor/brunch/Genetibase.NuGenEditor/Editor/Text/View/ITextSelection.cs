/* -----------------------------------------------
 * ITextSelection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public interface ITextSelection : IEnumerable<TextSpan>, IEnumerable
	{
		/// <summary>
		/// </summary>
		event EventHandler SelectionChanged;

		/// <summary>
		/// </summary>
		void Clear();

		/// <summary>
		/// </summary>
		void Select(TextSpan span);

		/// <summary>
		/// </summary>
		TextSpan ActiveSpan
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		Int32 Count
		{
			get;
		}

		/// <summary>
		/// </summary>
		Boolean IsActiveSpanReversed
		{
			get;
			set;
		}

		/// <summary>
		/// </summary>
		Boolean IsEmpty
		{
			get;
		}

		/// <summary>
		/// </summary>
		TextSpan this[Int32 index]
		{
			get;
		}
	}
}
