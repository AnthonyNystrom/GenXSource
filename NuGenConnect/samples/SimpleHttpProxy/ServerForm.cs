using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.Network.Sockets;

namespace SimpleHttpProxy
{
     class MyTextWriter : System.IO.TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        public override void Write(char value)
        {
            Logger.LogChar(value);
        }
    }


public partial class ServerForm : Form, ISettings
    {
        public ServerForm()
        {
            InitializeComponent();

            httpServer.RequestHandlers.Add(new GetRequestHandler(this));

            Logger.OnLogMessage += new Logger.LogEventHandler(Logger_OnLogMessage);
            Logger.OnLogChar += new Logger.LogCharEventHandler(Logger_OnLogChar);
        }

    void Logger_OnLogChar(char value)
    {       
        //if (InvokeRequired)
        //{
        //    Invoke(new MethodInvoker(delegate { rtbLog.AppendText(value.ToString()); }));
        //}
        //else
        //    rtbLog.AppendText(value.ToString());
    }

        void Logger_OnLogMessage(string format, params object[] args)
        {
            string s = string.Format(format, args) + Environment.NewLine;

            if (InvokeRequired)
            { 
                Invoke(new MethodInvoker(delegate {rtbLog.AppendText(s);}));
            }
            else 
                rtbLog.AppendText(s);
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

                TextWriterServerIntercept si = new TextWriterServerIntercept();
                si.TextWriter = new MyTextWriter();

                httpServer.ServerSocket.Intercept = si;


                btStart.Text = "Stop";
            }
        }

        private void btHelp_Click(object sender, EventArgs e)
        {
            using (DocForm form = new DocForm())
            {
                form.ShowDialog();
            }
        }

        #region ISettings Members

        public List<string> BannedSites
        {
            get
            {
                List<string> result = new List<string>();

                Invoke(new MethodInvoker(delegate
                {
                    foreach (string s in rtbBannedSites.Text.Split(new string[] { ";", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (s.Trim() == string.Empty) continue;
                        result.Add(s.Trim());
                    }                    
                }));
                return result;
            }            
        }

        #endregion
    }
}