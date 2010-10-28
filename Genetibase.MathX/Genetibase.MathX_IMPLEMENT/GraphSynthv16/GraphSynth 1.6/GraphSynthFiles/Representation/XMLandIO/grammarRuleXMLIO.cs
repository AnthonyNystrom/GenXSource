using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.UI;

namespace GraphSynth.Representation
{
    public partial class grammarRule
    {
        #region Fields
        private int numElementsInK;
        private string rulesDir;
        #endregion

        #region Open and Save Grammar Rules
        public static void saveRuleToXml(string filename, grammarRule rule, GraphControl graphControlLHS,
            GraphControl graphControlRHS, Label LglobalLabelsText, Label RglobalLabelsText)
        {
            rule.updateFromGraphControl(graphControlLHS);
            rule.updateFromGraphControl(graphControlRHS);
            rule.updateGraphControl(graphControlLHS, graphControlRHS, LglobalLabelsText, RglobalLabelsText);

            if (rule.L.checkForRepeatNames())
            {
                MessageBox.Show("Sorry, but you are not allowed to have repeat names in L. I have changed these " +
                    "names to be unique, which may have disrupted your context graph, K", "Repeat Names in L",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                rule.updateFromGraphControl(graphControlLHS);
                rule.updateFromGraphControl(graphControlRHS);
                rule.updateGraphControl(graphControlLHS, graphControlRHS, LglobalLabelsText, RglobalLabelsText);

            }
            if (rule.R.checkForRepeatNames())
            {
                MessageBox.Show("Sorry, but you are not allowed to have repeat names in R. I have changed" +
                    " these names to be unique, which may have disrupted your context graph, K", "Repeat Names in R",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                rule.updateFromGraphControl(graphControlLHS);
                rule.updateFromGraphControl(graphControlRHS);
                rule.updateGraphControl(graphControlLHS, graphControlRHS, LglobalLabelsText, RglobalLabelsText);

            }
            if ((rule.numElementsInK == 0) && (DialogResult.No == MessageBox.Show(
                    "There appears to be no common elements between the left and right hand sides of the rule." +
                    " Is this intentional? If so, click yes to continue.", "No Context Graph",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))) return;
            if ((rule.KarcsChangeDirection() != "") && (DialogResult.No ==
                MessageBox.Show("It appears that arc(s): " + rule.KarcsChangeDirection() +
                    " change direction (to = from or vice-versa). Even though the arc(s) might be undirected," +
                    " this can still lead to problems in the rule application, it is recommended that this is" +
                    " fixed before saving. Save anyway?", "Misdirected Arcs in K", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question))) return;

            saveRuleToXml(filename, rule);
        }

        private string KarcsChangeDirection()
        {
            string badArcNames = "";
            foreach (ruleArc a in L.arcs)
            {
                ruleArc b = (ruleArc)R.arcs.Find(delegate(arc c) { return (c.name == a.name); });
                if ((b != null) && ((a.To.name == b.From.name) || (a.From.name == b.To.name)))
                    badArcNames += a.name + ", ";
            }
            return badArcNames;
        }
        public static void saveRuleToXml(string filename, grammarRule ruleToSave)
        {
            StreamWriter ruleWriter = null;
            try
            {
                ruleWriter = new StreamWriter(filename);
                XmlSerializer ruleSerializer = new XmlSerializer(typeof(grammarRule));
                ruleSerializer.Serialize(ruleWriter, ruleToSave);
            }
            catch (Exception ioe)
            {
                MessageBox.Show(ioe.ToString(), "XML Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            finally
            {
                if (ruleWriter != null) ruleWriter.Close();
            }
        }


        public static grammarRule openRuleFromXml(string filename)
        {
            try
            {
                StreamReader ruleReader = new StreamReader(filename);
                try
                {
                    XmlSerializer ruleDeserializer = new XmlSerializer(typeof(grammarRule));
                    grammarRule newGrammarRule = (grammarRule)ruleDeserializer.Deserialize(ruleReader);
                    if (newGrammarRule.L == null) newGrammarRule.L = new designGraph();
                    if (newGrammarRule.R == null) newGrammarRule.R = new designGraph();
                    newGrammarRule.L.internallyConnectGraph();
                    newGrammarRule.R.internallyConnectGraph();

                    SearchIO.output("Successfully loaded rule: " + Path.GetFileName(filename), 4);

                    if (newGrammarRule.name == null)
                        newGrammarRule.name =
                            Path.GetFileName(filename).TrimEnd(new char[] { '.', 'x', 'm', 'l' });
                    newGrammarRule.rulesDir = Path.GetDirectoryName(filename) + "\\";
                    return newGrammarRule;
                }
                catch (Exception ioe)
                {
                    MessageBox.Show(ioe.Message, "XML Serialization Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                finally
                {
                    ruleReader.Close();
                }
            }
            catch (FileNotFoundException fnfe)
            {
                MessageBox.Show(fnfe.Message, "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        #endregion

        #region update display
        public void updateFromGraphControl(Netron.GraphLib.UI.GraphControl graphControl1)
        {
            try
            {
                node tempnode;
                arc temparc;
                Shape fromShape, toShape;
                designGraph graph;

                if (graphControl1.Name == "graphControlLHS") graph = L;
                else graph = R;

                foreach (Shape a in graphControl1.Shapes)
                {
                    if (!graph.nodes.Exists(delegate(node b) { return (b.displayShape == a); }))
                    {
                        graph.nodes.Add(new ruleNode(graph.nameFromText(a.Text)));
                        tempnode = graph.nodes[graph.lastNode];
                    }
                    else tempnode = graph.nodes.Find(delegate(node b) { return (b.displayShape == a); });
                    if (a.Text != "[Not_set]")
                    {
                        tempnode.name = graph.nameFromText(a.Text);
                        tempnode.localLabels = graph.labelsFromText(a.Text);
                    }
                    tempnode.screenX = a.X;
                    tempnode.screenY = a.Y;
                    tempnode.shapekey = node.lookupShapeKey(a);
                    tempnode.displayShape = a;
                }
                foreach (Connection a in graphControl1.Connections)
                {
                    if (!graph.arcs.Exists(delegate(arc b) { return (b.displayShape == a); }))
                    {
                        graph.arcs.Add(new ruleArc(graph.nameFromText(a.Text)));
                        temparc = graph.arcs[graph.lastArc];
                    }
                    else temparc = graph.arcs.Find(delegate(arc b) { return (b.displayShape == a); });
                    fromShape = a.From.BelongsTo;
                    toShape = a.To.BelongsTo;

                    temparc.From = graph.nodes.Find(delegate(node c)
                     { return (graph.sameName(c.name, fromShape.Text)); });
                    temparc.To = graph.nodes.Find(delegate(node c)
                     { return (graph.sameName(c.name, toShape.Text)); });

                    for (int i = 0; i != fromShape.Connectors.Count; i++)
                    {
                        if (fromShape.Connectors[i] == a.From)
                            temparc.fromConnector = i;
                    }
                    for (int i = 0; i != toShape.Connectors.Count; i++)
                    {
                        if (toShape.Connectors[i] == a.To)
                            temparc.toConnector = i;
                    }
                    if (a.Text != "[Not_set]")
                    {
                        temparc.name = graph.nameFromText(a.Text);
                        temparc.localLabels = graph.labelsFromText(a.Text);
                    }
                    temparc.curveStyle = a.LinePath;
                    if (a.LineEnd != ConnectionEnd.NoEnds)
                        temparc.directed = true;
                    else temparc.directed = false;
                    if (a.LineEnd == ConnectionEnd.BothFilledArrow)
                        temparc.doublyDirected = true;
                    else temparc.doublyDirected = false;
                    temparc.displayShape = a;
                }
                graph.internallyConnectGraph();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error updating the rule from the display. Please save work and re-open. (Error: " + e.ToString() + ")", "Error Updating Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void updateGraphControl(Netron.GraphLib.UI.GraphControl graphControlLHS,
            Netron.GraphLib.UI.GraphControl graphControlRHS, Label LglobalLabelsText, Label RglobalLabelsText)
        {
            try
            {
                numElementsInK = 0;

                foreach (node a in L.nodes)
                {
                    if (R.nodes.Exists(delegate(node b) { return (b.name == a.name); }))
                    {
                        numElementsInK++;
                        a.displayShape.ShapeColor = System.Drawing.Color.FromArgb(214, 145, 82);
                    }
                    else
                    {
                        a.displayShape.ShapeColor = System.Drawing.Color.White;
                    }
                }
                foreach (arc a in L.arcs)
                {
                    if (R.arcs.Exists(delegate(arc b) { return (b.name == a.name); }))
                    {
                        numElementsInK++;
                        a.displayShape.LineColor = System.Drawing.Color.FromArgb(214, 145, 82);
                        a.displayShape.LineWeight = ConnectionWeight.Fat;
                    }
                    else
                    {
                        a.displayShape.LineColor = System.Drawing.Color.Black;
                        a.displayShape.LineWeight = ConnectionWeight.Medium;
                    }
                }

                foreach (node a in R.nodes)
                {
                    if (L.nodes.Exists(delegate(node b) { return (b.name == a.name); }))
                    {
                        numElementsInK++;
                        a.displayShape.ShapeColor = System.Drawing.Color.FromArgb(214, 145, 82);
                    }
                    else
                    {
                        a.displayShape.ShapeColor = System.Drawing.Color.White;
                    }
                }
                foreach (arc a in R.arcs)
                {
                    if (L.arcs.Exists(delegate(arc b) { return (b.name == a.name); }))
                    {
                        numElementsInK++;
                        a.displayShape.LineColor = System.Drawing.Color.FromArgb(214, 145, 82);
                        a.displayShape.LineWeight = ConnectionWeight.Fat;
                    }
                    else
                    {
                        a.displayShape.LineColor = System.Drawing.Color.Black;
                        a.displayShape.LineWeight = ConnectionWeight.Medium;
                    }
                }
                L.updateGraphControl(graphControlLHS, LglobalLabelsText);
                R.updateGraphControl(graphControlRHS, RglobalLabelsText);
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error displaying the graphs. Please save work and re-open." +
                    " (Error: " + e.ToString() + ")", "Error Displaying Graphs", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region rulePropsBag
        /*** the property bag ***/
        private PropertyBag rulePropsBag;

        [XmlIgnore]
        public PropertyBag Bag
        {
            get { return rulePropsBag; }
        }
        /* Adds the basic properties of the shape */
        public void AddPropertiesToBag()
        {
            Bag.Properties.Add(new PropertySpec("Name", typeof(string), "General Rule Attributes",
                "A name/filename for the rule.", this.name));
            Bag.Properties.Add(new PropertySpec("induced", typeof(Boolean), "LHS Graph Attributes",
                "Does the rule contains ALL the arcs between the recognized nodes?", this.induced));
            Bag.Properties.Add(new PropertySpec("spanning", typeof(Boolean), "LHS Graph Attributes",
                "Does the rule contain ALL the nodes of the host graph?", this.spanning));
            Bag.Properties.Add(new PropertySpec("containsAllGlobalLabels", typeof(Boolean),
                "LHS Graph Attributes", "Does the rule contain ALL the global labels of the host?",
                this.containsAllGlobalLabels));
            Bag.Properties.Add(new PropertySpec("Embedding Rules", typeof(int),
                "LHS Graph Attributes", "A list of free arc embedding rules. Currently these can only be" +
                "drafted in the resulting xml file. A number here will set up blanks for latter editing.",
                this.embeddingRules));
            /*Bag.Properties.Add(new PropertySpec("LHS Name", typeof(string), "LHS Graph Attributes",
                "A decriptive name for the LHS.", this.L.name));*/
            Bag.Properties.Add(new PropertySpec("LHS Labels", typeof(List<string>), "LHS Graph Attributes",
                "Global labels assigned to the LHS of the rule.", this.L.globalLabels,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("LHS Variables", typeof(List<double>), "LHS Graph Attributes",
                "Global variables assigned to the LHS of the rule.", this.L.globalVariables,
                typeof(System.Drawing.Design.UITypeEditor), typeof(DoubleCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Parametric Recognize Functions", typeof(List<string>),
                "LHS Graph Attributes", "Names of the functions to be invoked for recognition.",
                this.recognizeFunctions, typeof(System.Drawing.Design.UITypeEditor),
                typeof(StringCollectionConverter)));
            /*Bag.Properties.Add(new PropertySpec("RHS Name", typeof(string), "RHS Graph Attributes",
                "A decriptive name for the RHS.", this.R.name));*/
            Bag.Properties.Add(new PropertySpec("RHS Labels", typeof(List<string>), "RHS Graph Attributes",
                "Global labels assigned to the RHS of the rule.", this.R.globalLabels,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("RHS Variables", typeof(List<double>), "RHS Graph Attributes",
                "Global variables assigned to the RHS of the rule.", this.R.globalVariables,
                typeof(System.Drawing.Design.UITypeEditor), typeof(DoubleCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Parametric Apply Functions", typeof(List<string>),
                "RHS Graph Attributes", "Names of the functions to be invoked for rule application.",
                this.applyFunctions, typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
        }

        /* Allows the propertygrid to set new values */
        protected void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "induced": e.Value = this.induced; break;
                case "spanning": e.Value = this.spanning; break;
                case "containsAllGlobalLabels": e.Value = this.containsAllGlobalLabels; break;
                case "Name": e.Value = this.name; break;
                case "Embedding Rules": e.Value = this.embeddingRules.Count; break;
                /*case "LHS Name": e.Value = this.L.name; break;*/
                case "LHS Labels": e.Value = this.L.globalLabels; break;
                case "LHS Variables": e.Value = this.L.globalVariables; break;
                /*case "RHS Name": e.Value = this.R.name; break;*/
                case "RHS Labels": e.Value = this.R.globalLabels; break;
                case "RHS Variables": e.Value = this.R.globalVariables; break;
                case "Parametric Apply Functions": e.Value = this.applyFunctions; break;
                case "Parametric Recognize Functions": e.Value = this.recognizeFunctions; break;
            }
        }

        /* Allows the propertygrid to set new values. */
        protected void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "induced": this.induced = (Boolean)e.Value; break;
                case "spanning": this.spanning = (Boolean)e.Value; break;
                case "containsAllGlobalLabels": this.containsAllGlobalLabels = (Boolean)e.Value; break;
                case "Name": this.name = (string)e.Value; break;
                case "Embedding Rules":
                    {
                        while (this.embeddingRules.Count < (int)e.Value)
                            this.embeddingRules.Add(new embeddingRule());
                        break;
                    }
                /*case "LHS Name": this.L.name = (string)e.Value; break;*/
                case "LHS Labels": this.L.globalLabels = (List<string>)e.Value; break;
                case "LHS Variables": this.L.globalVariables = (List<double>)e.Value; break;
                /*case "RHS Name": this.R.name = (string)e.Value; break;*/
                case "RHS Labels": this.R.globalLabels = (List<string>)e.Value; break;
                case "RHS Variables": this.R.globalVariables = (List<double>)e.Value; break;
                case "Parametric Apply Functions":
                    {
                        this.applyFunctions = checkForFunctions(false, (List<string>)e.Value);
                        break;
                    }
                case "Parametric Recognize Functions":
                    {
                        this.recognizeFunctions = checkForFunctions(true, (List<string>)e.Value);
                        break;
                    }
            }
        }


        public void initPropertiesBag()
        {
            rulePropsBag = new PropertyBag(this);
            this.AddPropertiesToBag();
            rulePropsBag.GetValue += new PropertySpecEventHandler(GetPropertyBagValue);
            rulePropsBag.SetValue += new PropertySpecEventHandler(SetPropertyBagValue);
        }
        private List<string> checkForFunctions(Boolean isThisRecognize, List<string> newFunctions)
        {
            string[] sourceFiles = Directory.GetFiles(rulesDir, "*.cs");
            Boolean found = false;
            List<string> filesWithFunc = new List<string>();
            List<string> functionNames = new List<string>();
            int i = 0;

            foreach (string funcName in newFunctions)
            {
                found = false;
                foreach (string file in sourceFiles)
                {
                    StreamReader r = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read), Encoding.UTF8);
                    if (r.ReadToEnd().Contains(funcName))
                    {
                        found = true;
                        filesWithFunc.Add(file);
                    }
                    r.Close();
                }
                if (found)
                {
                    string message = "Function, " + funcName + ", found in ";
                    foreach (string a in filesWithFunc)
                    {
                        message += Path.GetFileName(a);
                        message += ", ";
                    }
                    MessageBox.Show(message, "Function Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    functionNames.Add(funcName);
                }
                else
                {
                    DialogResult result = DialogResult.Cancel;
                    if (sourceFiles.GetLength(0) == 1)
                    {
                        result =
                            MessageBox.Show("Function, " + funcName + " not found. Would you like to add it to "
                            + Path.GetFileName(sourceFiles[0]) + "?", "Function Not Found", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);
                    }
                    else if (sourceFiles.GetLength(0) > 1)
                    {

                        for (i = 0; i != sourceFiles.GetLength(0); i++)
                        {
                            result =
                               MessageBox.Show("Function, " + funcName + " not found. Would you like to add it to "
                               + Path.GetFileName(sourceFiles[i]) +
                               "? (You can add to any source file - I will ask you about more after this...)",
                               "Function Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (result == DialogResult.Yes) break;
                        }
                    }
                    if (result != DialogResult.Yes)
                    {
                        MessageBox.Show("You must first create a C# source file to add the function, "
                            + funcName + " to. This can be done by adding a name of a file to the ruleSet display.",
                            "No Parameter Rule File Exists.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (isThisRecognize)
                    {
                        createRecognizeFunctionTemplate(sourceFiles[i], funcName);
                        functionNames.Add(funcName);
                    }
                    else
                    {
                        createApplyFunctionTemplate(sourceFiles[i], funcName);
                        functionNames.Add(funcName);
                    }
                }
            }
            return functionNames;
        }

        private void createRecognizeFunctionTemplate(string path, string funcName)
        {
            StreamReader r = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.UTF8);
            string fileString = r.ReadToEnd();
            int position = fileString.IndexOf("#endregion", 0);
            StringBuilder sb = new StringBuilder("");
            r.Close();

            sb.Append("\n/* This is RECOGNIZE for the rule entitled: ");
            sb.Append(this.name);
            sb.Append(" */");
            sb.Append("\npublic double ");
            sb.Append(funcName);
            sb.Append("(designGraph L,designGraph host,List<node> locatedNodes, List<arc> locatedArcs)\n");
            sb.Append("{\n/* here is where the code for the RECOGNIZE function is to be located.\n");
            sb.Append("* please remember that returning a positive real (double) is equivalent to\n");
            sb.Append("* a constraint violation. Zero and negative numbers are feasible. */\n");
            sb.Append("return 0.0;\n}\n");

            fileString = fileString.Insert(position, sb.ToString());

            StreamWriter w = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write), Encoding.UTF8);
            w.Write(fileString);
            w.Flush();
            w.Close();
        }
        private void createApplyFunctionTemplate(string path, string funcName)
        {
            StreamReader r = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.UTF8);
            string fileString = r.ReadToEnd();
            int position = fileString.IndexOf("#endregion", 0);
            position = fileString.IndexOf("#endregion", position + 1);
            StringBuilder sb = new StringBuilder("");
            r.Close();

            sb.Append("\n/* This is APPLY for the rule entitled: ");
            sb.Append(this.name);
            sb.Append(" */");
            sb.Append("\npublic designGraph ");
            sb.Append(funcName);
            sb.Append("(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)\n");
            sb.Append("{\n/* here is where the code for the APPLY function is to be located.\n");
            sb.Append("* please modify host (or located nodes) with the input from parameters. */\n");
            sb.Append("return host;\n}\n");

            fileString = fileString.Insert(position, sb.ToString());

            StreamWriter w = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write), Encoding.UTF8);
            w.Write(fileString);
            w.Flush();
            w.Close();
        }

        #endregion
    }

    public partial class ruleNode : node
    {
        #region nodePropsBag
        /* Adds the basic properties of the shape */
        public override void AddPropertiesToBag()
        {
            Bag.Properties.Add(new PropertySpec("Labels", typeof(List<string>), "Node Attributes",
                "Labels assigned to this particular node.", this.localLabels,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Variables", typeof(List<double>), "Node Attributes",
                "Variables assigned to this particular node.", this.localVariables,
                typeof(System.Drawing.Design.UITypeEditor), typeof(DoubleCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Name", typeof(string), "Node Attributes",
                "A name for this rule node. Note: the node name is NOT used in recognition, but it is used" +
                " to distinguish what nodes are the same in L and R, and for saving to XML.", this.name));
            Bag.Properties.Add(new PropertySpec("nodeType", typeof(string), "Node Attributes",
                "Nodes will be created in the host of this type.", this.XmlNodeType));
            Bag.Properties.Add(new PropertySpec("contains all local labels", typeof(Boolean),
                "Node Attributes", "Does the node only match with nodes in the host that contain the same" +
                " local labels (no more and no less)? <this is meaningless for RHS nodes>", this.containsAllLocalLabels));
            Bag.Properties.Add(new PropertySpec("strict degree match", typeof(Boolean),
                "Node Attributes", "Does the node match only with nodes that have the same number of arcs" +
                " connected to it?  <this is meaningless for RHS nodes>", this.strictDegreeMatch));
        }

        /* Allows the propertygrid to set new values */
        protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Labels": e.Value = this.localLabels; break;
                case "Variables": e.Value = this.localVariables; break;
                case "Name": e.Value = this.name; break;
                case "nodeType": e.Value = this.XmlNodeType; break;
                case "contains all local labels": e.Value = this.containsAllLocalLabels; break;
                case "strict degree match": e.Value = this.strictDegreeMatch; break;

            }
        }

        /* Allows the propertygrid to set new values. */
        protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Labels": this.localLabels = (List<string>)e.Value; break;
                case "Variables": this.localVariables = (List<double>)e.Value; break;
                case "Name": this.name = (string)e.Value; break;
                case "nodeType": this.XmlNodeType = (string)e.Value; break;
                case "contains all local labels": this.containsAllLocalLabels = (Boolean)e.Value; break;
                case "strict degree match": this.strictDegreeMatch = (Boolean)e.Value; break;
            }
        }

        #endregion
    }

    public partial class ruleArc : arc
    {
        #region arcPropsBag
        /* Adds the basic properties of the shape */
        public override void AddPropertiesToBag()
        {
            Bag.Properties.Add(new PropertySpec("Labels", typeof(List<string>), "Arc Attributes",
                "Labels assigned to this particular arc.", this.localLabels,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Variables", typeof(List<double>), "Arc Attributes",
                "Variables assigned to this particular arc.", this.localVariables,
                typeof(System.Drawing.Design.UITypeEditor), typeof(DoubleCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Name", typeof(string), "Arc Attributes",
                "A name for this rule arc. Note: the arc name is NOT used in recognition, but it is used" +
                " to distinguish what arcs are the same in L and R, and for saving to XML.", this.name));
            Bag.Properties.Add(new PropertySpec("arcType", typeof(string), "Arc Attributes",
                "Nodes will be created in the host of this type.", this.XmlArcType));
            Bag.Properties.Add(new PropertySpec("directed", typeof(Boolean), "Arc Attributes",
                "Does the arc have a definite direction from FROM to TO?", this.directed));
            Bag.Properties.Add(new PropertySpec("doubly directed", typeof(Boolean), "Arc Attributes",
                "Is the arc directed in BOTH directions?", this.doublyDirected));
            Bag.Properties.Add(new PropertySpec("contains all local labels", typeof(Boolean),
                "Arc Attributes", "Does the arc only match with arcs in the host that contain the same" +
                " local labels (no more and no lesss)? <this is meaningless for RHS arcs>", this.containsAllLocalLabels));
            Bag.Properties.Add(new PropertySpec("direction is equal", typeof(Boolean),
                "Arc Attributes", "Does the arc only match with arcs with the same direction booleans? " +
                "(If false, then undirected matches with directed and directed with doubly-directed.)" +
                " <this is meaningless for RHS arcs>",
                this.directionIsEqual));
            Bag.Properties.Add(new PropertySpec("null means null", typeof(Boolean),
                "Arc Attributes", "Null nodes can be matched with any node. If this is true, then the arc" +
                " is considered as a dangling and will only match with other danglings <this is meaningless for RHS arcs>",
                this.nullMeansNull));
        }

        /* Allows the propertygrid to set new values */
        protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Labels": e.Value = this.localLabels; break;
                case "Variables": e.Value = this.localVariables; break;
                case "Name": e.Value = this.name; break;
                case "arcType": e.Value = this.XmlArcType; break;
                case "directed": e.Value = this.directed; break;
                case "doubly directed": e.Value = this.doublyDirected; break;
                case "contains all local labels": e.Value = this.containsAllLocalLabels; break;
                case "direction is equal": e.Value = this.directionIsEqual; break;
                case "null means null": e.Value = this.nullMeansNull; break;
            }
        }

        /* Allows the propertygrid to set new values. */
        protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Labels": this.localLabels = (List<string>)e.Value; break;
                case "Variables": this.localVariables = (List<double>)e.Value; break;
                case "Name": this.name = (string)e.Value; break;
                case "arcType": this.XmlArcType = (string)e.Value; break;
                case "directed": this.directed = (Boolean)e.Value; break;
                case "doubly directed": this.doublyDirected = (Boolean)e.Value; break;
                case "contains all local labels": this.containsAllLocalLabels = (Boolean)e.Value; break;
                case "direction is equal": this.directionIsEqual = (Boolean)e.Value; break;
                case "null means null": this.nullMeansNull = (Boolean)e.Value; break;
            }
        }

        #endregion
    }
}
