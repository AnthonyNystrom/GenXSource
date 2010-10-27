using System;
using System.Collections.Generic;
using System.Text;

namespace Netron.Cobalt
{
    public static class Workbenches
    {
        public static void ChangeWorkbench(string benchName)
        {
            if (benchName.Equals("welcome", StringComparison.CurrentCultureIgnoreCase))
            {
                WelcomeBench();
            }
            else if (benchName.Equals("blank", StringComparison.CurrentCultureIgnoreCase))
            {
                BlankProject();
            }
            else if (benchName.Equals("yttrium", StringComparison.CurrentCultureIgnoreCase))
            { }
        }
        /// <summary>
        /// Changes the environment to the start-up/welcome bench which contains only the browser with
        /// the welcome screen. This screen allows you to choose a certain bench.
        /// </summary>
        public static void WelcomeBench()
        {
            Application.Menus.RootMenu.Visible = false;

            Application.Tabs.Diagram.Hide();
            Application.Tabs.Output.Hide();
            Application.Tabs.Property.Hide();
            Application.Tabs.Shapes.Hide();
            Application.Tabs.WebBrowser.Show();
            //Application.Tabs.Shell.Show();
            Application.WebBrowser.Navigate("doc://start/Welcome.htm");

        }

        /// <summary>
        /// Sets the environment to a generic diagramming state.
        /// </summary>
        public static void BlankProject()
        {
            Application.Menus.RootMenu.Visible = true;

            Application.Tabs.Diagram.Show();
            Application.Tabs.Output.Hide();
            //Application.Tabs.Property.Show();
            Application.Tabs.Shapes.Show();
            Application.Diagram.ShowPage = true;            
            Application.WebBrowser.Navigate("doc://Guide/");
            Application.Tabs.WebBrowser.Show(Application.MainForm.DockPanel, Netron.Neon.WinFormsUI.DockState.DockLeft); 
            //Application.Tabs.WebBrowser.Hide();
            
        }
    }
}
