/* -----------------------------------------------
 * FullScreenForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.NuGenPresenter
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal abstract class FullScreenForm : Form
	{
		protected override void OnLostFocus(EventArgs e)
		{
			this.Close();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if ((e.KeyCode & Keys.Escape) != Keys.None)
			{
				this.Close();
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			Cursor.Hide();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Cursor.Show();
		}

		protected FullScreenForm()
		{
			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.WindowState = FormWindowState.Maximized;
		}
	}
}
