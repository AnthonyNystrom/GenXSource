/* -----------------------------------------------
 * NuGenSmoothPanelExServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelExInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.PanelExInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenPanelExRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothPanelExServiceProvider : NuGenControlServiceProvider
	{
		private INuGenPanelExRenderer _panelExRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenPanelExRenderer PanelExRenderer
		{
			get
			{
				if (_panelExRenderer == null)
				{
					_panelExRenderer = new NuGenSmoothPanelExRenderer(this);
				}

				return _panelExRenderer;
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

			if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenPanelExRenderer))
			{
				Debug.Assert(this.PanelExRenderer != null, "this.PanelExRenderer != null");
				return this.PanelExRenderer;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPanelExServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothPanelExServiceProvider()
		{
		}
	}
}
