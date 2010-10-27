using System;
using Netron.Xeon;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace Netron.Cobalt.IDE
{
   
    /// <summary>
    /// The startup class holding the <see cref="STAThread"/>
    /// </summary>
    public static class Startup
    {
        private static HttpWebServer httpServer;

        internal static HttpWebServer HttpServer
        {
            get { return httpServer; }
            set { httpServer = value; }
        }


      /// <summary>
      /// The main entry point for the application.
      /// </summary>
        [STAThread]
        static void Main()
        {
            ///launch the Http server if necessary
            LaunchHttpServer();    
            //create a sink for unhandled exceptions
            System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //Using the enablevisualstyles method XP visual styles are applied to your winforms when the application is running on Windows XP.
            System.Windows.Forms.Application.EnableVisualStyles();
            //Setting the property to false will make the control use GDI for drawing text
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            //Launch the messaging loop
            System.Windows.Forms.Application.Run(new myform());
            //Application.Run(new Form1());
            //shut down the Http server if necessary
            ShutdownHttpServer();


        }

        /// <summary>
        /// Shutdowns the HTTP server.
        /// </summary>
        private static void ShutdownHttpServer()
        {
            try
            {
                if (httpServer != null)
                {
                    httpServer.Stop();
                    Trace.WriteLine("The internal HTTP listening on port " + Properties.Settings.Default.HttpPort.ToString() + " server was stopped.");
                }
            }
            catch (Exception exc)
            {

                Trace.WriteLine(exc.Message);
            }
        }

        /// <summary>
        /// Launches the HTTP server.
        /// </summary>
        private static void LaunchHttpServer()
        {
            try
            {
                httpServer = new HttpWebServer();
                //Properties.Settings sets = new Properties.Settings();
                Trace.WriteLine(Properties.Settings.Default.BinarySerializationSwitch);
                httpServer.Root = new DriveDirectory(Directory.GetParent(System.Windows.Forms.Application.ExecutablePath.ToString()).ToString() + Properties.Settings.Default.HttpDirectory);
                httpServer.Port = Properties.Settings.Default.HttpPort;
                // Start the server
                if (Properties.Settings.Default.HttpEnabled)
                    httpServer.Start();

                httpServer.RequestReceived += new HttpServer.RequestEventHandler(httpServer_RequestReceived);
                //httpServer.ValidRequestReceived += new HttpServer.RequestEventHandler(httpServer_ValidRequestReceived);
            }
            catch (Exception iexc)
            {
                Properties.Settings.Default.HttpDirectory = "/HTTP";
                Properties.Settings.Default.Save();
            }
        }

        
        /// <summary>
        /// This intercepts a request to the server and acts as an HTTP filter.
        /// If the uri contains the string '/addins/images/' it is an image comming from the addin assembly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void httpServer_RequestReceived(object sender, RequestEventArgs e)
        {
            string uri = e.Request.Uri.ToString();
            if (uri.ToLower().IndexOf("/addins/") > 0)
            {
                try
                {
                    string rest = uri.Substring(uri.ToLower().IndexOf("/addins/") + 8);
                                                   
                    string addinname = rest.Substring(0, rest.IndexOf("/"));
                    string document = rest.Substring(rest.IndexOf("/"));
                    int index = Application.Addins.FindAddinIndex(addinname);
                    Stream stream = null;
                    if (index >= 0)
                    {
                        stream = Application.Addins.Extensions[index].GetDocument(document);
                        if (stream == null)
                            e.Request.Response.ResponseCode = "404";                            
                        else
                            e.Request.Response.ResponseContent = stream;
                    }
                    else
                        return;
                }
                catch { }
            }
            // Trace.WriteLine(e.Request.Uri.ToString());
            
        }

        /// <summary>
        /// Handles the ThreadException event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Threading.ThreadExceptionEventArgs"/> instance containing the event data.</param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //I have used a logging service here, but feel free to do whatever you prefer.
            //BugLogging.AddBug(e.Exception.Source, e.Exception.Message);
        }
    }

    public class myform : MainForm
    {
        public myform()
        {
            Application.WebBrowser.ObjectForScripting = new BrowserAddin(this);
        }
    }
}
