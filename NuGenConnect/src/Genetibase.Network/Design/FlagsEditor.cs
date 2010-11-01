using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace Genetibase.Network.Design
{
    public class FlagsEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null)
                return UITypeEditorEditStyle.DropDown;
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                // Access the Property Browser's UI display service
                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService != null && value.GetType() == typeof(Ras.Ras.EntryOptions)) // edit Options flags
                {
                    // Create an instance of the UI editor control
                    OptionsFlagsEditorForm ed = new OptionsFlagsEditorForm();
                    // Pass the UI editor control the current property value
                    ed.Val = (Ras.Ras.EntryOptions)value;
                    // Display the UI editor control
                    editorService.ShowDialog(ed);
                    // Return the new property value from the UI editor control
                    return ed.Val;
                }
                else if (editorService != null && value.GetType() == typeof(Ras.Ras.EntryProtocols)) // edit Protocols flags
                {
                    // Create an instance of the UI editor control
                    ProtocolsFlagsEditorForm ed = new ProtocolsFlagsEditorForm();
                    // Pass the UI editor control the current property value
                    ed.Val = (Ras.Ras.EntryProtocols)value;
                    // Display the UI editor control
                    editorService.ShowDialog(ed);
                    // Return the new property value from the UI editor control
                    return ed.Val;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }

}
