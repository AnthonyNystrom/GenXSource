namespace GraphSynth.Forms
{
    partial class chooseDisplay
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
            this.applyButton = new System.Windows.Forms.Button();
            this.removeFromListButton1 = new System.Windows.Forms.Button();
            this.recognizedRulesList = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.undoButton = new System.Windows.Forms.Button();
            this.stopGenerationButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stopGenerationButton);
            this.panel1.Controls.Add(this.applyButton);
            this.panel1.Controls.Add(this.undoButton);
            this.panel1.Controls.Add(this.removeFromListButton1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 369);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 52);
            this.panel1.TabIndex = 0;
            // 
            // applyButton
            // 
            this.applyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyButton.Location = new System.Drawing.Point(140, 27);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(124, 23);
            this.applyButton.TabIndex = 1;
            this.applyButton.Text = "apply!";
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // removeFromListButton1
            // 
            this.removeFromListButton1.Location = new System.Drawing.Point(3, 3);
            this.removeFromListButton1.Name = "removeFromListButton1";
            this.removeFromListButton1.Size = new System.Drawing.Size(124, 23);
            this.removeFromListButton1.TabIndex = 0;
            this.removeFromListButton1.Text = "remove from list";
            this.removeFromListButton1.Click += new System.EventHandler(this.removeFromList_Click);
            // 
            // recognizedRulesList
            // 
            this.recognizedRulesList.BackColor = System.Drawing.SystemColors.Info;
            this.recognizedRulesList.CheckOnClick = true;
            this.recognizedRulesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recognizedRulesList.FormattingEnabled = true;
            this.recognizedRulesList.HorizontalScrollbar = true;
            this.recognizedRulesList.ImeMode = System.Windows.Forms.ImeMode.On;
            this.recognizedRulesList.Location = new System.Drawing.Point(0, 20);
            this.recognizedRulesList.Name = "recognizedRulesList";
            this.recognizedRulesList.ScrollAlwaysVisible = true;
            this.recognizedRulesList.Size = new System.Drawing.Size(267, 349);
            this.recognizedRulesList.TabIndex = 1;
            this.recognizedRulesList.ThreeDCheckBoxes = true;
            this.recognizedRulesList.DoubleClick += new System.EventHandler(this.showGraph_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(267, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Option      |   Rule #  | Location";
            // 
            // undoButton
            // 
            this.undoButton.Location = new System.Drawing.Point(140, 3);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(124, 23);
            this.undoButton.TabIndex = 0;
            this.undoButton.Text = "undo last rule";
            this.undoButton.Click += new System.EventHandler(this.undo_Click);
            // 
            // stopGenerationButton
            // 
            this.stopGenerationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopGenerationButton.ForeColor = System.Drawing.Color.Red;
            this.stopGenerationButton.Location = new System.Drawing.Point(3, 27);
            this.stopGenerationButton.Name = "stopGenerationButton";
            this.stopGenerationButton.Size = new System.Drawing.Size(124, 23);
            this.stopGenerationButton.TabIndex = 1;
            this.stopGenerationButton.Text = "stop generation";
            this.stopGenerationButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // chooseDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 421);
            this.Controls.Add(this.recognizedRulesList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "chooseDisplay";
            this.Text = "chooseDisplay";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button removeFromListButton1;
        public System.Windows.Forms.CheckedListBox recognizedRulesList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button stopGenerationButton;
        private System.Windows.Forms.Button undoButton;


    }
}