using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NuGenRealTime
{
    class NuGenLogFileViewer : Form
    {
        private RichTextBox richTextBox1;
        private Label label1;
        private Label label2;
        private ListBox listBox1;
        private Label label3;

        private string logname;

        public NuGenLogFileViewer(string logname)
        {
            this.logname = logname;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 61);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(289, 257);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Currently Viewing :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "";
            // 
            // listBox1
            // 
            foreach(string file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*" + logname + "*.log"))
            {
                listBox1.Items.Add(file.Substring(file.LastIndexOf('\\') + 1));
            }
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(323, 61);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(190, 251);
            this.listBox1.TabIndex = 2;
            this.listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Select a Log to View";
            // 
            // NuGenLogFileViewer
            // 
            this.ClientSize = new System.Drawing.Size(550, 350);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "NuGenLogFileViewer";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.MaximumSize = this.MinimumSize = this.Size;

        }

        void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileStream f = File.Open(listBox1.SelectedItem as string, FileMode.Open);

            StreamReader reader = new StreamReader(f);

            richTextBox1.Text = reader.ReadToEnd();            
        }

        private static NuGenLogFileViewer instance;

        public static void ShowItem(string logname)
        {
            instance = new NuGenLogFileViewer(logname);
            instance.ShowDialog();
        }
    }
}
