using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Genetibase.MathX.NugenCCalc;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public delegate void ViewStatusChangeHandler(object sender, StringArgs s);

	/// <summary>
	/// 
	/// </summary>
	[ToolboxItem(false)]
	public class PropertyView : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		protected NugenCCalcBase _component = null;
		private System.ComponentModel.Container components = null;

		public event ViewStatusChangeHandler OnViewStatusChange;

		/// <summary>
		/// Initializes a new instance of the PropertyView class.
		/// </summary>
		public PropertyView()
		{			
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the PropertyView class.
		/// </summary>
		/// <param name="component">NugenCCalcComponent component</param>
		public PropertyView(NugenCCalcBase component) : this()
		{			
			_component = component;
		}
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		protected void ViewStatusChanged(object sender, StringArgs e)
		{
			if (OnViewStatusChange!=null)
			{
				OnViewStatusChange(this, e);
			}
		}


		public virtual void SaveData()
		{
		
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// PropertyView
			// 
			this.BackColor = System.Drawing.Color.DarkKhaki;
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Name = "PropertyView";
			this.Size = new System.Drawing.Size(360, 312);

		}
		#endregion

	}
}
