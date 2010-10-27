using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Netron.Diagramming.Core;
namespace Netron.Cobalt.IDE
{
    /// <summary>
    /// This is the class passed to the WebBrowser.ObjectForScripting property, which can then be accessed via JavaScript.
    /// </summary>
    [ComVisible(true)]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class BrowserAddin
    {

        public event EventHandler OnRunScript;

        #region Properties

        /// <summary>
        /// the MainForm field
        /// </summary>
        private MainForm mMainForm;
        /// <summary>
        /// Gets or sets the MainForm
        /// </summary>
        public MainForm MainForm
        {
            get { return mMainForm; }
            set { mMainForm = value; }
        }

        #endregion

        #region Constructor
        ///<summary>
        ///Default constructor
        ///</summary>
        public BrowserAddin(MainForm mainForm)
        {
            this.mMainForm = mainForm;
        }
        #endregion

        #region Methods

        private void RaiseOnRunScript(string data)
        { 
            if(OnRunScript!=null)
                OnRunScript(this, EventArgs.Empty);
        }
        public void Test(string message)
        {
            MessageBox.Show(message);
            
        }

        /// <summary>
        /// This is the main method to execute scripts from an embedded sample in a webpage
        /// </summary>
        /// <param name="data"></param>
        public void RunScript(string data)
        {
            RaiseOnRunScript(data);
            ScriptExecutor exec = new ScriptExecutor(this.mMainForm);
            exec.OnRunOutput += new EventHandler<StringEventArgs>(exec_OnRunOutput);
            exec.ExecuteCode(data);
        }

        void exec_OnRunOutput(object sender, StringEventArgs e)
        {
            Application.Output.WriteLine("Exception", e.Data);
            Trace.WriteLine(e.Data);            
        }

        public void ChangeWorkbench(string benchName)
        {
            Workbenches.ChangeWorkbench(benchName);
        }

        public string FetchAddins()
        {

            if (Application.Addins.Extensions.Count == 0)
                return string.Empty;
            else
            {
                //string[] addins = new string[Application.Addins.Extensions.Count];
                StringBuilder sb = new StringBuilder();
                foreach (IAddin addin in Application.Addins.Extensions)
                {
                   // addins[i] = addin.Info.Name;
                    sb.Append(addin.Info.FullName);
                    sb.Append("|");
                }
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
        }

        public void LoadAddin(int index)
        {
            Application.Addins.Extensions[index].Load();
        }

        public string FetchImage(int index)
        { 
            return "http://localhost:" + Properties.Settings.Default.HttpPort.ToString() + "/Addins/images/" + index;
            
        }
        public string FetchDescription(int index)
        {
            return Application.Addins.Extensions[index].Info.Description;
        }
        public string FetchImpression(int index)
        {
            string url = Application.Addins.Extensions[index].Info.Image;
            if (!url.StartsWith("/"))
                url = "/" + url;
            return url;
        }
        #endregion

    }
}
