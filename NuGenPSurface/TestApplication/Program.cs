using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace TestApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (FileNotFoundException fnfe)
            {
                MessageBox.Show(String.Format("Filename:{0}\n\nSource:{1}\n\nMessage:{2}", fnfe.FileName, fnfe.Source, fnfe.Message), "FileNotFoundException");
            }
            catch (FileLoadException fle)
            {
                MessageBox.Show(String.Format("Filename:{0}\n\nSource:{1}\n\nMessage:{2}", fle.FileName, fle.Source, fle.Message), "FileLoadException");
            }
        }
    }
}