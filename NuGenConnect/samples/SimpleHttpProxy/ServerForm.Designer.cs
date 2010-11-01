namespace SimpleHttpProxy
{
    partial class ServerForm
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
            this.btStart = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.httpServer = new Genetibase.Network.Web.HttpServerComponent(this.components);
            this.btHelp = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbBannedSites = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btStart
            // 
            this.btStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btStart.Location = new System.Drawing.Point(380, 41);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(206, 23);
            this.btStart.TabIndex = 3;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(380, 70);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.SelectedObject = this.httpServer;
            this.propertyGrid.Size = new System.Drawing.Size(206, 288);
            this.propertyGrid.TabIndex = 2;
            // 
            // httpServer
            // 
            this.httpServer.Active = false;
            this.httpServer.AutoStartSession = false;
            this.httpServer.DefaultPort = 3128;
            this.httpServer.KeepAlive = false;
            this.httpServer.ServerSoftware = "Genetibase.Network.NET/1.0";
            this.httpServer.SessionTimeOut = 0;
            this.httpServer.UseTLS = Genetibase.Network.Sockets.UseTLSEnum.NoTLSSupport;
            // 
            // btHelp
            // 
            this.btHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btHelp.Location = new System.Drawing.Point(380, 12);
            this.btHelp.Name = "btHelp";
            this.btHelp.Size = new System.Drawing.Size(206, 23);
            this.btHelp.TabIndex = 4;
            this.btHelp.Text = "Howto setup proxy in IE";
            this.btHelp.UseVisualStyleBackColor = true;
            this.btHelp.Click += new System.EventHandler(this.btHelp_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLog.Location = new System.Drawing.Point(12, 170);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(362, 188);
            this.rtbLog.TabIndex = 5;
            this.rtbLog.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Proxy log";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Sites to ban.";
            // 
            // rtbBannedSites
            // 
            this.rtbBannedSites.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbBannedSites.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbBannedSites.Location = new System.Drawing.Point(12, 25);
            this.rtbBannedSites.Name = "rtbBannedSites";
            this.rtbBannedSites.Size = new System.Drawing.Size(362, 126);
            this.rtbBannedSites.TabIndex = 7;
            this.rtbBannedSites.Text = "*google.com; *sex.com;\n*cnn.com; *microsoft.com; *amazon.com; *msn.com;";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 370);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rtbBannedSites);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btHelp);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.propertyGrid);
            this.Name = "ServerForm";
            this.Text = "Simple Http Proxy";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private Genetibase.Network.Web.HttpServerComponent httpServer;
        private System.Windows.Forms.Button btHelp;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbBannedSites;
    }
}

