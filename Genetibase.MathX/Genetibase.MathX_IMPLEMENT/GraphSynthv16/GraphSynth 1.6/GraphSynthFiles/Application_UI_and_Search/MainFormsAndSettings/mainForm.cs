using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Reflection;
using GraphSynth.Representation;

using System.Drawing.Printing;

namespace GraphSynth.Forms
{
    public partial class mainForm : Form
    {
        #region Fields
        private List<graphDisplay> graphDisplays = new List<graphDisplay>();
        public List<grammarRuleDisplay> grammarDisplays = new List<grammarRuleDisplay>();
        public List<ruleSetDisplay> ruleSetDisplays = new List<ruleSetDisplay>();
        public List<searchProcessController> searchControls = new List<searchProcessController>();
        private graphLayout customLayouts = new graphLayout();
        public PropertiesWindow propWindow;
        Netron.GraphLib.UI.GraphControl dummyGC = new Netron.GraphLib.UI.GraphControl();
        /* see the properties activeGraphControlChild for the reason for this. */
        #endregion

        #region Constructor
        public mainForm()
        {
            InitializeComponent();

            Type gLayoutType = customLayouts.GetType();
            MethodInfo[] customMethods = gLayoutType.GetMethods();
            if (customMethods.GetLength(0) > 6)
            {
                string a = customMethods[2].ToString();
                customLayout2MenuItem.Text = a.Substring(5, a.Length - 62);
                customLayout2MenuItem.Visible = true;
            }
            if (customMethods.GetLength(0) > 5)
            {
                string a = customMethods[1].ToString();
                customLayout1MenuItem.Text = a.Substring(5, a.Length - 62);
                customLayout1MenuItem.Visible = true;
            }
            if (customMethods.GetLength(0) > 4)
            {
                string a = customMethods[0].ToString();
                customLayout0MenuItem.Text = a.Substring(5, a.Length - 62);
                customLayout0MenuItem.Visible = true;
            }
        }
        #endregion

        #region Properties
        [XmlIgnore]
        public Netron.GraphLib.UI.GraphControl activeGraphControlChild
        {
            /* this propertiessets activeDisplayForm to whatever graphDisplay is
             * currently active or the last to be active. */
            get
            {
                if (this.ActiveMdiChild == null) return dummyGC;
                /* this is a little hacky, it makes the unseen graphControl to avoid errors
                 * that may exist in functions that rely on activeGraphControlChild to return
                 * a non-null form. */
                else if ((this.ActiveMdiChild.GetType()) == (typeof(graphDisplay)))
                {
                    graphDisplay a = this.ActiveMdiChild as graphDisplay;
                    return a.graphControl1;
                }
                else if ((this.ActiveMdiChild.GetType()) == (typeof(grammarRuleDisplay)))
                {
                    grammarRuleDisplay a = this.ActiveMdiChild as grammarRuleDisplay;
                    if (a.graphControlLHS.ContainsFocus == true)
                        return a.graphControlLHS;
                    else return a.graphControlRHS;
                }
                else return graphDisplays[graphChildCount].graphControl1;
            }
        }
        [XmlIgnore]
        public designGraph activeGraph
        {
            get
            {
                if (this.ActiveMdiChild == null) return null;
                else if ((this.ActiveMdiChild.GetType()) == (typeof(graphDisplay)))
                {
                    graphDisplay a = this.ActiveMdiChild as graphDisplay;
                    return a.graph1;
                }
                else if ((this.ActiveMdiChild.GetType()) == (typeof(grammarRuleDisplay)))
                {
                    grammarRuleDisplay a = this.ActiveMdiChild as grammarRuleDisplay;
                    if (a.graphControlLHS.ContainsFocus == true)
                        return a.rule.L;
                    else return a.rule.R;
                }
                else return graphDisplays[graphChildCount].graph1;
            }
        }
        public int graphChildCount
        {
            get { return graphDisplays.Count - 1; }
        }
        public int grammarChildCount
        {
            get { return grammarDisplays.Count - 1; }
        }
        public int ruleSetChildCount
        {
            get { return ruleSetDisplays.Count - 1; }
        }
        #endregion

        #region File
        #region Print

        #endregion
        #region New
        private void newGraphItem_Click(object sender, EventArgs e)
        {
            int numPlusOne = graphChildCount + 1;
            addAndShowGraphDisplay(null, "Graph Display Window #" + numPlusOne.ToString());
        }
        private void newGrammar_Click(object sender, EventArgs e)
        {
            int numPlusOne = grammarChildCount + 1;
            grammarDisplays.Add(new grammarRuleDisplay("Grammar Rule Display #"
                + numPlusOne.ToString()));
            grammarDisplays[grammarChildCount].Show();
        }
        private void newRuleSetMenuItem_Click(object sender, EventArgs e)
        {
            int numPlusOne = ruleSetChildCount + 1;
            ruleSetDisplays.Add(new ruleSetDisplay(new ruleSet(), "Rule Set Display #"
                + numPlusOne.ToString()));
            ruleSetDisplays[ruleSetChildCount].Show();
        }
        #endregion
        #region Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename;
            try
            {
                filename = getOpenFilename(Program.settings.workingDirectory);
            }
            catch { filename = ""; }
            if (filename != "" && filename != null)
            {
                /* Load the file. */
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                /* create prefix<->namespace mappings (if any)  */
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
                /* Query the document */
                if (doc.SelectNodes("/grammarRule", nsMgr).Count > 0)
                {
                    grammarRule rule = grammarRule.openRuleFromXml(filename);
                    grammarDisplays.Add(new grammarRuleDisplay(rule, Path.GetFileName(filename)));
                    grammarDisplays[grammarChildCount].Show();
                }
                else if (doc.SelectNodes("/ruleSet", nsMgr).Count > 0)
                {
                    ruleSet rs = ruleSet.openRuleSetFromXml(filename, Program.settings.defaultGenSteps);
                    ruleSetDisplays.Add(new ruleSetDisplay(rs, Path.GetFileName(filename)));
                    ruleSetDisplays[ruleSetChildCount].Show();
                }
                else if (doc.SelectNodes("/designGraph", nsMgr).Count > 0)
                {
                    designGraph graph = designGraph.openGraphFromXml(filename);
                    addAndShowGraphDisplay(graph, Path.GetFileName(filename));
                }
                else if (doc.SelectNodes("/candidate", nsMgr).Count > 0)
                {
                    string tempString = "";
                    candidate c = candidate.openCandidateFromXml(filename);
                    SearchIO.output("The candidate found in " + filename, 0);
                    if (c.performanceParams.Count > 0)
                    {
                        tempString = "has the following performance parameters";
                        foreach (double a in c.performanceParams)
                            tempString += ": " + a.ToString();
                        SearchIO.output(tempString, 0);
                    }
                    if (c.age > 0)
                        SearchIO.output("The candidate has existed for " + c.age.ToString() + " iterations.", 0);
                    SearchIO.output("It's generation ended in RuleSet #" + c.activeRuleSetIndex.ToString(), 0);
                    tempString = "Generation terminated with";
                    foreach (GenerationStatuses a in c.GenerationStatus)
                        tempString += ": " + a.ToString();
                    SearchIO.output(tempString, 0);

                    addAndShowGraphDisplay(c.graph, "Candidate in " + Path.GetFileName(filename));
                }
                else
                {
                    MessageBox.Show("The XML files that you have attempted to open contains an unknown" +
                        "type (not designGraph, grammarRule, ruleSet, or candidate).", "XML Serialization Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        public string getOpenFilename(string dir)
        {
            try
            {
                OpenFileDialog fileChooser = new OpenFileDialog();
                fileChooser.Title = "Open a graph, rule, or rule set from ...";
                fileChooser.InitialDirectory = dir;
                fileChooser.Filter = "(xml files)|*.xml";
                DialogResult result = fileChooser.ShowDialog();
                if (result == DialogResult.Cancel) return "";
                return fileChooser.FileName;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }
        #endregion
        #region Save
        public void saveCurrentItem_Click(object sender, EventArgs e)
        {
            string filename;
            string name;
            if (this.ActiveMdiChild.GetType() == typeof(graphDisplay))
            {
                graphDisplay saveDisplay = (graphDisplay)this.ActiveMdiChild;
                saveDisplay.graphDisplay_Enter(sender, e);
                name = saveDisplay.graph1.name;
                try { filename = getSaveFilename("graph", name, Program.settings.workingDirectory); }
                catch { filename = ""; }
                if (filename != "" && filename != null)
                    designGraph.saveGraphToXml(filename, saveDisplay.graphControl1, saveDisplay.graph1);
            }
            else if (this.ActiveMdiChild.GetType() == typeof(grammarRuleDisplay))
            {
                grammarRuleDisplay saveDisplay = (grammarRuleDisplay)this.ActiveMdiChild;
                name = saveDisplay.rule.name;
                try { filename = getSaveFilename("grammar rule", name, Program.settings.rulesDirectory); }
                catch { filename = ""; }
                if (filename != "" && filename != null)
                    grammarRule.saveRuleToXml(filename, saveDisplay.rule, saveDisplay.graphControlLHS,
                        saveDisplay.graphControlRHS, saveDisplay.globalLabelsLText, saveDisplay.globalLabelsRText);
            }
            else if (this.ActiveMdiChild.GetType() == typeof(ruleSetDisplay))
            {
                ruleSetDisplay saveDisplay = (ruleSetDisplay)this.ActiveMdiChild;
                name = saveDisplay.ruleset.name;
                try { filename = getSaveFilename("rule set", name, Program.settings.rulesDirectory); }
                catch { filename = ""; }
                if (filename != "" && filename != null)
                    ruleSet.saveRuleSetToXml(filename, saveDisplay.ruleset);
            }
            else
                MessageBox.Show("Please select an window that contains a graph, rule, or rule set.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private string getSaveFilename(string fileType, string name, string dir)
        {
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.Title = "Save Active " + fileType + " as ...";
            fileChooser.InitialDirectory = dir;
            fileChooser.Filter = fileType + " file (*.xml)|*.xml";
            fileChooser.FileName = name;
            DialogResult result = fileChooser.ShowDialog();
            string filename;
            fileChooser.CheckFileExists = false;
            if (result == DialogResult.Cancel) return "";
            filename = fileChooser.FileName;
            if (filename == "" || filename == null)
                MessageBox.Show("Invalid file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return filename;
        }
        #endregion
        #region Print
        private void printPreviewMenuItem_Click_1(object sender, EventArgs e)
        {
            if ((this.ActiveMdiChild.GetType()) == (typeof(graphDisplay)))
            {
                graphDisplay g = this.ActiveMdiChild as graphDisplay;
                PrintDocument p = new PrintDocument();
                p.PrintPage += new PrintPageEventHandler(g.graphControl1.PrintCanvas);
                PrintPreviewDialog prev = new PrintPreviewDialog();
                prev.Document = p;
                p.DefaultPageSettings.Color = true;
                p.DefaultPageSettings.Landscape = true;
                p.DefaultPageSettings.Margins.Left = 100;
                p.DefaultPageSettings.Margins.Right = 100;
                p.DefaultPageSettings.Margins.Top = 100;
                p.DefaultPageSettings.Margins.Bottom = 100;
                prev.ShowDialog(g);


            }
            else
            {
                MessageBox.Show("Error", "Please select a graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Settings
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            globalSettingsDisplay settingsDisplay = new globalSettingsDisplay();
            settingsDisplay.MdiParent = this;
            settingsDisplay.Show();
        }
        #endregion
        #region Close
        public void closeCurrentItem_Click(object sender, EventArgs e)
        {
            Form formToClose = this.ActiveMdiChild;
            if (formToClose == null) return;
            if ((formToClose.GetType()) == (typeof(graphDisplay)))
            {
                graphDisplays.Remove((graphDisplay)formToClose);
            }
            else if ((formToClose.GetType()) == (typeof(grammarRuleDisplay)))
            {
                grammarDisplays.Remove((grammarRuleDisplay)formToClose);
            }
            else if ((formToClose.GetType()) == (typeof(ruleSetDisplay)))
            {
                ruleSetDisplays.Remove((ruleSetDisplay)formToClose);
            }
            formToClose.Close();
        }
        private void closeGraphsItem_Click(object sender, EventArgs e)
        {
            for (int i = graphDisplays.Count; i != 0; i--)
                graphDisplays[i - 1].Close();
            graphDisplays.Clear();
        }
        private void exitItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #endregion

        #region View
        #region Zoom
        private void zoom10MenuItem_Click(object sender, EventArgs e)
        {
            this.activeGraphControlChild.Zoom = 0.10f;
            this.activeGraphControlChild.Refresh();
        }
        private void zoom25MenuItem_Click(object sender, EventArgs e)
        {
            this.activeGraphControlChild.Zoom = 0.25f;
            this.activeGraphControlChild.Refresh();
        }
        private void zoom50MenuItem_Click(object sender, EventArgs e)
        {
            this.activeGraphControlChild.Zoom = 0.50f;
            this.activeGraphControlChild.Refresh();
        }
        private void zoom75MenuItem_Click(object sender, EventArgs e)
        {
            this.activeGraphControlChild.Zoom = 0.75f;
            this.activeGraphControlChild.Refresh();
        }
        private void zoom100MenuItem_Click(object sender, EventArgs e)
        {
            this.activeGraphControlChild.Zoom = 1.0f;
            this.activeGraphControlChild.Refresh();
        }
        private void zoom150MenuItem_Click(object sender, EventArgs e)
        {
            this.activeGraphControlChild.Zoom = 1.50f;
            this.activeGraphControlChild.Refresh();
        }
        private void zoom200MenuItem_Click(object sender, EventArgs e)
        {
            this.activeGraphControlChild.Zoom = 2.0f;
            this.activeGraphControlChild.Refresh();
        }
        private void zoom400MenuItem_Click(object sender, EventArgs e)
        {
            this.activeGraphControlChild.Zoom = 4.0f;
            this.activeGraphControlChild.Refresh();
        }
        private void zoomUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch ((int)(100 * (this.activeGraphControlChild.Zoom)))
            {
                case 10: this.activeGraphControlChild.Zoom = 0.25f; break;
                case 25: this.activeGraphControlChild.Zoom = 0.50f; break;
                case 50: this.activeGraphControlChild.Zoom = 0.75f; break;
                case 75: this.activeGraphControlChild.Zoom = 1.0f; break;
                case 100: this.activeGraphControlChild.Zoom = 1.25f; break;
                case 125: this.activeGraphControlChild.Zoom = 1.50f; break;
                case 150: this.activeGraphControlChild.Zoom = 2.0f; break;
                case 200: this.activeGraphControlChild.Zoom = 4.0f; break;
            }
            this.activeGraphControlChild.Refresh();
        }

        private void zoomDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch ((int)(100 * (this.activeGraphControlChild.Zoom)))
            {
                case 25: this.activeGraphControlChild.Zoom = 0.10f; break;
                case 50: this.activeGraphControlChild.Zoom = 0.25f; break;
                case 75: this.activeGraphControlChild.Zoom = 0.50f; break;
                case 100: this.activeGraphControlChild.Zoom = 0.75f; break;
                case 125: this.activeGraphControlChild.Zoom = 1.0f; break;
                case 150: this.activeGraphControlChild.Zoom = 1.25f; break;
                case 200: this.activeGraphControlChild.Zoom = 1.50f; break;
                case 400: this.activeGraphControlChild.Zoom = 2.0f; break;
            }
            this.activeGraphControlChild.Refresh();
        }
        #endregion
        #region Text
        private void hideTextMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Shape a in this.activeGraphControlChild.Shapes)
                a.ShowLabel = false;
            foreach (Connection a in this.activeGraphControlChild.Connections)
                a.ShowLabel = false;
            this.activeGraphControlChild.Refresh();
        }

        private void shrinkTextMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Shape a in this.activeGraphControlChild.Shapes)
            {
                a.ShowLabel = true;
                a.Font = new Font(a.Font.FontFamily, a.Font.Size / (1.2f));
            }
            foreach (Connection a in this.activeGraphControlChild.Connections)
            {
                a.ShowLabel = true;
                a.Font = new Font(a.Font.FontFamily, a.Font.Size / (1.2f));
            }
            this.activeGraphControlChild.Refresh();
        }

        private void enlargeTextMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Shape a in this.activeGraphControlChild.Shapes)
            {
                a.ShowLabel = true;
                a.Font = new Font(a.Font.FontFamily, (1.2f) * a.Font.Size);
            }
            foreach (Connection a in this.activeGraphControlChild.Connections)
            {
                a.ShowLabel = true;
                a.Font = new Font(a.Font.FontFamily, (1.2f) * a.Font.Size);
            }
            this.activeGraphControlChild.Refresh();
        }

        private void textDetailsMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Shape a in this.activeGraphControlChild.Shapes)
            {
                a.ShowLabel = true;
                a.Font = new Font(a.Font.FontFamily, 7.8f);
            }
            foreach (Connection a in this.activeGraphControlChild.Connections)
            {
                a.ShowLabel = true;
                a.Font = new Font(a.Font.FontFamily, 7.8f);
            }
            this.activeGraphControlChild.Refresh();
        }
        #endregion
        #region Layout
        private void cascadeItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }
        private void tileHorizItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }
        private void tileVertItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
        public void layoutSpringEmbedderItem_Click(object sender, EventArgs e)
        {
            foreach (Shape a in this.activeGraphControlChild.Shapes)
            {
                a.IsFixed = false;
                if (float.IsNaN(a.X)) a.X = 0.0f;
                if (float.IsNaN(a.Y)) a.Y = 0.0f;
            }
            this.activeGraphControlChild.GraphLayoutAlgorithm
                = Netron.GraphLib.GraphLayoutAlgorithms.SpringEmbedder;
            this.activeGraphControlChild.StartLayout();
        }
        public void layoutRandomizerItem_Click(object sender, EventArgs e)
        {
            foreach (Shape a in this.activeGraphControlChild.Shapes)
            {
                if (float.IsNaN(a.X)) a.X = 0.0f;
                if (float.IsNaN(a.Y)) a.Y = 0.0f;
            }
            this.activeGraphControlChild.GraphLayoutAlgorithm
                = Netron.GraphLib.GraphLayoutAlgorithms.Randomizer;
            this.activeGraphControlChild.StartLayout();
        }
        public void layoutTreeItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.activeGraphControlChild.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.Tree;
                this.activeGraphControlChild.StartLayout();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void customLayout0MenuItem_Click(object sender, EventArgs e)
        {
            Type gLayoutType = customLayouts.GetType();
            MethodInfo[] customMethods = gLayoutType.GetMethods();
            if (customMethods.GetLength(0) > 4)
            {
                object[] args = new object[2];
                args[0] = this.activeGraphControlChild;
                args[1] = this.activeGraph;
                customMethods[0].Invoke(customLayouts, args);
                this.activeGraphControlChild.Refresh();
            }
        }

        public void customLayout1MenuItem_Click(object sender, EventArgs e)
        {
            Type gLayoutType = customLayouts.GetType();
            MethodInfo[] customMethods = gLayoutType.GetMethods();
            if (customMethods.GetLength(0) > 5)
            {
                object[] args = new object[2];
                args[0] = this.activeGraphControlChild;
                args[1] = this.activeGraph;
                customMethods[1].Invoke(customLayouts, args);
                this.activeGraphControlChild.Refresh();
            }
        }

        public void customLayout2MenuItem_Click(object sender, EventArgs e)
        {
            Type gLayoutType = customLayouts.GetType();
            MethodInfo[] customMethods = gLayoutType.GetMethods();
            if (customMethods.GetLength(0) > 6)
            {
                object[] args = new object[2];
                args[0] = this.activeGraphControlChild;
                args[1] = this.activeGraph;
                customMethods[2].Invoke(customLayouts, args);
                this.activeGraphControlChild.Refresh();
            }
        }
        #endregion
        #endregion

        #region Design
        #region Set as Active

        private void designToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            /* first set all to true, in case user increased number in settings */
            this.setRuleSet9toolStripMenuItem.Visible = true;
            this.setRuleSet8toolStripMenuItem.Visible = true;
            this.setRuleSet7toolStripMenuItem.Visible = true;
            this.setRuleSet6toolStripMenuItem.Visible = true;
            this.setRuleSet5toolStripMenuItem.Visible = true;
            this.setRuleSet4toolStripMenuItem.Visible = true;
            this.setRuleSet3toolStripMenuItem.Visible = true;
            this.setRuleSet2toolStripMenuItem.Visible = true;
            this.setRuleSet1toolStripMenuItem.Visible = true;

            /* if settings specified fewer rulesets we disable them here. */
            if (Program.settings.numOfRuleSets < 10)
                this.setRuleSet9toolStripMenuItem.Visible = false;
            if (Program.settings.numOfRuleSets < 9)
                this.setRuleSet8toolStripMenuItem.Visible = false;
            if (Program.settings.numOfRuleSets < 8)
                this.setRuleSet7toolStripMenuItem.Visible = false;
            if (Program.settings.numOfRuleSets < 7)
                this.setRuleSet6toolStripMenuItem.Visible = false;
            if (Program.settings.numOfRuleSets < 6)
                this.setRuleSet5toolStripMenuItem.Visible = false;
            if (Program.settings.numOfRuleSets < 5)
                this.setRuleSet4toolStripMenuItem.Visible = false;
            if (Program.settings.numOfRuleSets < 4)
                this.setRuleSet3toolStripMenuItem.Visible = false;
            if (Program.settings.numOfRuleSets < 3)
                this.setRuleSet2toolStripMenuItem.Visible = false;
            if (Program.settings.numOfRuleSets < 2)
                this.setRuleSet1toolStripMenuItem.Visible = false;
        }
        private void setSeedMenuItem_Click(object sender, EventArgs e)
        {
            if ((this.ActiveMdiChild == null) || (this.ActiveMdiChild.GetType() != typeof(graphDisplay)))
                MessageBox.Show("The active window is not a graph.",
                    "Seed must be a graph.",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if ((Program.seed == null) ||
                  (DialogResult.Yes == MessageBox.Show("The graph " +
                Program.seed.name + " is already loaded as the seed."
               + " Replace it with the active graph?", "Seed already defined.",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information)))
            {
                graphDisplay activeGD = (graphDisplay)this.ActiveMdiChild;
                Program.seed = activeGD.graph1;
            }
        }
        private void defineRuleSet(int index)
        {
            if ((this.ActiveMdiChild == null) || (this.ActiveMdiChild.GetType() != typeof(ruleSetDisplay)))
                MessageBox.Show("The active window is not a Rule Set display.",
                    "Active Window is not RuleSet.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (Program.rulesets.GetLength(0) <= index)
                MessageBox.Show("There are only "+Program.settings.numOfRuleSets+" rulesets currently allocated."+
                    " Please change number of rule sets in the global settings.",
                    "RuleSet #"+index.ToString()+" not allocated.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if ((Program.rulesets[index] == null) ||
                (DialogResult.Yes == MessageBox.Show("The ruleset " +
                Program.rulesets[index].name + " is already loaded into rule set #"
                + index + ". Replace with active ruleset?", "RuleSet already defined.",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information)))
            {
                ruleSetDisplay activeRSC = (ruleSetDisplay)this.ActiveMdiChild;
                Program.rulesets[index] = activeRSC.ruleset;
                activeRSC.ruleset.ruleSetIndex = index;
            }
        }
        private void setRuleSet0toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.settings.numOfRuleSets < 1)
                MessageBox.Show("Please define a number of rule sets in File-->Settings that is one " +
                    "or greater.", "No Rule Sets Allotted.", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else defineRuleSet(0);
        }
        private void setRuleSet1toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(1); }
        private void setRuleSet2toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(2); }
        private void setRuleSet3toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(3); }
        private void setRuleSet4toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(4); }
        private void setRuleSet5toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(5); }
        private void setRuleSet6toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(6); }
        private void setRuleSet7toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(7); }
        private void setRuleSet8toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(8); }
        private void setRuleSet9toolStripMenuItem_Click(object sender, EventArgs e)
        { defineRuleSet(9); }

        private void clearRuleSetsAndSeedStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.seed = null;
            for (int i = 0; i != Program.settings.numOfRuleSets; i++)
                Program.rulesets[i] = null;
        }


        #endregion
        #region Run and Test
        private Boolean checkForSeedAndRuleSets()
        {
            if (Program.seed == null)
            {
                MessageBox.Show("Please define a seed graph first.", "Missing Seed Graph",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                Boolean foundOne = false;
                for (int i = Program.rulesets.GetLength(0) - 1; i >= 0; i--)
                {
                    if ((Program.rulesets[i] == null) && (foundOne || (i == 0)))
                    {
                        MessageBox.Show("There is a missing rule set at position #" + i.ToString(),
                            "Missing Rule Set", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else if ((Program.rulesets[i] == null) &&
                        (DialogResult.No == MessageBox.Show("No rule set at position #"
                        + i.ToString() + ". Is this intentional?",
                        "Missing Rule Set", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)))
                        return false;
                    else if (Program.rulesets[i] != null) foundOne = true;
                }
                return true;
            }
        }
        private void recognizeUserChooseApplyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkForSeedAndRuleSets())
            {
                /* just as in the GA we were using, one can define a custom RecognizeChooseApply method/class
                 * so long as the name is GenerationApproach. So, the definition of the class 
                 * chooseViaHumanGui may not be used in the future with completely automated design. */
                Boolean display = (DialogResult.Yes == MessageBox.Show("Would you like to see the graph " +
                    "after each rule application?", "Display Interim Graphs",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information));
                chooseViaHumanGui GenerationApproach = new chooseViaHumanGui(display);

                addAndShowSearchController(new searchProcessController(GenerationApproach), true);
                searchControls[searchControls.Count - 1].SendToBack();
            }
        }
        private void recognizeRandomChooseApToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkForSeedAndRuleSets())
            {
                Boolean display = (DialogResult.Yes == MessageBox.Show("Would you like to see the graph " +
                    "after each rule application?", "Display Interim Graphs",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information));
                Generation.randomChoose GenerationApproach = new Generation.randomChoose(Program.seed,
                    Program.rulesets, Program.settings.maxRulesToApply, display, Program.settings.recompileRules,
                    Program.settings.execDir, Program.settings.compiledparamRules);
                addAndShowSearchController(new searchProcessController(GenerationApproach), true);
                searchControls[searchControls.Count - 1].SendToBack();
            }
        }
        private void numOfRandSteps_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) numOfSteps_Changed();
        }
        private void numOfSteps_Changed()
        {
            if (checkForSeedAndRuleSets())
            {
                int num, i = 0;
                int[] numOfCalls = new int[Program.settings.numOfRuleSets];
                List<string> numbers = StringCollectionConverter.convert(this.numOfRandSteps.Text.Trim());
                Boolean successfulEntry = true;
                this.numOfRandSteps.Text = "#steps";
                foreach (string a in numbers)
                {
                    if (i == Program.settings.numOfRuleSets)
                    {
                        if (DialogResult.Yes == MessageBox.Show("There are no rulesets to assign to "
                            + a + ". Would you like to continue with current limits?",
                            "More arguments than rulesets.", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                        {
                            successfulEntry = true;
                            break;
                        }
                        else
                        {
                            successfulEntry = false;
                            break;
                        }
                    }
                    if ((int.TryParse(a, out num)) && (num > 0) && (num <= Program.settings.maxRulesToApply))
                        numOfCalls[i++] = num;
                    else if (DialogResult.Yes == MessageBox.Show("Unable to change \""
                        + a + "\" for Rule-Set #" + i
                        + " into number greater than zero and less than or equal to "
                        + Program.settings.maxRulesToApply
                        + ". Do you approve of setting to the value of " + Program.settings.maxRulesToApply
                        + "? (If no, this call will be cancelled.)",
                        "Value not defined.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                        numOfCalls[i++] = Program.settings.maxRulesToApply;
                    else
                    {
                        successfulEntry = false;
                        break;
                    }
                }
                if (successfulEntry)
                {
                    Boolean display = (DialogResult.Yes == MessageBox.Show("Would you like to see the graph " +
                        "after each rule application?", "Display Interim Graphs",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information));
                    Generation.randomChoose GenerationApproach = new Generation.randomChoose(Program.seed,
                        Program.rulesets, numOfCalls, display, Program.settings.recompileRules, Program.settings.execDir,
                        Program.settings.compiledparamRules);
                    addAndShowSearchController(new searchProcessController(GenerationApproach), true);
                    searchControls[searchControls.Count - 1].SendToBack();
                }
            }
        }
        private void runSearchProcessToolStripMenuItem_MouseUp(object sender, EventArgs e)
        {
            addAndShowSearchController(new searchProcessController(searchControls.Count),
                Program.settings.searchControllerPlayOnStart);
        }
        private void runSearchProcessToolStripMenuItem_Click(object sender, EventArgs e)
        { runSearchProcessToolStripMenuItem_MouseUp(sender, e); }
        #endregion

        #endregion

        #region Help
        private void aboutGraphSynthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutGraphSynth about = new AboutGraphSynth();
            about.Show();
        }

        private void showHelpMenuItem_Click(object sender, EventArgs e)
        {
            helpWindow help = new helpWindow();
            help.showHelpWindow();
            help.Show();
        }
        #endregion

        #region misc. Methods
        public void showProperties(Netron.GraphLib.UI.GraphControl GC, grammarRule rule, object sender,
            object[] props)
        {
            if (props.GetLength(0) > 0)
            {
                if ((propWindow == null) || !propWindow.CanSelect)
                {
                    propWindow = new PropertiesWindow();
                }
                if (this.ActiveMdiChild.WindowState.Equals(FormWindowState.Maximized))
                    propWindow.MdiParent = null;
                propWindow.Show();
                propWindow.Activate();
                propWindow.showProperties(GC, rule, props);
            }
        }
        public void showProperties(Netron.GraphLib.UI.GraphControl GC, designGraph graph, object sender, object[] props)
        {
            if (props.GetLength(0) > 0)
            {
                if ((propWindow == null) || !propWindow.CanSelect)
                {
                    propWindow = new PropertiesWindow();
                }
                if (this.ActiveMdiChild.WindowState.Equals(FormWindowState.Maximized))
                    propWindow.MdiParent = null;
                propWindow.Show();
                propWindow.Activate();
                propWindow.showProperties(GC, graph, props);
            }
        }

        public void addAndShowGraphDisplay(designGraph graph, string title)
        {
            graphDisplays.Add(new graphDisplay(graph, title));
            graphDisplays[graphChildCount].Show();
        }
        public void addAndShowRuleSetDisplay(ruleSet rs, string title)
        {
            ruleSetDisplays.Add(new ruleSetDisplay(rs, title));
            ruleSetDisplays[ruleSetChildCount].Show();
        }
        public void addAndShowSearchController(searchProcessController controller, Boolean playOnStart)
        {
            searchControls.Add(controller);
            searchControls[searchControls.Count - 1].Show();
            if (playOnStart)
                searchControls[searchControls.Count - 1].playButton_Click(null, null);

        }
        #endregion

    }
}
