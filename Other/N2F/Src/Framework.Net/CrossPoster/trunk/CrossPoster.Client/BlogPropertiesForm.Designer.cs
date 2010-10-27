namespace Next2Friends.CrossPoster.Client
{
    partial class BlogPropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlogPropertiesForm));
            this._blogListImages = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._blogList = new Next2Friends.CrossPoster.Client.Controls.ImageCombo();
            this._blogNameTextBox = new System.Windows.Forms.TextBox();
            this._blogAddressTextBox = new System.Windows.Forms.TextBox();
            this._usernameTextBox = new System.Windows.Forms.TextBox();
            this._passwordTextBox = new System.Windows.Forms.TextBox();
            this._blogNameLabel = new System.Windows.Forms.Label();
            this._blogEngineLabel = new System.Windows.Forms.Label();
            this._blogAddressLabel = new System.Windows.Forms.Label();
            this._usernameLabel = new System.Windows.Forms.Label();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _blogListImages
            // 
            this._blogListImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_blogListImages.ImageStream")));
            this._blogListImages.TransparentColor = System.Drawing.Color.Transparent;
            this._blogListImages.Images.SetKeyName(0, "Blogger.png");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._blogList, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._blogNameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._blogAddressTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._usernameTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._passwordTextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this._blogNameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._blogEngineLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._blogAddressLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._usernameLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._passwordLabel, 0, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(323, 123);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _blogList
            // 
            this._blogList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._blogList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._blogList.FormattingEnabled = true;
            this._blogList.ImageList = this._blogListImages;
            this._blogList.Location = new System.Drawing.Point(83, 27);
            this._blogList.Name = "_blogList";
            this._blogList.Size = new System.Drawing.Size(154, 22);
            this._blogList.TabIndex = 20;
            // 
            // _blogNameTextBox
            // 
            this._blogNameTextBox.Location = new System.Drawing.Point(83, 3);
            this._blogNameTextBox.Name = "_blogNameTextBox";
            this._blogNameTextBox.Size = new System.Drawing.Size(154, 21);
            this._blogNameTextBox.TabIndex = 10;
            // 
            // _blogAddressTextBox
            // 
            this._blogAddressTextBox.Location = new System.Drawing.Point(83, 51);
            this._blogAddressTextBox.Name = "_blogAddressTextBox";
            this._blogAddressTextBox.Size = new System.Drawing.Size(237, 21);
            this._blogAddressTextBox.TabIndex = 30;
            // 
            // _usernameTextBox
            // 
            this._usernameTextBox.Location = new System.Drawing.Point(83, 75);
            this._usernameTextBox.Name = "_usernameTextBox";
            this._usernameTextBox.Size = new System.Drawing.Size(154, 21);
            this._usernameTextBox.TabIndex = 40;
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Location = new System.Drawing.Point(83, 99);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.PasswordChar = '*';
            this._passwordTextBox.Size = new System.Drawing.Size(154, 21);
            this._passwordTextBox.TabIndex = 50;
            // 
            // _blogNameLabel
            // 
            this._blogNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._blogNameLabel.Location = new System.Drawing.Point(3, 0);
            this._blogNameLabel.Name = "_blogNameLabel";
            this._blogNameLabel.Size = new System.Drawing.Size(74, 24);
            this._blogNameLabel.TabIndex = 5;
            this._blogNameLabel.Text = "Blog Name:";
            this._blogNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _blogEngineLabel
            // 
            this._blogEngineLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._blogEngineLabel.Location = new System.Drawing.Point(3, 24);
            this._blogEngineLabel.Name = "_blogEngineLabel";
            this._blogEngineLabel.Size = new System.Drawing.Size(74, 24);
            this._blogEngineLabel.TabIndex = 6;
            this._blogEngineLabel.Text = "Blog Engine:";
            this._blogEngineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _blogAddressLabel
            // 
            this._blogAddressLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._blogAddressLabel.Location = new System.Drawing.Point(3, 48);
            this._blogAddressLabel.Name = "_blogAddressLabel";
            this._blogAddressLabel.Size = new System.Drawing.Size(74, 24);
            this._blogAddressLabel.TabIndex = 7;
            this._blogAddressLabel.Text = "Blog Address:";
            this._blogAddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _usernameLabel
            // 
            this._usernameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._usernameLabel.Location = new System.Drawing.Point(3, 72);
            this._usernameLabel.Name = "_usernameLabel";
            this._usernameLabel.Size = new System.Drawing.Size(74, 24);
            this._usernameLabel.TabIndex = 8;
            this._usernameLabel.Text = "Username:";
            this._usernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _passwordLabel
            // 
            this._passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._passwordLabel.Location = new System.Drawing.Point(3, 96);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(74, 27);
            this._passwordLabel.TabIndex = 9;
            this._passwordLabel.Text = "Password:";
            this._passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(167, 127);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 60;
            this._okButton.Text = "&Ok";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(249, 127);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 70;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // BlogPropertiesForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(333, 157);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlogPropertiesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Properties";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Next2Friends.CrossPoster.Client.Controls.ImageCombo _blogList;
        private System.Windows.Forms.ImageList _blogListImages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox _blogNameTextBox;
        private System.Windows.Forms.TextBox _blogAddressTextBox;
        private System.Windows.Forms.TextBox _usernameTextBox;
        private System.Windows.Forms.TextBox _passwordTextBox;
        private System.Windows.Forms.Label _blogNameLabel;
        private System.Windows.Forms.Label _blogEngineLabel;
        private System.Windows.Forms.Label _blogAddressLabel;
        private System.Windows.Forms.Label _usernameLabel;
        private System.Windows.Forms.Label _passwordLabel;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
    }
}