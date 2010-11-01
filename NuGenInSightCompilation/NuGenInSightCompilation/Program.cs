using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Dile.Disassemble;
using Dile;
using System.Threading;

namespace NuGenInSightCompilation
{
    static class Program
    {

        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Dile.Metadata.NuGenOpCodeGroups.Initialize();
            Dile.Controls.NuGenILEditorControl.Initialize();
            Application.EnableVisualStyles();

            NuGenInSightMainForm mainForm = new NuGenInSightMainForm();

#if RELEASE
            try
            {
#endif
                 Application.Run(mainForm);
#if RELEASE
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.Write("This is being caught at the Program.cs main method:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
#endif
        }
    }
}