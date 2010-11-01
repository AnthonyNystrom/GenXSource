/* -----------------------------------------------
 * NuGenToolTipInfoEditorUI.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenToolTipInfoEditorUI
	{
		private sealed class SizeBlock : UserControl
		{
			private FlowLayoutPanel _layoutPanel;
			private CheckBox _customSizeCheckBox;
			private SizeSpin _widthSpin;
			private SizeSpin _heightSpin;
			private Label _xLabel;

			public Size CustomSize
			{
				get
				{
					if (_customSizeCheckBox.Checked)
					{
						return new Size((int)_widthSpin.Value, (int)_heightSpin.Value);
					}

					return Size.Empty;
				}
				set
				{
					if (value != Size.Empty)
					{
						_customSizeCheckBox.Checked = true;
						_widthSpin.Value = value.Width;
						_heightSpin.Value = value.Height;
					}
				}
			}

			public SizeBlock()
			{
				this.SuspendLayout();

				_widthSpin = new SizeSpin();
				_widthSpin.Enabled = false;

				_xLabel = new Label();
				_xLabel.Height = 24;
				_xLabel.Width = 10;
				_xLabel.Text = Resources.Text_ToolTipInfoEditor_xLabel;
				_xLabel.TextAlign = ContentAlignment.MiddleCenter;
				
				_heightSpin = new SizeSpin();
				_heightSpin.Enabled = false;

				_customSizeCheckBox = new CheckBox();
				_customSizeCheckBox.AutoSize = true;
				_customSizeCheckBox.Text = Resources.Text_ToolTipInfoEditor_customSizeCheckBox;
				_customSizeCheckBox.CheckedChanged += delegate
				{
					_widthSpin.Enabled = _customSizeCheckBox.Checked;
					_heightSpin.Enabled = _customSizeCheckBox.Checked;
				};

				_layoutPanel = new FlowLayoutPanel();
				_layoutPanel.Dock = DockStyle.Fill;
				_layoutPanel.Controls.Add(_customSizeCheckBox);
				_layoutPanel.Controls.Add(_widthSpin);
				_layoutPanel.Controls.Add(_xLabel);
				_layoutPanel.Controls.Add(_heightSpin);
				_layoutPanel.Parent = this;
				_layoutPanel.WrapContents = false;

				this.Dock = DockStyle.Bottom;
				this.Height = 26;

				this.ResumeLayout(false);
			}
		}
	}
}
