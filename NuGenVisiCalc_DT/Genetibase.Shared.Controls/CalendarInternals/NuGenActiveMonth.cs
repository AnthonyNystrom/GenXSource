/* -----------------------------------------------
 * NuGenActiveMonth.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	[TypeConverter(typeof(NuGenActiveMonthConverter))]
	public class NuGenActiveMonth
	{
		private int m_month;
		private int m_year;
		private NuGenCalendar m_calendar;
		private bool m_raiseEvent = true;

		internal event EventHandler<NuGenMonthChangedEventArgs> MonthChanged;
		internal event EventHandler<NuGenBeforeMonthChangedEventArgs> BeforeMonthChanged;

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public NuGenCalendar Calendar
		{
			get
			{
				return m_calendar;
			}
		}

		[Browsable(false)]
		internal bool RaiseEvent
		{
			set
			{
				m_raiseEvent = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenActiveMonth"/> class.
		/// </summary>
		public NuGenActiveMonth(NuGenCalendar calendar)
		{
			m_calendar = calendar;
			m_year = DateTime.Now.Year;
			m_month = DateTime.Now.Month;
		}

		/// <summary>
		/// </summary>
		[Description("Current month")]
		[RefreshProperties(RefreshProperties.All)]
		[TypeConverter(typeof(ActiveMonthMonthConverter))]
		public int Month
		{
			get
			{
				return m_month;
			}
			set
			{
				if (m_month != value)
				{
					NuGenBeforeMonthChangedEventArgs args = new NuGenBeforeMonthChangedEventArgs(m_year, value, m_year, m_month);
					if ((BeforeMonthChanged != null) && (m_raiseEvent))
						BeforeMonthChanged(this, args);
					if (!args.Cancel)
					{
						m_month = value;
						ChangeMonth();
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Current calendar year.")]
		[RefreshProperties(RefreshProperties.All)]
		[TypeConverter(typeof(ActiveMonthYearConverter))]
		public int Year
		{
			get
			{
				return m_year;
			}
			set
			{
				if (m_year != value)
				{
					NuGenBeforeMonthChangedEventArgs args = new NuGenBeforeMonthChangedEventArgs(value, m_month, m_year, m_month);
					if ((BeforeMonthChanged != null) && (m_raiseEvent))
						BeforeMonthChanged(this, args);
					if (!args.Cancel)
					{
						m_year = value;
						ChangeMonth();
					}
				}
			}
		}

		private void ChangeMonth()
		{
			m_calendar.Month.SelectedMonth = DateTime.Parse(m_year.ToString() + "-" + m_month.ToString() + "-01");
			m_calendar.Setup();
			if ((MonthChanged != null) && (m_raiseEvent))
				MonthChanged(this, new NuGenMonthChangedEventArgs(m_month, m_year));
			m_calendar.Invalidate();
		}

	}
}
