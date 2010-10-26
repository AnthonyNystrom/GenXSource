/* -----------------------------------------------
 * NuGenSmoothRoundedPanelExServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

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
	public class NuGenSmoothRoundedPanelExServiceProvider : NuGenSmoothPanelExServiceProvider
	{
		private INuGenPanelExRenderer _panelExRenderer;

		/// <summary>
		/// </summary>
		protected override INuGenPanelExRenderer PanelExRenderer
		{
			get
			{
				if (_panelExRenderer == null)
				{
					_panelExRenderer = new NuGenSmoothRoundedPanelExRenderer(this);
				}

				return _panelExRenderer;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothRoundedPanelExServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothRoundedPanelExServiceProvider()
		{
		}
	}
}
