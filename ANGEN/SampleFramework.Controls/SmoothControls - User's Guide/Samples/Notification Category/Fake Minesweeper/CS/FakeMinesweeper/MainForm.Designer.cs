namespace FakeMinesweeper
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
			this._segmentOne = new Genetibase.Shared.Controls.NuGenSegDisplay();
			this._segmentTwo = new Genetibase.Shared.Controls.NuGenSegDisplay();
			this._segmentThree = new Genetibase.Shared.Controls.NuGenSegDisplay();
			this._goButton = new System.Windows.Forms.Button();
			this._timer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// _segmentOne
			// 
			this._segmentOne.Brightness = 3F;
			this._segmentOne.ColorLEDOff = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._segmentOne.ColorLEDOn = System.Drawing.Color.Red;
			this._segmentOne.Location = new System.Drawing.Point(11, 12);
			this._segmentOne.Name = "_segmentOne";
			this._segmentOne.Size = new System.Drawing.Size(18, 30);
			this._segmentOne.TabIndex = 0;
			this._segmentOne.Text = "nuGenSegDisplay1";
			this._segmentOne.Value = 0;
			// 
			// _segmentTwo
			// 
			this._segmentTwo.Brightness = 3F;
			this._segmentTwo.ColorLEDOff = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._segmentTwo.ColorLEDOn = System.Drawing.Color.Red;
			this._segmentTwo.Location = new System.Drawing.Point(27, 12);
			this._segmentTwo.Name = "_segmentTwo";
			this._segmentTwo.Size = new System.Drawing.Size(18, 30);
			this._segmentTwo.TabIndex = 1;
			this._segmentTwo.Text = "nuGenSegDisplay2";
			this._segmentTwo.Value = 0;
			// 
			// _segmentThree
			// 
			this._segmentThree.Brightness = 3F;
			this._segmentThree.ColorLEDOff = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._segmentThree.ColorLEDOn = System.Drawing.Color.Red;
			this._segmentThree.Location = new System.Drawing.Point(43, 12);
			this._segmentThree.Name = "_segmentThree";
			this._segmentThree.Size = new System.Drawing.Size(18, 30);
			this._segmentThree.TabIndex = 2;
			this._segmentThree.Text = "nuGenSegDisplay3";
			this._segmentThree.Value = 0;
			// 
			// _goButton
			// 
			this._goButton.Location = new System.Drawing.Point(67, 12);
			this._goButton.Name = "_goButton";
			this._goButton.Size = new System.Drawing.Size(35, 30);
			this._goButton.TabIndex = 3;
			this._goButton.Text = "&Go";
			this._goButton.UseVisualStyleBackColor = true;
			this._goButton.Click += new System.EventHandler(this._goButton_Click);
			// 
			// _timer
			// 
			this._timer.Tick += new System.EventHandler(this._timer_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this._goButton);
			this.Controls.Add(this._segmentThree);
			this.Controls.Add(this._segmentTwo);
			this.Controls.Add(this._segmentOne);
			this.Name = "MainForm";
			this.Text = "Fake Minesweeper";
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.Shared.Controls.NuGenSegDisplay _segmentOne;
		private Genetibase.Shared.Controls.NuGenSegDisplay _segmentTwo;
		private Genetibase.Shared.Controls.NuGenSegDisplay _segmentThree;
		private System.Windows.Forms.Button _goButton;
		private System.Windows.Forms.Timer _timer;
	}
}

