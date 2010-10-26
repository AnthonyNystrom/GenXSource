using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FakeIconEditor
{
	internal sealed partial class BlendForm : Form
	{
		public event EventHandler<ColorEventArgs> BlendColorChanged;

		private void OnBlendColorChanged(ColorEventArgs e)
		{
			if (this.BlendColorChanged != null)
			{
				this.BlendColorChanged(this, e);
			}
		}

		public void SetGradientBegin(Color gradientBegin)
		{
			_blendSelector.UpperColor = gradientBegin;
		}

		public void SetGradientEnd(Color gradientEnd)
		{
			_blendSelector.LowerColor = gradientEnd;
		}

		private void _blendSelector_ColorChanged(object sender, EventArgs e)
		{
			this.OnBlendColorChanged(new ColorEventArgs(_blendSelector.SelectedColor));
		}

		public BlendForm()
		{
			this.InitializeComponent();
		}
	}
}
