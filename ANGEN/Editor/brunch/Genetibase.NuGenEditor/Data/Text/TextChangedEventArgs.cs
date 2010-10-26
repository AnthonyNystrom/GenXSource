/* -----------------------------------------------
 * TextChangedEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Data.Text
{
	/// <summary>
	/// </summary>
	public class TextChangedEventArgs : EventArgs
	{
		/// <summary>
		/// </summary>
		public ITextVersion PriorVersion
		{
			get;
			private set;
		}

		/// <summary>
		/// </summary>
		public ITextEdit Source
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextChangedEventArgs"/> class.
		/// </summary>
		public TextChangedEventArgs(ITextEdit source, ITextVersion version)
		{
			this.Source = source;
			this.PriorVersion = version;
		}
	}
}
