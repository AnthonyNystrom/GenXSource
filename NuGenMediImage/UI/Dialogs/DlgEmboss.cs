using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Dialogs
{
    public partial class DlgEmboss : Form
    {
        private NuGenMediImageCtrl ngMediImage;

        public Image OriginalImage
        {
            set
            {
                this.pOrig.Image = value;
                this.pProc.Image = (Image)this.pOrig.Image.Clone();
            }
        }

        public int EmbossValue
        {
            get
            {
                return this.trkEmboss.Value;
            }
            set
            {
                this.trkEmboss.Value = value;
                this.trkEmboss_ValueChanged(null, null);
            }
        }

        public DlgEmboss(NuGenMediImageCtrl ngMediImage)
        {
            this.ngMediImage = ngMediImage;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void trkEmboss_ValueChanged(object sender, EventArgs e)
        {
            if (trkEmboss.Value > 0)
            {
                Bitmap b = Filters.Emboss((Bitmap)this.pOrig.Image.Clone(), this.trkEmboss.Value);
                Image old = pProc.Image;
                pProc.Image = b;
                old.Dispose();
                old = null;
            }
            else
            {
                Bitmap b = (Bitmap)this.pOrig.Image.Clone();
                Image old = pProc.Image;
                pProc.Image = b;
                old.Dispose();
                old = null;
            }
        }
    }
}