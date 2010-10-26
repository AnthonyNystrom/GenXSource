/* -----------------------------------------------
 * SlideShowSettingsForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.NuGenPresenter.Properties;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenPresenter
{
	internal sealed partial class SlideShowSettingsForm : Form
	{
		public bool RandomSlideAlignment
		{
			get
			{
				return _randomCheckBox.Checked;
			}
			set
			{
				_randomCheckBox.Checked = value;
			}
		}

		public ContentAlignment SlideAlignment
		{
			get
			{
				return _alignSelector.Alignment;
			}
			set
			{
				_alignSelector.Alignment = value;
			}
		}

		public bool SlideFitScreen
		{
			get
			{
				return _fitScreenCheckBox.Checked;
			}
			set
			{
				_fitScreenCheckBox.Checked = value;
			}
		}

		public int SlideLatency
		{
			get
			{
				return _latencySpin.Value;
			}
			set
			{
				_latencySpin.Value = Math.Min(999, Math.Max(1, value));
			}
		}

		public bool StretchSlide
		{
			get
			{
				return _stretchCheckBox.Checked;
			}
			set
			{
				_stretchCheckBox.Checked = value;
			}
		}

		public Color SlideBackgroundColor
		{
			get
			{
				return _colorBox.SelectedColor;
			}
			set
			{
				_colorBox.SelectedColor = value;
			}
		}

		public string SelectedSlidePath
		{
			get
			{
				return _directorySelector.SelectedPath;
			}
			set
			{
				_directorySelector.SelectedPath = value;
			}
		}

		public StringCollection SlidePathCollection
		{
			get
			{
				return _directorySelector.PathCollection;
			}
		}

		private void _stretchCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			_alignSelector.Enabled = _randomCheckBox.Enabled = _fitScreenCheckBox.Enabled = !_stretchCheckBox.Checked;
		}

		public SlideShowSettingsForm()
		{
			this.InitializeComponent();

			this.SetStyle(ControlStyles.Opaque, true);
			this.Text = Resources.Text_SlideShowForm_Text;

			_bkgndLabel.Text = Resources.Text_SlideShowForm_BkgndColor;
			_cancelButton.Text = Resources.Text_Cancel;
			_fitScreenCheckBox.Text = Resources.Text_SlideShowForm_FitScreen;
			_imageAlignLabel.Text = Resources.Text_SlideShowForm_Align;
			_latencyLabel.Text = Resources.Text_SlideShowForm_Latency;
			_okButton.Text = Resources.Text_Ok;
			_randomCheckBox.Text = Resources.Text_SlideShowForm_Random;
			_secondsLabel.Text = Resources.Text_SlideShowForm_Seconds;
			_sourceLabel.Text = Resources.Text_SlideShowForm_Source;
			_stretchCheckBox.Text = Resources.Text_SlideShowForm_Stretch;
		}
	}
}