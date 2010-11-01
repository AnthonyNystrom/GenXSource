using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.Network.Sockets.Protocols;
using Genetibase.Network.Web;
using System.IO;

namespace WorkSample
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();            
        }      


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (e.Argument.ToString())
            {
                case "GetServerTime":
                    {
                        HttpClient client = new HttpClient();

                        client.Port = 80;
                        client.Host = "localhost";                        
                        client.Connect();
                        try
                        {
                            string result = client.Get("http://localhost/gettime");
                            Invoke(new MethodInvoker(delegate 
                            {
                                MessageBox.Show(string.Format("Server time is {0}", result));
                            }));
                        }
                        finally
                        {
                            client.Disconnect();
                        }
                    }
                    break;

                case "GetFile":
                    {
                        HttpClient client = new HttpClient();

                        client.Port = 80;
                        client.Host = "localhost";
                        client.Connect();
                        try
                        {
                            MemoryStream mem = new MemoryStream();
                            client.Get("http://localhost/getfile?name=" + textBox1.Text, mem);
                            if (client.Response.ResponseCode == 200)
                            {
                                Invoke(new MethodInvoker(delegate
                                {
                                    Activate();
                                    saveFileDialog.Title = string.Format("Select a target file for requested file {0}", textBox1.Text);
                                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                                    {
                                        File.WriteAllBytes(saveFileDialog.FileName, mem.ToArray());
                                    }
                                }));
                            }
                            else throw new Exception("Failed to get file. Code:" + client.Response.ResponseCode.ToString());
                        }
                        finally
                        {
                            client.Disconnect();
                        }
                    }
                    break;        
            }                        
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enabled = false;
            backgroundWorker1.RunWorkerAsync("GetFile");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Enabled = false;
            backgroundWorker1.RunWorkerAsync("GetServerTime");

        }



           
      
    }
}