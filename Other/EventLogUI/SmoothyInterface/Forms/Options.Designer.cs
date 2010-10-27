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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tpUI = new System.Windows.Forms.TabControl();
			this.tpBehaviour = new System.Windows.Forms.TabPage();
			this.cbShowConfirmationsForClearingEventLogs = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnFailureAudit = new System.Windows.Forms.Button();
			this.btnChooseSucces = new System.Windows.Forms.Button();
			this.btnChooseError = new System.Windows.Forms.Button();
			this.btnChooseWarning = new System.Windows.Forms.Button();
			this.btnChooseInformation = new System.Windows.Forms.Button();
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
			this.cbEventDistinction = new System.Windows.Forms.ComboBox();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.panel1.SuspendLayout();
			this.tpUI.SuspendLayout();
			this.tpBehaviour.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(68, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(149, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnOK);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 276);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(301, 29);
			this.panel1.TabIndex = 5;
			// 
			// tpUI
			// 
			this.tpUI.Controls.Add(this.tpBehaviour);
			this.tpUI.Controls.Add(this.tabPage2);
			this.tpUI.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tpUI.Location = new System.Drawing.Point(0, 0);
			this.tpUI.Name = "tpUI";
			this.tpUI.SelectedIndex = 0;
			this.tpUI.Size = new System.Drawing.Size(301, 276);
			this.tpUI.TabIndex = 6;
			// 
			// tpBehaviour
			// 
			this.tpBehaviour.Controls.Add(this.cbShowConfirmationsForClearingEventLogs);
			this.tpBehaviour.Controls.Add(this.checkBox1);
			this.tpBehaviour.Location = new System.Drawing.Point(4, 22);
			this.tpBehaviour.Name = "tpBehaviour";
			this.tpBehaviour.Padding = new System.Windows.Forms.Padding(3);
			this.tpBehaviour.Size = new System.Drawing.Size(293, 250);
			this.tpBehaviour.TabIndex = 0;
			this.tpBehaviour.Text = "Behaviour";
			this.tpBehaviour.UseVisualStyleBackColor = true;
			// 
			// cbShowConfirmationsForClearingEventLogs
			// 
			this.cbShowConfirmationsForClearingEventLogs.AutoSize = true;
			this.cbShowConfirmationsForClearingEventLogs.Checked = global::EventMonitor.Properties.Settings.Default.ShowConfirmationsForCleaningEventLogs;
			this.cbShowConfirmationsForClearingEventLogs.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbShowConfirmationsForClearingEventLogs.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EventMonitor.Properties.Settings.Default, "ShowConfirmationsForCleaningEventLogs", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cbShowConfirmationsForClearingEventLogs.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cbShowConfirmationsForClearingEventLogs.Location = new System.Drawing.Point(8, 6);
			this.cbShowConfirmationsForClearingEventLogs.Name = "cbShowConfirmationsForClearingEventLogs";
			this.cbShowConfirmationsForClearingEventLogs.Size = new System.Drawing.Size(232, 17);
			this.cbShowConfirmationsForClearingEventLogs.TabIndex = 2;
			this.cbShowConfirmationsForClearingEventLogs.Text = "Show Confirmations For Clearing EventLogs";
			this.cbShowConfirmationsForClearingEventLogs.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = global::EventMonitor.Properties.Settings.Default.SafeGuardDeletionOfStandardLogs;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EventMonitor.Properties.Settings.Default, "SafeGuardDeletionOfStandardLogs", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.checkBox1.Location = new System.Drawing.Point(8, 29);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(232, 17);
			this.checkBox1.TabIndex = 3;
			this.checkBox1.Text = "Safeguard Deletion of Standard Event Logs";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.groupBox1);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.cbEventDistinction);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(293, 250);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "User Interface";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnFailureAudit);
			this.groupBox1.Controls.Add(this.btnChooseSucces);
			this.groupBox1.Controls.Add(this.btnChooseError);
			this.groupBox1.Controls.Add(this.btnChooseWarning);
			this.groupBox1.Controls.Add(this.btnChooseInformation);
			this.groupBox1.Controls.Add(this.panelColorFailureAudit);
			this.groupBox1.Controls.Add(this.panelColorSuccessAudit);
			this.groupBox1.Controls.Add(this.panelColorError);
			this.groupBox1.Controls.Add(this.panelColorWarning);
			this.groupBox1.Controls.Add(this.panelColourInformation);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(10, 38);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(273, 168);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Row Colors";
			// 
			// btnFailureAudit
			// 
			this.btnFailureAudit.Image = global::EventMonitor.Properties.Resources.color_wheel;
			this.btnFailureAudit.Location = new System.Drawing.Point(234, 125);
			this.btnFailureAudit.Name = "btnFailureAudit";
			this.btnFailureAudit.Size = new System.Drawing.Size(30, 26);
			this.btnFailureAudit.TabIndex = 14;
			this.btnFailureAudit.UseVisualStyleBackColor = true;
			this.btnFailureAudit.Click += new System.EventHandler(this.btnFailureAudit_Click);
			// 
			// btnChooseSucces
			// 
			this.btnChooseSucces.Image = global::EventMonitor.Properties.Resources.color_wheel;
			this.btnChooseSucces.Location = new System.Drawing.Point(234, 97);
			this.btnChooseSucces.Name = "btnChooseSucces";
			this.btnChooseSucces.Size = new System.Drawing.Size(30, 26);
			this.btnChooseSucces.TabIndex = 13;
			this.btnChooseSucces.UseVisualStyleBackColor = true;
			this.btnChooseSucces.Click += new System.EventHandler(this.btnChooseSucces_Click);
			// 
			// btnChooseError
			// 
			this.btnChooseError.Image = global::EventMonitor.Properties.Resources.color_wheel;
			this.btnChooseError.Location = new System.Drawing.Point(234, 69);
			this.btnChooseError.Name = "btnChooseError";
			this.btnChooseError.Size = new System.Drawing.Size(30, 26);
			this.btnChooseError.TabIndex = 12;
			this.btnChooseError.UseVisualStyleBackColor = true;
			this.btnChooseError.Click += new System.EventHandler(this.btnChooseError_Click);
			// 
			// btnChooseWarning
			// 
			this.btnChooseWarning.Image = global::EventMonitor.Properties.Resources.color_wheel;
			this.btnChooseWarning.Location = new System.Drawing.Point(234, 41);
			this.btnChooseWarning.Name = "btnChooseWarning";
			this.btnChooseWarning.Size = new System.Drawing.Size(30, 26);
			this.btnChooseWarning.TabIndex = 11;
			this.btnChooseWarning.UseVisualStyleBackColor = true;
			this.btnChooseWarning.Click += new System.EventHandler(this.btnChooseWarning_Click);
			// 
			// btnChooseInformation
			// 
			this.btnChooseInformation.Image = global::EventMonitor.Properties.Resources.color_wheel;
			this.btnChooseInformation.Location = new System.Drawing.Point(234, 13);
			this.btnChooseInformation.Name = "btnChooseInformation";
			this.btnChooseInformation.Size = new System.Drawing.Size(30, 26);
			this.btnChooseInformation.TabIndex = 10;
			this.btnChooseInformation.UseVisualStyleBackColor = true;
			this.btnChooseInformation.Click += new System.EventHandler(this.btnChooseInformation_Click);
			// 
			// panelColorFailureAudit
			// 
			this.panelColorFailureAudit.BackColor = global::EventMonitor.Properties.Settings.Default.ColorFailureAudit;
			this.panelColorFailureAudit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelColorFailureAudit.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorFailureAudit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.panelColorFailureAudit.Location = new System.Drawing.Point(93, 127);
			this.panelColorFailureAudit.Name = "panelColorFailureAudit";
			this.panelColorFailureAudit.Size = new System.Drawing.Size(135, 22);
			this.panelColorFailureAudit.TabIndex = 9;
			// 
			// panelColorSuccessAudit
			// 
			this.panelColorSuccessAudit.BackColor = global::EventMonitor.Properties.Settings.Default.ColorSuccesfulAudit;
			this.panelColorSuccessAudit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelColorSuccessAudit.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorSuccesfulAudit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.panelColorSuccessAudit.Location = new System.Drawing.Point(93, 99);
			this.panelColorSuccessAudit.Name = "panelColorSuccessAudit";
			this.panelColorSuccessAudit.Size = new System.Drawing.Size(135, 22);
			this.panelColorSuccessAudit.TabIndex = 8;
			// 
			// panelColorError
			// 
			this.panelColorError.BackColor = global::EventMonitor.Properties.Settings.Default.ColorError;
			this.panelColorError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelColorError.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorError", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.panelColorError.Location = new System.Drawing.Point(93, 71);
			this.panelColorError.Name = "panelColorError";
			this.panelColorError.Size = new System.Drawing.Size(135, 22);
			this.panelColorError.TabIndex = 7;
			// 
			// panelColorWarning
			// 
			this.panelColorWarning.BackColor = global::EventMonitor.Properties.Settings.Default.ColorWarning;
			this.panelColorWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelColorWarning.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorWarning", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.panelColorWarning.Location = new System.Drawing.Point(93, 43);
			this.panelColorWarning.Name = "panelColorWarning";
			this.panelColorWarning.Size = new System.Drawing.Size(135, 22);
			this.panelColorWarning.TabIndex = 6;
			// 
			// panelColourInformation
			// 
			this.panelColourInformation.BackColor = global::EventMonitor.Properties.Settings.Default.ColorInformation;
			this.panelColourInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelColourInformation.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::EventMonitor.Properties.Settings.Default, "ColorInformation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.panelColourInformation.Location = new System.Drawing.Point(93, 15);
			this.panelColourInformation.Name = "panelColourInformation";
			this.panelColourInformation.Size = new System.Drawing.Size(135, 22);
			this.panelColourInformation.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(7, 132);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(71, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Failure Audit :";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(7, 104);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(81, 13);
			this.label5.TabIndex = 3;
			this.label5.Text = "Success Audit :";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 76);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(35, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Error :";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Warning :";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(7, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Information :";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(119, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Event Distinction Style :";
			// 
			// cbEventDistinction
			// 
			this.cbEventDistinction.FormattingEnabled = true;
			this.cbEventDistinction.Location = new System.Drawing.Point(138, 9);
			this.cbEventDistinction.Name = "cbEventDistinction";
			this.cbEventDistinction.Size = new System.Drawing.Size(145, 21);
			this.cbEventDistinction.TabIndex = 0;
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(301, 305);
			this.Controls.Add(this.tpUI);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Options";
			this.ShowInTaskbar = false;
			this.Text = "Options";
			this.Load += new System.EventHandler(this.Options_Load);
			this.panel1.ResumeLayout(false);
			this.tpUI.ResumeLayout(false);
			this.tpBehaviour.ResumeLayout(false);
			this.tpBehaviour.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tpUI;
		private System.Windows.Forms.TabPage tpBehaviour;
		private System.Windows.Forms.CheckBox cbShowConfirmationsForClearingEventLogs;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbEventDistinction;
		private System.Windows.Forms.GroupBox groupBox1;
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
		private System.Windows.Forms.Button btnChooseInformation;
		private System.Windows.Forms.Button btnFailureAudit;
		private System.Windows.Forms.Button btnChooseSucces;
		private System.Windows.Forms.Button btnChooseError;
		private System.Windows.Forms.Button btnChooseWarning;
	}
}