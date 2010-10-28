using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;


namespace GraphSynth.Representation
{
    public class embeddingRule
    {
        public string freeArcLabel = "";
        public string LNodeName = "";
        public string neighborNodeLabel = "";
        /* the freeArc can be identified by one of the following :
         * 1. label of dangling arc in D (freeArcLabel)
         * 2. name of node in L-R that arc was recently attached to (note the name is from L not G)
         * (LNodeName)
         * 3. label of node in G that is currently attached to dangling arc in D (neighborNodeLabel)*/
        public string RNodeName = "";
        /* the RHS of the rule is simply the name of the R-node that the arc is to connect to. Since
         * this exists within the rule, there is no need to include any other defining character - of
         * course we still need to find the corresponding node in H1 to connect it to. Note, this is 
         * also the main quality that distinguishes the approach as NCE or NLC, as the control is given
         * to the each individual of R-L (or the daughter graph in the NCE lingo) as opposed to simply
         * a label based method. */
        public sbyte originalDirection = 0;
        public sbyte newDirection = 0;
        /* in order to give the edNCE approach the "ed" quality, we must allow for the possibility of
         * recognizing arcs having a particular direction. The original direction can be either +1 meaning
         * "to", or -1 meaning "from", or 0 meaning no imposed direction - this indicates what side of the 
         * arc is dangling. Furthermore, the newDirection, can specify a new direction of the arc ("to",
         * or "from" being the new connection) or "" (unspecified) for updating the arc. This allows us 
         * to change the direction of the arc, or keep it as is. */
        /* i used to have another booleans called delete "public Boolean delete = false;", which 
         * if delete is true would then remove the dangling arc from the graph. I later determined this was
         * not necessary since any arcs at the end will be deleted anyway. */
        public Boolean allowArcDuplication = false;
        /* if allowArcDuplication is true then for each rule that matches with the arc the arc will be 
         * duplicated. */

        public static Boolean arcIsFree(arc a, designGraph host, out sbyte freeEndIdentifier, node neighborNode)
        {
            if (a.From != null && a.To != null &&
                !host.nodes.Contains(a.From) && !host.nodes.Contains(a.To))
            {
                freeEndIdentifier = 0;
                /* if the nodes on either end of the freeArc are pointing to previous nodes 
                 * that were deleted in the first pushout then neighborNode is null (and as
                 * a result any rules using the neighborNodeLabel will not apply) and the 
                 * freeEndIdentifier is zero. */
                neighborNode = null;
                return true;
            }
            else if (a.From != null && !host.nodes.Contains(a.From))
            {
                freeEndIdentifier = -1;
                /* freeEndIdentifier set to -1 means that the From end of the arc must be the free end.*/
                neighborNode = a.To;
                return true;
            }
            else if (a.To != null && !host.nodes.Contains(a.To))
            {
                freeEndIdentifier = +1;
                /* freeEndIdentifier set to +1 means that the To end of the arc must be the free end.*/
                neighborNode = a.From;
                return true;
            }
            else
            {
                /* else, the arc is not a free arc after all and we simply break out 
                 * of this loop and try the next arc. */
                freeEndIdentifier = 0;
                neighborNode = null;
                return false;
            }

        }

        public node findNewNodeToConnect(designGraph R, designGraph Rmapping)
        {
            /* find R-L node that is to be connected with freeArc as well as old L-R node name*/
            if ((RNodeName != null) && (RNodeName != ""))
            {
                /* take the RNodeName from within the rule and get the proper reference to the new node.
                 * If there is no RNodeName, then the embedding rule will set the reference to null. */
                int index = R.nodes.FindIndex(delegate(node b) { return (b.name == RNodeName); });
                return Rmapping.nodes[index];
            }
            else return null;
        }
        public node findDeletedNode(designGraph L, designGraph Lmapping)
        {
            /* similarly, we can find the LNodeName (if one exists in this particular rule). Setting this
             * up now saves time and space in the below recognition if-then's. */
            if ((LNodeName != null) && (LNodeName != ""))
            {
                int index = L.nodes.FindIndex(delegate(node b) { return (b.name == LNodeName); });
                return Lmapping.nodes[index];
            }
            else return null;
        }

        public Boolean ruleIsRecognized(sbyte freeEndIdentifier, arc freeArc,
            node neighborNode, node nodeRemoved)
        {
            if (freeEndIdentifier * originalDirection >= 0)
            {
                /* this one is a little bit of enigmatic but clever coding if I do say so myself. Both
                    * of these variables can be either +1, 0, -1. If in multiplying the two together you 
                    * get -1 then this is the only incompability. Combinations of +1&+1, or +1&0, or 
                    * -1&-1 all mean that the arc has a free end on the requested side (From or To). */

                if ((freeArcLabel == null) || (freeArcLabel == "") ||
                                (freeArc.localLabels.Contains(freeArcLabel)))
                {
                    if ((neighborNodeLabel == null) || (neighborNodeLabel == "") ||
                        ((neighborNode != null) && (neighborNode.localLabels.Contains(neighborNodeLabel))))
                    {
                        if ((nodeRemoved == null) ||
                            ((freeArc.To == nodeRemoved) && (freeEndIdentifier >= 0)) ||
                            ((freeArc.From == nodeRemoved) && (freeEndIdentifier <= 0)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}