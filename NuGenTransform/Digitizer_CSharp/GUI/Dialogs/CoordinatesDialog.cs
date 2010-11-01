using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class CoordinatesDialog : Form
    {
        private CoordSettings origSettings;

        public CoordinatesDialog(CoordSettings origSettings)
        {
            this.origSettings = origSettings;

            InitializeComponent();

            InitializeDefaults();

            this.MaximumSize = Size;
        }

        private void InitializeDefaults()
        {
            switch (origSettings.frame)
            {
                case ReferenceFrame.Cartesian: 
                    cartesian.Checked = true;
                    degrees.Enabled = false;
                    rads.Enabled = false;
                    grads.Enabled = false;
                    break;
                case ReferenceFrame.Polar:
                    polar.Checked = true;
                    degrees.Enabled = true;
                    rads.Enabled = true;
                    grads.Enabled = true;
                    break;
            }

            switch (origSettings.thetaUnits)
            {
                case ThetaUnits.ThetaDegrees:
                    degrees.Checked = true; break;
                case ThetaUnits.ThetaGradians:
                    grads.Checked = true; break;
                case ThetaUnits.ThetaRadians:
                    rads.Checked = true; break;
            }

            switch (origSettings.xThetaScale)
            {
                case Genetibase.NuGenTransform.Scale.Log:
                    xthetalog.Checked = true; break;
                case Genetibase.NuGenTransform.Scale.Linear:
                    xthetalinear.Checked = true; break;
            }

            switch (origSettings.yRScale)
            {
                case Genetibase.NuGenTransform.Scale.Log:
                    yrlog.Checked = true; break;
                case Genetibase.NuGenTransform.Scale.Linear:
                    yrlinear .Checked = true; break;
            }
        }

        private void polar_CheckedChanged(object sender, EventArgs e)
        {
            degrees.Enabled = !degrees.Enabled;
            rads.Enabled = !rads.Enabled;
            grads.Enabled = !grads.Enabled;
        }

        public ReferenceFrame Frame
        {
            get
            {
                if (polar.Checked)
                    return ReferenceFrame.Polar;
                else
                    return ReferenceFrame.Cartesian;
            }
        }

        public Scale XThetaScale
        {
            get
            {
                if (xthetalinear.Checked)
                    return Genetibase.NuGenTransform.Scale.Linear;
                else
                    return Genetibase.NuGenTransform.Scale.Log;
            }
        }

        public Scale YRScale
        {
            get
            {
                if (yrlinear.Checked)
                    return Genetibase.NuGenTransform.Scale.Linear;
                else
                    return Genetibase.NuGenTransform.Scale.Log;
            }            
        }

        public ThetaUnits Units
        {
            get
            {
                if (rads.Checked)
                    return ThetaUnits.ThetaRadians;
                else if (grads.Checked)
                    return ThetaUnits.ThetaGradians;
                else
                    return ThetaUnits.ThetaDegrees;
            }
        }
    }
}