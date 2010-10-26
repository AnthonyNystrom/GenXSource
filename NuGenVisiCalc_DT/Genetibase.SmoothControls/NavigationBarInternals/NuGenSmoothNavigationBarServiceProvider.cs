/* -----------------------------------------------
 * NuGenSmoothNavigationBarServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.NavigationBarInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TitleInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.NavigationBarInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenNavigationBarRenderer"/></para>
	/// <para><see cref="INuGenNavigationBarLayoutManager"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenTitleRenderer"/></para>
	/// <para><see cref="INuGenToolStripRenderer"/></para>
	/// <para><see cref="INuGenToolTipLayoutManager"/></para>
	/// <para><see cref="INuGenToolTipRenderer"/></para>
	/// </summary>
	public class NuGenSmoothNavigationBarServiceProvider : NuGenImageControlServiceProvider
	{
		/*
		 * NavigationBarLayoutManager
		 */

		private INuGenNavigationBarLayoutManager _navigationBarLayoutManager;

		/// <summary></summary>
		protected virtual INuGenNavigationBarLayoutManager NavigationBarLayoutManager
		{
			get
			{
				if (_navigationBarLayoutManager == null)
				{
					_navigationBarLayoutManager = new NuGenSmoothNavigationBarLayoutManager();
				}

				return _navigationBarLayoutManager;
			}
		}

		/*
		 * NavigationBarRenderer
		 */

		private INuGenNavigationBarRenderer _navigationBarRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenNavigationBarRenderer NavigationBarRenderer
		{
			get
			{
				if (_navigationBarRenderer == null)
				{
					_navigationBarRenderer = new NuGenSmoothNavigationBarRenderer(this);
				}

				return _navigationBarRenderer;
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

			if (serviceType == typeof(INuGenNavigationBarLayoutManager))
			{
				Debug.Assert(this.NavigationBarLayoutManager != null, "this.NavigationBarLayoutManager != null");
				return this.NavigationBarLayoutManager;
			}
			else if (serviceType == typeof(INuGenNavigationBarRenderer))
			{
				Debug.Assert(this.NavigationBarRenderer != null, "this.NavigationBarRenderer != null");
				return this.NavigationBarRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenToolStripRenderer))
			{
				return NuGenSmoothServiceManager.ToolStripServiceProvider.GetService<INuGenToolStripRenderer>();
			}
			else if (serviceType == typeof(INuGenToolTipLayoutManager))
			{
				return NuGenSmoothServiceManager.ToolTipServiceProvider.GetService<INuGenToolTipLayoutManager>();
			}
			else if (serviceType == typeof(INuGenToolTipRenderer))
			{
				return NuGenSmoothServiceManager.ToolTipServiceProvider.GetService<INuGenToolTipRenderer>();
			}
			else if (serviceType == typeof(INuGenTitleRenderer))
			{
				return NuGenSmoothServiceManager.TitleServiceProvider.GetService<INuGenTitleRenderer>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothNavigationBarServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothNavigationBarServiceProvider()
		{
		}
	}
}
