using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GraphSynth.Representation
{
    public class option
    {
        /* these are presented in the choice for which rule to apply.
         * Option contains references to the location where the rule is
         * applicable, the rule itself, along with its number in the ruleSet
         * and the ruleSet's number when there are multiple ruleSets. */
        public int ruleSetIndex;
        public int ruleNumber;
        [XmlIgnore]
        public grammarRule rule;
        public designGraph location;

        public void apply(designGraph host, double[] parameters)
        {
            rule.apply(location, host, parameters);
        }
    }
}
