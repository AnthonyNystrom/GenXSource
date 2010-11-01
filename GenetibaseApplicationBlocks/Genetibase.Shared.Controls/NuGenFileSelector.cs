/* -----------------------------------------------
 * NuGenFileSelector.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Design;
using Genetibase.Shared;
using Genetibase.Shared.Xml;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides base functionality for path selection.
	/// </summary>
	[Designer(typeof(NuGenFileSelectorDesigner))]
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract class NuGenFileSelector : UserControl
	{
		#region Declarations.Components
	
		private Container components = null;
		private System.Windows.Forms.ComboBox pathComboBox;
		private System.Windows.Forms.Button browseButton;

		#endregion

		#region Declarations.Consts

		private const string CONFIG_PATH_ELEMENT = "Path";
		private const string CONFIG_GUID_ATTRIBUTE = "Guid";

		#endregion

		#region Properties.Behavior

		/*
		 * AddExtension
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the <see cref="P:NuGenFileSelector.FileDialog"/> automatically
		/// adds an extension to a file name if the user omits the extension.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Indicates whether the file dialog automatically adds an extension to a file if the user omits the extension.")]
		public bool AddExtension
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.AddExtension;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.AddExtension = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.AddExtension"/> property.
		/// </summary>
		protected virtual bool DefaultAddExtension
		{
			[DebuggerStepThrough]
			get 
			{
				return true;
			}
		}

		private bool ShouldSerializeAddExtension()
		{
			return this.AddExtension != this.DefaultAddExtension;
		}

		/*
		 * CheckFileExists
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the <see cref="P:NuGenFileSelector.FileDialog"/> displays a warning
		/// if the user specifies the name of the file that does not exist.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Indicates whether the file dialog displays a warning if the user specifies the name of the file that does not exist.")]		
		public bool CheckFileExists
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.CheckFileExists;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.CheckFileExists = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.CheckFileExists"/> property.
		/// </summary>
		protected virtual bool DefaultCheckFileExists
		{
			[DebuggerStepThrough]
			get 
			{
				return true;
			}
		}

		private bool ShouldSerializeCheckFileExists()
		{
			return this.CheckFileExists != this.DefaultCheckFileExists;
		}

		/*
		 * CheckPathExists
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the <see cref="P:NuGenFileSelector.FileDialog"/> displays a warning
		/// if the user specifies a path that does not exist.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Indicates whether the file dialog displays a warning if the user specifies a path that does not exist.")]
		public bool CheckPathExists
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.CheckPathExists;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.CheckPathExists = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.CheckPathExists"/> property.
		/// </summary>
		protected virtual bool DefaultCheckPathExists
		{
			[DebuggerStepThrough]
			get 
			{
				return true;
			}
		}

		private bool ShouldSerializeCheckPathExists()
		{
			return this.CheckPathExists != this.DefaultCheckPathExists;
		}

		/*
		 * DefaultExtension
		 */

		/// <summary>
		/// Gets or sets the default file name extension.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Default file name extension.")]
		public string DefaultExtension
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.DefaultExt;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.DefaultExt = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="T:NuGenFileSelector.DefaultExtension"/> property.
		/// </summary>
		protected virtual string DefaultDefaultExtension
		{
			[DebuggerStepThrough]
			get 
			{
				return "";
			}
		}

		private bool ShouldSerializeDefaultExtension()
		{
			return this.DefaultExtension != this.DefaultDefaultExtension;
		}

		/*
		 * DereferenceLinks
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the <see cref="P:NuGenFileSelector.FileDialog"/> returns the location
		/// of the file referenced by the shortcut or whether it returns the location of the shortcut (*.lnk).
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Indicates whether the file dialog returns the location of the file referenced by the shortcut or whether it returns the location of the shortcut (*.lnk).")]
		public bool DereferenceLinks
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.DereferenceLinks;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.DereferenceLinks = value; 
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.DereferenceLinks"/> property.
		/// </summary>
		protected virtual bool DefaultDereferenceLinks
		{
			[DebuggerStepThrough]
			get 
			{
				return true;
			}
		}

		private bool ShouldSerializeDereferenceLinks()
		{
			return this.DereferenceLinks != this.DefaultDereferenceLinks;
		}

		/*
		 * Filter
		 */

		/// <summary>
		/// Gets or sets the current file name filter string, which determines the choices that appear
		/// in the "Files of type" box in the <see cref="P:NuGenFileSelector.FileDialog"/>.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("The current file name filter string, which determines the choices that appear in the \"Files of type\" box in the file dialog.")]
		public string Filter
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.Filter;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.Filter = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.Filter"/> property.
		/// </summary>
		protected virtual string DefaultFilter
		{
			[DebuggerStepThrough]
			get 
			{
				return "";
			}
		}

		private bool ShouldSerializeFilter()
		{
			return this.Filter != this.DefaultFilter;
		}

		/*
		 * FilterIndex
		 */

		/// <summary>
		/// Gets or sets the index of the filter currently selected in the <see cref="P:NuGenFileSelector.FileDialog"/>.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("The index of the filter currently selected in the file dialog.")]
		public int FilterIndex
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.FilterIndex;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.FilterIndex = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.FilterIndex"/> property.
		/// </summary>
		protected virtual int DefaultFilterIndex
		{
			[DebuggerStepThrough]
			get 
			{
				return 1;
			}
		}

		private bool ShouldSerializeFilterIndex()
		{
			return this.Filter != this.DefaultFilter;
		}

		/*
		 * InitialDirectory
		 */

		/// <summary>
		/// Gets or sets the initial directory displayed by the <see cref="P:NuGenFileSelector.FileDialog"/>.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Initial directory displayed by the file dialog.")]
		public string InitialDirectory
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.InitialDirectory;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.InitialDirectory = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.InitialDirectory"/> property.
		/// </summary>
		protected virtual string DefaultInitialDirectory
		{
			[DebuggerStepThrough]
			get 
			{
				return "";
			}
		}

		private bool ShouldSerializeInitialDirectory()
		{
			return this.InitialDirectory != this.DefaultInitialDirectory;
		}

		/*
		 * RestoreDirectory
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the <see cref="P:NuGenFileSelector.FileDialog"/> restores the
		/// current directory before closing.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Indicates whether the file dialog restores the current directory before closing.")]
		public bool RestoreDirectory
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.RestoreDirectory;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.RestoreDirectory = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.RestoreDirectory"/> property.
		/// </summary>
		protected virtual bool DefaultRestoreDirectory
		{
			[DebuggerStepThrough]
			get 
			{
				return false;
			}
		}

		private bool ShouldSerializeRestoreDirectory()
		{
			return this.RestoreDirectory != this.DefaultRestoreDirectory;
		}

		/*
		 * ShowHelp
		 */
		
		/// <summary>
		/// Gets or sets the value indicating whether the Help button is displayed in the <see cref="P:NuGenFileSelector.FileDialog"/>.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Indicates whether the Help button is displayed in the file dialog.")]
		public bool ShowHelp
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.ShowHelp;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.ShowHelp = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.ShowHelp"/> property.
		/// </summary>
		protected virtual bool DefaultShowHelp
		{
			[DebuggerStepThrough]
			get 
			{
				return false;
			}
		}

		private bool ShouldSerializeShowHelp()
		{
			return this.ShowHelp != this.DefaultShowHelp;
		}

		/*
		 * Title
		 */

		/// <summary>
		/// Gets or sets the <see cref="P:NuGenFileSelector.FileDialog"/> title.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("File dialog title.")]
		public string Title
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.Title;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.Title = value;
			}
		}

		/*
		 * ValidateNames
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the <see cref="P:NuGenFileSelector.FileDialog"/> accepts only
		/// valid Win32 file names.
		/// </summary>
		[Browsable(true)]
		[Category("Behavior")]
		[Description("Indicates whether the file dialog accepts only valid Win32 file names.")]
		public bool ValidateNames
		{
			[DebuggerStepThrough]
			get 
			{
				return this.FileDialog.ValidateNames;
			}
			[DebuggerStepThrough]
			set 
			{
				this.FileDialog.ValidateNames = value;
			}
		}

		/// <summary>
		/// Gets the default value for the <see cref="P:NuGenFileSelector.ValidateNames"/> property.
		/// </summary>
		protected virtual bool DefaultValidateNames
		{
			[DebuggerStepThrough]
			get 
			{
				return true;
			}
		}

		private bool ShouldSerializeValidateNames()
		{
			return this.ValidateNames != this.DefaultValidateNames;
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * FileDialog
		 */
			
		/// <summary>
		/// Gets the <see cref="P:NuGenFileSelector.FileDialog"/> associated with this <see cref="T:NuGenFileSelector"/>.
		/// </summary>
		protected abstract FileDialog FileDialog
		{
			get;
		}

		/*
		 * Guid
		 */

		/// <summary>
		/// Determines the GUID for this <see cref="T:NuGenFileSelector"/>.
		/// </summary>
		private string guid = System.Guid.NewGuid().ToString();

		/// <summary>
		/// Gets the GUID for this <see cref="T:NuGenFileSelector"/>.
		/// </summary>
		[Browsable(false)]
		public string Guid
		{
			[DebuggerStepThrough]
			get 
			{
				return this.guid;
			}
			[DebuggerStepThrough]
			set 
			{
				this.guid = value;
			}
		}

		/*
		 * Path
		 */

		/// <summary>
		/// Gets the path selected by user. Returns <see langword="null"/> if this <see cref="T:NuGenFileSelector"/>
		/// contains no path.
		/// </summary>
		public string GetPath
		{
			get 
			{
				return this.pathComboBox.Items.Count > 0 ? this.pathComboBox.Items[0].ToString() : "";
			}
		}

		/// <summary>
		/// Sets the path selected by user.
		/// </summary>
		protected void SetPath(string value)
		{
			if (!this.ContainsItem(this.pathComboBox, value)) 
			{
				this.pathComboBox.Items.Insert(0, value);
				this.pathComboBox.Text = value;
				this.OnPathChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The <see cref="E:NuGenFileSelector.PathChanged"/> event identifier.
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
		/// Raises the <see cref="E:NuGenFileSelector.PathChanged"/> event.
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
		/// Resets the <see cref="P:NuGenFileSelector.FileDialog"/> properties to their default values.
		/// </summary>
		public void Reset()
		{
			this.FileDialog.Reset();
		}

		#endregion

		#region Methods.Public.LoadSave

		/*
		 * LoadConfig
		 */

		/// <summary>
		/// Loads state from the specified <see cref="T:Stream"/>.
		/// </summary>
		/// <param name="stream">Specifies the <see cref="T:Stream"/> to load state from.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
		public void LoadConfig(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

			NuGenXmlTextReader xmlTextReader = new NuGenXmlTextReader(stream);
			this.LoadConfig(xmlTextReader);
			xmlTextReader.Close();
		}

		/// <summary>
		/// Loads state from the specified path.
		/// </summary>
		/// <param name="filePath">Specifies the path to load state from.</param>
		public void LoadConfig(string filePath)
		{
			if (File.Exists(filePath)) 
			{
				using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
				{
					this.LoadConfig(fileStream);
				}
			}
		}

		/// <summary>
		/// Loads state from the specified <see cref="T:XmlTextReader"/>.
		/// </summary>
		/// <param name="xmlTextReader">Specifies the <see cref="T:XmlTextReader"/> to load state from.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="xmlTextReader"/> is <see langword="null"/>.</exception>
		public void LoadConfig(XmlTextReader xmlTextReader)
		{
			if (xmlTextReader == null)
			{
				throw new ArgumentNullException("xmlTextReader");
			}

			this.pathComboBox.Items.Clear();

			if (
				xmlTextReader.IsStartElement() &&
				xmlTextReader.Name == this.GetType().Name &&
				XmlConvert.DecodeName(xmlTextReader.GetAttribute(CONFIG_GUID_ATTRIBUTE)) == this.Guid
				)
			{
				while (xmlTextReader.Read()) 
				{
					if (!xmlTextReader.IsStartElement()) 
					{
						break;
					}

					if (xmlTextReader.Name == CONFIG_PATH_ELEMENT) 
					{
						this.pathComboBox.Items.Add(xmlTextReader.ReadString());
					}
				}
			}

			if (this.pathComboBox.Items.Count > 0) 
			{
				this.pathComboBox.Text = this.pathComboBox.Items[0].ToString();
			}
		}

		/*
		 * SaveConfig
		 */

		/// <summary>
		/// Saves the current state to the specified <see cref="T:Stream"/>.
		/// </summary>
		/// <param name="stream">Specifies the <see cref="T:Stream"/> to save the current state to.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
		public void SaveConfig(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

			NuGenXmlTextWriter xmlTextWriter = new NuGenXmlTextWriter(stream);
			
			xmlTextWriter.WriteStartDocument(true);
			this.SaveConfig(xmlTextWriter);
			xmlTextWriter.WriteEndDocument();
			
			xmlTextWriter.Flush();
		}

		/// <summary>
		/// Saves the current state to the specified path.
		/// </summary>
		/// <param name="filePath">Specifies the path to save the current state to.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="filePath"/> is <see langword="null"/>.</exception>
		public void SaveConfig(string filePath)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}

			using (FileStream stream = new FileStream(filePath, FileMode.Create))
			{
				this.SaveConfig(stream);
			}
		}

		/// <summary>
		/// Saves the current state to the specified <see cref="T:XmlTextWriter"/>.
		/// </summary>
		/// <param name="xmlTextWriter">Specifies the <see cref="T:XmlTextWriter"/> to save the current state to.</param>
		public void SaveConfig(XmlTextWriter xmlTextWriter)
		{
			if (xmlTextWriter == null)
			{
				throw new ArgumentNullException("xmlTextWriter");
			}

			xmlTextWriter.WriteStartElement(this.GetType().Name);
			xmlTextWriter.WriteAttributeString(CONFIG_GUID_ATTRIBUTE, XmlConvert.EncodeName(this.Guid));

			foreach (string path in this.pathComboBox.Items) 
			{
				xmlTextWriter.WriteElementString(CONFIG_PATH_ELEMENT, path);
			}

			xmlTextWriter.WriteEndElement();
		}

		#endregion

		#region Methods.Private

		private void InitializeFileDialog()
		{
			this.FileDialog.AddExtension = this.DefaultAddExtension;
			this.FileDialog.CheckFileExists = this.DefaultCheckFileExists;
			this.FileDialog.CheckPathExists = this.DefaultCheckPathExists;
			this.FileDialog.DefaultExt = this.DefaultDefaultExtension;
			this.FileDialog.DereferenceLinks = this.DefaultDereferenceLinks;
			this.FileDialog.Filter = this.DefaultFilter;
			this.FileDialog.FilterIndex = this.DefaultFilterIndex;
			this.FileDialog.InitialDirectory = this.DefaultInitialDirectory;
			this.FileDialog.RestoreDirectory = this.DefaultRestoreDirectory;
			this.FileDialog.ShowHelp = this.DefaultShowHelp;
			this.FileDialog.ValidateNames = this.DefaultValidateNames;
		}

		private bool ContainsItem(ComboBox comboBox, object item)
		{
			Debug.Assert(comboBox != null, "comboBox != null");
			Debug.Assert(item != null, "item != null");

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
			if (this.FileDialog != null) 
			{
				if (this.FileDialog.ShowDialog() == DialogResult.OK) 
				{
					this.SetPath(this.FileDialog.FileName);
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFileSelector"/> class.
		/// </summary>
		public NuGenFileSelector()
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
			this.browseButton.Location = new System.Drawing.Point(217, 1);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(32, 23);
			this.browseButton.TabIndex = 1;
			this.browseButton.Text = "&...";
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// NuGenFileSelector
			// 
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.pathComboBox);
			this.Name = "NuGenFileSelector";
			this.Size = new System.Drawing.Size(250, 25);
			this.ResumeLayout(false);

		}
		
		#endregion
	}
}
