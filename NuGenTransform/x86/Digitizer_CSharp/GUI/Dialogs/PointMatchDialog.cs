using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class PointMatchDialog : Form
    {
        private PointMatchSettings settings;

        public PointMatchDialog(PointMatchSettings settings)
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

            acceptedColorCombo.Items.AddRange(colors);
            rejectedColorCombo.Items.AddRange(colors);                   

            Color c = settings.rejectedColor;

            if (c.Equals(Color.Black))
                rejectedColorCombo.SelectedItem = "Black";
            else if (c.Equals((Color.Blue)))
                rejectedColorCombo.SelectedItem = "Blue";
            else if (c.Equals((Color.Cyan)))
                rejectedColorCombo.SelectedItem = "Cyan";
            else if (c.Equals((Color.Gold)))
                rejectedColorCombo.SelectedItem = "Gold";
            else if (c.Equals((Color.Green)))
                rejectedColorCombo.SelectedItem = "Green";
            else if (c.Equals((Color.Magenta)))
                rejectedColorCombo.SelectedItem = "Magenta";
            else if (c.Equals((Color.Red)))
                rejectedColorCombo.SelectedItem = "Red";
            else if (c.Equals((Color.Transparent)))
                rejectedColorCombo.SelectedItem = "Transparent";
            else if (c.Equals((Color.Yellow)))
                rejectedColorCombo.SelectedItem ="Yellow";

            c = settings.acceptedColor;

            if (c.Equals(Color.Black))
                acceptedColorCombo.SelectedItem = "Black";
            else if (c.Equals((Color.Blue)))
                acceptedColorCombo.SelectedItem = "Blue";
            else if (c.Equals((Color.Cyan)))
                acceptedColorCombo.SelectedItem = "Cyan";
            else if (c.Equals((Color.Gold)))
                acceptedColorCombo.SelectedItem = "Gold";
            else if (c.Equals((Color.Lime)))
                acceptedColorCombo.SelectedItem = "Green";
            else if (c.Equals((Color.Magenta)))
                acceptedColorCombo.SelectedItem = "Magenta";
            else if (c.Equals((Color.Red)))
                acceptedColorCombo.SelectedItem = "Red";
            else if (c.Equals((Color.Transparent)))
                acceptedColorCombo.SelectedItem = "Transparent";
            else if (c.Equals((Color.Yellow)))
                acceptedColorCombo.SelectedItem = "Yellow";

            minSizeTextBox.Text = settings.pointSize.ToString();
            pointSeperationTextBox.Text = settings.pointSeparation.ToString();
        }

        public PointMatchSettings PointMatchSettings
        {
            get
            {
                return settings;
            }
        }

        private void minLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            settings.pointSize = int.Parse(minSizeTextBox.Text);
        }

        private void pointSeperationTextBox_TextChanged(object sender, EventArgs e)
        {
            settings.pointSeparation = int.Parse(pointSeperationTextBox.Text);
        }

        private void acceptedColorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(acceptedColorCombo.SelectedItem.Equals("Black"))
                settings.acceptedColor = Color.Black;
            else if(acceptedColorCombo.SelectedItem.Equals("Blue"))
                settings.acceptedColor = Color.Blue;
            else if (acceptedColorCombo.SelectedItem.Equals("Cyan"))
                settings.acceptedColor = Color.Cyan;
            else if (acceptedColorCombo.SelectedItem.Equals("Gold"))
                settings.acceptedColor = Color.Gold;
            else if (acceptedColorCombo.SelectedItem.Equals("Magenta"))
                settings.acceptedColor = Color.Magenta;
            else if (acceptedColorCombo.SelectedItem.Equals("Red"))
                settings.acceptedColor = Color.Red;
            else if (acceptedColorCombo.SelectedItem.Equals("Transparent"))
                settings.acceptedColor = Color.Transparent;
            else if (acceptedColorCombo.SelectedItem.Equals("Yellow"))
                settings.acceptedColor = Color.Yellow;
            else if (acceptedColorCombo.SelectedItem.Equals("Green"))
                settings.acceptedColor = Color.Green;
        }

        private void rejectedColorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rejectedColorCombo.SelectedItem.Equals("Black"))
                settings.rejectedColor = Color.Black;
            else if (rejectedColorCombo.SelectedItem.Equals("Blue"))
                settings.rejectedColor = Color.Blue;
            else if (rejectedColorCombo.SelectedItem.Equals("Cyan"))
                settings.rejectedColor = Color.Cyan;
            else if (rejectedColorCombo.SelectedItem.Equals("Gold"))
                settings.rejectedColor = Color.Gold;
            else if (rejectedColorCombo.SelectedItem.Equals("Magenta"))
                settings.rejectedColor = Color.Magenta;
            else if (rejectedColorCombo.SelectedItem.Equals("Red"))
                settings.rejectedColor = Color.Red;
            else if (rejectedColorCombo.SelectedItem.Equals("Transparent"))
                settings.rejectedColor = Color.Transparent;
            else if (rejectedColorCombo.SelectedItem.Equals("Yellow"))
                settings.rejectedColor = Color.Yellow;
            else if (rejectedColorCombo.SelectedItem.Equals("Green"))
                settings.rejectedColor = Color.Green;
        }
    }
}