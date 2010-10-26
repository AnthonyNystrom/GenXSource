using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.NuGenImageWorks;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class frmWaterMark : Form
    {

        public Font waterMarkFont = SystemFonts.DefaultFont;
        public ContentAlignment textAlign = ContentAlignment.BottomCenter;
        public ContentAlignment imageAlign = ContentAlignment.TopCenter;
        public Image waterMarkImage = null;
        public String waterMarkText = null;

        public frmWaterMark()
        {
            InitializeComponent();
        }

        private void btnWMFont_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            dlg.Font = this.waterMarkFont;

            DialogResult res = dlg.ShowDialog();

            if (res != DialogResult.OK)
                return;            

            this.waterMarkFont = dlg.Font;
        }

        private void btnWMTAlign_Click(object sender, EventArgs e)
        {
            frmCA objCA = new frmCA();
            objCA.ContentAlignment = this.textAlign;

            DialogResult res = objCA.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            this.textAlign = objCA.ContentAlignment;
        }

        private void btnWMIAlign_Click(object sender, EventArgs e)
        {
            frmCA objCA = new frmCA();
            objCA.ContentAlignment = this.textAlign;

            DialogResult res = objCA.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            this.imageAlign = objCA.ContentAlignment;
        }

        private void ribbonButton8_Click(object sender, EventArgs e)
        {
            if (this.waterMarkImage != null)
                this.waterMarkImage.Dispose();

            this.waterMarkImage = null;
        }

        private void ribbonButton7_Click(object sender, EventArgs e)
        {
            DialogResult res = this.openFileDialog1.ShowDialog();

            if (res != DialogResult.OK)
                return;

            this.waterMarkImage = new Bitmap(this.openFileDialog1.FileName);
        }

        private void ribbonButtonOp5Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ribbonButtonOp5OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ribbonTextOp1_TextChanged(object sender, EventArgs e)
        {
            this.waterMarkText = ribbonTextOp1.Text;
        }
    }
}