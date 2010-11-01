namespace Genetibase.Debug
{
    partial class frmLogin
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
            this.grpBoxLogin = new System.Windows.Forms.GroupBox();
            this.lblDomain = new System.Windows.Forms.Label();
            this.tbDomain = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.lblAppName = new System.Windows.Forms.Label();
            this.tbApplicationName = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.cbUserProfile = new Janus.Windows.EditControls.UICheckBox();
            this.grpBoxLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxLogin
            // 
            this.grpBoxLogin.Controls.Add(this.cbUserProfile);
            this.grpBoxLogin.Controls.Add(this.uiButton2);
            this.grpBoxLogin.Controls.Add(this.uiButton1);
            this.grpBoxLogin.Controls.Add(this.lblDomain);
            this.grpBoxLogin.Controls.Add(this.tbDomain);
            this.grpBoxLogin.Controls.Add(this.lblPassword);
            this.grpBoxLogin.Controls.Add(this.tbPassword);
            this.grpBoxLogin.Controls.Add(this.lblUserName);
            this.grpBoxLogin.Controls.Add(this.tbUserName);
            this.grpBoxLogin.Controls.Add(this.lblAppName);
            this.grpBoxLogin.Controls.Add(this.tbApplicationName);
            this.grpBoxLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grpBoxLogin.Location = new System.Drawing.Point(13, 13);
            this.grpBoxLogin.Name = "grpBoxLogin";
            this.grpBoxLogin.Size = new System.Drawing.Size(315, 178);
            this.grpBoxLogin.TabIndex = 0;
            this.grpBoxLogin.TabStop = false;
            this.grpBoxLogin.Text = "Unter User Credentials";
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(6, 126);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(42, 13);
            this.lblDomain.TabIndex = 8;
            this.lblDomain.Text = "Domain";
            // 
            // tbDomain
            // 
            this.tbDomain.Location = new System.Drawing.Point(105, 123);
            this.tbDomain.Name = "tbDomain";
            this.tbDomain.Size = new System.Drawing.Size(127, 21);
            this.tbDomain.TabIndex = 7;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(6, 96);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(105, 93);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(127, 21);
            this.tbPassword.TabIndex = 5;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(6, 64);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(59, 13);
            this.lblUserName.TabIndex = 4;
            this.lblUserName.Text = "UserName ";
            // 
            // tbUserName
            // 
            this.tbUserName.AutoCompleteCustomSource.AddRange(new string[] {
            "administrator",
            "guest",
            "zippo",
            "aspnet"});
            this.tbUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbUserName.Location = new System.Drawing.Point(105, 61);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(127, 21);
            this.tbUserName.TabIndex = 3;
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Location = new System.Drawing.Point(6, 32);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(89, 13);
            this.lblAppName.TabIndex = 1;
            this.lblAppName.Text = "Application Name";
            // 
            // tbApplicationName
            // 
            this.tbApplicationName.Location = new System.Drawing.Point(105, 29);
            this.tbApplicationName.Name = "tbApplicationName";
            this.tbApplicationName.Size = new System.Drawing.Size(127, 21);
            this.tbApplicationName.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Title = "Browse File to Run";
            // 
            // uiButton1
            // 
            this.uiButton1.Location = new System.Drawing.Point(240, 27);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(61, 23);
            this.uiButton1.TabIndex = 11;
            this.uiButton1.Text = "Browse";
            this.uiButton1.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Location = new System.Drawing.Point(240, 59);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(61, 85);
            this.uiButton2.TabIndex = 12;
            this.uiButton2.Text = "RUN";
            this.uiButton2.Click += new System.EventHandler(this.btRun_Click);
            // 
            // cbUserProfile
            // 
            this.cbUserProfile.Location = new System.Drawing.Point(9, 149);
            this.cbUserProfile.Name = "cbUserProfile";
            this.cbUserProfile.Size = new System.Drawing.Size(104, 23);
            this.cbUserProfile.TabIndex = 13;
            this.cbUserProfile.Text = "Load User Profile";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 203);
            this.Controls.Add(this.grpBoxLogin);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLogin";
            this.Opacity = 0.9;
            this.Text = "User Credentials";
            this.grpBoxLogin.ResumeLayout(false);
            this.grpBoxLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxLogin;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.TextBox tbApplicationName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.TextBox tbDomain;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.EditControls.UICheckBox cbUserProfile;
    }
}