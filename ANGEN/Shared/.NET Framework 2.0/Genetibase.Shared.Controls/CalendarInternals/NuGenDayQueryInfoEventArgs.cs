/* -----------------------------------------------
 * NuGenDayQueryInfoEventArgs.cs
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
	public class NuGenDayQueryInfoEventArgs : EventArgs
	{
		#region Class Data

		private NuGenDateItem m_info;
		private bool m_ownerDraw;
		private NuGenDayState m_state;
		private DateTime m_date;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the DayRenderEventArgs class with default settings
		/// </summary>
		public NuGenDayQueryInfoEventArgs()
		{
		}

		/// <summary>
		/// Initializes a new instance of the DayRenderEventArgs class with default settings
		/// </summary>
		public NuGenDayQueryInfoEventArgs(NuGenDateItem info, DateTime date, NuGenDayState state)
		{
			this.m_info = info;
			this.m_date = date;
			this.m_state = state;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public NuGenDateItem Info
		{
			get
			{
				return m_info;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime Date
		{
			get
			{
				return m_date;
			}
		}

		/// <summary>
		/// </summary>
		public NuGenDayState State
		{
			get
			{
				return m_state;
			}
		}

		/// <summary>
		/// </summary>
		public bool OwnerDraw
		{
			set
			{
				m_ownerDraw = value;
			}
			get
			{
				return m_ownerDraw;
			}
		}

		#endregion
	}
}
