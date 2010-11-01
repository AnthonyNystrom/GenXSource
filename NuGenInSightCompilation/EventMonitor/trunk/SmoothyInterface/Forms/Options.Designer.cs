namespace SmoothyInterface.Forms
{
	partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.panelColorFailureAudit = new System.Windows.Forms.Panel();
            this.panelColorSuccessAudit = new System.Windows.Forms.Panel();
            this.panelColorError = new System.Windows.Forms.Panel();
            this.panelColorWarning = new System.Windows.Forms.Panel();
            this.panelColourInformation = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
            this.checkBox1 = new Janus.Windows.EditControls.UICheckBox();
            this.cbShowConfirmationsForClearingEventLogs = new Janus.Windows.EditControls.UICheckBox();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.cbEventDistinction = new Janus.Windows.EditControls.UIComboBox();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.uiButton3 = new Janus.Windows.EditControls.UIButton();
            this.uiButton4 = new Janus.Windows.EditControls.UIButton();
            this.uiButton5 = new Janus.Windows.EditControls.UIButton();
            this.uiButton6 = new Janus.Windows.EditControls.UIButton();
            this.uiButton7 = new Janus.Windows.EditControls.UIButton();
            this.uiButton8 = new Janus.Windows.EditControls.UIButton();
            this.uiButton9 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            this.uiTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelColorFailureAudit
            // 
            this.panelColorFailureAudit.BackColor = global::EventMonitor.Properties.Settings.Default.ColorFailureAudit;
            this.panelColorFailureAudit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorFailureAudit.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorFailureAudit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.panelColorFailureAudit.Location = new System.Drawing.Point(94, 135);
            this.panelColorFailureAudit.Name = "panelColorFailureAudit";
            this.panelColorFailureAudit.Size = new System.Drawing.Size(135, 22);
            this.panelColorFailureAudit.TabIndex = 9;
            // 
            // panelColorSuccessAudit
            // 
            this.panelColorSuccessAudit.BackColor = global::EventMonitor.Properties.Settings.Default.ColorSuccesfulAudit;
            this.panelColorSuccessAudit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorSuccessAudit.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorSuccesfulAudit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.panelColorSuccessAudit.Location = new System.Drawing.Point(94, 107);
            this.panelColorSuccessAudit.Name = "panelColorSuccessAudit";
            this.panelColorSuccessAudit.Size = new System.Drawing.Size(135, 22);
            this.panelColorSuccessAudit.TabIndex = 8;
            // 
            // panelColorError
            // 
            this.panelColorError.BackColor = global::EventMonitor.Properties.Settings.Default.ColorError;
            this.panelColorError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorError.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorError", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.panelColorError.Location = new System.Drawing.Point(94, 79);
            this.panelColorError.Name = "panelColorError";
            this.panelColorError.Size = new System.Drawing.Size(135, 22);
            this.panelColorError.TabIndex = 7;
            // 
            // panelColorWarning
            // 
            this.panelColorWarning.BackColor = global::EventMonitor.Properties.Settings.Default.ColorWarning;
            this.panelColorWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorWarning.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorWarning", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.panelColorWarning.Location = new System.Drawing.Point(94, 51);
            this.panelColorWarning.Name = "panelColorWarning";
            this.panelColorWarning.Size = new System.Drawing.Size(135, 22);
            this.panelColorWarning.TabIndex = 6;
            // 
            // panelColourInformation
            // 
            this.panelColourInformation.BackColor = global::EventMonitor.Properties.Settings.Default.ColorInformation;
            this.panelColourInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColourInformation.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorInformation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.panelColourInformation.Location = new System.Drawing.Point(94, 23);
            this.panelColourInformation.Name = "panelColourInformation";
            this.panelColourInformation.Size = new System.Drawing.Size(135, 22);
            this.panelColourInformation.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Failure Audit :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Success Audit :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Error :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Warning :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Information :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Event Distinction Style :";
            // 
            // uiTab1
            // 
            this.uiTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTab1.Location = new System.Drawing.Point(0, 0);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.Size = new System.Drawing.Size(303, 269);
            this.uiTab1.TabIndex = 7;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2});
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.uiButton3);
            this.uiTabPage1.Controls.Add(this.checkBox1);
            this.uiTabPage1.Controls.Add(this.uiButton4);
            this.uiTabPage1.Controls.Add(this.cbShowConfirmationsForClearingEventLogs);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 21);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(301, 247);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "Behavior";
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Controls.Add(this.uiButton2);
            this.uiTabPage2.Controls.Add(this.uiButton1);
            this.uiTabPage2.Controls.Add(this.cbEventDistinction);
            this.uiTabPage2.Controls.Add(this.label1);
            this.uiTabPage2.Controls.Add(this.uiGroupBox1);
            this.uiTabPage2.Location = new System.Drawing.Point(1, 21);
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.Size = new System.Drawing.Size(301, 247);
            this.uiTabPage2.TabStop = true;
            this.uiTabPage2.Text = "User Interface";
            // 
            // checkBox1
            // 
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(10, 35);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(194, 23);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Safeguard deletion of standard logs";
            // 
            // cbShowConfirmationsForClearingEventLogs
            // 
            this.cbShowConfirmationsForClearingEventLogs.BackColor = System.Drawing.Color.Transparent;
            this.cbShowConfirmationsForClearingEventLogs.Checked = true;
            this.cbShowConfirmationsForClearingEventLogs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowConfirmationsForClearingEventLogs.Location = new System.Drawing.Point(10, 6);
            this.cbShowConfirmationsForClearingEventLogs.Name = "cbShowConfirmationsForClearingEventLogs";
            this.cbShowConfirmationsForClearingEventLogs.Size = new System.Drawing.Size(202, 23);
            this.cbShowConfirmationsForClearingEventLogs.TabIndex = 8;
            this.cbShowConfirmationsForClearingEventLogs.Text = "Show confirmation for clearing logs";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.uiGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.Controls.Add(this.uiButton9);
            this.uiGroupBox1.Controls.Add(this.uiButton8);
            this.uiGroupBox1.Controls.Add(this.uiButton6);
            this.uiGroupBox1.Controls.Add(this.uiButton7);
            this.uiGroupBox1.Controls.Add(this.uiButton5);
            this.uiGroupBox1.Controls.Add(this.label2);
            this.uiGroupBox1.Controls.Add(this.label3);
            this.uiGroupBox1.Controls.Add(this.label4);
            this.uiGroupBox1.Controls.Add(this.label5);
            this.uiGroupBox1.Controls.Add(this.label6);
            this.uiGroupBox1.Controls.Add(this.panelColorFailureAudit);
            this.uiGroupBox1.Controls.Add(this.panelColourInformation);
            this.uiGroupBox1.Controls.Add(this.panelColorSuccessAudit);
            this.uiGroupBox1.Controls.Add(this.panelColorWarning);
            this.uiGroupBox1.Controls.Add(this.panelColorError);
            this.uiGroupBox1.Location = new System.Drawing.Point(12, 44);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(280, 165);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "Row Colors";
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiGroupBox1.Click += new System.EventHandler(this.uiGroupBox1_Click);
            // 
            // cbEventDistinction
            // 
            this.cbEventDistinction.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbEventDistinction.Location = new System.Drawing.Point(147, 11);
            this.cbEventDistinction.Name = "cbEventDistinction";
            this.cbEventDistinction.Size = new System.Drawing.Size(134, 20);
            this.cbEventDistinction.TabIndex = 2;
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiButton1.Location = new System.Drawing.Point(11, 221);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(75, 23);
            this.uiButton1.TabIndex = 3;
            this.uiButton1.Text = "OK";
            this.uiButton1.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiButton2.Location = new System.Drawing.Point(92, 221);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(75, 23);
            this.uiButton2.TabIndex = 4;
            this.uiButton2.Text = "Cancel";
            this.uiButton2.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // uiButton3
            // 
            this.uiButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiButton3.Location = new System.Drawing.Point(92, 221);
            this.uiButton3.Name = "uiButton3";
            this.uiButton3.Size = new System.Drawing.Size(75, 23);
            this.uiButton3.TabIndex = 9;
            this.uiButton3.Text = "Cancel";
            this.uiButton3.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // uiButton4
            // 
            this.uiButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiButton4.Location = new System.Drawing.Point(11, 221);
            this.uiButton4.Name = "uiButton4";
            this.uiButton4.Size = new System.Drawing.Size(75, 23);
            this.uiButton4.TabIndex = 8;
            this.uiButton4.Text = "OK";
            this.uiButton4.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // uiButton5
            // 
            this.uiButton5.Image = global::EventMonitor.Properties.Resources.color_wheel;
            this.uiButton5.Location = new System.Drawing.Point(235, 22);
            this.uiButton5.Name = "uiButton5";
            this.uiButton5.Size = new System.Drawing.Size(30, 25);
            this.uiButton5.TabIndex = 5;
            this.uiButton5.Click += new System.EventHandler(this.btnChooseInformation_Click);
            // 
            // uiButton6
            // 
            this.uiButton6.Image = global::EventMonitor.Properties.Resources.color_wheel;
            this.uiButton6.Location = new System.Drawing.Point(235, 50);
            this.uiButton6.Name = "uiButton6";
            this.uiButton6.Size = new System.Drawing.Size(30, 25);
            this.uiButton6.TabIndex = 5;
            this.uiButton6.Click += new System.EventHandler(this.btnChooseWarning_Click);
            // 
            // uiButton7
            // 
            this.uiButton7.Image = global::EventMonitor.Properties.Resources.color_wheel;
            this.uiButton7.Location = new System.Drawing.Point(235, 78);
            this.uiButton7.Name = "uiButton7";
            this.uiButton7.Size = new System.Drawing.Size(30, 25);
            this.uiButton7.TabIndex = 5;
            this.uiButton7.Click += new System.EventHandler(this.btnChooseError_Click);
            // 
            // uiButton8
            // 
            this.uiButton8.Image = global::EventMonitor.Properties.Resources.color_wheel;
            this.uiButton8.Location = new System.Drawing.Point(235, 106);
            this.uiButton8.Name = "uiButton8";
            this.uiButton8.Size = new System.Drawing.Size(30, 25);
            this.uiButton8.TabIndex = 5;
            this.uiButton8.Click += new System.EventHandler(this.btnChooseSucces_Click);
            // 
            // uiButton9
            // 
            this.uiButton9.Image = global::EventMonitor.Properties.Resources.color_wheel;
            this.uiButton9.Location = new System.Drawing.Point(235, 134);
            this.uiButton9.Name = "uiButton9";
            this.uiButton9.Size = new System.Drawing.Size(30, 25);
            this.uiButton9.TabIndex = 5;
            this.uiButton9.Click += new System.EventHandler(this.btnFailureAudit_Click);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 269);
            this.Controls.Add(this.uiTab1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowInTaskbar = false;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage2.ResumeLayout(false);
            this.uiTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelColorFailureAudit;
		private System.Windows.Forms.Panel panelColorSuccessAudit;
		private System.Windows.Forms.Panel panelColorError;
		private System.Windows.Forms.Panel panelColorWarning;
		private System.Windows.Forms.Panel panelColourInformation;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog colorDialog;
        private Janus.Windows.UI.Tab.UITab uiTab1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage2;
        private Janus.Windows.EditControls.UICheckBox checkBox1;
        private Janus.Windows.EditControls.UICheckBox cbShowConfirmationsForClearingEventLogs;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UIComboBox cbEventDistinction;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.EditControls.UIButton uiButton3;
        private Janus.Windows.EditControls.UIButton uiButton4;
        private Janus.Windows.EditControls.UIButton uiButton6;
        private Janus.Windows.EditControls.UIButton uiButton5;
        private Janus.Windows.EditControls.UIButton uiButton8;
        private Janus.Windows.EditControls.UIButton uiButton7;
        private Janus.Windows.EditControls.UIButton uiButton9;
	}
}