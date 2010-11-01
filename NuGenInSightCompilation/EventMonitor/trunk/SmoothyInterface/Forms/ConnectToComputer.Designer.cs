namespace SmoothyInterface.Forms
{
	partial class ConnectToComputer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectToComputer));
            this.label1 = new System.Windows.Forms.Label();
            this.dlComputerName = new Janus.Windows.EditControls.UIComboBox();
            this.btnConnect = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Computer Name :";
            // 
            // dlComputerName
            // 
            this.dlComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dlComputerName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.dlComputerName.Location = new System.Drawing.Point(107, 11);
            this.dlComputerName.Name = "dlComputerName";
            this.dlComputerName.Size = new System.Drawing.Size(213, 20);
            this.dlComputerName.TabIndex = 4;
            this.dlComputerName.TextChanged += new System.EventHandler(this.dlComputerName_TextChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(107, 49);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Location = new System.Drawing.Point(188, 49);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(75, 23);
            this.uiButton2.TabIndex = 5;
            this.uiButton2.Text = "Cancel";
            this.uiButton2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ConnectToComputer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 85);
            this.Controls.Add(this.uiButton2);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.dlComputerName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectToComputer";
            this.Text = "Connect To Computer";
            this.Load += new System.EventHandler(this.ConnectToComputer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UIComboBox dlComputerName;
        private Janus.Windows.EditControls.UIButton btnConnect;
        private Janus.Windows.EditControls.UIButton uiButton2;
	}
}