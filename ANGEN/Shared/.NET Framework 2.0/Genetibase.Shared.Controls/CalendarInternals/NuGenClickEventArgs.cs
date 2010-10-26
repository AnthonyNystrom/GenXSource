/* -----------------------------------------------
 * NuGenClickEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenClickEventArgs : EventArgs
	{
		private MouseButtons m_button;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenClickEventArgs"/> class.
		/// </summary>
		public NuGenClickEventArgs()
		{
			m_button = MouseButtons.Left;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenClickEventArgs"/> class.
		/// </summary>
		public NuGenClickEventArgs(MouseButtons button)
		{
			this.m_button = button;
		}

		/// <summary>
		/// </summary>
		public MouseButtons Button
		{
			get
			{
				return this.m_button;
			}
		}
	}
}
