using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace NeuroBox.Demo.LogicMultiTraining
{
	public class Pattern : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel pnlFF;
		private System.Windows.Forms.Panel pnlFT;
		private System.Windows.Forms.Panel pnlTF;
		private System.Windows.Forms.Panel pnlTT;
		private System.Windows.Forms.Label lblFF;
		private System.Windows.Forms.Label lblTF;
		private System.Windows.Forms.Label lblFT;
		private System.Windows.Forms.Label lblTT;

		private System.ComponentModel.Container components = null;

		public Pattern()
		{
			InitializeComponent();
		}

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

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlFF = new System.Windows.Forms.Panel();
			this.pnlFT = new System.Windows.Forms.Panel();
			this.pnlTF = new System.Windows.Forms.Panel();
			this.pnlTT = new System.Windows.Forms.Panel();
			this.lblFF = new System.Windows.Forms.Label();
			this.lblTF = new System.Windows.Forms.Label();
			this.lblFT = new System.Windows.Forms.Label();
			this.lblTT = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblTitle
			// 
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(152, 13);
			this.lblTitle.TabIndex = 45;
			this.lblTitle.Text = "XOR (antivalence):";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pnlFF
			// 
			this.pnlFF.BackColor = System.Drawing.Color.Lime;
			this.pnlFF.Location = new System.Drawing.Point(160, 0);
			this.pnlFF.Name = "pnlFF";
			this.pnlFF.Size = new System.Drawing.Size(10, 13);
			this.pnlFF.TabIndex = 48;
			// 
			// pnlFT
			// 
			this.pnlFT.BackColor = System.Drawing.Color.Red;
			this.pnlFT.Location = new System.Drawing.Point(250, 0);
			this.pnlFT.Name = "pnlFT";
			this.pnlFT.Size = new System.Drawing.Size(10, 13);
			this.pnlFT.TabIndex = 49;
			// 
			// pnlTF
			// 
			this.pnlTF.BackColor = System.Drawing.Color.Lime;
			this.pnlTF.Location = new System.Drawing.Point(340, 0);
			this.pnlTF.Name = "pnlTF";
			this.pnlTF.Size = new System.Drawing.Size(10, 13);
			this.pnlTF.TabIndex = 48;
			// 
			// pnlTT
			// 
			this.pnlTT.BackColor = System.Drawing.Color.Red;
			this.pnlTT.Location = new System.Drawing.Point(430, 0);
			this.pnlTT.Name = "pnlTT";
			this.pnlTT.Size = new System.Drawing.Size(10, 13);
			this.pnlTT.TabIndex = 49;
			// 
			// lblFF
			// 
			this.lblFF.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblFF.Location = new System.Drawing.Point(170, 0);
			this.lblFF.Name = "lblFF";
			this.lblFF.Size = new System.Drawing.Size(80, 13);
			this.lblFF.TabIndex = 45;
			this.lblFF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblTF
			// 
			this.lblTF.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblTF.Location = new System.Drawing.Point(350, 0);
			this.lblTF.Name = "lblTF";
			this.lblTF.Size = new System.Drawing.Size(80, 13);
			this.lblTF.TabIndex = 45;
			this.lblTF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblFT
			// 
			this.lblFT.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblFT.Location = new System.Drawing.Point(260, 0);
			this.lblFT.Name = "lblFT";
			this.lblFT.Size = new System.Drawing.Size(80, 13);
			this.lblFT.TabIndex = 45;
			this.lblFT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblTT
			// 
			this.lblTT.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblTT.Location = new System.Drawing.Point(440, 0);
			this.lblTT.Name = "lblTT";
			this.lblTT.Size = new System.Drawing.Size(80, 13);
			this.lblTT.TabIndex = 45;
			this.lblTT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Pattern
			// 
			this.Controls.Add(this.lblTT);
			this.Controls.Add(this.lblFT);
			this.Controls.Add(this.lblTF);
			this.Controls.Add(this.lblFF);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.pnlFF);
			this.Controls.Add(this.pnlFT);
			this.Controls.Add(this.pnlTF);
			this.Controls.Add(this.pnlTT);
			this.Name = "Pattern";
			this.Size = new System.Drawing.Size(520, 13);
			this.ResumeLayout(false);

		}
		#endregion


		public string Title
		{
			get {return lblTitle.Text;}
			set {lblTitle.Text = value;}
		}
		public string OutputFF
		{
			get {return lblFF.Text;}
			set {UpdateOutput(value,lblFF,pnlFF);}
		}
		public string OutputFT
		{
			get {return lblFT.Text;}
			set {UpdateOutput(value,lblFT,pnlFT);}
		}
		public string OutputTF
		{
			get {return lblTF.Text;}
			set {UpdateOutput(value,lblTF,pnlTF);}
		}
		public string OutputTT
		{
			get {return lblTT.Text;}
			set {UpdateOutput(value,lblTT,pnlTT);}
		}

		public void UpdateDesired(double ff, double ft, double tf, double tt)
		{
			if(ff < 0)
				pnlFF.BackColor = Color.Red;
			else
				pnlFF.BackColor = Color.Lime;
			if(ft < 0)
				pnlFT.BackColor = Color.Red;
			else
				pnlFT.BackColor = Color.Lime;
			if(tf < 0)
				pnlTF.BackColor = Color.Red;
			else
				pnlTF.BackColor = Color.Lime;
			if(tt < 0)
				pnlTT.BackColor = Color.Red;
			else
				pnlTT.BackColor = Color.Lime;
		}

		private void UpdateOutput(string output, Label box, Panel panel)
		{
			box.Text = output;
			if(output[0] == '-' && panel.BackColor == Color.Red
				|| output[0] != '-' && panel.BackColor == Color.Lime)
				box.ForeColor = Color.DarkGreen;
			else
				box.ForeColor = Color.Red;
			/*
			if(output[0] == '-' && panel.BackColor == Color.Red
				|| output[0] != '-' && panel.BackColor == Color.Lime)
				box.BackColor = Color.Lime;
			else
				box.BackColor = Color.Red;
			*/
		}

	}
}
