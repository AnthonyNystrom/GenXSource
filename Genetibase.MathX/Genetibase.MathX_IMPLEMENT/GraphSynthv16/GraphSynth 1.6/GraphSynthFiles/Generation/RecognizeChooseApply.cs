using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GraphSynth.Representation;
using System.Threading;

namespace GraphSynth.Generation
{
    /* this is the main file and class of generation. The model adopted is that one should
     * create an inherited class of this ABSTRACT class, as has been done for randomChoose, 
     * chooseViaHumanGui, etc. This file has gone through a lot of revision to make it 
     * general to a wide variety of problems. It should not have to be altered, rather one
     * can control aspects of the execution through the ruleSets. */
    public abstract class RecognizeChooseApply
    {
        #region Fields
        /* the integer choice is used in the main loop as well as in various implementation
         * of choose. As a result we define it as global to the class. */
        protected int choice = 0;

        /* the array maxNumOfCalls is set here and is to remain relatively constants. it
         * is copied to the numOfCallsLeft at the beginning of the RCACycle into the 
         * numOfCallsLeft slot. */
        protected int[] maxNumOfCalls;

        /* Often the same seed is used as a starting point with the generation process. That
         * seed is stored here as a global field of the class. */
        protected candidate seed;

        /* The ruleSet array used to perform the generation process is stored here. */
        protected ruleSet[] rulesets;
        protected int numOfRuleSets;

        /* a simple Boolean used for debugging or interactive generation. If true then
         * the host will be replotted after each apply action. */
        protected Boolean display;
        #endregion

        #region RecognizeChooseApplyCycle -- the Generation Process Defined
        /* Here is the main Recognize, Choose, and Apply Generation Cycle. It accepts the host candidate
         * (not graph), the index of what ruleSet to invoke, and an array of size equal to the number
         * of ruleSets. At the end of the process, it returns the updated candidate. The three step
         * process may, however exit at any of five places in the loop, these are described below.
         * 1. the ruleSet invoked may not have any calls left. This will cause the GenerationStatus
         *    to be CycleLimit, and the process will execute what is stored in the 3rd position of 
         *    generationSteps, ruleSet->nextGenerationStep[2], either Stop, Loop, GoToPrevious(ruleSet),
         *    GoToNext(ruleSet), or GoToRuleSet#
         * 2. the choice operation has sent a STOP message, or more precisely a negative # or
         *    a number greater than the list of option. This results in a GenerationStatus of Choice
         *    and the execution of ruleSet->nextGenerationStep[1] (any of the options stated above).
         * 3. there are no rules recognized for the graph. This results in a GenerationStatus of
         *    NoRules and the execution of ruleSet->nextGenerationStep[3] (any of the options above).
         * 4. A trigger rule has been applied. This results in a GenerationStatus of TriggerRule
         *    and the execution of ruleSet->nextGenerationStep[4] (any of the options stated above).
         * 5. the recognize, choose, and apply cycle performed as intended - no abnormal activites.
         *    This results in a GenerationStatus of Normal and the execution of 
         *    ruleSet->nextGenerationStep[0] (any of the options stated above).*/
        public void RecognizeChooseApplyCycle(candidate host, int ruleSetIndex, int[] numOfCallsLeft)
        {
            while ((ruleSetIndex >= 0) && (ruleSetIndex < numOfRuleSets))
            {
                host.activeRuleSetIndex = ruleSetIndex;
                SearchIO.output("Active Rule Set = " + ruleSetIndex.ToString(), 4);
                #region terminate immediately if there are no cycles left
                if (numOfCallsLeft[ruleSetIndex] == 0)
                {
                    /* it is possible that another ruleset intends to invoke this one, but your
                     * number of calls for this set has hit its limit. */
                    host.GenerationStatus[ruleSetIndex] = GenerationStatuses.CycleLimit;
                    ruleSetIndex = nextRuleSet(ruleSetIndex, GenerationStatuses.CycleLimit);
                    SearchIO.output("cycle limit reached", 4);
                    continue;
                }
                #endregion

                #region ***** RECOGNIZE *****
                SearchIO.output("begin RCA loop for RuleSet #" + ruleSetIndex.ToString(), 4);
                List<option> options = rulesets[ruleSetIndex].recognize(host.graph);

                SearchIO.output("There are " + options.Count.ToString() + " rule choices.", 4);
                if (options.Count == 0)
                {
                    /* There are no rules to recognize, exit here. */
                    host.GenerationStatus[ruleSetIndex] = GenerationStatuses.NoRules;
                    ruleSetIndex = nextRuleSet(ruleSetIndex, GenerationStatuses.NoRules);
                    continue;
                }
                if (SearchIO.terminateRequest) return;
                #endregion

                #region ***** CHOOSE *****
                if (rulesets[ruleSetIndex].choiceMethod == choiceMethods.Automatic)
                    choice = 0;
                else choice = choose(options, host);
                SearchIO.output("Choice = #" + choice.ToString(), 4);
                if (choice == -1)
                {
                    host.undoLastRule();
                    if (display) SearchIO.addAndShowGraphDisplay(host.graph.copy(),
                        "Revert to after calling " + host.numRulesCalled + " rules");
                    continue;
                }
                if ((choice < 0) || (choice >= options.Count))
                {
                    /* the overloaded choice function may want to communicate to the loop that it
                     * should finish the process. */
                    SearchIO.output("Choice received a STOP request", 4);
                    host.GenerationStatus[ruleSetIndex] = GenerationStatuses.Choice;
                    ruleSetIndex = nextRuleSet(ruleSetIndex, GenerationStatuses.Choice);
                    continue;
                }
                if (SearchIO.terminateRequest) return;
                #endregion

                #region ***** APPLY *****
                host.saveCurrent();
                options[choice].apply(host.graph, choose(options[choice], host));
                host.addToRecipe(options[choice]);
                SearchIO.output("Rule sucessfully applied", 4);

                /* display state? */
                if (display) SearchIO.addAndShowGraphDisplay(host.graph.copy(),
                    "After calling " + host.numRulesCalled + " rules");
                if (SearchIO.terminateRequest) return;
                #endregion

                #region Check to see if loop is done
                /* First thing we do is reduce the number of calls left. Note that if you start with
                 * a negative number, the process will continue to make it more negative - mimicking
                 * no cycle limit. It is safer to use the globalvar, maxRulesToApply though.*/
                numOfCallsLeft[ruleSetIndex]--;
                /* a significant change is made here in Version 1.1.2.0, it is actually the removal of
                 * code. We were checking the numOfCallsLeft here as well as the top, but it has been decided
                 * that it is ambiguous to check in both locations. Later, it may be determined that two
                 * independent cycle limits need to be imposed, but in the mean time, the following code will be 
                 * commented out.
                 * if (numOfCallsLeft[ruleSetIndex] == 0)
                 * {  /* there of no more calls on this ruleset allowed, the limit has been reached.
                 * SearchIO.output("The maximum num of calls has been reached", 4);
                 * host.GenerationStatus[ruleSetIndex] = GenerationStatuses.CycleLimit;
                 * ruleSetIndex = nextRuleSet(ruleSetIndex, GenerationStatuses.CycleLimit);
                 * }
                 * else  */
                if (options[choice].ruleNumber == rulesets[ruleSetIndex].triggerRuleNum)
                {   /* your ruleset loops until a trigger rule and the trigger rule was just called. */
                    SearchIO.output("The trigger rule has been chosen.", 4);
                    host.GenerationStatus[ruleSetIndex] = GenerationStatuses.TriggerRule;
                    ruleSetIndex = nextRuleSet(ruleSetIndex, GenerationStatuses.TriggerRule);
                }
                else
                {  /* Normal operation */
                    SearchIO.output("RCA loop executed normally.", 4);
                    host.GenerationStatus[ruleSetIndex] = GenerationStatuses.Normal;
                    ruleSetIndex = nextRuleSet(ruleSetIndex, GenerationStatuses.Normal);
                }
                #endregion
            }
        }

        /* A helper function to RecognizeChooseApplyCycle. This function returns what the new ruleSet
         * will be. Here the enumerator nextGenerationSteps and GenerationStatuses is used to great
         * affect. Understand that if a negative number is returned, the cycle will be stopped. */
        private int nextRuleSet(int ruleSetIndex, GenerationStatuses status)
        {
            if (rulesets[ruleSetIndex].nextGenerationStep[(int)status] == nextGenerationSteps.Loop)
                return ruleSetIndex;
            else if (rulesets[ruleSetIndex].nextGenerationStep[(int)status] == nextGenerationSteps.GoToNext)
                return ++ruleSetIndex;
            else if (rulesets[ruleSetIndex].nextGenerationStep[(int)status] == nextGenerationSteps.GoToPrevious)
                return --ruleSetIndex;
            else
                return (int)rulesets[ruleSetIndex].nextGenerationStep[(int)status];
        }
        #endregion

        #region Invoking the RecognizeChooseApplyCycle
        /* some functions for invoking the RecognizeChooseApplyCycle above. That function is 
         * protected so we invoke it through one of these four functions. This functions will
         * not work, however, if one has not yet initiated the generation process to establish
         * the fields of the Generation object. */
        public candidate generateOneCandidate()
        {
            int[] numOfCalls = new int[numOfRuleSets];
            /* this copy set is needed because array are reference types and the RCA cycle
             * will modify the numOfCalls inside of it. */
            maxNumOfCalls.CopyTo(numOfCalls, 0);
            candidate newCand = seed.copy();
            RecognizeChooseApplyCycle(newCand, seed.activeRuleSetIndex, numOfCalls);
            return newCand;
        }

        public void runGUIOrRandomTest()
        {
            candidate cand = generateOneCandidate();
            SearchIO.addAndShowGraphDisplay(cand.graph, "After Rule Application");
            System.Threading.Thread.CurrentThread.Abort();
        }

        public candidate[] GenerateArrayOfCandidates(int numToCandidates)
        {
            candidate[] candidates = new candidate[numToCandidates];
            int[] numOfCalls = new int[numOfRuleSets];
            for (int i = 0; i != numToCandidates; i++)
            {
                maxNumOfCalls.CopyTo(numOfCalls, 0);
                candidate newCand = seed.copy();
                RecognizeChooseApplyCycle(newCand, seed.activeRuleSetIndex, numOfCalls);
                candidates[i] = newCand;
            }
            return candidates;
        }

        public List<candidate> GenerateListOfCandidates(int numToCandidates)
        {
            List<candidate> candidates = new List<candidate>();
            int[] numOfCalls = new int[numOfRuleSets];
            for (int i = 0; i != numToCandidates; i++)
            {
                maxNumOfCalls.CopyTo(numOfCalls, 0);
                candidate newCand = seed.copy();
                RecognizeChooseApplyCycle(newCand, seed.activeRuleSetIndex, numOfCalls);
                candidates.Add(newCand);
            }
            return candidates;
        }
        #endregion

        #region Constructors
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets, int _maxRulesToApply,
            Boolean recompileRules, string execDir, string compiledparamRules)
            : this(_seed, _rulesets, null, false, recompileRules, execDir, compiledparamRules)
        {
            maxNumOfCalls = new int[numOfRuleSets];
            for (int i = 0; i != numOfRuleSets; i++)
                maxNumOfCalls[i] = _maxRulesToApply;
        }
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets, int _maxRulesToApply, Boolean _display,
             Boolean recompileRules, string execDir, string compiledparamRules)
            :
            this(_seed, _rulesets, null, _display, recompileRules, execDir, compiledparamRules)
        {
            maxNumOfCalls = new int[numOfRuleSets];
            for (int i = 0; i != numOfRuleSets; i++)
                maxNumOfCalls[i] = _maxRulesToApply;
        }
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets, int[] _maxNumOfCalls, Boolean _display,
             Boolean recompileRules, string execDir, string compiledparamRules)
        {
            SearchIO.output("initializing generation:", 4);
            this.numOfRuleSets = _rulesets.GetLength(0);
            this.rulesets = new ruleSet[numOfRuleSets];
            for (int i = 0; i != numOfRuleSets; i++)
                rulesets[i] = _rulesets[i].copy();
            SearchIO.output("There are " + numOfRuleSets + " rule sets.", 4);
            this.seed = new candidate(_seed.copy(), numOfRuleSets);
            SearchIO.output("Seed = " + seed.graph.name, 4);
            maxNumOfCalls = _maxNumOfCalls;
            this.display = _display;
            SearchIO.output("It is " + display.ToString() + " that the SearchIO will be displayed.", 4);
            ruleSet.loadAndCompileSourceFiles(rulesets, recompileRules, compiledparamRules, execDir);
        }
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets, Boolean _display)
            : this(_seed, _rulesets, -1, _display) { }
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets)
            : this(_seed, _rulesets, -1, false) { }
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets, int[] _maxNumOfCalls)
            : this(_seed, _rulesets, _maxNumOfCalls, false) { }
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets, int _maxRulesToApply)
            : this(_seed, _rulesets, _maxRulesToApply, false) { }
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets, int _maxRulesToApply, Boolean _display)
            : this(_seed, _rulesets, null, _display)
        {
            maxNumOfCalls = new int[numOfRuleSets];
            for (int i = 0; i != numOfRuleSets; i++)
                maxNumOfCalls[i] = _maxRulesToApply;
        }
        public RecognizeChooseApply(designGraph _seed, ruleSet[] _rulesets, int[] _maxNumOfCalls, Boolean _display)
        {
            SearchIO.output("initializing generation:", 4);
            this.numOfRuleSets = _rulesets.GetLength(0);
            this.rulesets = new ruleSet[numOfRuleSets];
            for (int i = 0; i != numOfRuleSets; i++)
                rulesets[i] = _rulesets[i].copy();
            SearchIO.output("There are " + numOfRuleSets + " rule sets.", 4);
            this.seed = new candidate(_seed.copy(), numOfRuleSets);
            SearchIO.output("Seed = " + seed.graph.name, 4);
            maxNumOfCalls = _maxNumOfCalls;
            this.display = _display;
            SearchIO.output("It is " + display.ToString() + " that the SearchIO will be displayed.", 4);
        }
        #endregion

        /* Here we outline what an inherited class must contain. Basically it should have 
         * methods for the 2 types of decisions that are made - decisions on what option
         * to invoke and decisions for the variables required for the process. */

        /* Given the list of options and the candidate, determine what option to invoke.
         * Return the integer index of this option from the list. */
        public abstract int choose(List<option> options, candidate cand);

        /* Given that the rule has now been chosen, determine the values needed by the
         * rule to properly apply it to the candidate, cand. The array of double is to
         * be determined by parametric apply rules written in complement C# files for 
         * the ruleSet being used. */
        public abstract double[] choose(option RC, candidate cand);
    }
}
