/* -----------------------------------------------
 * NuGenMonthChangedEventArgs.cs
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
	public class NuGenMonthChangedEventArgs : EventArgs
	{
		#region Class Data

		private int m_month;
		private int m_year;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>
		public NuGenMonthChangedEventArgs()
		{

		}

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>
		public NuGenMonthChangedEventArgs(int month, int year)
		{
			this.m_month = month;
			this.m_year = year;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public int Month
		{
			get
			{
				return m_month;
			}
		}

		/// <summary>
		/// </summary>
		public int Year
		{
			get
			{
				return m_year;
			}
		}

		#endregion
	}
}
