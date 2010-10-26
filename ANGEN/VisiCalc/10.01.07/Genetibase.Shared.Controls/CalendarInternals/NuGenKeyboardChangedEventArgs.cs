/* -----------------------------------------------
 * NuGenKeyboardChangedEventArgs.cs
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
	public class NuGenKeyboardChangedEventArgs : EventArgs
	{
		#region Class Data

		private NuGenKeyboard m_key;
		private Keys m_old;
		private Keys m_new;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the DayClickEventArgs class with default settings
		/// </summary>
		public NuGenKeyboardChangedEventArgs(NuGenKeyboard key, Keys oldKey, Keys newKey)
		{
			m_key = key;
			m_old = oldKey;
			m_new = newKey;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public Keys NewKey
		{
			get
			{
				return m_new;
			}
		}

		/// <summary>
		/// </summary>
		public Keys OldKey
		{
			get
			{
				return m_old;
			}
		}

		/// <summary>
		/// </summary>
		public NuGenKeyboard Key
		{
			get
			{
				return m_key;
			}
		}


		#endregion
	}
}
