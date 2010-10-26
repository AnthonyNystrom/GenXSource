/* -----------------------------------------------
 * NuGenWeeknumberClickEventArgs.cs
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
	public class NuGenWeeknumberClickEventArgs : EventArgs
	{
		#region Class Data

		private int m_week;
		private MouseButtons m_button;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>
		public NuGenWeeknumberClickEventArgs()
		{
			m_button = MouseButtons.Left;
		}

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>
		public NuGenWeeknumberClickEventArgs(int week, MouseButtons button)
		{
			this.m_week = week;
			this.m_button = button;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public int Week
		{
			get
			{
				return this.m_week;
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
