using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Summary description for ExpressionForm.
	/// </summary>
	public class ExpressionForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btOk;
		private System.Windows.Forms.Button btCancel;
		private Genetibase.MathX.NugenCCalc.Design.Controls.ExpressionView codeExpressionView1;
		protected NugenCCalcBase _component = null;

		public ExpressionForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public ExpressionForm(NugenCCalcBase component) : this()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExpressionForm));
			this.tbName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.codeExpressionView1 = new Genetibase.MathX.NugenCCalc.Design.Controls.ExpressionView();
			this.SuspendLayout();
			// 
			// tbName
			// 
			this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbName.Location = new System.Drawing.Point(88, 16);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(392, 21);
			this.tbName.TabIndex = 1;
			this.tbName.Text = "";
			this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Display name";
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOk.Location = new System.Drawing.Point(328, 376);
			this.btOk.Name = "btOk";
			this.btOk.TabIndex = 4;
			this.btOk.Text = "Ok";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(413, 377);
			this.btCancel.Name = "btCancel";
			this.btCancel.TabIndex = 3;
			this.btCancel.Text = "Cancel";
			// 
			// codeExpressionView1
			// 
			this.codeExpressionView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.codeExpressionView1.BackColor = System.Drawing.Color.DarkKhaki;
			this.codeExpressionView1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.codeExpressionView1.Location = new System.Drawing.Point(8, 48);
			this.codeExpressionView1.Name = "codeExpressionView1";
			this.codeExpressionView1.Size = new System.Drawing.Size(480, 319);
			this.codeExpressionView1.TabIndex = 5;
			// 
			// ExpressionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.DarkKhaki;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(496, 406);
			this.Controls.Add(this.codeExpressionView1);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.btCancel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExpressionForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add\\Edit expression";
			this.Load += new System.EventHandler(this.ExpressionForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ExpressionForm_Load(object sender, System.EventArgs e)
		{
			codeExpressionView1.Component = _component;
			this.tbName.Text = _component.FunctionParameters.Name;
		}

		private void tbName_TextChanged(object sender, System.EventArgs e)
		{
			_component.FunctionParameters.Name = this.tbName.Text;
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
			codeExpressionView1.SaveData();
			base.OnClosed(e);
		}



	}
}
