using System;
using System.Reflection;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Genetibase.MathX.NugenCCalc;

namespace Genetibase.MathX.NugenCCalc.Design.Editors
{
    /// <summary>
    /// Summary description for CodeEditor.
    /// </summary>
    public class SourceEditor : UITypeEditor
    {
        private IWindowsFormsEditorService _editorService;

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
                _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (_editorService != null)
                {
                    foreach (Component component in context.Container.Components)
                    {
                        if (component != null)
                        {
                            if (component is NugenCCalcBase)
                            {
                                if ((component as NugenCCalcBase).FunctionParameters == (FunctionParameters)context.Instance)
                                {
                                    context.OnComponentChanging();
                                    if (component is NugenCCalc2D)
                                    {
                                        NugenCCalcDesignerForm designerForm = new NugenCCalcDesignerForm((component as NugenCCalc2D));
                                        _editorService.ShowDialog(designerForm);
                                    }
                                    else
                                    {
                                        NugenCCalc3DDesignerForm designerForm = new NugenCCalc3DDesignerForm((component as NugenCCalc3D));
                                        _editorService.ShowDialog(designerForm);
                                    }
                                    context.OnComponentChanged();
                                }
                            }
                        }
                    }
                    RefreshPropertyGrid();
                }
            }
            return value;
        }

        private void RefreshPropertyGrid()
        {
            if (_editorService == null)
                return;
            object pg = _editorService.GetType().InvokeMember("OwnerGrid", BindingFlags.GetProperty,
                 null, _editorService, null);
            if (pg is PropertyGrid)
                (pg as PropertyGrid).Refresh();
        }
    }
}
