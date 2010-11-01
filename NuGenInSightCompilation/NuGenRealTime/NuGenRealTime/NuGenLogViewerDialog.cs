using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.NuGenMeters;

namespace NuGenRealTime
{
    class NuGenLogViewerDialog : Form
    {
        private ListBox listBox1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX buttonX2;

        private NuGenRealTimeLog log;

        public NuGenLogViewerDialog(NuGenRealTimeLog log)
        {
            this.log = log;
            InitializeComponent();
        }
    
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            foreach (INuGenCounter item in log.Items)
            {
                listBox1.Items.Add(item.CategoryName + " -> " + item.CounterName);
            }
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(229, 173);
            this.listBox1.TabIndex = 0;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Location = new System.Drawing.Point(12, 191);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.TabIndex = 1;
            this.buttonX1.Text = "Remove";
            this.buttonX1.Click += new EventHandler(buttonX1_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonX2.Location = new System.Drawing.Point(182, 207);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(75, 23);
            this.buttonX2.TabIndex = 1;
            this.buttonX2.Text = "OK";
            // 
            // NuGenLogViewerDialog
            // 
            this.ClientSize = new System.Drawing.Size(269, 242);
            this.Controls.Add(this.buttonX2);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.listBox1);
            this.Name = "NuGenLogViewerDialog";
            this.Text = "Counters Being Logged";
            this.ResumeLayout(false);
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

        }

        void buttonX1_Click(object sender, EventArgs e)
        {            
            log.RemoveGenericAt(listBox1.SelectedIndex);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private static NuGenLogViewerDialog instance;

        public static void ShowItem(NuGenRealTimeLog log)
        {
            instance = new NuGenLogViewerDialog(log);
            instance.ShowDialog();
        }
    }
}
