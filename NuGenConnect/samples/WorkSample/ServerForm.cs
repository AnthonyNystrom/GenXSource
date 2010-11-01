using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WorkSample
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            if (httpServer.Active)
            {
                httpServer.Active = false;
                btStart.Text = "Start";
            }
            else
            {
                httpServer.Active = true;
                btStart.Text = "Stop";
            }
        }

        private void httpServer_OnSessionStart(Genetibase.Network.Web.HttpSession Sender)
        {
            Log("Session started. Host:{0}\r\n", Sender.RemoteHost);
        }

        private void httpServer_OnSessionEnd(Genetibase.Network.Web.HttpSession Sender)
        {
            Log("Session ended. Host:{0}\r\n", Sender.RemoteHost);
        }

        
        private void Log(string format, params object[] args)
        {
            string s = string.Format(format, args) + Environment.NewLine;
            if (InvokeRequired)
            {
                this.Invoke(
                    new MethodInvoker(delegate { rtbConnections.AppendText(s); }));
            }
            else
                rtbConnections.AppendText(s);
        }

        private void httpServer_OnCommandGet(Genetibase.Network.Sockets.ContextRFC Sender, Genetibase.Network.Web.HttpRequestInfo ARequestInfo, Genetibase.Network.Web.HttpResponseInfo AResponseInfo, ref bool OHandled)
        {
            
            Log("GET Request. Sender:{0}. Request URI:{1}", ARequestInfo.RemoteIP, ARequestInfo.RequestURI);
            OHandled = true;

            switch (ARequestInfo.Document)
            { 
                case "/gettime":
                    AResponseInfo.ContentText = DateTime.Now.ToShortTimeString();
                    AResponseInfo.ResponseNo = 200;
                    break;

                case "/getfile":
                    if (ARequestInfo.Params.ContainsKey("name"))
                    {                        
                        string filename = "";
                        Invoke (new MethodInvoker( delegate {
                            Activate();
                            openFileDialog.Title = string.Format("Select a source file for requested file {0}", ARequestInfo.Params["name"]);
                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                filename = openFileDialog.FileName;
                            }
                        }));
                        if (filename != string.Empty)
                        {
                            //MemoryStream mem = new MemoryStream(File.ReadAllBytes(openFileDialog.FileName));
                            //using (StreamWriter sw = new StreamWriter(mem))
                            //{
                            //    sw.Write(File.ReadAllText(openFileDialog.FileName)); 
                            //}



                            AResponseInfo.ContentStream = new FileStream(filename, FileMode.Open); ;
                            //AResponseInfo.ContentStream.Position = 0;
                            AResponseInfo.ResponseNo = 200;
                        }

                    }
                    else
                    {
                        AResponseInfo.ResponseNo = 404;
                    }
                    break;

                default :
                    OHandled = false;
                    break;
            }

        }      
    }
}