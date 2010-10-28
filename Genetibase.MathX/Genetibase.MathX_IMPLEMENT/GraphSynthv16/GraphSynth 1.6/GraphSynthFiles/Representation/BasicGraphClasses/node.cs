using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace GraphSynth.Representation
{
    public partial class node 
    {
        public string name;
        /* a node contains both characterizing strings, known as localLabels
         * and numbers, stored as localVariables. */
        public List<string> localLabels = new List<string>();
        public List<double> localVariables = new List<double>();
        /* the list of arcs connecting to this node are stored here. */
        [XmlIgnore]
        public List<arc> arcs = new List<arc>();
        /* additionally these are divided into arcs coming into the node - 
         * those in which the head or TO of the arc connects to the node (arcsto),
         * and those leaving the node, tail of arc, FROM of the arc . */
        [XmlIgnore]
        public List<arc> arcsTo = new List<arc>();
        [XmlIgnore]
        public List<arc> arcsFrom = new List<arc>();
        /* The decision to ignore these  in the XML is to make the xml file more compact,
         * and avoid infinite loops in the (de-)serializtion. The arcs will contain to To
         * and From nodes to indicate how the graph is connected. */

        #region Constructors
        /* either make new node with a prescribed name, or give it a name never seen before. */
        public node(string newName)
        {
            name = newName;
        }
        public node() : this(Guid.NewGuid().ToString()) { }

        #endregion

        #region Properties
        /* the degree or valence of a node is the number of arcs connecting to it.
         * Currently this is used in recognition of a rule when the strictDegreeMatch
         * is checked. */
        public int degree
        {
            get { return arcs.Count; }
        }
        #endregion

        #region Copy Method
        public virtual node copy()
        {
            return (this.copy(new node(this.name)));
        }
        public virtual node copy(node copyOfNode)
        {
            copyOfNode.name = this.name;
            copyOfNode.shapekey = this.shapekey.ToString();
            copyOfNode.screenX = this.screenX;
            copyOfNode.screenY = this.screenY;
            copyOfNode.nodeType = this.nodeType;
            foreach (string label in this.localLabels)
                copyOfNode.localLabels.Add(label.ToString());
            foreach (double var in this.localVariables)
                copyOfNode.localVariables.Add(var);

            return copyOfNode;
        }
        #endregion
    }
}