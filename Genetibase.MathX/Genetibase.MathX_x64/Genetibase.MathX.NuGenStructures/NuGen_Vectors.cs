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
    public partial class NuGen_Vectors : Form
    {
        Genetibase.MathX.NuGenStructures.NuGenVec2D vector1, vector2, output_vector;
        Genetibase.MathX.NuGenStructures.NuGenVec3D vector1_3, vector2_3, output_vector_3; 

        public NuGen_Vectors()
        {
            InitializeComponent();
        }

        private void NuGen_Vectors_Load(object sender, EventArgs e)
        {


        }

        private void button_ok_Click(object sender, EventArgs e)
        {

            if (u_x1.Text == "" || u_x2.Text == "" || u_y1.Text == "" || u_y2.Text == "")
            {
                MessageBox.Show("Please Enter All The Values For Both The Vectors");
            }
            else
            {
                vector1 = new Genetibase.MathX.NuGenStructures.NuGenVec2D();

                vector1._x = new Double[2];
   
                vector1._x[0] = Double.Parse(u_x1.Text);
                vector1._x[1] =Double.Parse(u_y1.Text);


                vector2 = new Genetibase.MathX.NuGenStructures.NuGenVec2D();
                vector2._x = new Double[2];

                vector2._x[0] = double.Parse(u_x2.Text);
                vector2._x[1] = double.Parse(u_y2.Text);


                button_add.Enabled = true;
                button_div.Enabled = true;
                button_equal.Enabled = true;
                button_less_equal.Enabled = true;
                button_mul.Enabled = true;
               
                button_sub.Enabled = true;
                
            }

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            output_vector = vector1 + vector2;
            display_out(); 
 
        }
        public void display_out()
        {
            u_x.Text = output_vector._x[0].ToString();
            u_y.Text = output_vector._x[1].ToString();
   

            button_add.Enabled = false;
            button_div.Enabled = false;
            button_equal.Enabled = false;
            button_less_equal.Enabled = false;
            button_mul.Enabled = false;
            
            button_sub.Enabled = false;

        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            u_x1.Text = "" ;
            u_x2.Text = "" ;
            u_y1.Text = "" ;
            u_y2.Text = "";
            u_x.Text = "";
            u_y.Text = "";

            u_x1.Focus();

            button_add.Enabled = false;
            button_div.Enabled = false;
            button_equal.Enabled = false;
            button_less_equal.Enabled = false;
            button_mul.Enabled = false;
            
            button_sub.Enabled = false;

            button_clear.Focus();  


        }

        private void button_sub_Click(object sender, EventArgs e)
        {
            output_vector = vector1 - vector2;
            display_out(); 
 

        }

        private void button_mul_Click(object sender, EventArgs e)
        {
            output_vector = vector1 * vector2;
            display_out(); 
 
        }

        private void button_div_Click(object sender, EventArgs e)
        {
            string str1="1";
            str1 = Interaction.InputBox("Please Enter a decimal value by which you need to divide the Vector 1:", "Decimal Value", "1", 30, 30);

            u_x2.Text = "";            
            u_y2.Text = "";
            
            
            output_vector = vector1 / double.Parse(str1)  ;

            display_out(); 
 
        }

        private void button_equal_Click(object sender, EventArgs e)
        {
            if (vector1 == vector2)
            {
                MessageBox.Show("Both the Vectors Are Equal");  
            }
            else
            {
                MessageBox.Show("Both the Vectors Are Not Equal");
            }
        }

        private void button_less_equal_Click(object sender, EventArgs e)
        {
            if (vector1 <= vector2)
            {
                MessageBox.Show("Vector1 <= Vector2");
            }
            else if(vector2 <= vector1)  
            {
                MessageBox.Show("Vector2 <= Vector1");
            }                     

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (u_x1_3.Text == "" || u_x2_3.Text == "" || u_y1_3.Text == "" || u_y2_3.Text == "" || u_h1.Text=="" || u_h2.Text==""    )
            {
                MessageBox.Show("Please Enter All The Values For Both The Vectors");
            }
            else
            {
                vector1_3 = new Genetibase.MathX.NuGenStructures.NuGenVec3D();

                vector1_3._x = new Double[3];

                vector1_3._x[0] = Double.Parse(u_x1_3.Text);
                vector1_3._x[1] = Double.Parse(u_y1_3.Text);
                vector1_3._x[2] = Double.Parse(u_h1.Text);


                vector2_3 = new Genetibase.MathX.NuGenStructures.NuGenVec3D();
                vector2_3._x = new Double[3];

                vector2_3._x[0] = double.Parse(u_x2_3.Text);
                vector2_3._x[1] = double.Parse(u_y2_3.Text);
                vector2_3._x[2] = Double.Parse(u_h2.Text);


                button_add_3.Enabled = true;
                button_div_3.Enabled = true;
                button_equal_3.Enabled = true;
                button_less_equal_3.Enabled = true;
                button_mul_3.Enabled = true;

                button_sub_3.Enabled = true;

            }
        }

        private void button_add_3_Click(object sender, EventArgs e)
        {
            output_vector_3 = vector1_3 + vector2_3;
            display_out_for3D(); 
        }
        public void display_out_for3D()
        {
            u_x_3.Text = output_vector_3._x[0].ToString();
            u_y_3.Text = output_vector_3._x[1].ToString();
            u_h.Text = output_vector_3._x[2].ToString();   


            button_add_3.Enabled = false;
            button_div_3.Enabled = false;
            button_equal_3.Enabled = false;
            button_less_equal_3.Enabled = false;
            button_mul_3.Enabled = false;

            button_sub_3.Enabled = false;

            button_clear_3.Focus(); 

        }

        private void button_clear_3_Click(object sender, EventArgs e)
        {
            u_x1_3.Text = "";
            u_x2_3.Text = "";
            u_y1_3.Text = "";
            u_y2_3.Text = "";
            u_h1.Text = "";
            u_h2.Text = ""; 
            u_x_3.Text = "";
            u_y_3.Text = "";
            u_h.Text = "";

            u_x1_3.Focus();

            button_add_3.Enabled = false;
            button_div_3.Enabled = false;
            button_equal_3.Enabled = false;
            button_less_equal_3.Enabled = false;
            button_mul_3.Enabled = false;

            button_sub_3.Enabled = false;

        }

        private void button_sub_3_Click(object sender, EventArgs e)
        {
            output_vector_3 = vector1_3 - vector2_3;
            display_out_for3D(); 
        }

        private void button_mul_3_Click(object sender, EventArgs e)
        {

            string str1="1";
            str1 = Interaction.InputBox("Please Enter a decimal value by which you need to Multiply the Vector 1:", "Decimal Value", "1", 30, 30);

            u_x2_3.Text = "";
            u_y2_3.Text = "";
            u_h2.Text = ""; 


            output_vector_3 = vector1_3 * double.Parse(str1);

           

            display_out_for3D(); 
        }

        private void button_div_3_Click(object sender, EventArgs e)
        {
            string str1="1";
            str1 = Interaction.InputBox("Please Enter a decimal value by which you need to divide the Vector 1:", "Decimal Value", "1", 30, 30);

            u_x2_3.Text = "";
            u_y2_3.Text = "";
            u_h2.Text = "";


            output_vector_3 = vector1_3 / double.Parse(str1);

            display_out_for3D(); 
 
        }

        private void button_equal_3_Click(object sender, EventArgs e)
        {
            if (vector1_3 == vector2_3)
            {
                MessageBox.Show("Both the Vectors Are Equal");
            }
            else
            {
                MessageBox.Show("Both the Vectors Are Not Equal");
            }
        }

        private void button_less_equal_3_Click(object sender, EventArgs e)
        {
            if (vector1_3 <= vector2_3)
            {
                MessageBox.Show("Vector1 <= Vector2");
            }
            else if (vector2_3 <= vector1_3)
            {
                MessageBox.Show("Vector2 <= Vector1");
            }        
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