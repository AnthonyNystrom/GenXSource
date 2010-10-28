using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GraphSynth.Representation;

namespace GraphSynth.Generation
{
    public class randomChooseWithUndo : RecognizeChooseApply
    {
        protected Random rnd = new Random();

        public override int choose(List<option> options, candidate cand)
        {
            return rnd.Next(-1, options.Count);
        }
        public override double[] choose(option RC, candidate cand)
        { return null; }

        #region Constructors
        /* a constructor like these are needed to invoke the main constructor in RecognizeChooseApply.cs */
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets, int _maxRulesToApply,
            Boolean recompileRules, string execDir, string compiledparamRules)
            : base(_seed, _rulesets, _maxRulesToApply, false, recompileRules, execDir,
compiledparamRules) { }
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets, int _maxRulesToApply, Boolean _display,
             Boolean recompileRules, string execDir, string compiledparamRules)
            : base(_seed, _rulesets, _maxRulesToApply, _display, recompileRules, execDir, compiledparamRules) { }
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets, int[] _maxNumOfCalls, Boolean _display,
             Boolean recompileRules, string execDir, string compiledparamRules)
            : base(_seed, _rulesets, _maxNumOfCalls, _display, recompileRules, execDir, compiledparamRules) { }
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets, Boolean _display)
            : base(_seed, _rulesets, -1, _display) { }
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets)
            : base(_seed, _rulesets, -1, false) { }
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets, int[] _maxNumOfCalls)
            : base(_seed, _rulesets, _maxNumOfCalls, false) { }
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets, int _maxRulesToApply)
            : base(_seed, _rulesets, _maxRulesToApply, false) { }
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets, int _maxRulesToApply, Boolean _display)
            : base(_seed, _rulesets, _maxRulesToApply, _display) { }
        public randomChooseWithUndo(designGraph _seed, ruleSet[] _rulesets, int[] _maxNumOfCalls, Boolean _display)
            : base(_seed, _rulesets, _maxNumOfCalls, _display) { }
        #endregion

        #region Invoking the Generation
        /* One can use the functions provided in RecognizeChooseApply.cs under "Invoking the 
         * RecognizeChooseApplyCycle" since these will be inherited into every generation method, 
         * but there is no reason one could not write other functions directly into their specific
         * choose class. */
        public void makeOneRandomAddition(candidate cand)
        {
            /* the RecognizeChooseApplyCycle requires an array of ruleSet limits,
             * since we only intend to make one call on the activeRuleSet we make
             * an array (it should initialize to all zeros) of the proper length
             * and set its one value at the activeRuleSetIndex to 1. */
            int[] numOfCalls = new int[numOfRuleSets];
            numOfCalls[cand.activeRuleSetIndex] = 1;

            /* here the main cycle is invoked. First, we must pass a copy of the candidate
             * to the RCA cycle since the apply set will modify it, and then move the prevoius
             * state onto the prevStates under the candidate. It is not incorrect to state
             * merely the candidate here, but the prevStates will not be stored correctly.*/
            RecognizeChooseApplyCycle(cand, cand.activeRuleSetIndex, numOfCalls);
        }
        #endregion
    }
}

