using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace GraphSynth.Representation
{
    public class candidate
    {
        #region Constructor
        /* a candidate can be made with nothing or by passing the graph that will be set
         * to its current state. */
        public candidate() { }
        public candidate(designGraph _graph, int numRuleSets)
        {
            this.graph = _graph;
            for (int i = 0; i != numRuleSets; i++)
                GenerationStatus.Add(GenerationStatuses.Unspecified);
        }
        #endregion

        #region Fields
        private designGraph currentState;

        /* currently we store all the previous states of a candidate. This makes candidate a 
         * 'heavy' class, but it allows us to go back to how it existed quickly. */
        private List<designGraph> prevStates = new List<designGraph>();

        /* the recipe is a list of all the options that were chosen to create the candidate. 
         * Option is stored under representation. Each option contains, the rulesetindex, the
         * number of the rule, a reference to the rule, and the location of where the rule 
         * was applied. */
        public List<option> recipe = new List<option>();

        /* a list of numbers used to define a candidate's worth. While this is a public field, 
         * it may be less buggy to write code using the properties f0, f1, f2, f3, and f4 stored in
         * candidateXMLIO.cs under the directory IO and XML. */
        public List<double> performanceParams = new List<double>();

        /* the activeRuleSetIndex is set during the recognize->choose->apply generation. It is
         * very similar to the candidate property, lastRuleSetIndex stored in candidateXMLIO.cs 
         * however in certain subtle yet important occasions the two will differ. This will happen
         * if an RCA loop starts but doesn't complete apply. This happens if max number of calls is
         * reached, if choice is STOP, or no rules are recognized. */
        public int activeRuleSetIndex;

        /* This is an arbitrary value set by the search process. Likely it will be set to the #
         * of iterations the candidate has existed in. */
        public int age;

        /* Just like the discussion for activeRuleSetIndex, GenerationStatus stores what has
         * happened during the RCA generation loop. The is one for each ruleSet as each ruleSet
         * may have ended in a different way. The enumeration is stored under IOandXML->
         * globalSettings.cs, and the possible values are: Normal, Choice, CycleLimit,
         * NoRules, TriggerRule. */
        public List<GenerationStatuses> GenerationStatus = new List<GenerationStatuses>();

        #endregion

        #region Properties
        /* stating a candidate's graph is simply it's current state. However, if this
         * property is used in to set the graph to a new one, then we move the current
         * state onto the prevStates list. */
        public designGraph graph
        {
            get
            { return currentState; }
            set
            {
               // if (currentState != null)
               //     prevStates.Add(currentState.copy());
                currentState = value;
            }
        }
        [XmlIgnore]
        public int numRulesCalled
        {
            get
            {
                return this.recipe.Count;
            }
        }
        [XmlIgnore]
        public int lastRuleSetIndex
        {
            get
            {
                return this.recipe[recipe.Count - 1].ruleSetIndex;
            }
        }
        /* the following five properties simply make the performance
         * parameters easier to code. we simply refer to the as f0, f1,
         * f2, f3, and f4. In rare cases will you need more than these. 
         * If they have yet to be defined, they return Not-A-Number 
         * (a quality that C# double understands). */
        [XmlIgnore]
        public double f0
        {
            get
            {
                if (this.performanceParams.Count < 1)
                    return Double.NaN;
                else return performanceParams[0];
            }
            set
            {
                if (performanceParams.Count < 1)
                    performanceParams.Add(value);
                else performanceParams[0] = value;
            }
        }
        [XmlIgnore]
        public double f1
        {
            get
            {
                if (this.performanceParams.Count < 2)
                    return Double.NaN;
                else return performanceParams[1];
            }
            set
            {
                if (performanceParams.Count < 2)
                {
                    f0 = f0;
                    performanceParams.Add(value);
                }
                else performanceParams[1] = value;
            }
        }
        [XmlIgnore]
        public double f2
        {
            get
            {
                if (this.performanceParams.Count < 3)
                    return Double.NaN;
                else return performanceParams[2];
            }
            set
            {
                if (performanceParams.Count < 3)
                {
                    f0 = f0;
                    f1 = f1;
                    performanceParams.Add(value);
                }
                else performanceParams[2] = value;
            }
        }
        [XmlIgnore]
        public double f3
        {
            get
            {
                if (this.performanceParams.Count < 4)
                    return Double.NaN;
                else return performanceParams[3];
            }
            set
            {
                if (performanceParams.Count < 4)
                {
                    f0 = f0;
                    f1 = f1;
                    f2 = f2;
                    performanceParams.Add(value);
                }
                else performanceParams[3] = value;
            }
        }
        [XmlIgnore]
        public double f4
        {
            get
            {
                if (this.performanceParams.Count < 5)
                    return Double.NaN;
                else return performanceParams[4];
            }
            set
            {
                if (performanceParams.Count < 5)
                {
                    f0 = f0;
                    f1 = f1;
                    f2 = f2;
                    f3 = f3;
                    performanceParams.Add(value);
                }
                else performanceParams[4] = value;
            }
        }
        #endregion

        #region Misc Methods
        /* Save state is the result */
        public void saveCurrent()
        {
            if (currentState != null) prevStates.Add(currentState.copy());
        }
        /* This is called (currently only) from the RCA loop. This happens
         * directly after the rule is APPLIED. A rule application updates
         * the currentstate, so this correspondingly adds the option
         * to the recipe. */
        public void addToRecipe(option currentrule)
        {
            option newestrule = new option();
            newestrule.ruleSetIndex = currentrule.ruleSetIndex;
            newestrule.rule = currentrule.rule;
            newestrule.ruleNumber = currentrule.ruleNumber;
            newestrule.location = currentrule.location;
            recipe.Add(newestrule);
        }

        /* This is perhaps the whole reason previous states are used.
         * Rules cannot be guaranteed to work in reverse as they work
         * forward, so this simply resets the candidate to how it looked
         * prior to calling the last rule. */
        public void undoLastRule()
        {
            if (prevStates.Count > 0)
            {
                currentState = prevStates[prevStates.Count - 1];
                prevStates.RemoveAt(prevStates.Count - 1);
                recipe.RemoveAt(recipe.Count - 1);
                for (int i = 0; i != performanceParams.Count; i++)
                    performanceParams[i] = 0.0;
                age = 0;
            }
        }

        /* A copy of a candidate is returned. Very similar to designGraph copy.
         * We make sure to not do a shallow copy (ala Clone) since we are unsure
         * how each candidate may be changed in the future. */
        public candidate copy()
        {
            candidate copyOfCand = new candidate();

            copyOfCand.currentState = this.currentState.copy();
            foreach (designGraph d in prevStates)
                copyOfCand.prevStates.Add(d.copy());
            foreach (option rc in recipe)
            {
                option copiedRC = new option();
                copiedRC.ruleSetIndex = rc.ruleSetIndex;
                copiedRC.ruleNumber = rc.ruleNumber;
                copiedRC.rule = rc.rule;
                copiedRC.location = rc.location;
                copyOfCand.recipe.Add(copiedRC);
            }
            foreach (double f in this.performanceParams)
                copyOfCand.performanceParams.Add(f);

            foreach (GenerationStatuses a in this.GenerationStatus)
                copyOfCand.GenerationStatus.Add(a);


            return copyOfCand;
        }
        #endregion

        #region Open and Save Static Methods
        public static void saveToXml(List<candidate> candidates, string filename, string outputDir)
        {
            filename = Path.GetFileName(filename).TrimEnd(new char[] { '.', 'x', 'm', 'l' });
            for (int i = 0; i != candidates.Count; i++)
            {
                string counter = i.ToString();
                counter = counter.PadLeft(3, '0');
                saveToXml(candidates[i], outputDir + filename + counter + ".xml");
            }
        }
        public static void saveToXml(candidate[] candidates, string filename, string outputDir)
        {
            filename = Path.GetFileName(filename).TrimEnd(new char[] { '.', 'x', 'm', 'l' });
            for (int i = 0; i != candidates.GetLength(0); i++)
            {
                string counter = i.ToString();
                counter = counter.PadLeft(3, '0');
                saveToXml(candidates[i], outputDir + filename + counter + ".xml");
            }
        }
        public static void saveToXml(candidate c1, string filename)
        {
            StreamWriter candidateWriter = null;
            try
            {
                candidateWriter = new StreamWriter(filename);
                XmlSerializer candidateSerializer = new XmlSerializer(typeof(candidate));
                candidateSerializer.Serialize(candidateWriter, c1);
            }
            catch (Exception ioe)
            {
                MessageBox.Show(ioe.ToString(), "XML Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (candidateWriter != null) candidateWriter.Close();
            }
        }

        public static candidate openCandidateFromXml(string filename)
        {
            candidate newCandidate = null;
            StreamReader candidateReader = null;
            try
            {
                candidateReader = new StreamReader(filename);
                XmlSerializer candidateDeserializer = new XmlSerializer(typeof(candidate));
                newCandidate = (candidate)candidateDeserializer.Deserialize(candidateReader);
                if (newCandidate.graph != null)
                    newCandidate.graph.internallyConnectGraph();
                foreach (designGraph a in newCandidate.prevStates)
                    a.internallyConnectGraph();
            }
            catch (Exception ioe)
            {
                MessageBox.Show(ioe.ToString(), "XML Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (candidateReader != null) candidateReader.Close();
            }
            return newCandidate;
        }
        #endregion

    }
}
