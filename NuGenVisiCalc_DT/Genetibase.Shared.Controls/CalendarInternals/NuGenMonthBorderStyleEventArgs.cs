/* -----------------------------------------------
 * NuGenMonthBorderStyleEventArgs.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenMonthBorderStyleEventArgs : EventArgs
	{
		#region Class Data

		/// <summary>
		/// The property that has changed
		/// </summary>
		private NuGenMonthBorderStyle m_borderStyle;

		#endregion

		#region Constructor

		/// <summary>
		/// </summary>
		public NuGenMonthBorderStyleEventArgs()
		{
			m_borderStyle = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMonthBorderStyleEventArgs"/> class.
		/// </summary>
		public NuGenMonthBorderStyleEventArgs(NuGenMonthBorderStyle style)
		{
			this.m_borderStyle = style;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public NuGenMonthBorderStyle BorderStyle
		{
			get
			{
				return this.m_borderStyle;
			}
		}

		#endregion
	}
}
