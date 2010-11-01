/* -----------------------------------------------
 * NuGenInputEditor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Design
{
	/// <summary>
	/// Provides a <see cref="OpenFileDialog"/> during desing-time.
	/// </summary>
	public class NuGenInputEditor : NuGenCommonDialogEditorBase
	{
		#region Properties.Protected

		/*
		 * OpenFileDialog
		 */

		private OpenFileDialog _openFileDialog = null;

		/// <summary>
		/// </summary>
		protected OpenFileDialog OpenFileDialog
		{
			get
			{
				if (_openFileDialog == null)
				{
					_openFileDialog = new OpenFileDialog();
				}

				return _openFileDialog;
			}
		}

		#endregion

		#region Methods.Public.Overriden

		/*
		 * EditValue
		 */

		/// <summary>
		/// Edits the specified object's value using the editor style
		/// indicated by <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/>.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
		/// <param name="provider">An <see cref="T:System.IServiceProvider"/> that this editor can use to obtain services.</param>
		/// <param name="value">The object to edit.</param>
		/// <returns>The new value of the object.</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null) 
			{
				IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

				if (service == null) 
				{
					return value;
				}

				Debug.Assert(this.OpenFileDialog != null, "this.OpenFileDialog != null");
				this.InitializeOpenFileDialog(this.OpenFileDialog);

				if (this.OpenFileDialog.ShowDialog() == DialogResult.OK) 
				{
					value = this.OpenFileDialog.FileName;
				}
			}

			return value;
		}

		/*
		 * GetEditStyle
		 */

		/// <summary>
		/// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/> method.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
		/// <returns>
		/// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"/> value that
		/// indicates the style of editor used by <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"/>. If the <see cref="T:System.Drawing.Design.UITypeEditor"/> does not support this
		/// method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"/> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"/>.
		/// </returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		#endregion

		#region Methods.Protected.Virtual

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="openFileDialogToInitialize"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void InitializeOpenFileDialog(OpenFileDialog openFileDialogToInitialize)
		{
			if (openFileDialogToInitialize == null)
			{
				throw new ArgumentNullException("openFileDialogToInitialize");
			}

			openFileDialogToInitialize.AddExtension = true;
			openFileDialogToInitialize.CheckFileExists = true;
			openFileDialogToInitialize.CheckPathExists = true;
			openFileDialogToInitialize.DefaultExt = this.DefaultExtension;
			openFileDialogToInitialize.DereferenceLinks = true;
			openFileDialogToInitialize.Filter = this.Filter;
			openFileDialogToInitialize.FilterIndex = 0;
			openFileDialogToInitialize.Multiselect = false;
			openFileDialogToInitialize.Title = "Open File";
			openFileDialogToInitialize.ValidateNames = true;
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInputEditor"/> class.<para/>
		/// DefaultExtension = "". Filter = All Files (*.*)|*.*.
		/// </summary>
		public NuGenInputEditor()
			: this("", "All Files (*.*)|*.*")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInputEditor"/> class.
		/// </summary>
		/// <param name="defaultExtension">Specifies the default extension for the dialog.</param>
		/// <param name="filter">Specifies the filter for the dialog.</param>
		public NuGenInputEditor(string defaultExtension, string filter)
			: base(defaultExtension, filter)
		{
		}
		
		#endregion
	}
}
