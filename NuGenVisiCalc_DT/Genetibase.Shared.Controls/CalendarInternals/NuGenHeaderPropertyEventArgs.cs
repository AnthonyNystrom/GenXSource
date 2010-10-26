/* -----------------------------------------------
 * NuGenHeaderPropertyEventArgs.cs
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
	public class NuGenHeaderPropertyEventArgs : EventArgs
	{
		/// <summary>
		/// </summary>
		public NuGenHeaderProperty Property
		{
			get
			{
				return this.m_property;
			}
		}

		private NuGenHeaderProperty m_property;

		/// <summary>
		/// </summary>
		public NuGenHeaderPropertyEventArgs()
		{
			m_property = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHeaderPropertyEventArgs"/> class.
		/// </summary>
		public NuGenHeaderPropertyEventArgs(NuGenHeaderProperty property)
		{
			this.m_property = property;
		}
	}
}
