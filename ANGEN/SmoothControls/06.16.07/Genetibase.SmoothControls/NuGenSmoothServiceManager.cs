/* -----------------------------------------------
 * NuGenSmoothServiceManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.SmoothControls.AlignSelectorInternals;
using Genetibase.SmoothControls.ButtonInternals;
using Genetibase.SmoothControls.CalculatorInternals;
using Genetibase.SmoothControls.CalendarInternals;
using Genetibase.SmoothControls.CheckBoxInternals;
using Genetibase.SmoothControls.ColorBoxInternals;
using Genetibase.SmoothControls.ComboBoxInternals;
using Genetibase.SmoothControls.ListBoxInternals;
using Genetibase.SmoothControls.DirectorySelectorInternals;
using Genetibase.SmoothControls.DropDownInternals;
using Genetibase.SmoothControls.FontBoxInternals;
using Genetibase.SmoothControls.GroupBoxInternals;
using Genetibase.SmoothControls.HotKeySelectorInternals;
using Genetibase.SmoothControls.NavigationBarInternals;
using Genetibase.SmoothControls.PanelExInternals;
using Genetibase.SmoothControls.PanelInternals;
using Genetibase.SmoothControls.PictureBoxInternals;
using Genetibase.SmoothControls.PinpointInternals;
using Genetibase.SmoothControls.PopupContainerInternals;
using Genetibase.SmoothControls.PrintPreviewInternals;
using Genetibase.SmoothControls.ProgressBarInternals;
using Genetibase.SmoothControls.PropertyGridInternals;
using Genetibase.SmoothControls.RadioButtonInternals;
using Genetibase.SmoothControls.ScrollBarInternals;
using Genetibase.SmoothControls.SpinInternals;
using Genetibase.SmoothControls.SplitButtonInternals;
using Genetibase.SmoothControls.SplitContainerInternals;
using Genetibase.SmoothControls.SwitcherInternals;
using Genetibase.SmoothControls.TabControlInternals;
using Genetibase.SmoothControls.TaskBoxInternals;
using Genetibase.SmoothControls.TextBoxInternals;
using Genetibase.SmoothControls.TitleInternals;
using Genetibase.SmoothControls.TitlePanelInternals;
using Genetibase.SmoothControls.ToolStripInternals;
using Genetibase.SmoothControls.ToolTipInternals;
using Genetibase.SmoothControls.TrackBarInternals;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Service provider manager.
	/// </summary>
	internal static class NuGenSmoothServiceManager
	{
		public static readonly INuGenServiceProvider AlignDropDownServiceProvider = new NuGenSmoothAlignDropDownServiceProvider();
		public static readonly INuGenServiceProvider AlignSelectorServiceProvider = new NuGenSmoothAlignSelectorServiceProvider();
		public static readonly INuGenServiceProvider ButtonServiceProvider = new NuGenSmoothButtonServiceProvider();
		public static readonly INuGenServiceProvider CalculatorDropDownServiceProvider = new NuGenSmoothCalculatorDropDownServiceProvider();
		public static readonly INuGenServiceProvider CalculatorPopupServiceProvider = new NuGenSmoothCalculatorPopupServiceProvider();
		public static readonly INuGenServiceProvider CalendarServiceProvider = new NuGenSmoothCalendarServiceProvider();
		public static readonly INuGenServiceProvider CheckBoxServiceProvider = new NuGenSmoothCheckBoxServiceProvider();
		public static readonly INuGenServiceProvider ColorBoxServiceProvider = new NuGenSmoothColorBoxServiceProvider();
		public static readonly INuGenServiceProvider ColorBoxPopupServiceProvider = new NuGenSmoothColorBoxPopupServiceProvider();
		public static readonly INuGenServiceProvider ComboBoxServiceProvider = new NuGenSmoothComboBoxServiceProvider();
		public static readonly INuGenServiceProvider DirectorySelectorServiceProvider = new NuGenSmoothDirectorySelectorServiceProvider();
		public static readonly INuGenServiceProvider DropDownServiceProvider = new NuGenSmoothDropDownServiceProvider();
		public static readonly INuGenServiceProvider FontBoxServiceProvider = new NuGenSmoothFontBoxServiceProvider();
		public static readonly INuGenServiceProvider GroupBoxServiceProvider = new NuGenSmoothGroupBoxServiceProvider();
		public static readonly INuGenServiceProvider HotKeyPopupServiceProvider = new NuGenSmoothHotKeySelectorServiceProvider();
		public static readonly INuGenServiceProvider ListBoxServiceProvider = new NuGenSmoothListBoxServiceProvider();
		public static readonly INuGenServiceProvider MenuButtonServiceProvider = new NuGenSmoothMenuButtonServiceProvider();
		public static readonly INuGenServiceProvider NavigationBarServiceProvider = new NuGenSmoothNavigationBarServiceProvider();
		public static readonly INuGenServiceProvider PanelExServiceProvider = new NuGenSmoothPanelExServiceProvider();
		public static readonly INuGenServiceProvider PanelServiceProvider = new NuGenSmoothPanelServiceProvider();
		public static readonly INuGenServiceProvider PictureBoxServiceProvider = new NuGenSmoothPictureBoxServiceProvider();
		public static readonly INuGenServiceProvider PinpointServiceProvider = new NuGenSmoothPinpointServiceProvider();
		public static readonly INuGenServiceProvider PopupContainerServiceProvider = new NuGenSmoothPopupContainerServiceProvider();
		public static readonly INuGenServiceProvider PrintPreviewServiceProvider = new NuGenSmoothPrintPreviewServiceProvider();
		public static readonly INuGenServiceProvider ProgressBarServiceProvider = new NuGenSmoothProgressBarServiceProvider();
		public static readonly INuGenServiceProvider PropertyGridServiceProvider = new NuGenSmoothPropertyGridServiceProvider();
		public static readonly INuGenServiceProvider RadioButtonServiceProvider = new NuGenSmoothRadioButtonServiceProvider();
		public static readonly INuGenServiceProvider RoundedPanelExServiceProvider = new NuGenSmoothRoundedPanelExServiceProvider();
		public static readonly INuGenServiceProvider ScrollBarServiceProvider = new NuGenSmoothScrollBarServiceProvider();
		public static readonly INuGenServiceProvider SmoothServiceProvider = new NuGenSmoothServiceProvider();
		public static readonly INuGenServiceProvider SpinServiceProvider = new NuGenSmoothSpinServiceProvider();
		public static readonly INuGenServiceProvider SplitButtonServiceProvider = new NuGenSmoothSplitButtonServiceProvider();
		public static readonly INuGenServiceProvider SplitContainerServiceProvider = new NuGenSmoothSplitContainerServiceProvider();
		public static readonly INuGenServiceProvider SwitcherServiceProvider = new NuGenSmoothSwitcherServiceProvider();
		public static readonly INuGenServiceProvider TabControlServiceProvider = new NuGenSmoothTabControlServiceProvider();
		public static readonly INuGenServiceProvider TaskBoxServiceProvider = new NuGenSmoothTaskBoxServiceProvider();
		public static readonly INuGenServiceProvider TextBoxServiceProvider = new NuGenSmoothTextBoxServiceProvider();
		public static readonly INuGenServiceProvider TitleServiceProvider = new NuGenSmoothTitleServiceProvider();
		public static readonly INuGenServiceProvider TitlePanelServiceProvider = new NuGenSmoothTitlePanelServiceProvider();
		public static readonly INuGenServiceProvider ToolStripServiceProvider = new NuGenSmoothToolStripServiceProvider();
		public static readonly INuGenServiceProvider ToolTipServiceProvider = new NuGenSmoothToolTipServiceProvider();
		public static readonly INuGenServiceProvider TrackBarServiceProvider = new NuGenSmoothTrackBarServiceProvider();
	}
}
