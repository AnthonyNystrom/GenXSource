/* -----------------------------------------------
 * NuGenOutputEditor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides design-time path selection for the properties of type <see cref="T:System.String"/>.
	/// </summary>
	public class NuGenOutputEditor : NuGenCommonDialogEditorBase, IDisposable
	{
		private SaveFileDialog _saveFileDialog;

		/// <summary>
		/// </summary>
		protected SaveFileDialog SaveFileDialog
		{
			get
			{
				if (_saveFileDialog == null)
				{
					_saveFileDialog = new SaveFileDialog();
				}

				return _saveFileDialog;
			}
		}

		/// <summary>
		/// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> method.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
		/// <param name="provider">An <see cref="T:System.IServiceProvider"></see> that this editor can use to obtain services.</param>
		/// <param name="value">The object to edit.</param>
		/// <returns>
		/// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
		/// </returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

				if (service == null)
				{
					return value;
				}

				Debug.Assert(this.SaveFileDialog != null, "this.SaveFileDialog != null");
				this.InitializeOutputDialog(this.SaveFileDialog);

				if (this.SaveFileDialog.ShowDialog() == DialogResult.OK)
				{
					value = this.SaveFileDialog.FileName;
				}
			}

			return value;
		}

		/// <summary>
		/// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
		/// <returns>
		/// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
		/// </returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="saveFileDialogToInitialize"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void InitializeOutputDialog(SaveFileDialog saveFileDialogToInitialize)
		{
			if (saveFileDialogToInitialize == null)
			{
				throw new ArgumentNullException("saveFileDialogToInitialize");
			}

			saveFileDialogToInitialize.AddExtension = true;
			saveFileDialogToInitialize.CheckFileExists = false;
			saveFileDialogToInitialize.CheckPathExists = false;
			saveFileDialogToInitialize.DefaultExt = this.DefaultExtension;
			saveFileDialogToInitialize.Filter = this.Filter;
			saveFileDialogToInitialize.FilterIndex = 0;
			saveFileDialogToInitialize.OverwritePrompt = true;
			saveFileDialogToInitialize.RestoreDirectory = false;
			saveFileDialogToInitialize.ShowHelp = false;
			saveFileDialogToInitialize.Title = "Save File";
		}

		/// <summary>
		/// Initializes a new instance of the <c>Genetibase.UI.NuGenOutputEditor</c> class.
		/// </summary>
		public NuGenOutputEditor()
			: this("", "All Files (*.*)|*.*")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOutputEditor"/> class.
		/// </summary>
		/// <param name="defaultExtension">Specifies the default extension for the dialog.</param>
		/// <param name="filter">Specifies the filter for the dialog.</param>
		public NuGenOutputEditor(string defaultExtension, string filter)
			: base(defaultExtension, filter)
		{
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_saveFileDialog != null)
				{
					_saveFileDialog.Dispose();
					_saveFileDialog = null;
				}
			}
		}
	}
}
