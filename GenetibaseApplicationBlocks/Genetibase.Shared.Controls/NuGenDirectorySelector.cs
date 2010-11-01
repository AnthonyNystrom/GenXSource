/* -----------------------------------------------
 * NuGenDirectorySelector.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides GUI for directory selection.
	/// </summary>
	[Designer(typeof(NuGenDirectorySelectorDesigner))]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenDirectorySelector : UserControl
	{
		#region Declarations.Components
	
		private System.Windows.Forms.ComboBox pathComboBox;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.FolderBrowserDialog folderDialog;
		private Container components = null;

		#endregion

		#region Properties.Appearance

		/*
		 * Description
		 */

		/// <summary>
		/// Gets or sets the descriptive text displayed above the tree view control in the folder browser dialog box.
		/// </summary>
		[Browsable(true)]
		[Category("Appearance")]
		[Description("Determines the descriptive text displayed above the tree view control in the folder browser dialog box.")]
		public string Description
		{
			get
			{
				return this.folderDialog.Description;
			}
			set
			{
				this.folderDialog.Description = value;
			}
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * Path
		 */

		/// <summary>
		/// Gets or sets the path selected by user. Returns <see langword="null"/> if this <see cref="T:NuGenDirectorySelector"/>
		/// contains no path.
		/// </summary>
		[DefaultValue("")]
		public string Path
		{
			get 
			{
				return this.pathComboBox.Text;
			}
			set
			{
				if (this.pathComboBox.Text != value)
				{
					this.pathComboBox.Text = value;
					this.OnPathChanged(EventArgs.Empty);
				}

				if (!this.ContainsItem(this.pathComboBox, value) && value != "" && value != null) 
				{
					this.pathComboBox.Items.Insert(0, value);	
				}
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenDirectorySelector.PathChanged"/> event identifier.
		/// </summary>
		private static readonly object EventPathChanged = new object();

		/// <summary>
		/// Occurs when the path entered by user changes.
		/// </summary>
		[Browsable(true)]
		[Category("Property Changed")]
		[Description("Occurs when the path entered by user changes.")]
		public event EventHandler PathChanged
		{
			add 
			{
				this.Events.AddHandler(EventPathChanged, value);
			}
			remove 
			{
				this.Events.RemoveHandler(EventPathChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenDirectorySelector.PathChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnPathChanged(EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[EventPathChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Methods.Public.Common

		/*
		 * ClearHistory
		 */

		/// <summary>
		/// Clears the path history.
		/// </summary>
		public virtual void ClearHistory()
		{
			this.pathComboBox.Items.Clear();
		}

		/*
		 * Reset
		 */

		/// <summary>
		/// Resets the folder browser dialog properties to their default values.
		/// </summary>
		public void Reset()
		{
			this.folderDialog.Reset();
		}

		#endregion

		#region Methods.Private

		private bool ContainsItem(ComboBox comboBox, object item)
		{
			Debug.Assert(comboBox != null, "comboBox != null");

			foreach (object comboBoxItem in comboBox.Items) 
			{
				if (item == comboBoxItem) 
				{
					return true;
				}
			}

			return false;
		}

		#endregion

		#region EventHandlers

		private void browseButton_Click(object sender, System.EventArgs e)
		{
			if (this.folderDialog.ShowDialog() == DialogResult.OK) 
			{
				this.Path = this.folderDialog.SelectedPath;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDirectorySelector"/> class.
		/// </summary>
		public NuGenDirectorySelector()
		{
			InitializeComponent();
		}
		
		#endregion

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null) components.Dispose();
			}
			
			base.Dispose(disposing);
		}
		
		#endregion
		
		#region Component Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pathComboBox = new System.Windows.Forms.ComboBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// pathComboBox
			// 
			this.pathComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pathComboBox.Location = new System.Drawing.Point(0, 2);
			this.pathComboBox.Name = "pathComboBox";
			this.pathComboBox.Size = new System.Drawing.Size(211, 21);
			this.pathComboBox.TabIndex = 0;
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.browseButton.Location = new System.Drawing.Point(216, 1);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(32, 23);
			this.browseButton.TabIndex = 1;
			this.browseButton.Text = "&...";
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// NuGenDirectorySelector
			// 
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.pathComboBox);
			this.Name = "NuGenDirectorySelector";
			this.Size = new System.Drawing.Size(250, 25);
			this.ResumeLayout(false);

		}
		
		#endregion
	}
}
