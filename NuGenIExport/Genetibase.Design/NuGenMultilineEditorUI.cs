/* -----------------------------------------------
 * NuGenMultilineEditorUI.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Design.Properties;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Design
{
	/// <summary>
	/// Provides a multiline text editor UI.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	public partial class NuGenMultilineEditorUI : UserControl
	{
		#region Properties.Public

		/// <summary>
		/// Gets the text entered by the user.
		/// </summary>
		public string GetValue
		{
			get
			{
				return this.EditBox.Text;
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * EditBox
		 */
		
		private TextBox _editBox = null;

		/// <summary>
		/// </summary>
		protected TextBox EditBox
		{
			get
			{
				if (_editBox == null)
				{
					_editBox = new TextBox();
				}

				return _editBox;
			}
		}

		/*
		 * ToolTip
		 */

		private ToolTip _toolTip = null;

		/// <summary>
		/// </summary>
		protected ToolTip ToolTip
		{
			get
			{
				if (_toolTip == null)
				{
					Debug.Assert(_components != null, "_components != null");
					_toolTip = new ToolTip(_components);
				}

				return _toolTip;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * InitializeEditBox
		 */

		/// <summary>
		/// </summary>
		/// <param name="editBoxToInitialize"></param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="editBoxToInitialize"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void InitializeEditBox(TextBox editBoxToInitialize)
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
			editBoxToInitialize.Text = "";

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
		protected virtual void InitializeToolTip(ToolTip toolTipToInitialize)
		{
			if (toolTipToInitialize == null)
			{
				throw new ArgumentNullException("toolTipToInitialize");
			}

			toolTipToInitialize.SetToolTip(this.EditBox, Resources.ToolTip_MultilineUI_EditBox);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMultilineEditorUI"/> class.
		/// </summary>
		public NuGenMultilineEditorUI()
		{
			this.InitializeComponent();
			this.InitializeEditBox(this.EditBox);
			this.InitializeToolTip(this.ToolTip);
			
			this.EditBox.Parent = this;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMultilineEditorUI"/> class.
		/// </summary>
		/// <param name="text">Default text.</param>
		public NuGenMultilineEditorUI(string text)
			: this()
		{
			this.EditBox.Text = text;
		}

		#endregion
	}
}
