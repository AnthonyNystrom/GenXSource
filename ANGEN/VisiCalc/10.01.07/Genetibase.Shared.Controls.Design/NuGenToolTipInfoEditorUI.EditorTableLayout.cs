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
		[System.ComponentModel.DesignerCategory("Code")]
		private sealed class EditorTableLayoutPanel : TableLayoutPanel
		{
			public EditorTableLayoutPanel()
			{
				this.Dock = DockStyle.Fill;
			}
		}
	}
}
