/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	partial class ImageExportDialog
	{
		private sealed class Title : UserControl
		{
			public string Description
			{
				get
				{
					return _descriptionLabel.Text;
				}
				set
				{
					_descriptionLabel.Text = value;
				}
			}

			public string Caption
			{
				get
				{
					return _captionLabel.Text;
				}
				set
				{
					_captionLabel.Text = value;
				}
			}

			private static readonly Size _defaultSize = new Size(150, 50);

			protected override Size DefaultSize
			{
				get
				{
					return _defaultSize;
				}
			}

			private NuGenLabel _captionLabel;
			private NuGenLabel _descriptionLabel;
			private Font _captionFont;
			private Font _descriptionFont;

			public Title()
			{
				this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
				this.BackColor = Color.Transparent;
				this.Dock = DockStyle.Top;

				_captionFont = new Font("Verdana", 16, FontStyle.Bold);
				_descriptionFont = new Font("Verdana", 8, FontStyle.Bold);

				_descriptionLabel = new NuGenLabel();
				_descriptionLabel.Dock = DockStyle.Fill;
				_descriptionLabel.Font = _descriptionFont;
				_descriptionLabel.Parent = this;
				_descriptionLabel.Text = "adfadfasdfasdfasdf";

				_captionLabel = new NuGenLabel();
				_captionLabel.Dock = DockStyle.Top;
				_captionLabel.Font = _captionFont;
				_captionLabel.Parent = this;
				_captionLabel.Text = "adfasdf";
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (_captionFont != null)
					{
						_captionFont.Dispose();
						_captionFont = null;
					}

					if (_descriptionFont != null)
					{
						_descriptionFont.Dispose();
						_descriptionFont = null;
					}
				}

				base.Dispose(disposing);
			}
		}
	}
}
