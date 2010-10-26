using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Chem;
using System.Threading;
using NuGenSVisualLib.Logging;

namespace ChemDevEnv
{
    public partial class LoadingProgressDlg : Form
    {
        IChemFileWrapper loadedChemFile;
        Thread loadingThread;
        string filename;

        delegate void GenericDelegate();
        Delegate closeDelegate;
        Delegate updateDelegate;
        Delegate updateProcessDelegate;

        LoadingProgress progress;

        public LoadingProgressDlg(string filename)
        {
            InitializeComponent();

            this.filename = filename;
            closeDelegate = new GenericDelegate(CloseDlg);
            updateDelegate = new GenericDelegate(UpdateProgress);
            updateProcessDelegate = new EventHandler(UpdateProcess);
        }

        public IChemFileWrapper LoadedChemFile
        {
            get { return loadedChemFile; }
        }

        private void CloseDlg()
        {
            this.Close();
        }

        private void UpdateProgress()
        {
            uiProgressBar1.Value = progress.ProgressPercent;
            label3.Text = progress.CurrentProcessName;
        }

        private void UpdateProcess(object sender, EventArgs e)
        {
            uiProgressBar2.Value = ((IProcessLoadingProgress)sender).ProgressPercent;
        }

        private void LoadingProgressDlg_Load(object sender, EventArgs e)
        {
            // start loading thread
            loadingThread = new Thread(this.LoadingProcess);
            loadingThread.Start();
        }

        private void LoadingProcess()
        {
            progress = new LoadingProgress(3);
            progress.OnUpdate += new LoadingProgress.ProgressUpdateHandler(progress_OnUpdate);
            progress.OnProcessUpdate += new LoadingProgress.ProcessUpdateHandler(progress_OnProcessUpdate);
            loadedChemFile = MoleculeLoader.LoadFromFile(filename, progress);

            // close dlg
            this.Invoke(closeDelegate);
        }

        void progress_OnProcessUpdate(IProcessLoadingProgress process)
        {
            this.Invoke(updateProcessDelegate, process, null);
        }

        void progress_OnUpdate(LoadingProgress progress)
        {
            this.Invoke(updateDelegate);
        }
    }
}