using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NuGenSVisualLib;

namespace ChemDevEnv
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Splash splash = new Splash(args[0] == "-dev");
            Application.Run(splash);
            Application.Run(new DevEnvIDEWindow(splash.globalSettings));
        }
    }
}