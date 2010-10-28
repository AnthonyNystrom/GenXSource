using System;
using System.Collections.Generic;
using GraphSynth;
using GraphSynth.Representation;

namespace GraphSynth.ParamRules
{
    public partial class ParamRules
    {
        /* here are parametric rules written as part of the ruleSet.
        * these are compiled at runtime into a .dll indicated in the
        * App.config file. */
        #region Parametric Recognition Rules
        /* Parametric recognition rules receive as input:
* 1. the left hand side of the rule (L)
* 2. the entire host graph (host)
* 3. the location of the nodes in the host that L matches to (locatedNodes).
* 4. the location of the arcs in the host that L matches to (locatedArcs). */

        /* This is RECOGNIZE for the rule entitled: seedBurst5rule1 */
        public double divisibleBy5(designGraph L, designGraph host, List<node> locatedNodes, List<arc> locatedArcs)
        {
            return System.Math.Abs(locatedNodes[0].localVariables[0] % 5);
        }

        /* This is RECOGNIZE for the rule entitled: seedBurst3rule2 */
        public double divisibleBy3(designGraph L, designGraph host, List<node> locatedNodes, List<arc> locatedArcs)
        {
            return System.Math.Abs(locatedNodes[0].localVariables[0] % 3);
        }

        /* This is RECOGNIZE for the rule entitled: seedBurst2rule3 */
        public double divisibleBy2(designGraph L, designGraph host, List<node> locatedNodes, List<arc> locatedArcs)
        {
            return System.Math.Abs(locatedNodes[0].localVariables[0] % 2);
        }
        /* This is RECOGNIZE for the rule entitled: seedBurst1rule4 */
        public double isALeafNode(designGraph L, designGraph host, List<node> locatedNodes, List<arc> locatedArcs)
        {
            if ((locatedNodes[0].arcs.Count <= 1) && (locatedNodes[0].localVariables[0] > 1.0)) return 0.0;
            else return 1.0;
        }
        #endregion


        #region Parametric Application Rules
        /* Parametric application rules receive as input:
         * 1. the location designGraph indicating the nodes&arcs of host that match with L (Lmapping)
         * 2. the entire host graph (host)
         * 3. the location of the nodes in the host that R matches to (Rmapping).
         * 4. the parameters chosen by an agent for instantiating elements of Rmapping or host (parameters). */

        /* This is APPLY for the rule entitled: seedBurst5rule1 */
        public designGraph distributeTo5NewRoots(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
        {
            for (int i = 1; i <= 5; i++)
            {
                Rmapping.nodes[i].localVariables.Add(Rmapping.nodes[0].localVariables[0] / 5);
                //Rmapping.nodes[i].screenY = 0.0f;
                //Rmapping.nodes[i].screenX = 0.0f;
            }

            Rmapping.nodes[0].localVariables[0] = 0.1;
            return host;
        }

        /* This is APPLY for the rule entitled: seedBurst3rule2 */
        public designGraph distributeTo3NewRoots(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
        {
            for (int i = 1; i <= 3; i++)
            {
                Rmapping.nodes[i].localVariables.Add(Rmapping.nodes[0].localVariables[0] / 3);
                //Rmapping.nodes[i].screenY = 0.0f;
                //Rmapping.nodes[i].screenX = 0.0f;
            }

            Rmapping.nodes[0].localVariables[0] = 0.1;
            return host;
        }

        /* This is APPLY for the rule entitled: seedBurst2rule3 */
        public designGraph distributeTo2NewRoots(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
        {
            for (int i = 1; i <= 2; i++)
            {
                Rmapping.nodes[i].localVariables.Add(Rmapping.nodes[0].localVariables[0] / 2);
                //Rmapping.nodes[i].screenY = 0.0f;
                //Rmapping.nodes[i].screenX = 0.0f;
            }

            Rmapping.nodes[0].localVariables[0] = 0.1;
            return host;
        }

        /* This is APPLY for the rule entitled: seedBurst1rule4 */
        public designGraph distributeTo1NewRoot(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
        {
            Rmapping.nodes[1].localVariables.Add(Rmapping.nodes[0].localVariables[0] - 1);
            //Rmapping.nodes[1].screenY = 0.0f;
            //Rmapping.nodes[1].screenX = 0.0f;
            Rmapping.nodes[0].localVariables[0] = 0.1;
            return host;
        }
        #endregion


    }
}
