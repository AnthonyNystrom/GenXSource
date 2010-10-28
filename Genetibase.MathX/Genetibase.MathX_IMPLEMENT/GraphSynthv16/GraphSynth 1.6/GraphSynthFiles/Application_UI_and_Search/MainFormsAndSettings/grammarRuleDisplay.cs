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
    public partial class grammarRuleDisplay : Form
    {
        public grammarRule rule;

        #region Constructor
        public grammarRuleDisplay(grammarRule initRule, string formTitle)
        {
            initGrammarRuleDisplay(initRule, formTitle);
        }
        public grammarRuleDisplay()
        {
            initGrammarRuleDisplay(null, "");
        }

        public grammarRuleDisplay(grammarRule initRule)
        {
            initGrammarRuleDisplay(initRule, initRule.name);
        }

        public grammarRuleDisplay(string formTitle)
        {
            initGrammarRuleDisplay(null, formTitle);
        }
        private void initGrammarRuleDisplay(grammarRule initRule, string formTitle)
        {
            InitializeComponent();
            this.MdiParent = Program.main;
            graphControlLHS.LoadLibraries();
            graphControlRHS.LoadLibraries();

            if (initRule == null)
            {
                rule = new grammarRule();
                rule.L = new designGraph();
                rule.R = new designGraph();
            }
            else rule = initRule;
            rule.L.displayGraph(graphControlLHS, this.globalLabelsLText);
            rule.R.displayGraph(graphControlRHS, this.globalLabelsRText);
            if ((formTitle == null) || (formTitle == "")) this.Text = rule.name;
            else this.Text = formTitle;
            this.rule.updateGraphControl(graphControlLHS, graphControlRHS, globalLabelsLText, globalLabelsRText);
        }
        #endregion

        #region Connecting Events Functions
        private void graphControlLHS_OnShowProperties(object sender, object[] props)
        {
            Program.main.showProperties(this.graphControlLHS, rule, sender, props);
        }
        private void graphControlRHS_OnShowProperties(object sender, object[] props)
        {
            Program.main.showProperties(this.graphControlRHS, rule, sender, props);
        }
        private void globalLabelsLText_DoubleClick(object sender, EventArgs e)
        {
            Program.main.showProperties(this.graphControlLHS, rule, sender, new object[1]);
        }
        private void globalLabelsRText_DoubleClick(object sender, EventArgs e)
        {
            Program.main.showProperties(this.graphControlRHS, rule, sender, new object[1]);
        }
        #endregion
    }
}
