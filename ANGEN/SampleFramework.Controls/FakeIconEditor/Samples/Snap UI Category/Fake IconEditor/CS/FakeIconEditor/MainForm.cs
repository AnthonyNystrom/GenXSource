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
	public partial class MainForm : Form
	{
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			_blendForm.Dispose();
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

		private BlendForm _blendForm;
		private PaletteForm _paletteForm;

		public MainForm()
		{
			this.InitializeComponent();

			_blendForm = new BlendForm();
			_blendForm.Location = new Point(this.Right, this.Top);

			_paletteForm = new PaletteForm();
			_paletteForm.Location = new Point(this.Left - _paletteForm.Width, this.Top);
		}
	}
}
