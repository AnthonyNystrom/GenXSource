using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Summary description for EquationForm.
	/// </summary>
	public class EquationForm : System.Windows.Forms.Form
	{
		protected NugenCCalcBase _component = null;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbEquationName;
		private System.Windows.Forms.Button btCancel;
		private Genetibase.MathX.NugenCCalc.Design.Controls.EquationView equationView1;
		private System.Windows.Forms.Button btOK;

		public EquationForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public EquationForm(NugenCCalcBase component) : this()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EquationForm));
			this.label1 = new System.Windows.Forms.Label();
			this.tbEquationName = new System.Windows.Forms.TextBox();
			this.btCancel = new System.Windows.Forms.Button();
			this.btOK = new System.Windows.Forms.Button();
			this.equationView1 = new Genetibase.MathX.NugenCCalc.Design.Controls.EquationView();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Equation name";
			// 
			// tbEquationName
			// 
			this.tbEquationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbEquationName.Location = new System.Drawing.Point(16, 32);
			this.tbEquationName.Name = "tbEquationName";
			this.tbEquationName.Size = new System.Drawing.Size(406, 21);
			this.tbEquationName.TabIndex = 3;
			this.tbEquationName.Text = "";
			this.tbEquationName.TextChanged += new System.EventHandler(this.tbEquationName_TextChanged);
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(350, 359);
			this.btCancel.Name = "btCancel";
			this.btCancel.TabIndex = 4;
			this.btCancel.Text = "Cancel";
			// 
			// btOK
			// 
			this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOK.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOK.Location = new System.Drawing.Point(270, 359);
			this.btOK.Name = "btOK";
			this.btOK.TabIndex = 5;
			this.btOK.Text = "OK";
			// 
			// equationView1
			// 
			this.equationView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.equationView1.BackColor = System.Drawing.Color.DarkKhaki;
			this.equationView1.Component = null;
			this.equationView1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.equationView1.Location = new System.Drawing.Point(16, 56);
			this.equationView1.Name = "equationView1";
			this.equationView1.Size = new System.Drawing.Size(408, 296);
			this.equationView1.TabIndex = 6;
			// 
			// EquationForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.DarkKhaki;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(432, 390);
			this.Controls.Add(this.equationView1);
			this.Controls.Add(this.btOK);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.tbEquationName);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EquationForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add\\Edit equation";
			this.Load += new System.EventHandler(this.EquationForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void EquationForm_Load(object sender, System.EventArgs e)
		{
			equationView1.Component = _component;
			this.tbEquationName.Text = _component.FunctionParameters.Name;
		}

		private void tbEquationName_TextChanged(object sender, System.EventArgs e)
		{
			_component.FunctionParameters.Name = this.tbEquationName.Text;
		}

		public NugenCCalcBase Component
		{
			get
			{
				return _component;
			}
			set
			{
				_component = value;
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			equationView1.SaveData();
			base.OnClosed(e);
		}
	}
}
