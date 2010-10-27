namespace Netron.Cobalt
{
    partial class DocumentProperties
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
            if(disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentProperties));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.TheCancelButton = new System.Windows.Forms.Button();
            this.TheOKButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDocumentInformation = new System.Windows.Forms.TabPage();
            this.documentInformation1 = new Netron.Diagramming.Win.DocumentInformation();
            this.tabLayers = new System.Windows.Forms.TabPage();
            this.tabPages = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabDocumentInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "EditData.ico");
            this.imageList1.Images.SetKeyName(1, "Layers.ico");
            this.imageList1.Images.SetKeyName(2, "Pages.ico");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TheCancelButton);
            this.panel1.Controls.Add(this.TheOKButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 254);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(561, 53);
            this.panel1.TabIndex = 1;
            // 
            // TheCancelButton
            // 
            this.TheCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.TheCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.TheCancelButton.Location = new System.Drawing.Point(386, 18);
            this.TheCancelButton.Name = "TheCancelButton";
            this.TheCancelButton.Size = new System.Drawing.Size(75, 23);
            this.TheCancelButton.TabIndex = 1;
            this.TheCancelButton.Text = "Cancel";
            this.TheCancelButton.UseVisualStyleBackColor = true;
            this.TheCancelButton.Click += new System.EventHandler(this.TheCancelButton_Click);
            // 
            // TheOKButton
            // 
            this.TheOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.TheOKButton.Location = new System.Drawing.Point(467, 18);
            this.TheOKButton.Name = "TheOKButton";
            this.TheOKButton.Size = new System.Drawing.Size(75, 23);
            this.TheOKButton.TabIndex = 0;
            this.TheOKButton.Text = "OK";
            this.TheOKButton.UseVisualStyleBackColor = true;
            this.TheOKButton.Click += new System.EventHandler(this.TheOKButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabDocumentInformation);
            this.tabControl1.Controls.Add(this.tabLayers);
            this.tabControl1.Controls.Add(this.tabPages);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(561, 254);
            this.tabControl1.TabIndex = 2;
            // 
            // tabDocumentInformation
            // 
            this.tabDocumentInformation.BackColor = System.Drawing.Color.White;
            this.tabDocumentInformation.Controls.Add(this.documentInformation1);
            this.tabDocumentInformation.ImageKey = "EditData.ico";
            this.tabDocumentInformation.Location = new System.Drawing.Point(4, 26);
            this.tabDocumentInformation.Name = "tabDocumentInformation";
            this.tabDocumentInformation.Padding = new System.Windows.Forms.Padding(3);
            this.tabDocumentInformation.Size = new System.Drawing.Size(553, 224);
            this.tabDocumentInformation.TabIndex = 0;
            this.tabDocumentInformation.Text = "Information";
            this.tabDocumentInformation.ToolTipText = "The document information";
            // 
            // documentInformation1
            // 
            this.documentInformation1.Author = "";
            this.documentInformation1.CreationDate = new System.DateTime(((long)(0)));
            this.documentInformation1.Description = "";
            this.documentInformation1.Location = new System.Drawing.Point(24, 15);
            this.documentInformation1.Name = "documentInformation1";
            this.documentInformation1.Size = new System.Drawing.Size(433, 189);
            this.documentInformation1.TabIndex = 0;
            this.documentInformation1.Title = "";
            // 
            // tabLayers
            // 
            this.tabLayers.ImageKey = "Layers.ico";
            this.tabLayers.Location = new System.Drawing.Point(4, 26);
            this.tabLayers.Name = "tabLayers";
            this.tabLayers.Padding = new System.Windows.Forms.Padding(3);
            this.tabLayers.Size = new System.Drawing.Size(546, 224);
            this.tabLayers.TabIndex = 1;
            this.tabLayers.Text = "Layers";
            this.tabLayers.UseVisualStyleBackColor = true;
            // 
            // tabPages
            // 
            this.tabPages.ImageKey = "Pages.ico";
            this.tabPages.Location = new System.Drawing.Point(4, 26);
            this.tabPages.Name = "tabPages";
            this.tabPages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPages.Size = new System.Drawing.Size(546, 224);
            this.tabPages.TabIndex = 2;
            this.tabPages.Text = "Pages";
            this.tabPages.UseVisualStyleBackColor = true;
            // 
            // DocumentProperties
            // 
            this.AcceptButton = this.TheOKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.TheCancelButton;
            this.ClientSize = new System.Drawing.Size(561, 307);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DocumentProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Document Properties";
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabDocumentInformation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabDocumentInformation;
        private System.Windows.Forms.TabPage tabLayers;
        private System.Windows.Forms.TabPage tabPages;
        private System.Windows.Forms.Button TheCancelButton;
        private System.Windows.Forms.Button TheOKButton;
        private Netron.Diagramming.Win.DocumentInformation documentInformation1;
    }
}