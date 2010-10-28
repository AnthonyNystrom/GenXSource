using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Netron.GraphLib;
using GraphSynth.Representation;

namespace GraphSynth.Forms
{
    public partial class graphDisplay : Form
    {
        public designGraph graph1;

        #region Constructor
        public graphDisplay()
        {
            InitializeComponent();
            this.MdiParent = Program.main;
            graphControl1.LoadLibraries();
            graph1 = new designGraph();
        }
        public graphDisplay(designGraph initGraph)
        {
            InitializeComponent();
            this.MdiParent = Program.main;
            graphControl1.LoadLibraries();
            if (initGraph == null) graph1 = new designGraph();
            else graph1 = initGraph;
            this.Text = graph1.name;
            graph1.displayGraph(graphControl1, this.globalLabelsText);
        }
        public graphDisplay(designGraph initGraph, string formTitle)
        {
            InitializeComponent();
            this.MdiParent = Program.main;
            graphControl1.LoadLibraries();
            this.Text = formTitle;
            if (initGraph == null) graph1 = new designGraph();
            else graph1 = initGraph;
            graph1.displayGraph(graphControl1, this.globalLabelsText);
        }
        #endregion

        #region Connecting Events Functions
        private void graphControl1_OnShowProperties(object sender, object[] props)
        {
            Program.main.showProperties(graphControl1, graph1, sender, props);
        }
        public void graphDisplay_Enter(object sender, EventArgs e)
        {
            graph1.checkForRepeatNames();
            graph1.updateGraphControl(graphControl1, this.globalLabelsText);
            graph1.updateFromGraphControl(graphControl1);
            switch (Program.settings.defaultLayoutAlgorithm)
            {
                case defaultLayoutAlg.SpringEmbedder:
                    Program.main.layoutSpringEmbedderItem_Click(sender, e); break;
                case defaultLayoutAlg.Tree:
                    Program.main.layoutTreeItem_Click(sender, e); break;
                case defaultLayoutAlg.Random:
                    Program.main.layoutRandomizerItem_Click(sender, e); break;
                case defaultLayoutAlg.Custom1:
                    Program.main.customLayout0MenuItem_Click(sender, e); break;
                case defaultLayoutAlg.Custom2:
                    Program.main.customLayout1MenuItem_Click(sender, e); break;
                case defaultLayoutAlg.Custom3:
                    Program.main.customLayout2MenuItem_Click(sender, e); break;
            }
        }
        private void globalLabelsText_DoubleClick(object sender, EventArgs e)
        {
            Program.main.showProperties(graphControl1, graph1, sender, new object[1]);
        }
        #endregion


    }
}