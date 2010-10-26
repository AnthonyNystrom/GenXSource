/* -----------------------------------------------
 * NuGenFileSelector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	partial class NuGenFileSelector
	{
		private void InitializeComponent()
		{
			_pathComboBox = new System.Windows.Forms.ComboBox();
			_browseButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pathComboBox
			// 
			_pathComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			_pathComboBox.Location = new System.Drawing.Point(0, 2);
			_pathComboBox.Name = "pathComboBox";
			_pathComboBox.Size = new System.Drawing.Size(211, 21);
			_pathComboBox.TabIndex = 0;
			// 
			// browseButton
			// 
			_browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			_browseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			_browseButton.Location = new System.Drawing.Point(217, 1);
			_browseButton.Name = "browseButton";
			_browseButton.Size = new System.Drawing.Size(32, 23);
			_browseButton.TabIndex = 1;
			_browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// NuGenFileSelector
			// 
			this.Controls.Add(_browseButton);
			this.Controls.Add(_pathComboBox);
			this.Size = new System.Drawing.Size(250, 25);
			this.ResumeLayout(false);
		}

		private System.Windows.Forms.ComboBox _pathComboBox;
		private System.Windows.Forms.Button _browseButton;
	}
}
