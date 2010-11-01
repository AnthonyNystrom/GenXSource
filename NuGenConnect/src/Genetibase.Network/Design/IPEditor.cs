using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace Genetibase.Network.Design
{
    public class IPEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null)
                return UITypeEditorEditStyle.Modal;
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                // Access the Property Browser's UI display service
                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService != null)
                {
                    // Create an instance of the UI editor control
                    IPeditorForm ipe = new IPeditorForm();
                    // Pass the UI editor control the current property value
                    Ras.Ras.RASIPADDR i = (Ras.Ras.RASIPADDR)value;
                    ipe.A = i.a;
                    ipe.B = i.b;
                    ipe.C = i.c;
                    ipe.D = i.d;
                    // Display the UI editor control
                    if (editorService.ShowDialog(ipe) == DialogResult.OK)
                    {
                        i.a = ipe.A;
                        i.b = ipe.B;
                        i.c = ipe.C;
                        i.d = ipe.D;
                        return i;
                    }
                }
            }
            return base.EditValue(context, provider, value);
        }
    }
}
