using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VideoEncoder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //TurbineEncode t = new TurbineEncode();
            ThumbnailEncoder e = new ThumbnailEncoder();
        }
    }
}
