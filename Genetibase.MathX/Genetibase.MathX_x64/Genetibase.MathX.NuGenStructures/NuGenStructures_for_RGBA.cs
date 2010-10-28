using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic; 
using Genetibase.MathX.NuGenStructures;
 


namespace Genetibase.MathX.NuGenStructures
{
    public partial class NuGenStructures_for_RGBA : Form
    {

        public Genetibase.MathX.NuGenStructures.NuGenRGBA obj;
        public Genetibase.MathX.NuGenStructures.NuGenRGBA obj1;

        public NuGenStructures_for_RGBA()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (text_a.Text == "" || text_b.Text == "" || text_g.Text == "" || text_r.Text == "")
            {
                MessageBox.Show("Please Enter Values for R, G, B And A ");

            }
            else
            {
                button_add.Enabled = true;
                button_div.Enabled = true;
                button_mult.Enabled = true;
                button_sub.Enabled = true;

               
                obj = new Genetibase.MathX.NuGenStructures.NuGenRGBA();

                obj._x = new float[4];

                obj._x[0] = float.Parse(text_r.Text);
                obj._x[1] = float.Parse(text_g.Text);
                obj._x[2] = float.Parse(text_b.Text);
                obj._x[3] = float.Parse(text_a.Text);



            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string str1;
            str1 = Interaction.InputBox("Please Enter a float value which you need to ADD:", "Float Value", "1", 30, 30);
              
          

            obj1 = obj + float.Parse(str1);

            display_out(); 

        }
        public void display_out()
        {

            text_r_o.Text = obj1._x[0].ToString();
            text_g_o.Text = obj1._x[1].ToString();
            text_b_o.Text = obj1._x[2].ToString();
            text_a_o.Text = obj1._x[3].ToString(); 





        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Application.Exit();  
 
        }

        private void button_sub_Click(object sender, EventArgs e)
        {

            string str1;
            str1 = Interaction.InputBox("Please Enter a float value which you need to Subtract:", "Float Value", "1", 30, 30);



            obj1 = obj - float.Parse(str1);

            display_out(); 


        }

        private void button_mult_Click(object sender, EventArgs e)
        {
            string str1;
            str1 = Interaction.InputBox("Please Enter a float value which you need to Multiply:", "Float Value", "1", 30, 30);



            obj1 = obj * float.Parse(str1);

            display_out(); 

        }

        private void button_div_Click(object sender, EventArgs e)
        {
            string str1;
            str1 = Interaction.InputBox("Please Enter a float value which you need to Divide:", "Float Value", "1", 30, 30);



            obj1 = obj / float.Parse(str1);

            display_out(); 

        }

        private void button1_Click(object sender, EventArgs e)
        {

            text_r_o.Text = "";
            text_g_o.Text = "";
            text_b_o.Text = "";
            text_a_o.Text = "";

            text_r.Text = "";
            text_g.Text = "";
            text_b.Text = "";
            text_a.Text = "";

            button_add.Enabled = false ;
            button_div.Enabled = false;
            button_mult.Enabled = false;
            button_sub.Enabled = false;

            text_r.Focus(); 
        }

        private void structuresMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NuGen_Structures_Main next_form = new NuGen_Structures_Main();
            this.Visible = false;
            next_form.Visible = true;  
        }

        private void mathematicalOperationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NuGenScientificCalculator.NuGenSciCalc next_form = new NuGenScientificCalculator.NuGenSciCalc();

            //this.Visible = false;
            //next_form.Visible = true;
        }

    }
}