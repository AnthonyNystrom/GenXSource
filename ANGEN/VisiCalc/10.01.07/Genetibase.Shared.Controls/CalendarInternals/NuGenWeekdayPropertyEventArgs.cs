/* -----------------------------------------------
 * NuGenWeekdayPropertyEventArgs.cs
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
	public class NuGenWeekdayPropertyEventArgs : EventArgs
	{
		#region Class Data

		/// <summary>
		/// The property that has changed
		/// </summary>
		private NuGenWeekdayProperty m_property;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWeekdayPropertyEventArgs"/> class.
		/// </summary>
		public NuGenWeekdayPropertyEventArgs()
		{
			m_property = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWeekdayPropertyEventArgs"/> class.
		/// </summary>
		public NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty property)
		{
			this.m_property = property;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public NuGenWeekdayProperty Property
		{
			get
			{
				return this.m_property;
			}
		}

		#endregion
	}
}
