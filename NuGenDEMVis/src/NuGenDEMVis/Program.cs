using System;
using System.Threading;
using System.Windows.Forms;
using Genetibase.NuGenDEMVis.UI;
using Genetibase.NuGenRenderCore.Settings;

namespace Genetibase.NuGenDEMVis
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

            ProgramArgs progArgs = new ProgramArgs(args);

            // Start splash if required
            bool devMode = progArgs.CheckSwitch("-dev");
            SplashDlg splash = null;
            if (!devMode || progArgs.CheckSwitch("-splash"))
            {
                Thread splashTh = new Thread(SplashProcess);
                splashTh.Start(splash = new SplashDlg());
            }

            // load config and feed splash if present
            HashTableSettings gSettings = SplashDlg.LoadConfig(splash, devMode);

            Application.Run(new MainWindow(gSettings, devMode));
        }

        static void SplashProcess(object dlg)
        {
            ((SplashDlg)dlg).ShowDialog();
        }
    }
}