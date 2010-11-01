using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Genetibase.NuGenTransform
{
    public partial class ExportSettingsDialog : Form
    {
        private ExportSettings settings;
        private List<string> includedCurves;
        private List<string> excludedCurves;
        private NuGenPointSetCollection pointsets;

        private CoordSettings coordSettings;
        private GridMeshSettings gridSettings;        

        public ExportSettingsDialog(ExportSettings settings, NuGenDocument doc, List<string> includedCurves, List<string> excludedCurves)
        {
            this.settings = settings;
            this.includedCurves = includedCurves;
            this.excludedCurves = excludedCurves;
            this.pointsets = doc.PointSets;

            this.coordSettings = doc.CoordSettings;
            this.gridSettings = doc.GridDisplaySettings;

            InitializeComponent();
            InitializeDefaults();

            this.MaximumSize = Size;
        }

        public ExportSettings Settings
        {
            get
            {
                return settings;
            }
        }

        private void InitializeDefaults()
        {
            switch (settings.delimiters)
            {
                case ExportDelimiters.Commas:
                    commas.Checked = true; break;
                case ExportDelimiters.Spaces:
                    spaces.Checked = true; break;
                case ExportDelimiters.Tabs:
                    tabs.Checked = true; break;                
            }

            switch (settings.header)
            {
                case ExportHeader.HeaderGnuplot:
                    gnuplot.Checked = true; break;
                case ExportHeader.HeaderNone:
                    none.Checked = true; break;
                case ExportHeader.HeaderSimple:
                    simple.Checked = true; break;
            }

            switch (settings.layout)
            {
                case ExportLayout.AllCurvesOnEachLine:
                    allOneLine.Checked = true; break;
                case ExportLayout.OneCurveOnEachLine:
                    oneEachLine.Checked = true; break;
            }

            switch (settings.pointsSelection)
            {
                case ExportPointsSelection.XFromAllCurves:
                    intYXall.Checked = true; break;
                case ExportPointsSelection.XFromFirstCurve:
                    intYXFirst.Checked = true; break;
                case ExportPointsSelection.XFromGridLines:
                    intYGridline.Checked = true; break;
                case ExportPointsSelection.XYFromAllCurves:
                    rawXY.Checked = true; break;
            }

            CreatePreview();
        }

        private void AnythingChanged(object sender, System.EventArgs e)
        {
            if (intYXall.Checked)
                settings.pointsSelection = ExportPointsSelection.XFromAllCurves;
            else if (intYXFirst.Checked)
                settings.pointsSelection = ExportPointsSelection.XFromFirstCurve;
            else if (intYGridline.Checked)
                settings.pointsSelection = ExportPointsSelection.XFromGridLines;
            else if (rawXY.Checked)
                settings.pointsSelection = ExportPointsSelection.XYFromAllCurves;

            if (allOneLine.Checked)
                settings.layout = ExportLayout.AllCurvesOnEachLine;
            else
                settings.layout = ExportLayout.OneCurveOnEachLine;

            if (gnuplot.Checked)
                settings.header = ExportHeader.HeaderGnuplot;
            else if (none.Checked)
                settings.header = ExportHeader.HeaderNone;
            else
                settings.header = ExportHeader.HeaderSimple;

            if (commas.Checked)
                settings.delimiters = ExportDelimiters.Commas;
            else if (spaces.Checked)
                settings.delimiters = ExportDelimiters.Spaces;
            else
                settings.delimiters = ExportDelimiters.Tabs;

            listBox1.Items.Clear();
            listBox2.Items.Clear();

            foreach (string curve in includedCurves)
            {
                listBox1.Items.Add(curve);
            }

            foreach (string curve in excludedCurves)
            {
                listBox2.Items.Add(curve);
            }

            CreatePreview();
        }

        private void CreatePreview()
        {
            FileStream f = File.Create("xporttemp_1313_1313.tmp");

            pointsets.ExportToFile(f, coordSettings, gridSettings, settings);

            StreamReader reader = new StreamReader(File.Open("xporttemp_1313_1313.tmp", FileMode.Open));
            string text = reader.ReadToEnd();

            reader.Close();

            File.Delete("xporttemp_1313_1313.tmp");

            richTextBox1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (excludedCurves.Count == 0)
                return;

            string item = (string)listBox2.SelectedItem;

            excludedCurves.Remove(item);
            includedCurves.Add(item);

            AnythingChanged(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (includedCurves.Count == 0)
                return;

            string item = (string)listBox1.SelectedItem;

            excludedCurves.Add(item);
            includedCurves.Remove(item);

            AnythingChanged(sender, e);
        }
    }
}