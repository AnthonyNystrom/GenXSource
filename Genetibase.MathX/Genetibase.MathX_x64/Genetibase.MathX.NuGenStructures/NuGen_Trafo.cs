using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.MathX.NuGenStructures;
using Microsoft.VisualBasic; 

namespace Genetibase.MathX.NuGenStructures
{
    public partial class NuGen_Trafo : Form
    {
        Genetibase.MathX.NuGenStructures.NuGenTrafo2D var1, output;
        string str;

        public NuGen_Trafo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (text3_0.Text == "" || text3_1.Text == "" || text3_2.Text == "" || text3_3.Text == "" || text3_4.Text == "" || text3_5.Text == "" || text3_6.Text == "" || text3_7.Text == "" || text3_8.Text == "")
            {
                MessageBox.Show("Please Enter All The Values Correctly.");
            }

            else
            {
                var1 = new Genetibase.MathX.NuGenStructures.NuGenTrafo2D( double.Parse(text3_0.Text), double.Parse(text3_1.Text), double.Parse(text3_2.Text), double.Parse(text3_3.Text), double.Parse(text3_4.Text), double.Parse(text3_5.Text), double.Parse(text3_6.Text), double.Parse(text3_7.Text), double.Parse(text3_8.Text));

                str = Interaction.InputBox("Please Enter a Double value by which you need to Calculate with", "Double Value", "1", 30, 30);
               

            }
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            text3_0.Text = "";
            text3_1.Text = "";
            text3_2.Text = "";
            text3_3.Text = "";
            text3_4.Text = "";
            text3_5.Text = "";
            text3_6.Text = "";
            text3_7.Text = "";
            text3_8.Text = "";

            text3_0.Focus();
            Table_output.Visible = false ;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            output = var1 + double.Parse(str);
            display_out();
  
        }
        public void display_out()
        {
            text_o_0.Text = output._x[0].ToString() ;
            text_o_1.Text = output._x[1].ToString();
            text_o_2.Text = output._x[2].ToString();
            text_o_3.Text = output._x[3].ToString();
            text_o_4.Text = output._x[4].ToString();
            text_o_5.Text = output._x[5].ToString();
            text_o_6.Text = output._x[6].ToString();
            text_o_7.Text = output._x[7].ToString();
            text_o_8.Text = output._x[8].ToString();

            Table_output.Visible = true;  

        }

        private void button3_Click(object sender, EventArgs e)
        {
            output = var1 - double.Parse(str);
            display_out();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            output = var1 * double.Parse(str);
            display_out();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            output = var1 / double.Parse(str);
            display_out();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Application.Exit();  
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