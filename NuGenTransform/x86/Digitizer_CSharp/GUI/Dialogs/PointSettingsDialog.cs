using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class PointSettingsDialog : Form
    {
        private PointSetStyle origStyle;

        public PointSettingsDialog(PointSetStyle origStyle, bool axesScale)
        {
            this.origStyle = origStyle;
            InitializeComponent();
            InitializeDefaultSettings();         
            preview.Paint += new PaintEventHandler(preview_Paint);

            if (axesScale)
                lineConnectAsCombo.Enabled = false;

            this.MaximumSize = Size;
        }

        public PointSetStyle Style
        {
            get
            {
                return origStyle;
            }
        }

        void preview_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen linePen = new Pen(origStyle.lineColor, (int)origStyle.lineSize);
            g.DrawLine(linePen, new Point(50, 50), new Point(175, 50));

            NuGenView.DrawPoint(g, new Point(50, 50), origStyle);
            NuGenView.DrawPoint(g, new Point(175, 50), origStyle);
        }        

        private void InitializeDefaultSettings()
        {
            string[] sizeOptions = {
                "1", "2", "3", "4", "5", "6", "7", "8"
            };

            string[] pointTypeOptions = {
                "X", "Cross", "Triangle", "Square", "Diamond"
            };

            string[] colorOptions = {
                "Black", "Blue", "Cyan", "Gold", "Magenta", "Green", "Red", "Transparent", "Yellow"
            };

            string[] connectAs = {
                "Single Valued Function", "Contour"
            };

            sizeCombo.Items.AddRange(sizeOptions);
            lineSizeCombo.Items.AddRange(sizeOptions);
            pointLineSizeCombo.Items.AddRange(sizeOptions);

            pointInteriorColorCombo.Items.AddRange(colorOptions);
            pointLineColorCombo.Items.AddRange(colorOptions);
            lineColorCombo.Items.AddRange(colorOptions);

            lineConnectAsCombo.Items.AddRange(connectAs);

            shapeCombo.Items.AddRange(pointTypeOptions);

            //Select Defaults
            
            pointLineColorCombo.SelectedItem = origStyle.pointLineColor.Name;
            pointInteriorColorCombo.SelectedItem = origStyle.pointInColor.Name;
            lineColorCombo.SelectedItem = origStyle.lineColor.Name;

            sizeCombo.SelectedItem = ((int)origStyle.pointSize).ToString();
            lineSizeCombo.SelectedItem = ((int)origStyle.lineSize).ToString();
            pointLineSizeCombo.SelectedItem = ((int)origStyle.pointLineSize).ToString();

            switch (origStyle.lineConnectAs)
            {
                case LineConnectAs.Contour:
                    lineConnectAsCombo.SelectedItem = "Contour";
                    break;
                case LineConnectAs.SingleValuedFunction:
                    lineConnectAsCombo.SelectedItem = "Single Valued Function";
                    break;
            }

            switch (origStyle.pointShape)
            {
                case PointShape.Cross:
                    shapeCombo.SelectedItem = "Cross";
                    break;
                case PointShape.Diamond:
                    shapeCombo.SelectedItem = "Diamond";
                    break;
                case PointShape.Square:
                    shapeCombo.SelectedItem = "Square";
                    break;
                case PointShape.Triangle:
                    shapeCombo.SelectedItem = "Triangle";
                    break;
                case PointShape.X:
                    shapeCombo.SelectedItem = "X";
                    break;
            }
        }

        private void shapeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((string)shapeCombo.SelectedItem)
            {
                case "Cross":
                    origStyle.pointShape = PointShape.Cross; break;
                case "X":
                    origStyle.pointShape = PointShape.X; break;
                case "Square":
                    origStyle.pointShape = PointShape.Square; break;
                case "Triangle":
                    origStyle.pointShape = PointShape.Triangle; break;
                case "Diamond":
                    origStyle.pointShape = PointShape.Diamond; break;
            }

            Refresh();
        }

        private void sizeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int size = int.Parse((string)sizeCombo.SelectedItem);

            origStyle.pointSize = (PointSize)size;

            Refresh();
        }

        private void pointLineSizeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int size = int.Parse((string)pointLineSizeCombo.SelectedItem);

            origStyle.pointLineSize = (PointLineSize)size;

            Refresh();
        }

        private void pointLineColorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((string)pointLineColorCombo.SelectedItem)
            {
                case "Black":
                    origStyle.pointLineColor = Color.Black; break;
                case "Blue":
                    origStyle.pointLineColor = Color.Blue; break;
                case "Green":
                    origStyle.pointLineColor = Color.Lime; break;
                case "Gold":
                    origStyle.pointLineColor = Color.Gold; break;
                case "Cyan":
                    origStyle.pointLineColor = Color.Cyan; break;
                case "Magenta":
                    origStyle.pointLineColor = Color.Magenta; break;
                case "Transparent":
                    origStyle.pointLineColor = Color.Transparent; break;
                case "Red":
                    origStyle.pointLineColor = Color.Red; break;                
            }

            Refresh();
        }

        private void pointInteriorColorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((string)pointInteriorColorCombo.SelectedItem)
            {
                case "Black":
                    origStyle.pointInColor = Color.Black; break;
                case "Blue":
                    origStyle.pointInColor = Color.Blue; break;
                case "Green":
                    origStyle.pointInColor = Color.Lime; break;
                case "Gold":
                    origStyle.pointInColor = Color.Gold; break;
                case "Cyan":
                    origStyle.pointInColor = Color.Cyan; break;
                case "Magenta":
                    origStyle.pointInColor = Color.Magenta; break;
                case "Transparent":
                    origStyle.pointInColor = Color.Transparent; break;
                case "Red":
                    origStyle.pointInColor = Color.Red; break;
            }

            Refresh();
        }

        private void lineSizeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int size = int.Parse((string)lineSizeCombo.SelectedItem);

            origStyle.lineSize = (LineSize)size;

            Refresh();
        }

        private void lineColorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((string)lineColorCombo.SelectedItem)
            {
                case "Black":
                    origStyle.lineColor = Color.Black; break;
                case "Blue":
                    origStyle.lineColor = Color.Blue; break;
                case "Green":
                    origStyle.lineColor = Color.Lime; break;
                case "Gold":
                    origStyle.lineColor = Color.Gold; break;
                case "Cyan":
                    origStyle.lineColor = Color.Cyan; break;
                case "Magenta":
                    origStyle.lineColor = Color.Magenta; break;
                case "Transparent":
                    origStyle.lineColor = Color.Transparent; break;
                case "Red":
                    origStyle.lineColor = Color.Red; break;
            }

            Refresh();
        }

        private void lineConnectAsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lineConnectAsCombo.SelectedItem.Equals("Contour"))
                origStyle.lineConnectAs = LineConnectAs.Contour;
            else
                origStyle.lineConnectAs = LineConnectAs.SingleValuedFunction;
        }
    }
}