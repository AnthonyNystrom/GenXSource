/* -----------------------------------------------
 * NuGenDayEventArgs.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenDayEventArgs : EventArgs
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

		string m_date;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDayEventArgs"/> class.
		/// </summary>
		public NuGenDayEventArgs()
		{
			m_date = DateTime.Today.ToShortDateString();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDayEventArgs"/> class.
		/// </summary>
		public NuGenDayEventArgs(string date)
		{
			this.m_date = date;
		}
	}
}
