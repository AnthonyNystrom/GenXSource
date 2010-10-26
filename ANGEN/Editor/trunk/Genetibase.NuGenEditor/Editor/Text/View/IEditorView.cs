/* -----------------------------------------------
 * IEditorView.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor.Text.View
{
	/// <summary>
	/// </summary>
	public interface IEditorView : ITextView, IPropertyOwner
	{
		/// <summary>
		/// </summary>
		Brush Background
		{
			get;
			set;
		}
		/// <summary>
		/// </summary>
		FrameworkElement VisualElement
		{
			get;
		}
	}
}
