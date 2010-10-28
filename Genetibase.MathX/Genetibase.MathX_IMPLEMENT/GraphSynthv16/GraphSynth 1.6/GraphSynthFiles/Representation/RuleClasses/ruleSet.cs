using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace GraphSynth.Representation
{
    /* As far as I can tell, this is the first time the idea of a rule set
     * has been developed to this degree. In many applications we find that 
     * different sets of rules are needed. Many of these characteristics
     * are built into our current generation process. */
    public partial class ruleSet
    {
        #region Fields
        /* an arbitrary name for the ruleSet - usually set to the filename */
        public string name = "";

        /* the rules are clearly part of the set, but these are not stored
         * in the XML, only the ruleFileNames. In ruleSetXMLIO.cs the 
         * loading of rules is accopmlished. */
        [XmlIgnore]
        public List<grammarRule> rules = new List<grammarRule>();
        public List<string> ruleFileNames = new List<string>();

        /* A ruleSet can have one rule set to the triggerRule. If there is no
         * triggerRule, then this should stay at negative one (or any negative
         * number). When the trigger rule is applied, the generation process, will
         * exit to the specified generationStep (as described below). */
        public int triggerRuleNum = -1;

        /* For a particular set of rules, we need to specify what generation should
         * do if any of five conditions occur during the recognize->choose->apply
         * cycle. The enumerator, nextGenerationSteps, listed in globalSettings.cs
         * indicates what to do. The five correspond directly to the five elements
         * of another enumerator called GenerationStatuses. These five possibilties are:
         * Normal, Choice, CycleLimit, NoRules, TriggerRule. So, following normal operation 
         * of RCA (normal), we perform the first operation stated below, nextGenerationStep[0]
         * this will likely be to LOOP and contine apply rules. Defaults for these are
         * specified in App.config. */
        [XmlIgnore]
        public nextGenerationSteps[] nextGenerationStep
            = new nextGenerationSteps[5] { nextGenerationSteps.Unspecified,
                nextGenerationSteps.Unspecified, nextGenerationSteps.Unspecified,
                nextGenerationSteps.Unspecified, nextGenerationSteps.Unspecified };

        /* Many of the ruleSets that are to be created rely on some design intent, however
         * if this enumerator is set to Automatic then generation will invoke a rule once it 
         * is recognized. */
        public choiceMethods choiceMethod = choiceMethods.Design;

        /* Often when multiple ruleSets are used, some will produce feasible candidates, 
         * while others will only produce steps towards a feasible candidate. Here, we
         * classify a particular ruleSet as one of these. */
        public candidatesAre interimCandidates = candidatesAre.Unspecified;
        public candidatesAre finalCandidates = candidatesAre.Unspecified;

        /* For multiple ruleSets, a value to store its place within the set of ruleSets
         * proves a useful indicator. */
        public int ruleSetIndex;

        /* a C# file can be custom created to correspond to special recognize or apply
         * instructions that may exist. These '.cs' are stored here.  */
        public List<string> recognizeSourceFiles = new List<string>();
        public List<string> applySourceFiles = new List<string>();
        #endregion

        #region Methods
        /* This is the recognize function called within the RCA generation. It is 
         * fairly straightforward method that basically invokes the more complex
         * recognize function for each rule within it, and returns a list of
         * options. */
        public List<option> recognize(designGraph host)
        {
            List<designGraph> locations = new List<designGraph>();
            List<option> options = new List<option>();
            if (rules.Count == 0) return options;
            for (int i = 0; i != rules.Count; i++)
            {
                locations = rules[i].recognize(host);
                if (locations.Count > 0)
                    foreach (designGraph a in locations)
                    {
                        options.Add(new option());
                        options[options.Count - 1].ruleSetIndex = this.ruleSetIndex;
                        options[options.Count - 1].ruleNumber = i + 1;
                        options[options.Count - 1].rule = rules[i];
                        options[options.Count - 1].location = a;
                        if (this.choiceMethod == choiceMethods.Automatic) return options;
                        /* this is merely for efficiency - once we get one valid option for
                         * an Automatic ruleset we can exit and invoke that option. */
                    }
            }
            return options;
        }

        /* simple functions to add and remove rules from the ruleSet */
        public void Add(grammarRule newRule)
        {
            rules.Add(newRule);
        }
        public void Remove(grammarRule removeRule)
        {
            rules.Remove(removeRule);
        }

        public ruleSet copy()
        {
            ruleSet copyOfRuleSet = new ruleSet();
            foreach (string a in applySourceFiles)
                copyOfRuleSet.applySourceFiles.Add(a);
            foreach (string a in recognizeSourceFiles)
                copyOfRuleSet.recognizeSourceFiles.Add(a);
            copyOfRuleSet.choiceMethod = choiceMethod;
            copyOfRuleSet.finalCandidates = finalCandidates;
            copyOfRuleSet.generationAfterChoice = generationAfterChoice;
            copyOfRuleSet.generationAfterCycleLimit = generationAfterCycleLimit;
            copyOfRuleSet.generationAfterNormal = generationAfterNormal;
            copyOfRuleSet.generationAfterNoRules = generationAfterNoRules;
            copyOfRuleSet.generationAfterTriggerRule = generationAfterTriggerRule;
            copyOfRuleSet.interimCandidates = interimCandidates;
            copyOfRuleSet.name = name;
            foreach (string a in ruleFileNames)
                copyOfRuleSet.ruleFileNames.Add(a);
            foreach (grammarRule a in rules)
                copyOfRuleSet.rules.Add(a);
            copyOfRuleSet.rulesDir = rulesDir;
            copyOfRuleSet.ruleSetIndex = ruleSetIndex;
            copyOfRuleSet.triggerRuleNum = triggerRuleNum;
            return copyOfRuleSet;
        }
        #endregion
    }
}
