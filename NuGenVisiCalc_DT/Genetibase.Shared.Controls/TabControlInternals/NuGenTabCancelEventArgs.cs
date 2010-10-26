/* -----------------------------------------------
 * NuGenTabCancelEventArgs.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public class NuGenTabCancelEventArgs : EventArgs
	{
		#region Properties.Public

		/*
		 * TabPage
		 */

		private NuGenTabPage _tabPage = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		public NuGenTabPage TabPage
		{
			get
			{
				return _tabPage;
			}
		}

		/*
		 * Cancel
		 */

		private bool _cancel = false;

		/// <summary>
		/// </summary>
		public bool Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabCancelEventArgs"/> class.<para/>
		/// Cancel = <see langword="false"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="tabPage"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabCancelEventArgs(NuGenTabPage tabPage)
			: this(tabPage, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabCancelEventArgs"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="tabPage"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabCancelEventArgs(NuGenTabPage tabPage, bool cancel)
		{
			if (tabPage == null)
			{
				throw new ArgumentNullException("tabPage");
			}

			_tabPage = tabPage;
			_cancel = cancel;
		}

		#endregion
	}
}
