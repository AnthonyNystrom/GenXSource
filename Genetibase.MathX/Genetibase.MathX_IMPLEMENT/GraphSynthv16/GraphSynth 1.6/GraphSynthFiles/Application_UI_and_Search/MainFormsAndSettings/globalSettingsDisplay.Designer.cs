namespace GraphSynth.Forms
{
    partial class globalSettingsDisplay
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.browseWorkingDirButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.dirTab = new System.Windows.Forms.TabPage();
            this.browseHelpURLButton = new System.Windows.Forms.Button();
            this.browseHelpDirButton = new System.Windows.Forms.Button();
            this.browseRulseDirButton = new System.Windows.Forms.Button();
            this.browseOutputDirButton = new System.Windows.Forms.Button();
            this.browseInputDirButton = new System.Windows.Forms.Button();
            this.helpURLText = new System.Windows.Forms.TextBox();
            this.helpDirText = new System.Windows.Forms.TextBox();
            this.rDirText = new System.Windows.Forms.TextBox();
            this.oDirText = new System.Windows.Forms.TextBox();
            this.iDirText = new System.Windows.Forms.TextBox();
            this.wDirText = new System.Windows.Forms.TextBox();
            this.limitsBoolsTab = new System.Windows.Forms.TabPage();
            this.defSeedRulesTab = new System.Windows.Forms.TabPage();
            this.browseCompiledDLLButton = new System.Windows.Forms.Button();
            this.browseDefSeedButton = new System.Windows.Forms.Button();
            for (int i = 0; i != 10; i++)
            {
                this.RSText[i] = new System.Windows.Forms.Label();
                this.RSbutton[i] = new System.Windows.Forms.Button();
            }
            this.compiledRulesText = new System.Windows.Forms.TextBox();
            this.seedText = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.dirTab.SuspendLayout();
            this.limitsBoolsTab.SuspendLayout();
            this.defSeedRulesTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.Cornsilk;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(333, 350);
            this.propertyGrid1.TabIndex = 2;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.cancelButton);
            this.panel1.Controls.Add(this.applyButton);
            this.panel1.Controls.Add(this.saveButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 353);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 29);
            this.panel1.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(269, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(120, 3);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(143, 23);
            this.applyButton.TabIndex = 1;
            this.applyButton.Text = "Apply in this process.";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(3, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(111, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save to file.";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // browseWorkingDirButton
            // 
            this.browseWorkingDirButton.BackColor = System.Drawing.Color.Transparent;
            this.browseWorkingDirButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseWorkingDirButton.Location = new System.Drawing.Point(9, 7);
            this.browseWorkingDirButton.Name = "browseWorkingDirButton";
            this.browseWorkingDirButton.Size = new System.Drawing.Size(127, 22);
            this.browseWorkingDirButton.TabIndex = 3;
            this.browseWorkingDirButton.Text = "working directory:";
            this.browseWorkingDirButton.UseVisualStyleBackColor = false;
            this.browseWorkingDirButton.Click += new System.EventHandler(this.browseWorkingDirButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.limitsBoolsTab);
            this.tabControl1.Controls.Add(this.dirTab);
            this.tabControl1.Controls.Add(this.defSeedRulesTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(347, 382);
            this.tabControl1.TabIndex = 4;
            // 
            // dirTab
            // 
            this.dirTab.Controls.Add(this.browseHelpURLButton);
            this.dirTab.Controls.Add(this.browseHelpDirButton);
            this.dirTab.Controls.Add(this.browseRulseDirButton);
            this.dirTab.Controls.Add(this.browseOutputDirButton);
            this.dirTab.Controls.Add(this.browseInputDirButton);
            this.dirTab.Controls.Add(this.helpURLText);
            this.dirTab.Controls.Add(this.helpDirText);
            this.dirTab.Controls.Add(this.rDirText);
            this.dirTab.Controls.Add(this.oDirText);
            this.dirTab.Controls.Add(this.browseWorkingDirButton);
            this.dirTab.Controls.Add(this.iDirText);
            this.dirTab.Controls.Add(this.wDirText);
            this.dirTab.Location = new System.Drawing.Point(4, 22);
            this.dirTab.Name = "dirTab";
            this.dirTab.Padding = new System.Windows.Forms.Padding(3);
            this.dirTab.Size = new System.Drawing.Size(339, 356);
            this.dirTab.TabIndex = 0;
            this.dirTab.Text = "Directories";
            this.dirTab.UseVisualStyleBackColor = true;
            // 
            // browseHelpURLButton
            // 
            this.browseHelpURLButton.BackColor = System.Drawing.Color.Transparent;
            this.browseHelpURLButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseHelpURLButton.Location = new System.Drawing.Point(9, 282);
            this.browseHelpURLButton.Name = "browseHelpURLButton";
            this.browseHelpURLButton.Size = new System.Drawing.Size(110, 22);
            this.browseHelpURLButton.TabIndex = 3;
            this.browseHelpURLButton.Text = "paste help URL:";
            this.browseHelpURLButton.UseVisualStyleBackColor = false;
            this.browseHelpURLButton.Click += new System.EventHandler(this.browseHelpURLButton_Click);
            // 
            // browseHelpDirButton
            // 
            this.browseHelpDirButton.BackColor = System.Drawing.Color.Transparent;
            this.browseHelpDirButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseHelpDirButton.Location = new System.Drawing.Point(9, 227);
            this.browseHelpDirButton.Name = "browseHelpDirButton";
            this.browseHelpDirButton.Size = new System.Drawing.Size(110, 22);
            this.browseHelpDirButton.TabIndex = 3;
            this.browseHelpDirButton.Text = "help directory:";
            this.browseHelpDirButton.UseVisualStyleBackColor = false;
            this.browseHelpDirButton.Click += new System.EventHandler(this.browseHelpDirButton_Click);
            // 
            // browseRulseDirButton
            // 
            this.browseRulseDirButton.BackColor = System.Drawing.Color.Transparent;
            this.browseRulseDirButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseRulseDirButton.Location = new System.Drawing.Point(9, 172);
            this.browseRulseDirButton.Name = "browseRulseDirButton";
            this.browseRulseDirButton.Size = new System.Drawing.Size(110, 22);
            this.browseRulseDirButton.TabIndex = 3;
            this.browseRulseDirButton.Text = "rules directory:";
            this.browseRulseDirButton.UseVisualStyleBackColor = false;
            this.browseRulseDirButton.Click += new System.EventHandler(this.browseRulesDirButton_Click);
            // 
            // browseOutputDirButton
            // 
            this.browseOutputDirButton.BackColor = System.Drawing.Color.Transparent;
            this.browseOutputDirButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseOutputDirButton.Location = new System.Drawing.Point(9, 117);
            this.browseOutputDirButton.Name = "browseOutputDirButton";
            this.browseOutputDirButton.Size = new System.Drawing.Size(110, 22);
            this.browseOutputDirButton.TabIndex = 3;
            this.browseOutputDirButton.Text = "output directory:";
            this.browseOutputDirButton.UseVisualStyleBackColor = false;
            this.browseOutputDirButton.Click += new System.EventHandler(this.browseOutputDirButton_Click);
            // 
            // browseInputDirButton
            // 
            this.browseInputDirButton.BackColor = System.Drawing.Color.Transparent;
            this.browseInputDirButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseInputDirButton.Location = new System.Drawing.Point(9, 62);
            this.browseInputDirButton.Name = "browseInputDirButton";
            this.browseInputDirButton.Size = new System.Drawing.Size(110, 22);
            this.browseInputDirButton.TabIndex = 3;
            this.browseInputDirButton.Text = "input directory:";
            this.browseInputDirButton.UseVisualStyleBackColor = false;
            this.browseInputDirButton.Click += new System.EventHandler(this.browseInputDirButton_Click);
            // 
            // helpURLText
            // 
            this.helpURLText.Multiline = true;
            this.helpURLText.BackColor = System.Drawing.Color.LightGray;
            this.helpURLText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.helpURLText.ForeColor = System.Drawing.Color.DimGray;
            this.helpURLText.Location = new System.Drawing.Point(9, 282);
            this.helpURLText.Name = "helpURLText";
            this.helpURLText.Size = new System.Drawing.Size(324, 43);
            this.helpURLText.TabIndex = 5;
            this.helpURLText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.helpURLText.WordWrap = true;
            this.helpURLText.Leave += new System.EventHandler(helpURLText_Leave);
            // 
            // helpDirText
            // 
            this.helpDirText.Multiline = true;
            this.helpDirText.BackColor = System.Drawing.Color.LightGray;
            this.helpDirText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.helpDirText.ForeColor = System.Drawing.Color.DimGray;
            this.helpDirText.Location = new System.Drawing.Point(9, 227);
            this.helpDirText.Name = "helpDirText";
            this.helpDirText.Size = new System.Drawing.Size(324, 43);
            this.helpDirText.TabIndex = 5;
            this.helpDirText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.helpDirText.WordWrap = true;
            this.helpDirText.Leave += new System.EventHandler(helpDirText_Leave);
            // 
            // rDirText
            // 
            this.rDirText.Multiline = true;
            this.rDirText.BackColor = System.Drawing.Color.LightGray;
            this.rDirText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rDirText.ForeColor = System.Drawing.Color.DimGray;
            this.rDirText.Location = new System.Drawing.Point(9, 172);
            this.rDirText.Name = "rDirText";
            this.rDirText.Size = new System.Drawing.Size(324, 43);
            this.rDirText.TabIndex = 5;
            this.rDirText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.rDirText.WordWrap = true;
            this.rDirText.Leave += new System.EventHandler(rDirText_Leave);
            // 
            // oDirText
            // 
            this.oDirText.Multiline = true;
            this.oDirText.BackColor = System.Drawing.Color.LightGray;
            this.oDirText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.oDirText.ForeColor = System.Drawing.Color.DimGray;
            this.oDirText.Location = new System.Drawing.Point(9, 117);
            this.oDirText.Name = "oDirText";
            this.oDirText.Size = new System.Drawing.Size(324, 43);
            this.oDirText.TabIndex = 5;
            this.oDirText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.oDirText.WordWrap = true;
            this.oDirText.Leave += new System.EventHandler(oDirText_Leave);
            // 
            // iDirText
            // 
            this.iDirText.Multiline = true;
            this.iDirText.BackColor = System.Drawing.Color.LightGray;
            this.iDirText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iDirText.ForeColor = System.Drawing.Color.DimGray;
            this.iDirText.Location = new System.Drawing.Point(9, 62);
            this.iDirText.Name = "iDirText";
            this.iDirText.Size = new System.Drawing.Size(324, 43);
            this.iDirText.TabIndex = 5;
            this.iDirText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.iDirText.WordWrap = true;
            this.iDirText.Leave += new System.EventHandler(iDirText_Leave);
            // 
            // wDirText
            // 
            this.wDirText.Multiline = true;
            this.wDirText.BackColor = System.Drawing.Color.LightGray;
            this.wDirText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wDirText.ForeColor = System.Drawing.Color.DimGray;
            this.wDirText.Location = new System.Drawing.Point(9, 7);
            this.wDirText.Name = "wDirText";
            this.wDirText.Size = new System.Drawing.Size(324, 43);
            this.wDirText.TabIndex = 5;
            this.wDirText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.wDirText.WordWrap = true;
            this.wDirText.Leave += new System.EventHandler(wDirText_Leave);
            // 
            // limitsBoolsTab
            // 
            this.limitsBoolsTab.Controls.Add(this.propertyGrid1);
            this.limitsBoolsTab.Location = new System.Drawing.Point(4, 22);
            this.limitsBoolsTab.Name = "limitsBoolsTab";
            this.limitsBoolsTab.Padding = new System.Windows.Forms.Padding(3);
            this.limitsBoolsTab.Size = new System.Drawing.Size(339, 356);
            this.limitsBoolsTab.TabIndex = 1;
            this.limitsBoolsTab.Text = "Limits & Booleans";
            this.limitsBoolsTab.UseVisualStyleBackColor = true;
            // 
            // defSeedRulesTab
            // 
            this.defSeedRulesTab.Controls.Add(this.browseCompiledDLLButton);
            this.defSeedRulesTab.Controls.Add(this.browseDefSeedButton);
            for (int i = 0; i != 10; i++)
            {
                this.defSeedRulesTab.Controls.Add(this.RSText[i]);
                this.defSeedRulesTab.Controls.Add(this.RSbutton[i]);
            }
            this.defSeedRulesTab.Controls.Add(this.compiledRulesText);
            this.defSeedRulesTab.Controls.Add(this.seedText);
            this.defSeedRulesTab.Location = new System.Drawing.Point(4, 22);
            this.defSeedRulesTab.Name = "defSeedRulesTab";
            this.defSeedRulesTab.Size = new System.Drawing.Size(339, 356);
            this.defSeedRulesTab.TabIndex = 2;
            this.defSeedRulesTab.Text = "Seed & Rules";
            this.defSeedRulesTab.UseVisualStyleBackColor = true;
            this.defSeedRulesTab.Enter += new System.EventHandler(this.RefreshSeedRulesTab);
            // 
            // browseCompiledDLLButton
            // 
            this.browseCompiledDLLButton.BackColor = System.Drawing.Color.Transparent;
            this.browseCompiledDLLButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseCompiledDLLButton.Location = new System.Drawing.Point(8, 41);
            this.browseCompiledDLLButton.Name = "browseCompiledDLLButton";
            this.browseCompiledDLLButton.Size = new System.Drawing.Size(127, 22);
            this.browseCompiledDLLButton.TabIndex = 6;
            this.browseCompiledDLLButton.Text = "compiled rules DLL:";
            this.browseCompiledDLLButton.UseVisualStyleBackColor = false;
            this.browseCompiledDLLButton.Click += new System.EventHandler(this.browseCompiledDLLButton_Click);
            // 
            // browseDefSeedButton
            // 
            this.browseDefSeedButton.BackColor = System.Drawing.Color.Transparent;
            this.browseDefSeedButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseDefSeedButton.Location = new System.Drawing.Point(8, 8);
            this.browseDefSeedButton.Name = "browseDefSeedButton";
            this.browseDefSeedButton.Size = new System.Drawing.Size(127, 22);
            this.browseDefSeedButton.TabIndex = 6;
            this.browseDefSeedButton.Text = "default seed graph:";
            this.browseDefSeedButton.UseVisualStyleBackColor = false;
            this.browseDefSeedButton.Click += new System.EventHandler(this.browseDefSeedButton_Click);
            //
            // RSText
            //
            for (int i = 0; i != 10; i++)
            {
                this.RSText[i].BackColor = System.Drawing.Color.LightGray;
                this.RSText[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.RSText[i].ForeColor = System.Drawing.Color.DimGray;
                this.RSText[i].Location = new System.Drawing.Point(8, 74 + i * 25);
                this.RSText[i].Name = "RSText" + i.ToString();
                this.RSText[i].Size = new System.Drawing.Size(324, 22);
                this.RSText[i].TabIndex = 0;
                this.RSText[i].TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                this.RSText[i].AutoEllipsis = true;

                this.RSbutton[i].BackColor = System.Drawing.Color.Transparent;
                this.RSbutton[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.RSbutton[i].Location = new System.Drawing.Point(8, 74 + i * 25);
                this.RSbutton[i].Name = "RSbutton" + i.ToString();
                this.RSbutton[i].Size = new System.Drawing.Size(127, 22);
                this.RSbutton[i].TabIndex = i + 1;
                this.RSbutton[i].Text = "default RuleSet #" + i.ToString();
                this.RSbutton[i].UseVisualStyleBackColor = false;
                this.RSbutton[i].Click += new System.EventHandler(this.RSbutton_Click);

            }
            // 
            // compiledRulesText
            // 
            this.compiledRulesText.BackColor = System.Drawing.Color.LightGray;
            this.compiledRulesText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.compiledRulesText.ForeColor = System.Drawing.Color.DimGray;
            this.compiledRulesText.Location = new System.Drawing.Point(8, 41);
            this.compiledRulesText.Name = "compiledRulesText";
            this.compiledRulesText.Size = new System.Drawing.Size(324, 22);
            this.compiledRulesText.TabIndex = 7;
            this.compiledRulesText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.compiledRulesText.Leave += new System.EventHandler(compiledRulesText_Leave);
            // 
            // seedText
            // 
            this.seedText.BackColor = System.Drawing.Color.LightGray;
            this.seedText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.seedText.ForeColor = System.Drawing.Color.DimGray;
            this.seedText.Location = new System.Drawing.Point(8, 8);
            this.seedText.Name = "seedText";
            this.seedText.Size = new System.Drawing.Size(324, 22);
            this.seedText.TabIndex = 7;
            this.seedText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // globalSettingsDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.ClientSize = new System.Drawing.Size(347, 382);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "globalSettingsDisplay";
            this.Text = "Settings";
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.dirTab.ResumeLayout(false);
            this.limitsBoolsTab.ResumeLayout(false);
            this.defSeedRulesTab.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button browseWorkingDirButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage dirTab;
        private System.Windows.Forms.TabPage limitsBoolsTab;
        private System.Windows.Forms.TabPage defSeedRulesTab;
        private System.Windows.Forms.TextBox wDirText;
        private System.Windows.Forms.Button browseHelpDirButton;
        private System.Windows.Forms.Button browseRulseDirButton;
        private System.Windows.Forms.Button browseOutputDirButton;
        private System.Windows.Forms.Button browseInputDirButton;
        private System.Windows.Forms.TextBox helpDirText;
        private System.Windows.Forms.TextBox rDirText;
        private System.Windows.Forms.TextBox oDirText;
        private System.Windows.Forms.TextBox iDirText;
        private System.Windows.Forms.Button browseHelpURLButton;
        private System.Windows.Forms.TextBox helpURLText;
        private System.Windows.Forms.Button browseDefSeedButton;
        private System.Windows.Forms.Label seedText;
        private System.Windows.Forms.Button browseCompiledDLLButton;
        private System.Windows.Forms.TextBox compiledRulesText;

        private System.Windows.Forms.Button[] RSbutton = new System.Windows.Forms.Button[10];
        private System.Windows.Forms.Label[] RSText = new System.Windows.Forms.Label[10];

    }
}