/* -----------------------------------------------
 * NuGenToolTipInfoEditorUI.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenToolTipInfoEditorUI
	{
		private sealed class HeaderTextBox : TextBox
		{
			public HeaderTextBox()
			{
				this.Dock = DockStyle.Fill;
				this.Font = new Font(this.Font, FontStyle.Bold);
			}
		}
	}
}
