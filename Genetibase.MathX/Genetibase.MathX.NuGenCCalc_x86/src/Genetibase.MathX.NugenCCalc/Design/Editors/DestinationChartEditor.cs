using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Genetibase.MathX.NugenCCalc;

namespace Genetibase.MathX.NugenCCalc.Design.Editors
{
	/// <summary>
	/// Provides a user interface to select destination chart
	/// </summary>
    public class DestinationChartEditor : UITypeEditor
	{
		private IWindowsFormsEditorService _editorService;
        private NugenCCalcBase nugenCCalc;
        private ListBox _listBox;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if(context != null)
				return UITypeEditorEditStyle.DropDown;
			return base.GetEditStyle(context);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if((context != null) && (provider != null)) 
			{
				// Access the Property Browser's UI display service
				_editorService = (IWindowsFormsEditorService)	provider.GetService(typeof(IWindowsFormsEditorService));

				if(_editorService != null)
				{
					_listBox = new ListBox();
					_listBox.Click += new EventHandler(_listBox_Click);
					_listBox.BorderStyle = BorderStyle.None;
                    nugenCCalc = (NugenCCalcBase)context.Instance;

                    if (nugenCCalc.Site.DesignMode)
                    {
                        foreach (Component component in context.Container.Components)
                        {
                            try
                            {
                                if (nugenCCalc.ValidateControl(component))
                                    _listBox.Items.Add(component);
                            }
                            catch { }
                        }
                    }
                    else 
                    {
                        if (nugenCCalc.Owner != null)
                        {
                            IterateControls((nugenCCalc.Owner as Form).Controls);
                        }
                    }
					
					_editorService.DropDownControl(_listBox);
					if (_listBox.SelectedItem != null)
						value = _listBox.SelectedItem;
				}
			}
			return value;
		}

		private void _listBox_Click(object sender, EventArgs e)
		{
			if (_editorService != null) 
			{ 
				_editorService.CloseDropDown(); 
			}
		}

        private void IterateControls(Control.ControlCollection controls)
        {
            foreach (Control child in controls)
            {
                try
                {
                    if (nugenCCalc.ValidateControl(child))
                        _listBox.Items.Add(child);
                }
                catch { }
                if (child.HasChildren)
                    IterateControls(child.Controls);
            }
        }
	}
}
