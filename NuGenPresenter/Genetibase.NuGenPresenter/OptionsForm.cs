/* -----------------------------------------------
 * OptionsForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.NuGenPresenter.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace Genetibase.NuGenPresenter
{
	internal sealed partial class OptionsForm : Form
	{
		private void ReadSettings()
		{
			_drawModeHotKeys.SelectedHotKeys = Settings.Default.DrawHotKeys;
			_zoomModeHotKeys.SelectedHotKeys = Settings.Default.ZoomHotKeys;
			_clearHotKeys.SelectedHotKeys = Settings.Default.ClearHotKeys;
			_escapeHotKeys.SelectedHotKeys = Settings.Default.EscapeHotKeys;
			_saveHotKeys.SelectedHotKeys = Settings.Default.SaveHotKeys;
			_intervalSpin.Value = Settings.Default.Interval;
			_lockTransformHotKeys.SelectedHotKeys = Settings.Default.LockTransformHotKeys;
			_showPointerHotKeys.SelectedHotKeys = Settings.Default.ShowPointerHotKeys;
			_slideShowCheckBox.Checked = Settings.Default.SlideShow;
			_zoomInHotKeys.SelectedHotKeys = Settings.Default.ZoomInHotKeys;
			_zoomOutHotKeys.SelectedHotKeys = Settings.Default.ZoomOutHotKeys;
			_penColorBox.SelectedColor = Settings.Default.PenColor;
			_penWidthCombo.SelectedItem = Settings.Default.PenWidth;

			_randomSlideAlignment = Settings.Default.RandomSlideAlignment;
			_slideFitScreen = Settings.Default.SlideFitScreen;
			_slideBackgroundColor = Settings.Default.SlideBackgroundColor;
			_slideLatency = Settings.Default.SlideLatency;
			_selectedSlidePath = Settings.Default.SelectedSlidePath;
			_slidePathCollection = Settings.Default.SlidePathCollection;
			_stretchSlide = Settings.Default.StretchSlide;
			_slideAlignment = Settings.Default.SlideAlignment;
		}

		private void WriteSettings()
		{
			Settings.Default.DrawHotKeys = _drawModeHotKeys.SelectedHotKeys;
			Settings.Default.ZoomHotKeys = _zoomModeHotKeys.SelectedHotKeys;
			Settings.Default.ClearHotKeys = _clearHotKeys.SelectedHotKeys;
			Settings.Default.EscapeHotKeys = _escapeHotKeys.SelectedHotKeys;
			Settings.Default.SaveHotKeys = _saveHotKeys.SelectedHotKeys;
			Settings.Default.Interval = _intervalSpin.Value;
			Settings.Default.LockTransformHotKeys = _lockTransformHotKeys.SelectedHotKeys;
			Settings.Default.ShowPointerHotKeys = _showPointerHotKeys.SelectedHotKeys;
			Settings.Default.SlideShow = _slideShowCheckBox.Checked;
			Settings.Default.ZoomInHotKeys = _zoomInHotKeys.SelectedHotKeys;
			Settings.Default.ZoomOutHotKeys = _zoomOutHotKeys.SelectedHotKeys;
			Settings.Default.PenColor = _penColorBox.SelectedColor;
			Settings.Default.PenWidth = (int)_penWidthCombo.SelectedItem;

			Settings.Default.RandomSlideAlignment = _randomSlideAlignment;
			Settings.Default.SlideFitScreen = _slideFitScreen;
			Settings.Default.SlideBackgroundColor = _slideBackgroundColor;
			Settings.Default.SlideLatency = _slideLatency;
			Settings.Default.SelectedSlidePath = _selectedSlidePath;
			Settings.Default.SlidePathCollection = _slidePathCollection;
			Settings.Default.StretchSlide = _stretchSlide;
			Settings.Default.SlideAlignment = _slideAlignment;
		}

		private void _okButton_Click(object sender, EventArgs e)
		{
			this.WriteSettings();
			Settings.Default.Save();
		}

		private void _slideShowSettingsButton_Click(object sender, EventArgs e)
		{
			using (SlideShowSettingsForm slideShowForm = new SlideShowSettingsForm())
			{
				if (_slidePathCollection != null)
				{
					foreach (string path in _slidePathCollection)
					{
						slideShowForm.SlidePathCollection.Add(path);
					}
				}

				slideShowForm.RandomSlideAlignment = _randomSlideAlignment;
				slideShowForm.SlideFitScreen = _slideFitScreen;
				slideShowForm.SelectedSlidePath = _selectedSlidePath;
				slideShowForm.SlideBackgroundColor = _slideBackgroundColor;
				slideShowForm.SlideLatency = _slideLatency;
				slideShowForm.StretchSlide = _stretchSlide;
				slideShowForm.SlideAlignment = _slideAlignment;

				if (slideShowForm.ShowDialog() == DialogResult.OK)
				{
					_randomSlideAlignment = slideShowForm.RandomSlideAlignment;
					_slideFitScreen = slideShowForm.SlideFitScreen;
					_slideBackgroundColor = slideShowForm.SlideBackgroundColor;
					_slideLatency = slideShowForm.SlideLatency;
					_selectedSlidePath = slideShowForm.SelectedSlidePath;
					_stretchSlide = slideShowForm.StretchSlide;
					_slideAlignment = slideShowForm.SlideAlignment;

					if (_slidePathCollection != null)
					{
						_slidePathCollection.Clear();
					}
					else
					{
						_slidePathCollection = new StringCollection();
					}

					foreach (string path in slideShowForm.SlidePathCollection)
					{
						_slidePathCollection.Add(path);
					}
				}
			}
		}

		private bool _randomSlideAlignment;
		private Color _slideBackgroundColor;
		private bool _slideFitScreen;
		private int _slideLatency;
		private string _selectedSlidePath;
		private StringCollection _slidePathCollection;
		private bool _stretchSlide;
		private ContentAlignment _slideAlignment;

		public OptionsForm()
		{
			this.InitializeComponent();

			this.SetStyle(ControlStyles.Opaque, true);
			this.Text = Resources.Text_OptionsForm_Text;

			_penWidthCombo.Items.AddRange(new object[] { 1, 2, 3, 4, 5, 6 });

			_drawModeLabel.Text = Resources.Text_OptionsForm_DrawMode;
			_zoomModeLabel.Text = Resources.Text_OptionsForm_ZoomMode;
			_breakTimerGroupBox.Text = Resources.Text_OptionsForm_BreakTimer;
			_cancelButton.Text = Resources.Text_Cancel;
			_clearLabel.Text = Resources.Text_OptionsForm_Clear;
			_escapeLabel.Text = Resources.Text_OptionsForm_Escape;
			_saveLabel.Text = Resources.Text_OptionsForm_Save;
			_drawingGroupBox.Text = Resources.Text_OptionsForm_Drawing;
			_hotKeysGroupBox.Text = Resources.Text_OptionsForm_HotKeys;
			_intervalLabel.Text = Resources.Text_OptionsForm_Interval;
			_lockTransformLabel.Text = Resources.Text_OptionsForm_LockTransform;
			_minutesLabel.Text = Resources.Text_OptionsForm_Minutes;
			_okButton.Text = Resources.Text_Ok;
			_penColorLabel.Text = Resources.Text_OptionsForm_PenColor;
			_penWidthLabel.Text = Resources.Text_OptionsForm_PenWidth;
			_slideShowCheckBox.Text = Resources.Text_OptionsForm_SlideShow;
			_slideShowSettingsButton.Text = Resources.Text_OptionsForm_Settings;
			_showPointerLabel.Text = Resources.Text_OptionsForm_ShowPointer;
			_zoomInLabel.Text = Resources.Text_OptionsForm_ZoomIn;
			_zoomOutLabel.Text = Resources.Text_OptionsForm_ZoomOut;

			this.ReadSettings();
		}
	}
}