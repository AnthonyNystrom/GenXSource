/* -----------------------------------------------
 * NuGenSmoothImageExportServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls;
using Genetibase.SmoothControls.ButtonInternals;
using Genetibase.SmoothControls.CheckBoxInternals;
using Genetibase.SmoothControls.ComboBoxInternals;
using Genetibase.SmoothControls.DirectorySelectorInternals;
using Genetibase.SmoothControls.DropDownInternals;
using Genetibase.SmoothControls.PanelInternals;
using Genetibase.SmoothControls.ProgressBarInternals;
using Genetibase.SmoothControls.ScrollBarInternals;
using Genetibase.SmoothControls.SpinInternals;
using Genetibase.SmoothControls.SwitcherInternals;
using Genetibase.SmoothControls.TabControlInternals;
using Genetibase.SmoothControls.TextBoxInternals;
using Genetibase.SmoothControls.ToolStripInternals;
using Genetibase.SmoothControls.TrackBarInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothApplicationBlocks.ImageExportInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenCheckBoxLayoutManager"/></para>
	/// <para><see cref="INuGenCheckBoxRenderer"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenButtonLayoutManager"/></para>
	/// <para><see cref="INuGenButtonRenderer"/></para>
	/// <para><see cref="INuGenColorsProvider"/></para>
	/// <para><see cref="INuGenComboBoxRenderer"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// <para><see cref="INuGenDirectorySelectorRenderer"/></para>
	/// <para><see cref="INuGenFontFamiliesProvider"/></para>
	/// <para><see cref="INuGenImageListService"/></para>
	/// <para><see cref="INuGenInt32ValueConverter"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenProgressBarLayoutManager"/></para>
	/// <para><see cref="INuGenProgressBarRenderer"/></para>
	/// <para><see cref="INuGenScrollBarRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenSpinRenderer"/></para>
	/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
	/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
	/// <para><see cref="INuGenTabLayoutManager"/></para>
	/// <para><see cref="INuGenTabRenderer"/></para>
	/// <para><see cref="INuGenTabStateService"/></para>
	/// <para><see cref="INuGenTextBoxRenderer"/></para>
	/// <para><see cref="INuGenThumbnailLayoutManager"/></para>
	/// <para><see cref="INuGenThumbnailRenderer"/></para>
	/// <para><see cref="INuGenToolStripRenderer"/></para>
	/// <para><see cref="INuGenTrackBarRenderer"/></para>
	/// <para><see cref="INuGenValueTrackerService"/></para>
	/// </summary>
	public class NuGenSmoothImageExportServiceProvider : NuGenSmoothThumbnailContainerServiceProvider
	{
		private INuGenButtonLayoutManager _buttonLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenButtonLayoutManager ButtonLayoutManager
		{
			get
			{
				if (_buttonLayoutManager == null)
				{
					_buttonLayoutManager = new NuGenSmoothButtonLayoutManager();
				}

				return _buttonLayoutManager;
			}
		}

		private INuGenButtonRenderer _buttonRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenButtonRenderer ButtonRenderer
		{
			get
			{
				if (_buttonRenderer == null)
				{
					_buttonRenderer = new NuGenSmoothButtonRenderer(this);
				}

				return _buttonRenderer;
			}
		}

		private INuGenCheckBoxLayoutManager _checkBoxLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenCheckBoxLayoutManager CheckBoxLayoutManager
		{
			get
			{
				if (_checkBoxLayoutManager == null)
				{
					_checkBoxLayoutManager = new NuGenCheckBoxLayoutManager();
				}

				return _checkBoxLayoutManager;
			}
		}

		private INuGenCheckBoxRenderer _checkBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenCheckBoxRenderer CheckBoxRenderer
		{
			get
			{
				if (_checkBoxRenderer == null)
				{
					_checkBoxRenderer = new NuGenSmoothCheckBoxRenderer(this);
				}

				return _checkBoxRenderer;
			}
		}

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

		private INuGenDirectorySelectorRenderer _directorySelectorRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenDirectorySelectorRenderer DirectorySelectorRenderer
		{
			get
			{
				if (_directorySelectorRenderer == null)
				{
					_directorySelectorRenderer = new NuGenSmoothDirectorySelectorRenderer(this);
				}

				return _directorySelectorRenderer;
			}
		}

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

		/*
		 * Int32ValueConverter
		 */

		private INuGenInt32ValueConverter _int32ValueConverter;

		/// <summary>
		/// </summary>
		protected virtual INuGenInt32ValueConverter Int32ValueConverter
		{
			get
			{
				if (_int32ValueConverter == null)
				{
					_int32ValueConverter = new NuGenInt32ValueConverter();
				}

				return _int32ValueConverter;
			}
		}

		private INuGenProgressBarLayoutManager _progressBarLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenProgressBarLayoutManager ProgressBarLayoutManager
		{
			get
			{
				if (_progressBarLayoutManager == null)
				{
					_progressBarLayoutManager = new NuGenProgressBarLayoutManager();
				}

				return _progressBarLayoutManager;
			}
		}

		private INuGenProgressBarRenderer _progressBarRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenProgressBarRenderer ProgressBarRenderer
		{
			get
			{
				if (_progressBarRenderer == null)
				{
					_progressBarRenderer = new NuGenSmoothProgressBarRenderer(this);
				}

				return _progressBarRenderer;
			}
		}

		private INuGenSpinRenderer _spinRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenSpinRenderer SpinRenderer
		{
			get
			{
				if (_spinRenderer == null)
				{
					_spinRenderer = new NuGenSmoothSpinRenderer(this);
				}

				return _spinRenderer;
			}
		}

		private INuGenTabLayoutManager _tabLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenTabLayoutManager TabLayoutManager
		{
			get
			{
				if (_tabLayoutManager == null)
				{
					_tabLayoutManager = new NuGenSmoothTabLayoutManager();
				}

				return _tabLayoutManager;
			}
		}

		private INuGenTabRenderer _tabRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenTabRenderer TabRenderer
		{
			get
			{
				if (_tabRenderer == null)
				{
					_tabRenderer = new NuGenSmoothTabRenderer(this);
				}

				return _tabRenderer;
			}
		}

		private INuGenTabStateService _tabStateService;

		/// <summary>
		/// </summary>
		protected virtual INuGenTabStateService TabStateService
		{
			get
			{
				if (_tabStateService == null)
				{
					_tabStateService = new NuGenTabStateService();
				}

				return _tabStateService;
			}
		}

		private INuGenTextBoxRenderer _textBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenTextBoxRenderer TextBoxRenderer
		{
			get
			{
				if (_textBoxRenderer == null)
				{
					_textBoxRenderer = new NuGenSmoothTextBoxRenderer(this);
				}

				return _textBoxRenderer;
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
				Debug.Assert(this.ButtonLayoutManager != null, "this.ButtonLayoutManager != null");
				return this.ButtonLayoutManager;
			}
			else if (serviceType == typeof(INuGenButtonRenderer))
			{
				Debug.Assert(this.ButtonRenderer != null, "this.ButtonRenderer != null");
				return this.ButtonRenderer;
			}
			else if (serviceType == typeof(INuGenCheckBoxLayoutManager))
			{
				Debug.Assert(this.CheckBoxLayoutManager != null, "this.CheckBoxLayoutManager != null");
				return this.CheckBoxLayoutManager;
			}
			else if (serviceType == typeof(INuGenCheckBoxRenderer))
			{
				Debug.Assert(this.CheckBoxRenderer != null, "this.CheckBoxRenderer != null");
				return this.CheckBoxRenderer;
			}
			else if (serviceType == typeof(INuGenColorsProvider))
			{
				Debug.Assert(this.ColorsProvider != null, "this.ColorsProvider != null");
				return this.ColorsProvider;
			}
			else if (serviceType == typeof(INuGenComboBoxRenderer))
			{
				Debug.Assert(this.ComboBoxRenderer != null, "this.ComboBoxRenderer != null");
				return this.ComboBoxRenderer;
			}
			else if (serviceType == typeof(INuGenDirectorySelectorRenderer))
			{
				Debug.Assert(this.DirectorySelectorRenderer != null, "this.DirectorySelectorRenderer != null");
				return this.DirectorySelectorRenderer;
			}
			else if (serviceType == typeof(INuGenDropDownRenderer))
			{
				Debug.Assert(this.DropDownRenderer != null, "this.DropDownRenderer != null");
				return this.DropDownRenderer;
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
			else if (serviceType == typeof(INuGenInt32ValueConverter))
			{
				Debug.Assert(this.Int32ValueConverter != null, "this.Int32ValueConverter != null");
				return this.Int32ValueConverter;
			}
			else if (serviceType == typeof(INuGenTextBoxRenderer))
			{
				Debug.Assert(this.TextBoxRenderer != null, "this.TextBoxRenderer != null");
				return this.TextBoxRenderer;
			}
			else if (serviceType == typeof(INuGenProgressBarLayoutManager))
			{
				Debug.Assert(this.ProgressBarLayoutManager != null, "this.ProgressBarLayoutManager != null");
				return this.ProgressBarLayoutManager;
			}
			else if (serviceType == typeof(INuGenProgressBarRenderer))
			{
				Debug.Assert(this.ProgressBarRenderer != null, "this.ProgressBarRenderer != null");
				return this.ProgressBarRenderer;
			}
			else if (serviceType == typeof(INuGenSpinRenderer))
			{
				Debug.Assert(this.SpinRenderer != null, "this.SpinRenderer != null");
				return this.SpinRenderer;
			}
			else if (serviceType == typeof(INuGenTabLayoutManager))
			{
				Debug.Assert(this.TabLayoutManager != null, "this.TabLayoutManager != null");
				return this.TabLayoutManager;
			}
			else if (serviceType == typeof(INuGenTabRenderer))
			{
				Debug.Assert(this.TabRenderer != null, "this.TabRenderer != null");
				return this.TabRenderer;
			}
			else if (serviceType == typeof(INuGenTabStateService))
			{
				Debug.Assert(this.TabStateService != null, "this.TabStateService != null");
				return this.TabStateService;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothImageExportServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothImageExportServiceProvider()
		{
		}
	}
}
