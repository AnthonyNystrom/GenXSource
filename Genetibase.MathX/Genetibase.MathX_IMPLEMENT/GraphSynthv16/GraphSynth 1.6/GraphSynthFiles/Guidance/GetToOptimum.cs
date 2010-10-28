using System;
using System.Collections.Generic;
using System.Text;
using GraphSynth.Representation;


namespace GraphSynth.Guidance
{
    public class DoNothingButDisplay
    {
        public void displayBiggest(candidate[] candidates)
        {
            double best = double.NegativeInfinity;
            foreach (candidate c in candidates)
                if (c.f0 > best)
                    best = c.f0;
            SearchIO.miscObject = best;
        }
    }
}
