using res = FirefoxTabControl.Properties.Resources;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace FirefoxTabControl
{
	public partial class MainForm : Form
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.AddTabPage();
		}

		private void AddTabPage()
		{
			_tabControl.TabPages.Add(String.Format(CultureInfo.CurrentUICulture, "Tab {0}", ++_tabPageCount)).TabButtonImage = res.Blank;
		}

		private void _addTabButton_Click(object sender, EventArgs e)
		{
			this.AddTabPage();
		}

		private Int32 _tabPageCount;

		public MainForm()
		{
			this.InitializeComponent();
		}
	}
}
