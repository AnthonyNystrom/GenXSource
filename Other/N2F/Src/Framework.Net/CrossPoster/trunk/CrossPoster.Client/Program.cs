using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Next2Friends.CrossPoster.Client.LiveJournal;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var ljClient = XmlRpcProxyGen.Create<ILiveJournalClient>();            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
