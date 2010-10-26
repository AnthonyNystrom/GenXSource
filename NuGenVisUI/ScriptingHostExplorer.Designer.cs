namespace NuGenVisUI
{
    partial class ScriptingHostExplorer
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
            this.scriptEditorControl1 = new Genetibase.VisUI.Controls.ScriptEditorControl();
            this.SuspendLayout();
            // 
            // scriptEditorControl1
            // 
            this.scriptEditorControl1.Location = new System.Drawing.Point(38, 100);
            this.scriptEditorControl1.Name = "scriptEditorControl1";
            this.scriptEditorControl1.Size = new System.Drawing.Size(533, 259);
            this.scriptEditorControl1.TabIndex = 0;
            // 
            // ScriptingHostExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 394);
            this.Controls.Add(this.scriptEditorControl1);
            this.Name = "ScriptingHostExplorer";
            this.Text = "ScriptingHostExplorer";
            this.ResumeLayout(false);

        }

        #endregion

        private Genetibase.VisUI.Controls.ScriptEditorControl scriptEditorControl1;
    }
}