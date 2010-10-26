namespace Genetibase.UI.NuGenImageWorks
{
    partial class frmFR
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSelected = new System.Windows.Forms.Label();
            this.numbericUpDownAlphaEnd = new System.Windows.Forms.NumericUpDown();
            this.numbericUpDownAlphaStart = new System.Windows.Forms.NumericUpDown();
            this.numbericUpDownOffset = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ribbonButtonOp5Cancel = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonButtonOp5OK = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.bottom = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.right = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.left = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.top = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbericUpDownAlphaEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numbericUpDownAlphaStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numbericUpDownOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.ribbonButtonOp5Cancel);
            this.panel1.Controls.Add(this.ribbonButtonOp5OK);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numbericUpDownOffset);
            this.panel1.Controls.Add(this.numbericUpDownAlphaStart);
            this.panel1.Controls.Add(this.numbericUpDownAlphaEnd);
            this.panel1.Controls.Add(this.lblSelected);
            this.panel1.Controls.Add(this.bottom);
            this.panel1.Controls.Add(this.right);
            this.panel1.Controls.Add(this.left);
            this.panel1.Controls.Add(this.top);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 114);
            this.panel1.TabIndex = 0;
            // 
            // lblSelected
            // 
            this.lblSelected.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblSelected.ForeColor = System.Drawing.Color.SlateGray;
            this.lblSelected.Location = new System.Drawing.Point(26, 84);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(105, 20);
            this.lblSelected.TabIndex = 25;
            this.lblSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numbericUpDownAlphaEnd
            // 
            this.numbericUpDownAlphaEnd.Location = new System.Drawing.Point(252, 31);
            this.numbericUpDownAlphaEnd.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numbericUpDownAlphaEnd.Name = "numbericUpDownAlphaEnd";
            this.numbericUpDownAlphaEnd.Size = new System.Drawing.Size(43, 20);
            this.numbericUpDownAlphaEnd.TabIndex = 26;
            this.numbericUpDownAlphaEnd.ValueChanged += new System.EventHandler(this.numbericUpDownAlphaEnd_ValueChanged);
            // 
            // numbericUpDownAlphaStart
            // 
            this.numbericUpDownAlphaStart.Location = new System.Drawing.Point(252, 5);
            this.numbericUpDownAlphaStart.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numbericUpDownAlphaStart.Name = "numbericUpDownAlphaStart";
            this.numbericUpDownAlphaStart.Size = new System.Drawing.Size(43, 20);
            this.numbericUpDownAlphaStart.TabIndex = 27;
            this.numbericUpDownAlphaStart.ValueChanged += new System.EventHandler(this.numbericUpDownAlphaStart_ValueChanged);
            // 
            // numbericUpDownOffset
            // 
            this.numbericUpDownOffset.Location = new System.Drawing.Point(252, 57);
            this.numbericUpDownOffset.Name = "numbericUpDownOffset";
            this.numbericUpDownOffset.Size = new System.Drawing.Size(43, 20);
            this.numbericUpDownOffset.TabIndex = 28;
            this.numbericUpDownOffset.ValueChanged += new System.EventHandler(this.numbericUpDownOffset_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(177, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Start Alpha";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Ending Alpha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(177, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Offset";
            // 
            // ribbonButtonOp5Cancel
            // 
            this.ribbonButtonOp5Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ribbonButtonOp5Cancel.Image = global::Genetibase.UI.NuGenImageWorks.Properties.Resources.Cancel;
            this.ribbonButtonOp5Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButtonOp5Cancel.IsFlat = true;
            this.ribbonButtonOp5Cancel.IsPressed = false;
            this.ribbonButtonOp5Cancel.Location = new System.Drawing.Point(232, 87);
            this.ribbonButtonOp5Cancel.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButtonOp5Cancel.Name = "ribbonButtonOp5Cancel";
            this.ribbonButtonOp5Cancel.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButtonOp5Cancel.Size = new System.Drawing.Size(68, 24);
            this.ribbonButtonOp5Cancel.TabIndex = 33;
            this.ribbonButtonOp5Cancel.Text = "Cancel";
            this.ribbonButtonOp5Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButtonOp5Cancel.Click += new System.EventHandler(this.ribbonButtonOp5Cancel_Click);
            // 
            // ribbonButtonOp5OK
            // 
            this.ribbonButtonOp5OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ribbonButtonOp5OK.Image = global::Genetibase.UI.NuGenImageWorks.Properties.Resources.OK;
            this.ribbonButtonOp5OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButtonOp5OK.IsFlat = true;
            this.ribbonButtonOp5OK.IsPressed = false;
            this.ribbonButtonOp5OK.Location = new System.Drawing.Point(177, 87);
            this.ribbonButtonOp5OK.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButtonOp5OK.Name = "ribbonButtonOp5OK";
            this.ribbonButtonOp5OK.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButtonOp5OK.Size = new System.Drawing.Size(49, 24);
            this.ribbonButtonOp5OK.TabIndex = 32;
            this.ribbonButtonOp5OK.Text = "OK";
            this.ribbonButtonOp5OK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButtonOp5OK.Click += new System.EventHandler(this.ribbonButtonOp5OK_Click);
            // 
            // bottom
            // 
            this.bottom.IsFlat = false;
            this.bottom.IsPressed = false;
            this.bottom.Location = new System.Drawing.Point(54, 59);
            this.bottom.Margin = new System.Windows.Forms.Padding(1);
            this.bottom.Name = "bottom";
            this.bottom.Padding = new System.Windows.Forms.Padding(2);
            this.bottom.Size = new System.Drawing.Size(46, 24);
            this.bottom.TabIndex = 22;
            this.bottom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bottom.Click += new System.EventHandler(this.bottom_Click);
            // 
            // right
            // 
            this.right.IsFlat = false;
            this.right.IsPressed = false;
            this.right.Location = new System.Drawing.Point(102, 33);
            this.right.Margin = new System.Windows.Forms.Padding(1);
            this.right.Name = "right";
            this.right.Padding = new System.Windows.Forms.Padding(2);
            this.right.Size = new System.Drawing.Size(46, 24);
            this.right.TabIndex = 21;
            this.right.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.right.Click += new System.EventHandler(this.right_Click);
            // 
            // left
            // 
            this.left.IsFlat = false;
            this.left.IsPressed = false;
            this.left.Location = new System.Drawing.Point(7, 33);
            this.left.Margin = new System.Windows.Forms.Padding(1);
            this.left.Name = "left";
            this.left.Padding = new System.Windows.Forms.Padding(2);
            this.left.Size = new System.Drawing.Size(46, 24);
            this.left.TabIndex = 19;
            this.left.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.left.Click += new System.EventHandler(this.left_Click);
            // 
            // top
            // 
            this.top.IsFlat = false;
            this.top.IsPressed = false;
            this.top.Location = new System.Drawing.Point(54, 7);
            this.top.Margin = new System.Windows.Forms.Padding(1);
            this.top.Name = "top";
            this.top.Padding = new System.Windows.Forms.Padding(2);
            this.top.Size = new System.Drawing.Size(46, 24);
            this.top.TabIndex = 16;
            this.top.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.top.Click += new System.EventHandler(this.top_Click);
            // 
            // frmFR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(316, 124);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFR";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmFR";
            this.Load += new System.EventHandler(this.frmFR_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numbericUpDownAlphaEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numbericUpDownAlphaStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numbericUpDownOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private RibbonButton bottom;
        private RibbonButton right;
        private RibbonButton left;
        private RibbonButton top;
        private System.Windows.Forms.Label lblSelected;
        private System.Windows.Forms.NumericUpDown numbericUpDownAlphaStart;
        private System.Windows.Forms.NumericUpDown numbericUpDownAlphaEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numbericUpDownOffset;
        private RibbonButton ribbonButtonOp5Cancel;
        private RibbonButton ribbonButtonOp5OK;
    }
}