using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Cycler
{
	public partial class MainForm : Form
	{
		private void UpdateProgressBarValue(NuGenProgressBar progressBar)
		{
			Debug.Assert(progressBar != null, "progressBar != null");

			if (progressBar.Value == progressBar.Maximum)
			{
				progressBar.Value = progressBar.Minimum;
			}

			progressBar.Value++;
		}

		private void _goButton_Click(object sender, EventArgs e)
		{
			_timer.Enabled = !_timer.Enabled;

			if (_timer.Enabled)
			{
				_goButton.Text = "&Stop";
			}
			else
			{
				_goButton.Text = "&Start";
			}
		}

		private void _horzTrackBar_ValueChanged(object sender, EventArgs e)
		{
			_timer.Interval = 500 / _horzTrackBar.Value;
		}

		private void _scrollBar_ValueChanged(object sender, EventArgs e)
		{
			_vertProgressBar.Value = _scrollBar.Maximum - _scrollBar.Value;
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			this.UpdateProgressBarValue(_horzProgressBar);
			this.UpdateProgressBarValue(_horzProgressBar2);
		}

		public MainForm()
		{
			this.InitializeComponent();
		}
	}
}
