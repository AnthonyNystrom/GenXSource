using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.MathX.NugenCCalc;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public class EquationView : PropertyView
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cbFunctionType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RichTextBox rtbBody;
		private System.Windows.Forms.Panel pnlParametric2D;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlParametric3D;
		private System.Windows.Forms.TextBox tbY;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbX;
		private System.Windows.Forms.TextBox tbY3D;
		private System.Windows.Forms.TextBox tbX3D;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbZ3D;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.ComponentModel.IContainer components = null;

		public EquationView()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbFunctionType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.rtbBody = new System.Windows.Forms.RichTextBox();
			this.pnlParametric2D = new System.Windows.Forms.Panel();
			this.tbY = new System.Windows.Forms.TextBox();
			this.tbX = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlParametric3D = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.tbZ3D = new System.Windows.Forms.TextBox();
			this.tbY3D = new System.Windows.Forms.TextBox();
			this.tbX3D = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.pnlParametric2D.SuspendLayout();
			this.pnlParametric3D.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.cbFunctionType);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(0, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(440, 64);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Expression properties";
			// 
			// cbFunctionType
			// 
			this.cbFunctionType.DisplayMember = "Explicit";
			this.cbFunctionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFunctionType.Location = new System.Drawing.Point(120, 24);
			this.cbFunctionType.Name = "cbFunctionType";
			this.cbFunctionType.Size = new System.Drawing.Size(168, 21);
			this.cbFunctionType.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(8, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Function type";
			// 
			// rtbBody
			// 
			this.rtbBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.rtbBody.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.rtbBody.Location = new System.Drawing.Point(0, 80);
			this.rtbBody.Name = "rtbBody";
			this.rtbBody.Size = new System.Drawing.Size(440, 224);
			this.rtbBody.TabIndex = 12;
			this.rtbBody.Text = "";
			// 
			// pnlParametric2D
			// 
			this.pnlParametric2D.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlParametric2D.Controls.Add(this.tbY);
			this.pnlParametric2D.Controls.Add(this.tbX);
			this.pnlParametric2D.Controls.Add(this.label3);
			this.pnlParametric2D.Controls.Add(this.label1);
			this.pnlParametric2D.Location = new System.Drawing.Point(0, 80);
			this.pnlParametric2D.Name = "pnlParametric2D";
			this.pnlParametric2D.Size = new System.Drawing.Size(440, 224);
			this.pnlParametric2D.TabIndex = 14;
			// 
			// tbY
			// 
			this.tbY.Location = new System.Drawing.Point(0, 136);
			this.tbY.Multiline = true;
			this.tbY.Name = "tbY";
			this.tbY.Size = new System.Drawing.Size(432, 80);
			this.tbY.TabIndex = 3;
			this.tbY.Text = "";
			// 
			// tbX
			// 
			this.tbX.Location = new System.Drawing.Point(0, 24);
			this.tbX.Multiline = true;
			this.tbX.Name = "tbX";
			this.tbX.Size = new System.Drawing.Size(432, 80);
			this.tbX.TabIndex = 0;
			this.tbX.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Y(t)";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "X(t)";
			// 
			// pnlParametric3D
			// 
			this.pnlParametric3D.Controls.Add(this.label7);
			this.pnlParametric3D.Controls.Add(this.tbZ3D);
			this.pnlParametric3D.Controls.Add(this.tbY3D);
			this.pnlParametric3D.Controls.Add(this.tbX3D);
			this.pnlParametric3D.Controls.Add(this.label4);
			this.pnlParametric3D.Controls.Add(this.label5);
			this.pnlParametric3D.Location = new System.Drawing.Point(0, 80);
			this.pnlParametric3D.Name = "pnlParametric3D";
			this.pnlParametric3D.Size = new System.Drawing.Size(440, 224);
			this.pnlParametric3D.TabIndex = 15;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(0, 144);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(40, 16);
			this.label7.TabIndex = 10;
			this.label7.Text = "Z(t)";
			// 
			// tbZ3D
			// 
			this.tbZ3D.Location = new System.Drawing.Point(0, 160);
			this.tbZ3D.Multiline = true;
			this.tbZ3D.Name = "tbZ3D";
			this.tbZ3D.Size = new System.Drawing.Size(432, 56);
			this.tbZ3D.TabIndex = 9;
			this.tbZ3D.Text = "";
			// 
			// tbY3D
			// 
			this.tbY3D.Location = new System.Drawing.Point(0, 88);
			this.tbY3D.Multiline = true;
			this.tbY3D.Name = "tbY3D";
			this.tbY3D.Size = new System.Drawing.Size(432, 56);
			this.tbY3D.TabIndex = 7;
			this.tbY3D.Text = "";
			// 
			// tbX3D
			// 
			this.tbX3D.Location = new System.Drawing.Point(0, 16);
			this.tbX3D.Multiline = true;
			this.tbX3D.Name = "tbX3D";
			this.tbX3D.Size = new System.Drawing.Size(432, 56);
			this.tbX3D.TabIndex = 5;
			this.tbX3D.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 23);
			this.label4.TabIndex = 6;
			this.label4.Text = "Y(t)";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(0, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 23);
			this.label5.TabIndex = 4;
			this.label5.Text = "X(t)";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(0, 0);
			this.label6.Name = "label6";
			this.label6.TabIndex = 0;
			// 
			// EquationView
			// 
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.rtbBody);
			this.Controls.Add(this.pnlParametric2D);
			this.Controls.Add(this.pnlParametric3D);
			this.Name = "EquationView";
			this.Size = new System.Drawing.Size(440, 312);
			this.groupBox1.ResumeLayout(false);
			this.pnlParametric2D.ResumeLayout(false);
			this.pnlParametric3D.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		public override void SaveData()
		{
			try
			{
				string functionType = _component.FunctionParameters.GetType().ToString();
				switch(functionType)
				{
					case "Genetibase.MathX.NugenCCalc.Parametric2DParameters":
						_component.FunctionParameters.Formula = this.tbX.Text;
						((Parametric2DParameters)_component.FunctionParameters).FormulaY= this.tbY.Text;
						break;
					case "Genetibase.MathX.NugenCCalc.Parametric3DParameters":
						_component.FunctionParameters.Formula = this.tbX3D.Text;
						((Parametric3DParameters)_component.FunctionParameters).FormulaY= this.tbY3D.Text;
						((Parametric3DParameters)_component.FunctionParameters).FormulaZ= this.tbZ3D.Text;
						break;
					case "Genetibase.MathX.NugenCCalc.ParametricSurfaceParameters":
						_component.FunctionParameters.Formula = this.tbX3D.Text;
						((ParametricSurfaceParameters)_component.FunctionParameters).FormulaY= this.tbY3D.Text;
						((ParametricSurfaceParameters)_component.FunctionParameters).FormulaZ= this.tbZ3D.Text;
						break;
					default:
						_component.FunctionParameters.Formula = rtbBody.Text;
						break;
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public NugenCCalcBase Component
		{
			get
			{
				return _component;
			}
			set
			{
				if (_component != value)
				{
					_component = value;
					InitFunctionTypes();
					GetEquationValue();
					this.cbFunctionType.SelectedIndexChanged += new System.EventHandler(this.cbFunctionType_SelectedIndexChanged);
					//cbFunctionType_SelectedIndexChanged(this, EventArgs.Empty);
				}
			}
		}

        
		public void InitFunctionTypes()
		{
			this.cbFunctionType.Items.Clear();
			this.cbFunctionType.Items.AddRange(new object[] {
																"Explicit",
																"Implicit",
																"Parametric"});
			if (_component.FunctionParameters is Function3DParameters)
			{
				this.cbFunctionType.Items.Add("Parametric Surface");
			}
		}


		private void cbFunctionType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_component.FunctionParameters is Function2DParameters)
			{
				switch (this.cbFunctionType.Text)
				{
					case "Explicit":
						_component.FunctionParameters = new Explicit2DParameters();
						ShowSimpleView();
						break;
					case "Implicit":
						_component.FunctionParameters = new Implicit2DParameters();
						ShowSimpleView();
						break;
					case "Parametric":
						_component.FunctionParameters = new Parametric2DParameters();
						ShowParameter2DView();
						break;
				}
			}
			if (_component.FunctionParameters is Function3DParameters)
			{
				switch (this.cbFunctionType.Text)
				{
					case "Explicit":
						_component.FunctionParameters = new Explicit3DParameters();
						ShowSimpleView();
						break;
					case "Implicit":
						_component.FunctionParameters = new Implicit3DParameters();
						ShowSimpleView();
						break;
					case "Parametric":
						_component.FunctionParameters = new Parametric3DParameters();
						ShowParameter3DView();
						break;
					case "Parametric Surface":
						_component.FunctionParameters = new ParametricSurfaceParameters();
						ShowParameter3DSurfView();
						break;
				}
			}

		}

		private void ShowSimpleView()
		{
			this.pnlParametric2D.Hide();
			this.pnlParametric3D.Hide();
			this.rtbBody.Show();
		}

		private void ShowParameter2DView()
		{
			this.rtbBody.Hide();
			this.pnlParametric2D.Show();
			this.pnlParametric3D.Hide();
		}

		private void ShowParameter3DView()
		{
			this.rtbBody.Hide();
			this.pnlParametric2D.Hide();
			this.pnlParametric3D.Show();
			this.label5.Text = "X(t)";
			this.label4.Text = "Y(t)";
			this.label7.Text = "Z(t)";
		}

		private void ShowParameter3DSurfView()
		{
			this.rtbBody.Hide();
			this.pnlParametric2D.Hide();
			this.pnlParametric3D.Show();
			this.label5.Text = "X(u,v)";
			this.label4.Text = "Y(u,v)";
			this.label7.Text = "Z(u,v)";
		}
		


		private void GetEquationValue()
		{	
			if (_component.FunctionParameters is Function2DParameters)
			{
				if (_component.FunctionParameters is Explicit2DParameters)
				{
					this.cbFunctionType.SelectedIndex = 0;
					this.rtbBody.Text = _component.FunctionParameters.Formula;
					ShowSimpleView();
				}
				if (_component.FunctionParameters is Implicit2DParameters)
				{
					this.cbFunctionType.SelectedIndex = 1;
					this.rtbBody.Text = _component.FunctionParameters.Formula;
					ShowSimpleView();
				}
				if (_component.FunctionParameters is Parametric2DParameters)
				{
					this.cbFunctionType.SelectedIndex = 2;
					this.tbX.Text = _component.FunctionParameters.Formula;
					this.tbY.Text = ((Parametric2DParameters)_component.FunctionParameters).FormulaY;
					ShowParameter2DView();
				}
			}
			if (_component.FunctionParameters is Function3DParameters)
			{
				if (_component.FunctionParameters is Explicit3DParameters)
				{
					this.cbFunctionType.SelectedIndex = 0;
					this.rtbBody.Text = _component.FunctionParameters.Formula;
					ShowSimpleView();
				}
				if (_component.FunctionParameters is Implicit3DParameters)
				{
					this.cbFunctionType.SelectedIndex = 1;
					this.rtbBody.Text = _component.FunctionParameters.Formula;
					ShowSimpleView();
				}
				if (_component.FunctionParameters is Parametric3DParameters)
				{
					this.cbFunctionType.SelectedIndex = 2;
					this.tbX3D.Text = _component.FunctionParameters.Formula;
					this.tbY3D.Text = ((Parametric3DParameters)_component.FunctionParameters).FormulaY;
					this.tbZ3D.Text = ((Parametric3DParameters)_component.FunctionParameters).FormulaZ;
					ShowParameter3DView();
				}
				if (_component.FunctionParameters is ParametricSurfaceParameters)
				{
					this.cbFunctionType.SelectedIndex = 3;
					this.tbX3D.Text = _component.FunctionParameters.Formula;
					this.tbY3D.Text = ((ParametricSurfaceParameters)_component.FunctionParameters).FormulaY;
					this.tbZ3D.Text = ((ParametricSurfaceParameters)_component.FunctionParameters).FormulaZ;
					ShowParameter3DSurfView();
				}
			}
			
		}

		}
}

