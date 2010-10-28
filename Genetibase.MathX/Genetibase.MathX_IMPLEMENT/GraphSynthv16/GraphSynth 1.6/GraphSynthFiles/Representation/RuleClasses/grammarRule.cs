using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;


namespace GraphSynth.Representation
{
    /* Get ready, this file has a lot of theory in it. All the recognize and apply functions are found
     * here. There is a recognize function in ruleSet, and an apply in option but those are simply 
     * macros for the functions found here within grammarRule. */
    public partial class grammarRule
    {
        public string name;
        #region Subset or Equal Booleans and Matching Functions
        /* these are special booleans used by recognize. In many cases, the L will be a subset of the
         * host in all respects (a proper subset - a subgraph which is anything but equal). However, 
         * there may be times when the user wants to restrict the number of recognized locations, by 
         * looking for an EQUAL conditions as opposed to simply a SUBSET. The following booleans 
         * capture the possible ways in which the subgraph may/may not be a subset (boolean set to false)
         * or is equal (in this respective quality) to the host (boolean set to true). */
        public Boolean spanning;
        /* if true then all nodes in L must be in host and vice-verse - NOT a proper subset
        /* if false then proper subset. */
        public Boolean induced;
        /* if true then all arcs between the nodes in L must be in host and no more 
         * - again not a proper SUBSET
         * if false then proper subset.
         * this following function is the only to use induced and is only called early
         * in the Location Found case, and only then when induced is true. As its name implies it 
         * simply checks to see if there are any arcs in the host between the nodes recognized. */
        private Boolean noOtherArcsInHost(designGraph host, List<node> locatedNodes, List<arc> locatedArcs)
        {
            foreach (arc a in host.arcs)
            {
                if (!locatedArcs.Contains(a) && locatedNodes.Contains(a.From) && locatedNodes.Contains(a.To))
                    return false;
            }
            return true;
        }

        public Boolean containsAllGlobalLabels;
        /* just like the above, but applied to a set instead of a graph - the set of global labels.
         * if true then use equal
         * if false then use subset. 
         * this following function is the only to use containsAllGlobalLabels and is only called early
         * in the main recognize method below. */
        private Boolean graphLabelsMatch(List<string> hostLabels)
        {
            foreach (string label in L.globalLabels)
                if (!hostLabels.Contains(label)) return false;
            if (containsAllGlobalLabels)
                /* if 'containsAllLocalLabels' is true than your are checking for equality.
                 * but how to do that? basically if a is a subset of b AND b is a subset of a. */
                foreach (string label in hostLabels)
                    if (!L.globalLabels.Contains(label)) return false;
            return true;
        }
        #endregion

        public List<embeddingRule> embeddingRules = new List<embeddingRule>();
        /* after double pushout runs its course, we'd like to account for dangling arcs that were victims
         * of the G - (L - R) pushout. these rules are defined here following the edNCE approach (edge-directed 
         * Neighborhood Controlled Embedding) and discussed in more detail below. */

        public List<string> recognizeFunctions = new List<string>();
        [XmlIgnore]
        public List<MethodInfo> recognizeFuncs = new List<MethodInfo>();
        public List<string> applyFunctions = new List<string>();
        [XmlIgnore]
        public List<MethodInfo> applyFuncs = new List<MethodInfo>();
        [XmlIgnore]
        public object DLLofFunctions = null;
        /* any mathematical operations are fair game for the recognize and apply local variables.
         * At the end of a graph recognition, we check all the recognize functions, if any yield a 
         * positive number than the rule is infeasible. This is done case1LocationFound */


        [XmlIgnore]
        public List<designGraph> locations = new List<designGraph>();
        /* this is where we store the subgraphs or locations of where the
         * rule can be applied. It's global to a particular L but it is invoked
         * only at the very bottom of the recursion tree - see the end of
         * recognizeRecursion(). */
        public designGraph L;
        /* this is the left-hand-side of the rule. It is a graph that is to be recognized in 
         * the host graph. 
         * note that many fields below are also unnecesarily set to PUBLIC. 
         * please fix when above XML reader is complete. */
        public designGraph R;
        /* this is the right-hand-side of the rule. It is a graph that is to be inserted (glued)
         * into the host graph. */

        #region Recognize Methods
        /* here is the big one! Although it looks compact, a lot of time can be spent in 
         * the recursion that it invokes. Before we get to that, we wanna make sure that 
         * our time there is well spent. As a result, we try to rule out whether the rule
         * can even be applied at first -- hence the series of nested if-thens. If you don't
         * meet the first, leave now! likewise for the second. The third is a little trickier.
         * if there are no nodes or arcs in this rule, then it has already proven to be valid
         * by the global labels - thus return a single location labeled "anywhere".
         * if there are no nodes in the rule, then jump to the special function recognizeInitialArcInHost
         * finally, if the node with the highest degree is higher than the highest degree
         * of the host, then no need to recurse any further. Else, get into recognizeInitialNodeInHost,  
         * which further calls recognizeRecursion. */
        public List<designGraph> recognize(designGraph host)
        {
            locations.Clear();
            if (graphLabelsMatch(host.globalLabels))
                if ((!spanning) || ((spanning) && (L.nodes.Count == host.nodes.Count)))
                    if ((L.nodes[0] == null) && (L.arcs[0] == null))
                        locations.Add(new designGraph());
                    else if (L.nodes[0] == null)
                        recognizeInitialArcInHost(host);
                    else if (L.maxDegree <= host.maxDegree)
                        recognizeInitialNodeInHost(host);
            return locations;
        }

        private void recognizeInitialNodeInHost(designGraph host)
        {
            ruleNode startingLNode = (ruleNode)L.nodes[0];
            /* if the first node of L (L.nodes[0]) is not in the host, then we can stop
             * as a rule of thumb, the creator of the grammar rules should always put the
             * most restrictive node FIRST in L, this will allow for a more efficient recognize routine.*/
            foreach (node currentHostNode in host.nodes)
            {   /* see if it matches with each node in host. */
                if (startingLNode.matchWith(currentHostNode))
                {
                    List<node> locatedNodes = new List<node>(L.nodes.Count);
                    List<arc> locatedArcs = new List<arc>(L.arcs.Count);
                    /* we will be storing potential locations in these two lists.
                     * it will be necessary to keep making copies of these lists as
                     * we discover new potential subgraphs. So, I wanna make sure these
                     * are as light as possible. Instead of making a whole designGraph
                     * instance, we will just move around these 2 lists. The lists will
                     * point to the actual elements in host, but will correspond in size 
                     * and position to those in 'this' L. Since we know the size ahead
                     * of time, we can preset the 'capacity' of the list. however this doesn't
                     * mean the lists are actually that size, so we need to explicitly
                     * initialize the lists. */
                    for (int i = 0; i != L.nodes.Count; i++)
                        locatedNodes.Add(null);
                    for (int i = 0; i != L.arcs.Count; i++)
                        locatedArcs.Add(null);

                    locatedNodes[0] = currentHostNode;
                    /* we've made one match so set that up before invoking the recursion. */
                    recognizeRecursion(host, locatedNodes, locatedArcs, startingLNode, currentHostNode);
                }
            }
        }

        private void recognizeInitialArcInHost(designGraph host)
        {
            ruleArc startingLArc = (ruleArc)L.arcs[0];
            foreach (arc currentHostArc in host.arcs)
            {
                if (startingLArc.matchWith(currentHostArc))
                {
                    List<node> locatedNodes = new List<node>(L.nodes.Count);
                    List<arc> locatedArcs = new List<arc>(L.arcs.Count);
                    for (int i = 0; i != L.nodes.Count; i++)
                        locatedNodes.Add(null);
                    for (int i = 0; i != L.arcs.Count; i++)
                        locatedArcs.Add(null);
                    locatedArcs[0] = currentHostArc;
                    recognizeRecursion(host, locatedNodes, locatedArcs, null, null);
                }
            }
        }

        private void recognizeRecursion(designGraph host, List<node> locatedNodes, List<arc> locatedArcs,
            ruleNode fromLNode, node fromHostNode)
        {   /* Here is the main recursive function. Based on the current conditions within the recursion 
             * one of four cases maybe invoked.
             * 1. (case1LocationFound) All nodes and arcs within locatedNodes and locatedArcs have been 
             *    filled with pointers to nodes and arcs in the host. If this is the case, then we can add  
             *    the location. however, you will need to check the enigmatic INDUCED condition.
             */
            if (!locatedNodes.Contains(null) && !locatedArcs.Contains(null))
                case1LocationFound(host, locatedNodes, locatedArcs);
            else
            {   /* the last thing the recursion did was find a new node to start from.
                 * see if there are any valid arcs on the L that still need to be matched with
                 * the host. Here, currentLArcIndex is used instead of the actual reference
                 * to the L arc. Why? Because, the index is useful both to the L and to locatedArcs
                 * which lists arcs in the same way as they appear in the L. */
                int currentLArcIndex = -1;
                Boolean traverseForward = false;
                /* this odd little boolean is used to indicate whether or not we are following the
                 * arc in the proper direction regardless of the direction. We want to be able to follow 
                 * arcs backwards for recognition sake, so this is only useful in the eventual matchWith
                 * method if direction is important. */
                for (int i = 0; i != L.arcs.Count; i++)
                {   /* this for loop seeks a L node leaving our fromLNode. If there is more than one arc, than
                     * the loop may re-write currentLArcIndex and traverseForward. That's okay. Because we only
                     * want one at this point. The recursion will eventually come around to any others that may
                     * be skipped over here. */
                    if ((L.arcs[i].From == fromLNode) && (locatedArcs[i] == null))
                    {
                        currentLArcIndex = i;
                        traverseForward = true;
                    }
                    else if ((L.arcs[i].To == fromLNode) && (locatedArcs[i] == null))
                    {
                        currentLArcIndex = i;
                        traverseForward = false;
                    }
                }
                if (currentLArcIndex == -1)
                {   /* 2. (case2FindNewFromNode) if you get here, then it means that then were no more arcs
                     *    leaving the last node. Unfortunately, since Case 1 was not met, there are still 
                     *    openings in the locations - either arcs and/or nodes. */
                    case2FindNewFromNode(host, locatedNodes, locatedArcs, fromLNode);
                }
                else
                {
                    /* so, currentLArcIndex now, points to a LArc that has yet to be recognized. What we do from
                     * this point depends on whether that LArc points to an L node we have yet to recognize, an L
                     * node we have recognized, or null. */
                    ruleNode nextLNode = (ruleNode)L.arcs[currentLArcIndex].otherNode(fromLNode);
                    if (nextLNode == null)
                    {   /* 3. (case3DanglingNodes) If nextLNode is null then we need to simply find a match for 
                         * the arc indicated by currentLArcIndex. Similar to case2, this function will need to 
                         * find a new starting point in matching the graphs. */
                        case3DanglingLNode
                            (host, locatedNodes, locatedArcs, fromLNode, fromHostNode, currentLArcIndex, traverseForward);
                    }
                    else if (locatedNodes[L.nodes.IndexOf(nextLNode)] != null)
                    {   /* 4. (case4ConnectingBackToPrevRecNode) So, a proper arc was found leaving the 
                         *    last L node. Problem is, it points back to a node that we've already located.
                         *    That means that we also already found what host node it connects to. */
                        case4ConnectingBackToPrevRecNode
                            (host, locatedNodes, locatedArcs, nextLNode, fromHostNode, currentLArcIndex, traverseForward);
                    }
                    else
                    {   /* 5. (case5FindingNewNodes) Okay, so nothing strange here. You are following an arc
                         *    that is leading to a yet undiscovered node. Good luck! */
                        case5FindingNewNodes
                            (host, locatedNodes, locatedArcs, nextLNode, fromHostNode, currentLArcIndex, traverseForward);
                    }
                }
            }
        }

        private void case1LocationFound(designGraph host, List<node> locatedNodes, List<arc> locatedArcs)
        {
            Boolean paramFunctionsViolated = false;
            /* a complete subgraph has been found. However, there is two more conditions to check. 
             * The induced boolean indicates that if there are any arcs in the host between the 
             * nodes of the subgraph that are not in L then this is not a valid location. After this we
             * check the variables for a violation. */
            if (!induced || (induced && noOtherArcsInHost(host, locatedNodes, locatedArcs)))
            {
                object[] recognizeArguments = new object[4];
                recognizeArguments[0] = L;
                recognizeArguments[1] = host;
                recognizeArguments[2] = locatedNodes;
                recognizeArguments[3] = locatedArcs;

                foreach (MethodInfo recognizeFunction in recognizeFuncs)
                {
                    if ((double)recognizeFunction.Invoke(DLLofFunctions, recognizeArguments)
                        > 0)
                    {
                        paramFunctionsViolated = true;
                        break;
                    }
                }
                if (!paramFunctionsViolated)
                    locations.Add(new designGraph(locatedNodes, locatedArcs));
            }
        }

        private void case2FindNewFromNode(designGraph host, List<node> locatedNodes, List<arc> locatedArcs,
            ruleNode fromLNode)
        {
            int nextLNodeIndex = L.nodes.IndexOf(fromLNode) + 1;
            if (nextLNodeIndex == L.nodes.Count)
                nextLNodeIndex = 0;          /* these 3 prev.lines simply go to the next node in L 
                                              * - if you're at the end then wraparound to 0. */
            ruleNode nextLNode = (ruleNode)L.nodes[nextLNodeIndex];
            if (locatedNodes[nextLNodeIndex] == null)
            {   /* this acts like a mini-recognizeInitialNodeInHost function, we are forced to jump to a new starting
                 * point in L - careful, though, that we don't check it with a node that has already been included
                 * as part of the location. */
                foreach (node currentHostNode in host.nodes)
                {
                    if (!locatedNodes.Contains(currentHostNode) && (nextLNode.matchWith(currentHostNode)))
                    {
                        List<node> newlocatedNodes = new List<node>(locatedNodes); /* copy the locatedNodes to a new list.
                                                                                    * just in case the above foreach statement
                                                                                    * find several matches for our new 
                                                                                    * starting node - we wouldnt want to alter
                                                                                    * locatedNodes to affect that but rather
                                                                                    * merely to re-invoke the recusion.*/
                        newlocatedNodes[nextLNodeIndex] = currentHostNode;
                        recognizeRecursion(host, newlocatedNodes, locatedArcs, nextLNode, currentHostNode);
                    }
                }
            }
            /* so the next L node has already been recognized. Well, then we can restart the recursion as if we are
             * coming from this node. It's possible that recognizeRecursion will just throw you back into this function
             * but that's okay. we just advance to the next node and look for new 'openings'. */
            else recognizeRecursion(host, locatedNodes, locatedArcs, nextLNode, locatedNodes[nextLNodeIndex]);
        }

        private void case3DanglingLNode(designGraph host, List<node> locatedNodes, List<arc> locatedArcs,
            ruleNode fromLNode, node fromHostNode, int currentLArcIndex, Boolean traverseForward)
        {
            ruleArc currentLArc = (ruleArc)L.arcs[currentLArcIndex];           /* first we must match the arc to a possible arc
                                                                                * leaving the fromHostNode .*/
            node nextHostNode;
            List<arc> neighborHostArcs = host.arcs.FindAll(delegate(arc a)     /* there maybe several possible arcs*/
            {                                                                  /* that match with currentLArc, so we*/
                return (!locatedArcs.Contains(a)                               /* make a list called neighborHostArcs*/
                    && currentLArc.matchWith(a, fromHostNode, traverseForward));
            });
            if (neighborHostArcs.Count > 0)          /* if there are no recognized arcs we just leave. */
            {
                foreach (arc HostArc in neighborHostArcs)
                {   /* for each arc that was recognized, we now need to check that the destination node matches. */
                    nextHostNode = HostArc.otherNode(fromHostNode);
                    if (!currentLArc.nullMeansNull || (nextHostNode == null))
                    {   /* if nullMeansNull is false than ANY host node is fine even if its also null. If nullMeansNull
                         * is true, however, than we need to make sure fromHostNode is also null. */
                        List<arc> newlocatedArcs = new List<arc>(locatedArcs);
                        newlocatedArcs[currentLArcIndex] = HostArc;
                        /* re-invoking the recursion is "tough" from this point. since we just hit a dead end in L.
                         * the best thing to do is just use the very same fromLnode and fromHostNode that were 
                         * used in the previous recognizeRecursion. */
                        recognizeRecursion(host, locatedNodes, newlocatedArcs, fromLNode, fromHostNode);
                    }
                }
            }
        }

        private void case4ConnectingBackToPrevRecNode(designGraph host, List<node> locatedNodes,
             List<arc> locatedArcs, ruleNode nextLNode, node fromHostNode, int currentLArcIndex, Boolean traverseForward)
        {
            ruleArc currentLArc = (ruleArc)L.arcs[currentLArcIndex];           /* first we must match the arc to a possible arc
                                                                   * leaving the fromHostNode .*/
            node nextHostNode = locatedNodes[L.nodes.IndexOf(nextLNode)];
            List<arc> neighborHostArcs = host.arcs.FindAll(delegate(arc a)     /* there maybe several possible arcs*/
            {                                                                  /* that match with currentLArc, so we*/
                return (!locatedArcs.Contains(a)                               /* make a list called neighborHostArcs*/
                    && currentLArc.matchWith(a, fromHostNode, nextHostNode, traverseForward));
            });
            if (neighborHostArcs.Count > 0)          /* if there are no recognized arcs we just leave. */
            {
                foreach (arc HostArc in neighborHostArcs)
                {
                    List<node> newlocatedNodes = new List<node>(locatedNodes);
                    newlocatedNodes[L.nodes.FindIndex(delegate(node a)
                                                    { return (a == nextLNode); })] = nextHostNode;
                    List<arc> newlocatedArcs = new List<arc>(locatedArcs);
                    newlocatedArcs[currentLArcIndex] = HostArc;
                    recognizeRecursion(host, newlocatedNodes, newlocatedArcs, nextLNode, nextHostNode);
                }
            }
        }

        private void case5FindingNewNodes(designGraph host, List<node> locatedNodes, List<arc> locatedArcs,
            ruleNode nextLNode, node fromHostNode, int currentLArcIndex, Boolean traverseForward)
        {   /* this function starts very similar to Case 4. It is, however, more comlex since we need to match
             * the next node in L to a node in the host. The function begin the same as above by gathering the
             * potential arcs leaving the host and checking them for compatibility. */
            ruleArc currentLArc = (ruleArc)L.arcs[currentLArcIndex];
            node nextHostNode;
            List<arc> neighborHostArcs = host.arcs.FindAll(delegate(arc a)
            {
                return (!locatedArcs.Contains(a)
                    && currentLArc.matchWith(a, fromHostNode, traverseForward));
            });
            if (neighborHostArcs.Count > 0)
            {
                foreach (arc HostArc in neighborHostArcs)
                {   /* for each arc that was recognized, we now need to check that the destination node matches. */
                    nextHostNode = HostArc.otherNode(fromHostNode);
                    if (nextLNode.matchWith(nextHostNode))
                    {   /* if the nodes match than we can update locations and re-invoke the recursion. It is important
                         * to copy the locatedNodes to a new list, just in case the above foreach statement finds
                         * several matches for our new new L node.*/
                        List<node> newlocatedNodes = new List<node>(locatedNodes);
                        newlocatedNodes[L.nodes.FindIndex(delegate(node a)
                                                            { return (a == nextLNode); })] = nextHostNode;
                        List<arc> newlocatedArcs = new List<arc>(locatedArcs);
                        newlocatedArcs[currentLArcIndex] = HostArc;
                        recognizeRecursion(host, newlocatedNodes, newlocatedArcs, nextLNode, nextHostNode);
                    }
                }
            }
        }

        #endregion

        #region Apply Methods
        public void apply(designGraph Lmapping, designGraph host, double[] parameters)
        {
            /* Update the global labels.*/
            foreach (string a in L.globalLabels)
                if (!R.globalLabels.Contains(a))
                    host.globalLabels.Remove(a);          /* removing the labels in L but not in R...*/
            foreach (string a in R.globalLabels)
                if (!L.globalLabels.Contains(a))
                    host.globalLabels.Add(a.ToString());          /*...and adding the label in R but not in L.*/
            foreach (double a in L.globalVariables)         /* do the same now, for the variables. */
                if (!R.globalVariables.Contains(a))
                    host.globalVariables.Remove(a);          /* removing the labels in L but not in R...*/
            foreach (double a in R.globalVariables)
                if (!L.globalVariables.Contains(a))
                    host.globalVariables.Add(a);          /*...and adding the label in R but not in L.*/

            /* First set up the Rmapping, which is a list of nodes within the host
             * that corresponds in length and position to the nodes in R, just as 
             * Lmapping contains lists of nodes and arcs in the order they are 
             * referred to in L. */
            designGraph Rmapping = new designGraph();
            foreach (node a in R.nodes)     /* we do not know what these will point to yet, so just */
                Rmapping.nodes.Add(null);   /* make it of proper length at this point. */
            foreach (arc a in R.arcs)       /* DEBUG HINT: you should check Rmapping at the end of */
                Rmapping.arcs.Add(null);     /* the function - it should contain no nulls. */


            removeLdiffKfromHost(Lmapping, host);
            addRdiffKtoD(Lmapping, host, Rmapping);
            /* these two lines correspond to the two "pushouts" of the double pushout algorithm. 
             *     L <--- K ---> R     this is from freeArc embedding (aka edNCE)
             *     |      |      |        |      this is from the parametric update
             *     |      |      |        |       |
             *   host <-- D ---> H1 ---> H2 ---> H3
             * The first step is to create D by removing the part of L not found in K (the commonality).
             * Second, we add the elements of R not found in K to D to create the updated host, H. Note, 
             * that in order to do this, we must know what subgraph of the host we are manipulating - this
             * is the location mapping found by the recognize function. */

            freeArcEmbedding(Lmapping, host, Rmapping);
            /* however, there may still be a need to embed the graph with other arcs left dangling,
             * as in the "edge directed Node Controlled Embedding approach", which considers the neighbor-
             * hood of nodes and arcs of the recognized Lmapping. */
            updateParameters(Lmapping, host, Rmapping, parameters);
        }

        private void removeLdiffKfromHost(designGraph Lmapping, designGraph host)
        {
            /* foreach node in L - see if it "is" also in R - if it is in R than it "is" part of the 
             * commonality subgraph K, and thus should not be deleted as it is part of the connectivity
             * information for applying the rule. Note that what we mean by "is" is that there is a
             * node with the same name. The name tag in a node is not superficial - it contains
             * useful connectivity information. We use it as a stand in for referencing the same object
             * this is different than the local lables which are used for recognition and the storage
             * any important design information. */
            for (int i = 0; i != L.nodes.Count; i++)
            {
                if (!R.nodes.Exists(delegate(node b)
                    { return (b.name == L.nodes[i].name); }))
                {
                    /* if a node with the same name does not exist in R, then it is safe to remove it.
                     * The removeNode should is invoked with the "false false" switches of this function. 
                     * This causes the arcs to be unaffected by the deletion of a connecting node. Why 
                     * do this? It is important in the edNCE approach that is appended to the DPO approach
                     * (see the function freeArcEmbedding) in connecting up a new R to the elements of L 
                     * a node was connected to. */
                    host.removeNode(Lmapping.nodes[i], false, false);
                }
            }
            for (int i = 0; i != L.arcs.Count; i++)
            {
                if (!R.arcs.Exists(delegate(arc b)
                    { return (b.name == L.arcs[i].name); }))
                { /* the removal of arcs happens in a similar way. */
                    host.removeArc(Lmapping.arcs[i]);
                }
            }
        }

        private void addRdiffKtoD(designGraph Lmapping, designGraph D, designGraph Rmapping)
        {
            /* in this adding and gluing function, we are careful to distinguish
             * the Lmapping or recognized subgraph of L in the host - heretofore
             * known as Lmapping - from the mapping of new nodes and arcs of the
             * graph, which we call Rmapping. This is a complex function that goes
             * through 4 key steps:
             * 1. add the new nodes that are in R but not in L.
             * 2. update the remaining nodes common to L&R (aka K nodes) that might
             *    have had some label changes.
             * 3. add the new arcs that are in R but not in L. These may connect to
             *    either the newly connected nodes from step 1 or from the updated nodes
             *    of step 2.
             * 4. update the arcs common to L&R (aka K arcs) which might now be connected
             *    to new nodes created in step 1 (they are already connected to 
             *    nodes in K). Also make sure to update their labels just as K nodes were
             *    updated in step 2.

            /* here are some placeholders used in this bookeeping. Many are used multiple times
             * so we might as well declare them just once at the start. */
            int index1, index2;
            node from, to, KNode;
            arc KArc;

            for (int i = 0; i != R.nodes.Count; i++)
            {
                ruleNode rNode = (ruleNode)R.nodes[i];
                #region Step 1. add new nodes to D
                if (!L.nodes.Exists(delegate(node b)
                    { return (b.name == rNode.name); }))
                {
                    D.addNode(rNode.nodeType);         /* create a new node. */
                    Rmapping.nodes[i] = D.nodes[D.lastNode];          /* make sure it's referenced in Rmapping. */
                    /* labels cannot be set equal, since that merely sets the reference of this list
                     * to the same value. So, we need to make a complete copy. */
                    rNode.copy(D.nodes[D.lastNode]);
                    /* give that new node a name and labels to match with the R. */
                }
                #endregion
                #region Step 2. update K nodes
                else
                {
                    /* else, we may need to modify or update the node. In the pure graph
                     * grammar sense this is merely changing the local labels. In a way, 
                     * this is a like a set grammar. We need to find the labels in L that 
                     * are no longer in R and delete them, and we need to add the new labels
                     * that are in R but not already in L. The ones common to both are left
                     * alone. */
                    index1 = L.nodes.FindIndex(delegate(node b)
                        { return (rNode.name == b.name); });          /* find index of the common node in L...*/
                    KNode = Lmapping.nodes[index1];          /*...and then set Knode to the actual node in D.*/
                    Rmapping.nodes[i] = KNode;               /*also, make sure that the Rmapping is to this same node.*/
                    foreach (string a in L.nodes[index1].localLabels)
                        if (!rNode.localLabels.Contains(a))
                            KNode.localLabels.Remove(a);          /* removing the labels in L but not in R...*/
                    foreach (string a in rNode.localLabels)
                        if (!L.nodes[index1].localLabels.Contains(a))
                            KNode.localLabels.Add(a.ToString());          /*...and adding the label in R but not in L.*/
                    foreach (double a in L.nodes[index1].localVariables)         /* do the same now, for the variables. */
                        if (!rNode.localVariables.Contains(a))
                            KNode.localVariables.Remove(a);          /* removing the labels in L but not in R...*/
                    foreach (double a in rNode.localVariables)
                        if (!L.nodes[index1].localVariables.Contains(a))
                            KNode.localVariables.Add(a);          /*...and adding the label in R but not in L.*/
                    KNode.shapekey = rNode.shapekey;
                }
            }
                #endregion

            /* now moving onto the arcs (a little more challenging actually). */
            for (int i = 0; i != R.arcs.Count; i++)
            {
                ruleArc rArc = (ruleArc)R.arcs[i];
                #region Step 3. add new arcs to D
                if (!L.arcs.Exists(delegate(arc b)
                    { return (b.name == rArc.name); }))
                {
                    #region setting up where arc comes from
                    if (rArc.From == null)
                        from = null;
                    else if (L.nodes.Exists(delegate(node b)
                         { return (b.name == rArc.From.name); }))
                    /* if the arc is coming from a node that is in K, then it must've been
                     * part of the location (or Lmapping) that was originally recognized.*/
                    {
                        index1 = L.nodes.FindIndex(delegate(node b)
                        {
                            return (rArc.From.name == b.name);
                        });  /* therefore we need to find the position/index of that node in L. */

                        from = Lmapping.nodes[index1];
                        /* and that index1 will correspond to its image in Lmapping. Following,
                         * the Lmapping reference, we get to the proper node reference in D. */
                    }
                    else
                    /* if not in K then the arc connects to one of the new nodes that were 
                     * created at the beginning of this function (see step 1) and is now
                     * one of the references in Rmapping. */
                    {
                        index1 = R.nodes.FindIndex(delegate(node b)
                        {
                            return (rArc.From.name == b.name);
                        });
                        from = Rmapping.nodes[index1];
                    }
                    #endregion
                    #region setting up where arc goes to
                    /* this code is the same of "setting up where arc comes from - except here
                     * we do the same for the to connection of the arc. */
                    if (rArc.To == null)
                        to = null;
                    else if (L.nodes.Exists(delegate(node b) { return (b.name == rArc.To.name); }))
                    {
                        index1 = L.nodes.FindIndex(delegate(node b)
                        {
                            return (rArc.To.name == b.name);
                        });
                        to = Lmapping.nodes[index1];
                    }
                    else
                    {
                        index1 = R.nodes.FindIndex(delegate(node b)
                        {
                            return (rArc.To.name == b.name);
                        });
                        to = Rmapping.nodes[index1];
                    }
                    #endregion

                    D.addArc(rArc.name, rArc.arcType, from, to);
                    Rmapping.arcs[i] = D.arcs[D.lastArc];
                    rArc.copy(D.arcs[D.lastArc]);
                }
                #endregion
                #region Step 4. update K arcs
                else
                {
                    index2 = L.arcs.FindIndex(delegate(arc b)
                        { return (rArc.name == b.name); });
                    /* first find the position of the same arc in L. */
                    ruleArc currentLArc = (ruleArc)L.arcs[index2];
                    KArc = Lmapping.arcs[index2];    /* then find the actual arc in D that is to be changed.*/
                    /* one very subtle thing just happend here! (07/06/06) if the direction is reversed, then
                     * you might mess-up this Karc. We need to establish a boolean so that references 
                     * incorrectly altered. */
                    Boolean KArcIsReversed = false;
                    if ((Lmapping.nodes.IndexOf(KArc.From) != L.nodes.IndexOf(currentLArc.From)) &&
                        (Lmapping.nodes.IndexOf(KArc.To) != L.nodes.IndexOf(currentLArc.To)))
                        KArcIsReversed = true;

                    Rmapping.arcs[i] = KArc;
                    /*similar to Step 3., we first find how to update the from and to. */
                    if ((currentLArc.From != null) && (rArc.From == null))
                    {
                        /* this is a rare case in which you actually want to break an arc from its attached 
                         * node. If the corresponding L arc is not null only! if it is null then it may be 
                         * actually connected to something in the host, and we are in no place to remove it. */
                        if (KArcIsReversed) KArc.To = null;
                        else KArc.From = null;
                    }
                    else if (rArc.From != null)
                    {
                        index1 = R.nodes.FindIndex(delegate(node b) { return (rArc.From.name == b.name); });
                        /* find the position of node that this arc is supposed to connect to in R */
                        if (KArcIsReversed) KArc.To = Rmapping.nodes[index1];
                        else KArc.From = Rmapping.nodes[index1];
                    }
                    /* now do the same for the To connection. */
                    if ((currentLArc.To != null) && (rArc.To == null))
                    {
                        if (KArcIsReversed) KArc.From = null;
                        else KArc.To = null;
                    }
                    else if (rArc.To != null)
                    {
                        index1 = R.nodes.FindIndex(delegate(node b) { return (rArc.To.name == b.name); });
                        if (KArcIsReversed) KArc.From = Rmapping.nodes[index1];
                        else KArc.To = Rmapping.nodes[index1];
                    }
                    /* just like in Step 2, we may need to update the labels of the arc. */
                    foreach (string a in currentLArc.localLabels)
                        if (!rArc.localLabels.Contains(a))
                            KArc.localLabels.Remove(a);
                    foreach (string a in rArc.localLabels)
                        if (!currentLArc.localLabels.Contains(a))
                            KArc.localLabels.Add(a.ToString());
                    foreach (double a in currentLArc.localVariables)
                        if (!rArc.localVariables.Contains(a))
                            KArc.localVariables.Remove(a);
                    foreach (double a in rArc.localVariables)
                        if (!currentLArc.localVariables.Contains(a))
                            KArc.localVariables.Add(a);
                    KArc.curveStyle = rArc.curveStyle;
                    if (!KArc.directed || (KArc.directed && currentLArc.directionIsEqual))
                        KArc.directed = rArc.directed;
                    /* if the KArc is currently undirected or if it is and direction is equal
                     * then the directed should be inherited from R. */
                    if (!KArc.doublyDirected || (KArc.doublyDirected && currentLArc.directionIsEqual))
                        KArc.doublyDirected = rArc.doublyDirected;
                    KArc.fromConnector = rArc.fromConnector;
                    KArc.toConnector = rArc.toConnector;
                }
                #endregion
            }
        }

        private void freeArcEmbedding(designGraph Lmapping, designGraph host, designGraph Rmapping)
        {
            /* There are nodes in host which may have been left dangling due to the fact that their 
             * connected nodes were part of the L-R deletion. These now need to be either 1) connected
             * up to their new nodes, 2) their references to old nodes need to be changed to null if 
             * intentionally left dangling, or 3) the arcs are to be removed. In the function 
             * removeLdiffKfromHost we remove old nodes but leave their references intact on their 
             * connected arcs. This allows us to quickly find the list of freeArcs that are candidates 
             * for the embedding rules. Essentially, we are capturing the neighborhood within the host 
             * for the rule application, that is the arcs that are affected by the deletion of the L-R
             * subgraph. Should one check non-dangling non-neighborhood arcs? No, this would seem to 
             * cause a duplication of such an arc. Additionally, what node in host should the arc remain 
             * attached to?  There seems to be no rigor in applying these more global (non-neighborhood) 
             * changes within the literature as well for the general edNCE method. */
            sbyte freeEndIdentifier;
            node newNodeToConnect, nodeRemovedinLdiffRDeletion, toNode, fromNode;
            node  neighborNode = null;
            int numOfArcs = host.arcs.Count;

            for (int i = 0; i != numOfArcs; i++)
            {
                /* first, check to see if the arc is really a freeArc that needs updating. */
                if (embeddingRule.arcIsFree(host.arcs[i], host, out freeEndIdentifier, neighborNode))
                {
                    arc freeArc = host.arcs[i];
                    /* For each of the embedding rules, we see if it is applicable to the identified freeArc.
                     * The rule then modifies the arc by simply pointing it to the new node in R as indicated
                     * by the embedding Rule's RNodeName. NOTE: the order of the rules are important. If two
                     * rules are 'recognized' with the same freeArc only the first one will modify it, as it 
                     * will then remove it from the freeArc list. This is useful in that rules may have precedence
                     * to one another. There is an exception if the rule has allowArcDuplication set to true, 
                     * since this would simply create a copy of the arc. */
                    foreach (embeddingRule eRule in embeddingRules)
                    {
                        newNodeToConnect = eRule.findNewNodeToConnect(R, Rmapping);
                        nodeRemovedinLdiffRDeletion = eRule.findDeletedNode(L, Lmapping);

                        if (eRule.ruleIsRecognized(freeEndIdentifier, freeArc, 
                            neighborNode, nodeRemovedinLdiffRDeletion))
                        {
                            #region  set up new connection points
                            if (freeEndIdentifier >= 0)
                            {
                                if (eRule.newDirection >= 0)
                                {

                                    toNode = newNodeToConnect;
                                    fromNode = freeArc.From;
                                }
                                else
                                {
                                    toNode = freeArc.From;
                                    fromNode = newNodeToConnect;
                                }
                            }
                            else
                            {
                                if (eRule.newDirection <= 0)
                                {
                                    fromNode = newNodeToConnect;
                                    toNode = freeArc.To;
                                }
                                else
                                {
                                    fromNode = freeArc.To;
                                    toNode = newNodeToConnect;
                                }
                            }
                            #endregion

                            #region if making a copy of arc, duplicate it and all the characteristics
                            if (eRule.allowArcDuplication)
                            {
                                /* under the allowArcDuplication section, we will be making a copy of the 
                                 * freeArc. This seems a little error-prone at first, since if there is only
                                 * one rule that applies to freeArc then we will have good copy and the old
                                 * bad copy. However, at the end of this function, we go through the arcs again
                                 * and remove any arcs that still appear free. This also serves the purpose to 
                                 * delete any dangling nodes that were not recognized in any rules. */
                                host.addArc(freeArc.copy(), fromNode, toNode);
                            }
                            #endregion

                            #region else, just update the old freeArc
                            else
                            {
                                freeArc.From = fromNode;
                                freeArc.To = toNode;
                                break; /* skip to the next arc */
                                /* this is done so that no more embedding rules will be checked with this freeArc.*/
                            }
                            #endregion
                        }
                    }
                }
            }
            #region clean up (i.e. delete) any freeArcs that are still in host.arcs
            for (int i = host.arcs.Count - 1; i >= 0; i--)
            {
                /* this seems a little archaic to use this i-counter instead of foreach.
                 * the issue is that since we are removing nodes from the list as we go
                 * through it, we very well can't use foreach. The countdown allows us to 
                 * disregard problems with the deleting. */
                if ((host.arcs[i].From != null && !host.nodes.Contains(host.arcs[i].From)) ||
                    (host.arcs[i].To != null && !host.nodes.Contains(host.arcs[i].To)))
                    host.removeArc(host.arcs[i]);
            }
            #endregion
        }

        private void updateParameters(designGraph Lmapping,designGraph host,
            designGraph Rmapping, double[] parameters)
        {
            object[] applyArguments = new object[4];
            applyArguments[0] = Lmapping;
            applyArguments[1] = host;
            applyArguments[2] = Rmapping;
            applyArguments[3] = parameters;

            foreach (MethodInfo applyFunction in applyFuncs)
                applyFunction.Invoke(DLLofFunctions, applyArguments);
        }
         #endregion
    }
}

