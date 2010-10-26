/* -----------------------------------------------
 * NuGenSmoothFontBlockServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls;
using Genetibase.SmoothControls.ComboBoxInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.SmoothControls.SwitcherInternals;

namespace Genetibase.SmoothApplicationBlocks.FontInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenComboBoxRenderer"/></para>
	/// <para><see cref="INuGenFontFamiliesProvider"/></para>
	/// <para><see cref="INuGenImageListService"/></para>
	/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
	/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothFontBlockServiceProvider : NuGenControlServiceProvider
	{
		private INuGenComboBoxRenderer _comboBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenComboBoxRenderer ComboBoxRenderer
		{
			get
			{
				if (_comboBoxRenderer == null)
				{
					_comboBoxRenderer = new NuGenSmoothComboBoxRenderer(this);
				}

				return _comboBoxRenderer;
			}
		}

		private INuGenFontFamiliesProvider _fontFamiliesProvider;

		/// <summary>
		/// </summary>
		protected virtual INuGenFontFamiliesProvider FontFamiliesProvider
		{
			get
			{
				if (_fontFamiliesProvider == null)
				{
					_fontFamiliesProvider = new NuGenFontFamiliesProvider();
				}

				return _fontFamiliesProvider;
			}
		}

		private INuGenImageListService _imageListService;

		/// <summary>
		/// </summary>
		protected virtual INuGenImageListService ImageListService
		{
			get
			{
				if (_imageListService == null)
				{
					_imageListService = new NuGenImageListService();
				}

				return _imageListService;
			}
		}

		private INuGenSwitchButtonLayoutManager _switchButtonLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenSwitchButtonLayoutManager SwitchButtonLayoutManager
		{
			get
			{
				if (_switchButtonLayoutManager == null)
				{
					_switchButtonLayoutManager = new NuGenSmoothSwitchButtonLayoutManager();
				}

				return _switchButtonLayoutManager;
			}
		}

		private INuGenSwitchButtonRenderer _switchButtonRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenSwitchButtonRenderer SwitchButtonRenderer
		{
			get
			{
				if (_switchButtonRenderer == null)
				{
					_switchButtonRenderer = new NuGenSmoothSwitchButtonRenderer(this);
				}

				return _switchButtonRenderer;
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

			if (serviceType == typeof(INuGenComboBoxRenderer))
			{
				Debug.Assert(this.ComboBoxRenderer != null, "this.ComboBoxRenderer != null");
				return this.ComboBoxRenderer;
			}
			else if (serviceType == typeof(INuGenFontFamiliesProvider))
			{
				Debug.Assert(this.FontFamiliesProvider != null, "this.FontFamiliesProvider != null");
				return this.FontFamiliesProvider;
			}
			else if (serviceType == typeof(INuGenImageListService))
			{
				Debug.Assert(this.ImageListService != null, "this.ImageListService != null");
				return this.ImageListService;
			}
			else if (serviceType == typeof(INuGenSwitchButtonLayoutManager))
			{
				Debug.Assert(this.SwitchButtonLayoutManager != null, "this.SwitchButtonLayoutManager != null");
				return this.SwitchButtonLayoutManager;
			}
			else if (serviceType == typeof(INuGenSwitchButtonRenderer))
			{
				Debug.Assert(this.SwitchButtonRenderer != null, "this.SwitchButtonRenderer != null");
				return this.SwitchButtonRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothFontBlockServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothFontBlockServiceProvider()
		{
		}
	}
}
