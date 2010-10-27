namespace Next2Friends.CrossPoster.Client
{
    partial class ComposeForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._titleLabel = new System.Windows.Forms.Label();
            this._messageLabel = new System.Windows.Forms.Label();
            this._titleTextBox = new System.Windows.Forms.TextBox();
            this._messageTextBox = new System.Windows.Forms.TextBox();
            this._publishButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._titleLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._messageLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._titleTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._messageTextBox, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(487, 298);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _titleLabel
            // 
            this._titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._titleLabel.Location = new System.Drawing.Point(3, 0);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(74, 24);
            this._titleLabel.TabIndex = 0;
            this._titleLabel.Text = "Title:";
            // 
            // _messageLabel
            // 
            this._messageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._messageLabel.Location = new System.Drawing.Point(3, 24);
            this._messageLabel.Name = "_messageLabel";
            this._messageLabel.Size = new System.Drawing.Size(74, 274);
            this._messageLabel.TabIndex = 1;
            this._messageLabel.Text = "Message:";
            // 
            // _titleTextBox
            // 
            this._titleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._titleTextBox.Location = new System.Drawing.Point(83, 3);
            this._titleTextBox.Name = "_titleTextBox";
            this._titleTextBox.Size = new System.Drawing.Size(401, 21);
            this._titleTextBox.TabIndex = 2;
            this._titleTextBox.TextChanged += new System.EventHandler(this._inputTextBox_TextChanged);
            // 
            // _messageTextBox
            // 
            this._messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._messageTextBox.Location = new System.Drawing.Point(83, 27);
            this._messageTextBox.Multiline = true;
            this._messageTextBox.Name = "_messageTextBox";
            this._messageTextBox.Size = new System.Drawing.Size(401, 268);
            this._messageTextBox.TabIndex = 3;
            this._messageTextBox.TextChanged += new System.EventHandler(this._inputTextBox_TextChanged);
            // 
            // _publishButton
            // 
            this._publishButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._publishButton.Enabled = false;
            this._publishButton.Location = new System.Drawing.Point(330, 307);
            this._publishButton.Name = "_publishButton";
            this._publishButton.Size = new System.Drawing.Size(75, 23);
            this._publishButton.TabIndex = 1;
            this._publishButton.Text = "&Publish";
            this._publishButton.UseVisualStyleBackColor = true;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(411, 307);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // ComposeForm
            // 
            this.AcceptButton = this._publishButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(493, 336);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._publishButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComposeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Compose";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Label _messageLabel;
        private System.Windows.Forms.TextBox _titleTextBox;
        private System.Windows.Forms.TextBox _messageTextBox;
        private System.Windows.Forms.Button _publishButton;
        private System.Windows.Forms.Button _cancelButton;
    }
}