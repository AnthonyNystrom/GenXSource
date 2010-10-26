/* -----------------------------------------------
 * PresenterServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ComponentModel;
using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.ApplicationBlocks.PresenterInternals;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothApplicationBlocks.ImageExportInternals;
using Genetibase.SmoothControls;
using Genetibase.SmoothControls.ButtonInternals;
using Genetibase.SmoothControls.CheckBoxInternals;
using Genetibase.SmoothControls.ComboBoxInternals;
using Genetibase.SmoothControls.DirectorySelectorInternals;
using Genetibase.SmoothControls.DropDownInternals;
using Genetibase.SmoothControls.ListBoxInternals;
using Genetibase.SmoothControls.PanelInternals;
using Genetibase.SmoothControls.ProgressBarInternals;
using Genetibase.SmoothControls.RadioButtonInternals;
using Genetibase.SmoothControls.ScrollBarInternals;
using Genetibase.SmoothControls.SpinInternals;
using Genetibase.SmoothControls.SwitcherInternals;
using Genetibase.SmoothControls.TabControlInternals;
using Genetibase.SmoothControls.TextBoxInternals;
using Genetibase.SmoothControls.TrackBarInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenPresenter
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenCheckBoxLayoutManager"/></para>
	/// <para><see cref="INuGenCheckBoxRenderer"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	///	<para><see cref="INuGenButtonLayoutManager"/></para>
	///	<para><see cref="INuGenButtonRenderer"/></para>
	/// <para><see cref="INuGenColorsProvider"/></para>
	/// <para><see cref="INuGenComboBoxRenderer"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// <para><see cref="INuGenDropDownRenderer"/></para>
	/// <para><see cref="INuGenDirectorySelectorRenderer"/></para>
	/// <para><see cref="INuGenImageListService"/></para>
	/// <para><see cref="INuGenFontFamiliesProvider"/></para>
	/// <para><see cref="INuGenListBoxRenderer"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenProgressBarLayoutManager"/></para>
	/// <para><see cref="INuGenProgressBarRenderer"/></para>
	/// <para><see cref="INuGenRadioButtonLayoutManager"/></para>
	///	<para><see cref="INuGenRadioButtonRenderer"/></para>
	/// <para><see cref="INuGenScrollBarRenderer"/></para>
	/// <para><see cref="INuGenSpinRenderer"/></para>
	/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
	/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
	/// <para><see cref="INuGenTabStateTracker"/></para>
	///	<para><see cref="INuGenTabLayoutManager"/></para>
	/// <para><see cref="INuGenTabRenderer"/></para>
	/// <para><see cref="INuGenTempImageService"/></para>
	/// <para><see cref="INuGenTextBoxRenderer"/></para>
	/// <para><see cref="INuGenTrackBarRenderer"/></para>
	/// <para><see cref="INuGenThumbnailLayoutManager"/></para>
	/// <para><see cref="INuGenThumbnailRenderer"/></para>
	/// <para><see cref="INuGenToolStripRenderer"/></para>
	/// <para><see cref="INuGenValueTrackerService"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	internal class PresenterServiceProvider : NuGenImageControlServiceProvider, IDisposable
	{
		private INuGenButtonLayoutManager _buttonLayoutManager;
		private INuGenButtonRenderer _buttonRenderer;
		private INuGenCheckBoxLayoutManager _checkBoxLayoutManager;
		private INuGenCheckBoxRenderer _checkBoxRenderer;
		private INuGenColorsProvider _colorsProvider;
		private INuGenComboBoxRenderer _comboBoxRenderer;
		private INuGenControlImageManager _controlImageManager;
		private INuGenDirectorySelectorRenderer _directorySelectorRenderer;
		private INuGenDropDownRenderer _dropDownRenderer;
		private INuGenImageListService _imageListService;
		private INuGenFontFamiliesProvider _fontFamiliesProvider;
		private INuGenListBoxRenderer _listBoxRenderer;
		private INuGenPanelRenderer _panelRenderer;
		private INuGenProgressBarLayoutManager _progressBarLayoutManager;
		private INuGenProgressBarRenderer _progressBarRenderer;
		private INuGenRadioButtonLayoutManager _radioButtonLayoutManager;
		private INuGenRadioButtonRenderer _radioButtonRenderer;
		private INuGenScrollBarRenderer _scrollBarRenderer;
		private INuGenSpinRenderer _spinRenderer;
		private INuGenSwitchButtonLayoutManager _switchButtonLayoutManager;
		private INuGenSwitchButtonRenderer _switchButtonRenderer;
		private INuGenTabStateService _tabStateService;
		private INuGenTabLayoutManager _tabLayoutManager;
		private INuGenTabRenderer _tabRenderer;
		private INuGenTempImageService _tempImageService;
		private INuGenTextBoxRenderer _textBoxRenderer;
		private INuGenTrackBarRenderer _trackBarRenderer;
		private INuGenThumbnailLayoutManager _thumbnailLayoutManager;
		private INuGenThumbnailRenderer _thumbnailRenderer;
		private INuGenToolStripRenderer _toolStripRenderer;
		private INuGenValueTrackerService _valueTrackerService;
		private INuGenSmoothColorManager _smoothColorManager;

		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return _smoothColorManager;
			}
			else if (serviceType == typeof(INuGenButtonLayoutManager))
			{
				return _buttonLayoutManager;
			}
			else if (serviceType == typeof(INuGenButtonRenderer))
			{
				return _buttonRenderer;
			}
			else if (serviceType == typeof(INuGenCheckBoxLayoutManager))
			{
				return _checkBoxLayoutManager;
			}
			else if (serviceType == typeof(INuGenCheckBoxRenderer))
			{
				return _checkBoxRenderer;
			}
			else if (serviceType == typeof(INuGenColorsProvider))
			{
				return _colorsProvider;
			}
			else if (serviceType == typeof(INuGenComboBoxRenderer))
			{
				return _comboBoxRenderer;
			}
			else if (serviceType == typeof(INuGenControlImageManager))
			{
				return _controlImageManager;
			}
			else if (serviceType == typeof(INuGenDirectorySelectorRenderer))
			{
				return _directorySelectorRenderer;
			}
			else if (serviceType == typeof(INuGenDropDownRenderer))
			{
				return _dropDownRenderer;
			}
			else if (serviceType == typeof(INuGenImageListService))
			{
				return _imageListService;
			}
			else if (serviceType == typeof(INuGenFontFamiliesProvider))
			{
				return _fontFamiliesProvider;
			}
			else if (serviceType == typeof(INuGenListBoxRenderer))
			{
				return _listBoxRenderer;
			}
			else if (serviceType == typeof(INuGenInt32ValueConverter))
			{
				return new NuGenInt32ValueConverter();
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				return _panelRenderer;
			}
			else if (serviceType == typeof(INuGenProgressBarLayoutManager))
			{
				return _progressBarLayoutManager;
			}
			else if (serviceType == typeof(INuGenProgressBarRenderer))
			{
				return _progressBarRenderer;
			}
			else if (serviceType == typeof(INuGenRadioButtonLayoutManager))
			{
				return _radioButtonLayoutManager;
			}
			else if (serviceType == typeof(INuGenRadioButtonRenderer))
			{
				return _radioButtonRenderer;
			}
			else if (serviceType == typeof(INuGenScrollBarRenderer))
			{
				return _scrollBarRenderer;
			}
			else if (serviceType == typeof(INuGenSpinRenderer))
			{
				return _spinRenderer;
			}
			else if (serviceType == typeof(INuGenSwitchButtonLayoutManager))
			{
				return _switchButtonLayoutManager;
			}
			else if (serviceType == typeof(INuGenSwitchButtonRenderer))
			{
				return _switchButtonRenderer;
			}
			else if (serviceType == typeof(INuGenTabStateService))
			{
				return _tabStateService;
			}
			else if (serviceType == typeof(INuGenTabLayoutManager))
			{
				return _tabLayoutManager;
			}
			else if (serviceType == typeof(INuGenTabRenderer))
			{
				return _tabRenderer;
			}
			else if (serviceType == typeof(INuGenTempImageService))
			{
				return _tempImageService;
			}
			else if (serviceType == typeof(INuGenTextBoxRenderer))
			{
				return _textBoxRenderer;
			}
			else if (serviceType == typeof(INuGenTrackBarRenderer))
			{
				return _trackBarRenderer;
			}
			else if (serviceType == typeof(INuGenThumbnailLayoutManager))
			{
				return _thumbnailLayoutManager;
			}
			else if (serviceType == typeof(INuGenThumbnailRenderer))
			{
				return _thumbnailRenderer;
			}
			else if (serviceType == typeof(INuGenToolStripRenderer))
			{
				return _toolStripRenderer;
			}
			else if (serviceType == typeof(INuGenValueTrackerService))
			{
				return _valueTrackerService;
			}

			return base.GetService(serviceType);
		}

		public PresenterServiceProvider()
		{
			_buttonLayoutManager = new NuGenSmoothButtonLayoutManager();
			_buttonRenderer = new NuGenSmoothButtonRenderer(this);
			_checkBoxLayoutManager = new NuGenCheckBoxLayoutManager();
			_checkBoxRenderer = new NuGenSmoothCheckBoxRenderer(this);
			_colorsProvider = new NuGenColorsProvider();
			_comboBoxRenderer = new NuGenSmoothComboBoxRenderer(this);
			_controlImageManager = new NuGenControlImageManager();
			_imageListService = new NuGenImageListService();
			_directorySelectorRenderer = new NuGenSmoothDirectorySelectorRenderer(this);
			_dropDownRenderer = new NuGenSmoothDropDownRenderer(this);
			_fontFamiliesProvider = new NuGenFontFamiliesProvider();
			_listBoxRenderer = new NuGenSmoothListBoxRenderer(this);
			_panelRenderer = new NuGenSmoothPanelRenderer(this);
			_progressBarLayoutManager = new NuGenProgressBarLayoutManager();
			_progressBarRenderer = new NuGenSmoothProgressBarRenderer(this);
			_radioButtonLayoutManager = new NuGenRadioButtonLayoutManager();
			_radioButtonRenderer = new NuGenSmoothRadioButtonRenderer(this);
			_scrollBarRenderer = new NuGenSmoothScrollBarRenderer(this);
			_spinRenderer = new NuGenSmoothSpinRenderer(this);
			_switchButtonLayoutManager = new NuGenSmoothSwitchButtonLayoutManager();
			_switchButtonRenderer = new NuGenSmoothSwitchButtonRenderer(this);
			_tabStateService = new NuGenTabStateService();
			_tabLayoutManager = new NuGenSmoothTabLayoutManager();
			_tabRenderer = new NuGenSmoothTabRenderer(this);
			_tempImageService = new NuGenTempImageService();
			_textBoxRenderer = new NuGenSmoothTextBoxRenderer(this);
			_trackBarRenderer = new NuGenSmoothTrackBarRenderer(this);
			_thumbnailLayoutManager = new NuGenSmoothThumbnailLayoutManager();
			_thumbnailRenderer = new NuGenSmoothThumbnailRenderer(this);
			_toolStripRenderer = new NuGenSmoothToolStripRenderer();
			_valueTrackerService = new NuGenValueTrackerService();
			_smoothColorManager = new NuGenSmoothColorManager();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (_tempImageService != null)
			{
				_tempImageService.Dispose();
				_tempImageService = null;
			}
		}
	}
}
