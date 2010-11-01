using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class DiscretizeSettingsDialog : Form
    {
        private DiscretizeSettings settings;
        private Image originalImage;

        private ProgressDialog progressDialog;

        public DiscretizeSettingsDialog(DiscretizeSettings settings, Image originalImage)
        {
            this.settings = settings;
            this.originalImage = originalImage;

            InitializeComponent();

            switch (settings.discretizeMethod)
            {
                case DiscretizeMethod.DiscretizeForeground:
                    radioButton2.Checked = true; break;
                case DiscretizeMethod.DiscretizeIntensity:
                    radioButton1.Checked = true; break;
                case DiscretizeMethod.DiscretizeHue:
                    radioButton3.Checked = true; break;
                case DiscretizeMethod.DiscretizeSaturation:
                    radioButton4.Checked = true; break;
                case DiscretizeMethod.DiscretizeValue:
                    radioButton5.Checked = true; break;
            }

            ValueChanged(true);

            this.MaximumSize = Size;
        }

        public DiscretizeSettings DiscretizeSettings
        {
            get
            {
                return settings;
            }
        }

        public void ProgressUpdated(double progress)
        {
            if (progressDialog == null)
            {
                progressDialog = new ProgressDialog("Histogram Progress", "Generating Histograms");
                progressDialog.Show();
            }

            if (progress == 1.0)
            {
                progressDialog.Close();
                progressDialog = null;
                return;
            }

            progressDialog.Progress = progress;            
        }
    }
}