using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Reflection;
using System.Security.Permissions;
using System.Collections.ObjectModel;

namespace Genetibase.Shared.Controls.Design
{
    /// <summary>
    /// Provides a user interface that can edit a <see cref="NuGenUIItemCollection"/> at design time.
    /// </summary>
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class NuGenUIItemsEditor : UITypeEditor
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="NuGenUIItemsEditor"/> class.
        /// </summary>
        public NuGenUIItemsEditor()
        {
        }

        /// <summary>
        /// Gets the editor style used by the EditValue method.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <returns>A UITypeEditorEditStyle value that indicates the style of editor used by the EditValue method.
        /// If the UITypeEditor does not support this method, then GetEditStyle will return None.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the GetEditStyle method.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <param name="provider">An IServiceProvider that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>The new value of the object. If the value of the object has not changed, this should return the same object it was passed.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // Get associated items list
            NuGenUIItemCollection uiItemCollection = value as NuGenUIItemCollection;
            Collection<object> associatedItems = new Collection<object>();
            foreach (object item in uiItemCollection)
            {
                associatedItems.Add(item);
            }

            // Get command switchboard
            NuGenApplicationCommand command = uiItemCollection.ApplicationCommand;
            NuGenCommandManagerBase UISwitchboard = command.CommandManager;

            // Get available items list
            Collection<object> availableItems = new Collection<object>();
            foreach (object item in UISwitchboard.UIItemAdapter.GetAvailableUIItems())
            {
                availableItems.Add(item);
            }

            // Get editor service
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (editorService != null)
            {
                // Show form
                NuGenUIItemsForm formUIItems = new NuGenUIItemsForm(command, associatedItems, availableItems);
                if (editorService.ShowDialog(formUIItems) == DialogResult.OK)
                {
                    NuGenUIItemCollection modifiedUIItemCollection = new NuGenUIItemCollection(command);
                    foreach (Object item in associatedItems)
                    {
                        modifiedUIItemCollection.Add(item);
                    }
                    return modifiedUIItemCollection;
                }
            }
            return value;
        }
    }
}
