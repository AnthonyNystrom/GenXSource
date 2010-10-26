/* -----------------------------------------------
 * NuGenToolTipInfoEditorUI.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenToolTipInfoEditorUI
	{
		private sealed class RemarksBlock : UserControl
		{
			private EditorTableLayoutPanel _layoutPanel;
			private HeaderTextBox _remarksHeaderTextBox;
			private ContentTextBox _remarksTextBox;
			private EditorPictureBox _pictureBox;

			public string RemarksHeader
			{
				get
				{
					return _remarksHeaderTextBox.Text;
				}
				set
				{
					_remarksHeaderTextBox.Text = value;
				}
			}

			public string Remarks
			{
				get
				{
					return _remarksTextBox.Text;
				}
				set
				{
					_remarksTextBox.Text = value;
				}
			}

			public Image RemarksImage
			{
				get
				{
					return _pictureBox.Image;
				}
				set
				{
					_pictureBox.Image = value;
				}
			}

			public RemarksBlock(NuGenCustomTypeEditorServiceContext serviceContext)
			{
				if (serviceContext == null)
				{
					throw new ArgumentNullException("serviceContext");
				}

				_pictureBox = new EditorPictureBox(serviceContext);

				_remarksHeaderTextBox = new HeaderTextBox();
				_remarksHeaderTextBox.Dock = DockStyle.Fill;
				_remarksHeaderTextBox.TabIndex = 0;

				_remarksTextBox = new ContentTextBox();
				_remarksTextBox.Dock = DockStyle.Fill;
				_remarksTextBox.TabIndex = 1;

				_layoutPanel = new EditorTableLayoutPanel();
				_layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30));
				_layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
				_layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
				_layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
				_layoutPanel.Controls.Add(_pictureBox, 0, 0);
				_layoutPanel.Controls.Add(_remarksHeaderTextBox, 1, 0);
				_layoutPanel.Controls.Add(_remarksTextBox, 1, 1);
				_layoutPanel.Parent = this;

				this.Dock = DockStyle.Bottom;
			}
		}
	}
}
