/* -----------------------------------------------
 * NuGenMultilineEditorUI.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design.Properties;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides a multiline text editor UI.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	internal sealed partial class NuGenMultilineEditorUI : UserControl
	{
		#region Declarations.Fields

		private TextBox _editBox = new TextBox();
		private ToolTip _toolTip = new ToolTip();

		#endregion

		#region Properties.Public

		/// <summary>
		/// Gets the text entered by the user.
		/// </summary>
		public string GetValue
		{
			get
			{
				return _editBox.Text;
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * InitializeEditBox
		 */

		/// <summary>
		/// </summary>
		/// <param name="editBoxToInitialize"></param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="editBoxToInitialize"/> is <see langword="null"/>.
		/// </exception>
		private void InitializeEditBox(TextBox editBoxToInitialize)
		{
			if (editBoxToInitialize == null)
			{
				throw new ArgumentNullException("editBoxToInitialize");
			}

			this.SuspendLayout();

			editBoxToInitialize.Dock = DockStyle.Fill;
			editBoxToInitialize.Location = new Point(0, 0);
			editBoxToInitialize.Multiline = true;
			editBoxToInitialize.Size = new Size(150, 150);
			editBoxToInitialize.TabIndex = 1;
			editBoxToInitialize.Text = string.Empty;

			this.ResumeLayout(false);
		}

		/*
		 * InitializeToolTip
		 */

		/// <summary>
		/// </summary>
		/// <param name="toolTipToInitialize"></param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="toolTipToInitialize"/> is <see langword="null"/>.
		/// </exception>
		private void InitializeToolTip(ToolTip toolTipToInitialize)
		{
			if (toolTipToInitialize == null)
			{
				throw new ArgumentNullException("toolTipToInitialize");
			}

			_components.Add(toolTipToInitialize);
			toolTipToInitialize.SetToolTip(_editBox, Resources.ToolTip_MultilineUI_EditBox);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMultilineEditorUI"/> class.
		/// </summary>
		public NuGenMultilineEditorUI()
		{
			this.InitializeComponent();
			this.InitializeEditBox(_editBox);
			this.InitializeToolTip(_toolTip);
			
			_editBox.Parent = this;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMultilineEditorUI"/> class.
		/// </summary>
		/// <param name="text">Default text.</param>
		public NuGenMultilineEditorUI(string text)
			: this()
		{
			_editBox.Text = text;
		}

		#endregion
	}
}
