using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class ContentAlignmentCtrl : UserControl
    {

        private ContentAlignment ca;

        public ContentAlignment ContentAlignment
        {
            get { return ca; }
            set 
            { 
                ca = value;
                setCA();
                this.Invalidate(true);
            }
        }

        private void setCA()
        {
            this.lblSelected.Text = ca.ToString();
            unpressAll();

            switch (ca)
            {
                case ContentAlignment.TopLeft:
                    tl.IsPressed = true;
                    break;

                case ContentAlignment.TopCenter:
                    tc.IsPressed = true;
                    break;

                case ContentAlignment.TopRight:
                    tr.IsPressed = true;
                    break;

                case ContentAlignment.MiddleLeft:
                    ml.IsPressed = true;
                    break;

                case ContentAlignment.MiddleCenter:
                    mc.IsPressed = true;
                    break;

                case ContentAlignment.MiddleRight:
                    mr.IsPressed = true;
                    break;

                case ContentAlignment.BottomLeft:
                    bl.IsPressed = true;
                    break;

                case ContentAlignment.BottomCenter:
                    bc.IsPressed = true;
                    break;

                case ContentAlignment.BottomRight:
                    br.IsPressed = true;
                    break;
            }
        }

        public ContentAlignmentCtrl()
        {
            InitializeComponent();
        }

        private void tl_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.TopLeft;            
        }

        private void tc_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.TopCenter;
         
        }

        private void tr_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.TopRight;
         
        }

        private void ml_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.MiddleLeft;
         
        }

        private void mc_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.MiddleCenter;
         
        }

        private void mr_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.MiddleRight;
         
        }
        private void br_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.BottomRight;
         
        }

        private void bc_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.BottomCenter;
         
        }

        private void bl_Click(object sender, EventArgs e)
        {
            this.ContentAlignment = ContentAlignment.BottomLeft;
         
        }

        private void unpressAll()
        {
            tl.IsPressed = false;
            tc.IsPressed = false;
            tr.IsPressed = false;
            ml.IsPressed = false;
            mc.IsPressed = false;
            mr.IsPressed = false;
            bl.IsPressed = false;
            bc.IsPressed = false;
            br.IsPressed = false;
        }
    }
}
