namespace Next2Friends.CrossPoster.Client.Controls
{
    partial class PostMetadataPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._subjectStaticLabel = new System.Windows.Forms.Label();
            this._senderStaticLabel = new System.Windows.Forms.Label();
            this._dateStaticLabel = new System.Windows.Forms.Label();
            this._subjectLabel = new System.Windows.Forms.Label();
            this._senderLabel = new System.Windows.Forms.Label();
            this._dateLabel = new System.Windows.Forms.Label();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this._subjectStaticLabel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._senderStaticLabel, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._dateStaticLabel, 0, 2);
            this._tableLayoutPanel.Controls.Add(this._subjectLabel, 1, 0);
            this._tableLayoutPanel.Controls.Add(this._senderLabel, 1, 1);
            this._tableLayoutPanel.Controls.Add(this._dateLabel, 1, 2);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 3;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this._tableLayoutPanel.Size = new System.Drawing.Size(500, 52);
            this._tableLayoutPanel.TabIndex = 0;
            // 
            // _subjectStaticLabel
            // 
            this._subjectStaticLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._subjectStaticLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._subjectStaticLabel.Location = new System.Drawing.Point(3, 0);
            this._subjectStaticLabel.Name = "_subjectStaticLabel";
            this._subjectStaticLabel.Size = new System.Drawing.Size(74, 18);
            this._subjectStaticLabel.TabIndex = 0;
            this._subjectStaticLabel.Text = "Subject:";
            this._subjectStaticLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _senderStaticLabel
            // 
            this._senderStaticLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._senderStaticLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._senderStaticLabel.Location = new System.Drawing.Point(3, 18);
            this._senderStaticLabel.Name = "_senderStaticLabel";
            this._senderStaticLabel.Size = new System.Drawing.Size(74, 18);
            this._senderStaticLabel.TabIndex = 1;
            this._senderStaticLabel.Text = "Sender:";
            this._senderStaticLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _dateStaticLabel
            // 
            this._dateStaticLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dateStaticLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._dateStaticLabel.Location = new System.Drawing.Point(3, 36);
            this._dateStaticLabel.Name = "_dateStaticLabel";
            this._dateStaticLabel.Size = new System.Drawing.Size(74, 18);
            this._dateStaticLabel.TabIndex = 2;
            this._dateStaticLabel.Text = "Date:";
            this._dateStaticLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _subjectLabel
            // 
            this._subjectLabel.AutoEllipsis = true;
            this._subjectLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._subjectLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._subjectLabel.Location = new System.Drawing.Point(83, 0);
            this._subjectLabel.Name = "_subjectLabel";
            this._subjectLabel.Size = new System.Drawing.Size(414, 18);
            this._subjectLabel.TabIndex = 3;
            // 
            // _senderLabel
            // 
            this._senderLabel.AutoEllipsis = true;
            this._senderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._senderLabel.Location = new System.Drawing.Point(83, 18);
            this._senderLabel.Name = "_senderLabel";
            this._senderLabel.Size = new System.Drawing.Size(414, 18);
            this._senderLabel.TabIndex = 4;
            // 
            // _dateLabel
            // 
            this._dateLabel.AutoEllipsis = true;
            this._dateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dateLabel.Location = new System.Drawing.Point(83, 36);
            this._dateLabel.Name = "_dateLabel";
            this._dateLabel.Size = new System.Drawing.Size(414, 18);
            this._dateLabel.TabIndex = 5;
            // 
            // PostMetadataPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "PostMetadataPanel";
            this.Size = new System.Drawing.Size(500, 52);
            this._tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label _subjectStaticLabel;
        private System.Windows.Forms.Label _senderStaticLabel;
        private System.Windows.Forms.Label _dateStaticLabel;
        private System.Windows.Forms.Label _subjectLabel;
        private System.Windows.Forms.Label _senderLabel;
        private System.Windows.Forms.Label _dateLabel;
    }
}
