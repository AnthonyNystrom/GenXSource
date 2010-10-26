using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace DotMatrix
{
	public partial class MainForm : Form
	{
		private string GetDateMatrixText()
		{
			return DateTime.Now.ToShortDateString();
		}

		private string GetTimeMatrixText()
		{
			return DateTime.Now.ToLongTimeString();
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			_timeMatrix.Text = this.GetTimeMatrixText();
			_dateMatrix.Text = this.GetDateMatrixText();
		}

		private void _timer2_Tick(object sender, EventArgs e)
		{
			_counterMatrix.Text = (_counter++).ToString("0000", CultureInfo.CurrentUICulture);
			
			if (_counter == 10000)
			{
				_counter = 0;
			}
		}

		private int _counter;

		public MainForm()
		{
			this.InitializeComponent();
			_timeMatrix.Text = this.GetTimeMatrixText();
			_dateMatrix.Text = this.GetDateMatrixText();
			_timer.Enabled = true;
		}
	}
}
