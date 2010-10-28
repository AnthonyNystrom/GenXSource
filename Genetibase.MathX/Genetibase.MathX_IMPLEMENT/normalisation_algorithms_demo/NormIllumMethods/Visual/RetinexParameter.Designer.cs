namespace NormIllumMethods.Visual
{
    partial class RetinexParameter
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
            this.labSigma = new System.Windows.Forms.Label();
            this.labWidth = new System.Windows.Forms.Label();
            this.labSize = new System.Windows.Forms.Label();
            this.txtSigma = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.listBoxSigma = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.listBoxWidth = new System.Windows.Forms.ListBox();
            this.combBoxSize = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labSigma
            // 
            this.labSigma.AutoSize = true;
            this.labSigma.Location = new System.Drawing.Point(14, 29);
            this.labSigma.Name = "labSigma";
            this.labSigma.Size = new System.Drawing.Size(36, 13);
            this.labSigma.TabIndex = 0;
            this.labSigma.Text = "Sigma";
            // 
            // labWidth
            // 
            this.labWidth.AutoSize = true;
            this.labWidth.Location = new System.Drawing.Point(14, 66);
            this.labWidth.Name = "labWidth";
            this.labWidth.Size = new System.Drawing.Size(41, 13);
            this.labWidth.TabIndex = 1;
            this.labWidth.Text = "Weight";
            // 
            // labSize
            // 
            this.labSize.AutoSize = true;
            this.labSize.Location = new System.Drawing.Point(18, 98);
            this.labSize.Name = "labSize";
            this.labSize.Size = new System.Drawing.Size(27, 13);
            this.labSize.TabIndex = 2;
            this.labSize.Text = "Size";
            // 
            // txtSigma
            // 
            this.txtSigma.Location = new System.Drawing.Point(53, 26);
            this.txtSigma.Name = "txtSigma";
            this.txtSigma.Size = new System.Drawing.Size(38, 20);
            this.txtSigma.TabIndex = 3;
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(53, 63);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(38, 20);
            this.txtWidth.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(103, 33);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(27, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "-->";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(103, 58);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(27, 23);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Text = "<--";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // listBoxSigma
            // 
            this.listBoxSigma.FormattingEnabled = true;
            this.listBoxSigma.Location = new System.Drawing.Point(136, 29);
            this.listBoxSigma.Name = "listBoxSigma";
            this.listBoxSigma.Size = new System.Drawing.Size(39, 82);
            this.listBoxSigma.TabIndex = 8;
            this.listBoxSigma.SelectedValueChanged += new System.EventHandler(this.listBoxSigma_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Sigma";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Width";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(68, 137);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // listBoxWidth
            // 
            this.listBoxWidth.FormattingEnabled = true;
            this.listBoxWidth.Location = new System.Drawing.Point(174, 29);
            this.listBoxWidth.Name = "listBoxWidth";
            this.listBoxWidth.Size = new System.Drawing.Size(39, 82);
            this.listBoxWidth.TabIndex = 12;
            this.listBoxWidth.SelectedValueChanged += new System.EventHandler(this.listBoxWidth_SelectedValueChanged);
            // 
            // combBoxSize
            // 
            this.combBoxSize.FormattingEnabled = true;
            this.combBoxSize.Items.AddRange(new object[] {
            "3",
            "5",
            "7",
            "9",
            "11",
            "13",
            "15",
            "17",
            "19",
            "21"});
            this.combBoxSize.Location = new System.Drawing.Point(53, 95);
            this.combBoxSize.Name = "combBoxSize";
            this.combBoxSize.Size = new System.Drawing.Size(38, 21);
            this.combBoxSize.TabIndex = 13;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(130, 137);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // RetinexParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 172);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.combBoxSize);
            this.Controls.Add(this.listBoxWidth);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxSigma);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.txtSigma);
            this.Controls.Add(this.labSize);
            this.Controls.Add(this.labWidth);
            this.Controls.Add(this.labSigma);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RetinexParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parameter";
            this.Load += new System.EventHandler(this.RetinexParameter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labSigma;
        private System.Windows.Forms.Label labWidth;
        private System.Windows.Forms.Label labSize;
        private System.Windows.Forms.TextBox txtSigma;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ListBox listBoxSigma;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListBox listBoxWidth;
        private System.Windows.Forms.ComboBox combBoxSize;
        private System.Windows.Forms.Button btnCancel;
    }
}