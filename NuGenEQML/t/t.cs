using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sample3
{
    public class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            Application.DoEvents();
        }

        private void save_mathml (object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.InitialDirectory = "c:\\" ;
            dialog.Filter = "Mathml files (*.mathml)|*.mathml|All files (*.*)|*.*" ;
            dialog.FilterIndex = 1 ;
            dialog.RestoreDirectory = true ;
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                mathMLControl1.pub_Save (dialog.FileName); 
            }
        }

        private void save_mathml_pure (object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.InitialDirectory = "c:\\" ;
            dialog.Filter = "Mathml files (*.mathml)|*.mathml|All files (*.*)|*.*" ;
            dialog.FilterIndex = 1 ;
            dialog.RestoreDirectory = true ;
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                mathMLControl1.pub_SavePure (dialog.FileName); 
            }
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            mathMLControl1.pub_LoadXML("<math xmlns=\"http://www.w3.org/1998/Math/MathML\"><mi>Y</mi><mo>&equals;</mo><msqrt><mrow><mn>1</mn><mo>&plus;</mo><msup><mi>X</mi><mrow><mn>2</mn></mrow></msup></mrow></msqrt><mo>&plus;</mo></math>");
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new Form2());
        }

        private System.Windows.Forms.TextBox textBox1;
        private Button button1;
        private Button button2;
        private Button button3;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_ = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.mathMLControl1 = new Genetibase.MathX.NuGenEQML();
            this.SuspendLayout();
            // 
            // button_
            // 
            this.button_.Location = new System.Drawing.Point(6, 7);
            this.button_.Name = "button_";
            this.button_.Size = new System.Drawing.Size(63, 20);
            this.button_.TabIndex = 1;
            this.button_.Text = "Save";
            this.button_.Click += new System.EventHandler(this.save_mathml);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(6, 287);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(640, 297);
            this.textBox1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(74, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 20);
            this.button1.TabIndex = 1;
            this.button1.Text = "Export";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(141, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 20);
            this.button2.TabIndex = 1;
            this.button2.Text = "Load";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(208, 7);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(63, 20);
            this.button3.TabIndex = 1;
            this.button3.Text = "SavePure";
            this.button3.Click += new System.EventHandler(this.save_mathml_pure);
            // 
            // mathMLControl1
            // 
            this.mathMLControl1.AutoCloseBrackets = true;
            this.mathMLControl1.BackColor = System.Drawing.Color.White;
            this.mathMLControl1.Location = new System.Drawing.Point(6, 33);
            this.mathMLControl1.MC_BackgroundColor = System.Drawing.Color.White;
            this.mathMLControl1.MC_EnableStretchyBrackets = true;
            this.mathMLControl1.MC_FontSize = 14F;
            this.mathMLControl1.MC_UseDefaultContextMenu = true;
            this.mathMLControl1.Name = "mathMLControl1";
            this.mathMLControl1.ParentControl_DesignMode = false;
            this.mathMLControl1.Size = new System.Drawing.Size(640, 248);
            this.mathMLControl1.TabIndex = 0;
            this.mathMLControl1.Event_OnSelectionChanged += new System.EventHandler(this.mathMLControl1_Event_OnSelectionChanged);
            // 
            // Form2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(919, 752);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.mathMLControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button_);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Genetibase.MathX.NuGenEQML mathMLControl1;
        private Button button_;

        private void mathMLControl1_Event_OnSelectionChanged(object sender, System.EventArgs e)
        {
            textBox1.Text = mathMLControl1.pub_GetXML(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mathMLControl1.pub_Export();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.InitialDirectory = "c:\\" ;
            dialog.Filter = "Mathml files (*.mathml)|*.mathml|All files (*.*)|*.*" ;
            dialog.FilterIndex = 1 ;
            dialog.RestoreDirectory = true ;
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                mathMLControl1 .LoadFromFile(dialog.FileName);
            }
        } 
    }
}