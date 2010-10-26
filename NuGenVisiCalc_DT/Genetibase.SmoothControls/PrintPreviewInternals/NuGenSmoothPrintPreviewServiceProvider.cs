/* -----------------------------------------------
 * NuGenSmoothPrintPreviewServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PrintPreviewInternals;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.PrintPreviewInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateTracker"/></para>
	/// <para><see cref="INuGenControlStateTracker"/></para>
	/// <para><see cref="INuGenMenuItemCheckedTracker"/></para>
	/// <para><see cref="INuGenPrintPreviewToolStripManager"/></para>
	/// <para><see cref="INuGenSpinRenderer"/></para>
	/// <para><see cref="INuGenToolStripRenderer"/></para>
	/// </summary>
	public class NuGenSmoothPrintPreviewServiceProvider : NuGenControlServiceProvider
	{
		private INuGenMenuItemCheckedTracker _menuItemCheckedTracker;

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

		private INuGenPrintPreviewToolStripManager _printPreviewToolStripManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenPrintPreviewToolStripManager PrintPreviewToolStripManager
		{
			get
			{
				if (_printPreviewToolStripManager == null)
				{
					_printPreviewToolStripManager = new NuGenPrintPreviewToolStripManager();
				}

				return _printPreviewToolStripManager;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is <see langword="null"/>.</exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenMenuItemCheckedTracker))
			{
				Debug.Assert(this.MenuItemCheckedTracker != null, "this.MenuItemCheckedTracker != null");
				return this.MenuItemCheckedTracker;
			}
			else if (serviceType == typeof(INuGenPrintPreviewToolStripManager))
			{
				Debug.Assert(this.PrintPreviewToolStripManager != null, "this.PrintPreviewToolStripManager != null");
				return this.PrintPreviewToolStripManager;
			}
			else if (serviceType == typeof(INuGenSpinRenderer))
			{
				return NuGenSmoothServiceManager.SpinServiceProvider.GetService<INuGenSpinRenderer>();
			}
			else if (serviceType == typeof(INuGenToolStripRenderer))
			{
				return NuGenSmoothServiceManager.ToolStripServiceProvider.GetService<INuGenToolStripRenderer>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPrintPreviewServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothPrintPreviewServiceProvider()
		{
		}
	}
}
