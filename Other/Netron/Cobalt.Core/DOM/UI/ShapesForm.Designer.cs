namespace Netron.Cobalt
{
    partial class ShapesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShapesForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.ShapesListView = new System.Windows.Forms.ListView();
            this.shapesImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.ShapesListView);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(292, 248);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(292, 273);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // ShapesListView
            // 
            this.ShapesListView.BackColor = System.Drawing.Color.White;
            this.ShapesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ShapesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShapesListView.LargeImageList = this.shapesImageList;
            this.ShapesListView.Location = new System.Drawing.Point(0, 0);
            this.ShapesListView.MultiSelect = false;
            this.ShapesListView.Name = "ShapesListView";
            this.ShapesListView.ShowItemToolTips = true;
            this.ShapesListView.Size = new System.Drawing.Size(292, 248);
            this.ShapesListView.TabIndex = 2;
            this.ShapesListView.UseCompatibleStateImageBehavior = false;
            this.ShapesListView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.shapesListView_GiveFeedback);
            this.ShapesListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.shapesListView_MouseDown);
            // 
            // shapesImageList
            // 
            this.shapesImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("shapesImageList.ImageStream")));
            this.shapesImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.shapesImageList.Images.SetKeyName(0, "DefaultImage.png");
            this.shapesImageList.Images.SetKeyName(1, "ClassShape.png");
            this.shapesImageList.Images.SetKeyName(2, "DecisionShape.png");
            this.shapesImageList.Images.SetKeyName(3, "ImageShape.png");
            this.shapesImageList.Images.SetKeyName(4, "ComplexRectangle.png");
            // 
            // ShapesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.toolStripContainer1);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShapesForm";
            this.TabText = "ShapesForm";
            this.Text = "Shapes";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        public System.Windows.Forms.ListView ShapesListView;
        private System.Windows.Forms.ImageList shapesImageList;
    }
}