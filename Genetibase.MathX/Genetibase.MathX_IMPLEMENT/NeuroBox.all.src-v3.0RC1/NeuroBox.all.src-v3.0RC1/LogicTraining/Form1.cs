#region Copyright 2003-2006 Christoph Rüegg, Leopold Rehberger [GNU Public License]
/*
A Logic Operator Training Demonstration using NeuroBox Neural Network Library
Copyright (c) 2003-2006, Leopold Rehberger, Christoph Daniel Rueegg.
http://cdrnet.net/. All rights reserved.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace NeuroBox.Demo.LogicTraining
{
	public class Form1 : System.Windows.Forms.Form
	{
		Backend backend;
		double[] inputMatrix;
		double[] desiredMatrix;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtEpochs;
		private System.Windows.Forms.TextBox txtY1;
		private System.Windows.Forms.TextBox txtX1;
		private System.Windows.Forms.TextBox txtT4;
		private System.Windows.Forms.TextBox txtT3;
		private System.Windows.Forms.TextBox txtT2;
		private System.Windows.Forms.TextBox txtT1;
		private System.Windows.Forms.TextBox txtY4;
		private System.Windows.Forms.TextBox txtY3;
		private System.Windows.Forms.TextBox txtX3;
		private System.Windows.Forms.TextBox txtY2;
		private System.Windows.Forms.TextBox txtX2;
		private System.Windows.Forms.TextBox txtO4;
		private System.Windows.Forms.TextBox txtO3;
		private System.Windows.Forms.TextBox txtO2;
		private System.Windows.Forms.TextBox txtO1;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.ProgressBar progBar;
		private System.Windows.Forms.ComboBox cmbTemplates;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox chkBias;
		private System.Windows.Forms.TextBox txtX4;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;

		private System.ComponentModel.Container components = null;

		public Form1()
		{
			InitializeComponent();

			backend = new Backend();

			inputMatrix = new double[8];
			desiredMatrix = new double[4];
		}

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

		#region Vom Windows Form-Designer generierter Code
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEpochs = new System.Windows.Forms.TextBox();
            this.txtT4 = new System.Windows.Forms.TextBox();
            this.txtT3 = new System.Windows.Forms.TextBox();
            this.txtT2 = new System.Windows.Forms.TextBox();
            this.txtT1 = new System.Windows.Forms.TextBox();
            this.txtO4 = new System.Windows.Forms.TextBox();
            this.txtO3 = new System.Windows.Forms.TextBox();
            this.txtO2 = new System.Windows.Forms.TextBox();
            this.txtY4 = new System.Windows.Forms.TextBox();
            this.txtY3 = new System.Windows.Forms.TextBox();
            this.txtX3 = new System.Windows.Forms.TextBox();
            this.txtY2 = new System.Windows.Forms.TextBox();
            this.txtX2 = new System.Windows.Forms.TextBox();
            this.txtO1 = new System.Windows.Forms.TextBox();
            this.txtY1 = new System.Windows.Forms.TextBox();
            this.txtX1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.cmbTemplates = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtX4 = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.chkBias = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(392, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Neural Network Training: Logic Operators";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Epochs:";
            // 
            // txtEpochs
            // 
            this.txtEpochs.Location = new System.Drawing.Point(16, 64);
            this.txtEpochs.Name = "txtEpochs";
            this.txtEpochs.Size = new System.Drawing.Size(128, 21);
            this.txtEpochs.TabIndex = 2;
            this.txtEpochs.Text = "100";
            this.txtEpochs.TextChanged += new System.EventHandler(this.txtEpochs_TextChanged);
            // 
            // txtT4
            // 
            this.txtT4.Location = new System.Drawing.Point(152, 240);
            this.txtT4.Name = "txtT4";
            this.txtT4.Size = new System.Drawing.Size(88, 21);
            this.txtT4.TabIndex = 37;
            this.txtT4.Text = "-1";
            this.txtT4.TextChanged += new System.EventHandler(this.txtT4_TextChanged);
            // 
            // txtT3
            // 
            this.txtT3.Location = new System.Drawing.Point(152, 216);
            this.txtT3.Name = "txtT3";
            this.txtT3.Size = new System.Drawing.Size(88, 21);
            this.txtT3.TabIndex = 36;
            this.txtT3.Text = "1";
            this.txtT3.TextChanged += new System.EventHandler(this.txtT3_TextChanged);
            // 
            // txtT2
            // 
            this.txtT2.Location = new System.Drawing.Point(152, 192);
            this.txtT2.Name = "txtT2";
            this.txtT2.Size = new System.Drawing.Size(88, 21);
            this.txtT2.TabIndex = 35;
            this.txtT2.Text = "1";
            this.txtT2.TextChanged += new System.EventHandler(this.txtT2_TextChanged);
            // 
            // txtT1
            // 
            this.txtT1.Location = new System.Drawing.Point(152, 168);
            this.txtT1.Name = "txtT1";
            this.txtT1.Size = new System.Drawing.Size(88, 21);
            this.txtT1.TabIndex = 34;
            this.txtT1.Text = "-1";
            this.txtT1.TextChanged += new System.EventHandler(this.txtT1_TextChanged);
            // 
            // txtO4
            // 
            this.txtO4.Enabled = false;
            this.txtO4.Location = new System.Drawing.Point(272, 240);
            this.txtO4.Name = "txtO4";
            this.txtO4.Size = new System.Drawing.Size(96, 21);
            this.txtO4.TabIndex = 33;
            // 
            // txtO3
            // 
            this.txtO3.Enabled = false;
            this.txtO3.Location = new System.Drawing.Point(272, 216);
            this.txtO3.Name = "txtO3";
            this.txtO3.Size = new System.Drawing.Size(96, 21);
            this.txtO3.TabIndex = 32;
            // 
            // txtO2
            // 
            this.txtO2.Enabled = false;
            this.txtO2.Location = new System.Drawing.Point(272, 192);
            this.txtO2.Name = "txtO2";
            this.txtO2.Size = new System.Drawing.Size(96, 21);
            this.txtO2.TabIndex = 31;
            // 
            // txtY4
            // 
            this.txtY4.Enabled = false;
            this.txtY4.Location = new System.Drawing.Point(80, 240);
            this.txtY4.Name = "txtY4";
            this.txtY4.Size = new System.Drawing.Size(56, 21);
            this.txtY4.TabIndex = 30;
            this.txtY4.Text = "5";
            // 
            // txtY3
            // 
            this.txtY3.Enabled = false;
            this.txtY3.Location = new System.Drawing.Point(80, 216);
            this.txtY3.Name = "txtY3";
            this.txtY3.Size = new System.Drawing.Size(56, 21);
            this.txtY3.TabIndex = 28;
            this.txtY3.Text = "-5";
            // 
            // txtX3
            // 
            this.txtX3.Enabled = false;
            this.txtX3.Location = new System.Drawing.Point(16, 216);
            this.txtX3.Name = "txtX3";
            this.txtX3.Size = new System.Drawing.Size(56, 21);
            this.txtX3.TabIndex = 27;
            this.txtX3.Text = "5";
            // 
            // txtY2
            // 
            this.txtY2.Enabled = false;
            this.txtY2.Location = new System.Drawing.Point(80, 192);
            this.txtY2.Name = "txtY2";
            this.txtY2.Size = new System.Drawing.Size(56, 21);
            this.txtY2.TabIndex = 26;
            this.txtY2.Text = "5";
            // 
            // txtX2
            // 
            this.txtX2.Enabled = false;
            this.txtX2.Location = new System.Drawing.Point(16, 192);
            this.txtX2.Name = "txtX2";
            this.txtX2.Size = new System.Drawing.Size(56, 21);
            this.txtX2.TabIndex = 25;
            this.txtX2.Text = "-5";
            // 
            // txtO1
            // 
            this.txtO1.Enabled = false;
            this.txtO1.Location = new System.Drawing.Point(272, 168);
            this.txtO1.Name = "txtO1";
            this.txtO1.Size = new System.Drawing.Size(96, 21);
            this.txtO1.TabIndex = 24;
            // 
            // txtY1
            // 
            this.txtY1.Enabled = false;
            this.txtY1.Location = new System.Drawing.Point(80, 168);
            this.txtY1.Name = "txtY1";
            this.txtY1.Size = new System.Drawing.Size(56, 21);
            this.txtY1.TabIndex = 23;
            this.txtY1.Text = "-5";
            // 
            // txtX1
            // 
            this.txtX1.Enabled = false;
            this.txtX1.Location = new System.Drawing.Point(16, 168);
            this.txtX1.Name = "txtX1";
            this.txtX1.Size = new System.Drawing.Size(56, 21);
            this.txtX1.TabIndex = 22;
            this.txtX1.Text = "-5";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(272, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 15);
            this.label4.TabIndex = 41;
            this.label4.Text = "Actual Output:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(152, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 15);
            this.label3.TabIndex = 40;
            this.label3.Text = "Desired Output:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(80, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 15);
            this.label5.TabIndex = 39;
            this.label5.Text = "Y:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 15);
            this.label6.TabIndex = 38;
            this.label6.Text = "X:";
            // 
            // btnStart
            // 
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Location = new System.Drawing.Point(160, 64);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(208, 24);
            this.btnStart.TabIndex = 42;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // progBar
            // 
            this.progBar.Location = new System.Drawing.Point(16, 280);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(352, 16);
            this.progBar.TabIndex = 43;
            // 
            // cmbTemplates
            // 
            this.cmbTemplates.Items.AddRange(new object[] {
            "AND",
            "NAND",
            "OR",
            "NOR",
            "XOR (antivalence)",
            "XNOR (equivalence)",
            "TRUE",
            "FALSE",
            "X",
            "Y",
            "NOT X",
            "NOT Y",
            "Inhibition X",
            "Inhibition Y",
            "Implication X",
            "Implication Y"});
            this.cmbTemplates.Location = new System.Drawing.Point(16, 112);
            this.cmbTemplates.MaxDropDownItems = 16;
            this.cmbTemplates.Name = "cmbTemplates";
            this.cmbTemplates.Size = new System.Drawing.Size(128, 21);
            this.cmbTemplates.TabIndex = 44;
            this.cmbTemplates.Text = "XOR (antivalence)";
            this.cmbTemplates.SelectedIndexChanged += new System.EventHandler(this.cmbTemplates_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 16);
            this.label7.TabIndex = 1;
            this.label7.Text = "Load Training Template:";
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label8.Location = new System.Drawing.Point(16, 296);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(352, 16);
            this.label8.TabIndex = 45;
            this.label8.Text = "GPL Copyleft © 2003-2006 Christoph Rüegg, Leopold Rehberger";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(15, 191);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(58, 23);
            this.panel1.TabIndex = 46;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Lime;
            this.panel2.Location = new System.Drawing.Point(79, 191);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(58, 23);
            this.panel2.TabIndex = 47;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Red;
            this.panel3.Location = new System.Drawing.Point(15, 167);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(58, 23);
            this.panel3.TabIndex = 46;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Red;
            this.panel4.Location = new System.Drawing.Point(79, 167);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(58, 23);
            this.panel4.TabIndex = 46;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Red;
            this.panel5.Location = new System.Drawing.Point(79, 215);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(58, 23);
            this.panel5.TabIndex = 46;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Lime;
            this.panel6.Location = new System.Drawing.Point(79, 239);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(58, 23);
            this.panel6.TabIndex = 47;
            // 
            // txtX4
            // 
            this.txtX4.Enabled = false;
            this.txtX4.Location = new System.Drawing.Point(16, 240);
            this.txtX4.Name = "txtX4";
            this.txtX4.Size = new System.Drawing.Size(56, 21);
            this.txtX4.TabIndex = 30;
            this.txtX4.Text = "5";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Lime;
            this.panel7.Location = new System.Drawing.Point(15, 239);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(58, 23);
            this.panel7.TabIndex = 47;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Lime;
            this.panel8.Location = new System.Drawing.Point(15, 215);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(58, 23);
            this.panel8.TabIndex = 47;
            // 
            // chkBias
            // 
            this.chkBias.Checked = true;
            this.chkBias.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBias.Location = new System.Drawing.Point(160, 112);
            this.chkBias.Name = "chkBias";
            this.chkBias.Size = new System.Drawing.Size(16, 16);
            this.chkBias.TabIndex = 48;
            this.chkBias.CheckedChanged += new System.EventHandler(this.chkBias_CheckedChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(176, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 16);
            this.label9.TabIndex = 1;
            this.label9.Text = "Bias Neuron";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(256, 112);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 16);
            this.label10.TabIndex = 1;
            this.label10.Text = "Hidden Neurons: 4";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label11.Location = new System.Drawing.Point(16, 310);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(352, 16);
            this.label11.TabIndex = 45;
            this.label11.Text = "http://www.cdrnet.net/projects/neuro/";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(384, 325);
            this.Controls.Add(this.chkBias);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbTemplates);
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtT4);
            this.Controls.Add(this.txtT3);
            this.Controls.Add(this.txtT2);
            this.Controls.Add(this.txtT1);
            this.Controls.Add(this.txtO4);
            this.Controls.Add(this.txtO3);
            this.Controls.Add(this.txtO2);
            this.Controls.Add(this.txtY4);
            this.Controls.Add(this.txtY3);
            this.Controls.Add(this.txtX3);
            this.Controls.Add(this.txtY2);
            this.Controls.Add(this.txtX2);
            this.Controls.Add(this.txtO1);
            this.Controls.Add(this.txtY1);
            this.Controls.Add(this.txtX1);
            this.Controls.Add(this.txtEpochs);
            this.Controls.Add(this.txtX4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "NeuroBox Demo - Logic Operators";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.Run(new Form1());
		}

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			//Build a new network.
			backend.BuildNetwork();

			//Update the backend's input and training data.
			inputMatrix[0] = double.Parse(txtX1.Text);
			inputMatrix[1] = double.Parse(txtY1.Text);
			inputMatrix[2] = double.Parse(txtX2.Text);
			inputMatrix[3] = double.Parse(txtY2.Text);
			inputMatrix[4] = double.Parse(txtX3.Text);
			inputMatrix[5] = double.Parse(txtY3.Text);
			inputMatrix[6] = double.Parse(txtX4.Text);
			inputMatrix[7] = double.Parse(txtY4.Text);
			desiredMatrix[0] = double.Parse(txtT1.Text);
			desiredMatrix[1] = double.Parse(txtT2.Text);
			desiredMatrix[2] = double.Parse(txtT3.Text);
			desiredMatrix[3] = double.Parse(txtT4.Text);
			backend.SetMatrix(inputMatrix,desiredMatrix);

			ResetOut();

			int epochs = int.Parse(txtEpochs.Text); //the count of rounds.
			progBar.Maximum = epochs; //Progressbar Stuff
			progBar.Step = 1;
			progBar.Value = 0;

			for(int i=0;i<epochs;i++)
			{
				backend.TrainEpoche(); //Train all 4 patterns one time.
				progBar.PerformStep(); //Update the progress bar.
			}

			double[] output = backend.EvaluateOutputs();
			txtO1.Text = output[0].ToString();
			txtO2.Text = output[1].ToString();
			txtO3.Text = output[2].ToString();
			txtO4.Text = output[3].ToString();

			Cursor.Current = Cursors.Default;
		}

		private void cmbTemplates_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch(cmbTemplates.SelectedIndex)
			{
				case 0: //AND
					txtT1.Text = "-1";
					txtT2.Text = "-1";
					txtT3.Text = "-1";
					txtT4.Text = "1";
					break;
				case 1: //NAND
					txtT1.Text = "1";
					txtT2.Text = "1";
					txtT3.Text = "1";
					txtT4.Text = "-1";
					break;
				case 2: //OR
					txtT1.Text = "-1";
					txtT2.Text = "1";
					txtT3.Text = "1";
					txtT4.Text = "1";
					break;
				case 3: //NOR
					txtT1.Text = "1";
					txtT2.Text = "-1";
					txtT3.Text = "-1";
					txtT4.Text = "-1";
					break;
				case 4: //XOR (antivalence)
					txtT1.Text = "-1";
					txtT2.Text = "1";
					txtT3.Text = "1";
					txtT4.Text = "-1";
					break;
				case 5: //XNOR (equivalence)
					txtT1.Text = "1";
					txtT2.Text = "-1";
					txtT3.Text = "-1";
					txtT4.Text = "1";
					break;
				case 6: //TRUE
					txtT1.Text = "1";
					txtT2.Text = "1";
					txtT3.Text = "1";
					txtT4.Text = "1";
					break;
				case 7: //FALSE
					txtT1.Text = "-1";
					txtT2.Text = "-1";
					txtT3.Text = "-1";
					txtT4.Text = "-1";
					break;
				case 8: //X
					txtT1.Text = "-1";
					txtT2.Text = "-1";
					txtT3.Text = "1";
					txtT4.Text = "1";
					break;
				case 9: //Y
					txtT1.Text = "-1";
					txtT2.Text = "1";
					txtT3.Text = "-1";
					txtT4.Text = "1";
					break;
				case 10: //NOT X
					txtT1.Text = "1";
					txtT2.Text = "1";
					txtT3.Text = "-1";
					txtT4.Text = "-1";
					break;
				case 11: //NOT Y
					txtT1.Text = "1";
					txtT2.Text = "-1";
					txtT3.Text = "1";
					txtT4.Text = "-1";
					break;
				case 12: //Inhibition X
					txtT1.Text = "-1";
					txtT2.Text = "1";
					txtT3.Text = "-1";
					txtT4.Text = "-1";
					break;
				case 13: //Inhibition Y
					txtT1.Text = "-1";
					txtT2.Text = "-1";
					txtT3.Text = "1";
					txtT4.Text = "-1";
					break;
				case 14: //Implication X
					txtT1.Text = "1";
					txtT2.Text = "1";
					txtT3.Text = "-1";
					txtT4.Text = "1";
					break;
				case 15: //Implication Y
					txtT1.Text = "1";
					txtT2.Text = "-1";
					txtT3.Text = "1";
					txtT4.Text = "1";
					break;
			}
			ResetOut();
		}

		private void txtT1_TextChanged(object sender, System.EventArgs e)
		{
			ResetOut();
		}

		private void txtT2_TextChanged(object sender, System.EventArgs e)
		{
			ResetOut();
		}

		private void txtT3_TextChanged(object sender, System.EventArgs e)
		{
			ResetOut();
		}

		private void txtT4_TextChanged(object sender, System.EventArgs e)
		{
			ResetOut();
		}

		private void txtEpochs_TextChanged(object sender, System.EventArgs e)
		{
			ResetOut();
		}

		private void ResetOut()
		{
			progBar.Value = 0;
			txtO1.Text = "";
			txtO2.Text = "";
			txtO3.Text = "";
			txtO4.Text = "";
		}

		private void chkBias_CheckedChanged(object sender, System.EventArgs e)
		{
			backend.BiasNeuron = chkBias.Checked;
		}
	}
}
