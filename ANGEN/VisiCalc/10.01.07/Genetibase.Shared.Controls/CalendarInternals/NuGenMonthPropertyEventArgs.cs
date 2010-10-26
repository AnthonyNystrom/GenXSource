/* -----------------------------------------------
 * NuGenMonthPropertyEventArgs.cs
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
	public class NuGenMonthPropertyEventArgs : EventArgs
	{
		#region Class Data

		/// <summary>
		/// The property that has changed
		/// </summary>
		private NuGenMonthProperty m_property;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMonthPropertyEventArgs"/> class.
		/// </summary>
		public NuGenMonthPropertyEventArgs()
		{
			m_property = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMonthPropertyEventArgs"/> class.
		/// </summary>
		public NuGenMonthPropertyEventArgs(NuGenMonthProperty property)
		{
			this.m_property = property;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public NuGenMonthProperty Property
		{
			get
			{
				return this.m_property;
			}
		}

		#endregion
	}
}
