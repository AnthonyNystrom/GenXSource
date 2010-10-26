/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	partial class ImageExportDialog
	{
		private sealed class LayoutPanel : Panel
		{
			public LayoutPanel()
			{
				this.BackColor = Color.Transparent;
			}
		}
	}
}
