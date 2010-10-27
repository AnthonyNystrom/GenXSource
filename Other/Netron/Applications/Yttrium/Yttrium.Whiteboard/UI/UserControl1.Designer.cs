namespace Yttrium.Whiteboard.UI
{
    partial class UserControl1
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.udBuses = new System.Windows.Forms.NumericUpDown();
            this.udInputs = new System.Windows.Forms.NumericUpDown();
            this.cmbEntities = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.udBuses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udInputs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(349, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(52, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(307, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(36, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Buses:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(132, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Inputs:";
            // 
            // udBuses
            // 
            this.udBuses.Enabled = false;
            this.udBuses.Location = new System.Drawing.Point(264, 3);
            this.udBuses.Name = "udBuses";
            this.udBuses.Size = new System.Drawing.Size(36, 20);
            this.udBuses.TabIndex = 9;
            // 
            // udInputs
            // 
            this.udInputs.Enabled = false;
            this.udInputs.Location = new System.Drawing.Point(177, 3);
            this.udInputs.Name = "udInputs";
            this.udInputs.Size = new System.Drawing.Size(36, 20);
            this.udInputs.TabIndex = 8;
            this.udInputs.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // cmbEntities
            // 
            this.cmbEntities.FormattingEnabled = true;
            this.cmbEntities.Location = new System.Drawing.Point(5, 3);
            this.cmbEntities.Name = "cmbEntities";
            this.cmbEntities.Size = new System.Drawing.Size(121, 21);
            this.cmbEntities.TabIndex = 7;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.udBuses);
            this.Controls.Add(this.udInputs);
            this.Controls.Add(this.cmbEntities);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(447, 33);
            ((System.ComponentModel.ISupportInitialize)(this.udBuses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udInputs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown udBuses;
        private System.Windows.Forms.NumericUpDown udInputs;
        private System.Windows.Forms.ComboBox cmbEntities;

    }
}
