/* -----------------------------------------------
 * NuGenFooterPropertyEventArgs.cs
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
	public class NuGenFooterPropertyEventArgs : EventArgs
	{
		/// <summary>
		/// </summary>
		public NuGenFooterProperty Property
		{
			get
			{
				return this.m_property;
			}
		}

		private NuGenFooterProperty m_property;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFooterPropertyEventArgs"/> class.
		/// </summary>
		public NuGenFooterPropertyEventArgs()
		{
			m_property = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFooterPropertyEventArgs"/> class.
		/// </summary>
		public NuGenFooterPropertyEventArgs(NuGenFooterProperty property)
		{
			this.m_property = property;
		}
	}
}
