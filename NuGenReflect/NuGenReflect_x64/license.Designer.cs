namespace Genetibase.Debug
{
    partial class license
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
            this.uIPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uIButton1 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uIPanelManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // uIPanelManager1
            // 
            this.uIPanelManager1.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.uIPanelManager1.ContainerControl = this;
            // 
            // uIButton1
            // 
            this.uIButton1.Location = new System.Drawing.Point(221, 422);
            this.uIButton1.Name = "uIButton1";
            this.uIButton1.Size = new System.Drawing.Size(75, 23);
            this.uIButton1.TabIndex = 4;
            this.uIButton1.Text = "uiButton1";
            // 
            // license
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 709);
            this.Controls.Add(this.uIButton1);
            this.Name = "license";
            this.Text = "license";
            ((System.ComponentModel.ISupportInitialize)(this.uIPanelManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uIPanelManager1;
        private Janus.Windows.EditControls.UIButton uIButton1;
    }
}