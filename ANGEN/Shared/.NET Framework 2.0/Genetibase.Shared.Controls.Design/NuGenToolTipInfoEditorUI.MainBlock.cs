/* -----------------------------------------------
 * NuGenToolTipInfoEditorUI.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Design.Properties;
using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenToolTipInfoEditorUI
	{
		private sealed class MainBlock : UserControl
		{
			private ContentTextBox _textTextBox;
			private HeaderTextBox _headerTextBox;
			private EditorTableLayoutPanel _layoutPanel;
			private EditorPictureBox _pictureBox;

			public string Header
			{
				get
				{
					return _headerTextBox.Text;
				}
				set
				{
					_headerTextBox.Text = value;
				}
			}

			public new string Text
			{
				get
				{
					return _textTextBox.Text;
				}
				set
				{
					_textTextBox.Text = value;
				}
			}

			public Image Image
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

			private void _pictureBox_Paint(object sender, PaintEventArgs e)
			{
				if (_pictureBox.BackgroundImage == null && _pictureBox.Image == null)
				{
					Graphics g = e.Graphics;
					Rectangle bounds = _pictureBox.ClientRectangle;

					using (StringFormat sf = new StringFormat())
					{
						sf.Alignment = StringAlignment.Center;
						sf.LineAlignment = StringAlignment.Center;

						g.DrawString(
							Resources.Text_ToolTipInfoEditor_EditorPictureBox,
							_pictureBox.Font,
							SystemBrushes.WindowText,
							bounds,
							sf
						);
					}
				}
			}

			public MainBlock(NuGenCustomTypeEditorServiceContext serviceContext)
			{
				if (serviceContext == null)
				{
					throw new ArgumentNullException("serviceContext");
				}

				_pictureBox = new EditorPictureBox(serviceContext);
				_pictureBox.Paint += _pictureBox_Paint;

				_headerTextBox = new HeaderTextBox();
				_headerTextBox.Dock = DockStyle.Fill;
				_headerTextBox.TabIndex = 0;

				_textTextBox = new ContentTextBox();
				_textTextBox.Dock = DockStyle.Fill;
				_textTextBox.TabIndex = 1;

				_layoutPanel = new EditorTableLayoutPanel();
				_layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
				_layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
				_layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
				_layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
				_layoutPanel.Controls.Add(_headerTextBox, 0, 0);
				_layoutPanel.Controls.Add(_pictureBox, 0, 1);
				_layoutPanel.Controls.Add(_textTextBox, 1, 1);
				_layoutPanel.SetColumnSpan(_headerTextBox, 2);
				_layoutPanel.Parent = this;

				this.Dock = DockStyle.Fill;
			}
		}
	}
}
