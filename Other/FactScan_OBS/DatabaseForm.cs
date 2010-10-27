namespace FacScan
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class DatabaseForm : dataForm
    {
        public DatabaseForm()
        {
            this.components = null;
            this.InitializeComponent();
            base.StartPosition = FormStartPosition.CenterScreen;
            base.formTitle = "DataBase Management";
            base.showCaption("New Database");
        }

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {
            base.dataChanged();
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
			this.components = new System.ComponentModel.Container();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.mnuNew = new System.Windows.Forms.MenuItem();
			this.mnuOpen = new System.Windows.Forms.MenuItem();
			this.mnuSave = new System.Windows.Forms.MenuItem();
			this.mnuSaveAs = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.ofDlg = new System.Windows.Forms.OpenFileDialog();
			this.sfDlg = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSave,
            this.mnuSaveAs,
            this.mnuExit});
			// 
			// mnuNew
			// 
			this.mnuNew.Index = 0;
			this.mnuNew.Text = "&New";
			this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.Index = 1;
			this.mnuOpen.Text = "&Open";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuSave
			// 
			this.mnuSave.Index = 2;
			this.mnuSave.Text = "&Save";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Index = 3;
			this.mnuSaveAs.Text = "Save &As";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 4;
			this.mnuExit.Text = "&Close";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// dataGrid1
			// 
			this.dataGrid1.CaptionVisible = false;
			this.dataGrid1.DataMember = "";
			this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(0, 0);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(584, 384);
			this.dataGrid1.TabIndex = 0;
			this.dataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
			this.dataGrid1.CurrentCellChanged += new System.EventHandler(this.dataGrid1_CurrentCellChanged);
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.dataGrid1;
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			// 
			// DatabaseForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 405);
			this.Controls.Add(this.dataGrid1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "DatabaseForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "DatabaseForm";
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (!base.DataChanged)
            {
                base.Close();
            }
            else
            {
                base.askSave();
                base.Close();
            }
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            if (base.DataChanged)
            {
                base.askSave();
            }
            string text1 = "NewTable";
            DataTable table1 = new DataTable(text1);
            table1.Columns.Add();
            table1.Columns.Add();
            table1.Columns.Add();
            this.dataGrid1.DataSource = table1;
            this.dataGrid1.SetDataBinding(table1, null);
            base.showCaption(text1);
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            if (base.DataChanged)
            {
                base.askSave();
            }
            int num1 = 0;
            this.ofDlg.Filter = "Sites (*.fdb)|*.fdb|Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.ofDlg.DefaultExt = "Sites (*.fdb)|*.fdb";
            if (this.ofDlg.ShowDialog() == DialogResult.OK)
            {
                string text1 = this.ofDlg.FileName;
                try
                {
                    StreamReader reader1 = new StreamReader(text1);
                    DataTable table1 = new DataTable(text1);
                    while (reader1.Peek() != -1)
                    {
                        string[] textArray1 = reader1.ReadLine().Trim().Split(new char[] { '\t' });
                        int num2 = textArray1.GetUpperBound(0);
                        num1++;
                        if (num1 == 0)
                        {
                            for (int num3 = 0; num3 <= num2; num3++)
                            {
                                table1.Columns.Add();
                            }
                        }
                        DataRow row1 = table1.NewRow();
                        for (int num4 = 0; num4 <= num2; num4++)
                        {
                            row1[num4] = textArray1[num4];
                        }
                        table1.Rows.Add(row1);
                    }
                    this.dataGrid1.DataSource = table1;
                    this.dataGrid1.SetDataBinding(table1, null);
                    reader1.Close();
                }
                catch (Exception exception1)
                {
                    MessageBox.Show("!!!Error: " + exception1.Message);
                    base.Close();
                    Environment.Exit(0);
                }
                base.showCaption(text1);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            string text1 = this.ofDlg.FileName;
            if (text1 == "")
            {
                this.mnuSaveAs_Click(this, null);
            }
            else
            {
                try
                {
                    StreamWriter writer1;
                    if (!File.Exists(text1))
                    {
                        writer1 = File.CreateText(text1);
                    }
                    else
                    {
                        DialogResult result1 = MessageBox.Show("Do you really want to overwrite the original file?", "Overwrite Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result1 != DialogResult.Yes)
                        {
                            this.mnuSaveAs_Click(this, null);
                            return;
                        }
                        writer1 = new StreamWriter(text1);
                    }
                    DataTable table1 = (DataTable) this.dataGrid1.DataSource;
                    for (int num1 = 0; num1 < table1.Rows.Count; num1++)
                    {
                        for (int num2 = 0; num2 < table1.Columns.Count; num2++)
                        {
                            if (num2 != 0)
                            {
                                writer1.Write("\t");
                            }
                            writer1.Write(this.dataGrid1[num1, num2]);
                        }
                        writer1.WriteLine("");
                    }
                    writer1.Close();
                }
                catch (Exception exception1)
                {
                    Console.WriteLine("Error: " + exception1.Message);
                }
                base.dataSaved();
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            this.sfDlg.Filter = "Sites (*.fdb)|*.fdb|Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.sfDlg.DefaultExt = "Sites (*.fdb)|*.fdb";
            if (this.sfDlg.ShowDialog() == DialogResult.OK)
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
                        DialogResult result1 = MessageBox.Show("Do you really want to overwrite the original file?", "Overwrite Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result1 != DialogResult.Yes)
                        {
                            return;
                        }
                        writer1 = new StreamWriter(text1);
                    }
                    base.showCaption(text1);
                    DataTable table1 = (DataTable) this.dataGrid1.DataSource;
                    for (int num1 = 0; num1 < table1.Rows.Count; num1++)
                    {
                        for (int num2 = 0; num2 < table1.Columns.Count; num2++)
                        {
                            if (num2 != 0)
                            {
                                writer1.Write("\t");
                            }
                            writer1.Write(this.dataGrid1[num1, num2]);
                        }
                        writer1.WriteLine("");
                    }
                    writer1.Close();
                }
                catch (Exception exception1)
                {
                    Console.WriteLine("Error: " + exception1.Message);
                }
                base.dataSaved();
            }
		}

		private IContainer components;
        private DataGrid dataGrid1;
        private DataGridTableStyle dataGridTableStyle1;
        private MainMenu mainMenu1;
        private MenuItem mnuExit;
        private MenuItem mnuNew;
        private MenuItem mnuOpen;
        private MenuItem mnuSave;
        private MenuItem mnuSaveAs;
        private OpenFileDialog ofDlg;
        private SaveFileDialog sfDlg;
    }
}

