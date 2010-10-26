using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestProtein
{
    public partial class Form1 : Form
    {
        private NGVChem.OutputMethod outPutDg;
        private NGVChem.GetHeaderInfoMethod getHeaderInfo;
        private NGVChem.ProgressMethod resetProgress;
        private NGVChem.ProgressMethod initProgress;
        private NGVChem.ProgressMethod endProgress;
        private NGVChem.CurrentProgressValue setProgress;
        public Form1()
        {
            InitializeComponent();
            outPutDg = new NGVChem.OutputMethod(OutputLog);
            this.piRender.SetOutPutDelegate(outPutDg);

            getHeaderInfo = new NGVChem.GetHeaderInfoMethod(OutputHeader);
            resetProgress = new NGVChem.ProgressMethod(ResetProgress);
            initProgress = new NGVChem.ProgressMethod(InitProgress);
            endProgress = new NGVChem.ProgressMethod(EndProgress);
            setProgress = new NGVChem.CurrentProgressValue(SetProgress);

            this.piRender.SetHeaderInfoDelegate(getHeaderInfo);
            this.piRender.SetInitProgressDelegate(initProgress);
            this.piRender.SetProgressDelegate(setProgress);
            this.piRender.SetResetProgressDelegate(resetProgress);
            this.piRender.SetEndProgressDelegate(endProgress);

            this.panelCombox.Controls.Add(this.piRender.GetCombox());
           

            Panel resPanel =this.piRender.GetResiduesPanel();
            this.tabPageResidues.Controls.Add(resPanel);

            this.tabPageSelect.Controls.Add(this.piRender.GetSelectPanel());
            this.tabPageTree.Controls.Add(this.piRender.GetPDBTreePanel());

            this.piRender.OnPropertyChange += new NGVChem.PropertyChangeEvent(piRender_OnPropertyChange);
        }

        void piRender_OnPropertyChange(CPIProperty propertyEntity, object propertyObj)
        {
            this.propertyGrid.SelectedObject = propertyObj;
         
        }

        private void OutputLog(String logInfo)
        {
            this.txtOutput.Text += logInfo +Environment.NewLine;
        }
        private void OutputHeader(String headerInfo)
        {
            this.txtHeader.Text = headerInfo;
        }
        private void SetProgress(int currentNumber, int totalCount)
        {
            if (currentNumber < (int)totalCount)
            {
                this.toolStripProgressBar.Value = currentNumber;
            }
            else{
                this.toolStripProgressBar.Value = (int)totalCount;
            }
        }
        private void InitProgress(int totalCount)
        {
            this.toolStripProgressBar.Value = 0;
            this.toolStripProgressBar.Maximum =(int)totalCount;
        }
        private void EndProgress(int totalCount)
        {
            this.toolStripProgressBar.Value = 100;
            this.toolStripProgressBar.Maximum = 100;
        }
        private void ResetProgress(int value)
        {
            this.toolStripProgressBar.Value = 0;
            this.toolStripProgressBar.Maximum = 100;
        }

        #region Toolbar
        private void toolStripBtnOpen_Click(object sender, EventArgs e)
        {
            //this.piRender.OpenPDB(@"E:\Project\PreteinV3\TestProtein\bin\Debug\DownloadPDB\pdb4TNA.ent");
            this.piRender.OpenPDB();
        }

        private void toolStripBtnAdd_Click(object sender, EventArgs e)
        {
            this.piRender.OnAddPdb();
        }

        private void toolStripBtnClose_Click(object sender, EventArgs e)
        {
            this.piRender.OnClosePdb();
        }

        private void toolStripBtnOpenWk_Click(object sender, EventArgs e)
        {
            this.piRender.OpenWorkSpace();
        }

        private void toolStripBtnSaveWk_Click(object sender, EventArgs e)
        {
            this.piRender.SaveWorkSpace();
        }
        #endregion

        #region  LeftToobar
        private void A_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonWireframe();
        }

        private void B_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonStick();
        }

        private void C_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonBall();
        }

        private void D_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonBallStick();
        }

        private void E_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonRibbon();
        }

        private void F_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurface();
        }

        private void G_Click(object sender, EventArgs e)
        {
            this.piRender.OnCenterMolecule();
        }

        private void H_Click(object sender, EventArgs e)
        {
            this.piRender.OnFlagMoleculeSelectionCenter();
        }

        private void I_Click(object sender, EventArgs e)
        {
            this.piRender.OnViewAll();
        }

        private void J_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnit();
        }

        private void K_Click(object sender, EventArgs e)
        {
            this.piRender.OnAttatchBiounit();
        }

        private void T0_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(0);
        }

        private void T1_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(1);
        }

        private void T2_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(2);
        }

        private void T3_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(3);
        }

        private void T4_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(4);
        }

        private void T5_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(5);
        }

        private void T6_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(6);
        }

        private void T7_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(7);
        }

        private void T8_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(8);
        }

        private void T9_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(9);
        }

        private void T10_Click(object sender, EventArgs e)
        {
            this.piRender.OnButtonDotsurfaceWithResolution(10);
        }

        private void TS_Click(object sender, EventArgs e)
        {
            this.piRender.OnSurfaceGenAlgorithmMQ();
        }

        private void TM_Click(object sender, EventArgs e)
        {
            this.piRender.OnSurfaceGenAlgorithmMSMS();
        }

        private void Wm_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitStyle(0);
        }

        private void Sm_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitStyle(1);
        }

        private void BM_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitStyle(2);
        }

        private void BSM_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitStyle(3);
        }

        private void RM_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitStyle(4);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(0);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(1);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(2);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(3);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(4);
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(5);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(6);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(7);
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(8);
        }
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            this.piRender.OnDisplayBioUnitSurface(9);
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.piRender.OnSurfaceBiounitGenAlgorithmMQ();
        }

        private void bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.piRender.OnSurfaceBiounitGenAlgorithmMSMS();
        }
        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.piRender.OnFileScreenshot();
        }

        private bool showLog = false;
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            showLog = !showLog;
            this.piRender.ShowLog(showLog);

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ///this.piRender.CleanResource();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

    }
}
