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
    /// Provides a user interface that can edit a <see cref="NuGenContextMenuCollection"/> at design time.
    /// </summary>
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class NuGenContextMenusEditor : UITypeEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenContextMenusEditor"/> class.
        /// </summary>
        public NuGenContextMenusEditor()
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
            Collection<object> associatedItems = new Collection<object>();
            NuGenContextMenuCollection contextMenuCollection = value as NuGenContextMenuCollection;
            foreach (object item in contextMenuCollection)
            {
                associatedItems.Add(item);
            }

            // Get command switchboard
            NuGenCommandManagerBase UISwitchboard = contextMenuCollection.UISwitchboard;

            // Get available items list
            Collection<object> availableItems = new Collection<object>();
            foreach (object item in UISwitchboard.UIItemAdapter.GetAvailableContextMenus())
            {
                availableItems.Add(item);
            }

            // Get editor service
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (editorService != null)
            {
                // Show form
                NuGenContextMenusForm formContextMenus = new NuGenContextMenusForm(UISwitchboard, associatedItems, availableItems);
                if (editorService.ShowDialog(formContextMenus) == DialogResult.OK)
                {
                    NuGenContextMenuCollection modifiedContextMenuCollection = new NuGenContextMenuCollection(UISwitchboard);
                    foreach (object item in associatedItems)
                    {
                        modifiedContextMenuCollection.Add(item);
                    }
                    return modifiedContextMenuCollection;
                }
            }
            return value;
        }
    }
}
