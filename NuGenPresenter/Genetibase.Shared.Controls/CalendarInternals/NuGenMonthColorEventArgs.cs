/* -----------------------------------------------
 * NuGenMonthColorEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenMonthColorEventArgs : EventArgs
	{
		#region Class Data

		/// <summary>
		/// The property that has changed
		/// </summary>
		private NuGenMonthColor m_color;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMonthColorEventArgs"/> class.
		/// </summary>
		public NuGenMonthColorEventArgs()
		{
			m_color = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMonthColorEventArgs"/> class.
		/// </summary>
		public NuGenMonthColorEventArgs(NuGenMonthColor color)
		{
			this.m_color = color;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public NuGenMonthColor Color
		{
			get
			{
				return this.m_color;
			}
		}

		#endregion
	}
}
