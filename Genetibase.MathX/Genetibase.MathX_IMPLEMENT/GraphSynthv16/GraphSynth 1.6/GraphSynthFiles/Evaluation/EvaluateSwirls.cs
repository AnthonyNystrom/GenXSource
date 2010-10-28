using System;
using System.Collections.Generic;
using System.Text;
using GraphSynth.Representation;

namespace GraphSynth.Evaluation
{
    public class EvaluateSwirls
    {
        public void assignSwirlSizeToAllCandidates(candidate[] candidates)
        {
            Random rnd = new Random();
            foreach (candidate c in candidates)
                c.f0 = calcSwirlSize(c);
        }
        private double calcSwirlSize(candidate c)
        {
            double minX = double.PositiveInfinity;
            double minY = double.PositiveInfinity;
            double maxX = double.NegativeInfinity;
            double maxY = double.NegativeInfinity;
            foreach (vertex v in c.graph.nodes)
            {
                if (v.x < minX) minX = v.x;
                if (v.x > maxX) maxX = v.x;
                if (v.y < minY) minY = v.y;
                if (v.y > maxY) maxY = v.y;
            }
            return ((maxX - minX) * (maxY - minY));
        }
    }
}
