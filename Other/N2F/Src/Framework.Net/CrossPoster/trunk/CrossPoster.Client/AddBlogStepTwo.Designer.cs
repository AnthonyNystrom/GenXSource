namespace Next2Friends.CrossPoster.Client
{
    partial class AddBlogStepTwo
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
            this._backButton = new System.Windows.Forms.Button();
            this._finishButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._addressLabel = new System.Windows.Forms.Label();
            this._addressTextBox = new System.Windows.Forms.TextBox();
            this._usernameLabel = new System.Windows.Forms.Label();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._usernameTextBox = new System.Windows.Forms.TextBox();
            this._passwordTextBox = new System.Windows.Forms.TextBox();
            this._blogNameLabel = new System.Windows.Forms.Label();
            this._blogNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _backButton
            // 
            this._backButton.Location = new System.Drawing.Point(76, 111);
            this._backButton.Name = "_backButton";
            this._backButton.Size = new System.Drawing.Size(75, 23);
            this._backButton.TabIndex = 10;
            this._backButton.Text = "< &Back";
            this._backButton.UseVisualStyleBackColor = true;
            this._backButton.Click += new System.EventHandler(this._backButton_Click);
            // 
            // _finishButton
            // 
            this._finishButton.Location = new System.Drawing.Point(157, 111);
            this._finishButton.Name = "_finishButton";
            this._finishButton.Size = new System.Drawing.Size(75, 23);
            this._finishButton.TabIndex = 0;
            this._finishButton.Text = "&Finish";
            this._finishButton.UseVisualStyleBackColor = true;
            this._finishButton.Click += new System.EventHandler(this._finishButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(238, 111);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 5;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _addressLabel
            // 
            this._addressLabel.AutoSize = true;
            this._addressLabel.Location = new System.Drawing.Point(12, 9);
            this._addressLabel.Name = "_addressLabel";
            this._addressLabel.Size = new System.Drawing.Size(50, 13);
            this._addressLabel.TabIndex = 3;
            this._addressLabel.Text = "Address:";
            // 
            // _addressTextBox
            // 
            this._addressTextBox.Location = new System.Drawing.Point(76, 6);
            this._addressTextBox.Name = "_addressTextBox";
            this._addressTextBox.Size = new System.Drawing.Size(237, 21);
            this._addressTextBox.TabIndex = 6;
            // 
            // _usernameLabel
            // 
            this._usernameLabel.AutoSize = true;
            this._usernameLabel.Location = new System.Drawing.Point(12, 35);
            this._usernameLabel.Name = "_usernameLabel";
            this._usernameLabel.Size = new System.Drawing.Size(59, 13);
            this._usernameLabel.TabIndex = 5;
            this._usernameLabel.Text = "Username:";
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Location = new System.Drawing.Point(12, 61);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(57, 13);
            this._passwordLabel.TabIndex = 6;
            this._passwordLabel.Text = "Password:";
            // 
            // _usernameTextBox
            // 
            this._usernameTextBox.Location = new System.Drawing.Point(76, 32);
            this._usernameTextBox.Name = "_usernameTextBox";
            this._usernameTextBox.Size = new System.Drawing.Size(154, 21);
            this._usernameTextBox.TabIndex = 7;
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Location = new System.Drawing.Point(76, 58);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.PasswordChar = '*';
            this._passwordTextBox.Size = new System.Drawing.Size(154, 21);
            this._passwordTextBox.TabIndex = 8;
            // 
            // _blogNameLabel
            // 
            this._blogNameLabel.AutoSize = true;
            this._blogNameLabel.Location = new System.Drawing.Point(12, 87);
            this._blogNameLabel.Name = "_blogNameLabel";
            this._blogNameLabel.Size = new System.Drawing.Size(61, 13);
            this._blogNameLabel.TabIndex = 9;
            this._blogNameLabel.Text = "Blog Name:";
            // 
            // _blogNameTextBox
            // 
            this._blogNameTextBox.Location = new System.Drawing.Point(76, 84);
            this._blogNameTextBox.Name = "_blogNameTextBox";
            this._blogNameTextBox.Size = new System.Drawing.Size(154, 21);
            this._blogNameTextBox.TabIndex = 9;
            // 
            // AddBlogStepTwo
            // 
            this.AcceptButton = this._finishButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(323, 143);
            this.Controls.Add(this._blogNameTextBox);
            this.Controls.Add(this._blogNameLabel);
            this.Controls.Add(this._passwordTextBox);
            this.Controls.Add(this._usernameTextBox);
            this.Controls.Add(this._passwordLabel);
            this.Controls.Add(this._usernameLabel);
            this.Controls.Add(this._addressTextBox);
            this.Controls.Add(this._addressLabel);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._finishButton);
            this.Controls.Add(this._backButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddBlogStepTwo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blog Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _backButton;
        private System.Windows.Forms.Button _finishButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Label _addressLabel;
        private System.Windows.Forms.TextBox _addressTextBox;
        private System.Windows.Forms.Label _usernameLabel;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.TextBox _usernameTextBox;
        private System.Windows.Forms.TextBox _passwordTextBox;
        private System.Windows.Forms.Label _blogNameLabel;
        private System.Windows.Forms.TextBox _blogNameTextBox;
    }
}