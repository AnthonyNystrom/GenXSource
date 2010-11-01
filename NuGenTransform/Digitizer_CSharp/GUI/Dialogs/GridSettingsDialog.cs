using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class GridSettingsDialog : Form
    {
        private GridMeshSettings settings;
        private CoordSettings coordSettings;
        

        public GridSettingsDialog(GridMeshSettings settings, CoordSettings coordSettings)
        {            
            this.settings = settings;
            this.coordSettings = coordSettings;

            InitializeComponent();
            InitializeDefaults();

            this.MaximumSize = Size;
        }

        public void InitializeDefaults()
        {
            string[] gridSets = { "Count", "Start", "Step", "Stop" };
            xDisableCombo.Items.Clear();
            xDisableCombo.Items.AddRange(gridSets);
            yDisableCombo.Items.Clear();
            yDisableCombo.Items.AddRange(gridSets);

            switch (settings.gridSetX)
            {
                case GridSet.AllButCount:
                    xCount.Enabled = false; break;
                case GridSet.AllButStart:
                    xStart.Enabled = false; break;
                case GridSet.AllButStep:
                    xStep.Enabled = false; break;
                case GridSet.AllButStop:
                    xStop.Enabled = false; break;                    
            }

            xDisableCombo.SelectedIndex = (int)settings.gridSetX;

            switch (settings.gridSetY)
            {
                case GridSet.AllButCount:
                    yCount.Enabled = false; break;
                case GridSet.AllButStart:
                    yStart.Enabled = false; break;
                case GridSet.AllButStep:
                    yStep.Enabled = false; break;
                case GridSet.AllButStop:
                    yStop.Enabled = false; break;                    
            }

            yDisableCombo.SelectedIndex = (int)settings.gridSetY;

            xCount.Text = settings.countX.ToString();
            yCount.Text = settings.countY.ToString();

            xStart.Text = settings.startX.ToString();
            yStart.Text = settings.startY.ToString();

            xStep.Text = settings.stepX.ToString();
            yStep.Text = settings.stepY.ToString();

            xStop.Text = settings.stopX.ToString();
            yStop.Text = settings.stopY.ToString();
        }

        public GridMeshSettings Settings
        {
            get
            {
                settings.countX = int.Parse(xCount.Text);
                settings.countY = int.Parse(yCount.Text);

                settings.stepX = double.Parse(xStep.Text);
                settings.stepY = double.Parse(yStep.Text);

                settings.stopX = double.Parse(xStop.Text);
                settings.stopY = double.Parse(yStop.Text);

                settings.startX = int.Parse(xStart.Text);
                settings.startY = int.Parse(yStart.Text);


                settings.gridSetX = GetDisableX();
                settings.gridSetY = GetDisableY();

                return settings;
            }
        }

        private GridSet GetDisableX()
        {
            return (GridSet)xDisableCombo.SelectedIndex;
        }

        private GridSet GetDisableY()
        {
            return (GridSet)yDisableCombo.SelectedIndex;
        }

        private void xDisableCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            xCount.Enabled = true;
            xStep.Enabled = true;
            xStart.Enabled = true;
            xStop.Enabled = true;

            switch ((GridSet)xDisableCombo.SelectedIndex)
            {
                case GridSet.AllButCount:
                    xCount.Enabled = false; break;
                case GridSet.AllButStart:
                    xStart.Enabled = false; break;
                case GridSet.AllButStep:
                    xStep.Enabled = false; break;
                case GridSet.AllButStop:
                    xStop.Enabled = false; break;
            }

            settings.gridSetX = (GridSet)xDisableCombo.SelectedIndex;
        }

        private void yDisableCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            yCount.Enabled = true;
            yStep.Enabled = true;
            yStart.Enabled = true;
            yStop.Enabled = true;

            switch ((GridSet)yDisableCombo.SelectedIndex)
            {
                case GridSet.AllButCount:
                    yCount.Enabled = false; break;
                case GridSet.AllButStart:
                    yStart.Enabled = false; break;
                case GridSet.AllButStep:
                    yStep.Enabled = false; break;
                case GridSet.AllButStop:
                    yStop.Enabled = false; break;
            }

            settings.gridSetY = (GridSet)yDisableCombo.SelectedIndex;
        }

        private void XTextChanged(object sender, EventArgs e)
        {
            double step, start, stop;
            int count;

            try
            {
                step = double.Parse(xStep.Text);
                stop = double.Parse(xStop.Text);
                start = double.Parse(xStart.Text);
                count = int.Parse(xCount.Text);
            }
            catch (Exception ex)
            {
                return;
            }

            ComputeDisabledValue(coordSettings.xThetaScale == Genetibase.NuGenTransform.Scale.Linear, settings.gridSetX, ref count, ref start,
                ref step, ref stop);

            settings.stepX = step;
            settings.stopX = stop;
            settings.startX = start; 
            settings.countX = count;

            InitializeDefaults();
        }

        private void YTextChanged(object sender, EventArgs e)
        {
            double step, start, stop;
            int count;

            try
            {
                step = double.Parse(yStep.Text);
                stop = double.Parse(yStop.Text);
                start = double.Parse(yStart.Text);
                count = int.Parse(yCount.Text);
            }
            catch (Exception ex)
            {
                return;
            }

            ComputeDisabledValue(coordSettings.xThetaScale == Genetibase.NuGenTransform.Scale.Linear, settings.gridSetY, ref count, ref start,
                ref step, ref stop);

            settings.stepY = step;
            settings.stopY = stop;
            settings.startY = start;
            settings.countY = count;

            InitializeDefaults();
        }


        void ComputeDisabledValue(bool linear, GridSet gridSet, ref int count, ref double start,
                                        ref double step, ref double stop)
        {
            // validators prevent divide by zero errors and negative log errors from occurring below
            switch (gridSet)
            {
                case GridSet.AllButCount:
                    if (linear)
                        count = 1 + (int)(0.5 + (stop - start) / step);
                    else
                        count = 1 + (int)((Math.Log(stop) - Math.Log(start)) / Math.Log(step));
                    break;
                case GridSet.AllButStart:
                    if (linear)
                        start = stop - (count - 1.0) * step;
                    else
                        start = Math.Exp(Math.Log(stop) - (count - 1.0) * Math.Log(step));
                    break;
                case GridSet.AllButStep:
                    if (linear)
                        step = (stop - start) / (count - 1.0);
                    else
                        step = Math.Exp((Math.Log(stop) - Math.Log(start)) / (count - 1.0));
                    break;
                case GridSet.AllButStop:
                    if (linear)
                        stop = start + (count - 1.0) * step;
                    else
                        stop = Math.Exp(Math.Log(start) + (count - 1.0) * Math.Log(step));
                    break;
            }
        }
    }
}