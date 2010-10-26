using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CtrlGallery
{
	public partial class MainForm : Form
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_switcher.SelectedIndex = 0;
		}

		public MainForm()
		{
			this.InitializeComponent();
		}
	}
}
