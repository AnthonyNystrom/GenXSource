using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using Genetibase.MathX.Core;
using Genetibase.MathX.NugenCCalc;


namespace MathXDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void lblClearChart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            chart.GraphPane.CurveList.Clear();
            chart.Refresh();
        }

        #region Explicit

        private void btnCalcDerivative_Click(object sender, EventArgs e)
        {
            Function function = null;
            try
            {
                function = new Explicit2DFunction(txtExplicitFunction.Text);
                for (int i = 1; i <= nudDerivative.Value; i++)
                {
                    function = function.Derivative;
                }
                txtExplicitResult.Text = function.Expression;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void btnPlotDerivative_Click(object sender, EventArgs e)
        {
            try
            {
                NugenCCalc2D nugenCCalc2D = new NugenCCalc2D();
                nugenCCalc2D.ChartControl = chart;
                nugenCCalc2D.FunctionParameters = new Explicit2DParameters(txtExplicitResult.Text);
                nugenCCalc2D.Plot();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCalcValueAt_Click(object sender, EventArgs e)
        {
            try
            {
                Explicit2DFunction explicitFunction = null;
                explicitFunction = new Explicit2DFunction(txtExplicitFunction.Text);
                txtYValuleAt.Text = explicitFunction.ValueAt((double)nudValueAt.Value).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnPlotSource_Click(object sender, EventArgs e)
        {
            try
            {
                NugenCCalc2D nugenCCalc2D = new NugenCCalc2D();
                nugenCCalc2D.ChartControl = chart;
                nugenCCalc2D.FunctionParameters = new Explicit2DParameters(txtExplicitFunction.Text);
                nugenCCalc2D.Plot();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCalcInvert_Click(object sender, EventArgs e)
        {
            try
            {
                Explicit2DFunction function = new Explicit2DFunction(txtExplicitFunction.Text);
                Explicit2DFunction invertFunction = FunctionFactory.InverseExplicit2DFunction(function);
                txtInvertFunction.Text = invertFunction.Expression;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnPlotInvert_Click(object sender, EventArgs e)
        {
            try
            {
                NugenCCalc2D nugenCCalc2D = new NugenCCalc2D();
                nugenCCalc2D.ChartControl = chart;
                nugenCCalc2D.FunctionParameters = new Explicit2DParameters(txtInvertFunction.Text);
                nugenCCalc2D.Plot();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion


        #region Implicit

        private void btnPlotSourceImpl_Click(object sender, EventArgs e)
        {
            try
            {
                NugenCCalc2D nugenCCalc2D = new NugenCCalc2D();
                nugenCCalc2D.ChartControl = chart;
                nugenCCalc2D.FunctionParameters = new Implicit2DParameters(txtImplicitFunction.Text);
                nugenCCalc2D.Plot();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Function function = null;
            try
            {
                function = new Implicit2DFunction(txtImplicitFunction.Text);
                for (int i = 1; i <= nudDerivativeImpl.Value; i++)
                {
                    function = function.Derivative;
                }
                txtImplicitResult.Text = function.Expression;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void btnPlotDerivativeImpl_Click(object sender, EventArgs e)
        {
            try
            {
                NugenCCalc2D nugenCCalc2D = new NugenCCalc2D();
                nugenCCalc2D.ChartControl = chart;
                nugenCCalc2D.FunctionParameters = new Implicit2DParameters(txtImplicitResult.Text);
                nugenCCalc2D.Plot();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion

        #region Parametric

        private void btnPlotParamSource_Click(object sender, EventArgs e)
        {
            try
            {
                NugenCCalc2D nugenCCalc2D = new NugenCCalc2D();
                nugenCCalc2D.ChartControl = chart;
                nugenCCalc2D.FunctionParameters = new Parametric2DParameters(txtSourceX.Text, txtSourceY.Text);
                nugenCCalc2D.Plot();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCalcParametric_Click(object sender, EventArgs e)
        {
            try
            {
                Parameter2DFunction paramFunction = null;
                paramFunction = new Parameter2DFunction(txtSourceX.Text, txtSourceY.Text);
                Point2D point = paramFunction.ValueAt((double)nudtValue.Value);
                txtXValue.Text = point.X.ToString();
                txtYValue.Text = point.Y.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MathX Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion


    }
}