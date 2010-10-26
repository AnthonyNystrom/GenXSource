/* -----------------------------------------------
 * NuGenSmoothColorBoxServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.ColorBoxInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenDropDownRenderer"/></para>
	/// <para><see cref="INuGenTabRenderer"/></para>
	/// <para><see cref="INuGenTabStateService"/></para>
	/// <para><see cref="INuGenTabLayoutManager"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenListBoxRenderer"/></para>
	/// <para><see cref="INuGenButtonRenderer"/></para>
	/// <para><see cref="INuGenImageListService"/></para>
	/// <para><see cref="INuGenColorsProvider"/></para>
	/// </summary>
	public class NuGenSmoothColorBoxServiceProvider : NuGenSmoothColorBoxPopupServiceProvider
	{
		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is <see langword="null"/>.</exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == typeof(INuGenDropDownRenderer))
			{
				return NuGenSmoothServiceManager.DropDownServiceProvider.GetService<INuGenDropDownRenderer>();
			}

			return base.GetService(serviceType);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothColorBoxServiceProvider()
		{

		}
	}
}
