/* -----------------------------------------------
 * NuGenSmoothPictureBoxServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.PictureBoxInternals;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.PanelInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.PictureBoxInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenPictureBoxRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenSmoothPictureBoxLayoutManager"/></para>
	/// </summary>
	public class NuGenSmoothPictureBoxServiceProvider : NuGenPictureBoxServiceProvider
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

		private INuGenSmoothPictureBoxLayoutManager _pictureBoxLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenSmoothPictureBoxLayoutManager PictureBoxLayoutManager
		{
			get
			{
				if (_pictureBoxLayoutManager == null)
				{
					_pictureBoxLayoutManager = new NuGenSmoothPictureBoxLayoutManager();
				}

				return _pictureBoxLayoutManager;
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
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				Debug.Assert(this.PanelRenderer != null, "this.PanelRenderer != null");
				return this.PanelRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothPictureBoxLayoutManager))
			{
				Debug.Assert(this.PictureBoxLayoutManager != null, "this.PictureBoxLayoutManager != null");
				return this.PictureBoxLayoutManager;
			}
			
			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPictureBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothPictureBoxServiceProvider()
		{
		}
	}
}
