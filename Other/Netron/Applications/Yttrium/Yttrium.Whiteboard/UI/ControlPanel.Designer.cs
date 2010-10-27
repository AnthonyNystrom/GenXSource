namespace Yttrium.Whiteboard
{
    partial class ControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.btnNewSignal = new System.Windows.Forms.Button();
            this.btnNewBus = new System.Windows.Forms.Button();
            this.btnNewPort = new System.Windows.Forms.Button();
            this.btnBuildSample = new System.Windows.Forms.Button();
            this.btnEvaluate = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.entitySelector = new System.Windows.Forms.Panel();
            this.cmbEntities = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.udBuses = new System.Windows.Forms.NumericUpDown();
            this.udInputs = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.entitySelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udBuses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udInputs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNewSignal
            // 
            this.btnNewSignal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNewSignal.Location = new System.Drawing.Point(3, 3);
            this.btnNewSignal.Name = "btnNewSignal";
            this.btnNewSignal.Size = new System.Drawing.Size(75, 23);
            this.btnNewSignal.TabIndex = 14;
            this.btnNewSignal.Text = "New Signal";
            this.btnNewSignal.UseVisualStyleBackColor = true;
            this.btnNewSignal.Click += new System.EventHandler(this.btnNewSignal_Click);
            // 
            // btnNewBus
            // 
            this.btnNewBus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNewBus.Location = new System.Drawing.Point(84, 3);
            this.btnNewBus.Name = "btnNewBus";
            this.btnNewBus.Size = new System.Drawing.Size(75, 23);
            this.btnNewBus.TabIndex = 15;
            this.btnNewBus.Text = "New Bus";
            this.btnNewBus.UseVisualStyleBackColor = true;
            this.btnNewBus.Click += new System.EventHandler(this.btnNewBus_Click);
            // 
            // btnNewPort
            // 
            this.btnNewPort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNewPort.Location = new System.Drawing.Point(165, 3);
            this.btnNewPort.Name = "btnNewPort";
            this.btnNewPort.Size = new System.Drawing.Size(75, 23);
            this.btnNewPort.TabIndex = 16;
            this.btnNewPort.Text = "New Port";
            this.btnNewPort.UseVisualStyleBackColor = true;
            this.btnNewPort.Click += new System.EventHandler(this.btnNewPort_Click);
            // 
            // btnBuildSample
            // 
            this.btnBuildSample.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBuildSample.Location = new System.Drawing.Point(371, 3);
            this.btnBuildSample.Name = "btnBuildSample";
            this.btnBuildSample.Size = new System.Drawing.Size(156, 23);
            this.btnBuildSample.TabIndex = 13;
            this.btnBuildSample.Text = "Build Sample System";
            this.btnBuildSample.UseVisualStyleBackColor = true;
            this.btnBuildSample.Click += new System.EventHandler(this.btnBuildSample_Click);
            // 
            // btnEvaluate
            // 
            this.btnEvaluate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEvaluate.Location = new System.Drawing.Point(246, 3);
            this.btnEvaluate.Name = "btnEvaluate";
            this.btnEvaluate.Size = new System.Drawing.Size(119, 23);
            this.btnEvaluate.TabIndex = 17;
            this.btnEvaluate.Text = "Evaluate System";
            this.btnEvaluate.UseVisualStyleBackColor = true;
            this.btnEvaluate.Click += new System.EventHandler(this.btnEvaluate_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.btnNewSignal);
            this.flowLayoutPanel1.Controls.Add(this.btnNewBus);
            this.flowLayoutPanel1.Controls.Add(this.btnNewPort);
            this.flowLayoutPanel1.Controls.Add(this.btnEvaluate);
            this.flowLayoutPanel1.Controls.Add(this.btnBuildSample);
            this.flowLayoutPanel1.Controls.Add(this.entitySelector);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 13);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(649, 363);
            this.flowLayoutPanel1.TabIndex = 18;
            // 
            // entitySelector
            // 
            this.entitySelector.Controls.Add(this.cmbEntities);
            this.entitySelector.Controls.Add(this.btnCancel);
            this.entitySelector.Controls.Add(this.btnOk);
            this.entitySelector.Controls.Add(this.udBuses);
            this.entitySelector.Controls.Add(this.udInputs);
            this.entitySelector.Controls.Add(this.label2);
            this.entitySelector.Controls.Add(this.label1);
            this.entitySelector.Location = new System.Drawing.Point(3, 32);
            this.entitySelector.Name = "entitySelector";
            this.entitySelector.Size = new System.Drawing.Size(524, 51);
            this.entitySelector.TabIndex = 28;
            this.entitySelector.Visible = false;
            // 
            // cmbEntities
            // 
            this.cmbEntities.FormattingEnabled = true;
            this.cmbEntities.Location = new System.Drawing.Point(3, 14);
            this.cmbEntities.Name = "cmbEntities";
            this.cmbEntities.Size = new System.Drawing.Size(121, 21);
            this.cmbEntities.TabIndex = 21;
            this.cmbEntities.SelectedIndexChanged += new System.EventHandler(this.cmbEntities_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(346, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(52, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(304, 14);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(36, 23);
            this.btnOk.TabIndex = 26;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // udBuses
            // 
            this.udBuses.Enabled = false;
            this.udBuses.Location = new System.Drawing.Point(262, 14);
            this.udBuses.Name = "udBuses";
            this.udBuses.Size = new System.Drawing.Size(36, 20);
            this.udBuses.TabIndex = 23;
            // 
            // udInputs
            // 
            this.udInputs.Enabled = false;
            this.udInputs.Location = new System.Drawing.Point(175, 14);
            this.udInputs.Name = "udInputs";
            this.udInputs.Size = new System.Drawing.Size(36, 20);
            this.udInputs.TabIndex = 22;
            this.udInputs.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Buses:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(130, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Inputs:";
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 388);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(200, 200);
            this.Name = "ControlPanel";
            this.ShowHint = Netron.Neon.WinFormsUI.DockState.DockBottom;
            this.TabText = "Yttrium Panel";
            this.Text = "Yttrium control panel";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.entitySelector.ResumeLayout(false);
            this.entitySelector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udBuses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udInputs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNewSignal;
        private System.Windows.Forms.Button btnNewBus;
        private System.Windows.Forms.Button btnNewPort;
        private System.Windows.Forms.Button btnBuildSample;
        private System.Windows.Forms.Button btnEvaluate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel entitySelector;
        private System.Windows.Forms.ComboBox cmbEntities;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.NumericUpDown udBuses;
        private System.Windows.Forms.NumericUpDown udInputs;
        private System.Windows.Forms.Label label1;

    }
}