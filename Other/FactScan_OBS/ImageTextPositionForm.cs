namespace FacScan
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Resources;
    using System.Windows.Forms;

    public class ImageTextPositionForm : Form
    {
        public ImageTextPositionForm(TransFactor transfact)
        {
            this.imgList = new ImageList();
            this.myImg = new DrawImageClass();
            this.tf = new TransFactor();
            this.wkFac = new DataTable();
            this.avFac = new DataTable();
            this.comFac = new DataTable();
            this.wkSeq = new DataTable();
            this.avSeq = new DataTable();
            this.imgF = new ImageOptionForm();
            this.seqMAXLen = 0;
            this.zoom = new Rectangle(-1, -1, 0, 0);
            this.InitializeComponent();
            base.StartPosition = FormStartPosition.CenterScreen;
            if (((transfact.Factor.Rows.Count > 0) && (transfact.Sequence.Rows.Count > 0)) && (transfact.FactorPosition.Rows.Count > 0))
            {
                this.tf = transfact;
                this.init();
            }
            else
            {
                if (transfact.Factor.Rows.Count == 0)
                {
                    MessageBox.Show("Sites not loaded");
                }
                if (transfact.Sequence.Rows.Count == 0)
                {
                    MessageBox.Show("Sequences not loaded");
                }
                if (transfact.FactorPosition.Rows.Count == 0)
                {
                    MessageBox.Show("Please run Analyze->Search in Genes first.");
                }
                base.Close();
            }
        }

        private void btnFDn1_Click(object sender, EventArgs e)
        {
            int num1 = this.lsvFact.SelectedIndices.Count;
            if (num1 > 0)
            {
                for (int num2 = 0; num2 < num1; num2++)
                {
                    this.avFac.ImportRow(this.wkFac.Rows[this.lsvFact.SelectedIndices[num2]]);
                }
                for (int num3 = num1 - 1; num3 >= 0; num3--)
                {
                    this.wkFac.Rows[this.lsvFact.SelectedIndices[num3]].Delete();
                }
                this.synDataShow();
            }
        }

        private void btnFDnAll_Click(object sender, EventArgs e)
        {
            int num1 = this.wkFac.Rows.Count;
            for (int num2 = 0; num2 < num1; num2++)
            {
                this.avFac.Rows.Add(this.wkFac.Rows[num2].ItemArray);
            }
            this.wkFac.Rows.Clear();
            this.synDataShow();
        }

        private void btnFUp1_Click(object sender, EventArgs e)
        {
            int num1 = this.lsvFactAv.SelectedIndices.Count;
            if (num1 > 0)
            {
                for (int num2 = 0; num2 < num1; num2++)
                {
                    this.wkFac.ImportRow(this.avFac.Rows[this.lsvFactAv.SelectedIndices[num2]]);
                }
                for (int num3 = num1 - 1; num3 >= 0; num3--)
                {
                    this.avFac.Rows[this.lsvFactAv.SelectedIndices[num3]].Delete();
                }
                this.synDataShow();
            }
        }

        private void btnGDn1_Click(object sender, EventArgs e)
        {
            int num1 = this.lbxGeneWk.SelectedIndices.Count;
            if (num1 > 0)
            {
                for (int num2 = 0; num2 < num1; num2++)
                {
                    this.avSeq.ImportRow(this.wkSeq.Rows[this.lbxGeneWk.SelectedIndices[num2]]);
                }
                for (int num3 = num1 - 1; num3 >= 0; num3--)
                {
                    this.wkSeq.Rows[this.lbxGeneWk.SelectedIndices[num3]].Delete();
                }
                this.synDataShow();
            }
        }

        private void btnGDnAll_Click(object sender, EventArgs e)
        {
            int num1 = this.wkSeq.Rows.Count;
            for (int num2 = 0; num2 < num1; num2++)
            {
                this.avSeq.Rows.Add(this.wkSeq.Rows[num2].ItemArray);
            }
            this.wkSeq.Rows.Clear();
            this.synDataShow();
        }

        private void btnGUp1_Click(object sender, EventArgs e)
        {
            int num1 = this.lbxGeneAv.SelectedIndices.Count;
            if (num1 > 0)
            {
                for (int num2 = 0; num2 < num1; num2++)
                {
                    this.wkSeq.ImportRow(this.avSeq.Rows[this.lbxGeneAv.SelectedIndices[num2]]);
                }
                for (int num3 = num1 - 1; num3 >= 0; num3--)
                {
                    this.avSeq.Rows[this.lbxGeneAv.SelectedIndices[num3]].Delete();
                }
                this.synDataShow();
            }
        }

        private void btnGUpAll_Click(object sender, EventArgs e)
        {
            for (int num1 = 0; num1 < this.avSeq.Rows.Count; num1++)
            {
                this.wkSeq.ImportRow(this.avSeq.Rows[num1]);
            }
            this.avSeq.Rows.Clear();
            this.synDataShow();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                this.bmp.Dispose();
            }
            base.Dispose(disposing);
        }

        private void drawSeqText(string seq, string geneName)
        {
            this.Text = "Sequence View -- " + geneName;
            int num1 = Convert.ToInt32(this.imgF.txbLeftSpace.Text);
            int num2 = Convert.ToInt32(this.imgF.txbRightSpace.Text);
            int num3 = Convert.ToInt32(this.imgF.txbLineSpace.Text);
            int num4 = Convert.ToInt32(this.imgF.txbTopSpace.Text);
            int num5 = 1;
            float single1 = this.picBox.Width;
            float single7 = this.picBox.Height;
            try
            {
                int num6 = Convert.ToInt16(this.imgF.txbSymbolSize.Text);
                int num7 = Convert.ToInt16(this.imgF.txbFontSize.Text);
                int num8 = seq.Length;
                Pen pen1 = new Pen(Color.Black, (float) num5);
                new Pen(Color.Red, (float) num5);
                Brush brush1 = pen1.Brush;
                Font font1 = new Font("Courier New", (float) num7);
                this.bmp = new Bitmap(this.picBox.Width, this.picBox.Height);
                SizeF ef1 = Graphics.FromImage(this.bmp).MeasureString(seq, font1);
                int num9 = (int) (((single1 - num1) - num2) / (ef1.Width / ((float) num8)));
                float single2 = ef1.Width / ((float) num9);
                int num10 = (num8 / num9) + 1;
                this.bmp = new Bitmap(this.picBox.Width, (num10 * num3) + num4);
                Graphics graphics1 = Graphics.FromImage(this.bmp);
                this.picBox.Dock = DockStyle.Top;
                this.picBox.Height = this.bmp.Height;
                graphics1.SmoothingMode = SmoothingMode.AntiAlias;
                graphics1.FillRectangle(new SolidBrush(Color.White), 0, 0, this.bmp.Width, this.bmp.Height);
                string text1 = "";
                for (int num11 = 0; num11 < num8; num11 += num9)
                {
                    if ((num11 + num9) < seq.Length)
                    {
                        text1 = seq.Substring(num11, num9);
                    }
                    else
                    {
                        text1 = seq.Substring(num11);
                    }
                    int num12 = ((num11 / num9) * num3) + num4;
                    graphics1.DrawString(text1, font1, brush1, (float) num1, (float) num12);
                    char[] chArray1 = text1.ToCharArray();
                    for (int num13 = 0; num13 < chArray1.Length; num13++)
                    {
                        if (chArray1[num13] == 'A')
                        {
                            chArray1[num13] = 'T';
                        }
                        else if (chArray1[num13] == 'T')
                        {
                            chArray1[num13] = 'A';
                        }
                        else if (chArray1[num13] == 'C')
                        {
                            chArray1[num13] = 'G';
                        }
                        else if (chArray1[num13] == 'G')
                        {
                            chArray1[num13] = 'C';
                        }
                    }
                    string text2 = new string(chArray1);
                    graphics1.DrawString(text2, font1, brush1, (float) num1, num12 + ef1.Height);
                }
                for (int num14 = 0; num14 < this.tf.FactorPosition.Rows.Count; num14++)
                {
                    if ((this.tf.FactorPosition.Rows[num14]["gene"].ToString() == geneName) && this.wkFac.Rows.Contains(this.tf.FactorPosition.Rows[num14]["acce"]))
                    {
                        DataRow row1 = this.tf.Factor.Rows.Find(this.tf.FactorPosition.Rows[num14]["acce"]);
                        string text3 = row1["patt"].ToString();
                        ef1 = graphics1.MeasureString(text3, font1);
                        float single3 = ef1.Width - ((ef1.Width / ((float) text3.Length)) / 3f);
                        float single4 = ef1.Height;
                        string text4 = this.tf.FactorPosition.Rows[num14]["pos"].ToString();
                        string text5 = text4.Substring(1, 1);
                        int num15 = Convert.ToInt16(text4.Substring(3));
                        string text6 = new MyStringUtilClass().dup("A", (num15 % num9) - 1);
                        ef1 = graphics1.MeasureString(text6, font1);
                        if (text6.Length > 0)
                        {
                            single2 = ef1.Width / ((float) text6.Length);
                        }
                        float single5 = (ef1.Width + num1) - (single2 / 3f);
                        float single6 = ((num15 / num9) * num3) + num4;
                        if (text5 == "-")
                        {
                            single6 += single4;
                        }
                        graphics1.DrawRectangle(pen1, single5, single6, single3, single4);
                        int num16 = Convert.ToInt16(this.wkFac.Rows.Find(this.tf.FactorPosition.Rows[num14]["acce"])["imgIndex"].ToString());
                        if (text5 == "+")
                        {
                            single6 -= num6;
                        }
                        else
                        {
                            single6 += single4;
                        }
                        graphics1.DrawImage(this.imgList.Images[num16], single5, single6, (float) num6, (float) num6);
                        if (this.imgF.cbxShowLabel.Checked)
                        {
                            int num17 = Convert.ToInt16(this.imgF.txbAngle.Text);
                            string text7 = this.wkFac.Rows.Find(this.tf.FactorPosition.Rows[num14]["acce"])["site"].ToString();
                            switch (text7)
                            {
                                case "":
                                case "unknown":
                                case "undefined":
                                    text7 = this.wkFac.Rows.Find(this.tf.FactorPosition.Rows[num14]["acce"])["fact"].ToString();
                                    break;
                            }
                            int num18 = Convert.ToInt16(this.imgF.txbTrunc.Text);
                            if (text7.Length > num18)
                            {
                                text7 = text7.Substring(0, num18);
                            }
                            if (this.imgF.cbxShowPos.Checked)
                            {
                                text7 = text7 + " " + this.tf.FactorPosition.Rows[num14]["pos"].ToString();
                            }
                            if (text5 == "-")
                            {
                                num17 = 0 - num17;
                                single6 += num6 * 2;
                                single5 += num6;
                            }
                            this.myImg.DrawAngleString(graphics1, single5, single6 - num6, (float) (0 - num17), font1, text7);
                        }
                    }
                }
                graphics1.Dispose();
                int num19 = (base.Width - this.pnlImg.Left) - 12;
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Draw Text Error: " + exception1.Message, "Error");
            }
        }

        private void init()
        {
            Color[] colorArray1 = this.myImg.color;
            this.wkFac = this.tf.Factor.Copy();
            this.wkFac.Columns.Add("imgIndex", System.Type.GetType("System.Int16"));
            this.wkFac.Columns.Add("wkState", System.Type.GetType("System.Int16"));
            this.comFac = this.wkFac.Clone();
            this.avFac = this.wkFac.Clone();
            this.imgList = this.myImg.creatSymbolList(this.wkFac.Rows.Count * 2);
            for (int num1 = 0; num1 < this.wkFac.Rows.Count; num1++)
            {
                this.wkFac.Rows[num1]["imgIndex"] = num1;
            }
            this.lsvFact.Clear();
            this.lsvFact.Dock = DockStyle.Fill;
            this.lsvFact.CheckBoxes = false;
            this.lsvFact.SmallImageList = this.imgList;
            this.lsvFact.View = View.Details;
            this.lsvFactAv.Clear();
            this.lsvFactAv.Dock = DockStyle.Fill;
            this.lsvFactAv.CheckBoxes = false;
            this.lsvFactAv.SmallImageList = this.imgList;
            this.lsvFactAv.View = View.Details;
            for (int num2 = 0; num2 < this.wkFac.Columns.Count; num2++)
            {
                this.lsvFact.Columns.Add(this.wkFac.Columns[num2].ColumnName, 50, HorizontalAlignment.Left);
            }
            for (int num3 = 0; num3 < this.avFac.Columns.Count; num3++)
            {
                this.lsvFactAv.Columns.Add(this.avFac.Columns[num3].ColumnName, 50, HorizontalAlignment.Left);
            }
            this.wkSeq = this.tf.Sequence.Copy();
            this.wkSeq.Columns.Add("imgIndex", System.Type.GetType("System.Int16"));
            this.wkSeq.Columns.Add("wkState", System.Type.GetType("System.Int16"));
            this.avSeq = this.wkSeq.Clone();
            this.lbxGeneWk.DataSource = this.tf.unique(this.wkSeq, "name");
            for (int num4 = 0; num4 < this.wkSeq.Rows.Count; num4++)
            {
                this.wkSeq.Rows[num4]["imgIndex"] = num4;
            }
            for (int num5 = 0; num5 < this.tf.CommonFactorPosition.Rows.Count; num5++)
            {
                string text1 = this.tf.CommonFactorPosition.Rows[num5]["acce"].ToString();
                DataRow row1 = this.tf.Factor.Rows.Find(text1);
                if (!this.comFac.Rows.Contains(text1))
                {
                    this.comFac.ImportRow(row1);
                }
            }
            for (int num6 = 0; num6 < this.comFac.Rows.Count; num6++)
            {
                this.comFac.Rows[num6]["imgIndex"] = num6;
            }
            this.synDataShow();
            for (int num7 = 0; num7 < this.wkSeq.Rows.Count; num7++)
            {
                int num8 = 0;
                if (this.seqMAXLen < (num8 = this.wkSeq.Rows[num7]["seq"].ToString().Length))
                {
                    this.seqMAXLen = num8;
                }
            }
            this.bmp = new Bitmap(this.picBox.Width, this.picBox.Height);
        }

        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageTextPositionForm));
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.mnuRefresh = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuOption = new System.Windows.Forms.MenuItem();
			this.mnuZoomI = new System.Windows.Forms.MenuItem();
			this.mnuZoomO = new System.Windows.Forms.MenuItem();
			this.mnuZoomFit = new System.Windows.Forms.MenuItem();
			this.mnuSav = new System.Windows.Forms.MenuItem();
			this.mnuExt = new System.Windows.Forms.MenuItem();
			this.imgList = new System.Windows.Forms.ImageList(this.components);
			this.ofDlg = new System.Windows.Forms.OpenFileDialog();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tbpFact = new System.Windows.Forms.TabPage();
			this.lsvFactAv = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lsvFact = new System.Windows.Forms.ListView();
			this.colHdFact = new System.Windows.Forms.ColumnHeader();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnFUp1 = new System.Windows.Forms.Button();
			this.imgListButtons = new System.Windows.Forms.ImageList(this.components);
			this.tbnFUpAll = new System.Windows.Forms.Button();
			this.btnFDnAll = new System.Windows.Forms.Button();
			this.btnFDn1 = new System.Windows.Forms.Button();
			this.tbpGene = new System.Windows.Forms.TabPage();
			this.lbxGeneAv = new System.Windows.Forms.ListBox();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.panel3 = new System.Windows.Forms.Panel();
			this.lbxGeneWk = new System.Windows.Forms.ListBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.btnGUp1 = new System.Windows.Forms.Button();
			this.btnGUpAll = new System.Windows.Forms.Button();
			this.btnGDnAll = new System.Windows.Forms.Button();
			this.btnGDn1 = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.sbpFactWk = new System.Windows.Forms.StatusBarPanel();
			this.sbpFactAv = new System.Windows.Forms.StatusBarPanel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.sbpGeneWk = new System.Windows.Forms.StatusBarPanel();
			this.sbpGeneAv = new System.Windows.Forms.StatusBarPanel();
			this.sbpZoom = new System.Windows.Forms.StatusBarPanel();
			this.sbpEmpty = new System.Windows.Forms.StatusBarPanel();
			this.pnlImg = new System.Windows.Forms.Panel();
			this.picBox = new System.Windows.Forms.PictureBox();
			this.sfDlg = new System.Windows.Forms.SaveFileDialog();
			this.tabControl1.SuspendLayout();
			this.tbpFact.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tbpGene.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sbpFactWk)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpFactAv)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpGeneWk)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpGeneAv)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpZoom)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpEmpty)).BeginInit();
			this.pnlImg.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuRefresh,
            this.menuItem1,
            this.mnuOption,
            this.mnuZoomI,
            this.mnuZoomO,
            this.mnuZoomFit,
            this.mnuSav,
            this.mnuExt});
			// 
			// mnuRefresh
			// 
			this.mnuRefresh.Index = 0;
			this.mnuRefresh.Text = "&Refresh";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.Text = "&Common Sites";
			// 
			// mnuOption
			// 
			this.mnuOption.Index = 2;
			this.mnuOption.Text = "&Option";
			// 
			// mnuZoomI
			// 
			this.mnuZoomI.Index = 3;
			this.mnuZoomI.Text = "Zoom In &+";
			this.mnuZoomI.Visible = false;
			// 
			// mnuZoomO
			// 
			this.mnuZoomO.Index = 4;
			this.mnuZoomO.Text = "Zoom Out &-";
			this.mnuZoomO.Visible = false;
			// 
			// mnuZoomFit
			// 
			this.mnuZoomFit.Index = 5;
			this.mnuZoomFit.Text = "Zoom &Fit";
			this.mnuZoomFit.Visible = false;
			// 
			// mnuSav
			// 
			this.mnuSav.Index = 6;
			this.mnuSav.Text = "&Save Image";
			// 
			// mnuExt
			// 
			this.mnuExt.Index = 7;
			this.mnuExt.Text = "&Exit";
			// 
			// imgList
			// 
			this.imgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.imgList.ImageSize = new System.Drawing.Size(20, 20);
			this.imgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tbpFact);
			this.tabControl1.Controls.Add(this.tbpGene);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(200, 558);
			this.tabControl1.TabIndex = 1;
			// 
			// tbpFact
			// 
			this.tbpFact.Controls.Add(this.lsvFactAv);
			this.tbpFact.Controls.Add(this.splitter2);
			this.tbpFact.Controls.Add(this.panel1);
			this.tbpFact.Location = new System.Drawing.Point(4, 22);
			this.tbpFact.Name = "tbpFact";
			this.tbpFact.Size = new System.Drawing.Size(192, 532);
			this.tbpFact.TabIndex = 0;
			this.tbpFact.Text = "Sites";
			// 
			// lsvFactAv
			// 
			this.lsvFactAv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lsvFactAv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lsvFactAv.FullRowSelect = true;
			this.lsvFactAv.Location = new System.Drawing.Point(0, 395);
			this.lsvFactAv.Name = "lsvFactAv";
			this.lsvFactAv.Size = new System.Drawing.Size(192, 137);
			this.lsvFactAv.TabIndex = 3;
			this.lsvFactAv.UseCompatibleStateImageBehavior = false;
			this.lsvFactAv.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvFactAv_ColumnClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Factor";
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(0, 392);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(192, 3);
			this.splitter2.TabIndex = 2;
			this.splitter2.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lsvFact);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(192, 392);
			this.panel1.TabIndex = 1;
			// 
			// lsvFact
			// 
			this.lsvFact.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHdFact});
			this.lsvFact.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lsvFact.FullRowSelect = true;
			this.lsvFact.Location = new System.Drawing.Point(0, 0);
			this.lsvFact.Name = "lsvFact";
			this.lsvFact.Size = new System.Drawing.Size(192, 352);
			this.lsvFact.TabIndex = 1;
			this.lsvFact.UseCompatibleStateImageBehavior = false;
			this.lsvFact.DoubleClick += new System.EventHandler(this.lsvFact_DoubleClick);
			this.lsvFact.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lsvFact_MouseUp);
			this.lsvFact.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvFact_ColumnClick);
			// 
			// colHdFact
			// 
			this.colHdFact.Text = "Factor";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnFUp1);
			this.panel2.Controls.Add(this.tbnFUpAll);
			this.panel2.Controls.Add(this.btnFDnAll);
			this.panel2.Controls.Add(this.btnFDn1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 352);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(192, 40);
			this.panel2.TabIndex = 0;
			// 
			// btnFUp1
			// 
			this.btnFUp1.ImageIndex = 0;
			this.btnFUp1.ImageList = this.imgListButtons;
			this.btnFUp1.Location = new System.Drawing.Point(16, 8);
			this.btnFUp1.Name = "btnFUp1";
			this.btnFUp1.Size = new System.Drawing.Size(24, 24);
			this.btnFUp1.TabIndex = 0;
			// 
			// imgListButtons
			// 
			this.imgListButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListButtons.ImageStream")));
			this.imgListButtons.TransparentColor = System.Drawing.Color.Transparent;
			this.imgListButtons.Images.SetKeyName(0, "AboutForm_pictureBox1.png");
			this.imgListButtons.Images.SetKeyName(1, "");
			this.imgListButtons.Images.SetKeyName(2, "");
			this.imgListButtons.Images.SetKeyName(3, "");
			// 
			// tbnFUpAll
			// 
			this.tbnFUpAll.ImageIndex = 2;
			this.tbnFUpAll.ImageList = this.imgListButtons;
			this.tbnFUpAll.Location = new System.Drawing.Point(53, 8);
			this.tbnFUpAll.Name = "tbnFUpAll";
			this.tbnFUpAll.Size = new System.Drawing.Size(24, 24);
			this.tbnFUpAll.TabIndex = 0;
			// 
			// btnFDnAll
			// 
			this.btnFDnAll.ImageIndex = 3;
			this.btnFDnAll.ImageList = this.imgListButtons;
			this.btnFDnAll.Location = new System.Drawing.Point(90, 8);
			this.btnFDnAll.Name = "btnFDnAll";
			this.btnFDnAll.Size = new System.Drawing.Size(24, 24);
			this.btnFDnAll.TabIndex = 0;
			// 
			// btnFDn1
			// 
			this.btnFDn1.ImageIndex = 1;
			this.btnFDn1.ImageList = this.imgListButtons;
			this.btnFDn1.Location = new System.Drawing.Point(127, 8);
			this.btnFDn1.Name = "btnFDn1";
			this.btnFDn1.Size = new System.Drawing.Size(24, 24);
			this.btnFDn1.TabIndex = 0;
			// 
			// tbpGene
			// 
			this.tbpGene.Controls.Add(this.lbxGeneAv);
			this.tbpGene.Controls.Add(this.splitter3);
			this.tbpGene.Controls.Add(this.panel3);
			this.tbpGene.Location = new System.Drawing.Point(4, 22);
			this.tbpGene.Name = "tbpGene";
			this.tbpGene.Size = new System.Drawing.Size(192, 553);
			this.tbpGene.TabIndex = 1;
			this.tbpGene.Text = "Genes";
			// 
			// lbxGeneAv
			// 
			this.lbxGeneAv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbxGeneAv.Location = new System.Drawing.Point(0, 403);
			this.lbxGeneAv.Name = "lbxGeneAv";
			this.lbxGeneAv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbxGeneAv.Size = new System.Drawing.Size(192, 147);
			this.lbxGeneAv.TabIndex = 2;
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 400);
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(192, 3);
			this.splitter3.TabIndex = 1;
			this.splitter3.TabStop = false;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.lbxGeneWk);
			this.panel3.Controls.Add(this.panel4);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(192, 400);
			this.panel3.TabIndex = 0;
			// 
			// lbxGeneWk
			// 
			this.lbxGeneWk.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbxGeneWk.Location = new System.Drawing.Point(0, 0);
			this.lbxGeneWk.Name = "lbxGeneWk";
			this.lbxGeneWk.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbxGeneWk.Size = new System.Drawing.Size(192, 355);
			this.lbxGeneWk.TabIndex = 3;
			this.lbxGeneWk.SelectedIndexChanged += new System.EventHandler(this.lbxGeneWk_SelectedIndexChanged);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.btnGUp1);
			this.panel4.Controls.Add(this.btnGUpAll);
			this.panel4.Controls.Add(this.btnGDnAll);
			this.panel4.Controls.Add(this.btnGDn1);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(0, 360);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(192, 40);
			this.panel4.TabIndex = 2;
			// 
			// btnGUp1
			// 
			this.btnGUp1.ImageList = this.imgListButtons;
			this.btnGUp1.Location = new System.Drawing.Point(16, 8);
			this.btnGUp1.Name = "btnGUp1";
			this.btnGUp1.Size = new System.Drawing.Size(24, 24);
			this.btnGUp1.TabIndex = 0;
			// 
			// btnGUpAll
			// 
			this.btnGUpAll.ImageList = this.imgListButtons;
			this.btnGUpAll.Location = new System.Drawing.Point(53, 8);
			this.btnGUpAll.Name = "btnGUpAll";
			this.btnGUpAll.Size = new System.Drawing.Size(24, 24);
			this.btnGUpAll.TabIndex = 0;
			// 
			// btnGDnAll
			// 
			this.btnGDnAll.ImageList = this.imgListButtons;
			this.btnGDnAll.Location = new System.Drawing.Point(90, 8);
			this.btnGDnAll.Name = "btnGDnAll";
			this.btnGDnAll.Size = new System.Drawing.Size(24, 24);
			this.btnGDnAll.TabIndex = 0;
			// 
			// btnGDn1
			// 
			this.btnGDn1.ImageList = this.imgListButtons;
			this.btnGDn1.Location = new System.Drawing.Point(127, 8);
			this.btnGDn1.Name = "btnGDn1";
			this.btnGDn1.Size = new System.Drawing.Size(24, 24);
			this.btnGDn1.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(200, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 558);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// sbpFactWk
			// 
			this.sbpFactWk.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpFactWk.Name = "sbpFactWk";
			this.sbpFactWk.Text = "sbpFactWk";
			this.sbpFactWk.Width = 71;
			// 
			// sbpFactAv
			// 
			this.sbpFactAv.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpFactAv.Name = "sbpFactAv";
			this.sbpFactAv.Text = "sbpFactAv";
			this.sbpFactAv.Width = 68;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 558);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbpFactWk,
            this.sbpFactAv,
            this.sbpGeneWk,
            this.sbpGeneAv,
            this.sbpZoom,
            this.sbpEmpty});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(704, 22);
			this.statusBar1.TabIndex = 0;
			// 
			// sbpGeneWk
			// 
			this.sbpGeneWk.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpGeneWk.Name = "sbpGeneWk";
			this.sbpGeneWk.Text = "sbpGeneWk";
			this.sbpGeneWk.Width = 77;
			// 
			// sbpGeneAv
			// 
			this.sbpGeneAv.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpGeneAv.Name = "sbpGeneAv";
			this.sbpGeneAv.Text = "sbpGeneAv";
			this.sbpGeneAv.Width = 73;
			// 
			// sbpZoom
			// 
			this.sbpZoom.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpZoom.Name = "sbpZoom";
			this.sbpZoom.Width = 10;
			// 
			// sbpEmpty
			// 
			this.sbpEmpty.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.sbpEmpty.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.sbpEmpty.Name = "sbpEmpty";
			this.sbpEmpty.Width = 388;
			// 
			// pnlImg
			// 
			this.pnlImg.AutoScroll = true;
			this.pnlImg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlImg.Controls.Add(this.picBox);
			this.pnlImg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlImg.Location = new System.Drawing.Point(203, 0);
			this.pnlImg.Name = "pnlImg";
			this.pnlImg.Size = new System.Drawing.Size(501, 558);
			this.pnlImg.TabIndex = 3;
			// 
			// picBox
			// 
			this.picBox.BackColor = System.Drawing.SystemColors.HighlightText;
			this.picBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picBox.Location = new System.Drawing.Point(0, 0);
			this.picBox.Name = "picBox";
			this.picBox.Size = new System.Drawing.Size(497, 554);
			this.picBox.TabIndex = 5;
			this.picBox.TabStop = false;
			this.picBox.Paint += new System.Windows.Forms.PaintEventHandler(this.picBox_Paint);
			this.picBox.Resize += new System.EventHandler(this.mnuRefresh_Click);
			// 
			// ImageTextPositionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(704, 601);
			this.Controls.Add(this.pnlImg);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.statusBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "ImageTextPositionForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Sequence View";
			this.tabControl1.ResumeLayout(false);
			this.tbpFact.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.tbpGene.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.sbpFactWk)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpFactAv)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpGeneWk)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpGeneAv)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpZoom)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpEmpty)).EndInit();
			this.pnlImg.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
			this.ResumeLayout(false);

        }

        private void lbxGeneWk_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text1 = this.lbxGeneWk.SelectedItem.ToString();
            this.drawSeqText(this.wkSeq.Rows.Find(text1)["seq"].ToString(), text1);
            this.picBox.Invalidate();
        }

        private void lsvFact_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.lsvFact.ListViewItemSorter = new FacScan.ImageTextPositionForm.ListViewItemComparer(e.Column);
        }

        private void lsvFact_DoubleClick(object sender, EventArgs e)
        {
            int num1 = this.lsvFact.SelectedItems[0].Index;
            this.wkFac.Rows[num1]["imgIndex"] = (Convert.ToInt16(this.wkFac.Rows[num1]["imgIndex"].ToString()) + 1) % this.imgList.Images.Count;
            this.synDataShow("factor");
            this.lbxGeneWk_SelectedIndexChanged(this, null);
        }

        private void lsvFact_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ofDlg.Filter = "Bit Map (*.BMP)|*.BMP|Jpeg (*.JPG)|*.JPG|GIF (*.GIF)|*.GIF|PNG (*.PNG)|*.PNG|TIFF (*.TIF)|*.TIF|WMF (*.WMF)|*.WMF|All files (*.*)|*.*";
                if (this.ofDlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Bitmap bitmap1 = new Bitmap(this.ofDlg.FileName);
                        this.imgList.Images.Add(bitmap1, bitmap1.GetPixel(1, 1));
                        int num1 = this.lsvFact.SelectedItems[0].Index;
                        this.wkFac.Rows[num1]["imgIndex"] = this.imgList.Images.Count - 1;
                        this.synDataShow();
                        bitmap1.Dispose();
                    }
                    catch (Exception exception1)
                    {
                        MessageBox.Show("Load Symbol from Image File Error: " + exception1.Message);
                        base.Close();
                        Environment.Exit(0);
                    }
                }
            }
        }

        private void lsvFactAv_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.lsvFactAv.ListViewItemSorter = new FacScan.ImageTextPositionForm.ListViewItemComparer(e.Column);
        }

        private void magnify()
        {
            try
            {
                if ((this.zoom.Width >= 5) && (this.zoom.Height >= 5))
                {
                    this.picBox.Dock = DockStyle.None;
                    Graphics graphics1 = this.picBox.CreateGraphics();
                    int num1 = 0x10;
                    int num2 = this.picBox.Width;
                    int num3 = this.picBox.Height;
                    int num4 = this.pnlImg.Width / this.zoom.Width;
                    int num5 = this.pnlImg.Height / this.zoom.Height;
                    if (num4 > num1)
                    {
                        num4 = num1;
                    }
                    if (num5 > num1)
                    {
                        num5 = num1;
                    }
                    this.picBox.Width = num2 * num4;
                    this.picBox.Height = num3 * num5;
                    Label label1 = new Label();
                    label1.Location = new Point(this.zoom.Left, this.zoom.Top);
                    label1.Size = new Size(this.zoom.Width, this.zoom.Height);
                    label1.Name = "label";
                    label1.Text = "LABEL!ADDED!";
                    label1.Visible = true;
                    this.pnlImg.Controls.Add(label1);
                    this.pnlImg.ScrollControlIntoView(label1);
                    graphics1.DrawImageUnscaled(this.bmp, 0, 0);
                    this.picBox.Invalidate();
                    graphics1.Dispose();
                    label1.Dispose();
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Error: " + exception1.Message, "Error!");
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Change working factor collection?", "Confirm change", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataTable table1 = new DataTable();
                table1 = this.comFac;
                this.comFac = this.wkFac;
                this.wkFac = table1;
                this.avFac.Rows.Clear();
                this.synDataShow();
                if (this.menuItem1.Text == "&Common Sites")
                {
                    this.menuItem1.Text = "&All Sites";
                }
                else if (this.menuItem1.Text == "&All Sites")
                {
                    this.menuItem1.Text = "&Common Sites";
                }
            }
        }

        private void mnuExt_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Close this window?", "", MessageBoxButtons.YesNo);
            if (result1 == DialogResult.Yes)
            {
                base.Close();
            }
        }

        private void mnuOption_Click(object sender, EventArgs e)
        {
            this.imgF.ShowDialog();
            this.mnuRefresh_Click(this, null);
        }

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            this.lbxGeneWk_SelectedIndexChanged(this, null);
        }

        private void mnuSav_Click(object sender, EventArgs e)
        {
            this.myImg.SaveImage(this.sfDlg, this.bmp);
        }

        private void mnuZoomFit_Click(object sender, EventArgs e)
        {
            Graphics graphics1 = this.picBox.CreateGraphics();
            graphics1.DrawImageUnscaled(this.bmp, 0, 0);
            graphics1.Dispose();
            this.picBox.Dock = DockStyle.Fill;
            this.picBox.Invalidate();
        }

        private void mnuZoomI_Click(object sender, EventArgs e)
        {
            int num1 = this.picBox.Width;
            int num2 = this.picBox.Height;
            this.picBox.Dock = DockStyle.None;
            this.picBox.Width = num1;
            this.picBox.Height = num2;
            this.picBox.Width = Convert.ToInt32((double) (this.picBox.Width * 1.25));
            this.picBox.Height = Convert.ToInt32((double) (this.picBox.Height * 1.25));
            Graphics graphics1 = this.picBox.CreateGraphics();
            graphics1.DrawImageUnscaled(this.bmp, 0, 0);
            graphics1.Dispose();
            this.picBox.Invalidate();
        }

        private void mnuZoomO_Click(object sender, EventArgs e)
        {
            int num1 = this.picBox.Width;
            int num2 = this.picBox.Height;
            this.picBox.Dock = DockStyle.None;
            this.picBox.Width = num1;
            this.picBox.Height = num2;
            this.picBox.Width = Convert.ToInt32((double) (((double) this.picBox.Width) / 1.25));
            this.picBox.Height = Convert.ToInt32((double) (((double) this.picBox.Height) / 1.25));
            Graphics graphics1 = this.picBox.CreateGraphics();
            graphics1.DrawImageUnscaled(this.bmp, 0, 0);
            graphics1.Dispose();
            this.picBox.Invalidate();
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.zoom.Location = new Point(e.X, e.Y);
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((this.zoom.Left != -1) && (this.zoom.Top != -1))
            {
                Graphics graphics1 = this.picBox.CreateGraphics();
                graphics1.DrawImageUnscaled(this.bmp, 0, 0);
                this.picBox.Invalidate();
                Point point1 = base.PointToScreen(new Point(this.zoom.Left + this.pnlImg.Left, this.zoom.Top));
                int num1 = Math.Min(e.X, this.pnlImg.Width);
                int num2 = Math.Min(e.Y, this.pnlImg.Height);
                if (e.X < 0)
                {
                    num1 = 0;
                }
                if (e.Y < 0)
                {
                    num2 = 0;
                }
                Size size1 = new Size(num1 - this.zoom.Left, num2 - this.zoom.Top);
                this.zoom.Size = size1;
                Rectangle rectangle1 = new Rectangle(point1, size1);
                ControlPaint.DrawReversibleFrame(rectangle1, this.BackColor, FrameStyle.Dashed);
                ControlPaint.DrawReversibleFrame(rectangle1, this.BackColor, FrameStyle.Dashed);
                graphics1.Dispose();
            }
            this.sbpEmpty.Text = string.Concat(new object[] { "X: ", e.X, " Y: ", e.Y });
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.zoom.Left != -1)
            {
                Point point1 = base.PointToScreen(new Point(this.zoom.Left + this.pnlImg.Left, this.zoom.Top));
                Rectangle rectangle1 = new Rectangle(point1, this.zoom.Size);
                ControlPaint.DrawReversibleFrame(rectangle1, this.BackColor, FrameStyle.Dashed);
                ControlPaint.DrawReversibleFrame(rectangle1, this.BackColor, FrameStyle.Dashed);
                this.zoom.Location = new Point(-1, -1);
            }
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            this.lbxGeneWk_SelectedIndexChanged(this, null);
            e.Graphics.DrawImageUnscaled(this.bmp, 0, 0);
        }

        private void synDataShow()
        {
            this.synDataShow("factor");
            this.synDataShow("gene");
        }

        private void synDataShow(string which)
        {
            int num1 = this.wkFac.Rows.Count;
            int num2 = this.wkFac.Columns.Count;
            int num8 = this.wkSeq.Rows.Count;
            int num9 = this.wkSeq.Columns.Count;
            try
            {
                switch (which)
                {
                    case "factor":
                        this.lsvFact.Items.Clear();
                        for (int num3 = 0; num3 < num1; num3++)
                        {
                            this.lsvFact.Items.Add(this.wkFac.Rows[num3]["id"].ToString(), Convert.ToInt16(this.wkFac.Rows[num3]["imgIndex"]));
                            for (int num4 = 1; num4 < num2; num4++)
                            {
                                string text1 = this.wkFac.Rows[num3][num4].ToString();
                                this.lsvFact.Items[num3].SubItems.Add(text1);
                            }
                        }
                        num1 = this.avFac.Rows.Count;
                        num2 = this.avFac.Columns.Count;
                        this.lsvFactAv.Items.Clear();
                        for (int num5 = 0; num5 < num1; num5++)
                        {
                            this.lsvFactAv.Items.Add(this.avFac.Rows[num5]["id"].ToString(), Convert.ToInt16(this.avFac.Rows[num5]["imgIndex"]));
                            for (int num6 = 1; num6 < num2; num6++)
                            {
                                string text2 = this.avFac.Rows[num5][num6].ToString();
                                this.lsvFactAv.Items[num5].SubItems.Add(text2);
                            }
                        }
                        break;

                    case "gene":
                        this.lbxGeneWk.DataSource = this.tf.unique(this.wkSeq, "name");
                        this.lbxGeneAv.DataSource = this.tf.unique(this.avSeq, "name");
                        this.sbpFactWk.Text = "Working Sites: " + this.lsvFact.Items.Count.ToString();
                        this.sbpFactAv.Text = "Unused Sites: " + this.lsvFactAv.Items.Count.ToString();
                        this.sbpGeneWk.Text = "Working Gens: " + this.lbxGeneWk.Items.Count.ToString();
                        this.sbpGeneAv.Text = "Unused Gens: " + this.lbxGeneAv.Items.Count.ToString();
                        break;
                }
                this.picBox.Invalidate();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Synchronize data show Error: " + exception1.Message, "Error");
            }
        }

        private void tbnFUpAll_Click(object sender, EventArgs e)
        {
            for (int num1 = 0; num1 < this.avFac.Rows.Count; num1++)
            {
                this.wkFac.ImportRow(this.avFac.Rows[num1]);
            }
            this.avFac.Rows.Clear();
            this.synDataShow();
        }


        private DataTable avFac;
        private DataTable avSeq;
        private Bitmap bmp;
        private Button btnFDn1;
        private Button btnFDnAll;
        private Button btnFUp1;
        private Button btnGDn1;
        private Button btnGDnAll;
        private Button btnGUp1;
        private Button btnGUpAll;
        private ColumnHeader colHdFact;
        private ColumnHeader columnHeader1;
        private DataTable comFac;
        private IContainer components;
        private ImageOptionForm imgF;
        private ImageList imgList;
        private ImageList imgListButtons;
        private ListBox lbxGeneAv;
        private ListBox lbxGeneWk;
        private ListView lsvFact;
        private ListView lsvFactAv;
        private MainMenu mainMenu1;
        private MenuItem menuItem1;
        private MenuItem mnuExt;
        private MenuItem mnuOption;
        private MenuItem mnuRefresh;
        private MenuItem mnuSav;
        private MenuItem mnuZoomFit;
        private MenuItem mnuZoomI;
        private MenuItem mnuZoomO;
        private DrawImageClass myImg;
        private OpenFileDialog ofDlg;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private PictureBox picBox;
        private Panel pnlImg;
        private StatusBarPanel sbpEmpty;
        private StatusBarPanel sbpFactAv;
        private StatusBarPanel sbpFactWk;
        private StatusBarPanel sbpGeneAv;
        private StatusBarPanel sbpGeneWk;
        private StatusBarPanel sbpZoom;
        private int seqMAXLen;
        private SaveFileDialog sfDlg;
        private Splitter splitter1;
        private Splitter splitter2;
        private Splitter splitter3;
        private StatusBar statusBar1;
        private TabControl tabControl1;
        private Button tbnFUpAll;
        private TabPage tbpFact;
        private TabPage tbpGene;
        private TransFactor tf;
        private DataTable wkFac;
        private DataTable wkSeq;
        private Rectangle zoom;


        private class ListViewItemComparer : IComparer
        {
            public ListViewItemComparer()
            {
                this.col = 0;
            }

            public ListViewItemComparer(int column)
            {
                this.col = column;
            }

            public int Compare(object x, object y)
            {
                if ((this.col != 0) && (this.col != 4))
                {
                    return string.Compare(((ListViewItem) x).SubItems[this.col].Text, ((ListViewItem) y).SubItems[this.col].Text);
                }
                int num2 = Convert.ToInt32(((ListViewItem) x).SubItems[this.col].Text);
                int num3 = Convert.ToInt32(((ListViewItem) y).SubItems[this.col].Text);
                if (num2 > num3)
                {
                    return 1;
                }
                if (num2 == num3)
                {
                    return 0;
                }
                return -1;
            }


            private int col;
        }
    }
}

