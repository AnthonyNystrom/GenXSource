using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace Genetibase.Network.Design
{
    public class ScriptEditor : UITypeEditor
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
                    OpenFileDialog of = new OpenFileDialog();
                    of.InitialDirectory = "c:\\";
                    of.Filter = "Script files (*.scp)|*.scp|All files (*.*)|*.*";
                    of.RestoreDirectory = true;
                    // Display the UI editor control
                    if (of.ShowDialog() == DialogResult.OK)
                        return (of.FileName.Length > 0 ? of.FileName : null);
                    else
                        return null;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }
}
