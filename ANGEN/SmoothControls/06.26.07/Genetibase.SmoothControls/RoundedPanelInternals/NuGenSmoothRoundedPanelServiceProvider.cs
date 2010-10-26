/* -----------------------------------------------
 * NuGenSmoothRoundedPanelServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.RoundedPanelInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.RoundedPanelInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenRoundedPanelRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothRoundedPanelServiceProvider : NuGenControlServiceProvider
	{
		private INuGenRoundedPanelRenderer _roundedPanelRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenRoundedPanelRenderer RoundedPanelRenderer
		{
			get
			{
				if (_roundedPanelRenderer == null)
				{
					_roundedPanelRenderer = new NuGenSmoothRoundedPanelRenderer(this);
				}

				return _roundedPanelRenderer;
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
			else if (serviceType == typeof(INuGenRoundedPanelRenderer))
			{
				Debug.Assert(this.RoundedPanelRenderer != null, "this.RoundedPanelRenderer != null");
				return this.RoundedPanelRenderer;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothRoundedPanelServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothRoundedPanelServiceProvider()
		{
		}
	}
}
