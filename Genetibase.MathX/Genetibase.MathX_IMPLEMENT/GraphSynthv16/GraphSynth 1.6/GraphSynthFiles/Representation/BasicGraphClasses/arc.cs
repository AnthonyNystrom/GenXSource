using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace GraphSynth.Representation
{
    public partial class arc 
    {
        public string name;
        /* just like a node, an arc can contain both characterizing strings, known as localLabels
         * and numbers, stored as localVariables. */
        public List<string> localLabels = new List<string>();
        public List<double> localVariables = new List<double>();
        /* each arc connects to two and only two nodes, these are stored in protected elements. */
        protected node from = null;
        protected node to = null;
        /* arc can have a meaningful direction or be labelled doubly-directed. These
         * protected fields are controlled by the similarly named properties below. */
        protected Boolean _directed;
        protected Boolean _doublyDirected;

        #region Properties
        /* From and To do all the work of from and to. Getting the value is simple, but
         * setting a to or from involves removing or adding the arc to the elements of 
         * the node. */
        [XmlIgnore]
        public node From
        {

            get { return from; }
            set
            {
                /* if you are disconnecting an arc... */
                if ((value == null) && (from != null))
                {
                    from.arcs.Remove(this);
                    from.arcsFrom.Remove(this);
                }
                /* if you are connecting an arc to a new node...*/
                if ((value != null) && (!value.arcs.Contains(this)))
                {
                    value.arcs.Add(this);
                    value.arcsFrom.Add(this);
                }
                from = value;
            }
        }
        [XmlIgnore]
        public node To
        {

            get { return to; }
            set
            {
                /* if you are disconnecting an arc... */
                if ((value == null) && (to != null))
                {
                    to.arcs.Remove(this);
                    to.arcsTo.Remove(this);
                }
                /* if you are connecting an arc to a new node...*/
                if ((value != null) && (!value.arcs.Contains(this)))
                {
                    value.arcs.Add(this);
                    value.arcsTo.Add(this);
                }
                to = value;
            }
        }

        /* these Boolean properties manage the protected elements. The trick here
         * is that an arc cannot be doubly-directed and not directed, but it is
         * possible to be directed and not doubly-directed. */
        public Boolean directed
        {
            get { return _directed; }
            set
            {
                if ((!value) && (_doublyDirected == true))
                {
                    _directed = value;
                    _doublyDirected = false;
                }
                else _directed = value;
            }
        }
        public Boolean doublyDirected
        {
            get { return _doublyDirected; }
            set
            {
                if ((value) && (_directed == false))
                {
                    _doublyDirected = value;
                    _directed = true;
                }
                else _doublyDirected = value;
            }
        }

        public node otherNode(node node1)
        /* well, this isn't exactly a property, but it's kinda used like one.
         * here, we know one of the nodes that the arc is connected to, but not
         * the other. So, we are simply asking for the node other than the one we know.*/
        {
            if (this.from == node1) return this.to;
            else if (this.to == node1) return this.from;
            else return null;
        }


        #endregion

        #region Constructors
        /* either make new arc with a prescribed name, or give it a name never seen before. 
         * additionally one can provide the connecting nodes at this time or later. */
        public arc(string newName, node fromNode, node toNode)
        {
            name = newName;
            from = fromNode;
            to = toNode;
        }
        public arc(string newName) : this(newName, null, null) { }
        public arc() : this(Guid.NewGuid().ToString(), null, null) { }
        #endregion

        #region Copy Method
        public virtual arc copy()
        {
            return (this.copy(new arc(this.name)));
        }
        public virtual arc copy(arc copyOfArc)
        {
            foreach (string label in this.localLabels)
                copyOfArc.localLabels.Add(label.ToString());
            foreach (double var in this.localVariables)
                copyOfArc.localVariables.Add(var);
            copyOfArc.curveStyle = this.curveStyle;
            copyOfArc.directed = this.directed;
            copyOfArc.doublyDirected = this.doublyDirected;
            copyOfArc.fromConnector = this.fromConnector;
            copyOfArc.toConnector = this.toConnector;
            copyOfArc.arcType = this.arcType;

            return copyOfArc;
        }
        #endregion
    }
}