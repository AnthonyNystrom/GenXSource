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
        #endregion


        #region Parametric Application Rules
        /* Parametric application rules receive as input:
         * 1. the location designGraph indicating the nodes&arcs of host that match with L (Lmapping)
         * 2. the entire host graph (host)
         * 3. the location of the nodes in the host that R matches to (Rmapping).
         * 4. the parameters chosen by an agent for instantiating elements of Rmapping or host (parameters). */

        /* This is APPLY for the rule entitled: swirlRule1  */
        public designGraph slopeOfNewEdge(designGraph Lmapping, designGraph host, designGraph Rmapping, double[] parameters)
        {
            edge oldEdge = (edge)Rmapping.arcs[0];
            vertex oldVertex0 = (vertex)Rmapping.nodes[0];
            vertex oldVertex1 = (vertex)Rmapping.nodes[1];
            double newAngle = System.Math.Atan2((oldVertex1.y - oldVertex0.y), (oldVertex1.x - oldVertex0.x)) - 0.4;
            double newLength = 1.05 * oldEdge.length;
            vertex newVertex = (vertex)Rmapping.nodes[2];
            newVertex.x = oldVertex1.x + newLength * System.Math.Cos(newAngle);
            newVertex.screenX = (float)newVertex.x;
            newVertex.y = oldVertex1.y + newLength * System.Math.Sin(newAngle);
            newVertex.screenY = (float)newVertex.y;

            return host;
        }
        #endregion


    }
}
