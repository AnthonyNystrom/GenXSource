namespace Facade
{
    using Attrs;
    using MathTable;
    using Nodes;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Resources;
    using System.Windows.Forms;

    internal class MatrixPropertiesDialog : Form
    {
        internal class MathTableButton : Button
        {
            public MathTableButton()
            {
                this.row = 0;
                this.col = 0;
            }

            public int row;
            public int col;
        }

        public MatrixPropertiesDialog()
        {
            this.numRows = 6;
            this.numCols = 6;
            this.buttonsOrigin = new Point(40, 0xd8);
            this.container = null;
            this.success_ = false;
            this.bWidth = 0x18;
            this.bHeight = 0x18;
            this.canvasWidth = 10;
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.container != null))
            {
                this.container.Dispose();
            }
            base.Dispose(disposing);
        }
        
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Return)
            {
                this.Success = true;
                base.Close();
            }
            else if (keyData == Keys.Escape)
            {
                this.Success = false;
                base.Close();
            }
            else
            {
                return base.ProcessDialogKey(keyData);
            }
            return true;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatrixPropertiesDialog));
            this.group1 = new System.Windows.Forms.GroupBox();
            this.tablealignAxis = new System.Windows.Forms.RadioButton();
            this.tablealignbaseline = new System.Windows.Forms.RadioButton();
            this.tablealigncenter = new System.Windows.Forms.RadioButton();
            this.tablealignbottom = new System.Windows.Forms.RadioButton();
            this.tablealigntop_ = new System.Windows.Forms.RadioButton();
            this.group2 = new System.Windows.Forms.GroupBox();
            this.rowalignaxis = new System.Windows.Forms.RadioButton();
            this.rowalignbaseline = new System.Windows.Forms.RadioButton();
            this.rowaligncenter = new System.Windows.Forms.RadioButton();
            this.rowalignbottom = new System.Windows.Forms.RadioButton();
            this.rowaligntop = new System.Windows.Forms.RadioButton();
            this.group3 = new System.Windows.Forms.GroupBox();
            this.colaligncenter = new System.Windows.Forms.RadioButton();
            this.colalignright = new System.Windows.Forms.RadioButton();
            this.colalignleft = new System.Windows.Forms.RadioButton();
            this.group4 = new System.Windows.Forms.GroupBox();
            this.tablelinestylesolid = new System.Windows.Forms.RadioButton();
            this.tablelinestyledashed = new System.Windows.Forms.RadioButton();
            this.tablelinestylenone = new System.Windows.Forms.RadioButton();
            this.rowspacing = new System.Windows.Forms.TextBox();
            this.group5 = new System.Windows.Forms.GroupBox();
            this.collinestylesolid = new System.Windows.Forms.RadioButton();
            this.collinestyledashed = new System.Windows.Forms.RadioButton();
            this.collinestylenone = new System.Windows.Forms.RadioButton();
            this.colSpacing_ = new System.Windows.Forms.TextBox();
            this.group6 = new System.Windows.Forms.GroupBox();
            this.tablelinesolid = new System.Windows.Forms.RadioButton();
            this.tablelinedashed = new System.Windows.Forms.RadioButton();
            this.tablelinenone = new System.Windows.Forms.RadioButton();
            this.tablespacing = new System.Windows.Forms.TextBox();
            this.equalCols = new System.Windows.Forms.CheckBox();
            this.equalRows = new System.Windows.Forms.CheckBox();
            this.cancelButton = new Glass.GlassButton();
            this.okButton = new Glass.GlassButton();
            this.tabcontrol = new System.Windows.Forms.TabControl();
            this.tableTab = new System.Windows.Forms.TabPage();
            this.group7 = new System.Windows.Forms.GroupBox();
            this.rowstab = new System.Windows.Forms.TabPage();
            this.group8 = new System.Windows.Forms.GroupBox();
            this.group9 = new System.Windows.Forms.GroupBox();
            this.colsTab = new System.Windows.Forms.TabPage();
            this.group10 = new System.Windows.Forms.GroupBox();
            this.group11 = new System.Windows.Forms.GroupBox();
            this.cellsTab = new System.Windows.Forms.TabPage();
            this.group12 = new System.Windows.Forms.GroupBox();
            this.rowalignaxis_ = new System.Windows.Forms.RadioButton();
            this.rowalignbaseline_ = new System.Windows.Forms.RadioButton();
            this.rowaligncenter_ = new System.Windows.Forms.RadioButton();
            this.rowalignbottom_ = new System.Windows.Forms.RadioButton();
            this.rowaligntop_ = new System.Windows.Forms.RadioButton();
            this.group13 = new System.Windows.Forms.GroupBox();
            this.raligncenter = new System.Windows.Forms.RadioButton();
            this.rowalignright = new System.Windows.Forms.RadioButton();
            this.rowalignleft = new System.Windows.Forms.RadioButton();
            this.selAllbutton = new Glass.GlassButton();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            this.group4.SuspendLayout();
            this.group5.SuspendLayout();
            this.group6.SuspendLayout();
            this.tabcontrol.SuspendLayout();
            this.tableTab.SuspendLayout();
            this.group7.SuspendLayout();
            this.rowstab.SuspendLayout();
            this.group8.SuspendLayout();
            this.group9.SuspendLayout();
            this.colsTab.SuspendLayout();
            this.group10.SuspendLayout();
            this.group11.SuspendLayout();
            this.cellsTab.SuspendLayout();
            this.group12.SuspendLayout();
            this.group13.SuspendLayout();
            this.SuspendLayout();
            // 
            // group1
            // 
            this.group1.Controls.Add(this.tablealignAxis);
            this.group1.Controls.Add(this.tablealignbaseline);
            this.group1.Controls.Add(this.tablealigncenter);
            this.group1.Controls.Add(this.tablealignbottom);
            this.group1.Controls.Add(this.tablealigntop_);
            this.group1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group1, "group1");
            this.group1.Name = "group1";
            this.group1.TabStop = false;
            // 
            // tablealignAxis
            // 
            this.tablealignAxis.Checked = true;
            resources.ApplyResources(this.tablealignAxis, "tablealignAxis");
            this.tablealignAxis.Name = "tablealignAxis";
            this.tablealignAxis.TabStop = true;
            this.tablealignAxis.CheckedChanged += new System.EventHandler(this.taAxisClick);
            // 
            // tablealignbaseline
            // 
            resources.ApplyResources(this.tablealignbaseline, "tablealignbaseline");
            this.tablealignbaseline.Name = "tablealignbaseline";
            this.tablealignbaseline.CheckedChanged += new System.EventHandler(this.taBaselineClick);
            // 
            // tablealigncenter
            // 
            resources.ApplyResources(this.tablealigncenter, "tablealigncenter");
            this.tablealigncenter.Name = "tablealigncenter";
            this.tablealigncenter.CheckedChanged += new System.EventHandler(this.taCenterClick);
            // 
            // tablealignbottom
            // 
            resources.ApplyResources(this.tablealignbottom, "tablealignbottom");
            this.tablealignbottom.Name = "tablealignbottom";
            this.tablealignbottom.CheckedChanged += new System.EventHandler(this.taBottomClick);
            // 
            // tablealigntop_
            // 
            resources.ApplyResources(this.tablealigntop_, "tablealigntop_");
            this.tablealigntop_.Name = "tablealigntop_";
            this.tablealigntop_.CheckedChanged += new System.EventHandler(this.tatopClick);
            // 
            // group2
            // 
            this.group2.Controls.Add(this.rowalignaxis);
            this.group2.Controls.Add(this.rowalignbaseline);
            this.group2.Controls.Add(this.rowaligncenter);
            this.group2.Controls.Add(this.rowalignbottom);
            this.group2.Controls.Add(this.rowaligntop);
            this.group2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group2, "group2");
            this.group2.Name = "group2";
            this.group2.TabStop = false;
            // 
            // rowalignaxis
            // 
            resources.ApplyResources(this.rowalignaxis, "rowalignaxis");
            this.rowalignaxis.Name = "rowalignaxis";
            this.rowalignaxis.CheckedChanged += new System.EventHandler(this.raaxisClick);
            // 
            // rowalignbaseline
            // 
            this.rowalignbaseline.Checked = true;
            resources.ApplyResources(this.rowalignbaseline, "rowalignbaseline");
            this.rowalignbaseline.Name = "rowalignbaseline";
            this.rowalignbaseline.TabStop = true;
            this.rowalignbaseline.CheckedChanged += new System.EventHandler(this.raBaselineClick);
            // 
            // rowaligncenter
            // 
            resources.ApplyResources(this.rowaligncenter, "rowaligncenter");
            this.rowaligncenter.Name = "rowaligncenter";
            this.rowaligncenter.CheckedChanged += new System.EventHandler(this.raCenterClick);
            // 
            // rowalignbottom
            // 
            resources.ApplyResources(this.rowalignbottom, "rowalignbottom");
            this.rowalignbottom.Name = "rowalignbottom";
            this.rowalignbottom.CheckedChanged += new System.EventHandler(this.raBottomClick);
            // 
            // rowaligntop
            // 
            resources.ApplyResources(this.rowaligntop, "rowaligntop");
            this.rowaligntop.Name = "rowaligntop";
            this.rowaligntop.CheckedChanged += new System.EventHandler(this.raTopClick);
            // 
            // group3
            // 
            this.group3.Controls.Add(this.colaligncenter);
            this.group3.Controls.Add(this.colalignright);
            this.group3.Controls.Add(this.colalignleft);
            this.group3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group3, "group3");
            this.group3.Name = "group3";
            this.group3.TabStop = false;
            // 
            // colaligncenter
            // 
            this.colaligncenter.Checked = true;
            resources.ApplyResources(this.colaligncenter, "colaligncenter");
            this.colaligncenter.Name = "colaligncenter";
            this.colaligncenter.TabStop = true;
            this.colaligncenter.CheckedChanged += new System.EventHandler(this.caCenterclick);
            // 
            // colalignright
            // 
            resources.ApplyResources(this.colalignright, "colalignright");
            this.colalignright.Name = "colalignright";
            this.colalignright.CheckedChanged += new System.EventHandler(this.caRightClick);
            // 
            // colalignleft
            // 
            resources.ApplyResources(this.colalignleft, "colalignleft");
            this.colalignleft.Name = "colalignleft";
            this.colalignleft.CheckedChanged += new System.EventHandler(this.caLeftClick);
            // 
            // group4
            // 
            this.group4.Controls.Add(this.tablelinestylesolid);
            this.group4.Controls.Add(this.tablelinestyledashed);
            this.group4.Controls.Add(this.tablelinestylenone);
            this.group4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group4, "group4");
            this.group4.Name = "group4";
            this.group4.TabStop = false;
            // 
            // tablelinestylesolid
            // 
            resources.ApplyResources(this.tablelinestylesolid, "tablelinestylesolid");
            this.tablelinestylesolid.Name = "tablelinestylesolid";
            this.tablelinestylesolid.CheckedChanged += new System.EventHandler(this.tlsolidClick);
            // 
            // tablelinestyledashed
            // 
            resources.ApplyResources(this.tablelinestyledashed, "tablelinestyledashed");
            this.tablelinestyledashed.Name = "tablelinestyledashed";
            this.tablelinestyledashed.CheckedChanged += new System.EventHandler(this.tlDashedClick);
            // 
            // tablelinestylenone
            // 
            this.tablelinestylenone.Checked = true;
            resources.ApplyResources(this.tablelinestylenone, "tablelinestylenone");
            this.tablelinestylenone.Name = "tablelinestylenone";
            this.tablelinestylenone.TabStop = true;
            this.tablelinestylenone.CheckedChanged += new System.EventHandler(this.tlNoneClick);
            // 
            // rowspacing
            // 
            this.rowspacing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.rowspacing, "rowspacing");
            this.rowspacing.Name = "rowspacing";
            this.rowspacing.TextChanged += new System.EventHandler(this.rspacingchanged);
            // 
            // group5
            // 
            this.group5.Controls.Add(this.collinestylesolid);
            this.group5.Controls.Add(this.collinestyledashed);
            this.group5.Controls.Add(this.collinestylenone);
            this.group5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group5, "group5");
            this.group5.Name = "group5";
            this.group5.TabStop = false;
            // 
            // collinestylesolid
            // 
            resources.ApplyResources(this.collinestylesolid, "collinestylesolid");
            this.collinestylesolid.Name = "collinestylesolid";
            this.collinestylesolid.CheckedChanged += new System.EventHandler(this.clSolidClick);
            // 
            // collinestyledashed
            // 
            resources.ApplyResources(this.collinestyledashed, "collinestyledashed");
            this.collinestyledashed.Name = "collinestyledashed";
            this.collinestyledashed.CheckedChanged += new System.EventHandler(this.clDashedClick);
            // 
            // collinestylenone
            // 
            this.collinestylenone.Checked = true;
            resources.ApplyResources(this.collinestylenone, "collinestylenone");
            this.collinestylenone.Name = "collinestylenone";
            this.collinestylenone.TabStop = true;
            this.collinestylenone.CheckedChanged += new System.EventHandler(this.clNoneClick);
            // 
            // colSpacing_
            // 
            this.colSpacing_.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.colSpacing_, "colSpacing_");
            this.colSpacing_.Name = "colSpacing_";
            this.colSpacing_.TextChanged += new System.EventHandler(this.cspacingChanged);
            // 
            // group6
            // 
            this.group6.Controls.Add(this.tablelinesolid);
            this.group6.Controls.Add(this.tablelinedashed);
            this.group6.Controls.Add(this.tablelinenone);
            this.group6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group6, "group6");
            this.group6.Name = "group6";
            this.group6.TabStop = false;
            // 
            // tablelinesolid
            // 
            resources.ApplyResources(this.tablelinesolid, "tablelinesolid");
            this.tablelinesolid.Name = "tablelinesolid";
            this.tablelinesolid.CheckedChanged += new System.EventHandler(this.tlSolidChecked);
            // 
            // tablelinedashed
            // 
            resources.ApplyResources(this.tablelinedashed, "tablelinedashed");
            this.tablelinedashed.Name = "tablelinedashed";
            this.tablelinedashed.CheckedChanged += new System.EventHandler(this.tlDashedChecked);
            // 
            // tablelinenone
            // 
            this.tablelinenone.Checked = true;
            resources.ApplyResources(this.tablelinenone, "tablelinenone");
            this.tablelinenone.Name = "tablelinenone";
            this.tablelinenone.TabStop = true;
            this.tablelinenone.CheckedChanged += new System.EventHandler(this.tlNoneChecked);
            // 
            // tablespacing
            // 
            resources.ApplyResources(this.tablespacing, "tablespacing");
            this.tablespacing.Name = "tablespacing";
            this.tablespacing.TextChanged += new System.EventHandler(this.tspacingChanged);
            // 
            // equalCols
            // 
            resources.ApplyResources(this.equalCols, "equalCols");
            this.equalCols.Name = "equalCols";
            this.equalCols.CheckedChanged += new System.EventHandler(this.EqualColsChanged);
            // 
            // equalRows
            // 
            resources.ApplyResources(this.equalRows, "equalRows");
            this.equalRows.Name = "equalRows";
            this.equalRows.CheckedChanged += new System.EventHandler(this.EqualRowsCheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Click += new System.EventHandler(this.OnCancel);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.OnOk);
            // 
            // tabcontrol
            // 
            this.tabcontrol.Controls.Add(this.tableTab);
            this.tabcontrol.Controls.Add(this.rowstab);
            this.tabcontrol.Controls.Add(this.colsTab);
            this.tabcontrol.Controls.Add(this.cellsTab);
            resources.ApplyResources(this.tabcontrol, "tabcontrol");
            this.tabcontrol.Name = "tabcontrol";
            this.tabcontrol.SelectedIndex = 0;
            this.tabcontrol.SelectedIndexChanged += new System.EventHandler(this.TabChange);
            // 
            // tableTab
            // 
            this.tableTab.Controls.Add(this.group7);
            this.tableTab.Controls.Add(this.group6);
            this.tableTab.Controls.Add(this.group1);
            resources.ApplyResources(this.tableTab, "tableTab");
            this.tableTab.Name = "tableTab";
            // 
            // group7
            // 
            this.group7.Controls.Add(this.tablespacing);
            this.group7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group7, "group7");
            this.group7.Name = "group7";
            this.group7.TabStop = false;
            // 
            // rowstab
            // 
            this.rowstab.Controls.Add(this.group8);
            this.rowstab.Controls.Add(this.group9);
            this.rowstab.Controls.Add(this.group4);
            this.rowstab.Controls.Add(this.group2);
            resources.ApplyResources(this.rowstab, "rowstab");
            this.rowstab.Name = "rowstab";
            // 
            // group8
            // 
            this.group8.Controls.Add(this.rowspacing);
            this.group8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group8, "group8");
            this.group8.Name = "group8";
            this.group8.TabStop = false;
            // 
            // group9
            // 
            this.group9.Controls.Add(this.equalRows);
            this.group9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group9, "group9");
            this.group9.Name = "group9";
            this.group9.TabStop = false;
            // 
            // colsTab
            // 
            this.colsTab.Controls.Add(this.group10);
            this.colsTab.Controls.Add(this.group11);
            this.colsTab.Controls.Add(this.group5);
            this.colsTab.Controls.Add(this.group3);
            resources.ApplyResources(this.colsTab, "colsTab");
            this.colsTab.Name = "colsTab";
            // 
            // group10
            // 
            this.group10.Controls.Add(this.colSpacing_);
            this.group10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group10, "group10");
            this.group10.Name = "group10";
            this.group10.TabStop = false;
            // 
            // group11
            // 
            this.group11.Controls.Add(this.equalCols);
            this.group11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group11, "group11");
            this.group11.Name = "group11";
            this.group11.TabStop = false;
            // 
            // cellsTab
            // 
            this.cellsTab.Controls.Add(this.group12);
            this.cellsTab.Controls.Add(this.group13);
            resources.ApplyResources(this.cellsTab, "cellsTab");
            this.cellsTab.Name = "cellsTab";
            // 
            // group12
            // 
            this.group12.Controls.Add(this.rowalignaxis_);
            this.group12.Controls.Add(this.rowalignbaseline_);
            this.group12.Controls.Add(this.rowaligncenter_);
            this.group12.Controls.Add(this.rowalignbottom_);
            this.group12.Controls.Add(this.rowaligntop_);
            this.group12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group12, "group12");
            this.group12.Name = "group12";
            this.group12.TabStop = false;
            // 
            // rowalignaxis_
            // 
            resources.ApplyResources(this.rowalignaxis_, "rowalignaxis_");
            this.rowalignaxis_.Name = "rowalignaxis_";
            this.rowalignaxis_.CheckedChanged += new System.EventHandler(this.raAxisChecked);
            // 
            // rowalignbaseline_
            // 
            this.rowalignbaseline_.Checked = true;
            resources.ApplyResources(this.rowalignbaseline_, "rowalignbaseline_");
            this.rowalignbaseline_.Name = "rowalignbaseline_";
            this.rowalignbaseline_.TabStop = true;
            this.rowalignbaseline_.CheckedChanged += new System.EventHandler(this.raBaselineChecked);
            // 
            // rowaligncenter_
            // 
            resources.ApplyResources(this.rowaligncenter_, "rowaligncenter_");
            this.rowaligncenter_.Name = "rowaligncenter_";
            this.rowaligncenter_.CheckedChanged += new System.EventHandler(this.raCenterChecked);
            // 
            // rowalignbottom_
            // 
            resources.ApplyResources(this.rowalignbottom_, "rowalignbottom_");
            this.rowalignbottom_.Name = "rowalignbottom_";
            this.rowalignbottom_.CheckedChanged += new System.EventHandler(this.raBottomChecked);
            // 
            // rowaligntop_
            // 
            resources.ApplyResources(this.rowaligntop_, "rowaligntop_");
            this.rowaligntop_.Name = "rowaligntop_";
            this.rowaligntop_.CheckedChanged += new System.EventHandler(this.raTopChecked);
            // 
            // group13
            // 
            this.group13.Controls.Add(this.raligncenter);
            this.group13.Controls.Add(this.rowalignright);
            this.group13.Controls.Add(this.rowalignleft);
            this.group13.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.group13, "group13");
            this.group13.Name = "group13";
            this.group13.TabStop = false;
            // 
            // raligncenter
            // 
            this.raligncenter.Checked = true;
            resources.ApplyResources(this.raligncenter, "raligncenter");
            this.raligncenter.Name = "raligncenter";
            this.raligncenter.TabStop = true;
            this.raligncenter.CheckedChanged += new System.EventHandler(this.rCenterCheck);
            // 
            // rowalignright
            // 
            resources.ApplyResources(this.rowalignright, "rowalignright");
            this.rowalignright.Name = "rowalignright";
            this.rowalignright.CheckedChanged += new System.EventHandler(this.raRightChecked);
            // 
            // rowalignleft
            // 
            resources.ApplyResources(this.rowalignleft, "rowalignleft");
            this.rowalignleft.Name = "rowalignleft";
            this.rowalignleft.CheckedChanged += new System.EventHandler(this.raLeftChecked);
            // 
            // selAllbutton
            // 
            this.selAllbutton.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.selAllbutton, "selAllbutton");
            this.selAllbutton.Name = "selAllbutton";
            this.selAllbutton.Click += new System.EventHandler(this.SelectAllClick);
            // 
            // MatrixPropertiesDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tabcontrol);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.selAllbutton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatrixPropertiesDialog";
            this.group1.ResumeLayout(false);
            this.group2.ResumeLayout(false);
            this.group3.ResumeLayout(false);
            this.group4.ResumeLayout(false);
            this.group5.ResumeLayout(false);
            this.group6.ResumeLayout(false);
            this.tabcontrol.ResumeLayout(false);
            this.tableTab.ResumeLayout(false);
            this.group7.ResumeLayout(false);
            this.group7.PerformLayout();
            this.rowstab.ResumeLayout(false);
            this.group8.ResumeLayout(false);
            this.group8.PerformLayout();
            this.group9.ResumeLayout(false);
            this.colsTab.ResumeLayout(false);
            this.group10.ResumeLayout(false);
            this.group10.PerformLayout();
            this.group11.ResumeLayout(false);
            this.cellsTab.ResumeLayout(false);
            this.group12.ResumeLayout(false);
            this.group13.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public void SetTarget(Node node)
        {
            this.center = new Point(this.bWidth + this.canvasWidth, this.tabcontrol.Height + 30);
            if (node != null)
            {
                this.matrix = new MTable(node);
                this.numRows = this.matrix.RowCount;
                this.numCols = this.matrix.ColCount;
            }
            if (node != null)
            {
                this.CreateButtons();
                for (int i = 0; i < this.numRows; i++)
                {
                    for (int j = 0; j < this.numCols; j++)
                    {
                        this.buttons[i][j].Visible = false;
                        this.buttons[i][j].FlatStyle = FlatStyle.Flat;
                    }
                }
                this.RecalcButtons();
                this.SelectTableTab();
                if (this.matrix.align == TableAlign.TOP)
                {
                    this.tablealigntop_.Checked = true;
                }
                else if (this.matrix.align == TableAlign.BOTTOM)
                {
                    this.tablealignbottom.Checked = true;
                }
                else if (this.matrix.align == TableAlign.CENTER)
                {
                    this.tablealigncenter.Checked = true;
                }
                else if (this.matrix.align == TableAlign.BASELINE)
                {
                    this.tablealignbaseline.Checked = true;
                }
                else if (this.matrix.align == TableAlign.AXIS)
                {
                    this.tablealignAxis.Checked = true;
                }
                if (this.matrix.frame == TableLineStyle.NONE)
                {
                    this.tablelinenone.Checked = true;
                }
                else if (this.matrix.frame == TableLineStyle.SOLID)
                {
                    this.tablelinesolid.Checked = true;
                }
                else if (this.matrix.frame == TableLineStyle.DASHED)
                {
                    this.tablelinedashed.Checked = true;
                }
                this.equalRows.Checked = this.matrix.equalRows;
                this.equalCols.Checked = this.matrix.equalColumns;
                this.tablespacing.Text = this.matrix.framespacing;
            }
        }

        private void taCenterClick(object sender, EventArgs e)
        {
            this.SetTableAlign();
        }

        private void taBaselineClick(object sender, EventArgs e)
        {
            this.SetTableAlign();
        }

        private void taAxisClick(object sender, EventArgs e)
        {
            this.SetTableAlign();
        }

        private void SetTableAlign()
        {
            if (this.tablealigntop_.Checked)
            {
                this.matrix.align = TableAlign.TOP;
            }
            else if (this.tablealigncenter.Checked)
            {
                this.matrix.align = TableAlign.CENTER;
            }
            else if (this.tablealignAxis.Checked)
            {
                this.matrix.align = TableAlign.AXIS;
            }
            else if (this.tablealignbaseline.Checked)
            {
                this.matrix.align = TableAlign.BASELINE;
            }
            else if (this.tablealignbottom.Checked)
            {
                this.matrix.align = TableAlign.BOTTOM;
            }
        }

        private void EqualRowsCheckedChanged(object sender, EventArgs e)
        {
            this.matrix.equalRows = this.equalRows.Checked;
        }

        private void raTopClick(object sender, EventArgs e)
        {
            this.SetRowAlign();
        }

        private void raBottomClick(object sender, EventArgs e)
        {
            this.SetRowAlign();
        }

        private void raCenterClick(object sender, EventArgs e)
        {
            this.SetRowAlign();
        }

        private void raBaselineClick(object sender, EventArgs e)
        {
            this.SetRowAlign();
        }

        private void raaxisClick(object sender, EventArgs e)
        {
            this.SetRowAlign();
        }

        private void TabChange(object sender, EventArgs e)
        {
            switch (this.tabcontrol.SelectedIndex)
            {
                case 0:
                {
                    if (this.matrix.selKind_ != TableCellKind.SelAll)
                    {
                        this.SelectTableTab();
                    }
                    break;
                }
                case 1:
                {
                    if (this.matrix.selKind_ != TableCellKind.RowSelected)
                    {
                        this.SelectRowTab(0);
                    }
                    break;
                }
                case 2:
                {
                    if (this.matrix.selKind_ != TableCellKind.ColSelected)
                    {
                        this.SelectColTab(0);
                    }
                    break;
                }
                case 3:
                {
                    if ((this.matrix.selKind_ != TableCellKind.RowColSelected) && (this.matrix.selKind_ != TableCellKind.BottomSelected))
                    {
                        this.SelectCellTab(0, 0);
                    }
                    break;
                }
            }
            this.Recheck();
        }

        private void SetRowAlign()
        {
            if (this.rowaligntop.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.TOP);
            }
            else if (this.rowaligncenter.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.CENTER);
            }
            else if (this.rowalignbaseline.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.BASELINE);
            }
            else if (this.rowalignaxis.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.AXIS);
            }
            else if (this.rowalignbottom.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.BOTTOM);
            }
            this.RecalcButtons();
        }

        private void rspacingchanged(object sender, EventArgs e)
        {
            this.matrix.SetRowSpacing(this.rowspacing.Text);
        }

        private void tlNoneClick(object sender, EventArgs e)
        {
            this.SetLineStyle();
        }

        private void tlsolidClick(object sender, EventArgs e)
        {
            this.SetLineStyle();
        }

        private void tlDashedClick(object sender, EventArgs e)
        {
            this.SetLineStyle();
        }

        private void SetLineStyle()
        {
            if (this.tablelinestylenone.Checked)
            {
                this.matrix.SetLineStyle(TableLineStyle.NONE);
            }
            else if (this.tablelinestylesolid.Checked)
            {
                this.matrix.SetLineStyle(TableLineStyle.SOLID);
            }
            else if (this.tablelinestyledashed.Checked)
            {
                this.matrix.SetLineStyle(TableLineStyle.DASHED);
            }
            base.Invalidate();
        }

        private void EqualColsChanged(object sender, EventArgs e)
        {
            this.matrix.equalColumns = this.equalCols.Checked;
        }

        private void caLeftClick(object sender, EventArgs e)
        {
            this.SetColAlign();
        }

        private void caCenterclick(object sender, EventArgs e)
        {
            this.SetColAlign();
        }

        private void caRightClick(object sender, EventArgs e)
        {
            this.SetColAlign();
        }

        private void tspacingChanged(object sender, EventArgs e)
        {
            this.matrix.framespacing = this.tablespacing.Text;
        }

        private void SetColAlign()
        {
            if (this.colalignleft.Checked)
            {
                this.matrix.SetColAlign(HAlign.LEFT);
            }
            else if (this.colaligncenter.Checked)
            {
                this.matrix.SetColAlign(HAlign.CENTER);
            }
            else if (this.colalignright.Checked)
            {
                this.matrix.SetColAlign(HAlign.RIGHT);
            }
            this.RecalcButtons();
        }

        private void cspacingChanged(object sender, EventArgs e)
        {
            this.matrix.SetColSpacing(this.colSpacing_.Text);
        }

        private void clNoneClick(object sender, EventArgs e)
        {
            this.SetTableLineStyle();
        }

        private void clSolidClick(object sender, EventArgs e)
        {
            this.SetTableLineStyle();
        }

        private void clDashedClick(object sender, EventArgs e)
        {
            this.SetTableLineStyle();
        }

        private void SetTableLineStyle()
        {
            if (this.collinestylenone.Checked)
            {
                this.matrix.SetTableLineStyle(TableLineStyle.NONE);
            }
            else if (this.collinestylesolid.Checked)
            {
                this.matrix.SetTableLineStyle(TableLineStyle.SOLID);
            }
            else if (this.collinestyledashed.Checked)
            {
                this.matrix.SetTableLineStyle(TableLineStyle.DASHED);
            }
            base.Invalidate();
        }

        private void raLeftChecked(object sender, EventArgs e)
        {
            this.SetRowColAlign();
        }

        private void rCenterCheck(object sender, EventArgs e)
        {
            this.SetRowColAlign();
        }

        private void raRightChecked(object sender, EventArgs e)
        {
            this.SetRowColAlign();
        }

        private void SetRowColAlign()
        {
            if (this.rowalignleft.Checked)
            {
                this.matrix.SetColAlign(HAlign.LEFT);
            }
            else if (this.raligncenter.Checked)
            {
                this.matrix.SetColAlign(HAlign.CENTER);
            }
            else if (this.rowalignright.Checked)
            {
                this.matrix.SetColAlign(HAlign.RIGHT);
            }
            this.RecalcButtons();
        }

        private void tlNoneChecked(object sender, EventArgs e)
        {
            this.SetFrame();
        }

        private void raTopChecked(object sender, EventArgs e)
        {
            this.PropogateRowAlign();
        }

        private void raBottomChecked(object sender, EventArgs e)
        {
            this.PropogateRowAlign();
        }

        private void raCenterChecked(object sender, EventArgs e)
        {
            this.PropogateRowAlign();
        }

        private void raBaselineChecked(object sender, EventArgs e)
        {
            this.PropogateRowAlign();
        }

        private void raAxisChecked(object sender, EventArgs e)
        {
            this.PropogateRowAlign();
        }

        private void PropogateRowAlign()
        {
            if (this.rowaligntop_.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.TOP);
            }
            else if (this.rowaligncenter_.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.CENTER);
            }
            else if (this.rowalignbaseline_.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.BASELINE);
            }
            else if (this.rowalignaxis_.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.AXIS);
            }
            else if (this.rowalignbottom_.Checked)
            {
                this.matrix.SetRowAlign(RowAlign.BOTTOM);
            }
            this.RecalcButtons();
        }

        private void SelectAllClick(object sender, EventArgs e)
        {
            this.SelectTableTab();
        }

        private void WholeRowClick(object sender, EventArgs e)
        {
            this.SelectRowTab(((MathTableButton) sender).row);
            this.Recheck();
        }

        private void WholeColClick(object sender, EventArgs e)
        {
            this.SelectColTab(((MathTableButton) sender).col);
            this.Recheck();
        }

        private void ButtonsClick(object sender, EventArgs e)
        {
            this.SelectCellTab(((MathTableButton) sender).row, ((MathTableButton) sender).col);
            this.Recheck();
        }

        private void tlSolidChecked(object sender, EventArgs e)
        {
            this.SetFrame();
        }

        private void bottomsClick(object sender, EventArgs e)
        {
            int col = ((MathTableButton) sender).col;
            int row = ((MathTableButton) sender).row;
            this.SelectCellTab(row, col);
            this.Recheck();
        }

        private void SelectTableTab()
        {
            this.matrix.SelectAll();
            this.tabcontrol.SelectedIndex = 0;
            this.RecalcButtons();
        }

        private void SelectRowTab(int row)
        {
            this.matrix.SetCurRow(row);
            this.tabcontrol.SelectedIndex = 1;
            this.RecalcButtons();
        }

        private void SelectColTab(int col)
        {
            this.matrix.SetCurCol(col);
            this.tabcontrol.SelectedIndex = 2;
            this.RecalcButtons();
        }

        private void SelectCellTab(int row, int col)
        {
            this.matrix.SetCurRowCol(row, col);
            this.RecalcButtons();
            this.tabcontrol.SelectedIndex = 3;
        }

        private void CreateButtons()
        {
            int hhh = 0;
            int hh = 0;
            hh = (base.ClientRectangle.Height - (this.center.Y - this.bHeight)) - 30;
            hhh = (this.center.Y + (this.numRows * (this.bHeight + this.canvasWidth))) - (this.center.Y - this.bHeight);
            if (hhh <= hh)
            {
                while (hhh < (hh - 10))
                {
                    base.Height -= 10;
                    hh = (base.ClientRectangle.Height - (this.center.Y - this.bHeight)) - 30;
                }
                this.okButton.Location = new Point((((base.ClientRectangle.Width - this.cancelButton.Width) - this.okButton.Width) - 10) - 10, (base.ClientRectangle.Height - this.okButton.Height) - 10);
                this.cancelButton.Location = new Point((base.ClientRectangle.Width - this.cancelButton.Width) - 10, (base.ClientRectangle.Height - this.okButton.Height) - 10);
            }
            else
            {
                while (hhh > hh)
                {
                    this.bHeight -= 2;
                    this.bWidth -= 2;
                    this.canvasWidth--;
                    hhh = (this.center.Y + (this.numRows * (this.bHeight + this.canvasWidth))) - (this.center.Y - this.bHeight);
                }
            }
            this.selAllbutton.Location = new Point(this.center.X - this.bWidth, this.center.Y - this.bHeight);
            this.selAllbutton.Size = new Size((this.bWidth * 2) / 3, (this.bHeight * 2) / 3);
            this.bottoms = new MathTableButton[this.numRows];
            this.buttons = new MathTableButton[this.numRows][];
            this.rowButtons = new MathTableButton[this.numRows];
            this.colButtons = new MathTableButton[this.numCols];
            for (int i = 0; i < this.numRows; i++)
            {
                this.buttons[i] = new MathTableButton[this.numCols];
            }
            for (int i = 0; i < this.numCols; i++)
            {
                this.colButtons[i] = new MathTableButton();
                this.colButtons[i].Location = new Point(this.center.X + (i * (this.bWidth + this.canvasWidth)), this.center.Y - this.bHeight);
                this.colButtons[i].Name = "Col_" + i.ToString() + "";
                this.colButtons[i].Size = new Size(this.bWidth, (this.bHeight * 2) / 3);
                this.colButtons[i].col = i;
                this.colButtons[i].Click += new EventHandler(this.WholeColClick);
                base.Controls.AddRange(new Control[] { this.colButtons[i] });
                this.colButtons[i].FlatStyle = FlatStyle.Flat;
            }
            for (int i = 0; i < this.numRows; i++)
            {
                this.rowButtons[i] = new MathTableButton();
                this.rowButtons[i].Location = new Point(this.center.X - this.bWidth, this.center.Y + (i * (this.bHeight + this.canvasWidth)));
                this.rowButtons[i].Name = "Row_" + i.ToString() + "";
                this.rowButtons[i].Size = new Size((this.bWidth * 2) / 3, this.bHeight);
                this.rowButtons[i].row = i;
                this.rowButtons[i].Click += new EventHandler(this.WholeRowClick);
                base.Controls.AddRange(new Control[] { this.rowButtons[i] });
                this.rowButtons[i].FlatStyle = FlatStyle.Flat;

                this.bottoms[i] = new MathTableButton();
                this.bottoms[i].row = i;
                this.bottoms[i].col = this.matrix.ColCount;
                this.bottoms[i].BackColor = SystemColors.ControlLightLight;
                this.bottoms[i].FlatStyle = FlatStyle.Flat;
                this.bottoms[i].Location = new Point(this.buttonsOrigin.X + (this.numCols * 0x22), this.buttonsOrigin.Y + (i * 0x22));
                this.bottoms[i].Name = string.Concat(new string[] { "Cell_", i.ToString(), "_", this.numCols.ToString(), "" });
                this.bottoms[i].Size = new Size(0x18, 0x18);
                this.bottoms[i].Click += new EventHandler(this.bottomsClick);
                base.Controls.AddRange(new Control[] { this.bottoms[i] });
                this.bottoms[i].FlatStyle = FlatStyle.Flat;

                for (int j = 0; j < this.numCols; j++)
                {
                    this.buttons[i][j] = new MathTableButton();
                    this.buttons[i][j].row = i;
                    this.buttons[i][j].col = j;
                    this.buttons[i][j].BackColor = SystemColors.ControlLightLight;
                    this.buttons[i][j].FlatStyle = FlatStyle.Flat;
                    this.buttons[i][j].Location = new Point(this.buttonsOrigin.X + (j * 0x22), this.buttonsOrigin.Y + (i * 0x22));
                    this.buttons[i][j].Name = string.Concat(new string[] { "Cell_", i.ToString(), "_", j.ToString(), "" });
                    this.buttons[i][j].Size = new Size(0x18, 0x18);
                    this.buttons[i][j].Click += new EventHandler(this.ButtonsClick);
                    this.buttons[i][j].FlatStyle = FlatStyle.Flat;
                    base.Controls.AddRange(new Control[] { this.buttons[i][j] });
                }
            }
        }

        private void RecalcButtons()
        {
            for (int i = 0; i < this.numRows; i++)
            {
                this.bottoms[i].BackColor = Color.LightGray;
                for (int j = 0; j < this.numCols; j++)
                {
                    this.buttons[i][j].BackColor = Color.LightGray;
                }
            }
            switch (this.matrix.selKind_)
            {
                case TableCellKind.SelAll:
                {
                    for (int i = 0; i < this.numRows; i++)
                    {
                        for (int j = 0; j < this.numCols; j++)
                        {
                            this.buttons[i][j].BackColor = Color.LightBlue;
                        }
                    }
                    break;
                }
                case TableCellKind.RowSelected:
                {
                    for (int i = 0; i < this.numCols; i++)
                    {
                        this.buttons[this.matrix.curRow][i].BackColor = Color.LightBlue;
                    }
                    break;
                }
                case TableCellKind.ColSelected:
                {
                    for (int i = 0; i < this.numRows; i++)
                    {
                        this.buttons[i][this.matrix.curCol].BackColor = Color.LightBlue;
                    }
                    break;
                }
                case TableCellKind.RowColSelected:
                {
                    if (this.matrix.curCol >= this.matrix.ColCount)
                    {
                        if (this.matrix.curCol == this.matrix.ColCount)
                        {
                            this.bottoms[this.matrix.curRow].BackColor = Color.LightBlue;
                        }
                        break;
                    }
                    this.buttons[this.matrix.curRow][this.matrix.curCol].BackColor = Color.LightBlue;
                    break;
                }
                case TableCellKind.BottomSelected:
                {
                    this.bottoms[this.matrix.curRow].BackColor = Color.LightBlue;
                    break;
                }
            }
            for (int i = 0; i < this.matrix.RowCount; i++)
            {
                MRow row = this.matrix.GetRow(i);
                if (!row.isLabeled)
                {
                    this.bottoms[i].Visible = false;
                    continue;
                }
                this.bottoms[i].Visible = true;
                MCell cell = row.cell;
                int hv = 0;
                int rv = 0;
                HAlign hAlign = cell.GetColAlign();
                RowAlign rowAlign = cell.GetRowAlign();
                switch (hAlign)
                {
                    case HAlign.LEFT:
                    {
                        hv = -this.canvasWidth / 2;
                        break;
                    }
                    case HAlign.CENTER:
                    {
                        hv = 0;
                        break;
                    }
                    case HAlign.RIGHT:
                        hv = this.canvasWidth / 2;
                        break;
                }
            
                switch (rowAlign)
                {
                    case RowAlign.TOP:
                    {
                        rv = -this.canvasWidth / 2;
                        break;
                    }
                    case RowAlign.BOTTOM:
                        rv = this.canvasWidth / 2;
                        break;

                    case RowAlign.CENTER:
                    case RowAlign.BASELINE:
                    case RowAlign.AXIS:
                    {
                        rv = 0;
                        break;
                    }
                }
            
                this.bottoms[i].Location = new Point(((this.center.X + (cell.colSpan * (this.bWidth + this.canvasWidth))) + hv) + 30, (this.center.Y + (i * (this.bHeight + this.canvasWidth))) + rv);
                if (cell.tableAttrs != null)
                {
                    this.bottoms[i].Size = new Size((this.bWidth * cell.tableAttrs.columnSpan) + (this.canvasWidth * (cell.tableAttrs.columnSpan - 1)), (this.bHeight * cell.tableAttrs.rowSpan) + (this.canvasWidth * (cell.tableAttrs.rowSpan - 1)));
                }
                else
                {
                    this.bottoms[i].Size = new Size(this.bWidth, this.bHeight);
                }
            }
            for (int i = 0; i < this.matrix.RowCount; i++)
            {
                MRow row = this.matrix.GetRow(i);
                for (int j = 0; j < row.Count; j++)
                {
                    MCell cell = row.Get(j);
                    this.buttons[i][cell.colSpan].Visible = true;
                    int halignV = 0;
                    int ralignV = 0;
                    HAlign hAlign = cell.GetColAlign();
                    RowAlign rowAlign = cell.GetRowAlign();
                    switch (hAlign)
                    {
                        case HAlign.LEFT:
                        {
                            halignV = -this.canvasWidth / 2;
                            break;
                        }
                        case HAlign.CENTER:
                        {
                            halignV = 0;
                            break;
                        }
                        case HAlign.RIGHT:
                            halignV = this.canvasWidth / 2;
                            break;
                    }
                
                    switch (rowAlign)
                    {
                        case RowAlign.TOP:
                        {
                            ralignV = -this.canvasWidth / 2;
                            break;
                        }
                        case RowAlign.BOTTOM:
                            ralignV = this.canvasWidth / 2;
                            break;

                        case RowAlign.CENTER:
                        case RowAlign.BASELINE:
                        case RowAlign.AXIS:
                        {
                            ralignV = 0;
                            break;
                        }
                    }
                
                    this.buttons[i][cell.colSpan].Location = new Point((this.center.X + (cell.colSpan * (this.bWidth + this.canvasWidth))) + halignV, (this.center.Y + (i * (this.bHeight + this.canvasWidth))) + ralignV);
                    if (cell.tableAttrs != null)
                    {
                        this.buttons[i][cell.colSpan].Size = new Size((this.bWidth * cell.tableAttrs.columnSpan) + (this.canvasWidth * (cell.tableAttrs.columnSpan - 1)), (this.bHeight * cell.tableAttrs.rowSpan) + (this.canvasWidth * (cell.tableAttrs.rowSpan - 1)));
                    }
                    else
                    {
                        this.buttons[i][cell.colSpan].Size = new Size(this.bWidth, this.bHeight);
                    }
                }
            }
        }

        private void Recheck()
        {
            bool fpouind;
            MCell lrow;
            switch (this.matrix.selKind_)
            {
                case TableCellKind.SelAll:
                    return;

                case TableCellKind.RowSelected:
                {
                    int cRow = this.matrix.curRow;
                    RowAlign rowAlign = RowAlign.UNKNOWN;
                    bool changed = true;
                    MRow row = this.matrix.GetRow(cRow);
                    for (int i = 0; i < row.cells.Count; i++)
                    {
                        MCell cell = this.matrix.Get(cRow, i);
                        if (cell != null)
                        {
                            if ((i > 0) && (cell.rowAlign != rowAlign))
                            {
                                changed = false;
                            }
                            rowAlign = cell.rowAlign;
                        }
                    }
                    if (changed)
                    {
                        if (rowAlign == RowAlign.TOP)
                        {
                            this.rowaligntop.Checked = true;
                        }
                        else if (rowAlign == RowAlign.CENTER)
                        {
                            this.rowaligncenter.Checked = true;
                        }
                        else if (rowAlign == RowAlign.BASELINE)
                        {
                            this.rowalignbaseline.Checked = true;
                        }
                        else if (rowAlign == RowAlign.AXIS)
                        {
                            this.rowalignaxis.Checked = true;
                        }
                        else if (rowAlign == RowAlign.BOTTOM)
                        {
                            this.rowalignbottom.Checked = true;
                        }
                    }
                    else
                    {
                        this.rowaligntop.Checked = false;
                        this.rowaligncenter.Checked = false;
                        this.rowalignbaseline.Checked = false;
                        this.rowalignaxis.Checked = false;
                        this.rowalignbottom.Checked = false;
                    }
                    this.rowspacing.Text = row.spacing;
                    switch (row.lines)
                    {
                        case TableLineStyle.NONE:
                        {
                            this.tablelinestylenone.Checked = true;
                            return;
                        }
                        case TableLineStyle.SOLID:
                        {
                            this.tablelinestylesolid.Checked = true;
                            return;
                        }
                        case TableLineStyle.DASHED:
                        {
                            this.tablelinestyledashed.Checked = true;
                            return;
                        }
                    }
                    return;
                }
                case TableCellKind.ColSelected:
                {
                    int ccol = this.matrix.curCol;
                    HAlign columnAlign = HAlign.UNKNOWN;
                    bool need = true;
                    for (int i = 0; i < this.numRows; i++)
                    {
                        MCell cell = this.matrix.Get(i, ccol);
                        if (cell != null)
                        {
                            if ((i > 0) && (cell.columnAlign != columnAlign))
                            {
                                need = false;
                            }
                            columnAlign = cell.columnAlign;
                        }
                    }
                    if (need)
                    {
                        if (columnAlign == HAlign.LEFT)
                        {
                            this.colalignleft.Checked = true;
                        }
                        else if (columnAlign == HAlign.CENTER)
                        {
                            this.colaligncenter.Checked = true;
                        }
                        else if (columnAlign == HAlign.RIGHT)
                        {
                            this.colalignright.Checked = true;
                        }
                    }
                    else
                    {
                        this.colalignleft.Checked = false;
                        this.colaligncenter.Checked = false;
                        this.colalignright.Checked = false;
                    }
                    this.colSpacing_.Text = this.matrix.GetColSpacing(ccol);
                    switch (this.matrix.GetTableLineStyle(ccol))
                    {
                        case TableLineStyle.NONE:
                        {
                            this.collinestylenone.Checked = true;
                            return;
                        }
                        case TableLineStyle.SOLID:
                        {
                            this.collinestylesolid.Checked = true;
                            return;
                        }
                        case TableLineStyle.DASHED:
                        {
                            this.collinestyledashed.Checked = true;
                            return;
                        }
                    }
                    return;
                }
                case TableCellKind.RowColSelected:
                case TableCellKind.BottomSelected:
                {
                    fpouind = false;
                    lrow = null;
                    if (this.matrix.selKind_ != TableCellKind.RowColSelected)
                    {
                        if (this.matrix.selKind_ == TableCellKind.BottomSelected)
                        {
                            MRow mRow = this.matrix.GetRow(this.matrix.curRow);
                            if (mRow.isLabeled && (mRow.cell != null))
                            {
                                lrow = mRow.cell;
                                fpouind = true;
                            }
                        }
                        break;
                    }
                    lrow = this.matrix.Get(this.matrix.curRow, this.matrix.curCol);
                    fpouind = true;
                    break;
                }
                default:
                    return;
            }
            if (fpouind)
            {
                if (lrow.columnAlign == HAlign.LEFT)
                {
                    this.rowalignleft.Checked = true;
                }
                else if (lrow.columnAlign == HAlign.CENTER)
                {
                    this.raligncenter.Checked = true;
                }
                else if (lrow.columnAlign == HAlign.RIGHT)
                {
                    this.rowalignright.Checked = true;
                }
                if (lrow.rowAlign == RowAlign.TOP)
                {
                    this.rowaligntop_.Checked = true;
                }
                else if (lrow.rowAlign == RowAlign.CENTER)
                {
                    this.rowaligncenter_.Checked = true;
                }
                else if (lrow.rowAlign == RowAlign.BASELINE)
                {
                    this.rowalignbaseline_.Checked = true;
                }
                else if (lrow.rowAlign == RowAlign.AXIS)
                {
                    this.rowalignaxis_.Checked = true;
                }
                else if (lrow.rowAlign == RowAlign.BOTTOM)
                {
                    this.rowalignbottom_.Checked = true;
                }
            }
        }

        private void tlDashedChecked(object sender, EventArgs e)
        {
            this.SetFrame();
        }

        private void OnOk(object sender, EventArgs e)
        {
            this.Success = true;
            base.Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.Success = false;
            base.Close();
        }

        private void SetFrame()
        {
            if (this.tablelinenone.Checked)
            {
                this.matrix.frame = TableLineStyle.NONE;
            }
            else if (this.tablelinesolid.Checked)
            {
                this.matrix.frame = TableLineStyle.SOLID;
            }
            else if (this.tablelinedashed.Checked)
            {
                this.matrix.frame = TableLineStyle.DASHED;
            }
            base.Invalidate();
        }

        private void tatopClick(object sender, EventArgs e)
        {
            this.SetTableAlign();
        }

        private void taBottomClick(object sender, EventArgs e)
        {
            this.SetTableAlign();
        }


        public bool Success
        {
            get
            {
                return this.success_;
            }
            set
            {
                this.success_ = value;
            }
        }


        private GroupBox group1;
        private GroupBox group2;
        private RadioButton rowalignaxis;
        private RadioButton rowalignbaseline;
        private RadioButton rowaligncenter;
        private RadioButton rowalignbottom;
        private RadioButton rowaligntop;
        private RadioButton colaligncenter;
        private RadioButton colalignright;
        private RadioButton colalignleft;
        private RadioButton tablelinestylesolid;
        private RadioButton tablelinestyledashed;
        private GroupBox group3;
        private RadioButton tablelinestylenone;
        private RadioButton collinestylesolid;
        private RadioButton collinestyledashed;
        private RadioButton collinestylenone;
        private GroupBox group6;
        private RadioButton tablelinesolid;
        private RadioButton tablelinedashed;
        private RadioButton tablelinenone;
        private CheckBox equalRows;
        private CheckBox equalCols;
        private GroupBox group4;
        private TextBox rowspacing;
        private TextBox colSpacing_;
        private Glass.GlassButton cancelButton;
        private Glass.GlassButton okButton;
        private MathTableButton[][] buttons;
        private MathTableButton[] bottoms;
        private MathTableButton[] rowButtons;
        private MathTableButton[] colButtons;
        private int numRows;
        private int numCols;
        private GroupBox group5;
        private Point buttonsOrigin;
        private Container container;
        private bool success_;
        private TabControl tabcontrol;
        private TabPage tableTab;
        private TabPage rowstab;
        private TabPage colsTab;
        private TabPage cellsTab;
        private Glass.GlassButton selAllbutton;
        private GroupBox group12;
        private RadioButton tablealigntop_;
        private GroupBox group13;
        private GroupBox group11;
        private GroupBox group9;
        private RadioButton raligncenter;
        private RadioButton rowalignright;
        private RadioButton rowalignleft;
        private RadioButton rowalignaxis_;
        private RadioButton rowalignbaseline_;
        private RadioButton rowaligncenter_;
        private RadioButton rowalignbottom_;
        private RadioButton tablealignbottom;
        private RadioButton rowaligntop_;
        private GroupBox group7;
        private GroupBox group8;
        private GroupBox group10;
        private TextBox tablespacing;
        public MTable matrix;
        private Point center;
        private int bWidth;
        private int bHeight;
        private RadioButton tablealigncenter;
        private int canvasWidth;
        private RadioButton tablealignbaseline;
        private RadioButton tablealignAxis;
    }
}

