#region Copyright 2006 Christoph Rüegg [GNU Public License]
/*
A Logic Multi Operator Training Demonstration using NeuroBox Neural Network Library
Copyright (c) 2006, Christoph Daniel Rueegg, http://cdrnet.net/.
All rights reserved.

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

namespace NeuroBox.Demo.LogicMultiTraining
{
	public class Form1 : System.Windows.Forms.Form
	{
		Backend backend;
		double[] inputMatrix;
		double[] desiredMatrix;

		private NeuroBox.Demo.LogicMultiTraining.Pattern[] pattern;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtEpochs;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.ProgressBar progBar;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox chkBias;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label lblHiddenNeurons;
		private System.Windows.Forms.CheckBox chkGrouped;
		private System.Windows.Forms.Label label5;

		private System.ComponentModel.Container components = null;

		public Form1()
		{
			InitializeComponent();

			backend = new Backend();

			inputMatrix = new double[8];
			desiredMatrix = new double[4];

			pattern = new Pattern[16];
			string[] names = backend.PatternNames();
			for(int i=0;i<16;i++)
			{
				pattern[i] = new Pattern();
				pattern[i].Title = names[i];
				pattern[i].Location = new Point(0,144 + i*13);
				pattern[i].UpdateDesired(backend.GetOutputFF(i),backend.GetOutputFT(i),backend.GetOutputTF(i),backend.GetOutputTT(i));
				Controls.Add(pattern[i]);
			}

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
			this.label4 = new System.Windows.Forms.Label();
			this.btnStart = new System.Windows.Forms.Button();
			this.progBar = new System.Windows.Forms.ProgressBar();
			this.label8 = new System.Windows.Forms.Label();
			this.chkBias = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.lblHiddenNeurons = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.chkGrouped = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(536, 32);
			this.label1.TabIndex = 0;
			this.label1.Text = "Neural Network Training: Multi Logic Operators";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Epochs:";
			// 
			// txtEpochs
			// 
			this.txtEpochs.Location = new System.Drawing.Point(16, 80);
			this.txtEpochs.Name = "txtEpochs";
			this.txtEpochs.Size = new System.Drawing.Size(104, 21);
			this.txtEpochs.TabIndex = 2;
			this.txtEpochs.Text = "100";
			this.txtEpochs.TextChanged += new System.EventHandler(this.txtEpochs_TextChanged);
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Red;
			this.label4.Location = new System.Drawing.Point(160, 131);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 13);
			this.label4.TabIndex = 41;
			this.label4.Text = "X= -5";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnStart
			// 
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnStart.Location = new System.Drawing.Point(136, 80);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(384, 24);
			this.btnStart.TabIndex = 42;
			this.btnStart.Text = "Start";
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// progBar
			// 
			this.progBar.Location = new System.Drawing.Point(16, 376);
			this.progBar.Name = "progBar";
			this.progBar.Size = new System.Drawing.Size(504, 16);
			this.progBar.TabIndex = 43;
			// 
			// label8
			// 
			this.label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label8.Location = new System.Drawing.Point(16, 392);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(504, 16);
			this.label8.TabIndex = 45;
			this.label8.Text = "GPL Copyleft © 2006 Christoph Rüegg, http://www.cdrnet.net/projects/neuro/";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkBias
			// 
			this.chkBias.Checked = true;
			this.chkBias.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkBias.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkBias.Location = new System.Drawing.Point(136, 56);
			this.chkBias.Name = "chkBias";
			this.chkBias.Size = new System.Drawing.Size(16, 16);
			this.chkBias.TabIndex = 48;
			this.chkBias.CheckedChanged += new System.EventHandler(this.chkBias_CheckedChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(152, 56);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(64, 16);
			this.label9.TabIndex = 1;
			this.label9.Text = "Bias Neuron";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblHiddenNeurons
			// 
			this.lblHiddenNeurons.Location = new System.Drawing.Point(368, 56);
			this.lblHiddenNeurons.Name = "lblHiddenNeurons";
			this.lblHiddenNeurons.Size = new System.Drawing.Size(152, 16);
			this.lblHiddenNeurons.TabIndex = 1;
			this.lblHiddenNeurons.Text = "Hidden Neurons: 64 (4x16)";
			this.lblHiddenNeurons.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(205, 131);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 13);
			this.label3.TabIndex = 41;
			this.label3.Text = "Y= -5";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Lime;
			this.label11.Location = new System.Drawing.Point(295, 131);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(45, 13);
			this.label11.TabIndex = 41;
			this.label11.Text = "Y= 5";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.Red;
			this.label12.Location = new System.Drawing.Point(250, 131);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(45, 13);
			this.label12.TabIndex = 41;
			this.label12.Text = "X= -5";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label13
			// 
			this.label13.BackColor = System.Drawing.Color.Red;
			this.label13.Location = new System.Drawing.Point(385, 131);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(45, 13);
			this.label13.TabIndex = 41;
			this.label13.Text = "Y= -5";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label14
			// 
			this.label14.BackColor = System.Drawing.Color.Lime;
			this.label14.Location = new System.Drawing.Point(340, 131);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(45, 13);
			this.label14.TabIndex = 41;
			this.label14.Text = "X= 5";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label15
			// 
			this.label15.BackColor = System.Drawing.Color.Lime;
			this.label15.Location = new System.Drawing.Point(475, 131);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(45, 13);
			this.label15.TabIndex = 41;
			this.label15.Text = "Y= 5";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label16
			// 
			this.label16.BackColor = System.Drawing.Color.Lime;
			this.label16.Location = new System.Drawing.Point(430, 131);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(45, 13);
			this.label16.TabIndex = 41;
			this.label16.Text = "X= 5";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkGrouped
			// 
			this.chkGrouped.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkGrouped.Location = new System.Drawing.Point(232, 56);
			this.chkGrouped.Name = "chkGrouped";
			this.chkGrouped.Size = new System.Drawing.Size(16, 16);
			this.chkGrouped.TabIndex = 48;
			this.chkGrouped.CheckedChanged += new System.EventHandler(this.chkGrouped_CheckedChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(248, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 16);
			this.label5.TabIndex = 1;
			this.label5.Text = "Grouped Hidden Layer";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(530, 421);
			this.Controls.Add(this.chkBias);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.progBar);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.txtEpochs);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.lblHiddenNeurons);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.chkGrouped);
			this.Controls.Add(this.label5);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "NeuroBox Demo - Multi Logic Operators";
			this.ResumeLayout(false);

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

			//lblHiddenNeurons.Text = "Hidden Neurons: " + backend.HiddenNeuronCount;

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
			for(int i=0;i<16;i++)
			{
				pattern[i].OutputFF = Math.Round(output[i],8).ToString();
				pattern[i].OutputFT = Math.Round(output[i+16],8).ToString();
				pattern[i].OutputTF = Math.Round(output[i+32],8).ToString();
				pattern[i].OutputTT = Math.Round(output[i+48],8).ToString();
			}

			Cursor.Current = Cursors.Default;
		}

		private void txtEpochs_TextChanged(object sender, System.EventArgs e)
		{
			ResetOut();
		}

		private void ResetOut()
		{
			/*
			progBar.Value = 0;
			txtO1.Text = "";
			txtO2.Text = "";
			txtO3.Text = "";
			txtO4.Text = "";
			*/
		}

		private void chkBias_CheckedChanged(object sender, System.EventArgs e)
		{
			backend.BiasNeuron = chkBias.Checked;
		}

		private void chkGrouped_CheckedChanged(object sender, System.EventArgs e)
		{
			backend.GroupedHiddenLayer = chkGrouped.Checked;
			if(chkGrouped.Checked)
				lblHiddenNeurons.Text = "Hidden Neurons: 64 (4x16)";
			else
				lblHiddenNeurons.Text = "Hidden Neurons: 32";
		}
	}
}
