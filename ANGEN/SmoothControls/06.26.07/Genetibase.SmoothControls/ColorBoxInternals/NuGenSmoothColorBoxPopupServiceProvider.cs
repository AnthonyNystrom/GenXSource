/* -----------------------------------------------
 * NuGenSmoothColorBoxPopupServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
	/// <para><see cref="INuGenButtonLayoutManager"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenTabRenderer"/></para>
	/// <para><see cref="INuGenTabStateService"/></para>
	/// <para><see cref="INuGenTabLayoutManager"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenListBoxRenderer"/></para>
	/// <para><see cref="INuGenButtonRenderer"/></para>
	/// <para><see cref="INuGenImageListService"/></para>
	/// <para><see cref="INuGenColorsProvider"/></para>
	/// </summary>
	public class NuGenSmoothColorBoxPopupServiceProvider : NuGenImageControlServiceProvider
	{
		private INuGenColorsProvider _colorsProvider;

		/// <summary>
		/// </summary>
		protected virtual INuGenColorsProvider ColorsProvider
		{
			get
			{
				if (_colorsProvider == null)
				{
					_colorsProvider = new NuGenColorsProvider();
				}

				return _colorsProvider;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is <see langword="null"/>.</exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == typeof(INuGenTabRenderer))
			{
				return NuGenSmoothServiceManager.TabControlServiceProvider.GetService<INuGenTabRenderer>();
			}
			else if (serviceType == typeof(INuGenTabStateService))
			{
				return NuGenSmoothServiceManager.TabControlServiceProvider.GetService<INuGenTabStateService>();
			}
			else if (serviceType == typeof(INuGenListBoxRenderer))
			{
				return NuGenSmoothServiceManager.ListBoxServiceProvider.GetService<INuGenListBoxRenderer>();
			}
			else if (serviceType == typeof(INuGenImageListService))
			{
				return NuGenSmoothServiceManager.ListBoxServiceProvider.GetService<INuGenImageListService>();
			}
			else if (serviceType == typeof(INuGenColorsProvider))
			{
				Debug.Assert(this.ColorsProvider != null, "this.ColorsProvider != null");
				return this.ColorsProvider;
			}
			else if (serviceType == typeof(INuGenTabLayoutManager))
			{
				return NuGenSmoothServiceManager.TabControlServiceProvider.GetService<INuGenTabLayoutManager>();
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				return NuGenSmoothServiceManager.PanelServiceProvider.GetService<INuGenPanelRenderer>();
			}
			else if (serviceType == typeof(INuGenButtonRenderer))
			{
				return NuGenSmoothServiceManager.ButtonServiceProvider.GetService<INuGenButtonRenderer>();
			}
			else if (serviceType == typeof(INuGenButtonLayoutManager))
			{
				return NuGenSmoothServiceManager.ButtonServiceProvider.GetService<INuGenButtonLayoutManager>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorBoxPopupServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothColorBoxPopupServiceProvider()
		{
		}
	}
}
