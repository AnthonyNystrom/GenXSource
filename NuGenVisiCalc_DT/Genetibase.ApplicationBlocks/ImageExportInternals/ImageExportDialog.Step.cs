/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		internal sealed class Step : UserControl
		{
			private string _caption;

			public string Caption
			{
				get
				{
					return _caption;
				}
			}

			private string _description;

			public string Description
			{
				get
				{
					return _description;
				}
			}

			public Step(string caption, string description)
			{
				_caption = caption;
				_description = description;

				this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

				this.BackColor = Color.Transparent;
				this.Dock = DockStyle.Fill;
				this.Visible = false;
			}
		}
	}
}
