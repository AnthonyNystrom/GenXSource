/* -----------------------------------------------
 * NuGenDayStateChangedEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenDayStateChangedEventArgs : EventArgs
	{
		/// <summary>
		/// </summary>
		public string Date
		{
			get
			{
				return this.m_date;
			}
		}

		/// <summary>
		/// </summary>
		public NuGenDayState OldState
		{
			get
			{
				return this.m_oldState;
			}
		}

		/// <summary>
		/// </summary>
		public NuGenDayState NewState
		{
			get
			{
				return this.m_newState;
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

		private string m_date;
		private NuGenDayState m_oldState;
		private NuGenDayState m_newState;
		private bool m_cancel;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDayStateChangedEventArgs"/> class.
		/// </summary>
		public NuGenDayStateChangedEventArgs(string date, NuGenDayState oldState, NuGenDayState newState)
		{
			m_date = date;
			m_newState = newState;
			m_oldState = oldState;
			m_cancel = false;
		}
	}
}
