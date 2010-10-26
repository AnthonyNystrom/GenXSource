/* -----------------------------------------------
 * NuGenCalendarColorEventArgs.cs
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
	public class NuGenCalendarColorEventArgs : EventArgs
	{
		#region Class Data

		/// <summary>
		/// The color that has changed
		/// </summary>
		private NuGenCalendarColor m_color;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the MozItemEventArgs class with default settings
		/// </summary>
		public NuGenCalendarColorEventArgs()
		{
			m_color = 0;
		}


		/// <summary>
		/// Initializes a new instance of the MozItemEventArgs class with default settings
		/// </summary>
		public NuGenCalendarColorEventArgs(NuGenCalendarColor color)
		{
			this.m_color = color;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public NuGenCalendarColor Color
		{
			get
			{
				return this.m_color;
			}
		}

		#endregion
	}
}
