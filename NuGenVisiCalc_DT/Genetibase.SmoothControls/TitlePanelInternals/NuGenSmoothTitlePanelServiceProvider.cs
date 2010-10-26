/* -----------------------------------------------
 * NuGenSmoothTitlePanelServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TitleInternals;
using Genetibase.Shared.Controls.TitlePanelInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.TitlePanelInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenTitleRenderer"/></para>
	/// <para><see cref="INuGenTitlePanelLayoutManager"/></para>
	/// </summary>
	public class NuGenSmoothTitlePanelServiceProvider : NuGenImageControlServiceProvider
	{
		private INuGenTitlePanelLayoutManager _titlePanelLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenTitlePanelLayoutManager TitlePanelLayoutManager
		{
			get
			{
				if (_titlePanelLayoutManager == null)
				{
					_titlePanelLayoutManager = new NuGenSmoothTitlePanelLayoutManager();
				}

				return _titlePanelLayoutManager;
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

			if (serviceType == typeof(INuGenPanelRenderer))
			{
				return NuGenSmoothServiceManager.PanelServiceProvider.GetService<INuGenPanelRenderer>();
			}
			else if (serviceType == typeof(INuGenTitleRenderer))
			{
				return NuGenSmoothServiceManager.TitleServiceProvider.GetService<INuGenTitleRenderer>();
			}
			else if (serviceType == typeof(INuGenTitlePanelLayoutManager))
			{
				Debug.Assert(this.TitlePanelLayoutManager != null, "this.TitlePanelLayoutManager != null");
				return this.TitlePanelLayoutManager;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTitlePanelServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothTitlePanelServiceProvider()
		{
		}
	}
}
