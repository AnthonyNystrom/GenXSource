using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Genetibase.NuGenTransform
{
    public partial class GridRemovalSettingsDialog : Form
    {
        private GridRemovalSettings settings;
        private DiscretizeSettings discretizeSettings;
        private CoordSettings coordSettings;
        private NuGenScreenTranslate transform;
        private Color bgColor;
        private Image originalImage;

        private GridMeshSettings gridRemovalMesh;

        private ProgressDialog dlg;

        public GridRemovalSettingsDialog(NuGenDocument doc)
        {
            this.settings = doc.GridRemovalSettings;
            this.originalImage = doc.OriginalImage;
            this.discretizeSettings = doc.DiscretizeSettings;
            this.transform = doc.Transform;
            this.bgColor = doc.BackgroundColor;
            this.coordSettings = doc.CoordSettings;

            if (doc.ValidAxes)
                this.gridRemovalMesh = doc.GridDisplaySettings;
            else
                this.gridRemovalMesh.initialized = false;

            discretizeSettings.discretizeMethod = DiscretizeMethod.DiscretizeForeground;

            InitializeComponent();
            InitializeDefaults();

            if (!(doc.ValidAxes || doc.ValidScale))
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                checkBox2.Enabled = false;
                checkBox2.Checked = false;
                checkBox3.Enabled = false;
                checkBox3.Checked = false;
            }

            histogram.ValueChanged = this.ValueChanged;

            ValueChanged(true);

            this.MaximumSize = Size;
        }

        private void InitializeDefaults()
        {
            textBox1.Text = settings.thinThickness.ToString();
            textBox2.Text = settings.gridDistance.ToString();
            textBox3.Text = settings.gapSeparation.ToString();

            checkBox1.Checked = settings.removeColor;
            checkBox2.Checked = settings.removeGridlines;
            checkBox3.Checked = settings.removeThinLines;
        }

        public GridRemovalSettings GridRemovalSettings
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

        Thread discretizeThread = null;

        void ValueChanged(bool ignoreThread)
        {
            discretizeSettings = histogram.Settings;
            settings.foregroundThresholdLow = histogram.Settings.foregroundThresholdLow;
            settings.foregroundThresholdHigh = histogram.Settings.foregroundThresholdHigh;

            if (ignoreThread && discretizeThread != null)
            {
                discretizeThread.Abort();
                discretizeThread = null;
            }

            if (discretizeThread == null)
            {
                discretizeThread = new Thread(new ThreadStart(this.DiscretizeGo));
                discretizeThread.Start();
            }

        }

        private void DiscretizeGo()
        {            
            NuGenDiscretize discretize = new NuGenDiscretize(originalImage.Clone() as Image, discretizeSettings);
            NuGenGridRemoval gridRemoval = new NuGenGridRemoval(originalImage, discretize);

            gridRemoval.RemoveAndConnect(transform, coordSettings, settings, bgColor);

            Image img = gridRemoval.GetImage();

            previewPanel.BackgroundImage = img;
            previewPanel.BackgroundImageLayout = ImageLayout.Stretch;
            Refresh();

            discretizeThread = null;
        }


        public void ProgressUpdated(double progress)
        {
            if (dlg == null)
            {
                dlg = new ProgressDialog("Histogram Progress", "Generating Histograms");
                dlg.Show();
            }

            if(progress == 1.0) 
            {
                dlg.Close();
                dlg = null;
                return;
            }            

            dlg.Progress = progress;
        }
    }
}