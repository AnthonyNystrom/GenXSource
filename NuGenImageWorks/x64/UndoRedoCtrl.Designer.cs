namespace Genetibase.UI.NuGenImageWorks
{
    partial class UndoRedoCtrl
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
            this.ribbonGroup25 = new Genetibase.UI.NuGenImageWorks.RibbonGroup();
            this.ribbonButton30 = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonButton31 = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonGroup25.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonGroup25
            // 
            this.ribbonGroup25.Controls.Add(this.ribbonButton30);
            this.ribbonGroup25.Controls.Add(this.ribbonButton31);
            this.ribbonGroup25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ribbonGroup25.Location = new System.Drawing.Point(0, 0);
            this.ribbonGroup25.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonGroup25.Name = "ribbonGroup25";
            this.ribbonGroup25.Size = new System.Drawing.Size(112, 82);
            this.ribbonGroup25.TabIndex = 15;
            this.ribbonGroup25.TabStop = false;
            // 
            // ribbonButton30
            // 
            this.ribbonButton30.Image = global::Genetibase.UI.NuGenImageWorks.Properties.Resources.redo;
            this.ribbonButton30.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton30.IsFlat = true;
            this.ribbonButton30.IsPressed = false;
            this.ribbonButton30.Location = new System.Drawing.Point(9, 35);
            this.ribbonButton30.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton30.Name = "ribbonButton30";
            this.ribbonButton30.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton30.Size = new System.Drawing.Size(97, 24);
            this.ribbonButton30.TabIndex = 4;
            this.ribbonButton30.Text = "Redo";
            this.ribbonButton30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton30.Click += new System.EventHandler(this.ribbonButton30_Click);
            // 
            // ribbonButton31
            // 
            this.ribbonButton31.Image = global::Genetibase.UI.NuGenImageWorks.Properties.Resources.undo;
            this.ribbonButton31.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton31.IsFlat = true;
            this.ribbonButton31.IsPressed = false;
            this.ribbonButton31.Location = new System.Drawing.Point(9, 9);
            this.ribbonButton31.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton31.Name = "ribbonButton31";
            this.ribbonButton31.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton31.Size = new System.Drawing.Size(97, 24);
            this.ribbonButton31.TabIndex = 3;
            this.ribbonButton31.Text = "Undo";
            this.ribbonButton31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton31.Click += new System.EventHandler(this.ribbonButton31_Click);
            // 
            // UndoRedoCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ribbonGroup25);
            this.Name = "UndoRedoCtrl";
            this.Size = new System.Drawing.Size(112, 82);
            this.ribbonGroup25.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RibbonGroup ribbonGroup25;
        private RibbonButton ribbonButton30;
        private RibbonButton ribbonButton31;
    }
}
