namespace FacScan
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class MainForm : dataForm
    {
        public MainForm()
        {
            this.tf = new TransFactor();
            this.singleText = "";
            this.prgBarMax = 0;
            this.prgValue = 0;
            this.caltotal = 0;
            this.caldone = 0;
            this.runSrchSngl = false;
            this.runSrchComm = false;
            this.running = "";
            this.filename = "";
            this.InitializeComponent();
            this.initFormControls();
            this.tf.creatDataTables();
            this.timer1.Enabled = false;
            this.fOption = new MainOptionForm();
            if (this.fOption.ckbShowWizard.Checked)
            {
                this.mnuStartWizard_Click(this, null);
            }
        }

        private void dgCount_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string text1 = this.dgCount[this.dgCount.CurrentCell.RowNumber, 0].ToString();
                for (int num1 = 0; num1 < this.tf.CommonFactorPosition.Rows.Count; num1++)
                {
                    this.dataGrid1.UnSelect(num1);
                    if (this.dataGrid1[num1, 2].ToString() == text1)
                    {
                        this.dataGrid1.Select(num1);
                    }
                }
                if (text1.Substring(0, 1) == "R")
                {
                    Process.Start("http://www.gene-regulation.com/cgi-bin/pub/databases/transfac/getTF.cgi?AC=" + text1);
                }
                if (text1.Substring(0, 1) == "S")
                {
                    DataRow row1 = this.tf.Factor.Rows.Find(text1);
                    if (row1 != null)
                    {
                        string text2 = row1["cite"].ToString();
                        string text3 = text2.Substring(0, text2.IndexOf(":"));
                        string text4 = text3.Substring(0, text3.LastIndexOf(" ")).Trim();
                        string text5 = text3.Substring(text3.LastIndexOf(" ")).Trim();
                        text2 = text2.Substring(text2.IndexOf(":") + 1);
                        string text6 = text2.Substring(0, text2.IndexOf("-")).Trim();
                        string text7 = text2.Substring(text2.IndexOf("(") + 1, 4);
                        Process.Start("http://www.ncbi.nlm.nih.gov/entrez/query.fcgi?CMD=Search&DB=pubmed&term=" + (text4 + "[jo] " + text5 + "[vi] " + text6 + "[pg] " + text7 + "[dp]"));
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Show Selected Items Error: \r\nCheck the Citation column." + exception1.Message, "Error");
            }
        }

        private string display(ArrayList al, DataTable dtRes)
        {
            int num1 = 0x12;
            string text1 = "-" + num1.ToString().Trim();
            string text2 = "{0," + text1 + "}{1," + text1 + "}{2," + text1 + "}{3," + text1 + "}{4," + text1 + "}";
            string text3 = "{0," + text1 + "}";
            string text4 = "";
            try
            {
                text4 = "Sites:\r\n\r\n";
                foreach (string text5 in al)
                {
                    text4 = text4 + text5 + "\r\n";
                    text4 = text4 + this.tf.dup("-", num1 * 5) + "--\r\n";
                    DataRow row1 = this.tf.Factor.Rows.Find(text5);
                    for (int num2 = 1; num2 < this.tf.Factor.Columns.Count; num2++)
                    {
                        text4 = text4 + string.Format("{0}:\t{1}\r\n", this.tf.Factor.Columns[num2].ColumnName.ToString(), row1[num2]);
                    }
                    text4 = text4 + "\r\n";
                }
                text4 = text4 + "\r\n" + this.tf.dup("*", num1 * 5) + "**\r\n";
                text4 = text4 + "\r\nPositions: " + dtRes.Rows.Count;
                text4 = text4 + "\r\n" + this.tf.dup("-", num1 * 5) + "--\r\n";
                text4 = text4 + string.Format(text2, new object[] { "Gene", "Position", "Accession", "Pattern", "Site Name" }) + "\r\n";
                text4 = text4 + this.tf.dup(this.tf.dup("-", num1 - 3) + "   ", 5) + "\r\n";
                for (int num3 = 0; num3 < dtRes.Rows.Count; num3++)
                {
                    string text6 = dtRes.Rows[num3]["acce"].ToString();
                    DataRow row2 = this.tf.Factor.Rows.Find(text6);
                    text4 = text4 + string.Format(text3, dtRes.Rows[num3]["gene"]);
                    text4 = text4 + string.Format(text3, dtRes.Rows[num3]["pos"]);
                    text4 = text4 + string.Format(text3, row2["acce"]);
                    text4 = text4 + string.Format(text3, row2["patt"]);
                    text4 = text4 + string.Format(text3, row2["site"]);
                    text4 = text4 + "\r\n";
                }
                text4 = text4 + "\r\n" + this.tf.dup("=", num1 * 5) + "==\r\n";
                text4 = text4 + "\r\n\r\n";
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Display Text Error: " + exception1.Message, "Error");
            }
            return text4;
        }

        private string display(int p, DataTable dtRes)
        {
            int num1 = 0x12;
            string text1 = "-" + num1.ToString().Trim();
            string text2 = "{0," + text1 + "}{1," + text1 + "}{2," + text1 + "}{3," + text1 + "}{4," + text1 + "}";
            string text3 = "{0," + text1 + "}";
            string text4 = "";
            try
            {
                text4 = text4 + "Gene:\t" + this.tf.Sequence.Rows[p]["name"].ToString() + "\r\n";
                object obj1 = text4;
                text4 = string.Concat(new object[] { obj1, "Length:\t", this.tf.Sequence.Rows[p]["seq"].ToString().Length, " base pairs\r\n" });
                if (this.fOption.ckbShowSeq.Checked)
                {
                    text4 = text4 + this.tf.Sequence.Rows[p]["seq"] + "\r\n";
                }
                text4 = text4 + "\r\nPositions: " + dtRes.Rows.Count;
                text4 = text4 + "\r\n" + this.tf.dup("-", num1 * 5) + "--\r\n";
                text4 = text4 + string.Format(text2, new object[] { "Acce", "Pattern", "Position", "Site", "Factor" }) + "\r\n";
                text4 = text4 + this.tf.dup(this.tf.dup("-", num1 - 3) + "   ", 5) + "\r\n";
                int num2 = 0;
                for (int num3 = 0; num3 < dtRes.Rows.Count; num3++)
                {
                    string text5 = dtRes.Rows[num3]["acce"].ToString();
                    DataRow row1 = this.tf.Factor.Rows.Find(text5);
                    text4 = text4 + string.Format(text3, row1["acce"]);
                    text4 = text4 + string.Format(text3, row1["patt"]);
                    num2++;
                    text4 = text4 + string.Format(text3, dtRes.Rows[num2]["pos"]);
                    text4 = text4 + string.Format(text3, row1["site"]);
                    text4 = text4 + string.Format(text3, row1["fact"]);
                    text4 = text4 + "\r\n";
                }
                text4 = text4 + "\r\n" + this.tf.dup("*", num1 * 5) + "**\r\n";
                text4 = text4 + "\r\n\r\n";
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Display Text Error: " + exception1.Message, "Error");
            }
            return text4;
        }

        private void displayThread()
        {
            try
            {
                for (int num1 = 0; num1 < this.tf.Sequence.Rows.Count; num1++)
                {
                    DataTable table1 = this.tf.FactorPosition.Clone();
                    for (int num2 = 0; num2 < this.tf.FactorPosition.Rows.Count; num2++)
                    {
                        if (this.tf.Sequence.Rows[num1]["name"].ToString() == this.tf.FactorPosition.Rows[num2]["gene"].ToString())
                        {
                            table1.Rows.Add(this.tf.FactorPosition.Rows[num2].ItemArray);
                        }
                    }
                    this.singleText = this.singleText + this.display(num1, table1);
                    this.prgValue++;
                }
                this.running = "display done";
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Display Thread Error: " + exception1.Message, "Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void init(WizardForm f)
        {
            try
            {
                string text1 = f.txbFactFile.Text;
                this.dv = new DataView();
                this.dt = new DataTable();
                this.readdb(text1);
                if (f.rbnUseDB.Checked)
                {
                    this.mnuSelGene_Click(this, null);
                }
                else
                {
                    if (f.rbnSeq.Checked)
                    {
                        this.readSeqText(f.txbSeq.Text);
                    }
                    if (f.rbnSeqFile.Checked)
                    {
                        this.tf.Sequence = this.readSeqFile(f.txbSeqFile.Text);
                    }
                }
                this.status();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Initialization Error: " + exception1.Message, "Error");
            }
        }

        private void initFormControls()
        {
            base.StartPosition = FormStartPosition.CenterScreen;
            this.textBox1.Font = new Font("Courier New", 9f);
            this.textBox1.Multiline = true;
            this.textBox1.Dock = DockStyle.Fill;
            this.textBox1.ScrollBars = ScrollBars.Both;
            this.textBox1.Text = "";
            base.formTitle = "FactScan";
            base.showCaption("New Sequence");
        }

        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.sbpFact = new System.Windows.Forms.StatusBarPanel();
			this.sbpGene = new System.Windows.Forms.StatusBarPanel();
			this.sbpSingle = new System.Windows.Forms.StatusBarPanel();
			this.sbpCommon = new System.Windows.Forms.StatusBarPanel();
			this.sbpComFac = new System.Windows.Forms.StatusBarPanel();
			this.sbpTime = new System.Windows.Forms.StatusBarPanel();
			this.sbpEmpty = new System.Windows.Forms.StatusBarPanel();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuNew = new System.Windows.Forms.MenuItem();
			this.mnuSaveProject = new System.Windows.Forms.MenuItem();
			this.mnuLoadProject = new System.Windows.Forms.MenuItem();
			this.mnuCancelLoadPrj = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.mnuSave = new System.Windows.Forms.MenuItem();
			this.mnuSaveAs = new System.Windows.Forms.MenuItem();
			this.numSaveCommon = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.mnuFileLoadSigScan = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuLoadFactor = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.menuItem24 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.mnuGeneNewSeq = new System.Windows.Forms.MenuItem();
			this.mnuLoadGenes = new System.Windows.Forms.MenuItem();
			this.mnuSelGene = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.mnuSrchSingle = new System.Windows.Forms.MenuItem();
			this.mnuAbort = new System.Windows.Forms.MenuItem();
			this.mnuDisplayCurrent = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuSrchCommon = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuShowImage = new System.Windows.Forms.MenuItem();
			this.mnuShowTextImg = new System.Windows.Forms.MenuItem();
			this.mnuOption = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.mnuStartWizard = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.ofDlg = new System.Windows.Forms.OpenFileDialog();
			this.tabControl2 = new System.Windows.Forms.TabControl();
			this.tpgFact = new System.Windows.Forms.TabPage();
			this.lbxFact = new System.Windows.Forms.ListBox();
			this.tpgGene = new System.Windows.Forms.TabPage();
			this.lbxGene = new System.Windows.Forms.ListBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.dgCount = new System.Windows.Forms.DataGrid();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.sfDlg = new System.Windows.Forms.SaveFileDialog();
			this.prgBar = new System.Windows.Forms.ProgressBar();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.sbpFact)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpGene)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpSingle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpCommon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpComFac)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpEmpty)).BeginInit();
			this.tabControl2.SuspendLayout();
			this.tpgFact.SuspendLayout();
			this.tpgGene.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// toolBar1
			// 
			this.toolBar1.ButtonSize = new System.Drawing.Size(39, 20);
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(944, 26);
			this.toolBar1.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 427);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbpFact,
            this.sbpGene,
            this.sbpSingle,
            this.sbpCommon,
            this.sbpComFac,
            this.sbpTime,
            this.sbpEmpty});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(944, 22);
			this.statusBar1.TabIndex = 2;
			// 
			// sbpFact
			// 
			this.sbpFact.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpFact.MinWidth = 100;
			this.sbpFact.Name = "sbpFact";
			this.sbpFact.Text = "Sites: ";
			// 
			// sbpGene
			// 
			this.sbpGene.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpGene.MinWidth = 100;
			this.sbpGene.Name = "sbpGene";
			this.sbpGene.Text = "Gene: ";
			// 
			// sbpSingle
			// 
			this.sbpSingle.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpSingle.MinWidth = 100;
			this.sbpSingle.Name = "sbpSingle";
			this.sbpSingle.Text = "Single: ";
			// 
			// sbpCommon
			// 
			this.sbpCommon.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpCommon.MinWidth = 100;
			this.sbpCommon.Name = "sbpCommon";
			this.sbpCommon.Text = "Common: ";
			// 
			// sbpComFac
			// 
			this.sbpComFac.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpComFac.MinWidth = 100;
			this.sbpComFac.Name = "sbpComFac";
			this.sbpComFac.Text = "Common Sites:";
			// 
			// sbpTime
			// 
			this.sbpTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpTime.MinWidth = 100;
			this.sbpTime.Name = "sbpTime";
			this.sbpTime.Text = "Time used";
			// 
			// sbpEmpty
			// 
			this.sbpEmpty.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.sbpEmpty.Name = "sbpEmpty";
			this.sbpEmpty.Width = 327;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem4,
            this.menuItem7,
            this.menuItem10,
            this.mnuOption,
            this.menuItem14});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNew,
            this.mnuSaveProject,
            this.mnuLoadProject,
            this.mnuCancelLoadPrj,
            this.menuItem6,
            this.mnuSave,
            this.mnuSaveAs,
            this.numSaveCommon,
            this.menuItem11,
            this.mnuFileLoadSigScan,
            this.menuItem5,
            this.mnuExit});
			this.menuItem1.Text = "&File";
			// 
			// mnuNew
			// 
			this.mnuNew.Index = 0;
			this.mnuNew.Text = "&New Project";
			this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// mnuSaveProject
			// 
			this.mnuSaveProject.Index = 1;
			this.mnuSaveProject.Text = "Save &Project...";
			this.mnuSaveProject.Click += new System.EventHandler(this.mnuSaveProject_Click);
			// 
			// mnuLoadProject
			// 
			this.mnuLoadProject.Index = 2;
			this.mnuLoadProject.Text = "&Load Project...";
			this.mnuLoadProject.Click += new System.EventHandler(this.mnuLoadProject_Click);
			// 
			// mnuCancelLoadPrj
			// 
			this.mnuCancelLoadPrj.Index = 3;
			this.mnuCancelLoadPrj.Text = "Cancel Load Project";
			this.mnuCancelLoadPrj.Click += new System.EventHandler(this.mnuCancelLoadPrj_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "-";
			// 
			// mnuSave
			// 
			this.mnuSave.Index = 5;
			this.mnuSave.Text = "&Save Results";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Index = 6;
			this.mnuSaveAs.Text = "S&ave Results as...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// numSaveCommon
			// 
			this.numSaveCommon.Index = 7;
			this.numSaveCommon.Text = "Save &Common as...";
			this.numSaveCommon.Click += new System.EventHandler(this.mnuSaveCommon_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 8;
			this.menuItem11.Text = "-";
			// 
			// mnuFileLoadSigScan
			// 
			this.mnuFileLoadSigScan.Index = 9;
			this.mnuFileLoadSigScan.Text = "Load from SIGSCAN &web search result...";
			this.mnuFileLoadSigScan.Click += new System.EventHandler(this.mnuFileLoadSigScan_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 10;
			this.menuItem5.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 11;
			this.mnuExit.Text = "&Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuLoadFactor,
            this.menuItem23,
            this.menuItem24});
			this.menuItem4.Text = "&Sites";
			// 
			// mnuLoadFactor
			// 
			this.mnuLoadFactor.Index = 0;
			this.mnuLoadFactor.Text = "&Load Sites/Factors...";
			this.mnuLoadFactor.Click += new System.EventHandler(this.mnuLoadFactor_Click);
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 1;
			this.menuItem23.Text = "&Edit Database...";
			this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
			// 
			// menuItem24
			// 
			this.menuItem24.Index = 2;
			this.menuItem24.Text = "&Build Databases...";
			this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 2;
			this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGeneNewSeq,
            this.mnuLoadGenes,
            this.mnuSelGene});
			this.menuItem7.Text = "&Gene";
			// 
			// mnuGeneNewSeq
			// 
			this.mnuGeneNewSeq.Index = 0;
			this.mnuGeneNewSeq.Text = "&New Sequences...";
			this.mnuGeneNewSeq.Click += new System.EventHandler(this.mnuGeneNewSeq_Click);
			// 
			// mnuLoadGenes
			// 
			this.mnuLoadGenes.Index = 1;
			this.mnuLoadGenes.Text = "&Load from File...";
			this.mnuLoadGenes.Click += new System.EventHandler(this.mnuLoadGenes_Click);
			// 
			// mnuSelGene
			// 
			this.mnuSelGene.Index = 2;
			this.mnuSelGene.Text = "&From Gene Names...";
			this.mnuSelGene.Click += new System.EventHandler(this.mnuSelGene_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 3;
			this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSrchSingle,
            this.mnuAbort,
            this.mnuDisplayCurrent,
            this.menuItem2,
            this.mnuSrchCommon,
            this.menuItem3,
            this.mnuShowImage,
            this.mnuShowTextImg});
			this.menuItem10.Text = "&Analyze";
			// 
			// mnuSrchSingle
			// 
			this.mnuSrchSingle.Index = 0;
			this.mnuSrchSingle.Text = "&Search in genes";
			this.mnuSrchSingle.Click += new System.EventHandler(this.mnuSrchSingle_Click);
			// 
			// mnuAbort
			// 
			this.mnuAbort.Index = 1;
			this.mnuAbort.Text = "&Cancel Search";
			this.mnuAbort.Click += new System.EventHandler(this.mnuAbort_Click);
			// 
			// mnuDisplayCurrent
			// 
			this.mnuDisplayCurrent.Index = 2;
			this.mnuDisplayCurrent.Text = "&Display Current Results";
			this.mnuDisplayCurrent.Click += new System.EventHandler(this.mnuDisplayCurrent_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.Text = "-";
			// 
			// mnuSrchCommon
			// 
			this.mnuSrchCommon.Index = 4;
			this.mnuSrchCommon.Text = "C&ommon Sites";
			this.mnuSrchCommon.Click += new System.EventHandler(this.mnuSrchCommon_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 5;
			this.menuItem3.Text = "-";
			// 
			// mnuShowImage
			// 
			this.mnuShowImage.Index = 6;
			this.mnuShowImage.Text = "&Line View";
			this.mnuShowImage.Click += new System.EventHandler(this.mnuShowImage_Click);
			// 
			// mnuShowTextImg
			// 
			this.mnuShowTextImg.Index = 7;
			this.mnuShowTextImg.Text = "Se&quence View";
			this.mnuShowTextImg.Click += new System.EventHandler(this.mnuShowTextImg_Click);
			// 
			// mnuOption
			// 
			this.mnuOption.Index = 4;
			this.mnuOption.Text = "&Options";
			this.mnuOption.Click += new System.EventHandler(this.mnuOption_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 5;
			this.menuItem14.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuStartWizard,
            this.menuItem16,
            this.mnuHelp,
            this.menuItem17});
			this.menuItem14.Text = "&Help";
			// 
			// mnuStartWizard
			// 
			this.mnuStartWizard.Index = 0;
			this.mnuStartWizard.Text = "&Start Wizard...";
			this.mnuStartWizard.Click += new System.EventHandler(this.mnuStartWizard_Click);
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 1;
			this.menuItem16.Text = "-";
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 2;
			this.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.mnuHelp.Text = "H&elp...";
			this.mnuHelp.Click += new System.EventHandler(this.mnuHelp_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 3;
			this.menuItem17.Text = "&About";
			this.menuItem17.Click += new System.EventHandler(this.menuItem17_Click);
			// 
			// tabControl2
			// 
			this.tabControl2.Controls.Add(this.tpgFact);
			this.tabControl2.Controls.Add(this.tpgGene);
			this.tabControl2.Dock = System.Windows.Forms.DockStyle.Left;
			this.tabControl2.Location = new System.Drawing.Point(0, 26);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(128, 401);
			this.tabControl2.TabIndex = 4;
			// 
			// tpgFact
			// 
			this.tpgFact.Controls.Add(this.lbxFact);
			this.tpgFact.Location = new System.Drawing.Point(4, 22);
			this.tpgFact.Name = "tpgFact";
			this.tpgFact.Size = new System.Drawing.Size(120, 375);
			this.tpgFact.TabIndex = 0;
			this.tpgFact.Text = "Sites";
			// 
			// lbxFact
			// 
			this.lbxFact.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbxFact.Location = new System.Drawing.Point(0, 0);
			this.lbxFact.Name = "lbxFact";
			this.lbxFact.Size = new System.Drawing.Size(120, 368);
			this.lbxFact.TabIndex = 3;
			this.lbxFact.DoubleClick += new System.EventHandler(this.lbxFact_DoubleClick);
			// 
			// tpgGene
			// 
			this.tpgGene.Controls.Add(this.lbxGene);
			this.tpgGene.Location = new System.Drawing.Point(4, 22);
			this.tpgGene.Name = "tpgGene";
			this.tpgGene.Size = new System.Drawing.Size(120, 375);
			this.tpgGene.TabIndex = 1;
			this.tpgGene.Text = "Genes";
			// 
			// lbxGene
			// 
			this.lbxGene.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbxGene.Location = new System.Drawing.Point(0, 0);
			this.lbxGene.Name = "lbxGene";
			this.lbxGene.Size = new System.Drawing.Size(120, 368);
			this.lbxGene.TabIndex = 5;
			this.lbxGene.DoubleClick += new System.EventHandler(this.lbxGene_DoubleClick);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(128, 26);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 401);
			this.splitter1.TabIndex = 10;
			this.splitter1.TabStop = false;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(131, 26);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(813, 401);
			this.tabControl1.TabIndex = 8;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.textBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(805, 375);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Sites in Single Sequence";
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.MaxLength = 655360;
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(805, 375);
			this.textBox1.TabIndex = 7;
			this.textBox1.Text = "textBox1";
			this.textBox1.WordWrap = false;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dgCount);
			this.tabPage2.Controls.Add(this.splitter2);
			this.tabPage2.Controls.Add(this.dataGrid1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(805, 375);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Sites Shared by Multiple Sequences";
			// 
			// dgCount
			// 
			this.dgCount.CaptionText = "Common Sites by Gene Count";
			this.dgCount.DataMember = "";
			this.dgCount.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgCount.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgCount.Location = new System.Drawing.Point(355, 0);
			this.dgCount.Name = "dgCount";
			this.dgCount.ReadOnly = true;
			this.dgCount.RowHeaderWidth = 20;
			this.dgCount.Size = new System.Drawing.Size(450, 375);
			this.dgCount.TabIndex = 10;
			this.dgCount.DoubleClick += new System.EventHandler(this.dgCount_DoubleClick);
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(352, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(3, 375);
			this.splitter2.TabIndex = 1;
			this.splitter2.TabStop = false;
			// 
			// dataGrid1
			// 
			this.dataGrid1.CaptionText = "Common Sites by Gene Position";
			this.dataGrid1.DataMember = "";
			this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Left;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(0, 0);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.ReadOnly = true;
			this.dataGrid1.RowHeaderWidth = 20;
			this.dataGrid1.Size = new System.Drawing.Size(352, 375);
			this.dataGrid1.TabIndex = 9;
			// 
			// prgBar
			// 
			this.prgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.prgBar.Location = new System.Drawing.Point(0, 8);
			this.prgBar.Name = "prgBar";
			this.prgBar.Size = new System.Drawing.Size(944, 12);
			this.prgBar.TabIndex = 13;
			this.prgBar.Visible = false;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(944, 449);
			this.Controls.Add(this.prgBar);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.tabControl2);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.toolBar1);
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "FactScan";
			((System.ComponentModel.ISupportInitialize)(this.sbpFact)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpGene)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpSingle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpCommon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpComFac)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpEmpty)).EndInit();
			this.tabControl2.ResumeLayout(false);
			this.tpgFact.ResumeLayout(false);
			this.tpgGene.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        private void lbxFact_DoubleClick(object sender, EventArgs e)
        {
            string text1 = this.lbxFact.SelectedItem.ToString();
            int num3 = this.lbxFact.SelectedIndex;
            DataTable table1 = this.tf.FactorPosition.Clone();
            ArrayList list1 = new ArrayList();
            for (int num1 = 0; num1 < this.tf.Factor.Rows.Count; num1++)
            {
                if (this.tf.Factor.Rows[num1]["site"].ToString() == text1)
                {
                    list1.Add(this.tf.Factor.Rows[num1]["acce"].ToString());
                }
            }
            foreach (string text2 in list1)
            {
                for (int num2 = 0; num2 < this.tf.FactorPosition.Rows.Count; num2++)
                {
                    if (text2 == this.tf.FactorPosition.Rows[num2]["acce"].ToString())
                    {
                        table1.Rows.Add(this.tf.FactorPosition.Rows[num2].ItemArray);
                    }
                }
            }
            this.textBox1.Text = this.display(list1, table1);
        }

        private void lbxGene_DoubleClick(object sender, EventArgs e)
        {
            string text1 = this.lbxGene.SelectedItem.ToString();
            int num1 = this.lbxGene.SelectedIndex;
            DataTable table1 = this.tf.FactorPosition.Clone();
            for (int num2 = 0; num2 < this.tf.FactorPosition.Rows.Count; num2++)
            {
                if (text1 == this.tf.FactorPosition.Rows[num2]["gene"].ToString())
                {
                    table1.Rows.Add(this.tf.FactorPosition.Rows[num2].ItemArray);
                }
            }
            this.textBox1.Text = this.display(num1, table1);
        }

        private void loadPrjThread()
        {
            this.readFacPos(this.filename);
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
            new AboutForm().Show();
        }

        private void menuItem23_Click(object sender, EventArgs e)
        {
            new DatabaseForm().ShowDialog();
        }

        private void menuItem24_Click(object sender, EventArgs e)
        {
            new DBMergeForm().ShowDialog();
        }

        private void mnuAbort_Click(object sender, EventArgs e)
        {
            if ((this.threadSchSgl != null) && this.threadSchSgl.IsAlive)
            {
                DialogResult result1 = MessageBox.Show("Abort Search?", "Abort", MessageBoxButtons.OKCancel);
                if (result1 == DialogResult.OK)
                {
                    this.threadSchSgl.Abort();
                    this.prgBar.Visible = false;
                    this.timer1.Enabled = false;
                    this.status();
                }
            }
        }

        private void mnuCancelLoadPrj_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Abort Loading Project File?", "Abort", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.thLoadPrj.Abort();
                this.timer1.Enabled = false;
                this.prgBar.Visible = false;
                this.status();
            }
        }

        private void mnuDisplayCurrent_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            this.prgBar.Maximum = this.tf.Sequence.Rows.Count;
            this.prgBar.Minimum = 0;
            this.prgBar.Visible = true;
            this.prgBar.Value = 0;
            this.prgValue = 0;
            this.t = DateTime.Now;
            this.running = "display";
            this.timer1.Enabled = true;
            Thread thread1 = new Thread(new ThreadStart(this.displayThread));
            thread1.IsBackground = true;
            thread1.Start();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (base.DataChanged)
                {
                    DialogResult result1 = MessageBox.Show("Save file?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if ((result1 != DialogResult.Cancel) && (result1 == DialogResult.Yes))
                    {
                        if (this.sfDlg.FileName != "")
                        {
                            this.saveFile(this.sfDlg.FileName);
                            base.Close();
                        }
                        else
                        {
                            this.mnuSaveAs_Click(this, null);
                            base.Close();
                        }
                    }
                }
                else
                {
                    base.Close();
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Menu Exit Error: " + exception1.Message, "Error");
            }
        }

        private void mnuFileLoadSigScan_Click(object sender, EventArgs e)
        {
            try
            {
                ImportSigScan scan1 = new ImportSigScan(this.tf);
                scan1.ShowDialog();
                this.tf.Factor = scan1.tf.Factor;
                this.tf.FactorPosition = scan1.tf.FactorPosition;
                this.tf.Sequence = scan1.tf.Sequence;
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Load WWW SignalScan Result File Error: " + exception1.Message, "Error");
            }
            this.status();
        }

        private void mnuGeneNewSeq_Click(object sender, EventArgs e)
        {
            try
            {
                NewSeq seq1 = new NewSeq();
                seq1.ShowDialog();
                if (seq1.textBox1.Text != "")
                {
                    if (this.tf.Sequence.Rows.Count > 0)
                    {
                        switch (MessageBox.Show("Append to existing gene list?", "New Genes", MessageBoxButtons.YesNoCancel))
                        {
                            case DialogResult.Cancel:
                                return;

                            case DialogResult.No:
                                this.tf.Sequence.Rows.Clear();
                                break;
                        }
                    }
                    this.readSeqText(seq1.textBox1.Text);
                    this.status();
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Input New Gene Error: " + exception1.Message, "Error");
            }
        }

        private void mnuHelp_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"help\Help.htm"))
            {
                Process.Start(@"help\Help.htm");
            }
            else
            {
                MessageBox.Show("Can not find help file - Help.htm");
            }
        }

        private void mnuLoadFactor_Click(object sender, EventArgs e)
        {
            this.ofDlg.Filter = "Sites (*.fdb)|*.fdb|Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.ofDlg.DefaultExt = "Sites (*.fdb)|*.fdb";
            try
            {
                if ((this.ofDlg.ShowDialog() != DialogResult.OK) || (this.ofDlg.FileName == ""))
                {
                    return;
                }
                switch (MessageBox.Show("Append to existing sites?", "New Sites Loading...", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        this.readdb(this.ofDlg.FileName);
                        goto Label_0099;

                    case DialogResult.No:
                        this.tf.Factor.Rows.Clear();
                        this.readdb(this.ofDlg.FileName);
                        break;
                }
            Label_0099:
                this.status();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Load Factor Error: " + exception1.Message, "Error");
            }
        }

        private void mnuLoadGenes_Click(object sender, EventArgs e)
        {
            try
            {
                this.ofDlg.Filter = "Sequences (*.sdb)|*.sdb|Text (*.txt)|*.txt|All Files (*.*)|*.*";
                this.ofDlg.DefaultExt = "Sequences (*.sdb)|*.sdb";
                if ((this.ofDlg.ShowDialog() == DialogResult.OK) && (this.ofDlg.FileName != ""))
                {
                    DataTable table1 = this.tf.Sequence.Clone();
                    string[] textArray1 = new string[this.tf.Sequence.Columns.Count];
                    LoadSeqState state1 = new LoadSeqState();
                    LoadSeqFile file1 = new LoadSeqFile();
                    ArrayList list1 = new ArrayList();
                    file1.ShowDialog();
                    state1 = file1.state;
                    if (!state1.Cancel)
                    {
                        if (state1.Append)
                        {
                            table1 = this.readSeqFile(this.ofDlg.FileName);
                        }
                        else
                        {
                            this.tf.Sequence.Rows.Clear();
                            table1 = this.readSeqFile(this.ofDlg.FileName);
                        }
                        for (int num1 = 0; num1 < table1.Rows.Count; num1++)
                        {
                            DataRow row1 = this.tf.Sequence.NewRow();
                            row1[1] = table1.Rows[num1][1].ToString();
                            string text1 = table1.Rows[num1][2].ToString();
                            if (!this.tf.Sequence.Rows.Contains(row1[1].ToString()))
                            {
                                try
                                {
                                    if ((state1.Start == 0) && (state1.Length == 0))
                                    {
                                        row1[2] = text1;
                                    }
                                    if ((state1.Start == 0) && (state1.Length > 0))
                                    {
                                        if (state1.Length < text1.Length)
                                        {
                                            row1[2] = text1.Substring(0, state1.Length);
                                        }
                                        else
                                        {
                                            row1[2] = text1;
                                        }
                                    }
                                    if ((state1.Start == -1) && (state1.Length > 0))
                                    {
                                        if (state1.Length < text1.Length)
                                        {
                                            row1[2] = text1.Substring(text1.Length - state1.Length);
                                        }
                                        else
                                        {
                                            row1[2] = text1;
                                        }
                                    }
                                    if ((state1.Start > 0) && (state1.Length > 0))
                                    {
                                        if ((state1.Start + state1.Length) < text1.Length)
                                        {
                                            row1[2] = text1.Substring(state1.Start, state1.Length);
                                        }
                                        else
                                        {
                                            row1[2] = text1;
                                        }
                                    }
                                    this.tf.Sequence.Rows.Add(row1);
                                }
                                catch (Exception exception1)
                                {
                                    MessageBox.Show("string out of bound error: " + exception1.Message);
                                }
                            }
                            else
                            {
                                list1.Add(row1[1].ToString());
                            }
                            if (list1.Count > 0)
                            {
                                string text2 = "";
                                for (int num2 = 0; num2 < list1.Count; num2++)
                                {
                                    text2 = text2 + list1[num2] + "\t";
                                }
                                MessageBox.Show("Duplicate items found:\r\n" + text2, "Warning");
                            }
                        }
                    }
                }
                this.status();
            }
            catch (Exception exception2)
            {
                MessageBox.Show("Load Gene Error: " + exception2.Message);
            }
        }

        private void mnuLoadProject_Click(object sender, EventArgs e)
        {
            try
            {
                this.ofDlg.Filter = "Project (*.prj)|*.prj|All Files (*.*)|*.*";
                this.ofDlg.DefaultExt = "Project (*.prj)|*.prj";
                if ((this.ofDlg.ShowDialog() == DialogResult.OK) && (this.ofDlg.FileName != ""))
                {
                    this.filename = this.ofDlg.FileName;
                    switch (MessageBox.Show("Append to existing Factor-Position table?", "New Factor-Position Loading...", MessageBoxButtons.YesNoCancel))
                    {
                        case DialogResult.Cancel:
                            return;

                        case DialogResult.No:
                            this.tf.FactorPosition.Rows.Clear();
                            this.tf.Factor.Rows.Clear();
                            this.tf.Sequence.Rows.Clear();
                            break;
                    }
                    this.prgBar.Visible = true;
                    this.prgBar.Minimum = 0;
                    this.prgBar.Value = 0;
                    this.t = DateTime.Now;
                    this.timer1.Enabled = true;
                    this.thLoadPrj = new Thread(new ThreadStart(this.loadPrjThread));
                    this.thLoadPrj.IsBackground = true;
                    this.running = "load prj";
                    this.thLoadPrj.Start();
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Load Project Error: " + exception1.Message, "Error");
            }
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            if (((this.tf.Factor.Rows.Count > 0) || (this.tf.Sequence.Rows.Count > 0)) || (this.tf.FactorPosition.Rows.Count > 0))
            {
                DialogResult result1 = MessageBox.Show("Discard current project?", "Creat New Project", MessageBoxButtons.OKCancel);
                if (result1 == DialogResult.OK)
                {
                    this.textBox1.Text = "";
                    this.tf.FactorPosition.Rows.Clear();
                    this.tf.Factor.Rows.Clear();
                    this.tf.Sequence.Rows.Clear();
                    this.dataGrid1.DataSource = null;
                    this.dataGrid1.SetDataBinding(null, null);
                    this.dgCount.DataSource = null;
                    this.dgCount.SetDataBinding(null, null);
                    this.status();
                }
            }
        }

        private void mnuOption_Click(object sender, EventArgs e)
        {
            this.fOption.ShowDialog();
            this.textBox1.WordWrap = this.fOption.ckbWrap.Checked;
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            this.sfDlg.Filter = "Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.sfDlg.DefaultExt = "Text (*.txt)|*.txt";
            if (base.DataChanged)
            {
                DialogResult result1 = MessageBox.Show("Save factor table of single sequence?", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result1 == DialogResult.OK)
                {
                    if (this.sfDlg.FileName != "")
                    {
                        this.saveFile(this.sfDlg.FileName);
                    }
                    else
                    {
                        this.mnuSaveAs_Click(this, null);
                    }
                }
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            this.sfDlg.Filter = "Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.sfDlg.DefaultExt = "Text (*.txt)|*.txt";
            DialogResult result1 = this.sfDlg.ShowDialog();
            if (result1 == DialogResult.OK)
            {
                if (this.sfDlg.FileName != "")
                {
                    this.saveFile(this.sfDlg.FileName);
                }
                else
                {
                    this.mnuSaveAs_Click(this, null);
                }
            }
        }

        private void mnuSaveCommon_Click(object sender, EventArgs e)
        {
            if ((this.tf.FactorPosition != null) && (this.tf.FactorPosition.Rows.Count > 0))
            {
                this.sfDlg.Filter = "Text (*.txt)|*.txt|All Files (*.*)|*.*";
                this.sfDlg.DefaultExt = "Text (*.txt)|*.txt";
                DialogResult result1 = this.sfDlg.ShowDialog();
                if ((result1 == DialogResult.OK) && (this.sfDlg.FileName != ""))
                {
                    this.saveCommon(this.sfDlg.FileName);
                }
            }
            else
            {
                MessageBox.Show("Please run Search Common first.");
            }
        }

        private void mnuSaveProject_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDlg.Filter = "Project (*.prj)|*.prj|All Files (*.*)|*.*";
                this.sfDlg.DefaultExt = "Project (*.prj)|*.prj";
                DialogResult result1 = this.sfDlg.ShowDialog();
                if ((result1 == DialogResult.OK) && (this.sfDlg.FileName != ""))
                {
                    this.saveSingleTable(this.sfDlg.FileName);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Save Project Error: " + exception1.Message, "Error");
            }
        }

        private void mnuSelGene_Click(object sender, EventArgs e)
        {
            try
            {
                GeneForm9999 form1 = new GeneForm9999(this.tf);
                form1.ShowDialog();
                if (form1.sq != null)
                {
                    this.tf.Sequence = form1.sq;
                }
                this.status();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Menu Load 9999 Error: " + exception1.Message, "Error");
            }
        }

        private void mnuShowImage_Click(object sender, EventArgs e)
        {
            try
            {
                new ImageViewForm(this.tf).Show();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Menu Show Image Error: " + exception1.Message, "Error");
            }
        }

        private void mnuShowTextImg_Click(object sender, EventArgs e)
        {
            try
            {
                new ImageTextPositionForm(this.tf).Show();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Menu Show Sequence View Error: " + exception1.Message, "Error");
            }
        }

        private void mnuSrchCommon_Click(object sender, EventArgs e)
        {
            this.prgBar.Visible = true;
            this.prgBar.Minimum = 0;
            this.prgBar.Maximum = 20;
            this.runSrchComm = true;
            this.prgValue = 3;
            this.commFPNumber = Convert.ToInt32(this.fOption.txbCFnumber.Text);
            if ((this.threadSchSgl == null) && (this.tf.FactorPosition.Rows.Count == 0))
            {
                MessageBox.Show("Please run Analyze -> Search in Genes first", "Reminder");
            }
            else if ((this.threadSchSgl != null) && this.threadSchSgl.IsAlive)
            {
                MessageBox.Show("Single Gene Search not finished yet.\n\r\r\n\r\nHave a cup of coffe! 8-D", "Please be patient...");
            }
            else
            {
                this.timer1.Enabled = true;
                Thread thread1 = new Thread(new ThreadStart(this.SearchThread));
                thread1.IsBackground = true;
                thread1.Start();
            }
        }

        private void mnuSrchSingle_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Text = "";
                this.prgBar.Minimum = 0;
                this.prgBar.Maximum = this.tf.Sequence.Rows.Count;
                this.prgBar.Value = 0;
                this.prgBar.Visible = true;
                this.prgValue = 0;
                this.timer1.Enabled = true;
                this.timer1.Interval = 200;
                this.runSrchSngl = true;
                this.threadSchSgl = new Thread(new ThreadStart(this.SearchThread));
                this.threadSchSgl.IsBackground = true;
                this.threadSchSgl.Start();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Search Single Thread Error: " + exception1.Message, "Error");
            }
        }

        private void mnuStartWizard_Click(object sender, EventArgs e)
        {
            try
            {
                this.wf = new WizardForm();
                this.wf.ShowDialog(this);
                if (!this.wf.cancel)
                {
                    this.init(this.wf);
                    if ((this.tf.Factor.Rows.Count > 0) && (this.tf.Sequence.Rows.Count > 0))
                    {
                        if (this.fOption.ckbSearchSingle.Checked)
                        {
                            this.mnuSrchSingle_Click(this, null);
                        }
                        if (this.fOption.ckbSearchCommon.Checked)
                        {
                            this.mnuSrchCommon_Click(this, null);
                        }
                        if (this.fOption.ckbShowLineV.Checked)
                        {
                            this.mnuShowImage_Click(this, null);
                        }
                        if (this.fOption.ckbShowSeqV.Checked)
                        {
                            this.mnuShowTextImg_Click(this, null);
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Start Wizard Error: " + exception1.Message, "Error");
            }
        }

        public int readdb(string db)
        {
            int num1 = 0;
            ArrayList list1 = new ArrayList();
            ArrayList list2 = new ArrayList();
            try
            {
                if (!File.Exists(db))
                {
                    MessageBox.Show("Can not find database!");
                    return 0;
                }
                StreamReader reader1 = new StreamReader(db);
                while (reader1.Peek() != -1)
                {
                    string text1 = reader1.ReadLine();
                    if (text1 != "")
                    {
                        string[] textArray1 = text1.Split(new char[] { '\t' });
                        if (textArray1.Length < 5)
                        {
                            switch (MessageBox.Show("Data Base Error! \r\nContinue?", "Error", MessageBoxButtons.YesNoCancel))
                            {
                                case DialogResult.No:
                                    base.Close();
                                    Environment.Exit(0);
                                    break;

                                case DialogResult.Cancel:
                                    return 0;
                            }
                        }
                        DataRow row1 = this.tf.Factor.NewRow();
                        for (int num2 = 0; num2 < textArray1.Length; num2++)
                        {
                            row1[num2 + 1] = textArray1[num2];
                        }
                        if (textArray1[2].Length >= Convert.ToInt32(this.fOption.txbPattLen.Text))
                        {
                            if (!this.tf.Factor.Rows.Contains(textArray1[4]))
                            {
                                this.tf.Factor.Rows.Add(row1);
                            }
                            else
                            {
                                list1.Add(textArray1[4]);
                            }
                        }
                        else
                        {
                            list2.Add(textArray1[4]);
                        }
                        num1++;
                    }
                }
                reader1.Close();
                if (list1.Count > 0)
                {
                    string text2 = "";
                    for (int num3 = 0; num3 < list1.Count; num3++)
                    {
                        text2 = text2 + list1[num3] + "\t";
                    }
                    MessageBox.Show("Duplicate items found:\r\n" + text2, "Warning");
                }
                if (list2.Count <= 0)
                {
                    return num1;
                }
                string text3 = "";
                for (int num4 = 0; num4 < list2.Count; num4++)
                {
                    text3 = text3 + list2[num4] + "\t";
                }
                MessageBox.Show(string.Concat(new object[] { "Skipped items for pattern length < ", Convert.ToInt32(this.fOption.txbPattLen.Text), ":\r\n\r\n", text3 }), "Warning");
            }
            catch (Exception exception1)
            {
                DialogResult result2 = MessageBox.Show("Read Factor Data Base Error: " + exception1.Message + "\r\nContinue?", "Error", MessageBoxButtons.YesNo);
                if (result2 == DialogResult.No)
                {
                    base.Close();
                    Environment.Exit(0);
                }
            }
            return num1;
        }

        private void readFacPos(string file)
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            ArrayList list1 = new ArrayList();
            ArrayList list2 = new ArrayList();
            ArrayList list3 = new ArrayList();
            try
            {
                if (!File.Exists(file))
                {
                    MessageBox.Show("Can not find file!");
                }
                else
                {
                    StreamReader reader1 = new StreamReader(file);
                    while (reader1.Peek() != -1)
                    {
                        string[] textArray1 = reader1.ReadLine().Trim().Split(new char[] { '\t' });
                        string text2 = textArray1[0].Trim();
                        if (text2 != null)
                        {
                            text2 = string.IsInterned(text2);
                            if (text2 == ">Factor")
                            {
                                this.prgBarMax = Convert.ToInt32(textArray1[1]);
                                this.prgValue = 0;
                            }
                            else
                            {
                                DataRow row1;
                                if (text2 == ">")
                                {
                                    row1 = this.tf.Factor.NewRow();
                                    this.prgValue++;
                                    for (int num4 = 1; num4 < textArray1.Length; num4++)
                                    {
                                        row1[num4] = textArray1[num4];
                                    }
                                    if (!this.tf.Factor.Rows.Contains(textArray1[5]))
                                    {
                                        this.tf.Factor.Rows.Add(row1);
                                        num1++;
                                        continue;
                                    }
                                    list1.Add(textArray1[5]);
                                    continue;
                                }
                                if (text2 == "%Sequence")
                                {
                                    this.prgValue = 0;
                                    this.prgBarMax = Convert.ToInt32(textArray1[1]);
                                    continue;
                                }
                                if (text2 == "%")
                                {
                                    row1 = this.tf.Sequence.NewRow();
                                    this.prgValue++;
                                    for (int num5 = 1; num5 < textArray1.Length; num5++)
                                    {
                                        row1[num5] = textArray1[num5];
                                    }
                                    if (!this.tf.Sequence.Rows.Contains(textArray1[1]))
                                    {
                                        this.tf.Sequence.Rows.Add(row1);
                                        num2++;
                                        continue;
                                    }
                                    list2.Add(textArray1[1]);
                                    continue;
                                }
                                if (text2 == "$FacPos")
                                {
                                    this.prgValue = 0;
                                    this.prgBarMax = Convert.ToInt32(textArray1[1]);
                                    continue;
                                }
                                if (text2 == "$")
                                {
                                    row1 = this.tf.FactorPosition.NewRow();
                                    this.prgValue++;
                                    for (int num6 = 1; num6 < textArray1.Length; num6++)
                                    {
                                        row1[num6] = textArray1[num6];
                                    }
                                    if (!this.tf.FactorPosition.Rows.Contains(textArray1[4]))
                                    {
                                        this.tf.FactorPosition.Rows.Add(row1);
                                        num3++;
                                    }
                                    else
                                    {
                                        list3.Add(textArray1[4]);
                                    }
                                }
                            }
                        }
                    }
                    reader1.Close();
                    this.running = "load prj done";
                    if (((list1.Count > 0) || (list2.Count > 0)) || (list3.Count > 0))
                    {
                        object obj1;
                        string text1 = "";
                        if (list1.Count > 0)
                        {
                            text1 = "Sites: " + list1.Count + "\r\n";
                            for (int num7 = 0; num7 < list1.Count; num7++)
                            {
                                text1 = text1 + list1[num7] + "\t";
                            }
                            text1 = text1 + "\r\n\r\n";
                        }
                        if (list2.Count > 0)
                        {
                            obj1 = text1;
                            text1 = string.Concat(new object[] { obj1, "Sequences: ", list2.Count, "\r\n" });
                            for (int num8 = 0; num8 < list2.Count; num8++)
                            {
                                text1 = text1 + list2[num8] + "\t";
                            }
                            text1 = text1 + "\r\n\r\n";
                        }
                        if (list3.Count > 0)
                        {
                            obj1 = text1;
                            text1 = string.Concat(new object[] { obj1, "Positions: ", list3.Count, "\r\n" });
                            for (int num9 = 0; num9 < list3.Count; num9++)
                            {
                                text1 = text1 + list3[num9] + "\t";
                            }
                        }
                        MessageBox.Show("Duplicate items found:\r\n\r\n" + text1, "Warning");
                    }
                    MessageBox.Show(string.Concat(new object[] { "Total items loaded \r\nSites: ", num1, "\r\nSequences: ", num2, "\r\nPositions: ", num3 }), "Project loaded successfully");
                }
            }
            catch (Exception exception1)
            {
                this.timer1.Enabled = false;
                MessageBox.Show("Load Project File Error: " + exception1.Message);
            }
        }

        private DataTable readSeqFile(string seqfile)
        {
            string text3 = "";
            string text4 = "";
            int num1 = 0;
            ArrayList list1 = new ArrayList();
            DataTable table1 = this.tf.Sequence.Clone();
            try
            {
                DataRow row1;
                if (!File.Exists(seqfile))
                {
                    MessageBox.Show("Can not find sequence file!");
                    return null;
                }
                StreamReader reader1 = new StreamReader(seqfile);
                while (reader1.Peek() != -1)
                {
                    string text1 = reader1.ReadLine().Trim();
                    if (text1 != "")
                    {
                        if ((num1 == 0) && (text1[0] != '>'))
                        {
                            switch (MessageBox.Show("Check sequence format. Only FASTA format recognizable curently.\r\nContinue?", "Error", MessageBoxButtons.YesNoCancel))
                            {
                                case DialogResult.No:
                                    base.Close();
                                    Environment.Exit(0);
                                    break;

                                case DialogResult.Cancel:
                                    return null;
                            }
                        }
                        if (text1[0] == '>')
                        {
                            string text2 = text1.Substring(1);
                            if (num1 >= 1)
                            {
                                row1 = table1.NewRow();
                                row1[1] = text3;
                                row1[2] = text4;
                                if (!table1.Rows.Contains(text3))
                                {
                                    table1.Rows.Add(row1);
                                }
                                else
                                {
                                    list1.Add(text3);
                                }
                            }
                            num1++;
                            text3 = text2;
                            text4 = "";
                        }
                        else
                        {
                            text4 = text4 + text1;
                        }
                    }
                }
                row1 = table1.NewRow();
                row1[1] = text3;
                row1[2] = text4;
                if (!table1.Rows.Contains(text3))
                {
                    table1.Rows.Add(row1);
                }
                else
                {
                    list1.Add(text3);
                }
                reader1.Close();
                if (list1.Count <= 0)
                {
                    return table1;
                }
                string text5 = "";
                for (int num2 = 0; num2 < list1.Count; num2++)
                {
                    text5 = text5 + list1[num2] + "\t";
                }
                MessageBox.Show("Duplicate items found:\r\n" + text5, "Warning");
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Read Sequence File Error: Check sequence file! \r\n" + exception1.Message, "Error");
            }
            return table1;
        }

        private int readSeqText(string seqtext)
        {
            string text3 = "";
            string text4 = "";
            int num1 = 0;
            ArrayList list1 = new ArrayList();
            try
            {
                DataRow row1;
                if (seqtext == "")
                {
                    return 0;
                }
                string[] textArray1 = seqtext.Split(new char[] { '\n' });
                for (int num2 = 0; num2 < textArray1.Length; num2++)
                {
                    string text1 = textArray1[num2].Trim();
                    if (text1 != "")
                    {
                        if ((num1 == 0) && (text1[0] != '>'))
                        {
                            DialogResult result1 = MessageBox.Show("Check sequence format. Only FASTA format recognizable curently.\r\nContinue?", "Error", MessageBoxButtons.OKCancel);
                            if (result1 == DialogResult.Cancel)
                            {
                                return 0;
                            }
                        }
                        if (text1[0] == '>')
                        {
                            string text2 = text1.Substring(1, text1.Length - 1);
                            if (num1 >= 1)
                            {
                                row1 = this.tf.Sequence.NewRow();
                                row1[1] = text3;
                                row1[2] = text4;
                                if (!this.tf.Sequence.Rows.Contains(text3))
                                {
                                    this.tf.Sequence.Rows.Add(row1);
                                }
                                else
                                {
                                    list1.Add(text3);
                                }
                            }
                            num1++;
                            text3 = text2;
                            text4 = "";
                        }
                        else
                        {
                            text4 = text4 + text1;
                        }
                    }
                }
                row1 = this.tf.Sequence.NewRow();
                row1[1] = text3;
                row1[2] = text4;
                if (!this.tf.Sequence.Rows.Contains(text3))
                {
                    this.tf.Sequence.Rows.Add(row1);
                }
                else
                {
                    list1.Add(text3);
                }
                if (list1.Count <= 0)
                {
                    return num1;
                }
                string text5 = "";
                for (int num3 = 0; num3 < list1.Count; num3++)
                {
                    text5 = text5 + list1[num3] + "\t";
                }
                MessageBox.Show("Duplicate items found:\r\n" + text5, "Warning");
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Read Sequence Text Error: " + exception1.Message + "\r\nCheck sequence format. Only FASTA format recognizable curently.", "Error");
            }
            return num1;
        }

        private void saveCommon(string file)
        {
            StreamWriter writer1 = null;
            try
            {
                if (!File.Exists(file))
                {
                    writer1 = File.CreateText(file);
                }
                else
                {
                    writer1 = new StreamWriter(file);
                }
                DataView view1 = (DataView) this.dataGrid1.DataSource;
                view1.Sort = "Accession,Gene";
                int num1 = view1.Table.Rows.Count;
                int num2 = view1.Table.Columns.Count;
                writer1.WriteLine(this.dataGrid1.CaptionText);
                for (int num3 = -1; num3 < num1; num3++)
                {
                    for (int num4 = 0; num4 < num2; num4++)
                    {
                        if (num3 == -1)
                        {
                            writer1.Write(view1.Table.Columns[num4].ColumnName.ToString());
                        }
                        else
                        {
                            writer1.Write(this.dataGrid1[num3, num4].ToString());
                        }
                        if (num4 < (num2 - 1))
                        {
                            writer1.Write("\t");
                        }
                    }
                    writer1.WriteLine();
                }
                writer1.WriteLine("\r\n\r\n");
                DataTable table1 = (DataTable) this.dgCount.DataSource;
                num1 = table1.Rows.Count;
                num2 = table1.Columns.Count;
                writer1.WriteLine(this.dgCount.CaptionText);
                for (int num5 = -1; num5 < num1; num5++)
                {
                    for (int num6 = 0; num6 < num2; num6++)
                    {
                        if (num5 == -1)
                        {
                            writer1.Write(table1.Columns[num6].ColumnName.ToString());
                        }
                        else
                        {
                            writer1.Write(this.dgCount[num5, num6].ToString());
                        }
                        if (num6 < (num2 - 1))
                        {
                            writer1.Write("\t");
                        }
                    }
                    writer1.WriteLine();
                }
                writer1.Close();
                base.dataSaved();
                base.showCaption(file);
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Save Common File Error: " + exception1.Message);
                writer1.Close();
            }
        }

        private void saveFile(string file)
        {
            try
            {
                StreamWriter writer1;
                if (!File.Exists(file))
                {
                    writer1 = File.CreateText(file);
                }
                else
                {
                    writer1 = new StreamWriter(file);
                }
                writer1.WriteLine(this.textBox1.Text);
                writer1.Close();
                base.dataSaved();
                base.showCaption(file);
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Save File Error: " + exception1.Message);
            }
        }

        private void saveSingleTable(string file)
        {
            try
            {
                StreamWriter writer1;
                if (!File.Exists(file))
                {
                    writer1 = File.CreateText(file);
                }
                else
                {
                    writer1 = new StreamWriter(file);
                }
                int num1 = 0;
                int num2 = 0;
                num1 = this.tf.Factor.Rows.Count;
                num2 = this.tf.Factor.Columns.Count;
                writer1.WriteLine(">Factor\t{0}\t{1}", num1, num2);
                for (int num3 = 0; num3 < num1; num3++)
                {
                    writer1.Write(">");
                    for (int num4 = 1; num4 < num2; num4++)
                    {
                        writer1.Write("\t");
                        writer1.Write(this.tf.Factor.Rows[num3][num4].ToString());
                    }
                    writer1.WriteLine();
                }
                num1 = this.tf.Sequence.Rows.Count;
                num2 = this.tf.Sequence.Columns.Count;
                writer1.WriteLine("%Sequence\t{0}\t{1}", num1, num2 - 1);
                for (int num5 = 0; num5 < num1; num5++)
                {
                    writer1.Write("%");
                    for (int num6 = 1; num6 < num2; num6++)
                    {
                        writer1.Write("\t");
                        writer1.Write(this.tf.Sequence.Rows[num5][num6].ToString());
                    }
                    writer1.WriteLine();
                }
                num1 = this.tf.FactorPosition.Rows.Count;
                num2 = this.tf.FactorPosition.Columns.Count;
                writer1.WriteLine("$FacPos\t{0}\t{1}", num1, num2 - 1);
                for (int num7 = 0; num7 < num1; num7++)
                {
                    writer1.Write("$");
                    for (int num8 = 1; num8 < num2; num8++)
                    {
                        writer1.Write("\t");
                        writer1.Write(this.tf.FactorPosition.Rows[num7][num8].ToString());
                    }
                    writer1.WriteLine();
                }
                writer1.Close();
                base.dataSaved();
                base.showCaption(file);
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Save Single Table File Error: " + exception1.Message);
            }
        }

        private void SearchCommonThread()
        {
            try
            {
                DataRow row1;
                this.t = DateTime.Now;
                this.tf.CommonFactorPosition.Clear();
                this.tf.searchCommon();
                this.prgValue = 15;
                DataTable table1 = new DataTable("commFP");
                table1.Columns.Add("Gene", typeof(string));
                table1.Columns.Add("Position", typeof(string));
                table1.Columns.Add("Accession", typeof(string));
                table1.Columns.Add("Site", typeof(string));
                table1.Columns.Add("Factor", typeof(string));
                for (int num1 = 0; num1 < this.tf.CommonFactorPosition.Rows.Count; num1++)
                {
                    row1 = table1.NewRow();
                    row1[0] = this.tf.CommonFactorPosition.Rows[num1]["gene"];
                    row1[1] = this.tf.CommonFactorPosition.Rows[num1]["pos"];
                    row1[2] = this.tf.CommonFactorPosition.Rows[num1]["acce"].ToString();
                    row1[3] = this.tf.Factor.Rows.Find(this.tf.CommonFactorPosition.Rows[num1]["acce"])["site"].ToString();
                    row1[4] = this.tf.Factor.Rows.Find(this.tf.CommonFactorPosition.Rows[num1]["acce"])["fact"].ToString();
                    table1.Rows.Add(row1);
                }
                this.prgValue = 0x11;
                this.dv = table1.DefaultView;
                this.dv.Sort = "Accession,Gene";
                this.dt = new DataTable();
                ArrayList list1 = this.tf.AcceList;
                ArrayList[] listArray1 = this.tf.SeqList;
                this.dt.Columns.Add("Accession");
                this.dt.Columns.Add("Site");
                this.dt.Columns.Add("Factor");
                this.dt.Columns.Add("Pattern");
                this.dt.Columns.Add("Citation");
                this.dt.Columns.Add("Gene Count");
                this.dt.Columns.Add("Genes");
                for (int num2 = 0; num2 < list1.Count; num2++)
                {
                    string text1 = "";
                    ArrayList list2 = this.tf.unique(listArray1[num2]);
                    for (int num3 = 0; num3 < list2.Count; num3++)
                    {
                        if (num3 > 0)
                        {
                            text1 = text1 + "\t";
                        }
                        text1 = text1 + list2[num3].ToString();
                    }
                    row1 = this.dt.NewRow();
                    row1[0] = list1[num2].ToString();
                    row1[1] = this.tf.Factor.Rows.Find(list1[num2])["site"].ToString();
                    row1[2] = this.tf.Factor.Rows.Find(list1[num2])["fact"].ToString();
                    row1[3] = this.tf.Factor.Rows.Find(list1[num2])["patt"].ToString();
                    row1[4] = this.tf.Factor.Rows.Find(list1[num2])["cite"].ToString();
                    row1[5] = list2.Count.ToString().PadLeft(8, ' ');
                    row1[6] = text1;
                    if (Convert.ToInt32(row1[5].ToString().Trim()) >= this.commFPNumber)
                    {
                        this.dt.Rows.Add(row1);
                    }
                }
                this.running = "common done";
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Search Common Sites Error: " + exception1.Message, "Error");
            }
        }

        private void SearchThread()
        {
            try
            {
                if (this.runSrchSngl)
                {
                    this.threadSchSgl = new Thread(new ThreadStart(this.SingleSearchThread));
                    this.threadSchSgl.IsBackground = true;
                    this.running = "single";
                    this.threadSchSgl.Start();
                    this.threadSchSgl.Join();
                    this.running = "single done";
                    Thread.Sleep(200);
                }
                if (this.runSrchComm)
                {
                    if ((this.threadSchSgl != null) && this.threadSchSgl.IsAlive)
                    {
                        this.threadSchSgl.Join();
                    }
                    this.prgValue = 3;
                    Thread thread1 = new Thread(new ThreadStart(this.SearchCommonThread));
                    thread1.IsBackground = true;
                    this.running = "common";
                    thread1.Start();
                    thread1.Join();
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Search Thread Error: " + exception1.Message, "Error");
            }
        }

        private void SingleSearchThread()
        {
            try
            {
                this.t = DateTime.Now;
                if (this.tf.Factor.Rows.Count > 500)
                {
                    DialogResult result1 = MessageBox.Show("This may take a long time for long sequences and a lot of sites.\r\nContinue?", "Warning", MessageBoxButtons.OKCancel);
                    if (result1 == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                this.caltotal = this.tf.Factor.Rows.Count * this.tf.Sequence.Rows.Count;
                this.tf.FactorPosition.Clear();
                DataTable table1 = this.tf.FactorPosition.Clone();
                for (int num1 = 0; num1 < this.tf.Sequence.Rows.Count; num1++)
                {
                    this.caldone = num1;
                    table1 = this.tf.findFac(this.tf.Sequence.Rows[num1]["name"].ToString(), this.tf.Sequence.Rows[num1]["seq"].ToString());
                    for (int num2 = 0; num2 < table1.Rows.Count; num2++)
                    {
                        this.tf.FactorPosition.Rows.Add(table1.Rows[num2].ItemArray);
                    }
                    this.prgValue++;
                    this.singleText = this.singleText + this.display(num1, table1);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Search Single Error: " + exception1.Message, "Error");
            }
        }

        private void status()
        {
            try
            {
                this.sbpFact.Text = "Sites: " + this.tf.Factor.Rows.Count;
                this.sbpGene.Text = "Gene: " + this.tf.Sequence.Rows.Count;
                this.sbpSingle.Text = "Single: " + this.tf.FactorPosition.Rows.Count.ToString();
                this.sbpCommon.Text = "Common: " + this.tf.CommonFactorPosition.Rows.Count.ToString() + " items";
                this.sbpComFac.Text = "Common Sites: " + this.tf.unique(this.tf.CommonFactorPosition, "acce").Count.ToString();
                this.lbxGene.DataSource = this.tf.DataColumnToArray(this.tf.Sequence, "name");
                this.lbxGene.Invalidate();
                this.lbxFact.DataSource = this.tf.DataColumnToArray(this.tf.Factor, "site");
                this.lbxFact.Invalidate();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Show Status Error: " + exception1.Message, "Error");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan span1 = (TimeSpan) (DateTime.Now - this.t);
            try
            {
                if (this.running == "single")
                {
                    if (this.prgValue <= this.prgBar.Maximum)
                    {
                        this.prgBar.Value = this.prgValue;
                    }
                    this.textBox1.Text = this.singleText;
                    if (span1.TotalSeconds > 0)
                    {
                        this.sbpTime.Text = string.Format("Time elapsed: {0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { (int) span1.TotalHours, span1.Minutes, span1.Seconds, span1.Milliseconds });
                        double num1 = (this.caldone * this.tf.Factor.Rows.Count) + this.tf.CalculationsDone;
                        if (num1 > 0)
                        {
                            double num2 = (((double) this.caltotal) / num1) * span1.TotalSeconds;
                            double num3 = num2 - span1.TotalSeconds;
                            TimeSpan span2 = TimeSpan.FromSeconds(num3);
                            this.sbpEmpty.Text = string.Format("Time remaining: {0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { (int) span2.TotalHours, span2.Minutes, span2.Seconds, span2.Milliseconds });
                        }
                    }
                }
                if (this.running == "single done")
                {
                    this.sbpTime.Text = string.Format("Time elapsed: {0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { (int) span1.TotalHours, span1.Minutes, span1.Seconds, span1.Milliseconds });
                    this.sbpEmpty.Text = "";
                    this.textBox1.Text = this.singleText;
                    this.prgBar.Visible = false;
                    this.singleText = "";
                    if (!this.runSrchComm)
                    {
                        this.timer1.Enabled = false;
                    }
                    else
                    {
                        this.prgBar.Visible = true;
                        this.prgBar.Minimum = 0;
                        this.prgBar.Maximum = 20;
                    }
                    this.status();
                    this.runSrchSngl = false;
                    this.running = "";
                }
                if (this.running == "common")
                {
                    if ((this.prgValue <= this.prgBar.Maximum) && (this.prgValue >= this.prgBar.Minimum))
                    {
                        this.prgBar.Value = this.prgValue;
                    }
                    if (span1.TotalSeconds > 0)
                    {
                        this.sbpTime.Text = string.Format("Time elapsed: {0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { (int) span1.TotalHours, span1.Minutes, span1.Seconds, span1.Milliseconds });
                    }
                }
                if (this.running == "common done")
                {
                    this.dataGrid1.DataSource = this.dv;
                    this.dataGrid1.SetDataBinding(this.dv, null);
                    this.dgCount.DataSource = this.dt;
                    this.dgCount.SetDataBinding(this.dt, null);
                    this.tabPage2.BringToFront();
                    this.tabPage2.Show();
                    this.prgBar.Value = this.prgBar.Maximum;
                    base.dataChanged();
                    this.status();
                    this.prgBar.Visible = false;
                    this.sbpTime.Text = string.Format("Time elapsed: {0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { (int) span1.TotalHours, span1.Minutes, span1.Seconds, span1.Milliseconds });
                    this.timer1.Enabled = false;
                    this.runSrchComm = false;
                    this.running = "";
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Timer search show status error: " + exception1.Message, "Error");
            }
            try
            {
                if (this.running == "load prj")
                {
                    this.prgBar.Maximum = this.prgBarMax;
                    if (this.prgValue <= this.prgBarMax)
                    {
                        this.prgBar.Value = this.prgValue;
                    }
                    this.sbpTime.Text = string.Format("Time elapsed: {0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { (int) span1.TotalHours, span1.Minutes, span1.Seconds, span1.Milliseconds });
                }
                if (this.running == "load prj done")
                {
                    this.sbpTime.Text = string.Format("Time elapsed: {0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { (int) span1.TotalHours, span1.Minutes, span1.Seconds, span1.Milliseconds });
                    this.status();
                    this.running = "";
                    this.prgBar.Visible = false;
                    this.timer1.Enabled = false;
                    this.mnuDisplayCurrent_Click(this, null);
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show("Timer load prj show status error: " + exception2.Message, "Error");
            }
            try
            {
                if (this.running == "display")
                {
                    if (this.prgValue <= this.prgBar.Maximum)
                    {
                        this.prgBar.Value = this.prgValue;
                    }
                    this.sbpTime.Text = string.Format("Time elapsed: {0:D2}:{1:D2}:{2:D2}.{3:D3}", new object[] { (int) span1.TotalHours, span1.Minutes, span1.Seconds, span1.Milliseconds });
                    this.textBox1.Text = this.singleText;
                }
                if (this.running == "display done")
                {
                    this.textBox1.Text = this.singleText;
                    this.running = "";
                    this.prgBar.Visible = false;
                    this.timer1.Enabled = false;
                    this.singleText = "";
                }
            }
            catch (Exception exception3)
            {
                MessageBox.Show("Timer display show status error: " + exception3.Message, "Error");
                return;
            }
        }


        private int caldone;
        private int caltotal;
        private int commFPNumber;
        private IContainer components;
        private DataGrid dataGrid1;
        private DataGrid dgCount;
        private DataTable dt;
        private DataView dv;
        private string filename;
        private MainOptionForm fOption;
        private ListBox lbxFact;
        private ListBox lbxGene;
        private MainMenu mainMenu1;
        private MenuItem menuItem1;
        private MenuItem menuItem10;
        private MenuItem menuItem11;
        private MenuItem menuItem14;
        private MenuItem menuItem16;
        private MenuItem menuItem17;
        private MenuItem menuItem2;
        private MenuItem menuItem23;
        private MenuItem menuItem24;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private MenuItem mnuAbort;
        private MenuItem mnuCancelLoadPrj;
        private MenuItem mnuDisplayCurrent;
        private MenuItem mnuExit;
        private MenuItem mnuFileLoadSigScan;
        private MenuItem mnuGeneNewSeq;
        private MenuItem mnuHelp;
        private MenuItem mnuLoadFactor;
        private MenuItem mnuLoadGenes;
        private MenuItem mnuLoadProject;
        private MenuItem mnuNew;
        private MenuItem mnuOption;
        private MenuItem mnuSave;
        private MenuItem mnuSaveAs;
        private MenuItem mnuSaveProject;
        private MenuItem mnuSelGene;
        private MenuItem mnuShowImage;
        private MenuItem mnuShowTextImg;
        private MenuItem mnuSrchCommon;
        private MenuItem mnuSrchSingle;
        private MenuItem mnuStartWizard;
        private MenuItem numSaveCommon;
        private OpenFileDialog ofDlg;
        private ProgressBar prgBar;
        private int prgBarMax;
        private int prgValue;
        private string running;
        private bool runSrchComm;
        private bool runSrchSngl;
        private StatusBarPanel sbpComFac;
        private StatusBarPanel sbpCommon;
        private StatusBarPanel sbpEmpty;
        private StatusBarPanel sbpFact;
        private StatusBarPanel sbpGene;
        private StatusBarPanel sbpSingle;
        private StatusBarPanel sbpTime;
        private SaveFileDialog sfDlg;
        private string singleText;
        private Splitter splitter1;
        private Splitter splitter2;
        private StatusBar statusBar1;
        private DateTime t;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textBox1;
        private TransFactor tf;
        private Thread thLoadPrj;
        private Thread threadSchSgl;
        private System.Windows.Forms.Timer timer1;
        private ToolBar toolBar1;
        private TabPage tpgFact;
        private TabPage tpgGene;
        private WizardForm wf;
    }
}

