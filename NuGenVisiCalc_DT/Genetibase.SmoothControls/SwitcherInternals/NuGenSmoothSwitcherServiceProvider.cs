/* -----------------------------------------------
 * NuGenSmoothSwitcherServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.PanelInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.SwitcherInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
	/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothSwitcherServiceProvider : NuGenControlServiceProvider
	{
		private INuGenPanelRenderer _panelRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenPanelRenderer PanelRenderer
		{
			get
			{
				if (_panelRenderer == null)
				{
					_panelRenderer = new NuGenSmoothPanelRenderer(this);
				}

				return _panelRenderer;
			}
		}

		private INuGenSwitchButtonLayoutManager _switchButtonLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenSwitchButtonLayoutManager SwitchButtonLayoutManager
		{
			get
			{
				if (_switchButtonLayoutManager == null)
				{
					_switchButtonLayoutManager = new NuGenSmoothSwitchButtonLayoutManager();
				}

				return _switchButtonLayoutManager;
			}
		}

		private INuGenSwitchButtonRenderer _switchButtonRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenSwitchButtonRenderer SwitchButtonRenderer
		{
			get
			{
				if (_switchButtonRenderer == null)
				{
					_switchButtonRenderer = new NuGenSmoothSwitchButtonRenderer(this);
				}

				return _switchButtonRenderer;
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
			else if (serviceType == typeof(INuGenSwitchButtonRenderer))
			{
				Debug.Assert(this.SwitchButtonRenderer != null, "this.SwitchButtonRenderer != null");
				return this.SwitchButtonRenderer;
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				Debug.Assert(this.PanelRenderer != null, "this.PanelRenderer != null");
				return this.PanelRenderer;
			}
			else if (serviceType == typeof(INuGenSwitchButtonLayoutManager))
			{
				Debug.Assert(this.SwitchButtonLayoutManager != null, "this.SwitchButtonLayoutManager != null");
				return this.SwitchButtonLayoutManager;
			}
			
			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSwitcherServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothSwitcherServiceProvider()
		{
		}
	}
}
