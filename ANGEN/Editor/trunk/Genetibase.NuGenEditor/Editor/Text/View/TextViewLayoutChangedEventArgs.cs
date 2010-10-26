/* -----------------------------------------------
 * TextViewLayoutChangedEventArgs.cs
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
	public class TextViewLayoutChangedEventArgs : EventArgs
	{
		private Span _changeSpan;

		/// <summary>
		/// </summary>
		public Span ChangeSpan
		{
			get
			{
				return _changeSpan;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextViewLayoutChangedEventArgs"/> class.
		/// </summary>
		public TextViewLayoutChangedEventArgs(Span changeSpan)
		{
			_changeSpan = changeSpan;
		}
	}
}
