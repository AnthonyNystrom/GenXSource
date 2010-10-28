using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public class SourceView : Genetibase.MathX.NugenCCalc.Design.Controls.PropertyView
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ComboBox cb_Source;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.ComponentModel.IContainer components = null;

		public SourceView(NugenCCalcBase component)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			_component = component;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SourceView));
			this.label1 = new System.Windows.Forms.Label();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.cb_Source = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(0, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(224, 16);
			this.label1.TabIndex = 15;
			this.label1.Text = "Source Type";
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(32, 32);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// cb_Source
			// 
			this.cb_Source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_Source.Items.AddRange(new object[] {
														   "Code expression",
														   "Equation"});
			this.cb_Source.Location = new System.Drawing.Point(0, 72);
			this.cb_Source.Name = "cb_Source";
			this.cb_Source.Size = new System.Drawing.Size(320, 21);
			this.cb_Source.TabIndex = 14;
			this.cb_Source.SelectedIndexChanged += new System.EventHandler(this.cb_Source_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Tahoma", 21.75F);
			this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
			this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label2.ImageIndex = 0;
			this.label2.ImageList = this.imageList1;
			this.label2.Location = new System.Drawing.Point(352, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(168, 32);
			this.label2.TabIndex = 13;
			this.label2.Text = "Source";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(520, 8);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(0, 104);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(520, 360);
			this.panel1.TabIndex = 16;
			// 
			// SourceView
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.cb_Source);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Name = "SourceView";
			this.Size = new System.Drawing.Size(528, 464);
			this.Load += new System.EventHandler(this.SourceView_Load);
			this.ResumeLayout(false);

		}
		#endregion

		public override void SaveData()
		{
			if (_currentView != null)
				_currentView.SaveData();
		}



		private void cb_Source_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(this.cb_Source.Text)
			{
				case "Code expression":
					_component.FunctionParameters.SourceType = SourceType.CodeExpression;
					break;
				case "Equation":
					_component.FunctionParameters.SourceType = SourceType.Equation;
					break;
			}
			InitView();
		}

		private void SourceView_Load(object sender, System.EventArgs e)
		{
			switch(_component.FunctionParameters.SourceType)
			{
				case SourceType.CodeExpression:
					this.cb_Source.SelectedItem = this.cb_Source.Items[0];
					break;
				case SourceType.Equation:
					this.cb_Source.SelectedItem = this.cb_Source.Items[1];
					break;
			}
		}


		private PropertyView _currentView = null;

		private void InitView()
		{
			PropertyView newView = null;
			if (_component.FunctionParameters.SourceType == SourceType.Equation)
			{
				newView = new EquationView();
				((EquationView)newView).Component = _component;
			}
			else
			{
				newView = new ExpressionView();
				((ExpressionView)newView).Component = _component;
			}

			this.panel1.Controls.Add(newView);
			newView.Dock = DockStyle.Fill;
			if (_currentView != null)
			{
				_currentView.SaveData();
				_currentView.Dispose();
				_currentView = null;
			}
			_currentView = newView;
		}
	
	}
}

