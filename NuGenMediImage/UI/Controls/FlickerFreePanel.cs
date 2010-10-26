using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public partial class FlickerFreePanel : System.Windows.Forms.Panel
    {
        public FlickerFreePanel()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
