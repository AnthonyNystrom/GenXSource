using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Netron.GraphLib;

namespace GraphSynth.Representation
{
    public partial class designGraph
    {
        #region Constructors
        public designGraph() { }

        /* here is a constructor that will be rarely used but is convenient in 
         * certain applications. If one invokes new designgraph(num), then this
         * constructor will complete a complete graph of num nodes. A complete 
         * graph is where each node connects to every other node. */
        public designGraph(int numNodes)
        {
            for (int i = 0; i != numNodes; i++)
            {
                addNode("seedNode" + i.ToString());
            }
            int k = 0;
            for (int i = 0; i != numNodes; i++)
            {
                for (int j = i + 1; j != numNodes; j++)
                {

                    addArc("seedArc" + k.ToString(), nodes[i], nodes[j]);
                    k++;
                }
            }
        }
        /* currently this constructor is used within the recognize function of the 
         * grammar rule to establish each of the recognized locations. */
        public designGraph(List<node> newNodes, List<arc> newArcs)
        {
            foreach (node n in newNodes)
                nodes.Add(n);
            foreach (arc a in newArcs)
                arcs.Add(a);
        }
        /* another rarely used constructor is to create a random graph that takes
         * two parameters: the number of nodes, and the average degree. Note: that
         * there is no guarantee that the graph will be connected. */
        public designGraph(int numNodes, int aveDegree)
        {
            double arcProb = (double)aveDegree / (numNodes + 1);
            Random rnd = new Random();
            for (int i = 0; i != numNodes; i++)
            {
                addNode("seedNode" + i.ToString());
            }
            int k = 0;
            for (int i = 0; i != numNodes; i++)
            {
                for (int j = i + 1; j != numNodes; j++)
                {
                    if ((double)rnd.Next(1000) / 1000 <= arcProb)
                    {
                        addArc("seedArc" + k.ToString(), nodes[i], nodes[j]);
                        k++;
                    }
                }
            }
        }
        #endregion

        #region Open & Save
        public static void saveGraphToXml
            (string filename, Netron.GraphLib.UI.GraphControl graphControl1, designGraph graph1)
        {
            graph1.updateFromGraphControl(graphControl1);
            graph1.checkForRepeatNames();
            saveGraphToXml(filename, graph1);
        }
        public static void saveGraphToXml(string filename, designGraph graph1)
        {
            graph1.checkForRepeatNames();
            StreamWriter graphWriter = null;
            try
            {
                graphWriter = new StreamWriter(filename);
                XmlSerializer graphSerializer = new XmlSerializer(typeof(designGraph));
                graphSerializer.Serialize(graphWriter, graph1);
            }
            catch (Exception ioe)
            {
                MessageBox.Show(ioe.ToString(), "XML Serialization Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            finally
            {
                if (graphWriter != null) graphWriter.Close();
            }
        }

        public static designGraph openGraphFromXml(string filename)
        {
            StreamReader graphReader = null;
            designGraph newDesignGraph = null;
            try
            {
                graphReader = new StreamReader(filename);
                XmlSerializer graphDeserializer = new XmlSerializer(typeof(designGraph));
                newDesignGraph = (designGraph)graphDeserializer.Deserialize(graphReader);
                SearchIO.output(Path.GetFileName(filename) + " successfully loaded.");
                newDesignGraph.internallyConnectGraph();
                if (newDesignGraph.name == null)
                    newDesignGraph.name = Path.GetFileName(filename).TrimEnd(new char[] { '.', 'x', 'm', 'l' });
            }
            catch (Exception ioe)
            { SearchIO.output("Error Opening Graph" + ioe.ToString()); }
            finally
            {
                if (graphReader != null) graphReader.Close();
            }
            return newDesignGraph;
        }

        #endregion

        #region Update display
        public void updateFromGraphControl(Netron.GraphLib.UI.GraphControl graphControl1)
        {
            try
            {
                node tempnode;
                List<node> tempNodes = new List<node>();
                arc temparc;
                List<arc> tempArcs = new List<arc>();
                Shape fromShape, toShape;

                foreach (Shape a in graphControl1.Shapes)
                {
                    if (!nodes.Exists(delegate(node b) { return (b.displayShape == a); }))
                    {
                        tempnode = new node(nameFromText(a.Text));
                        tempNodes.Add(tempnode);
                    }
                    else
                    {
                        tempnode = nodes.Find(delegate(node b) { return (b.displayShape == a); });
                        tempNodes.Add(tempnode);
                    }
                    if (a.Text != "[Not_set]")
                    {
                        tempnode.name = nameFromText(a.Text);
                        tempnode.localLabels = labelsFromText(a.Text);
                    }
                    tempnode.screenX = a.X;
                    tempnode.screenY = a.Y;
                    tempnode.shapekey = node.lookupShapeKey(a);
                    tempnode.displayShape = a;
                }
                nodes = tempNodes;
                foreach (Connection a in graphControl1.Connections)
                {
                    if (!arcs.Exists(delegate(arc b) { return (b.displayShape == a); }))
                    {
                        temparc = new arc(nameFromText(a.Text));
                        tempArcs.Add(temparc);
                    }
                    else
                    {
                        temparc = arcs.Find(delegate(arc b) { return (b.displayShape == a); });
                        tempArcs.Add(temparc);
                    }
                    fromShape = a.From.BelongsTo;
                    toShape = a.To.BelongsTo;

                    temparc.From = nodes.Find(delegate(node c)
                     { return (sameName(c.name, fromShape.Text)); });
                    temparc.To = nodes.Find(delegate(node c)
                     { return (sameName(c.name, toShape.Text)); });

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
                        temparc.name = nameFromText(a.Text);
                        temparc.localLabels = labelsFromText(a.Text);
                    }
                    temparc.curveStyle = a.LinePath;
                    if (a.LineEnd == ConnectionEnd.NoEnds)
                        temparc.directed = false;
                    else if (a.LineEnd == ConnectionEnd.BothFilledArrow ||
                                a.LineEnd == ConnectionEnd.BothOpenArrow)
                        temparc.doublyDirected = true;
                    else
                    {
                        temparc.doublyDirected = false;
                        temparc.directed = true;
                        if (a.LineEnd == ConnectionEnd.LeftFilledArrow ||
                                a.LineEnd == ConnectionEnd.LeftOpenArrow)
                        {
                            /* we have a little situation on our hands here. the user drew the arc 
                             * from one node to another but now wants it to go the other way. */
                            tempnode = temparc.To;
                            temparc.To = temparc.From;
                            temparc.From = tempnode;
                        }
                    }
                    temparc.displayShape = a;
                }
                arcs = tempArcs;
                this.internallyConnectGraph();
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error updating graph from the display. Please save work and re-open. (Error: " + e.ToString() + ")", "Error Updating Graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void updateGraphControl(Netron.GraphLib.UI.GraphControl graphControl1,
            Label globalLabelsText)
        {
            try
            {
                string defaultShapeKey = "8ED1469D-90B2-43ab-B000-4FF5C682F530";
                Boolean isFixed = true;
                Random rnd = new Random();

                #region Global Labels
                if (this.globalLabels.Count > 0)
                {
                    globalLabelsText.Text = StringCollectionConverter.convert(this.globalLabels);
                }
                else
                {
                    globalLabelsText.Text = " ";
                }
                #endregion
                #region display the nodes
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    node n = nodes[i];
                    #region if it has no displayShape
                    if (n.displayShape == null)
                    {
                        if (n.shapekey == null) n.shapekey = defaultShapeKey;
                        if (n.screenX == 0.0 && n.screenY == 0.0)
                        {
                            n.screenX = rnd.Next(50, graphControl1.Width - 100);
                            n.screenY = rnd.Next(20, graphControl1.Height - 20);
                            isFixed = false;
                        }
                        else isFixed = true;
                        n.displayShape = graphControl1.AddShape(n.shapekey, new PointF(n.screenX, n.screenY));
                        /* if the prev. statement didn't take, it's likely the shapekey wasn't recognized.
                         * there we try again with the default ShapeKey. */
                        if (n.displayShape == null)
                            n.displayShape = graphControl1.AddShape(defaultShapeKey, new PointF(n.screenX, n.screenY));
                    }
                    #endregion

                    else if (!graphControl1.Shapes.Contains(n.displayShape))
                    {
                        /* a shape is defined for the node but is not displayed */
                        n.displayShape = graphControl1.AddShape(n.displayShape);
                    }
                    n.displayShape.Text = textForNode(n);
                    n.displayShape.IsFixed = isFixed;

                    /* make sure node is of right type - if not call the replacement function */
                    if ((n.nodeType != null) && (n.GetType() != typeof(GraphSynth.Representation.ruleNode))
                        && (n.GetType() != n.nodeType))
                        replaceNodeWithInheritedType(n);
                }
                #endregion
                #region display the arcs
                for (int i = arcs.Count - 1; i >= 0; i--)
                {
                    arc a = arcs[i];
                    node fromNode = a.From;
                    node toNode = a.To;

                    Connector fromConnect = null;
                    Connector toConnect = null;
                    if ((fromNode == null) || (fromNode.displayShape == null))
                    {
                        a.fromConnector = 0;
                        if (a.displayShape == null || a.displayShape.From.BelongsTo == null)
                            fromConnect = addNullShape(graphControl1, 0).Connectors[0];
                        else fromConnect = a.displayShape.From;
                    }
                    else if ((a.fromConnector == -1) ||
                        (a.fromConnector >= fromNode.displayShape.Connectors.Count))
                    {
                        a.fromConnector = rnd.Next(fromNode.displayShape.Connectors.Count);
                        fromConnect = fromNode.displayShape.Connectors[a.fromConnector];
                    }
                    else { fromConnect = fromNode.displayShape.Connectors[a.fromConnector]; }
                    /* now repeat same process for To */
                    if ((toNode == null) || (toNode.displayShape == null))
                    {
                        a.toConnector = 0;
                        if (a.displayShape == null || a.displayShape.To.BelongsTo == null)
                            toConnect = addNullShape(graphControl1, 1).Connectors[0];
                        else toConnect = a.displayShape.To;
                    }
                    else if ((a.toConnector == -1) ||
                        (a.toConnector >= toNode.displayShape.Connectors.Count))
                    {
                        a.toConnector = rnd.Next(toNode.displayShape.Connectors.Count);
                        toConnect = toNode.displayShape.Connectors[a.toConnector];
                    }
                    else { toConnect = toNode.displayShape.Connectors[a.toConnector]; }

                    if (((a.displayShape != null) && (!graphControl1.Connections.Contains(a.displayShape)) &&
                         ((a.displayShape.From == null) || (a.displayShape.To == null))) ||
                         ((a.displayShape == null) && (fromConnect == null) && (toConnect == null)))
                        removeArc(a);
                    else
                    {
                        if (a.displayShape == null)
                            a.displayShape = graphControl1.AddConnection(fromConnect, toConnect);
                        else if (!graphControl1.Connections.Contains(a.displayShape))
                            a.displayShape = graphControl1.AddConnection(a.displayShape.From, a.displayShape.To);

                        /* a shape is defined for the node but is not displayed 
                         * Rectangular, Default, Bezier */
                        if (a.curveStyle != null)
                            a.displayShape.LinePath = a.curveStyle;
                        if (a.doublyDirected)
                            a.displayShape.LineEnd = ConnectionEnd.BothFilledArrow;
                        else if (a.directed)
                            a.displayShape.LineEnd = ConnectionEnd.RightFilledArrow;
                        else a.displayShape.LineEnd = ConnectionEnd.NoEnds;
                        a.displayShape.Text = textForArc(a);
                        a.displayShape.ShowLabel = true;
                    }

                    /* make sure node is of right type - if not call the replacement function */
                    if ((a.arcType != null) && (a.GetType() != typeof(GraphSynth.Representation.ruleArc))
                        && (a.GetType() != a.arcType))
                        replaceArcWithInheritedType(a, fromNode, toNode);
                }
                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show("There was an error displaying the graph. Please save work and re-open. (Error: " + e.ToString() + ")", "Error Displaying Graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void replaceNodeWithInheritedType(node origNode)
        {
            this.addNode(origNode.name, origNode.nodeType);
            origNode.copy(nodes[lastNode]);
            nodes[lastNode].displayShape = origNode.displayShape;
            foreach (arc a in origNode.arcsFrom)
                a.From = nodes[lastNode];
            foreach (arc a in origNode.arcsTo)
                a.To = nodes[lastNode];

            this.removeNode(origNode);
        }
        private void replaceArcWithInheritedType(arc origArc, node fromNode, node toNode)
        {
            this.addArc(origArc.name, origArc.arcType, fromNode, toNode);
            origArc.copy(arcs[lastArc]);
            arcs[lastArc].displayShape = origArc.displayShape;
            this.removeArc(origArc);
        }
        private Shape addNullShape(Netron.GraphLib.UI.GraphControl graphControl1, int onRight)
        {
            Shape nullNode = graphControl1.AddBasicShape(
                BasicShapeType.tinyZNode, "null",
                new PointF(graphControl1.Width * onRight, graphControl1.Height / 2));
            nullNode.ShapeColor = Color.White;
            nullNode.ZOrder = 10;
            return nullNode;
        }
        public void displayGraph(Netron.GraphLib.UI.GraphControl graphControl1, Label globalLabelsText)
        {
            this.updateGraphControl(graphControl1, globalLabelsText);

            graphControl1.StartLayout();

            initPropertiesBag();
        }
        #endregion

        #region name to text
        public Boolean sameName(string currentName, string candidateName)
        {
            return currentName == nameFromText(candidateName);
        }
        public string nameFromText(string candidateName)
        {
            return StringCollectionConverter.convert(candidateName)[0];
        }
        public string textForNode(node a)
        {
            return textFromNameAndLabels(a.name, a.localLabels);
        }
        public string textForArc(arc a)
        {
            return textFromNameAndLabels(a.name, a.localLabels);
        }
        public string textFromNameAndLabels(string name, List<string> labels)
        {
            if (labels.Count > 0)
            {
                return name + " (" + StringCollectionConverter.convert(labels) + ")";
            }
            else return name;
        }
        public List<string> labelsFromText(string text)
        {
            List<string> labels = StringCollectionConverter.convert(text);
            labels.RemoveAt(0);
            return labels;
        }
        #endregion

        #region misc. Methods
        public designGraph copy()
        {
            /* at times we want to copy a graph and not refer to the same objects. This happens mainly
             * (rather initially what inspired this function) when the seed graph is copied into a candidate.*/
            int toIndex, fromIndex;
            designGraph copyOfGraph = new designGraph();

            copyOfGraph.name = name;
            foreach (string label in globalLabels)
                copyOfGraph.globalLabels.Add(label.ToString());
            foreach (node origNode in nodes)
            {
                copyOfGraph.nodes.Add(origNode.copy());
            }
            foreach (arc origArc in arcs)
            {
                arc copyOfArc = origArc.copy();
                toIndex = nodes.FindIndex(delegate(node a) { return (a == origArc.To); });
                fromIndex = nodes.FindIndex(delegate(node b) { return (b == origArc.From); });
                copyOfGraph.addArc(copyOfArc, fromIndex, toIndex);
            }
            return copyOfGraph;
        }
        public Boolean checkForRepeatNames()
        {
            Boolean anyNameChanged = false;
            for (int i = 0; i != nodes.Count; i++)
            {
                Boolean nameChanged = false;
                for (int j = i + 1; j != nodes.Count; j++)
                    if (sameName(nodes[i].name, nodes[j].name))
                    {
                        nodes[j].name += j.ToString();
                        nameChanged = true;
                        anyNameChanged = true;
                    }
                if (nameChanged) nodes[i].name += i.ToString();
            }
            for (int i = 0; i != arcs.Count; i++)
            {
                Boolean nameChanged = false;
                for (int j = i + 1; j != arcs.Count; j++)

                    if (sameName(arcs[i].name, arcs[j].name))
                    {
                        arcs[j].name += j.ToString();
                        nameChanged = true;
                        anyNameChanged = true;
                    }

                if (nameChanged) arcs[i].name += i.ToString();
            }
            return anyNameChanged;
        }
        public void internallyConnectGraph()
        {
            for (int i = nodes.Count; i != 0; i--)
            {
                if (nodes[(i - 1)].name.StartsWith("null"))
                    nodes.RemoveAt(i - 1);
            }
            foreach (arc a in arcs)
            {
                if ((a.From == null) || a.From.name.StartsWith("null"))
                    a.From = null;
                else
                {
                    a.From = nodes.Find(delegate(node b)
                            {
                                return (sameName(b.name, a.From.name));
                            });
                }
                if ((a.To == null) || a.To.name.StartsWith("null"))
                    a.To = null;
                else
                {
                    a.To = nodes.Find(delegate(node b)
                            {
                                return (sameName(b.name, a.To.name));
                            });
                }

            }
        }
        #endregion

        #region graphPropsBag
        /*** the property bag ***/
        private PropertyBag graphPropsBag;

        [XmlIgnore]
        public PropertyBag Bag
        {
            get { return graphPropsBag; }
        }
        /* Adds the basic properties of the shape */
        public void AddPropertiesToBag()
        {
            /*the global lables of the shape */
            Bag.Properties.Add(new PropertySpec("Global Labels", typeof(List<string>), "Graph Specific Attributes",
                "Labels assigned to the entire graph", this.globalLabels,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Variables", typeof(List<double>), "Graph Specific Attributes",
                "Variables assigned to the entire graph", this.globalVariables,
                typeof(System.Drawing.Design.UITypeEditor), typeof(DoubleCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Name", typeof(string), "Graph Specific Attributes",
                "A name/filename for the graph", this.name));
        }

        /* Allows the propertygrid to set new values */
        protected void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Global Labels": e.Value = this.globalLabels; break;
                case "Variables": e.Value = this.globalVariables; break;
                case "Name": e.Value = this.name; break;
                case "Graphic": e.Value = this.graphic; break;
            }
        }

        /* Allows the propertygrid to set new values. */
        protected void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Global Labels": this.globalLabels = (List<string>)e.Value; break;
                case "Variables": this.globalVariables = (List<double>)e.Value; break;
                case "Name": this.name = (string)e.Value; break;
            }
        }

        public void initPropertiesBag()
        {
            graphPropsBag = new PropertyBag(this);
            this.AddPropertiesToBag();
            graphPropsBag.GetValue += new PropertySpecEventHandler(GetPropertyBagValue);
            graphPropsBag.SetValue += new PropertySpecEventHandler(SetPropertyBagValue);
        }
        #endregion

    }
    [XmlInclude(typeof(GraphSynth.Representation.ruleNode))]
    public partial class node
    {
        #region displayGraph specific values
        public string shapekey = "";
        public float screenX = 0.0F;
        public float screenY = 0.0F;
        [XmlIgnore]
        public Shape displayShape;

        public static string lookupShapeKey(Shape a)
        {
            string shapecase = a.GetType().ToString();
            string ZFunKey = typeof(Netron.GraphLib.BasicShapes.ZNode).ToString();
            string simpleNodeKey = typeof(Netron.GraphLib.BasicShapes.SimpleNode).ToString();
            string smallZFunKey = typeof(Netron.GraphLib.BasicShapes.smallZNode).ToString();
            string tinyZFunKey = typeof(Netron.GraphLib.BasicShapes.tinyZNode).ToString();

            if (shapecase == ZFunKey) return "6E92FCD0-75DF-4f8f-A5B2-2927E22F4F0F";
            else if (shapecase == smallZFunKey) return "b2178640-076f-4520-b33c-c603466bc2fc";
            else if (shapecase == tinyZFunKey) return "4F878611-3196-4d12-BA36-705F502C8A6B";
            else if (shapecase == simpleNodeKey) return "57AF94BA-4129-45dc-B8FD-F82CA3B4433E";
            else return "8ED1469D-90B2-43ab-B000-4FF5C682F530";
        }
        #endregion displayGraph specific values

        #region Type of Node to Create in Host
        [XmlIgnore]
        public Type nodeType;
        [XmlElement("nodeType")]
        public string XmlNodeType
        {
            get
            {
                if ((nodeType != null) && (nodeType.ToString() != "GraphSynth.node"))
                    return nodeType.ToString();
                else return null;
            }
            set
            {
                nodeType = Type.GetType((string)value);
                /* if the user typed a Type but we can't find it, it is likely that
                 * it is being compiled within GraphSynth, so prepend with various
                 * namespaces. */
                if (nodeType == null)
                    nodeType = Type.GetType("GraphSynth." + (string)value);
                if (nodeType == null)
                    nodeType = Type.GetType("GraphSynth.Representation." + (string)value);
                if (nodeType == null)
                    nodeType = Type.GetType("GraphSynth.node");
            }
        }
        #endregion

        #region nodePropsBag
        /*** the property bag ***/
        private PropertyBag nodePropsBag;

        [XmlIgnore]
        public PropertyBag Bag
        {
            get { return nodePropsBag; }
        }
        /* Adds the basic properties of the shape */
        public virtual void AddPropertiesToBag()
        {
            /* the global lables of the shape */
            Bag.Properties.Add(new PropertySpec("Labels", typeof(List<string>), "Node Attributes",
                "Labels assigned to this particular node.", this.localLabels,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Variables", typeof(List<double>), "Node Attributes",
                "Variables assigned to this particular node.", this.localVariables,
                typeof(System.Drawing.Design.UITypeEditor), typeof(DoubleCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Name", typeof(string), "Node Attributes",
                "A name for this node. Note: the node name is NOT used in recognition, but simply used as" +
                " an identifier to the user, and for XML which requires a unique name.", this.name));
            Bag.Properties.Add(new PropertySpec("nodeType", typeof(string), "Node Attributes",
               "Nodes will be created in the host of this type.", this.XmlNodeType));
        }

        /* Allows the propertygrid to set new values */
        protected virtual void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Labels": e.Value = this.localLabels; break;
                case "Variables": e.Value = this.localVariables; break;
                case "Name": e.Value = this.name; break;
                case "nodeType": e.Value = this.XmlNodeType; break;
            }
        }

        /* Allows the propertygrid to set new values. */
        protected virtual void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Labels": this.localLabels = (List<string>)e.Value; break;
                case "Variables": this.localVariables = (List<double>)e.Value; break;
                case "Name": this.name = (string)e.Value; break;
                case "nodeType": this.XmlNodeType = (string)e.Value; break;
            }
        }

        public void initPropertiesBag()
        {
            nodePropsBag = new PropertyBag(this);
            this.AddPropertiesToBag();
            nodePropsBag.GetValue += new PropertySpecEventHandler(GetPropertyBagValue);
            nodePropsBag.SetValue += new PropertySpecEventHandler(SetPropertyBagValue);
        }

        #endregion
    }

    [XmlInclude(typeof(GraphSynth.Representation.ruleArc))]
    public partial class arc
    {
        [XmlElement("From")]
        public string XmlFrom
        {
            get
            {
                if (from != null)
                    return from.name;
                else return null;
            }
            set
            {
                if (from == null)
                    from = new node();
                from.name = value;
            }
        }

        [XmlElement("To")]
        public string XmlTo
        {
            get
            {
                if (to != null)
                    return to.name;
                else return null;
            }
            set
            {
                if (to == null)
                    to = new node();
                to.name = value;
            }
        }

        #region Type of Arc to Create in Host
        [XmlIgnore]
        public Type arcType;
        [XmlElement("arcType")]
        public string XmlArcType
        {
            get
            {
                if ((arcType != null) && (arcType.ToString() != "GraphSynth.arc"))
                    return arcType.ToString();
                else return null;
            }
            set
            {
                arcType = Type.GetType((string)value);
                /* if the user typed a Type but we can't find it, it is likely that
                 * * it is being compiled within GraphSynth, so prepend with various
                 * * namespaces. */
                if (arcType == null)
                    arcType = Type.GetType("GraphSynth." + (string)value);
                if (arcType == null)
                    arcType = Type.GetType("GraphSynth.Representation." + (string)value);
                if (arcType == null)
                    arcType = Type.GetType("GraphSynth.arc");
            }
        }
        #endregion

        #region displayGraph specific values
        [XmlIgnore]
        public Connection displayShape;
        public int toConnector = -1;
        public int fromConnector = -1;
        public string curveStyle = string.Empty;
        #endregion

        #region arcPropsBag
        /*** the property bag ***/
        private PropertyBag arcPropsBag;

        [XmlIgnore]
        public PropertyBag Bag
        {
            get { return arcPropsBag; }
        }
        /* Adds the basic properties of the shape */
        public virtual void AddPropertiesToBag()
        {
            Bag.Properties.Add(new PropertySpec("Labels", typeof(List<string>), "Arc Attributes",
                "Labels assigned to this particular arc.", this.localLabels,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Variables", typeof(List<double>), "Arc Attributes",
                "Variables assigned to this particular arc.", this.localVariables,
                typeof(System.Drawing.Design.UITypeEditor), typeof(DoubleCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Name", typeof(string), "Arc Attributes",
                "A name for this node. Note: the node name is NOT used in recognition, but simply used as" +
                " an identifier to the user, and for XML which requires a unique name.", this.name));
            Bag.Properties.Add(new PropertySpec("arcType", typeof(string), "Arc Attributes",
                "Nodes will be created in the host of this type.", this.XmlArcType));
            Bag.Properties.Add(new PropertySpec("directed", typeof(Boolean), "Arc Attributes",
                "Does the arc have a definite direction from FROM to TO?", this.directed));
            Bag.Properties.Add(new PropertySpec("doubly directed", typeof(Boolean), "Arc Attributes",
                "Is the arc directed in BOTH directions?", this.doublyDirected));
        }

        /* Allows the propertygrid to set new values */
        protected virtual void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Labels": e.Value = this.localLabels; break;
                case "Variables": e.Value = this.localVariables; break;
                case "Name": e.Value = this.name; break;
                case "arcType": e.Value = this.XmlArcType; break;
                case "directed": e.Value = this.directed; break;
                case "doubly directed": e.Value = this.doublyDirected; break;
            }
        }

        /* Allows the propertygrid to set new values. */
        protected virtual void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Labels": this.localLabels = (List<string>)e.Value; break;
                case "Variables": this.localVariables = (List<double>)e.Value; break;
                case "Name": this.name = (string)e.Value; break;
                case "arcType": this.XmlArcType = (string)e.Value; break;
                case "directed": this.directed = (Boolean)e.Value; break;
                case "doubly directed": this.doublyDirected = (Boolean)e.Value; break;
            }
        }

        public void initPropertiesBag()
        {
            arcPropsBag = new PropertyBag(this);
            this.AddPropertiesToBag();
            arcPropsBag.GetValue += new PropertySpecEventHandler(GetPropertyBagValue);
            arcPropsBag.SetValue += new PropertySpecEventHandler(SetPropertyBagValue);
        }

        #endregion
    }

}
