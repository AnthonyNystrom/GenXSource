using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GraphSynth.Representation
{
    /* here we define additional qualities used only by arcs in the grammar rules. */
    public partial class ruleArc : arc
    {
        #region Constructors
        public ruleArc(string newName) : base(newName) { }
        public ruleArc() : base() { }
        #endregion

        #region Subset or Equal Booleans
        /* The following booleans capture the possible ways in which an arc may/may not be a subset 
         * (boolean set to false) or is equal (in this respective quality) to the host (boolean set 
         * to true). These are special subset or equal booleans used by recognize. For this 
         * fundamental arc classes, only these three possible conditions exist. */
        public Boolean containsAllLocalLabels;
        /* if true then all the localLabels in the lArc match with those in the host arc, if false 
         * then lArc only needs to be a subset on host arc localLabels. */
        public Boolean directionIsEqual;
        /* this boolean is to distinguish that the directionality
         * within an arc matches perfectly. If false then all (singly)-directed arcs
         * will match with doubly-directed arcs, and all undirected arcs will match with all
         * directed and doubly-directed arcs. Of course, a directed arc going one way will 
         * still not match with a directed arc going the other way.
         * If true, then undirected only matches with undirected, directed only with directed (again, the
         * actual direction must match too), and doubly-directed only with doubly-directed. */
        public Boolean nullMeansNull;
        /* for a lack of a better name - this play on "no means no" applies to dangling arcs that point
         * to null instead of pointing to another node. If this is set to false, then we are saying a 
         * null reference on an arc can be matched with a null in the graph or any node in the graph. 
         * Like the above, a false value is like a subset in that null is a subset of any actual node. 
         * And a true value means it must match exactly or in otherwords, "null means null" - null 
         * matches only with a null in the host. If you want the rule to be recognized only when an actual
         * node is present simply add a dummy node with no distinguishing characteristics. That would
         * in turn nullify this boolean since this boolean only applies when a null pointer exists in
         * the rule. */
        #endregion

        #region Methods
        public Boolean matchWith(arc hostArc, node fromHostNode, node toHostNode, Boolean traverseForward)
        {
            if (matchWith(hostArc))
            {
                if (this.directed
                    && (((hostArc.To == toHostNode) && (hostArc.From == fromHostNode) && traverseForward)
                        || ((hostArc.From == toHostNode) && (hostArc.To == fromHostNode) && !traverseForward)))
                    return true;
                else if (((hostArc.To == toHostNode) && (hostArc.From == fromHostNode))
                        || ((hostArc.From == toHostNode) && (hostArc.To == fromHostNode)))
                    return true;
                else return false;
            }
            else return false;
        }
        public Boolean matchWith(arc hostArc, node fromHostNode, Boolean traverseForward)
        {
            if (matchWith(hostArc))
            {
                if (this.directed)
                {
                    if (((hostArc.From == fromHostNode) && traverseForward)
                        || ((hostArc.To == fromHostNode) && !traverseForward))
                        return true;
                    else return false;
                }
                else if ((hostArc.From == fromHostNode) || (hostArc.To == fromHostNode))
                    return true;
                else return false;
            }
            else return false;
        }
        public Boolean matchWith(arc hostArc)
        {
            if (hostArc != null)
            {
                if ((directionIsEqual && (this.doublyDirected == hostArc.doublyDirected)
                    && (this.directed == hostArc.directed))
                    || (!directionIsEqual
                    && (hostArc.doublyDirected || !this.directed
                    || (this.directed && hostArc.directed && !this.doublyDirected))))
                /* pardon my french, but this statement is a bit of a mindf**k. What it says is if 
                 * directionIsEqual, then simply the boolean state of the doublyDirected and directed 
                 * must be identical in L and in the host. Otherwise, one of three things must be equal.
                 * first, hostArc's doublyDirected is true so whatever LArc's qualities are, it is a subset of it.
                 * second, LArc's not directed so it is a subset with everything else.
                 * third, they both are singly directed and LArc is not doublyDirected. */
                {
                    if ((labelsMatch(this.localLabels, hostArc.localLabels)) &&
                        (intendedTypesMatch(this.arcType, hostArc.arcType)))
                        return true;
                    else return false;
                }
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
        private Boolean intendedTypesMatch(Type LArcType, Type hostArcType)
        {
            if ((LArcType == null) ||
                (LArcType == typeof(GraphSynth.Representation.arc)) ||
                (LArcType == typeof(GraphSynth.Representation.ruleArc)) ||
                (LArcType == hostArcType))
                return true;
            else return false;
        }
        #endregion
    }
}