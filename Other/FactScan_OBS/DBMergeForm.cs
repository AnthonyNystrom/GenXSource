namespace FacScan
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class DBMergeForm : dataForm
    {
        public DBMergeForm()
        {
            this.Fac = new DataTable("Fac");
            this.Fac2 = new DataTable("Fac2");
            this.components = null;
            this.InitializeComponent();
            base.StartPosition = FormStartPosition.CenterScreen;
            this.dataGrid2.Width = (base.Width - this.dataGrid1.Width) - 120;
        }

        private void btnMoveAllBk_Click(object sender, EventArgs e)
        {
            this.mnuMoveAllBk_Click(this, null);
        }

        private void btnMoveAllTo_Click(object sender, EventArgs e)
        {
            this.mnuMoveAllTo_Click(this, null);
        }

        private void btnMoveBk_Click(object sender, EventArgs e)
        {
            this.mnuMoveBk_Click(this, null);
        }

        private void btnMoveTo_Click(object sender, EventArgs e)
        {
            this.mnuMoveTo_Click(this, null);
        }

        private void dataGrid2_CurrentCellChanged(object sender, EventArgs e)
        {
            base.dataChanged();
        }

        private void DBMergeForm_Closing(object sender, CancelEventArgs e)
        {
            this.mnuCancel_Click(this, null);
        }

        private void DBMergeForm_Load(object sender, EventArgs e)
        {
            base.formTitle = "Build Data Base";
            base.showCaption("New Database");
        }

        private void DBMergeForm_SizeChanged(object sender, EventArgs e)
        {
            this.splitter1_SplitterMoved(this, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGrid2 = new DataGrid();
            this.mainMenu1 = new MainMenu();
            this.mnuLoad = new MenuItem();
            this.mnuMoveTo = new MenuItem();
            this.mnuMoveBk = new MenuItem();
            this.mnuMoveAllTo = new MenuItem();
            this.mnuMoveAllBk = new MenuItem();
            this.mnuSave = new MenuItem();
            this.mnuCancel = new MenuItem();
            this.splitter2 = new Splitter();
            this.ofDlg = new OpenFileDialog();
            this.sfDlg = new SaveFileDialog();
            this.dataGrid1 = new DataGrid();
            this.panel1 = new Panel();
            this.btnMoveTo = new Button();
            this.btnMoveBk = new Button();
            this.btnMoveAllTo = new Button();
            this.btnMoveAllBk = new Button();
            this.splitter1 = new Splitter();
            this.dataGrid2.BeginInit();
            this.dataGrid1.BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.dataGrid2.CaptionBackColor = SystemColors.ActiveBorder;
            this.dataGrid2.CaptionForeColor = SystemColors.Highlight;
            this.dataGrid2.CaptionText = "Destination";
            this.dataGrid2.DataMember = "";
            this.dataGrid2.Dock = DockStyle.Right;
            this.dataGrid2.HeaderForeColor = SystemColors.ControlText;
            this.dataGrid2.Location = new Point(0x218, 0);
            this.dataGrid2.Name = "dataGrid2";
            this.dataGrid2.SelectionBackColor = SystemColors.Highlight;
            this.dataGrid2.Size = new Size(0x188, 0x215);
            this.dataGrid2.TabIndex = 0;
            this.dataGrid2.CurrentCellChanged += new EventHandler(this.dataGrid2_CurrentCellChanged);
            this.mainMenu1.MenuItems.AddRange(new MenuItem[] { this.mnuLoad, this.mnuMoveTo, this.mnuMoveBk, this.mnuMoveAllTo, this.mnuMoveAllBk, this.mnuSave, this.mnuCancel });
            this.mnuLoad.Index = 0;
            this.mnuLoad.Text = "&Load";
            this.mnuLoad.Click += new EventHandler(this.mnuLoad_Click);
            this.mnuMoveTo.Index = 1;
            this.mnuMoveTo.Text = "&Move To";
            this.mnuMoveTo.Click += new EventHandler(this.mnuMoveTo_Click);
            this.mnuMoveBk.Index = 2;
            this.mnuMoveBk.Text = "Move &Back";
            this.mnuMoveBk.Click += new EventHandler(this.mnuMoveBk_Click);
            this.mnuMoveAllTo.Index = 3;
            this.mnuMoveAllTo.Text = "Move All &To";
            this.mnuMoveAllTo.Click += new EventHandler(this.mnuMoveAllTo_Click);
            this.mnuMoveAllBk.Index = 4;
            this.mnuMoveAllBk.Text = "Move &All Back";
            this.mnuMoveAllBk.Click += new EventHandler(this.mnuMoveAllBk_Click);
            this.mnuSave.Index = 5;
            this.mnuSave.Text = "&Save";
            this.mnuSave.Click += new EventHandler(this.mnuSave_Click);
            this.mnuCancel.Index = 6;
            this.mnuCancel.Text = "&Close";
            this.mnuCancel.Click += new EventHandler(this.mnuCancel_Click);
            this.splitter2.Dock = DockStyle.Right;
            this.splitter2.Location = new Point(0x215, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new Size(3, 0x215);
            this.splitter2.TabIndex = 4;
            this.splitter2.TabStop = false;
            this.splitter2.SplitterMoved += new SplitterEventHandler(this.splitter2_SplitterMoved);
            this.dataGrid1.AllowSorting = false;
            this.dataGrid1.CaptionBackColor = SystemColors.ActiveBorder;
            this.dataGrid1.CaptionForeColor = SystemColors.ActiveCaption;
            this.dataGrid1.CaptionText = "Source";
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = DockStyle.Left;
            this.dataGrid1.HeaderForeColor = SystemColors.ControlText;
            this.dataGrid1.Location = new Point(0, 0);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.RowHeadersVisible = false;
            this.dataGrid1.Size = new Size(0x1a0, 0x215);
            this.dataGrid1.TabIndex = 7;
            this.panel1.Controls.Add(this.btnMoveTo);
            this.panel1.Controls.Add(this.btnMoveBk);
            this.panel1.Controls.Add(this.btnMoveAllTo);
            this.panel1.Controls.Add(this.btnMoveAllBk);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0x1a0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x75, 0x215);
            this.panel1.TabIndex = 8;
            this.btnMoveTo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnMoveTo.Location = new Point(0x20, 160);
            this.btnMoveTo.Name = "btnMoveTo";
            this.btnMoveTo.Size = new Size(0x38, 0x18);
            this.btnMoveTo.TabIndex = 20;
            this.btnMoveTo.Text = "->";
            this.btnMoveTo.Click += new EventHandler(this.btnMoveTo_Click);
            this.btnMoveBk.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnMoveBk.Location = new Point(0x20, 200);
            this.btnMoveBk.Name = "btnMoveBk";
            this.btnMoveBk.Size = new Size(0x38, 0x18);
            this.btnMoveBk.TabIndex = 0x15;
            this.btnMoveBk.Text = "<-";
            this.btnMoveBk.Click += new EventHandler(this.btnMoveBk_Click);
            this.btnMoveAllTo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnMoveAllTo.Location = new Point(0x20, 0xf8);
            this.btnMoveAllTo.Name = "btnMoveAllTo";
            this.btnMoveAllTo.Size = new Size(0x38, 0x18);
            this.btnMoveAllTo.TabIndex = 0x12;
            this.btnMoveAllTo.Text = ">>";
            this.btnMoveAllTo.Click += new EventHandler(this.btnMoveAllTo_Click);
            this.btnMoveAllBk.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.btnMoveAllBk.Location = new Point(0x20, 0x120);
            this.btnMoveAllBk.Name = "btnMoveAllBk";
            this.btnMoveAllBk.Size = new Size(0x38, 0x18);
            this.btnMoveAllBk.TabIndex = 0x13;
            this.btnMoveAllBk.Text = "<<";
            this.btnMoveAllBk.Click += new EventHandler(this.btnMoveAllBk_Click);
            this.splitter1.BorderStyle = BorderStyle.FixedSingle;
            this.splitter1.Location = new Point(0x1a0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 0x215);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            this.splitter1.SplitterMoved += new SplitterEventHandler(this.splitter1_SplitterMoved);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x3a0, 0x215);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.dataGrid1);
            base.Controls.Add(this.splitter2);
            base.Controls.Add(this.dataGrid2);
            base.Menu = this.mainMenu1;
            base.Name = "DBMergeForm";
            this.Text = "DBMergeForm";
            base.Closing += new CancelEventHandler(this.DBMergeForm_Closing);
            base.SizeChanged += new EventHandler(this.DBMergeForm_SizeChanged);
            base.Load += new EventHandler(this.DBMergeForm_Load);
            this.dataGrid2.EndInit();
            this.dataGrid1.EndInit();
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            if (base.DataChanged)
            {
                switch (base.askSave())
                {
                    case DialogResult.Yes:
                        switch (this.sfDlg.ShowDialog())
                        {
                            case DialogResult.OK:
                                this.saveFile();
                                break;

                            case DialogResult.Cancel:
                                return;
                        }
                        break;

                    case DialogResult.Cancel:
                        return;
                }
            }
            base.Close();
        }

        private void mnuLoad_Click(object sender, EventArgs e)
        {
            this.ofDlg.Filter = "Sites (*.fdb)|*.fdb|Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.ofDlg.DefaultExt = "Sites (*.fdb)|*.fdb";
            if (this.ofDlg.ShowDialog() == DialogResult.OK)
            {
                this.Fac = this.readdb(this.ofDlg.FileName);
            }
            this.dataGrid1.DataSource = this.Fac;
            this.dataGrid1.SetDataBinding(this.Fac, null);
            if (this.Fac2.Columns.Count == 0)
            {
                this.Fac2 = this.Fac.Clone();
                this.dataGrid2.DataSource = this.Fac2;
                this.dataGrid2.SetDataBinding(this.Fac2, null);
            }
        }

        private void mnuMoveAllBk_Click(object sender, EventArgs e)
        {
            int num1 = this.Fac2.Rows.Count;
            DialogResult result1 = MessageBox.Show("Remove all rows from the new database?", "Move Confirm", MessageBoxButtons.YesNo);
            if (result1 != DialogResult.No)
            {
                try
                {
                    for (int num2 = 0; num2 < num1; num2++)
                    {
                        this.Fac.Rows.Add(this.Fac2.Rows[0].ItemArray);
                        this.Fac2.Rows[0].Delete();
                    }
                    base.dataChanged();
                }
                catch (Exception exception1)
                {
                    MessageBox.Show("Move All Datarow Back Error! " + exception1.Message);
                }
            }
        }

        private void mnuMoveAllTo_Click(object sender, EventArgs e)
        {
            int num1 = this.Fac.Rows.Count;
            int num2 = 0;
            DialogResult result1 = MessageBox.Show("Move all rows to the new database?", "Move Confirm", MessageBoxButtons.YesNo);
            if (result1 != DialogResult.No)
            {
                try
                {
                    for (int num3 = 0; num3 < num1; num3++)
                    {
                        if (this.verifyDuplicatePatt(this.Fac.Rows[num2], this.Fac2.Columns["Column2"]))
                        {
                            this.dataGrid1.Select(this.dataGrid1.CurrentCell.RowNumber);
                            switch (MessageBox.Show("Duplicate pattern found in target DataBase!\r\nMove anyway?", "Move Confirm", MessageBoxButtons.YesNoCancel))
                            {
                                case DialogResult.No:
                                    num2++;
                                    goto Label_00DB;

                                case DialogResult.Cancel:
                                    return;
                            }
                        }
                        this.Fac2.Rows.Add(this.Fac.Rows[num2].ItemArray);
                        this.Fac.Rows[num2].Delete();
                    Label_00DB:;
                    }
                    base.dataChanged();
                }
                catch (Exception exception1)
                {
                    MessageBox.Show("Move All Datarow Error! " + exception1.Message);
                }
            }
        }

        private void mnuMoveBk_Click(object sender, EventArgs e)
        {
            int num1 = this.dataGrid2.CurrentCell.RowNumber;
            try
            {
                if (num1 < this.Fac2.Rows.Count)
                {
                    this.Fac.Rows.Add(this.Fac2.Rows[num1].ItemArray);
                    this.Fac2.Rows[num1].Delete();
                }
                this.dataGrid2.Select(this.dataGrid2.CurrentCell.RowNumber);
                base.dataChanged();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Move Datarow Back Error! " + exception1.Message);
            }
        }

        private void mnuMoveTo_Click(object sender, EventArgs e)
        {
            int num1 = this.dataGrid1.CurrentCell.RowNumber;
            try
            {
                if (num1 < this.Fac.Rows.Count)
                {
                    if (this.verifyDuplicatePatt(this.Fac.Rows[num1], this.Fac2.Columns["Column2"]))
                    {
                        DialogResult result1 = MessageBox.Show("Duplicate pattern found in target DataBase!\r\nMove anyway?", "Move Confirm", MessageBoxButtons.YesNo);
                        if (result1 != DialogResult.Yes)
                        {
                            goto Label_00AA;
                        }
                    }
                    this.Fac2.Rows.Add(this.Fac.Rows[num1].ItemArray);
                    this.Fac.Rows[num1].Delete();
                }
            Label_00AA:
                this.dataGrid1.Select(this.dataGrid1.CurrentCell.RowNumber);
                base.dataChanged();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Copy Datarow Error! " + exception1.Message);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            this.sfDlg.Filter = "Sites (*.fdb)|*.fdb|Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.sfDlg.DefaultExt = "Sites (*.fdb)|*.fdb";
            if (this.sfDlg.ShowDialog() == DialogResult.OK)
            {
                this.saveFile();
            }
        }

        public DataTable readdb(string db)
        {
            int num1 = 0;
            DataTable table1 = new DataTable();
            try
            {
                if (!File.Exists(db))
                {
                    MessageBox.Show("Can not find database!");
                    return null;
                }
                StreamReader reader1 = new StreamReader(db);
                while (reader1.Peek() != -1)
                {
                    string[] textArray1 = reader1.ReadLine().Trim().Split(new char[] { '\t' });
                    if (num1 == 0)
                    {
                        for (int num2 = 0; num2 < (textArray1.Length - 1); num2++)
                        {
                            table1.Columns.Add();
                        }
                    }
                    DataRow row1 = table1.NewRow();
                    for (int num3 = 0; num3 < (textArray1.Length - 1); num3++)
                    {
                        row1[num3] = textArray1[num3];
                    }
                    table1.Rows.Add(row1);
                    num1++;
                }
                reader1.Close();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Read Database Error! Database " + db + "\r\n" + exception1.Message);
            }
            return table1;
        }

        private void saveFile()
        {
            string text1 = this.sfDlg.FileName;
            try
            {
                StreamWriter writer1;
                if (!File.Exists(text1))
                {
                    writer1 = File.CreateText(text1);
                }
                else
                {
                    writer1 = new StreamWriter(text1);
                }
                DataTable table1 = (DataTable) this.dataGrid2.DataSource;
                for (int num1 = 0; num1 < table1.Rows.Count; num1++)
                {
                    for (int num2 = 0; num2 < table1.Columns.Count; num2++)
                    {
                        if (num2 != 0)
                        {
                            writer1.Write("\t");
                        }
                        writer1.Write(this.dataGrid2[num1, num2]);
                    }
                    writer1.WriteLine("");
                }
                writer1.Close();
                base.dataSaved();
                base.showCaption(text1);
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Save File Error! File: " + text1 + "\r\n" + exception1.Message);
            }
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.dataGrid2.Width = (((base.Width - this.dataGrid1.Width) - this.splitter1.Width) - this.splitter2.Width) - 120;
        }

        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.dataGrid1.Width = (this.splitter2.Left - this.splitter1.Width) - 120;
        }

        private bool verifyDuplicatePatt(DataRow dRow, DataColumn dCol)
        {
            ArrayList list1 = new ArrayList();
            string text1 = dRow.ItemArray[2].ToString();
            for (int num1 = 0; num1 < dCol.Table.Rows.Count; num1++)
            {
                list1.Add(dCol.Table.Rows[num1].ItemArray[2].ToString());
            }
            return list1.Contains(text1);
        }


        private Button btnMoveAllBk;
        private Button btnMoveAllTo;
        private Button btnMoveBk;
        private Button btnMoveTo;
        private Container components;
        private DataGrid dataGrid1;
        private DataGrid dataGrid2;
        public DataTable Fac;
        public DataTable Fac2;
        private MainMenu mainMenu1;
        private MenuItem mnuCancel;
        private MenuItem mnuLoad;
        private MenuItem mnuMoveAllBk;
        private MenuItem mnuMoveAllTo;
        private MenuItem mnuMoveBk;
        private MenuItem mnuMoveTo;
        private MenuItem mnuSave;
        private OpenFileDialog ofDlg;
        private Panel panel1;
        private SaveFileDialog sfDlg;
        private Splitter splitter1;
        private Splitter splitter2;
    }
}

