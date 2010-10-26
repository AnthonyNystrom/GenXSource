namespace DotMatrix
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._timer = new System.Windows.Forms.Timer(this.components);
			this._timeMatrix = new Genetibase.Shared.Controls.NuGenMatrixLabel();
			this._dateMatrix = new Genetibase.Shared.Controls.NuGenMatrixLabel();
			this._counterMatrix = new Genetibase.Shared.Controls.NuGenMatrixLabel();
			this._timer2 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// _timer
			// 
			this._timer.Interval = 1000;
			this._timer.Tick += new System.EventHandler(this._timer_Tick);
			// 
			// _timeMatrix
			// 
			this._timeMatrix.AutoSize = true;
			this._timeMatrix.DotHeight = 8;
			this._timeMatrix.DotWidth = 8;
			this._timeMatrix.Location = new System.Drawing.Point(12, 12);
			this._timeMatrix.Name = "_timeMatrix";
			this._timeMatrix.OnColor = System.Drawing.Color.Red;
			this._timeMatrix.OnColorShadow = System.Drawing.Color.Brown;
			this._timeMatrix.Size = new System.Drawing.Size(48, 40);
			this._timeMatrix.TabIndex = 0;
			this._timeMatrix.Text = " ";
			// 
			// _dateMatrix
			// 
			this._dateMatrix.AutoSize = true;
			this._dateMatrix.DotWidth = 3;
			this._dateMatrix.Location = new System.Drawing.Point(12, 58);
			this._dateMatrix.Name = "_dateMatrix";
			this._dateMatrix.OffColor = System.Drawing.Color.Navy;
			this._dateMatrix.OnColorShadow = System.Drawing.Color.Blue;
			this._dateMatrix.Size = new System.Drawing.Size(18, 25);
			this._dateMatrix.TabIndex = 1;
			this._dateMatrix.Text = " ";
			// 
			// _counterMatrix
			// 
			this._counterMatrix.AutoSize = true;
			this._counterMatrix.DotHeight = 10;
			this._counterMatrix.DotWidth = 10;
			this._counterMatrix.Location = new System.Drawing.Point(12, 89);
			this._counterMatrix.Name = "_counterMatrix";
			this._counterMatrix.Size = new System.Drawing.Size(60, 50);
			this._counterMatrix.TabIndex = 2;
			this._counterMatrix.Text = " ";
			// 
			// _timer2
			// 
			this._timer2.Enabled = true;
			this._timer2.Interval = 50;
			this._timer2.Tick += new System.EventHandler(this._timer2_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(582, 386);
			this.Controls.Add(this._counterMatrix);
			this.Controls.Add(this._dateMatrix);
			this.Controls.Add(this._timeMatrix);
			this.Name = "MainForm";
			this.Text = "DotMatrix";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Genetibase.Shared.Controls.NuGenMatrixLabel _timeMatrix;
		private System.Windows.Forms.Timer _timer;
		private Genetibase.Shared.Controls.NuGenMatrixLabel _dateMatrix;
		private Genetibase.Shared.Controls.NuGenMatrixLabel _counterMatrix;
		private System.Windows.Forms.Timer _timer2;
	}
}

