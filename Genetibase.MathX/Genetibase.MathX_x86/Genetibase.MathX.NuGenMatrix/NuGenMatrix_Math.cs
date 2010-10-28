using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.MathX.NuGenMatrix;
using Microsoft.VisualBasic;
using NuGen_Matrix;


namespace Genetibase.MathX.NuGenMatrix
{

 
    public partial class NuGenMatrix_Math : Form
    {

        public Genetibase.MathX.NuGenMatrix.NuGenMatrix list1, list2,output_list; 
         
        public NuGenMatrix_Math()
        {
            InitializeComponent();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

           

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox27_TextChanged(object sender, EventArgs e)
        {

        }

        private void NuGenMatrix_Math_Load(object sender, EventArgs e)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (text1_3_0.Text == "" || text1_3_1.Text == "" || text1_3_2.Text == "" || text1_3_3.Text == "" || text1_3_4.Text == "" || text1_3_5.Text == "" || text1_3_6.Text == "" || text1_3_7.Text == "" || text1_3_8.Text == "" || text3_0.Text == "" || text3_1.Text == "" || text3_2.Text == "" || text3_3.Text == "" || text3_4.Text == "" || text3_5.Text == "" || text3_6.Text == "" || text3_7.Text == "" || text3_8.Text == "")
            {
                MessageBox.Show("Please Enter Both The Metrices Correctly.");
            }

            else
            {

                //decimal[,] data1 ={ decimal.Parse(text3_0.Text), decimal.Parse(text3_1.Text), decimal.Parse(text3_2.Text), decimal.Parse(text3_3.Text), decimal.Parse(text3_4.Text), decimal.Parse(text3_5.Text), decimal.Parse(text3_6.Text), decimal.Parse(text3_7.Text), decimal.Parse(text3_8.Text) };
                //decimal[,] data2 ={ decimal.Parse(text1_3_0.Text ), decimal.Parse(text1_3_1.Text ), decimal.Parse(text1_3_2.Text), decimal.Parse(text1_3_3.Text), decimal.Parse(text1_3_4.Text), decimal.Parse(text1_3_5.Text), decimal.Parse(text1_3_6.Text), decimal.Parse(text1_3_7.Text), decimal.Parse(text1_3_8.Text) };
                decimal[,] data1=new decimal[3,3];

                decimal[,] data2 = new decimal[3, 3];


                data1[0, 0] = decimal.Parse(text3_0.Text);
                data1[0, 1]=decimal.Parse(text3_1.Text);
                data1[0, 2]=decimal.Parse(text3_2.Text);
                data1[1, 0]=decimal.Parse(text3_3.Text);
                data1[1, 1]=decimal.Parse(text3_4.Text);
                data1[1, 2]=decimal.Parse(text3_5.Text);
                data1[2, 0]=decimal.Parse(text3_6.Text);
                data1[2, 1] = decimal.Parse(text3_7.Text);
                data1[2, 2] = decimal.Parse(text3_8.Text);


                data2[0, 0] = decimal.Parse(text1_3_0.Text);
                data2[0, 1] = decimal.Parse(text1_3_1.Text);
                data2[0, 2] = decimal.Parse(text1_3_2.Text);
                data2[1, 0] = decimal.Parse(text1_3_3.Text);
                data2[1, 1] = decimal.Parse(text1_3_4.Text);
                data2[1, 2] = decimal.Parse(text1_3_5.Text);
                data2[2, 0] = decimal.Parse(text1_3_6.Text);
                data2[2, 1] = decimal.Parse(text1_3_7.Text);
                data2[2, 2] = decimal.Parse(text1_3_8.Text);





                //list1 = new NuGenMatrix(3, 3, data1);


                //list2 = new NuGenMatrix(3, 3, data2);

                list1 = new NuGenMatrix(data1);
                list2 = new NuGenMatrix(data2);



                mat_add.Enabled = true;
                mat_div.Enabled = true;
                mat_equ.Enabled = true;
                mat_mult.Enabled = true;
                mat_not_equ.Enabled = true;
                mat_sub.Enabled = true;

            }

        }

        private void mat_add_Click(object sender, EventArgs e)
        {
            output_list = list1 + list2;

            display_out();
                        

 
        }
        public void display_out()
        {
            Table_output.Visible = true;

            text_o_0.Text = (output_list.Data[0, 0].ToString());
            text_o_1.Text = (output_list.Data[0, 1].ToString());
            text_o_2.Text = (output_list.Data[0, 2].ToString());
            text_o_3.Text = (output_list.Data[1, 0].ToString());
            text_o_4.Text = (output_list.Data[1, 1].ToString());
            text_o_5.Text = (output_list.Data[1, 2].ToString());
            text_o_6.Text = (output_list.Data[2, 0].ToString());
            text_o_7.Text = (output_list.Data[2, 1].ToString());
            text_o_8.Text = (output_list.Data[2, 2].ToString());
            Table_output.Enabled = false;

        }

        private void mat_sub_Click(object sender, EventArgs e)
        {
            output_list = list1 - list2;

            display_out();

        }

        private void mat_mult_Click(object sender, EventArgs e)
        {
            output_list = list1 * list2;

            display_out();
        }

        private void mat_div_Click(object sender, EventArgs e)
        {

            text1_3_0.Text ="";
            text1_3_1.Text = "";
            text1_3_2.Text = "";
            text1_3_3.Text = "";
            text1_3_4.Text = "";
            text1_3_5.Text = "";
            text1_3_6.Text = "";
            text1_3_7.Text = "";
            text1_3_8.Text = "";

            
            string str1;
            str1 = Interaction.InputBox("Please Enter a decimal value by which you need to divide the Matrix 1:","Decimal Value", "1",30,30);
  
            

            output_list = decimal.Parse(str1) / list1;

            display_out();

          

        }

        private void mat_equ_Click(object sender, EventArgs e)
        {
            if (list1 == list2)
                MessageBox.Show ("The Two Metrices Are EQUAL");
            else
                MessageBox.Show ("NOT EQUAL");
           
        }

        private void mat_not_equ_Click(object sender, EventArgs e)
        {
            if (list1 != list2)
                MessageBox.Show("The Two Metrices Are Not EQUAL");
            else
                MessageBox.Show("EQUAL");

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           text1_3_0.Text = (list1.Data[0, 0].ToString());
           text1_3_1.Text = (list1.Data[0, 1].ToString());
           text1_3_2.Text = (list1.Data[0, 2].ToString());
           text1_3_3.Text = (list1.Data[1, 0].ToString());
           text1_3_4.Text = (list1.Data[1, 1].ToString());
           text1_3_5.Text = (list1.Data[1, 2].ToString());
           text1_3_6.Text = (list1.Data[2, 0].ToString());
           text1_3_7.Text = (list1.Data[2, 1].ToString());
           text1_3_8.Text = (list1.Data[2, 2].ToString());


           text3_0.Text = (list2.Data[0, 0].ToString());
           text3_1.Text = (list2.Data[0, 1].ToString());
           text3_2.Text = (list2.Data[0, 2].ToString());
           text3_3.Text = (list2.Data[1, 0].ToString());
           text3_4.Text = (list2.Data[1, 1].ToString());
           text3_5.Text = (list2.Data[1, 2].ToString());
           text3_6.Text = (list2.Data[2, 0].ToString());
           text3_7.Text = (list2.Data[2, 1].ToString());
           text3_8.Text = (list2.Data[2, 2].ToString());

            ok_button.Enabled = true;

            mat_add.Enabled = false;
            mat_div.Enabled = false;
            mat_equ.Enabled =false;
            mat_mult.Enabled = false;
            mat_not_equ.Enabled = false;
            mat_sub.Enabled = false;

            clear_button.Enabled = true;
            ok_button.Focus();
             
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            text1_3_0.Text = "";
            text1_3_1.Text = "";
            text1_3_2.Text = "";
            text1_3_3.Text = "";
            text1_3_4.Text = "";
            text1_3_5.Text = "";
            text1_3_6.Text = "";
            text1_3_7.Text = "";
            text1_3_8.Text = "";


            text3_0.Text = "";
            text3_1.Text = "";
            text3_2.Text = "";
            text3_3.Text = "";
            text3_4.Text = "";
            text3_5.Text = "";
            text3_6.Text = "";
            text3_7.Text = "";
            text3_8.Text = "";

            Table_output.Visible = false;  

            text3_0.Focus();



        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Application.Exit();  
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Table3_3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void text1_3_1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Table1_3_3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Table_output_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //private void matrixOperationsMenuToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Genetibase.MathX.NuGenMatrixOperations.NuGen_operations_Menu next_form = new Genetibase.MathX.NuGenMatrixOperations.NuGen_operations_Menu();
        //    this.Visible = false;
        //    next_form.Visible = true;   

        //}

        private void mathematicalOperationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NuGenScientificCalculator.NuGenSciCalc next_form = new NuGenScientificCalculator.NuGenSciCalc();

            //this.Visible = false;
            //next_form.Visible = true;
        }
    }
}