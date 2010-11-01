/* -----------------------------------------------
 * NuGenTransparencyEditor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Design
{
	/// <summary>
	/// Represents the UITypeEditor to set transparency level.
	/// </summary>
	public sealed class NuGenTransparencyEditor : UITypeEditor
	{
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
			if (value.GetType() != typeof(int)) 
			{
				return value;
			}

			IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			
			if (editorService != null) 
			{
				NuGenTransparencyEditorUI transparencyEditor = new NuGenTransparencyEditorUI((int)value);
				editorService.DropDownControl(transparencyEditor);

				return transparencyEditor.GetValue;
			}

			return value;
		}

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
			return UITypeEditorEditStyle.DropDown;
		}

		/// <summary>
		/// Indicates whether the specified context supports painting a representation of an object's value
		/// within the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
		/// <returns>
		/// 	<see langword="true"/>
		/// if <see cref="M:System.Drawing.Design.UITypeEditor.PaintValue(System.Object,System.Drawing.Graphics,System.Drawing.Rectangle)"/> is implemented; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}
	}
}
