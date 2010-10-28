namespace GraphSynth.Forms
{
    partial class graphDisplay
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
            this.graphControl1 = new Netron.GraphLib.UI.GraphControl();
            this.globalLabelsText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // graphControl1
            // 
            this.graphControl1.AllowAddConnection = true;
            this.graphControl1.AllowAddShape = true;
            this.graphControl1.AllowDeleteShape = true;
            this.graphControl1.AllowDrop = true;
            this.graphControl1.AllowMoveShape = true;
            this.graphControl1.AutomataPulse = 10;
            this.graphControl1.AutoScroll = true;
            this.graphControl1.BackgroundColor = System.Drawing.Color.Transparent;
            this.graphControl1.BackgroundImagePath = null;
            this.graphControl1.BackgroundType = Netron.GraphLib.CanvasBackgroundType.Gradient;
            this.graphControl1.DefaultConnectionEnd = Netron.GraphLib.ConnectionEnd.NoEnds;
            this.graphControl1.DefaultConnectionPath = "Default";
            this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControl1.DoTrack = false;
            this.graphControl1.EnableContextMenu = true;
            this.graphControl1.EnableLayout = true;
            this.graphControl1.EnableToolTip = true;
            this.graphControl1.FileName = null;
            this.graphControl1.GradientBottom = System.Drawing.Color.LightSteelBlue;
            this.graphControl1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.graphControl1.GradientTop = System.Drawing.Color.MintCream;
            this.graphControl1.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.Tree;
            this.graphControl1.GridSize = 5;
            this.graphControl1.Location = new System.Drawing.Point(0, 0);
            this.graphControl1.Name = "graphControl1";
            this.graphControl1.RestrictToCanvas = true;
            this.graphControl1.ShowAutomataController = false;
            this.graphControl1.ShowGrid = false;
            this.graphControl1.Size = new System.Drawing.Size(335, 374);
            this.graphControl1.Snap = true;
            this.graphControl1.TabIndex = 0;
            this.graphControl1.Text = "graphControl1";
            this.graphControl1.Zoom = 1F;
            this.graphControl1.OnShowProperties += new Netron.GraphLib.PropertiesInfo(this.graphControl1_OnShowProperties);
            // 
            // globalLabelsText
            // 
            this.globalLabelsText.AutoSize = true;
            this.globalLabelsText.BackColor = System.Drawing.Color.Transparent;
            this.globalLabelsText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.globalLabelsText.Location = new System.Drawing.Point(2, 2);
            this.globalLabelsText.Name = "globalLabelsText";
            this.globalLabelsText.Size = new System.Drawing.Size(11, 16);
            this.globalLabelsText.TabIndex = 1;
            this.globalLabelsText.Text = " ";
            this.globalLabelsText.DoubleClick += new System.EventHandler(this.globalLabelsText_DoubleClick);
            // 
            // graphDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 374);
            this.Controls.Add(this.globalLabelsText);
            this.Controls.Add(this.graphControl1);
            this.Name = "graphDisplay";
            this.Text = "graphDisplay";
            this.Enter += new System.EventHandler(this.graphDisplay_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Netron.GraphLib.UI.GraphControl graphControl1;
        public System.Windows.Forms.Label globalLabelsText;
    }
}