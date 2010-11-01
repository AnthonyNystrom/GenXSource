namespace WorkSample
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
            this.httpServer = new Genetibase.Network.Web.HttpServerComponent(this.components);
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.btStart = new System.Windows.Forms.Button();
            this.rtbConnections = new System.Windows.Forms.RichTextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // httpServer
            // 
            this.httpServer.Active = false;
            this.httpServer.DefaultPort = 80;
            this.httpServer.KeepAlive = false;
            this.httpServer.ServerSoftware = "Genetibase.Network.NET/1.0";
            this.httpServer.SessionTimeOut = 0;
            this.httpServer.UseTLS = Genetibase.Network.Sockets.UseTLSEnum.NoTLSSupport;
            this.httpServer.OnSessionEnd += new Genetibase.Network.Web.SessionEventHandler(this.httpServer_OnSessionEnd);
            this.httpServer.OnSessionStart += new Genetibase.Network.Web.SessionEventHandler(this.httpServer_OnSessionStart);
            this.httpServer.OnCommandGet += new Genetibase.Network.Web.HttpCommandEventHandler(this.httpServer_OnCommandGet);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(412, 41);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.SelectedObject = this.httpServer;
            this.propertyGrid.Size = new System.Drawing.Size(206, 219);
            this.propertyGrid.TabIndex = 0;
            // 
            // btStart
            // 
            this.btStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btStart.Location = new System.Drawing.Point(412, 12);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(206, 23);
            this.btStart.TabIndex = 1;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // rtbConnections
            // 
            this.rtbConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbConnections.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConnections.Location = new System.Drawing.Point(12, 266);
            this.rtbConnections.Name = "rtbConnections";
            this.rtbConnections.Size = new System.Drawing.Size(606, 112);
            this.rtbConnections.TabIndex = 2;
            this.rtbConnections.Text = "";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 390);
            this.Controls.Add(this.rtbConnections);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.propertyGrid);
            this.Name = "ServerForm";
            this.Text = "Server";
            this.ResumeLayout(false);

        }

        #endregion

        private Genetibase.Network.Web.HttpServerComponent httpServer;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.RichTextBox rtbConnections;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}