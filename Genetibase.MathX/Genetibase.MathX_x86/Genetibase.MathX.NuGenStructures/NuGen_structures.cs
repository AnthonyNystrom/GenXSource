using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.MathX.NuGenStructures;


namespace Genetibase.MathX.NuGenStructures
{
    public partial class NuGen_structures : Form
    {

        public Genetibase.MathX.NuGenStructures.NuGenBox2D box1,box2,out_put;
        
        public Genetibase.MathX.NuGenStructures.NuGenBox2F box1_f, box2_f, out_put_f;
        public Genetibase.MathX.NuGenStructures.NuGenBox3F box1_3_f, box2_3_f, output_f; 

        public Genetibase.MathX.NuGenStructures.NuGenBox3D box1_3, box2_3, output;

        

        //public Genetibase.MathX.NuGenStructures.NuGenBox3F box1_3_f, box2_3_f, output_f; 


        public NuGen_structures()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (Genetibase.MathX.NuGenStructures.NuGen_Structures_Main.ind == 1)
            {


                if (u_x1.Text == "" || u_x2.Text == "" || u_y1.Text == "" || u_y2.Text == "" || l_x1.Text == "" || l_x2.Text == "" || l_y1.Text == "" || l_y2.Text == "")
                {
                    MessageBox.Show("Please Input All The Co-ordinates.");

                }
                else
                {
                    button_add.Enabled = true;

                    //NuGenBox2D lower_u, upper_u;


                    box1 = new Genetibase.MathX.NuGenStructures.NuGenBox2D();
                    box2 = new Genetibase.MathX.NuGenStructures.NuGenBox2D();

                    //upper_u = new NuGenBox2D();

                    box1.lower = new NuGenPnt2D();
                    box1.lower._x = new Double[2];

                    box1.lower._x[0] = (double.Parse(l_x1.Text));
                    box1.lower._x[1] = (double.Parse(l_y1.Text));

                    box1.upper._x = new Double[2];
                    box1.upper._x[0] = (double.Parse(u_x1.Text));
                    box1.upper._x[1] = double.Parse(u_y1.Text);

                    box2.lower._x = new Double[2];
                    box2.lower._x[0] = double.Parse(l_x2.Text);
                    box2.lower._x[1] = double.Parse(l_y2.Text);

                    box2.upper._x = new Double[2];
                    box2.upper._x[0] = double.Parse(u_x2.Text);
                    box2.upper._x[1] = double.Parse(u_y2.Text);


                }
            }
            else if (Genetibase.MathX.NuGenStructures.NuGen_Structures_Main.ind == 2)
            {
                if (u_x1.Text == "" || u_x2.Text == "" || u_y1.Text == "" || u_y2.Text == "" || l_x1.Text == "" || l_x2.Text == "" || l_y1.Text == "" || l_y2.Text == "")
                {
                    MessageBox.Show("Please Input All The Co-ordinates.");

                }
                else
                {
                    button_add.Enabled = true;

                    //NuGenBox2D lower_u, upper_u;


                    box1_f = new Genetibase.MathX.NuGenStructures.NuGenBox2F();
                    box2_f= new Genetibase.MathX.NuGenStructures.NuGenBox2F();

                    //upper_u = new NuGenBox2D();

                    box1_f.lower = new NuGenPnt2F();
                    box1_f.lower._x = new float[2];

                    box1_f.lower._x[0] = (float.Parse(l_x1.Text));
                    box1_f.lower._x[1] = (float.Parse(l_y1.Text));

                    box1_f.upper._x = new float[2];
                    box1_f.upper._x[0] = (float.Parse(u_x1.Text));
                    box1_f.upper._x[1] = float.Parse(u_y1.Text);

                    box2_f.lower._x = new float[2];
                    box2_f.lower._x[0] = float.Parse(l_x2.Text);
                    box2_f.lower._x[1] = float.Parse(l_y2.Text);

                    box2_f.upper._x = new float[2];
                    box2_f.upper._x[0] = float.Parse(u_x2.Text);
                    box2_f.upper._x[1] = float.Parse(u_y2.Text);


                }

            }
           
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (Genetibase.MathX.NuGenStructures.NuGen_Structures_Main.ind == 1)
            {
                out_put = box1 + box2;
                //double temp;


                l_x.Text = out_put.lower._x[0].ToString();
                l_y.Text = out_put.lower._x[1].ToString();
                u_x.Text = out_put.upper._x[0].ToString();
                u_y.Text = out_put.upper._x[1].ToString();

            }
            else if (Genetibase.MathX.NuGenStructures.NuGen_Structures_Main.ind == 2)
            {
                out_put_f = box1_f + box2_f;
                //double temp;


                l_x.Text = out_put_f.lower._x[0].ToString();
                l_y.Text = out_put_f.lower._x[1].ToString();
                u_x.Text = out_put_f.upper._x[0].ToString();
                u_y.Text = out_put_f.upper._x[1].ToString();

            }
        }

        private void NuGen_structures_Load(object sender, EventArgs e)
        {

        }

        private void button_add_3_Click(object sender, EventArgs e)
        {
            if (Genetibase.MathX.NuGenStructures.NuGen_Structures_Main.ind == 1)
            {
                output = box1_3 + box2_3;
                //double temp;


                l_x_3.Text = output.lower._x[0].ToString();
                l_y_3.Text = output.lower._x[1].ToString();
                l_h.Text = output.lower._x[2].ToString();

                u_x_3.Text = output.upper._x[0].ToString();
                u_y_3.Text = output.upper._x[1].ToString();
                //u_h.Text = output.upper ._x[2].ToString(); 

                u_h.Text = l_h.Text;

            }
            else if (Genetibase.MathX.NuGenStructures.NuGen_Structures_Main.ind == 2)
            {

                output_f = box1_3_f + box2_3_f;
                //double temp;


                l_x_3.Text = output_f.lower._x[0].ToString();
                l_y_3.Text = output_f.lower._x[1].ToString();
                l_h.Text = output_f.lower._x[2].ToString();

                u_x_3.Text = output_f.upper._x[0].ToString();
                u_y_3.Text = output_f.upper._x[1].ToString();
                //u_h.Text = output_f.upper ._x[2].ToString(); 

                u_h.Text = l_h.Text;

            }

             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Genetibase.MathX.NuGenStructures.NuGen_Structures_Main.ind == 1)
            {
                if (u_x1_3.Text == "" || u_x2_3.Text == "" || u_h1.Text == "" || u_h2.Text == "" || u_y1_3.Text == "" || u_y2_3.Text == "" || l_x1_3.Text == "" || l_x2_3.Text == "" || l_h1.Text == "" || l_h2.Text == "" || l_y1_3.Text == "" || l_y2_3.Text == "")
                {

                    MessageBox.Show("Please Input All The Co-ordinates.");


                }
                else
                {

                    button_add_3.Enabled = true;

                    //NuGenBox2D lower_u, upper_u;


                    box1_3 = new Genetibase.MathX.NuGenStructures.NuGenBox3D();
                    box2_3 = new Genetibase.MathX.NuGenStructures.NuGenBox3D();

                    //upper_u = new NuGenBox2D();

                    //box1_3.lower = new NuGenPnt3D();


                    box1_3.lower._x = new Double[3];

                    box1_3.lower._x[0] = (double.Parse(l_x1_3.Text));
                    box1_3.lower._x[1] = (double.Parse(l_y1_3.Text));
                    box1_3.lower._x[2] = (double.Parse(l_h1.Text));


                    box1_3.upper._x = new Double[3];
                    box1_3.upper._x[0] = (double.Parse(u_x1_3.Text));
                    box1_3.upper._x[1] = double.Parse(u_y1_3.Text);
                    box1_3.lower._x[2] = (double.Parse(u_h1.Text));


                    box2_3.lower._x = new Double[3];
                    box2_3.lower._x[0] = double.Parse(l_x2_3.Text);
                    box2_3.lower._x[1] = double.Parse(l_y2_3.Text);
                    box2_3.lower._x[2] = (double.Parse(l_h2.Text));

                    box2_3.upper._x = new Double[3];
                    box2_3.upper._x[0] = double.Parse(u_x2_3.Text);
                    box2_3.upper._x[1] = double.Parse(u_y2_3.Text);
                    box2_3.lower._x[2] = (double.Parse(u_h2.Text));



                }
            }
            else if(Genetibase.MathX.NuGenStructures.NuGen_Structures_Main.ind ==2)    
            {
                if (u_x1_3.Text == "" || u_x2_3.Text == "" || u_h1.Text == "" || u_h2.Text == "" || u_y1_3.Text == "" || u_y2_3.Text == "" || l_x1_3.Text == "" || l_x2_3.Text == "" || l_h1.Text == "" || l_h2.Text == "" || l_y1_3.Text == "" || l_y2_3.Text == "")
                {

                    MessageBox.Show("Please Input All The Co-ordinates.");


                }
                else
                {

                    button_add_3.Enabled = true;

                    //NuGenBox2D lower_u, upper_u;


                    box1_3_f = new Genetibase.MathX.NuGenStructures.NuGenBox3F();
                    box2_3_f = new Genetibase.MathX.NuGenStructures.NuGenBox3F();

                    //upper_u = new NuGenBox2D();

                    //box1_3.lower = new NuGenPnt3D();


                    box1_3_f.lower._x = new float[3];

                    box1_3_f.lower._x[0] = (float.Parse(l_x1_3.Text));
                    box1_3_f.lower._x[1] = (float.Parse(l_y1_3.Text));
                    box1_3_f.lower._x[2] = (float.Parse(l_h1.Text));


                    box1_3_f.upper._x = new float[3];
                    box1_3_f.upper._x[0] = (float.Parse(u_x1_3.Text));
                    box1_3_f.upper._x[1] = float.Parse(u_y1_3.Text);
                    box1_3_f.lower._x[2] = (float.Parse(u_h1.Text));


                    box2_3_f.lower._x = new float[3];
                    box2_3_f.lower._x[0] = float.Parse(l_x2_3.Text);
                    box2_3_f.lower._x[1] = float.Parse(l_y2_3.Text);
                    box2_3_f.lower._x[2] = (float.Parse(l_h2.Text));

                    box2_3_f.upper._x = new float[3];
                    box2_3_f.upper._x[0] = float.Parse(u_x2_3.Text);
                    box2_3_f.upper._x[1] = float.Parse(u_y2_3.Text);
                    box2_3_f.lower._x[2] = (float.Parse(u_h2.Text));

                }

            }
        }

        private void u_y_TextChanged(object sender, EventArgs e)
        {

        }

        private void u_y2_TextChanged(object sender, EventArgs e)
        {

        }

        private void u_y1_TextChanged(object sender, EventArgs e)
        {

        }

        private void u_x2_TextChanged(object sender, EventArgs e)
        {

        }

        private void u_x_TextChanged(object sender, EventArgs e)
        {

        }

        private void u_x1_TextChanged(object sender, EventArgs e)
        {

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