namespace Netron.Cobalt
{
    partial class ShellForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShellForm));
            this.Shell = new Netron.Cobalt.ShellControl();
            this.SuspendLayout();
            // 
            // Shell
            // 
            this.Shell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Shell.Location = new System.Drawing.Point(0, 0);
            this.Shell.Name = "Shell";
            this.Shell.Prompt = ">>>";
            this.Shell.ShellTextBackColor = System.Drawing.Color.Black;
            this.Shell.ShellTextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Shell.ShellTextForeColor = System.Drawing.Color.LawnGreen;
            this.Shell.Size = new System.Drawing.Size(292, 273);
            this.Shell.TabIndex = 0;
            this.Shell.CommandEntered += new Netron.Cobalt.EventCommandEntered(this.shellControl1_CommandEntered);
            // 
            // ShellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.Shell);
            this.DockableAreas = ((Netron.Neon.WinFormsUI.DockAreas)((Netron.Neon.WinFormsUI.DockAreas.Float | Netron.Neon.WinFormsUI.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShellForm";
            this.TabText = "Cobalt Shell";
            this.Text = "ShellForm";
            this.ResumeLayout(false);

        }

        #endregion

        public ShellControl Shell;

    }
}