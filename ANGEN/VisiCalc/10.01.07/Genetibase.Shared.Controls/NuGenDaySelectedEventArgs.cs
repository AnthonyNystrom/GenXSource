/* -----------------------------------------------
 * NuGenDaySelectedEventArgs.cs
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
	public class NuGenDaySelectedEventArgs : EventArgs
	{
		private string[] m_days;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDaySelectedEventArgs"/> class.
		/// </summary>
		public NuGenDaySelectedEventArgs(string[] days)
		{
			this.m_days = days;
		}

		/// <summary>
		/// </summary>
		public string[] Days
		{
			get
			{
				return this.m_days;
			}
		}
	}
}
