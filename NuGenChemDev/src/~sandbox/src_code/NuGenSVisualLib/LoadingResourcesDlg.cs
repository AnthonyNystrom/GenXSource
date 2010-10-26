using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace NuGenSVisualLib
{
    public partial class LoadingResourcesDlg : Form
    {
        Thread progressProcess;

        Delegate updateProgress;
        Delegate closeDialog;

        public LoadingResourcesDlg()
        {
            InitializeComponent();

            updateProgress = new EventHandler(this.UpdateProgress);
            closeDialog = new EventHandler(this.CloseDialog);
        }

        void UpdateProgress(object sender, EventArgs e)
        {
            progressBar1.Value = (int)sender;
        }

        void CloseDialog(object sender, EventArgs e)
        {
            this.Close();
        }

        public int Progress
        {
            get
            {
                return progressBar1.Value;
            }
            set
            {
                this.BeginInvoke(updateProgress, new object[] { value });
            }
        }

        public Thread ProgressProcess
        {
            get
            {
                return progressProcess;
            }
            set
            {
                progressProcess = value;
            }
        }

        private void LoadingResourcesDlg_Load(object sender, EventArgs e)
        {
            // start process thread
            progressProcess.Start(this);
        }

        internal void TryClose()
        {
            this.BeginInvoke(closeDialog);
        }
    }
}