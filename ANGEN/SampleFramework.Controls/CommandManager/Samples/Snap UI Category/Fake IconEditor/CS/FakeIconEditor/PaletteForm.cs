using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FakeIconEditor
{
	public partial class PaletteForm : Form
	{
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
