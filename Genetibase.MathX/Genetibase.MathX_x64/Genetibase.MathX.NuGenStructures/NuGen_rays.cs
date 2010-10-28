using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic; 


namespace Genetibase.MathX.NuGenStructures
{
    public partial class NuGen_rays : Form
    {

        public Genetibase.MathX.NuGenStructures.NuGenRay2D ray1;
        public Genetibase.MathX.NuGenStructures.NuGenRay3D ray;
  
        public NuGen_rays()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (u_x1.Text == "" || u_x2.Text == "" || u_y1.Text == "" || u_y2.Text == "" )
            {
                MessageBox.Show("Please Input All The Co-ordinates.");

            }
            else
            {
                Genetibase.MathX.NuGenStructures.NuGenPnt2D obj1 = new Genetibase.MathX.NuGenStructures.NuGenPnt2D();
                Genetibase.MathX.NuGenStructures.NuGenVec2D obj2 = new Genetibase.MathX.NuGenStructures.NuGenVec2D();

                obj1._x = new double[2];
                obj2._x = new double[2];

                obj1._x[0] = double.Parse(u_x1.Text);
                obj1._x[1] = double.Parse(u_y1.Text);

                obj2._x[0] = double.Parse(u_x2.Text);
                obj2._x[1] = double.Parse(u_y2.Text);

                ray1 = new NuGenRay2D(obj1, obj2);

                button_add.Enabled = true;
  
                
                }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string str1 = "1";
            str1 = Interaction.InputBox("Please Enter a Float value of which you need Find The Position :", "Float Value", "1", 30, 30);
            Genetibase.MathX.NuGenStructures.NuGenPnt2D obj1 = new Genetibase.MathX.NuGenStructures.NuGenPnt2D();

            obj1 = ray1.GetPointOnRay(double.Parse(str1));

            u_x.Text = obj1._x[0].ToString();
            u_y.Text = obj1._x[1].ToString(); 

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (u_x1_3.Text == "" || u_x2_3.Text == "" || u_y1_3.Text == "" || u_y2_3.Text == "")
            {
                MessageBox.Show("Please Input All The Co-ordinates.");

            }
            else
            {
                Genetibase.MathX.NuGenStructures.NuGenPnt3D obj1 = new Genetibase.MathX.NuGenStructures.NuGenPnt3D();
                Genetibase.MathX.NuGenStructures.NuGenVec3D obj2 = new Genetibase.MathX.NuGenStructures.NuGenVec3D();

                obj1._x = new double[3];
                obj2._x = new double[3];

                obj1._x[0] = double.Parse(u_x1_3.Text);
                obj1._x[1] = double.Parse(u_y1_3.Text);
                obj1._x[2] = double.Parse(u_h1.Text);     

                obj2._x[0] = double.Parse(u_x2_3.Text);
                obj2._x[1] = double.Parse(u_y2_3.Text);
                obj2._x[2] = double.Parse(u_h2.Text);
    

                ray = new NuGenRay3D(obj1, obj2);

                button_add_3.Enabled = true;


            }
        }

        private void button_add_3_Click(object sender, EventArgs e)
        {
            string str1 = "1";
            str1 = Interaction.InputBox("Please Enter a Float value of which you need Find The Position :", "Float Value", "1", 30, 30);
            Genetibase.MathX.NuGenStructures.NuGenPnt3D obj1 = new Genetibase.MathX.NuGenStructures.NuGenPnt3D();

            obj1 = ray.GetPointOnRay(double.Parse(str1));

            u_x_3.Text = obj1._x[0].ToString();
            u_y_3.Text = obj1._x[1].ToString();
            u_h.Text = obj1._x[2].ToString();    


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