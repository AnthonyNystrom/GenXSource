namespace Genetibase.NuGenDEMVis.UI
{
    partial class NewVisDlg
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
            if (reader != null)
                reader.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.dataSourceControl1 = new Genetibase.NuGenDEMVis.UI.DataSourceControl();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.dataProfileControl1 = new Genetibase.NuGenDEMVis.UI.DataProfileControl();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.dataSourceControl1);
            this.uiGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(567, 121);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "Data Source(s)";
            // 
            // dataSourceControl1
            // 
            this.dataSourceControl1.DataSource = null;
            this.dataSourceControl1.DataSourceGroups = null;
            this.dataSourceControl1.Location = new System.Drawing.Point(6, 19);
            this.dataSourceControl1.Name = "dataSourceControl1";
            this.dataSourceControl1.Size = new System.Drawing.Size(555, 96);
            this.dataSourceControl1.TabIndex = 0;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.dataProfileControl1);
            this.uiGroupBox2.Location = new System.Drawing.Point(12, 139);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(567, 210);
            this.uiGroupBox2.TabIndex = 1;
            this.uiGroupBox2.Text = "Data Profile";
            // 
            // dataProfileControl1
            // 
            this.dataProfileControl1.Location = new System.Drawing.Point(11, 19);
            this.dataProfileControl1.Name = "dataProfileControl1";
            this.dataProfileControl1.Profiles = null;
            this.dataProfileControl1.Size = new System.Drawing.Size(550, 185);
            this.dataProfileControl1.TabIndex = 0;
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.uiButton1.Location = new System.Drawing.Point(504, 368);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(75, 23);
            this.uiButton1.TabIndex = 2;
            this.uiButton1.Text = "Create";
            this.uiButton1.Click += new System.EventHandler(this.uiClose_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uiButton2.Location = new System.Drawing.Point(423, 368);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(75, 23);
            this.uiButton2.TabIndex = 3;
            this.uiButton2.Text = "Cancel";
            this.uiButton2.Click += new System.EventHandler(this.uiClose_Click);
            // 
            // NewVisDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(591, 403);
            this.Controls.Add(this.uiButton2);
            this.Controls.Add(this.uiButton1);
            this.Controls.Add(this.uiGroupBox2);
            this.Controls.Add(this.uiGroupBox1);
            this.Name = "NewVisDlg";
            this.Text = "New Visualization";
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private DataSourceControl dataSourceControl1;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private DataProfileControl dataProfileControl1;
    }
}