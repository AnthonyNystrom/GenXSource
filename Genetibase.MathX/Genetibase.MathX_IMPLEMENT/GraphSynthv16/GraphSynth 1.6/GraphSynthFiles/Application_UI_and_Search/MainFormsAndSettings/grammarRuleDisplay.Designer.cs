namespace GraphSynth.Forms
{
    partial class grammarRuleDisplay
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
            this.components = new System.ComponentModel.Container();
            this.graphControlLHS = new Netron.GraphLib.UI.GraphControl();
            this.graphControlRHS = new Netron.GraphLib.UI.GraphControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.globalLabelsLText = new System.Windows.Forms.Label();
            this.globalLabelsRText = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphControlLHS
            // 
            this.graphControlLHS.AllowAddConnection = true;
            this.graphControlLHS.AllowAddShape = true;
            this.graphControlLHS.AllowDeleteShape = true;
            this.graphControlLHS.AllowDrop = true;
            this.graphControlLHS.AllowMoveShape = true;
            this.graphControlLHS.AutomataPulse = 10;
            this.graphControlLHS.AutoScroll = true;
            this.graphControlLHS.BackgroundColor = System.Drawing.Color.White;
            this.graphControlLHS.BackgroundImagePath = null;
            this.graphControlLHS.BackgroundType = Netron.GraphLib.CanvasBackgroundType.Gradient;
            this.graphControlLHS.DefaultConnectionEnd = Netron.GraphLib.ConnectionEnd.NoEnds;
            this.graphControlLHS.DefaultConnectionPath = "Default";
            this.graphControlLHS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControlLHS.DoTrack = false;
            this.graphControlLHS.EnableContextMenu = true;
            this.graphControlLHS.EnableLayout = true;
            this.graphControlLHS.EnableToolTip = true;
            this.graphControlLHS.FileName = null;
            this.graphControlLHS.GradientBottom = System.Drawing.Color.MintCream;
            this.graphControlLHS.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.graphControlLHS.GradientTop = System.Drawing.Color.LightSteelBlue;
            this.graphControlLHS.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.Tree;
            this.graphControlLHS.GridSize = 5;
            this.graphControlLHS.Location = new System.Drawing.Point(0, 0);
            this.graphControlLHS.Name = "graphControlLHS";
            this.graphControlLHS.RestrictToCanvas = true;
            this.graphControlLHS.ShowAutomataController = false;
            this.graphControlLHS.ShowGrid = false;
            this.graphControlLHS.Size = new System.Drawing.Size(287, 270);
            this.graphControlLHS.Snap = true;
            this.graphControlLHS.TabIndex = 0;
            this.graphControlLHS.Text = "graphControlLHS";
            this.graphControlLHS.Zoom = 1F;
            this.graphControlLHS.OnShowProperties += new Netron.GraphLib.PropertiesInfo(this.graphControlLHS_OnShowProperties);
            // 
            // graphControlRHS
            // 
            this.graphControlRHS.AllowAddConnection = true;
            this.graphControlRHS.AllowAddShape = true;
            this.graphControlRHS.AllowDeleteShape = true;
            this.graphControlRHS.AllowDrop = true;
            this.graphControlRHS.AllowMoveShape = true;
            this.graphControlRHS.AutomataPulse = 10;
            this.graphControlRHS.AutoScroll = true;
            this.graphControlRHS.BackgroundColor = System.Drawing.Color.White;
            this.graphControlRHS.BackgroundImagePath = null;
            this.graphControlRHS.BackgroundType = Netron.GraphLib.CanvasBackgroundType.Gradient;
            this.graphControlRHS.DefaultConnectionEnd = Netron.GraphLib.ConnectionEnd.NoEnds;
            this.graphControlRHS.DefaultConnectionPath = "Default";
            this.graphControlRHS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControlRHS.DoTrack = false;
            this.graphControlRHS.EnableContextMenu = true;
            this.graphControlRHS.EnableLayout = true;
            this.graphControlRHS.EnableToolTip = true;
            this.graphControlRHS.FileName = null;
            this.graphControlRHS.GradientBottom = System.Drawing.Color.LightSteelBlue;
            this.graphControlRHS.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.graphControlRHS.GradientTop = System.Drawing.Color.MintCream;
            this.graphControlRHS.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.Tree;
            this.graphControlRHS.GridSize = 5;
            this.graphControlRHS.Location = new System.Drawing.Point(0, 0);
            this.graphControlRHS.Name = "graphControlRHS";
            this.graphControlRHS.RestrictToCanvas = true;
            this.graphControlRHS.ShowAutomataController = false;
            this.graphControlRHS.ShowGrid = false;
            this.graphControlRHS.Size = new System.Drawing.Size(298, 270);
            this.graphControlRHS.Snap = true;
            this.graphControlRHS.TabIndex = 1;
            this.graphControlRHS.Text = "graphControlRHS";
            this.graphControlRHS.Zoom = 1F;
            this.graphControlRHS.OnShowProperties += new Netron.GraphLib.PropertiesInfo(this.graphControlRHS_OnShowProperties);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.MidnightBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Left Hand Side (recognize sub-graph)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.MidnightBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(102, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Right Hand Side (apply sub-graph)";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.globalLabelsLText);
            this.splitContainer1.Panel1.Controls.Add(this.graphControlLHS);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.globalLabelsRText);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.graphControlRHS);
            this.splitContainer1.Size = new System.Drawing.Size(595, 270);
            this.splitContainer1.SplitterDistance = 287;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // globalLabelsLText
            // 
            this.globalLabelsLText.AutoSize = true;
            this.globalLabelsLText.BackColor = System.Drawing.Color.LightSteelBlue;
            this.globalLabelsLText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.globalLabelsLText.Location = new System.Drawing.Point(2, 18);
            this.globalLabelsLText.Name = "globalLabelsLText";
            this.globalLabelsLText.Size = new System.Drawing.Size(0, 16);
            this.globalLabelsLText.TabIndex = 2;
            this.globalLabelsLText.DoubleClick += new System.EventHandler(this.globalLabelsLText_DoubleClick);
            // 
            // globalLabelsRText
            // 
            this.globalLabelsRText.AutoSize = true;
            this.globalLabelsRText.BackColor = System.Drawing.Color.MintCream;
            this.globalLabelsRText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.globalLabelsRText.Location = new System.Drawing.Point(2, 18);
            this.globalLabelsRText.Name = "globalLabelsRText";
            this.globalLabelsRText.Size = new System.Drawing.Size(0, 16);
            this.globalLabelsRText.TabIndex = 4;
            this.globalLabelsRText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.globalLabelsRText.DoubleClick += new System.EventHandler(this.globalLabelsRText_DoubleClick);
            // 
            // grammarRuleDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 270);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "grammarRuleDisplay";
            this.Text = "grammarRuleDisplay";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Netron.GraphLib.UI.GraphControl graphControlLHS;
        public Netron.GraphLib.UI.GraphControl graphControlRHS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.Label globalLabelsLText;
        public System.Windows.Forms.Label globalLabelsRText;
    }
}