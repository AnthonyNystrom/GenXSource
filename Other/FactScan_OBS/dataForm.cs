namespace FacScan
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class dataForm : Form
    {
        public DialogResult askSave()
        {
            return MessageBox.Show("Data has been changed. Save it?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

		public dataForm()
		{
			this.InitializeComponent();
		}

        public void dataChanged()
        {
            if (this.Text.IndexOf("*") == -1)
            {
                this.Text = this.Text + "*";
            }
            this.datachanged = true;
        }

        public void dataSaved()
        {
            if (this.Text.IndexOf("*") != -1)
            {
                this.Text = this.Text.Substring(0, this.Text.IndexOf("*"));
            }
            this.datachanged = false;
        }

        private void InitializeComponent()
        {
			this.SuspendLayout();
			// 
			// dataForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "dataForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

        }

        public void showCaption(string db)
        {
            string[] textArray1 = db.Split(new char[] { '\\' });
            this.Text = this.title + " - " + textArray1[textArray1.Length - 1];
        }


        public bool DataChanged
        {
            get
            {
                return this.datachanged;
            }
            set
            {
                this.datachanged = value;
            }
        }

        public string formTitle
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }


        private bool datachanged;
        private string title;
    }
}

