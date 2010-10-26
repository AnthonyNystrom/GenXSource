using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FakeIconEditor
{
	internal sealed partial class PaletteForm : Form
	{
		public event EventHandler<ColorEventArgs> GradientBeginChanged;

		private void OnGradientBeginChanged(ColorEventArgs e)
		{
			if (this.GradientBeginChanged != null)
			{
				this.GradientBeginChanged(this, e);
			}
		}

		public event EventHandler<ColorEventArgs> GradientEndChanged;

		private void OnGradientEndChanged(ColorEventArgs e)
		{
			if (this.GradientEndChanged != null)
			{
				this.GradientEndChanged(this, e);
			}
		}

		private void _colorSelector_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.OnGradientBeginChanged(new ColorEventArgs(_colorSelector.SelectedColor));
			}
			else if (e.Button == MouseButtons.Right)
			{
				this.OnGradientEndChanged(new ColorEventArgs(_colorSelector.SelectedColor));
			}
		}

		private void _colorSelector_SelectedColorChanged(object sender, EventArgs e)
		{
			_colorHistory.SelectedColor = _colorSelector.SelectedColor;
		}

		public PaletteForm()
		{
			this.InitializeComponent();
		}
	}
}
