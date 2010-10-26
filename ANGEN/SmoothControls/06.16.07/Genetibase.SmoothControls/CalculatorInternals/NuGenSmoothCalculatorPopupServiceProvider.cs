/* -----------------------------------------------
 * NuGenSmoothCalculatorPopupServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.CalculatorInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenButtonRenderer"/></para>
	/// <para><see cref="INuGenButtonLayoutManager"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenTextBoxRenderer"/></para>
	/// </summary>
	public class NuGenSmoothCalculatorPopupServiceProvider : NuGenImageControlServiceProvider
	{
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

			if (serviceType == typeof(INuGenButtonRenderer))
			{
				return NuGenSmoothServiceManager.ButtonServiceProvider.GetService<INuGenButtonRenderer>();
			}
			else if (serviceType == typeof(INuGenButtonLayoutManager))
			{
				return NuGenSmoothServiceManager.ButtonServiceProvider.GetService<INuGenButtonLayoutManager>();
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				return NuGenSmoothServiceManager.PanelServiceProvider.GetService<INuGenPanelRenderer>();
			}
			else if (serviceType == typeof(INuGenTextBoxRenderer))
			{
				return NuGenSmoothServiceManager.TextBoxServiceProvider.GetService<INuGenTextBoxRenderer>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalculatorPopupServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothCalculatorPopupServiceProvider()
		{
		}
	}
}
