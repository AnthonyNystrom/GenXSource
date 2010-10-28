using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace GenetiBase.MathX.NuGenStatistic
{
    public partial class NuGenStatistic_Form : Form
    {
         
           public double[] list1 ;
        public NuGenStatistic_Form()
        {
            InitializeComponent();
        }

        private void NuGenStatistic_Form_Load(object sender, EventArgs e)
        {
           // GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new NuGenStatistics();
            

           // GenetiBase.MathX.NuGenStatistic.NuGenStatistics.a;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text =obj1.min().ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.b(obj1).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int j;
            int i;
            i = 0;
            j = int.Parse(l1.Text);
            list1=new double [j];
            string str = null;
           

            for (i = 0; i < j; i++)
            {
                str = Interaction.InputBox("Enter Values For List", "", "", 30, 30);
                list1[i] = double.Parse(str);
                str = null;
            }

        }

        private void range_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.range ().ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.middle_of_range ().ToString();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.max ().ToString();
        }

        private void mean_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.mean ().ToString();
        }

        private void coverience_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.cov(obj1).ToString();
        }

        private void length_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.length ().ToString();
        }

        private void mode_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.mode ().ToString();
        }

        private void a_factor_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.a(obj1).ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void inter_range_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.IQ().ToString();
        }

        private void Standard_Deviation_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.s ().ToString();
        }

        private void index_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.YULE ().ToString();
        }

        
        private void corelation_coefficient_Click(object sender, EventArgs e)
        {
            GenetiBase.MathX.NuGenStatistic.NuGenStatistics obj1 = new GenetiBase.MathX.NuGenStatistic.NuGenStatistics(list1);
            res.Text = obj1.r(obj1).ToString();
        }

        private void clear_Click(object sender, EventArgs e)
        {
            l1.Text = "";
            //l2.Text ="";
            res.Text ="";

            l1.Focus ();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Application.Exit();  
        }

        private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NuGenScientificCalculator.NuGenSciCalc next_form = new NuGenScientificCalculator.NuGenSciCalc();

            //this.Visible = false;
            //next_form.Visible = true;
        }
    }
}