using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GraphSynth.Representation
{
    public partial class designGraph
    {
        #region Fields
        public string name;
        public List<string> globalLabels = new List<string>();
        public List<double> globalVariables = new List<double>();
        public List<node> nodes = new List<node>();
        public List<arc> arcs = new List<arc>();
        #endregion

        #region Properties
        [XmlIgnore]
        public int lastNode
        {
            get { return nodes.Count - 1; }
        }
        [XmlIgnore]
        public int lastArc
        {
            get { return arcs.Count - 1; }
        }
        [XmlIgnore]
        public List<int> graphic
        {
            get
            {
                List<int> _graphic = new List<int>();
                foreach (node currentNode in nodes)
                {
                    _graphic.Add(currentNode.degree);
                }
                return _graphic;
            }
        }
        [XmlIgnore]
        public int maxDegree
        {
            /* apparently there is no MAX function.
             * I thought there was something better, but
             * in the meantime - we write our own.
             * get { return max(this.graphic); } */
            get
            {
                int maxV = 0;
                foreach (int v in this.graphic)
                {
                    if (v > maxV) maxV = v;
                }
                return maxV;
            }
        }
        #endregion

        #region Add and Remove Nodes and Arcs Methods
        /* Here is a series of important graph management functions
         * while it would be easy to just call, for example, ".arcs.add",
         * the difficulty comes in properly linking the nodes 
         * likewise with the nodes and their dangling arcs. */
        #region addArc
        public void addArc(string newName, node fromNode, node toNode)
        { /* this is the main addArc. the remaining three all invoke this one. */
            arcs.Add(new arc(newName, fromNode, toNode));
            arcs[lastArc].From = fromNode;
            arcs[lastArc].To = toNode;
        }
        public void addArc(string newName, int fromNode, int toNode)
        {
            if ((fromNode == -1) && (toNode == -1))
                addArc(newName, null, null);
            else if (fromNode == -1)
                addArc(newName, null, nodes[toNode]);
            else if (toNode == -1)
                addArc(newName, nodes[fromNode], null);
            else addArc(newName, nodes[fromNode], nodes[toNode]);
        }
        public void addArc(Type arcType, int fromNode, int toNode)
        {
            string name = Guid.NewGuid().ToString();
            addArc(name, arcType, fromNode, toNode);
        }
        public void addArc(string newName, Type arcType, int fromNode, int toNode)
        {
            if (arcType == null)
                addArc(newName, fromNode, toNode);
            else
            {
                if ((fromNode == -1) && (toNode == -1))
                    addArc(newName, arcType, null, null);
                else if (fromNode == -1)
                    addArc(newName, arcType, null, nodes[toNode]);
                else if (toNode == -1)
                    addArc(newName, arcType, nodes[fromNode], null);
                else addArc(newName, arcType, nodes[fromNode], nodes[toNode]);
            }
        }
        public void addArc(Type arcType, node fromNode, node toNode)
        {
            string name = Guid.NewGuid().ToString();
            addArc(name, arcType, fromNode, toNode);
        }
        public void addArc(string name, Type arcType, node fromNode, node toNode)
        {
            if (arcType == null)
                addArc(name, fromNode, toNode);
            else
            {
                Type[] types = new Type[3];
                types[0] = typeof(string);
                types[1] = typeof(node);
                types[2] = typeof(node);
                System.Reflection.ConstructorInfo arcConstructor = arcType.GetConstructor(types);

                object[] inputs = new object[3];
                inputs[0] = name;
                inputs[1] = fromNode;
                inputs[2] = toNode;
                arcs.Add((arc)arcConstructor.Invoke(inputs));

                if (fromNode != null)
                {
                    fromNode.arcs.Add(arcs[lastArc]);
                    fromNode.arcsFrom.Add(arcs[lastArc]);
                }
                if (toNode != null)
                {
                    toNode.arcs.Add(arcs[lastArc]);
                    toNode.arcsTo.Add(arcs[lastArc]);
                }
            }
        }
        public void addArc(arc newArc, int fromNode, int toNode)
        {
            if ((fromNode == -1) && (toNode == -1))
                addArc(newArc, null, null);
            else if (fromNode == -1)
                addArc(newArc, null, nodes[toNode]);
            else if (toNode == -1)
                addArc(newArc, nodes[fromNode], null);
            else addArc(newArc, nodes[fromNode], nodes[toNode]);
        }
        public void addArc(arc newArc, node fromNode, node toNode)
        {
            newArc.From = fromNode;
            newArc.To = toNode;
            arcs.Add(newArc);
        }
        #endregion

        #region removeArc
        public void removeArc(int arcIndex)
        {
            removeArc(arcs[arcIndex]);
        }
        public void removeArc(arc arcToRemove)
        {
            if (arcToRemove.From != null)
            {
                arcToRemove.From.arcs.Remove(arcToRemove);
                arcToRemove.From.arcsFrom.Remove(arcToRemove);
            }
            if (arcToRemove.To != null)
            {
                arcToRemove.To.arcs.Remove(arcToRemove);
                arcToRemove.To.arcsTo.Remove(arcToRemove);
            }
            arcs.Remove(arcToRemove);
        }
        #endregion

        #region addNode
        public void addNode(string newName)
        {
            nodes.Add(new node(newName));
        }
        public void addNode(Type nodeType)
        {
            string name = Guid.NewGuid().ToString();
            addNode(name, nodeType);
        }
        public void addNode(string name, Type nodeType)
        {
            if (nodeType == null)
                addNode(name);
            else
            {
                Type[] types = new Type[1];
                types[0] = typeof(string);
                System.Reflection.ConstructorInfo nodeConstructor = nodeType.GetConstructor(types);

                object[] inputs = new object[1];
                inputs[0] = name;
                nodes.Add((node)nodeConstructor.Invoke(inputs));
            }
        }
        #endregion

        #region removeNode
        /* removing a node is a little more complicated than removing arcs
         * since we need to decide what to do with dangling arcs. As a result
         * there are two booleans that specify how to handle the arcs.
         * removeArcToo will simply delete the attached arcs if true, otherwise it
         * will leave them dangling (default is false).
         * removeNodeRef will change the references within the attached arcs to null
         * if set to true, or will leave them if false (default is true). */
        public void removeNode(node nodeToRemove, Boolean removeArcsToo, Boolean removeNodeRef)
        { /* this is the main method, all other overloads eventually funnel into this one. */
            if (removeArcsToo)
            {
                foreach (arc connectedArc in nodeToRemove.arcs)
                    removeArc(connectedArc);
                nodes.Remove(nodeToRemove);
            }
            else if (removeNodeRef)
            {
                List<arc> connectedArcs = new List<arc>();
                connectedArcs.AddRange(nodeToRemove.arcs);
                foreach (arc connectedArc in connectedArcs)
                    if (connectedArc.From == nodeToRemove)
                        connectedArc.From = null;
                    else connectedArc.To = null;
                nodes.Remove(nodeToRemove);
            }
            else nodes.Remove(nodeToRemove);
        }
        public void removeNode(int nodeIndex, Boolean removeArcsToo, Boolean removeNodeRef)
        {
            removeNode(nodes[nodeIndex], removeArcsToo, removeNodeRef);
        }
        public void removeNode(node nodeToRemove, Boolean removeArcsToo)
        {
            removeNode(nodeToRemove, removeArcsToo, true);
        }
        public void removeNode(int nodeIndex, Boolean removeArcsToo)
        {
            removeNode(nodes[nodeIndex], removeArcsToo, true);
        }
        public void removeNode(node nodeToRemove)
        {
            removeNode(nodeToRemove, false, true);
        }
        public void removeNode(int nodeIndex)
        {
            removeNode(nodes[nodeIndex], false, true);
        }
        #endregion
        #endregion
    }
}