namespace Genetibase.NuGenMediImage.UI.Controls
{
    partial class ViewerAnnot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewerAnnot));
            this.drawArea = new Genetibase.NuGenAnnotation.DrawArea();
            this.viewer = new Genetibase.NuGenMediImage.UI.Controls.Viewer();
            this.SuspendLayout();
            // 
            // drawArea
            // 
            this.drawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Pointer;
            this.drawArea.BackColor = System.Drawing.Color.Red;
            this.drawArea.BrushType = Genetibase.NuGenAnnotation.FillBrushes.BrushType.Brown;
            this.drawArea.CurrentBrush = null;
            this.drawArea.CurrentPen = null;
            this.drawArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawArea.DocManager = null;
            this.drawArea.DrawFilled = false;
            this.drawArea.DrawNetRectangle = false;
            this.drawArea.FillColor = System.Drawing.Color.Red;
            this.drawArea.LineColor = System.Drawing.Color.Black;
            this.drawArea.LineWidth = -1;
            this.drawArea.Location = new System.Drawing.Point(0, 0);
            this.drawArea.MyParent = null;
            this.drawArea.Name = "drawArea";
            this.drawArea.NetRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.drawArea.OriginalPanY = 0;
            this.drawArea.Panning = false;
            this.drawArea.PanX = 0;
            this.drawArea.PanY = 0;
            this.drawArea.PenType = Genetibase.NuGenAnnotation.DrawingPens.PenType.Generic;
            this.drawArea.Rotation = 0F;
            this.drawArea.Size = new System.Drawing.Size(495, 376);
            this.drawArea.TabIndex = 0;            
            this.drawArea.Zoom = 1F;
            // 
            // viewer
            // 
            this.viewer.AnnotationMode = true;
            this.viewer.AutoScroll = true;
            this.viewer.BackColor = System.Drawing.Color.Black;
            this.viewer.Brightness = 0F;
            this.viewer.Contrast = 1F;
            this.viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewer.Emboss = 0;
            this.viewer.HeaderVisible = false;
            this.viewer.Image = null;
            this.viewer.Location = new System.Drawing.Point(0, 0);
            this.viewer.MeasurementUnit = Genetibase.NuGenMediImage.MeasurementUnits.Inches;
            this.viewer.Name = "viewer";
            this.viewer.SelectedIndex = 0;
            this.viewer.Size = new System.Drawing.Size(495, 376);
            this.viewer.TabIndex = 1;
            this.viewer.Zoom = 1;
            this.viewer.ZoomBoxSize = new System.Drawing.Size(100, 100);
            this.viewer.ZoomBoxZoom = 2;
            this.viewer.ZoomFit = false;
            this.viewer.ZoomWithDrag = false;
            // 
            // ViewerAnnot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.drawArea);
            this.Name = "ViewerAnnot";
            this.Size = new System.Drawing.Size(495, 376);
            this.ResumeLayout(false);

        }

        #endregion

        private Genetibase.NuGenAnnotation.DrawArea drawArea;
        private Viewer viewer;        
    }
}
