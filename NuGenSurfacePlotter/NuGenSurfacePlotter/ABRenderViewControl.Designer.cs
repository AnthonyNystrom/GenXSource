namespace NuGenCRBase.Managed.MDX1.Direct3D
{
    partial class ABRenderViewControl
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
            this.SuspendLayout();
            // 
            // ABRenderViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ABRenderViewControl";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ABRenderViewControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ABRenderViewControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ABRenderViewControl_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
