using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NuGenInSightCompilation
{
    public partial class ProjectNameForm : Form
    {
        public ProjectNameForm()
        {
            InitializeComponent();
        }

        private String name;

        public String ProjectName
        {
            get
            {
                return name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                name = textBox1.Text;
                Close();
            }
            else
            {
                MessageBox.Show("You must enter a project name");
            }
        }
    }
}