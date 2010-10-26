/* -----------------------------------------------
 * NuGenCollectionEditorDialogBlock.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Represents a panel with Ok and Cancel buttons.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenCollectionEditorDialogBlock : Panel
	{
		private Button _okButton;
		private Button _cancelButton;

		/// <summary>
		/// </summary>
		public Button GetOkButton()
		{
			return _okButton;
		}

		/// <summary>
		/// </summary>
		public Button GetCancelButton()
		{
			return _cancelButton;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCollectionEditorDialogBlock"/> class.
		/// </summary>
		public NuGenCollectionEditorDialogBlock()
		{
			_okButton = new Button();
			_cancelButton = new Button();
			
			_okButton.DialogResult = DialogResult.OK;
			_okButton.TabIndex = 10;
			_okButton.Text = Resources.Text_CollectionEditor_okButton;
			
			_cancelButton.DialogResult = DialogResult.Cancel;
			_cancelButton.TabIndex = 20;
			_cancelButton.Text = Resources.Text_CollectionEditor_cancelButton;

			this.SuspendLayout();

			this.Controls.Add(_okButton);
			this.Controls.Add(_cancelButton);

			this.Height = 29;
			this.Padding = new Padding(0, 5, 0, 0);

			foreach (Control ctrl in this.Controls)
			{
				ctrl.Dock = DockStyle.Right;
			}

			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
