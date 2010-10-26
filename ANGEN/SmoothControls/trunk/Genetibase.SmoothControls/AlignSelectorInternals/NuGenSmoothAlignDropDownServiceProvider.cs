/* -----------------------------------------------
 * NuGenSmoothAlignDropDownServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.DropDownInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.SmoothControls.PanelInternals;

namespace Genetibase.SmoothControls.AlignSelectorInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenRadioButtonLayoutManager"/></para>
	/// <para><see cref="INuGenRadioButtonRenderer"/></para>
	/// </summary>
	public class NuGenSmoothAlignDropDownServiceProvider : NuGenSmoothAlignSelectorServiceProvider
	{
		private INuGenDropDownRenderer _dropDownRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenDropDownRenderer DropDownRenderer
		{
			get
			{
				if (_dropDownRenderer == null)
				{
					_dropDownRenderer = new NuGenSmoothDropDownRenderer(this);
				}

				return _dropDownRenderer;
			}
		}

		private INuGenControlImageManager _controlImageManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenControlImageManager ControlImageManager
		{
			get
			{
				if (_controlImageManager == null)
				{
					_controlImageManager = new NuGenControlImageManager();
				}

				return _controlImageManager;
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

			if (serviceType == typeof(INuGenDropDownRenderer))
			{
				return this.DropDownRenderer;
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				return this.PanelRenderer;
			}
			else if (serviceType == typeof(INuGenControlImageManager))
			{
				return this.ControlImageManager;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothAlignDropDownServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothAlignDropDownServiceProvider()
		{
		}
	}
}
