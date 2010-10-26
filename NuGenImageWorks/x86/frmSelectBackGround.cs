using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class frmSelectBackGround : Form
    {

        MainForm frmMain = null;

        public frmSelectBackGround()
        {
            InitializeComponent();
        }

        public frmSelectBackGround(MainForm frmMain)
        {
            this.frmMain = frmMain;
            InitializeComponent();
        }

        private void pnlBgColor_Click(object sender, EventArgs e)
        {
            ColorDialog clrDlg = new ColorDialog();

            DialogResult res = clrDlg.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            this.pnlBgColor.Image = null;
            this.pnlBgColor.BackColor = clrDlg.Color;

            Extra.Enable = true;
            Extra.BackgroundColor = clrDlg.Color;

            Bitmap b = frmMain.FilterEffects(false);

            if (this.pbxPreview.Image != null)
            {
                Image prev = this.pbxPreview.Image;
                this.pbxPreview.Image = null;
                prev.Dispose();
            }

            this.pbxPreview.Image = b;
        }

        private void frmSelectBackGround_Load(object sender, EventArgs e)
        {            
            Bitmap b = frmMain.FilterEffects(false);
            this.pbxPreview.Image = b;
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
    }
}