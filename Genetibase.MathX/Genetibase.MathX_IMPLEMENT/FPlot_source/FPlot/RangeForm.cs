using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for RangeForm.
	/// </summary>
	public class RangeForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox x0TextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox y0TextBox;
		private System.Windows.Forms.TextBox y1TextBox;
		private System.Windows.Forms.TextBox x1TextBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox fixytox;
		private System.Windows.Forms.TextBox z1TextBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox z0TextBox;
		private System.Windows.Forms.Label label8;
		private GraphControl graph;

		public RangeForm(GraphControl graph)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.graph = graph;
			x0TextBox.KeyPress += new KeyPressEventHandler(numberKeyPress);
			y0TextBox.KeyPress += new KeyPressEventHandler(numberKeyPress);
			x1TextBox.KeyPress += new KeyPressEventHandler(numberKeyPress);
			y1TextBox.KeyPress += new KeyPressEventHandler(numberKeyPress);
			z0TextBox.KeyPress += new KeyPressEventHandler(numberKeyPress);
			z1TextBox.KeyPress += new KeyPressEventHandler(numberKeyPress);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RangeForm));
			this.label1 = new System.Windows.Forms.Label();
			this.x0TextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.y0TextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.y1TextBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.x1TextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.fixytox = new System.Windows.Forms.CheckBox();
			this.z1TextBox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.z0TextBox = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "x";
			// 
			// x0TextBox
			// 
			this.x0TextBox.Location = new System.Drawing.Point(24, 32);
			this.x0TextBox.Name = "x0TextBox";
			this.x0TextBox.Size = new System.Drawing.Size(104, 20);
			this.x0TextBox.TabIndex = 1;
			this.x0TextBox.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "y";
			// 
			// y0TextBox
			// 
			this.y0TextBox.Location = new System.Drawing.Point(24, 56);
			this.y0TextBox.Name = "y0TextBox";
			this.y0TextBox.Size = new System.Drawing.Size(104, 20);
			this.y0TextBox.TabIndex = 3;
			this.y0TextBox.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "From:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(152, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(24, 16);
			this.label4.TabIndex = 5;
			this.label4.Text = "To:";
			// 
			// y1TextBox
			// 
			this.y1TextBox.Location = new System.Drawing.Point(168, 56);
			this.y1TextBox.Name = "y1TextBox";
			this.y1TextBox.Size = new System.Drawing.Size(104, 20);
			this.y1TextBox.TabIndex = 9;
			this.y1TextBox.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(152, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(16, 24);
			this.label5.TabIndex = 8;
			this.label5.Text = "y";
			// 
			// x1TextBox
			// 
			this.x1TextBox.Location = new System.Drawing.Point(168, 32);
			this.x1TextBox.Name = "x1TextBox";
			this.x1TextBox.Size = new System.Drawing.Size(104, 20);
			this.x1TextBox.TabIndex = 7;
			this.x1TextBox.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(152, 32);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(16, 16);
			this.label6.TabIndex = 6;
			this.label6.Text = "x";
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(16, 144);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 10;
			this.okButton.Text = "&Ok";
			this.okButton.Click += new System.EventHandler(this.okClick);
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(104, 144);
			this.applyButton.Name = "applyButton";
			this.applyButton.TabIndex = 11;
			this.applyButton.Text = "&Apply";
			this.applyButton.Click += new System.EventHandler(this.applyClick);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(192, 144);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 12;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelClick);
			// 
			// fixytox
			// 
			this.fixytox.Location = new System.Drawing.Point(8, 112);
			this.fixytox.Name = "fixytox";
			this.fixytox.Size = new System.Drawing.Size(144, 24);
			this.fixytox.TabIndex = 13;
			this.fixytox.Text = "Fix y-Scale to x-Scale";
			// 
			// z1TextBox
			// 
			this.z1TextBox.Location = new System.Drawing.Point(168, 80);
			this.z1TextBox.Name = "z1TextBox";
			this.z1TextBox.Size = new System.Drawing.Size(104, 20);
			this.z1TextBox.TabIndex = 17;
			this.z1TextBox.Text = "";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(152, 80);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(16, 24);
			this.label7.TabIndex = 16;
			this.label7.Text = "z";
			// 
			// z0TextBox
			// 
			this.z0TextBox.Location = new System.Drawing.Point(24, 80);
			this.z0TextBox.Name = "z0TextBox";
			this.z0TextBox.Size = new System.Drawing.Size(104, 20);
			this.z0TextBox.TabIndex = 15;
			this.z0TextBox.Text = "";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 80);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(16, 24);
			this.label8.TabIndex = 14;
			this.label8.Text = "z";
			// 
			// RangeForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(288, 179);
			this.Controls.Add(this.z1TextBox);
			this.Controls.Add(this.z0TextBox);
			this.Controls.Add(this.y1TextBox);
			this.Controls.Add(this.x1TextBox);
			this.Controls.Add(this.y0TextBox);
			this.Controls.Add(this.x0TextBox);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.fixytox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RangeForm";
			this.ShowInTaskbar = false;
			this.Text = "Set view range";
			this.ResumeLayout(false);

		}
		#endregion

		public void Reset() {
			x0TextBox.Text = graph.Model.x0.ToString();
			y0TextBox.Text = graph.Model.y0.ToString();
			x1TextBox.Text = graph.Model.x1.ToString();
			y1TextBox.Text = graph.Model.y1.ToString();
			z0TextBox.Text = graph.Model.z0.ToString();
			z1TextBox.Text = graph.Model.z1.ToString();
			fixytox.Checked = graph.Model.FixYtoX;
		}
		
		private void Apply() {
			try {
				double x0 = double.Parse(x0TextBox.Text);
				double y0 = double.Parse(y0TextBox.Text);
				double x1 = double.Parse(x1TextBox.Text);
				double y1 = double.Parse(y1TextBox.Text);
				double z0 = double.Parse(z0TextBox.Text);
				double z1 = double.Parse(z1TextBox.Text);
			
				graph.Model.FixYtoX = fixytox.Checked;
				graph.SetRange(x0, x1, y0, y1, z0, z1);
				Reset();
			} catch {
				DialogResult res = MessageBox.Show("One of the entered numbers is invalid.");
				throw new System.Exception();
			}
		}

		private void numberKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && ((int)e.KeyChar >= (int)' ') && (e.KeyChar != '.') &&
				(e.KeyChar != ',') && (e.KeyChar != '-');
		}

		private void okClick(object sender, System.EventArgs e)
		{
			try { Apply(); this.Hide(); }
			catch {}
			Reset();
		}

		private void applyClick(object sender, System.EventArgs e)
		{
			try { Apply(); }
			catch {}
			Reset();
		}

		private void cancelClick(object sender, System.EventArgs e)
		{
			this.Hide();
		}

	}
}
