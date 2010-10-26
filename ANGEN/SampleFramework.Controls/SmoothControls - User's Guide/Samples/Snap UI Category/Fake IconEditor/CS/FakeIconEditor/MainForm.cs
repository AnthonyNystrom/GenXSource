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
	internal sealed partial class MainForm : Form
	{
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			_blendForm.BlendColorChanged -= _blendForm_BlendColorChanged;
			_blendForm.Dispose();

			_paletteForm.GradientBeginChanged -= _paletteForm_GradientBeginChanged;
			_paletteForm.GradientEndChanged -= _paletteForm_GradientEndChanged;
			_paletteForm.Dispose();

			NuGenUISnap.UnregisterExternalReferenceForm(this);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			NuGenUISnap.RegisterExternalReferenceForm(this);
			_blendForm.Show();
			_paletteForm.Show();
		}

		private void _blendForm_BlendColorChanged(Object sender, ColorEventArgs e)
		{
			_colorPanel.SelectedColor = e.Color;
		}

		private void _paletteForm_GradientBeginChanged(Object sender, ColorEventArgs e)
		{
			_blendForm.SetGradientBegin(e.Color);
		}

		private void _paletteForm_GradientEndChanged(Object sender, ColorEventArgs e)
		{
			_blendForm.SetGradientEnd(e.Color);
		}

		private BlendForm _blendForm;
		private PaletteForm _paletteForm;

		public MainForm()
		{
			this.InitializeComponent();

			_blendForm = new BlendForm();
			_blendForm.BlendColorChanged += _blendForm_BlendColorChanged;
			_blendForm.Location = new Point(this.Right, this.Top);

			_paletteForm = new PaletteForm();
			_paletteForm.GradientBeginChanged += _paletteForm_GradientBeginChanged;
			_paletteForm.GradientEndChanged += _paletteForm_GradientEndChanged;
			_paletteForm.Location = new Point(this.Left - _paletteForm.Width, this.Top);
		}
	}
}
