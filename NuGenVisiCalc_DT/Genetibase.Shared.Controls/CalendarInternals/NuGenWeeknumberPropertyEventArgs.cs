/* -----------------------------------------------
 * NuGenWeeknumberPropertyEventArgs.cs
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
	public class NuGenWeeknumberPropertyEventArgs : EventArgs
	{
		#region Class Data

		/// <summary>
		/// The property that has changed
		/// </summary>
		private NuGenWeeknumberProperty m_property;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWeeknumberPropertyEventArgs"/> class.
		/// </summary>
		public NuGenWeeknumberPropertyEventArgs()
		{
			m_property = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWeeknumberPropertyEventArgs"/> class.
		/// </summary>
		public NuGenWeeknumberPropertyEventArgs(NuGenWeeknumberProperty property)
		{
			this.m_property = property;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public NuGenWeeknumberProperty Property
		{
			get
			{
				return this.m_property;
			}
		}

		#endregion
	}
}
