/* -----------------------------------------------
 * NuGenVisiCalcServiceProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenToolStripAutoSizeService"/><para/>
	/// <see cref="INuGenWindowStateTracker"/><para/>
	/// <see cref="INuGenMenuItemCheckedTracker"/><para/>
	/// </summary>
	internal class NuGenVisiCalcServiceProvider : INuGenServiceProvider
	{
		#region INuGenServiceProvider Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public T GetService<T>() where T : class
		{
			return (T)this.GetService(typeof(T));
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * MenuItemCheckedTracker
		 */

		private INuGenMenuItemCheckedTracker _menuItemCheckedTracker = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenMenuItemCheckedTracker MenuItemCheckedTracker
		{
			get
			{
				if (_menuItemCheckedTracker == null)
				{
					_menuItemCheckedTracker = new NuGenMenuItemCheckedTracker();
				}

				return _menuItemCheckedTracker;
			}
		}

		/*
		 * ToolStripAutoSizeService
		 */

		private INuGenToolStripAutoSizeService _toolStripAutoSizeService = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenToolStripAutoSizeService ToolStripAutoSizeService
		{
			get
			{
				if (_toolStripAutoSizeService == null)
				{
					_toolStripAutoSizeService = new NuGenToolStripAutoSizeService();
				}

				return _toolStripAutoSizeService;
			}
		}

		/*
		 * WindowStateTracker
		 */

		private INuGenWindowStateTracker _windowStateTracker = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenWindowStateTracker WindowStateTracker
		{
			get
			{
				if (_windowStateTracker == null)
				{
					_windowStateTracker = new NuGenWindowStateTracker();
				}

				return _windowStateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns><see langword="null"/> if the specified service was not found.</returns>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceType"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		protected virtual object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenToolStripAutoSizeService))
			{
				Debug.Assert(this.ToolStripAutoSizeService != null, "this.ToolStripAutoSizeService != null");
				return this.ToolStripAutoSizeService;
			}

			if (serviceType == typeof(INuGenWindowStateTracker))
			{
				Debug.Assert(this.WindowStateTracker != null, "this.WindowStateTracker != null");
				return this.WindowStateTracker;
			}

			if (serviceType == typeof(INuGenMenuItemCheckedTracker))
			{
				Debug.Assert(this.MenuItemCheckedTracker != null, "this.MenuItemCheckedTracker != null");
				return this.MenuItemCheckedTracker;
			}

			return null;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenVisiCalcServiceProvider"/> class.
		/// </summary>
		public NuGenVisiCalcServiceProvider()
		{
		}

		#endregion
	}
}
