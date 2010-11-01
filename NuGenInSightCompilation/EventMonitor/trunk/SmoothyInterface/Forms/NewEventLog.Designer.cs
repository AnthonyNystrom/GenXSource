namespace SmoothyInterface.Forms
{
	partial class NewEventLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewEventLog));
            this.label1 = new System.Windows.Forms.Label();
            this.tbEventLogName = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbSourceName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Event Log Name :";
            // 
            // tbEventLogName
            // 
            this.tbEventLogName.Location = new System.Drawing.Point(111, 8);
            this.tbEventLogName.Name = "tbEventLogName";
            this.tbEventLogName.Size = new System.Drawing.Size(213, 20);
            this.tbEventLogName.TabIndex = 3;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // tbSourceName
            // 
            this.tbSourceName.Location = new System.Drawing.Point(111, 34);
            this.tbSourceName.Name = "tbSourceName";
            this.tbSourceName.Size = new System.Drawing.Size(213, 20);
            this.tbSourceName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Source Name :";
            // 
            // uiButton1
            // 
            this.uiButton1.Location = new System.Drawing.Point(93, 64);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(75, 23);
            this.uiButton1.TabIndex = 6;
            this.uiButton1.Text = "Create";
            this.uiButton1.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Location = new System.Drawing.Point(174, 64);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(75, 23);
            this.uiButton2.TabIndex = 7;
            this.uiButton2.Text = "Cancel";
            this.uiButton2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // NewEventLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 95);
            this.Controls.Add(this.uiButton2);
            this.Controls.Add(this.uiButton1);
            this.Controls.Add(this.tbSourceName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbEventLogName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewEventLog";
            this.ShowInTaskbar = false;
            this.Text = "New Event Log";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbEventLogName;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.TextBox tbSourceName;
		private System.Windows.Forms.Label label2;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
	}
}