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
    public partial class NuGen_Structures_Main : Form
    {
        public static int ind = 0;
        public NuGen_Structures_Main()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (option_box.Checked)
            {

                NuGen_Structures_Main.ind = 1;

                Genetibase.MathX.NuGenStructures.NuGen_structures next_form;
                next_form = new Genetibase.MathX.NuGenStructures.NuGen_structures();

                this.Visible = false;
                next_form.Visible = true; 
            }
            

            else if (option_Pnt.Checked)
            {

                NuGen_Structures_Main.ind = 2;

                Genetibase.MathX.NuGenStructures.NuGen_pnt  next_form;
                next_form = new Genetibase.MathX.NuGenStructures.NuGen_pnt();

                this.Visible = false;
                next_form.Visible = true;
            }

            else if (option_ray.Checked)
            {

                NuGen_Structures_Main.ind = 3;

                Genetibase.MathX.NuGenStructures.NuGen_rays  next_form;
                next_form = new Genetibase.MathX.NuGenStructures.NuGen_rays();

                this.Visible = false;
                next_form.Visible = true;
            }
            else if (option_rgb.Checked)
            {
                NuGen_Structures_Main.ind = 4;

                Genetibase.MathX.NuGenStructures.NuGenStructures_for_RGBA next_form = new Genetibase.MathX.NuGenStructures.NuGenStructures_for_RGBA();

                this.Visible  = false;
                next_form.Visible = true;

            }

            else if (option_vectors.Checked)
            {
                NuGen_Structures_Main.ind = 5;

                Genetibase.MathX.NuGenStructures.NuGen_Vectors next_form = new Genetibase.MathX.NuGenStructures.NuGen_Vectors ();

                this.Visible = false;
                next_form.Visible = true;





            }
            else if (option_trafo.Checked)
            {

                NuGen_Structures_Main.ind = 6;

                Genetibase.MathX.NuGenStructures.NuGen_Trafo  next_form;
                next_form = new Genetibase.MathX.NuGenStructures.NuGen_Trafo ();

                this.Visible = false;
                next_form.Visible = true;
            }
              
              
        }

        private void option_box_CheckedChanged(object sender, EventArgs e)
        {
           

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Application.Exit();  
        }

        private void option_ray_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void mathematicalOperationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NuGenScientificCalculator.NuGenSciCalc next_form = new NuGenScientificCalculator.NuGenSciCalc();

            //this.Visible = false;
            //next_form.Visible = true;
        }
    }
}