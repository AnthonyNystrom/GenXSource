/* -----------------------------------------------
 * NuGenSmoothDialogBlockServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks;
using Genetibase.ApplicationBlocks.DialogInternals;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls;
using Genetibase.SmoothControls.PanelInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothApplicationBlocks.DialogInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenDialogBlockLayoutManager"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothDialogBlockServiceProvider : NuGenControlServiceProvider
	{
		private INuGenDialogBlockLayoutManager _dialogBlockLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenDialogBlockLayoutManager DialogBlockLayoutManager
		{
			get
			{
				if (_dialogBlockLayoutManager == null)
				{
					_dialogBlockLayoutManager = new NuGenSmoothDialogBlockLayoutManager();
				}

				return _dialogBlockLayoutManager;
			}
		}

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

			if (serviceType == typeof(INuGenDialogBlockLayoutManager))
			{
				Debug.Assert(this.DialogBlockLayoutManager != null, "this.DialogBlockLayoutManager != null");
				return this.DialogBlockLayoutManager;
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				Debug.Assert(this.PanelRenderer != null, "this.PanelRenderer != null");
				return this.PanelRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDialogBlockServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothDialogBlockServiceProvider()
		{
		}
	}
}
