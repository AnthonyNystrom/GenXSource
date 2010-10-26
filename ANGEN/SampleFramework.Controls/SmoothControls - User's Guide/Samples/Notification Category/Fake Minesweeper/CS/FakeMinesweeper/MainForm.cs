using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FakeMinesweeper
{
	public partial class MainForm : Form
	{
		private void _goButton_Click(object sender, EventArgs e)
		{
			_timer.Enabled = true;
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			int hundreds = _counter / 100;
			int tempNum = _counter - hundreds * 100;
			int tens = tempNum / 10;
			tempNum = tempNum - tens * 10;

			_segmentOne.Value = hundreds;
			_segmentTwo.Value = tens;
			_segmentThree.Value = tempNum;

			_counter++;

			if (_counter == 1000)
			{
				_counter = 0;
			}
		}

		private int _counter;

		public MainForm()
		{
			InitializeComponent();
		}
	}
}
