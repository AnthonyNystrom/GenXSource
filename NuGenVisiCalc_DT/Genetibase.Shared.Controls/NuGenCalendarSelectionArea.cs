/* -----------------------------------------------
 * NuGenCalendarSelectionArea.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.CalendarInternals;

using System;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenCalendarSelectionArea
	{
		#region Private members
		
		int m_selBegin;
		int m_selEnd;
		DateTime m_selBeginDate;
		DateTime m_selEndDate;
		NuGenMonth m_month;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalendarSelectionArea"/> class.
		/// </summary>
		public NuGenCalendarSelectionArea()
		{
			m_selBegin = -1;
			m_selEnd = -1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalendarSelectionArea"/> class.
		/// </summary>
		public NuGenCalendarSelectionArea(int begin,int end)
		{
			m_selBegin = begin;
			m_selEnd = end;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalendarSelectionArea"/> class.
		/// </summary>
		public NuGenCalendarSelectionArea(int begin, int end, NuGenMonth month)
		{
			m_selBegin = begin;
			m_selEnd = end;
			m_month = month;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalendarSelectionArea"/> class.
		/// </summary>
		public NuGenCalendarSelectionArea(NuGenMonth month)
		{
			m_selBegin = -1;
			m_selEnd = -1;
			m_month = month;
		}
		
		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public DateTime BeginDate
		{
			get
			{
				return m_selBeginDate;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime EndDate
		{
			get
			{
				return m_selEndDate;
			}
		}

		/// <summary>
		/// </summary>
		public int Begin
		{
			get
			{
				return m_selBegin;
			}
			set
			{
			    m_selBegin = value;		
				if (m_selBegin!=-1)
					m_selBeginDate = m_month.m_days[m_selBegin].Date; 
			}
		}

		/// <summary>
		/// </summary>
		public int End
		{
			get
			{
				
				return m_selEnd;
			}
			set
			{
				m_selEnd = value;		
				if (m_selEnd!=-1)
					m_selEndDate = m_month.m_days[m_selEnd].Date;
			}
		}

		/// <summary>
		/// </summary>
		internal NuGenMonth Month
		{
			get
			{
				return m_month;
			}
			set	
			{
				m_month = value;
			}
		}
	
		#endregion

		#region Methods

		/// <summary>
		/// </summary>
		public new string ToString()
		{
			if ((m_selBegin==-1) || (m_selEnd==-1))
				return "Empty";
			else
				return m_selBegin.ToString()+":"+m_selEnd.ToString();
		}

		/// <summary>
		/// </summary>
		public string[] DaysInSelection()
		{
			string[] days = new string[0];
			return days;
		}

		#endregion
	}
}
