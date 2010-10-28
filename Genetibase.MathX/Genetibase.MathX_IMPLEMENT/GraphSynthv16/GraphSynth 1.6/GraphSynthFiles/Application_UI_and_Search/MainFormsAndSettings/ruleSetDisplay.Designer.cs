namespace GraphSynth.Forms
{
    partial class ruleSetDisplay
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.moveCheckedUp = new System.Windows.Forms.Button();
            this.moveCheckedDown = new System.Windows.Forms.Button();
            this.saveAndCloseButton = new System.Windows.Forms.Button();
            this.addRuleButton = new System.Windows.Forms.Button();
            this.deleteCheckedButton = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.clearAllButton = new System.Windows.Forms.Button();
            this.checkAllButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(161, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rules Loaded into Rule Set";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.deleteCheckedButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.moveCheckedUp);
            this.panel1.Controls.Add(this.moveCheckedDown);
            this.panel1.Controls.Add(this.saveAndCloseButton);
            this.panel1.Controls.Add(this.addRuleButton);
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 196);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 273);
            this.panel1.TabIndex = 1;
            // 
            // moveCheckedUp
            // 
            this.moveCheckedUp.Location = new System.Drawing.Point(155, 248);
            this.moveCheckedUp.Name = "moveCheckedUp";
            this.moveCheckedUp.Size = new System.Drawing.Size(27, 22);
            this.moveCheckedUp.TabIndex = 3;
            this.moveCheckedUp.Text = "up";
            this.moveCheckedUp.UseVisualStyleBackColor = true;
            this.moveCheckedUp.Click += new System.EventHandler(this.moveCheckedUp_Click);
            // 
            // moveCheckedDown
            // 
            this.moveCheckedDown.Location = new System.Drawing.Point(182, 248);
            this.moveCheckedDown.Name = "moveCheckedDown";
            this.moveCheckedDown.Size = new System.Drawing.Size(27, 22);
            this.moveCheckedDown.TabIndex = 3;
            this.moveCheckedDown.Text = "dn";
            this.moveCheckedDown.UseVisualStyleBackColor = true;
            this.moveCheckedDown.Click += new System.EventHandler(this.moveCheckedDown_Click);
            // 
            // saveAndCloseButton
            // 
            this.saveAndCloseButton.BackColor = System.Drawing.Color.Transparent;
            this.saveAndCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveAndCloseButton.ForeColor = System.Drawing.Color.Black;
            this.saveAndCloseButton.Location = new System.Drawing.Point(214, 248);
            this.saveAndCloseButton.Name = "saveAndCloseButton";
            this.saveAndCloseButton.Size = new System.Drawing.Size(56, 22);
            this.saveAndCloseButton.TabIndex = 2;
            this.saveAndCloseButton.Text = "save";
            this.saveAndCloseButton.UseVisualStyleBackColor = false;
            this.saveAndCloseButton.Click += new System.EventHandler(this.saveAndCloseButton_Click);
            // 
            // addRuleButton
            // 
            this.addRuleButton.ForeColor = System.Drawing.Color.Black;
            this.addRuleButton.Location = new System.Drawing.Point(3, 248);
            this.addRuleButton.Name = "addRuleButton";
            this.addRuleButton.Size = new System.Drawing.Size(66, 22);
            this.addRuleButton.TabIndex = 1;
            this.addRuleButton.Text = "Add Rule";
            this.addRuleButton.Click += new System.EventHandler(this.addRuleButton_Click);
            // 
            // deleteCheckedButton
            // 
            this.deleteCheckedButton.ForeColor = System.Drawing.Color.Black;
            this.deleteCheckedButton.Location = new System.Drawing.Point(122, 248);
            this.deleteCheckedButton.Name = "deleteCheckedButton";
            this.deleteCheckedButton.Size = new System.Drawing.Size(33, 22);
            this.deleteCheckedButton.TabIndex = 0;
            this.deleteCheckedButton.Text = "del";
            this.deleteCheckedButton.Click += new System.EventHandler(this.removeCheckedButton_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGrid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.MintCream;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(273, 245);
            this.propertyGrid1.TabIndex = 2;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 24);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(273, 169);
            this.checkedListBox1.TabIndex = 3;
            this.checkedListBox1.DoubleClick += new System.EventHandler(this.showRule_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.clearAllButton);
            this.panel2.Controls.Add(this.checkAllButton);
            this.panel2.Controls.Add(this.checkedListBox1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(273, 193);
            this.panel2.TabIndex = 2;
            // 
            // clearAllButton
            // 
            this.clearAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearAllButton.ForeColor = System.Drawing.Color.Black;
            this.clearAllButton.Location = new System.Drawing.Point(219, 1);
            this.clearAllButton.Name = "clearAllButton";
            this.clearAllButton.Size = new System.Drawing.Size(51, 22);
            this.clearAllButton.TabIndex = 5;
            this.clearAllButton.Text = "clear all";
            this.clearAllButton.UseVisualStyleBackColor = true;
            this.clearAllButton.Click += new System.EventHandler(this.clearAllButton_Click);
            // 
            // checkAllButton
            // 
            this.checkAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkAllButton.ForeColor = System.Drawing.Color.Black;
            this.checkAllButton.Location = new System.Drawing.Point(163, 1);
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.Size = new System.Drawing.Size(51, 22);
            this.checkAllButton.TabIndex = 4;
            this.checkAllButton.Text = "check all";
            this.checkAllButton.UseVisualStyleBackColor = true;
            this.checkAllButton.Click += new System.EventHandler(this.checkAllButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "checked:";
            // 
            // ruleSetDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(273, 469);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "ruleSetDisplay";
            this.Text = "ruleSetDisplay";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveAndCloseButton;
        private System.Windows.Forms.Button addRuleButton;
        private System.Windows.Forms.Button deleteCheckedButton;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button checkAllButton;
        private System.Windows.Forms.Button clearAllButton;
        private System.Windows.Forms.Button moveCheckedUp;
        private System.Windows.Forms.Button moveCheckedDown;
        private System.Windows.Forms.Label label2;
    }
}