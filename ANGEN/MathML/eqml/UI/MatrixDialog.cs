namespace UI
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    internal class MatrixDialog : Form
    {
        public MatrixDialog()
        {
            this.success = false;
            this.container = null;
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
                this.success = true;
                base.Close();
            }
            else if (keyData == Keys.Escape)
            {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatrixDialog));
            this.numrows_ = new System.Windows.Forms.Label();
            this.numcols_ = new System.Windows.Forms.Label();
            this.cancelButton = new Glass.GlassButton();
            this.okButton = new Glass.GlassButton();
            this.rows = new System.Windows.Forms.NumericUpDown();
            this.cols = new System.Windows.Forms.NumericUpDown();
            this.twoBy1 = new Glass.GlassButton();
            this.twoBy2 = new Glass.GlassButton();
            this.threeBy1 = new Glass.GlassButton();
            this.threeBy3 = new Glass.GlassButton();
            ((System.ComponentModel.ISupportInitialize)(this.rows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cols)).BeginInit();
            this.SuspendLayout();
            // 
            // numrows_
            // 
            resources.ApplyResources(this.numrows_, "numrows_");
            this.numrows_.Name = "numrows_";
            // 
            // numcols_
            // 
            resources.ApplyResources(this.numcols_, "numcols_");
            this.numcols_.Name = "numcols_";
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
            // rows
            // 
            this.rows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.rows, "rows");
            this.rows.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.rows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rows.Name = "rows";
            this.rows.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // cols
            // 
            this.cols.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.cols, "cols");
            this.cols.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.cols.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cols.Name = "cols";
            this.cols.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // twoBy1
            // 
            resources.ApplyResources(this.twoBy1, "twoBy1");
            this.twoBy1.Name = "twoBy1";
            this.twoBy1.Click += new System.EventHandler(this.TwoByOne);
            // 
            // twoBy2
            // 
            resources.ApplyResources(this.twoBy2, "twoBy2");
            this.twoBy2.Name = "twoBy2";
            this.twoBy2.Click += new System.EventHandler(this.TwoByTwo);
            // 
            // threeBy1
            // 
            resources.ApplyResources(this.threeBy1, "threeBy1");
            this.threeBy1.Name = "threeBy1";
            this.threeBy1.Click += new System.EventHandler(this.ThreeByOne);
            // 
            // threeBy3
            // 
            resources.ApplyResources(this.threeBy3, "threeBy3");
            this.threeBy3.Name = "threeBy3";
            this.threeBy3.Click += new System.EventHandler(this.ThreeByThree);
            // 
            // MatrixDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.threeBy3);
            this.Controls.Add(this.threeBy1);
            this.Controls.Add(this.twoBy2);
            this.Controls.Add(this.twoBy1);
            this.Controls.Add(this.cols);
            this.Controls.Add(this.rows);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.numcols_);
            this.Controls.Add(this.numrows_);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatrixDialog";
            ((System.ComponentModel.ISupportInitialize)(this.rows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cols)).EndInit();
            this.ResumeLayout(false);

        }

        

        private void OnOk(object sender, EventArgs e)
        {
            this.success = true;
            base.Close();
        }

        private void TwoByOne(object sender, EventArgs e)
        {
            this.rows.Value = new decimal(2);
            this.cols.Value = new decimal(1);
        }

        private void TwoByTwo(object sender, EventArgs e)
        {
            this.rows.Value = new decimal(2);
            this.cols.Value = new decimal(2);
        }

        private void ThreeByOne(object sender, EventArgs e)
        {
            this.rows.Value = new decimal(3);
            this.cols.Value = new decimal(1);
        }

        private void ThreeByThree(object sender, EventArgs e)
        {
            this.rows.Value = new decimal(3);
            this.cols.Value = new decimal(3);
        }

        private void OnCancel(object sender, EventArgs e)
        {
            base.Close();
        }


        public int NumCols
        {
            get
            {
                return (int) this.cols.Value;
            }
        }

        public int NumRows
        {
            get
            {
                return (int) this.rows.Value;
            }
        }

        public bool Success
        {
            get
            {
                return this.success;
            }
        }


        private Label numrows_;
        private Label numcols_;
        private Glass.GlassButton threeBy3;
        private Container container;
        private Glass.GlassButton cancelButton;
        private Glass.GlassButton okButton;
        private bool success;
        private NumericUpDown rows;
        private NumericUpDown cols;
        private Glass.GlassButton twoBy1;
        private Glass.GlassButton twoBy2;
        private Glass.GlassButton threeBy1;
    }
}

