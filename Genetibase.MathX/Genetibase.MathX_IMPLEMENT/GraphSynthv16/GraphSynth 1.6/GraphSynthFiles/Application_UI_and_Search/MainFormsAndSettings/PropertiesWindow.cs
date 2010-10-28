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
    public partial class PropertiesWindow : Form
    {
        #region Fields
        Netron.GraphLib.UI.GraphControl graphControl;
        grammarRule rule;
        designGraph graph;
        #endregion

        #region Constructor
        public PropertiesWindow()
        {
            InitializeComponent();
            this.MdiParent = Program.main;
        }

        #endregion

        public void showProperties(Netron.GraphLib.UI.GraphControl GC, grammarRule rule1, object[] props)
        {
            graphControl = GC;
            rule = rule1;

            if (graphControl.Name == "graphControlLHS") graph = rule.L;
            else graph = rule.R;

            rule.updateFromGraphControl(graphControl);
            try
            {
                this.graphRulePropsTab.Text = "Rule Properties";
                if (rule.Bag == null) rule.initPropertiesBag();
                this.graphRuleProps.SelectedObject = rule.Bag;
            }
            catch (Exception exc)
            {
                MessageBox.Show("The properties of the rule has thrown an exception and cannot be displayed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SearchIO.output(exc.Message, 2);
            }
            showDisplayProperties(graphControl, (Netron.GraphLib.PropertyBag)props[0]);
            showRuleNodeArcProperties((Netron.GraphLib.PropertyBag)props[0], graph);
        }
        public void showProperties(Netron.GraphLib.UI.GraphControl GC, designGraph graph1, object[] props)
        {
            graphControl = GC;
            graph = graph1;
            rule = null;
            try
            {
                graph.updateFromGraphControl(graphControl);
                if (graph.Bag == null) graph.initPropertiesBag();
                this.graphRuleProps.SelectedObject = graph.Bag;
                this.graphRulePropsTab.Text = "Graph Properties";
            }
            catch (Exception exc)
            {
                MessageBox.Show("The properties of the graph has thrown an exception and cannot be displayed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SearchIO.output(exc.Message, 2);
            }
            showDisplayProperties(graphControl, (Netron.GraphLib.PropertyBag)props[0]);
            showNodeArcProperties((Netron.GraphLib.PropertyBag)props[0], graph);
        }

        private void showNodeArcProperties(PropertyBag shapeProps, designGraph graph)
        {
            this.nodeArcProps.Enabled = true;
            this.propertiesTabControl.SelectedIndex = 0;
            try
            {
                if ((shapeProps.Owner.GetType()) == (typeof(Connection)))
                {
                    arc temparc = graph.arcs.Find(delegate(arc b)
                        { return (b.displayShape == shapeProps.Owner); });
                    if (temparc.Bag == null) temparc.initPropertiesBag();
                    this.nodeArcProps.SelectedObject = temparc.Bag;
                }
                else
                {
                    node tempnode = graph.nodes.Find(delegate(node b)
                        { return (b.displayShape == shapeProps.Owner); });
                    if (tempnode.Bag == null) tempnode.initPropertiesBag();
                    this.nodeArcProps.SelectedObject = tempnode.Bag;
                }
            }
            catch
            {
                this.nodeArcProps.Enabled = false;
                this.propertiesTabControl.SelectedIndex = 1;
            }
        }
        private void showRuleNodeArcProperties(PropertyBag shapeProps, designGraph graph)
        {
            this.nodeArcProps.Enabled = true;
            this.propertiesTabControl.SelectedIndex = 0;
            try
            {
                if ((shapeProps.Owner.GetType()) == (typeof(Connection)))
                {
                    ruleArc temparc = (ruleArc)graph.arcs.Find(delegate(arc b)
                        { return (b.displayShape == shapeProps.Owner); });
                    if (temparc.Bag == null) temparc.initPropertiesBag();
                    this.nodeArcProps.SelectedObject = temparc.Bag;
                }
                else
                {
                    ruleNode tempnode = (ruleNode)graph.nodes.Find(delegate(node b)
                        { return (b.displayShape == shapeProps.Owner); });
                    if (tempnode.Bag == null) tempnode.initPropertiesBag();
                    this.nodeArcProps.SelectedObject = tempnode.Bag;
                }
            }
            catch
            {
                this.nodeArcProps.Enabled = false;
                this.propertiesTabControl.SelectedIndex = 1;
            }
        }
        private void showDisplayProperties(Netron.GraphLib.UI.GraphControl GC, PropertyBag shapeProps)
        {
            try
            {
                this.displayProps.SelectedObject = shapeProps;
            }
            catch (Exception exc)
            {
                MessageBox.Show("The display properties cannot be displayed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SearchIO.output(exc.Message, 2);
            }
        }

        private void Props_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (rule != null)
            {
                grammarRuleDisplay gDisplay = (grammarRuleDisplay)graphControl.FindForm();
                rule.updateGraphControl(gDisplay.graphControlLHS, gDisplay.graphControlRHS,
                    gDisplay.globalLabelsLText, gDisplay.globalLabelsRText);
                }
            else
            {
                graphDisplay gDisplay = (graphDisplay)graphControl.FindForm();
                graph.updateGraphControl(graphControl, gDisplay.globalLabelsText);
            }
        }

        private void displayProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (rule != null)
                rule.updateFromGraphControl(graphControl);
            else
                graph.updateFromGraphControl(graphControl);
            /* after you update the nodes, arcs - you may need to re-update the display based
             * on changes there (e.g. new K elements) */
            Props_PropertyValueChanged(s, e);
        }
    }
}