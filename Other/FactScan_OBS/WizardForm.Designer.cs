/* -----------------------------------------------
 * WizardForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace FacScan
{
	partial class WizardForm
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txbFactFile = new System.Windows.Forms.TextBox();
			this.rbnTFD = new System.Windows.Forms.RadioButton();
			this.rbnTransFact = new System.Windows.Forms.RadioButton();
			this.rbnCust = new System.Windows.Forms.RadioButton();
			this.button3 = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rbnUseDB = new System.Windows.Forms.RadioButton();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.txbSeq = new System.Windows.Forms.TextBox();
			this.rbnSeq = new System.Windows.Forms.RadioButton();
			this.rbnSeqFile = new System.Windows.Forms.RadioButton();
			this.txbSeqFile = new System.Windows.Forms.TextBox();
			this.button4 = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.opnFD = new System.Windows.Forms.OpenFileDialog();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Times New Roman", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label1.Location = new System.Drawing.Point(112, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(304, 32);
			this.label1.TabIndex = 0;
			this.label1.Text = "FactScan Wizard";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txbFactFile);
			this.groupBox2.Controls.Add(this.rbnTFD);
			this.groupBox2.Controls.Add(this.rbnTransFact);
			this.groupBox2.Controls.Add(this.rbnCust);
			this.groupBox2.Controls.Add(this.button3);
			this.groupBox2.Location = new System.Drawing.Point(24, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(504, 56);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "1. DataBase";
			// 
			// txbFactFile
			// 
			this.txbFactFile.Enabled = false;
			this.txbFactFile.Location = new System.Drawing.Point(264, 24);
			this.txbFactFile.Name = "txbFactFile";
			this.txbFactFile.Size = new System.Drawing.Size(200, 22);
			this.txbFactFile.TabIndex = 6;
			this.txbFactFile.Text = "SampleSites.fdb";
			this.txbFactFile.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txbFactFile_MouseUp);
			// 
			// rbnTFD
			// 
			this.rbnTFD.Location = new System.Drawing.Point(24, 24);
			this.rbnTFD.Name = "rbnTFD";
			this.rbnTFD.Size = new System.Drawing.Size(56, 24);
			this.rbnTFD.TabIndex = 3;
			this.rbnTFD.Text = "TFD";
			this.rbnTFD.CheckedChanged += new System.EventHandler(this.rbnTFD_CheckedChanged);
			// 
			// rbnTransFact
			// 
			this.rbnTransFact.Location = new System.Drawing.Point(80, 24);
			this.rbnTransFact.Name = "rbnTransFact";
			this.rbnTransFact.Size = new System.Drawing.Size(80, 24);
			this.rbnTransFact.TabIndex = 4;
			this.rbnTransFact.Text = "TransFac";
			this.rbnTransFact.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
			// 
			// rbnCust
			// 
			this.rbnCust.Checked = true;
			this.rbnCust.Location = new System.Drawing.Point(176, 24);
			this.rbnCust.Name = "rbnCust";
			this.rbnCust.Size = new System.Drawing.Size(72, 24);
			this.rbnCust.TabIndex = 5;
			this.rbnCust.TabStop = true;
			this.rbnCust.Text = "Custom";
			this.rbnCust.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button3.Location = new System.Drawing.Point(464, 24);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(24, 24);
			this.button3.TabIndex = 7;
			this.button3.Text = "...";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rbnUseDB);
			this.groupBox3.Controls.Add(this.linkLabel1);
			this.groupBox3.Controls.Add(this.txbSeq);
			this.groupBox3.Controls.Add(this.rbnSeq);
			this.groupBox3.Controls.Add(this.rbnSeqFile);
			this.groupBox3.Controls.Add(this.txbSeqFile);
			this.groupBox3.Controls.Add(this.button4);
			this.groupBox3.Location = new System.Drawing.Point(24, 128);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(504, 264);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "2. Sequences";
			// 
			// rbnUseDB
			// 
			this.rbnUseDB.Location = new System.Drawing.Point(8, 232);
			this.rbnUseDB.Name = "rbnUseDB";
			this.rbnUseDB.Size = new System.Drawing.Size(416, 24);
			this.rbnUseDB.TabIndex = 14;
			this.rbnUseDB.Text = "Extract sequences by gene names (need 9999 database)";
			this.rbnUseDB.CheckedChanged += new System.EventHandler(this.rbnUseDB_CheckedChanged);
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(208, 60);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(112, 16);
			this.linkLabel1.TabIndex = 12;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "FASTA";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// txbSeq
			// 
			this.txbSeq.Enabled = false;
			this.txbSeq.Location = new System.Drawing.Point(16, 80);
			this.txbSeq.Multiline = true;
			this.txbSeq.Name = "txbSeq";
			this.txbSeq.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txbSeq.Size = new System.Drawing.Size(456, 144);
			this.txbSeq.TabIndex = 13;
			this.txbSeq.Text = "Paste seuqences in FASTA format";
			this.txbSeq.WordWrap = false;
			this.txbSeq.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox2_MouseUp);
			// 
			// rbnSeq
			// 
			this.rbnSeq.Location = new System.Drawing.Point(8, 56);
			this.rbnSeq.Name = "rbnSeq";
			this.rbnSeq.Size = new System.Drawing.Size(208, 24);
			this.rbnSeq.TabIndex = 11;
			this.rbnSeq.Text = "Sequences (in FASTA format)";
			this.rbnSeq.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
			// 
			// rbnSeqFile
			// 
			this.rbnSeqFile.Checked = true;
			this.rbnSeqFile.Location = new System.Drawing.Point(8, 24);
			this.rbnSeqFile.Name = "rbnSeqFile";
			this.rbnSeqFile.Size = new System.Drawing.Size(48, 24);
			this.rbnSeqFile.TabIndex = 8;
			this.rbnSeqFile.TabStop = true;
			this.rbnSeqFile.Text = "File";
			this.rbnSeqFile.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
			// 
			// txbSeqFile
			// 
			this.txbSeqFile.Location = new System.Drawing.Point(56, 24);
			this.txbSeqFile.Name = "txbSeqFile";
			this.txbSeqFile.Size = new System.Drawing.Size(408, 22);
			this.txbSeqFile.TabIndex = 9;
			this.txbSeqFile.Text = "SampleSequences19.sdb";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(464, 24);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(24, 24);
			this.button4.TabIndex = 10;
			this.button4.Text = "...";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(296, 416);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(104, 24);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(24, 416);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(104, 24);
			this.button2.TabIndex = 15;
			this.button2.Text = "Help";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(408, 416);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(104, 24);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// WizardForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(550, 459);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WizardForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Wizard";
			this.Closed += new System.EventHandler(this.WizardForm_Closed);
			this.Load += new System.EventHandler(this.WizardForm_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		private Button btnCancel;
		private Button btnOK;
		private Button button2;
		private Button button3;
		private Button button4;
		private Container components;
		private GroupBox groupBox2;
		private GroupBox groupBox3;
		private Label label1;
		private LinkLabel linkLabel1;
		private OpenFileDialog opnFD;
		private RadioButton rbnCust;
		public RadioButton rbnSeq;
		public RadioButton rbnSeqFile;
		private RadioButton rbnTFD;
		private RadioButton rbnTransFact;
		public RadioButton rbnUseDB;
		public TextBox txbFactFile;
		public TextBox txbSeq;
		public TextBox txbSeqFile;
	}
}
