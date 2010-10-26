/* -----------------------------------------------
 * NuGenWeekdayClickEventArgs.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenWeekdayClickEventArgs : EventArgs
	{
		#region Class Data

		private int m_day;
		private MouseButtons m_button;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>
		public NuGenWeekdayClickEventArgs()
		{
			m_button = MouseButtons.Left;
		}

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>
		public NuGenWeekdayClickEventArgs(int day, MouseButtons button)
		{
			this.m_day = day;
			this.m_button = button;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public int Day
		{
			get
			{
				return this.m_day;
			}
		}

		/// <summary>
		/// </summary>
		public MouseButtons Button
		{
			get
			{
				return this.m_button;
			}
		}

		#endregion
	}
}
