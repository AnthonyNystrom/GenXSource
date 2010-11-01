using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Genetibase.Network.Web;

namespace WorkSample
{

static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new ServerForm().Show();
            Application.Run(new ClientForm());            
        }


    }
}