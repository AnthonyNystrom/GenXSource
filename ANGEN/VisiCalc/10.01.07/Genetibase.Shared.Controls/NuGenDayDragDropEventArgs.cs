/* -----------------------------------------------
 * NuGenDayDragDropEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenDayDragDropEventArgs : EventArgs
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
		public int KeyState
		{
			get
			{
				return this.m_keyState;
			}
		}

		/// <summary>
		/// </summary>
		public IDataObject Data
		{
			get
			{
				return this.m_data;
			}
		}

		private string m_date;
		private int m_keyState;
		private IDataObject m_data;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDayDragDropEventArgs"/> class.
		/// </summary>
		public NuGenDayDragDropEventArgs(IDataObject data, int keystate, string date)
		{
			m_date = date;
			m_data = data;
			m_keyState = keystate;
		}
	}
}
