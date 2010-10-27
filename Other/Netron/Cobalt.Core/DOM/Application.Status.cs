using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Netron.Cobalt
{
    static partial class Application
    {
        public static class Status
        {
            public static StatusStrip StatusBar
            {
                get { return MainForm.StatusStrip; }
            }

            public static ToolStripProgressBar StatusProgressBar
            {
                get { return MainForm.StatusProgressBar; }
            }
            public static void ShowStatusText(string text)
            {

                MainForm.InfoStatusLabel.Text = text;
                MainForm.InfoStatusLabel.Visible = true;
                MainForm.StatusTimer.Start();
            }

           
        }
    }
}
