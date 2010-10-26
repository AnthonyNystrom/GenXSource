/* -----------------------------------------------
 * NuGenBeforeMonthChangedEventArgs.cs
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
	public class NuGenBeforeMonthChangedEventArgs : EventArgs
	{
		#region Class Data

		private bool m_cancel;
		private int m_oldYear;
		private int m_oldMonth;
		private int m_newYear;
		private int m_newMonth;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>

		public NuGenBeforeMonthChangedEventArgs(int newYear, int newMonth, int oldYear, int oldMonth)
		{
			m_newYear = newYear;
			m_newMonth = newMonth;
			m_oldYear = oldYear;
			m_oldMonth = oldMonth;
			m_cancel = false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public int OldYear
		{
			get
			{
				return m_oldYear;
			}
		}

		/// <summary>
		/// </summary>
		public int OldMonth
		{
			get
			{
				return m_oldMonth;
			}
		}

		/// <summary>
		/// </summary>
		public int NewYear
		{
			get
			{
				return m_newYear;
			}
			set
			{
				m_newYear = value;
			}
		}

		/// <summary>
		/// </summary>
		public int NewMonth
		{
			get
			{
				return m_newMonth;
			}
			set
			{
				m_newMonth = value;
			}
		}

		/// <summary>
		/// </summary>
		public bool Cancel
		{
			get
			{
				return m_cancel;
			}
			set
			{
				m_cancel = value;
			}
		}

		#endregion
	}
}
