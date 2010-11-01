using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class SegmentsDialog : Form
    {
        private SegmentSettings settings;

        public SegmentsDialog(SegmentSettings settings)
        {
            this.settings = settings;

            InitializeComponent();
            InitializeDefaults();

            this.MaximumSize = Size;
        }

        private void InitializeDefaults()
        {
            string[] colors = {
                "Black" , "Blue", "Cyan", "Gold", "Green", "Magenta" , "Red", "Transparent", "Yellow"
            };

            string[] sizes = {
                "1", "2", "3", "4", "5", "6", "7", "8"
            };

            lineSizeCombo.Items.AddRange(sizes);
            lineColorCombo.Items.AddRange(colors);

            int size = (int)settings.lineSize;

            lineSizeCombo.SelectedItem = size;

            Color c = settings.lineColor;

            if (c.Equals(Color.Black))
                lineColorCombo.SelectedItem = "Black";
            else if (c.Equals((Color.Blue)))
                lineColorCombo.SelectedItem = "Blue";
            else if (c.Equals((Color.Cyan)))
                lineColorCombo.SelectedItem = "Cyan";
            else if (c.Equals((Color.Gold)))
                lineColorCombo.SelectedItem = "Gold";
            else if (c.Equals((Color.Lime)))
                lineColorCombo.SelectedItem = "Green";
            else if (c.Equals((Color.Magenta)))
                lineColorCombo.SelectedItem = "Magenta";
            else if (c.Equals((Color.Red)))
                lineColorCombo.SelectedItem = "Red";
            else if (c.Equals((Color.Transparent)))
                lineColorCombo.SelectedItem = "Transparent";
            else if (c.Equals((Color.Yellow)))
                lineColorCombo.SelectedItem ="Yellow";

            lineSizeCombo.SelectedItem = size.ToString();

            fillCornersCheckBox.Checked = settings.fillCorners;

            minLengthTextBox.Text = settings.minPoints.ToString();
            pointSeperationTextBox.Text = settings.pointSeparation.ToString();
        }

        public SegmentSettings SegmentSettings
        {
            get
            {
                return settings;
            }

            set
            {
                settings = value;
            }
        }

        private void minLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            settings.minPoints = int.Parse(minLengthTextBox.Text);
        }

        private void pointSeperationTextBox_TextChanged(object sender, EventArgs e)
        {
            settings.pointSeparation = int.Parse(pointSeperationTextBox.Text);
        }

        private void fillCornersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            settings.fillCorners = fillCornersCheckBox.Checked;
        }

        private void lineSizeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.lineSize = (LineSize)int.Parse((string)lineSizeCombo.SelectedItem);
        }

        private void lineColorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lineColorCombo.SelectedItem.Equals("Black"))
                settings.lineColor = Color.Black;
            else if (lineColorCombo.SelectedItem.Equals("Blue"))
                settings.lineColor = Color.Blue;
            else if (lineColorCombo.SelectedItem.Equals("Cyan"))
                settings.lineColor = Color.Cyan;
            else if (lineColorCombo.SelectedItem.Equals("Gold"))
                settings.lineColor = Color.Gold;
            else if (lineColorCombo.SelectedItem.Equals("Magenta"))
                settings.lineColor = Color.Magenta;
            else if (lineColorCombo.SelectedItem.Equals("Red"))
                settings.lineColor = Color.Red;
            else if (lineColorCombo.SelectedItem.Equals("Transparent"))
                settings.lineColor = Color.Transparent;
            else if (lineColorCombo.SelectedItem.Equals("Yellow"))
                settings.lineColor = Color.Yellow;
            else if (lineColorCombo.SelectedItem.Equals("Green"))
                settings.lineColor = Color.Lime;
        }
    }
}