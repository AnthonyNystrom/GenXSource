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
		private sealed class SizeSpin : NumericUpDown
		{
			public SizeSpin()
			{
				this.Minimum = 1;
				this.Maximum = int.MaxValue;
				this.Width = 50;
			}
		}
	}
}
