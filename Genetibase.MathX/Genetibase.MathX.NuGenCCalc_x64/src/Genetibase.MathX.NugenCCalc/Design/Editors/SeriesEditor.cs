using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Genetibase.MathX.NugenCCalc.Adapters ;

namespace Genetibase.MathX.NugenCCalc.Design.Editors
{
	/// <summary>
	/// Provides a user interface to select series of chart. 
	/// </summary>
    public class SeriesEditor : UITypeEditor
	{
		private IWindowsFormsEditorService _editorService;

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
				_editorService = (IWindowsFormsEditorService)	provider.GetService(typeof(IWindowsFormsEditorService));

				if(_editorService != null)
				{
					SeriesListBox _listBox = new SeriesListBox();
					_listBox.Click += new EventHandler(_listBox_Click);
					_listBox.BorderStyle = BorderStyle.None;

					foreach(Series series in ((NugenCCalc2D)context.Instance).CurrentAdapter.GetSeries())
					{
						_listBox.Items.Add(series);
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
	}
}
