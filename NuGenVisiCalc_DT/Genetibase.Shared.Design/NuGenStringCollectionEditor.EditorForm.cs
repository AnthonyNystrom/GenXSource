/* -----------------------------------------------
 * NuGenStringCollectionEditor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	partial class NuGenStringCollectionEditor
	{
		private sealed class EditorForm : CollectionForm
		{
			#region Methods.Protected.Overridden

			/*
			 * OnEditValueChanged
			 */

			/// <summary>
			/// Provides an opportunity to perform processing when a collection value has changed.
			/// </summary>
			protected override void OnEditValueChanged()
			{
				object[] items = base.Items;
				StringBuilder editBoxText = new StringBuilder();

				

				for (int i = 0; i < items.Length; i++)
				{
					if (items[i] is string)
					{
						editBoxText.Append((string)items[i]);

						if (i != (items.Length - 1))
						{
							editBoxText.Append(System.Environment.NewLine);
						}
					}
				}

				_editBox.Text = editBoxText.ToString();
				_editBox.SelectionStart = 0;
				_editBox.SelectionLength = 0;
			}

			#endregion

			#region Methods.Private

			/*
			 * InitializeComponent
			 */

			private void InitializeComponent()
			{
				_editBox = new TextBox();
				_editBoxLabel = new Label();
				_okButton = new Button();
				_cancelButton = new Button();

				this.SuspendLayout();
				this.Padding = new Padding(10);

				_editBox.Multiline = true;
				_editBox.Dock = DockStyle.Fill;
				_editBox.AcceptsTab = true;
				_editBox.AcceptsReturn = true;
				_editBox.ScrollBars = ScrollBars.Both;
				_editBox.KeyDown += _editBox_KeyDown;

				_editBoxLabel.Dock = DockStyle.Top;
				_editBoxLabel.Text = Resources.Text_StringCollectionEditor_editBoxLabel;

				Panel buttonBlock = new Panel();
				buttonBlock.Controls.AddRange(new Control[] { _okButton, _cancelButton });
				buttonBlock.Dock = DockStyle.Bottom;
				buttonBlock.Height = 30;

				_cancelButton.DialogResult = DialogResult.Cancel;
				_cancelButton.Text = Resources.Text_StringCollectionEditor_cancelButton;
				_cancelButton.Top = buttonBlock.Height / 2 - _cancelButton.Height / 2;
				_cancelButton.Left = buttonBlock.Width - _cancelButton.Width;

				_okButton.DialogResult = DialogResult.OK;
				_okButton.Text = Resources.Text_StringCollectionEditor_okButton;
				_okButton.Top = _cancelButton.Top;
				_okButton.Left = _cancelButton.Left - _okButton.Width - 5;
				_okButton.Click += _okButton_Click;

				foreach (Control ctrl in buttonBlock.Controls)
				{
					if (ctrl is Button)
					{
						ctrl.Anchor = AnchorStyles.Right | AnchorStyles.Top;
					}
				}

				this.Controls.AddRange(new Control[] { _editBox, _editBoxLabel, buttonBlock });

				this.CancelButton = _cancelButton;
				this.MaximizeBox = false;
				this.MinimizeBox = false;
				this.MinimumSize = this.Size;
				this.Text = Resources.Text_StringCollectionEditor_EditorForm;
				this.ShowIcon = false;
				this.ShowInTaskbar = false;

				this.ResumeLayout(false);
			}

			#endregion

			#region EventHandlers

			private void _editBox_KeyDown(object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Escape)
				{
					_cancelButton.PerformClick();
					e.Handled = true;
				}
			}

			private void _okButton_Click(object sender, EventArgs e)
			{
				string[] editBoxItems = _editBox.Text.Split(new char[] { '\n' });
				object[] items = base.Items;
				int editBoxItemsLength = editBoxItems.Length;
				
				for (int i = 0; i < editBoxItemsLength; i++)
				{
					editBoxItems[i] = editBoxItems[i].Trim(new char[] { '\r' });
				}
				
				bool shouldMakeChanges = true;
				
				if (editBoxItemsLength == items.Length)
				{
					int currentItemIndex = 0;
					
					while (currentItemIndex < editBoxItemsLength)
					{
						if (!editBoxItems[currentItemIndex].Equals((string)items[currentItemIndex]))
						{
							break;
						}

						currentItemIndex++;
					}
					
					if (currentItemIndex == editBoxItemsLength)
					{
						shouldMakeChanges = false;
					}
				}
				
				if (!shouldMakeChanges)
				{
					base.DialogResult = DialogResult.Cancel;
				}
				else
				{
					if ((editBoxItems.Length > 0) && (editBoxItems[editBoxItems.Length - 1].Length == 0))
					{
						editBoxItemsLength--;
					}
					
					object[] newItems = new object[editBoxItemsLength];
					
					for (int i = 0; i < editBoxItemsLength; i++)
					{
						newItems[i] = editBoxItems[i];
					}
					
					base.Items = newItems;
				}
			}

			#endregion

			private TextBox _editBox;
			private Label _editBoxLabel;
			private Button _okButton;
			private Button _cancelButton;

			/// <summary>
			/// Initializes a new instance of the <see cref="EditorForm"/> class.
			/// </summary>
			public EditorForm(NuGenStringCollectionEditor editor)
				: base(editor)
			{
				this.InitializeComponent();
			}
		}
	}
}
