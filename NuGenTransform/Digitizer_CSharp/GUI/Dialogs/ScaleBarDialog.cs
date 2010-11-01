using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public enum LengthUnits
    {
        Unitless,
        Angstrom,
        Nanometer,
        Milimeter,
        Centimeter,
        Decimeter,
        Meter,
        Kilometer,
        LightYear,
        Parsec,
        Inch,
        Foot,
        Yard,
        Mile,
        AstronomicalUnit,
        Fathom,
        Micrometer,
        Picometer
    }

    public partial class ScaleBarDialog : Form
    {
        public ScaleBarDialog()
        {
            InitializeComponent();
            textBox1.Text = "0";

            this.MaximumSize = Size;

            Array units = Enum.GetValues(typeof(LengthUnits));

            foreach (int num in units)
            {                
                comboBox1.Items.Add((LengthUnits)num);
            }

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
        }

        public double Length
        {
            get
            {
                return double.Parse(textBox1.Text);
            }
        }

        public LengthUnits Units
        {
            get
            {
                return (LengthUnits)comboBox1.SelectedItem;
            }
        }
    }
}