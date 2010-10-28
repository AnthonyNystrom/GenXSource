using System.Threading;
namespace GraphSynth.Forms
{
    partial class searchProcessController
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

            if (searchThread.ThreadState == ThreadState.Suspended)
                searchThread.Resume(); 
            if (searchThread.ThreadState != ThreadState.Aborted) 
                searchThread.Abort();
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
            this.iterationLabel = new System.Windows.Forms.Label();
            this.iterationBox = new System.Windows.Forms.Label();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.miscBox = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.verbosityComboBox = new System.Windows.Forms.ComboBox();
            this.timeText = new System.Windows.Forms.Label();
            this.timeDisplay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // iterationLabel
            // 
            this.iterationLabel.AutoSize = true;
            this.iterationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iterationLabel.Location = new System.Drawing.Point(3, 3);
            this.iterationLabel.Name = "iterationLabel";
            this.iterationLabel.Size = new System.Drawing.Size(68, 16);
            this.iterationLabel.TabIndex = 1;
            this.iterationLabel.Text = "iterations :";
            // 
            // iterationBox
            // 
            this.iterationBox.BackColor = System.Drawing.Color.Black;
            this.iterationBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iterationBox.ForeColor = System.Drawing.Color.White;
            this.iterationBox.Location = new System.Drawing.Point(68, 2);
            this.iterationBox.Name = "iterationBox";
            this.iterationBox.Padding = new System.Windows.Forms.Padding(1);
            this.iterationBox.Size = new System.Drawing.Size(57, 23);
            this.iterationBox.TabIndex = 2;
            this.iterationBox.Text = "0";
            this.iterationBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pauseButton
            // 
            this.pauseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseButton.Image = global::GraphSynth.Properties.Resources.pause;
            this.pauseButton.Location = new System.Drawing.Point(56, 52);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(56, 50);
            this.pauseButton.TabIndex = 3;
            this.pauseButton.Text = "pause";
            this.pauseButton.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopButton.Image = global::GraphSynth.Properties.Resources.stop;
            this.stopButton.Location = new System.Drawing.Point(112, 52);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(56, 50);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "stop";
            this.stopButton.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // abortButton
            // 
            this.abortButton.BackgroundImage = global::GraphSynth.Properties.Resources.abort;
            this.abortButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abortButton.Location = new System.Drawing.Point(168, 52);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(56, 50);
            this.abortButton.TabIndex = 3;
            this.abortButton.Text = "abort";
            this.abortButton.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Click += new System.EventHandler(this.hardStopButton_Click);
            // 
            // playButton
            // 
            this.playButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playButton.Image = global::GraphSynth.Properties.Resources.play;
            this.playButton.Location = new System.Drawing.Point(0, 52);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(56, 50);
            this.playButton.TabIndex = 3;
            this.playButton.Text = "play";
            this.playButton.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // miscBox
            // 
            this.miscBox.BackColor = System.Drawing.Color.Black;
            this.miscBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.miscBox.ForeColor = System.Drawing.Color.White;
            this.miscBox.Location = new System.Drawing.Point(129, 2);
            this.miscBox.Name = "miscBox";
            this.miscBox.Padding = new System.Windows.Forms.Padding(3);
            this.miscBox.Size = new System.Drawing.Size(90, 23);
            this.miscBox.TabIndex = 2;
            this.miscBox.Text = "misc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "priority :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "verbosity :";
            // 
            // priorityComboBox
            // 
            this.priorityComboBox.BackColor = System.Drawing.Color.White;
            this.priorityComboBox.ForeColor = System.Drawing.Color.Black;
            this.priorityComboBox.FormattingEnabled = true;
            this.priorityComboBox.Items.AddRange(new object[] {
            "lowest",
            "below normal",
            "normal",
            "above normal",
            "highest"});
            this.priorityComboBox.Location = new System.Drawing.Point(72, 103);
            this.priorityComboBox.MaxDropDownItems = 10;
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.Size = new System.Drawing.Size(139, 21);
            this.priorityComboBox.TabIndex = 4;
            this.priorityComboBox.SelectedIndexChanged += new System.EventHandler(this.priorityComboBox_SelectedIndexChanged);
            // 
            // verbosityComboBox
            // 
            this.verbosityComboBox.BackColor = System.Drawing.Color.White;
            this.verbosityComboBox.ForeColor = System.Drawing.Color.Black;
            this.verbosityComboBox.FormattingEnabled = true;
            this.verbosityComboBox.Items.AddRange(new object[] {
            "lowest",
            "below normal",
            "normal",
            "above normal",
            "highest"});
            this.verbosityComboBox.Location = new System.Drawing.Point(72, 127);
            this.verbosityComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.verbosityComboBox.MaxDropDownItems = 10;
            this.verbosityComboBox.Name = "verbosityComboBox";
            this.verbosityComboBox.Size = new System.Drawing.Size(139, 21);
            this.verbosityComboBox.TabIndex = 4;
            this.verbosityComboBox.SelectedIndexChanged += new System.EventHandler(this.verbosityComboBox_SelectedIndexChanged);
            // 
            // timeText
            // 
            this.timeText.AutoSize = true;
            this.timeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeText.Location = new System.Drawing.Point(3, 31);
            this.timeText.Name = "timeText";
            this.timeText.Size = new System.Drawing.Size(113, 16);
            this.timeText.TabIndex = 5;
            this.timeText.Text = "time: (mm:ss.sss):";
            // 
            // timeDisplay
            // 
            this.timeDisplay.BackColor = System.Drawing.Color.Black;
            this.timeDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeDisplay.ForeColor = System.Drawing.Color.White;
            this.timeDisplay.Location = new System.Drawing.Point(129, 28);
            this.timeDisplay.Name = "timeDisplay";
            this.timeDisplay.Padding = new System.Windows.Forms.Padding(1);
            this.timeDisplay.Size = new System.Drawing.Size(90, 23);
            this.timeDisplay.TabIndex = 6;
            this.timeDisplay.Text = "00:00:00.00";
            this.timeDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchProcessController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 149);
            this.Controls.Add(this.timeDisplay);
            this.Controls.Add(this.timeText);
            this.Controls.Add(this.verbosityComboBox);
            this.Controls.Add(this.priorityComboBox);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.miscBox);
            this.Controls.Add(this.iterationBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.iterationLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "searchProcessController";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Search Process Controller";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label iterationLabel;
        private System.Windows.Forms.Label iterationBox;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button abortButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Label miscBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox priorityComboBox;
        private System.Windows.Forms.ComboBox verbosityComboBox;
        private System.Windows.Forms.Label timeText;
        private System.Windows.Forms.Label timeDisplay;



    }
}