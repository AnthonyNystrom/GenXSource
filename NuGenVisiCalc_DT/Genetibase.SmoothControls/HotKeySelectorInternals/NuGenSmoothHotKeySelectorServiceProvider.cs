/* -----------------------------------------------
 * NuGenSmoothHotKeySelectorServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.LabelInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.HotKeySelectorInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateTracker"/></para>
	/// <para><see cref="INuGenControlStateTracker"/></para>
	/// <para><see cref="INuGenButtonLayoutManager"/></para>
	/// <para><see cref="INuGenButtonRenderer"/></para>
	/// <para><see cref="INuGenCheckBoxLayoutManager"/></para>
	/// <para><see cref="INuGenCheckBoxRenderer"/></para>
	/// <para><see cref="INuGenComboBoxRenderer"/></para>
	/// <para><see cref="INuGenDropDownRenderer"/></para>
	/// <para><see cref="INuGenLabelLayoutManager"/></para>
	/// <para><see cref="INuGenLabelRenderer"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// </summary>
	public class NuGenSmoothHotKeySelectorServiceProvider : NuGenImageControlServiceProvider
	{
		private INuGenLabelLayoutManager _labelLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenLabelLayoutManager LabelLayoutManager
		{
			get
			{
				if (_labelLayoutManager == null)
				{
					_labelLayoutManager = new NuGenLabelLayoutManager();
				}

				return _labelLayoutManager;
			}
		}

		private INuGenLabelRenderer _labelRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenLabelRenderer LabelRenderer
		{
			get
			{
				if (_labelRenderer == null)
				{
					_labelRenderer = new NuGenLabelRenderer();
				}

				return _labelRenderer;
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

			if (serviceType == typeof(INuGenButtonLayoutManager))
			{
				return NuGenSmoothServiceManager.ButtonServiceProvider.GetService<INuGenButtonLayoutManager>();
			}
			else if (serviceType == typeof(INuGenButtonRenderer))
			{
				return NuGenSmoothServiceManager.ButtonServiceProvider.GetService<INuGenButtonRenderer>();
			}
			else if (serviceType == typeof(INuGenCheckBoxLayoutManager))
			{
				return NuGenSmoothServiceManager.CheckBoxServiceProvider.GetService<INuGenCheckBoxLayoutManager>();
			}
			else if (serviceType == typeof(INuGenCheckBoxRenderer))
			{
				return NuGenSmoothServiceManager.CheckBoxServiceProvider.GetService<INuGenCheckBoxRenderer>();
			}
			else if (serviceType == typeof(INuGenComboBoxRenderer))
			{
				return NuGenSmoothServiceManager.ComboBoxServiceProvider.GetService<INuGenComboBoxRenderer>();
			}
			else if (serviceType == typeof(INuGenDropDownRenderer))
			{
				return NuGenSmoothServiceManager.DropDownServiceProvider.GetService<INuGenDropDownRenderer>();
			}
			else if (serviceType == typeof(INuGenLabelLayoutManager))
			{
				Debug.Assert(this.LabelLayoutManager != null, "this.LabelLayoutManager != null");
				return this.LabelLayoutManager;
			}
			else if (serviceType == typeof(INuGenLabelRenderer))
			{
				Debug.Assert(this.LabelRenderer != null, "this.LabelRenderer != null");
				return this.LabelRenderer;
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				return NuGenSmoothServiceManager.PanelServiceProvider.GetService<INuGenPanelRenderer>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothHotKeySelectorServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothHotKeySelectorServiceProvider()
		{
		}
	}
}
