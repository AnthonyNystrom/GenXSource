using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Netron.Cobalt;
using Application = Netron.Cobalt.Application;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Drawing;
using Netron.Neon.WinFormsUI;
namespace Yttrium.Whiteboard
{
    public class CobaltAddin : IAddin
    {
        private AddinInfo info;
        private ControlPanel controlPanel;
        public CobaltAddin()
        {
            info = new AddinInfo();
            info.Image="/Yttrium/images/Yttrium.gif";            
            info.Description = "Yttrium is an experimental computer algebra architecture, implementing ideas and concepts of formal hardware engineering and digital information engineering, looking at abstract math and algebra from a different, new angle.\n\n Yttrium is connected to the other new Math.NET packages like Numerics, PreciseNumerics and SignalProcessing, and thus takes direct advantage of the optimized numerical routines.";
            info.FullName = "Math.Net's Yttrium";
            info.ShortName = "Yttrium";

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        public void Load()
        {
            Application.MainForm.MainMenuStrip.Visible = true;
            Application.Tabs.Diagram.Show();
            Application.Tabs.Diagram.ChangeToolbarVisibility(DiagramMenuStrip.All, false);
            Application.WebBrowser.Navigate("root/addins/Yttrium/index.htm");
            Application.Tabs.WebBrowser.Show(Application.MainForm.DockPanel, Netron.Neon.WinFormsUI.DockState.DockLeft);
            
            //new Form2().Show(Application.MainForm.DockPanel, DockState.Float);
            //this show the control panel in floating mode
            controlPanel = new ControlPanel();
            //controlPanel.Show(Application.MainForm.DockPanel, Netron.Neon.WinFormsUI.DockState.Float);

            //this shows the control panel in a sub-level of the diagram control
            DockPane pane = new DockPane(Application.Tabs.Diagram, DockState.Document, true); 
            controlPanel.Show(pane, DockAlignment.Bottom, 0.1);

            //add a new channel especially for Yttrium, I must admire your work I guess ;-)
            Application.Output.AddChannel("Yttrium output");
            //change the color of the diagramming background
            Application.Diagram.BackColor = Color.FloralWhite;
        }


        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemname = args.Name.Split(',')[0];
           // Trace.WriteLine("Resolving: " + args.Name);
            string binPath = Path.GetDirectoryName(this.GetType().Assembly.Location) + "\\";

            Assembly ass = Assembly.LoadFile(binPath + assemname + ".dll");
            //return ass;
            return ass;
        }

        public void Unload()
        {
            Application.Tabs.Diagram.LineStyleStrip.Visible = false;

            controlPanel.Hide();
            controlPanel.Dispose();
        }


        public AddinInfo Info
        {
            get {
                return info; }
        }




        public Stream GetDocument(string address)
        {
            try
            {
                //convert to full assembly location
                address = address.Replace("/", ".");
                if (!address.StartsWith("."))
                    address = "." + address;
                address = "Yttrium.Whiteboard.docs" + address;
                address = address.Replace("..", ".");
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(address);
                return stream;
            }
            catch (Exception exc)
            { 
                Trace.WriteLine(exc.Message);
            }
            return null;
        }
    }
}
