using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace GraphSynth.Representation
{
    public partial class ruleNode : node
    {
        #region Constructors
        public ruleNode(string newName) : base(newName) { }
        public ruleNode() : base() { }
        #endregion

        #region Subset or Equal Booleans
        /* The following booleans capture the possible ways in which a node may/may not be a subset 
         * (boolean set to false) or is equal (in this respective quality) to the host (boolean set 
         * to true). These are special subset or equal booleans used by recognize. For this 
         * fundamental node classes, only these two possible conditions exist. */
        public Boolean containsAllLocalLabels;
        /* if true then all the localLabels in the lNode match with those in the host node, if false 
         * then lNode only needs to be a subset on host node localLabels. */
        public Boolean strictDegreeMatch;
        /* this boolean is to distinguish that a particular node
         * of L has all of the arcs of the host node. Again,
         * if true then use equal
         * if false then use subset 
         * NOTE: this is commonly misunderstood to be the same as induced. The difference is that this
         * applies to each node in the LHS and includes arcs that reference nodes not found on the LHS*/
        #endregion

        #region Methods
        public Boolean matchWith(node hostNode)
        {
            if (hostNode != null)
            {
                if (((strictDegreeMatch && (this.degree == hostNode.degree)) ||
                    (!strictDegreeMatch && (this.degree <= hostNode.degree))) &&
                    (labelsMatch(this.localLabels, hostNode.localLabels)) &&
                    (intendedTypesMatch(this.nodeType, hostNode.nodeType)))
                    return true;
                else return false;
            }
            else return false;
        }

        private Boolean labelsMatch(List<string> LLabels, List<string> hostLabels)
        {

            foreach (string label in LLabels)
                if (!hostLabels.Contains(label)) return false;
            if (containsAllLocalLabels)
                /* if 'containsAllLocalLabels' is true than your are checking for equality.
                 * but how to do that? basically if a is a subset of b AND b is a subset of a. */
                foreach (string label in hostLabels)
                    if (!LLabels.Contains(label)) return false;
            return true;
        }

        private Boolean intendedTypesMatch(Type LNodeType, Type hostNodeType)
        {
            if ((LNodeType == null) ||
                (LNodeType == typeof(GraphSynth.Representation.node)) ||
                (LNodeType == typeof(GraphSynth.Representation.ruleNode)) ||
                (LNodeType == hostNodeType))
                return true;
            else return false;
        }
        #endregion
    }
}