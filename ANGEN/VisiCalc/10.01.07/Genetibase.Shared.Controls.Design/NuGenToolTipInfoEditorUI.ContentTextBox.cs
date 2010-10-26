/* -----------------------------------------------
 * NuGenToolTipInfoEditorUI.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
