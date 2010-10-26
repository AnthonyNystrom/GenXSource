/* -----------------------------------------------
 * AlignEventArgs.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	internal sealed class AlignEventArgs : EventArgs
	{
		private ContentAlignment _align;

		public ContentAlignment Align
		{
			get
			{
				return _align;
			}
		}

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>
		public AlignEventArgs()
			: this(ContentAlignment.MiddleCenter)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AlignEventArgs"/> class.
		/// </summary>
		public AlignEventArgs(ContentAlignment align)
		{
			_align = align;
		}
	}
}
