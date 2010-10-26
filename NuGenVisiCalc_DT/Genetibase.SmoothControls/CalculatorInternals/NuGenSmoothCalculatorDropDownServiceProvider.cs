/* -----------------------------------------------
 * NuGenSmoothCalculatorDropDownServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.CalculatorInternals;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Drawing;
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
	/// <para><see cref="INuGenDropDownRenderer"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenTextBoxRenderer"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// </summary>
	public class NuGenSmoothCalculatorDropDownServiceProvider : NuGenSmoothCalculatorPopupServiceProvider
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

			if (serviceType == typeof(INuGenDropDownRenderer))
			{
				return NuGenSmoothServiceManager.DropDownServiceProvider.GetService<INuGenDropDownRenderer>();
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalculatorDropDownServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothCalculatorDropDownServiceProvider()
		{
		}
	}
}
