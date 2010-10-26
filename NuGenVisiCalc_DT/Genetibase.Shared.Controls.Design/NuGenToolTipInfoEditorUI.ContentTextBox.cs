/* -----------------------------------------------
 * NuGenToolTipInfoEditorUI.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenToolTipInfoEditorUI
	{
		private sealed class ContentTextBox : TextBox
		{
			public ContentTextBox()
			{
				this.Dock = DockStyle.Fill;
				this.Multiline = true;
				this.ScrollBars = ScrollBars.Vertical;
			}
		}
	}
}
