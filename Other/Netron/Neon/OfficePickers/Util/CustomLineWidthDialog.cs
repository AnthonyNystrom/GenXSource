using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Netron.Neon.OfficePickers
{
    class CustomLineWidthDialog : CommonDialog
    {

        private float mLineWidth = 1F;

        public float LineWidth
        {
            get { return mLineWidth; }
            set { mLineWidth = value; }
        }

        public override void Reset()
        {
            MessageBox.Show("As to be reset?.");
        }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            LineWeightForm frm = new LineWeightForm();            
            frm.ShowDialog();
            
            mLineWidth = frm.LineWeight;

            return true;
        }
    }
}
