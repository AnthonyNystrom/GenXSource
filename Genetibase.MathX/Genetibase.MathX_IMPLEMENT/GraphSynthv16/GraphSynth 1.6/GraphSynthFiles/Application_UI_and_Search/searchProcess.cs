using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Threading;
using System.ComponentModel;
using GraphSynth.Representation;
using GraphSynth.Generation;
using GraphSynth.Forms;
using System.Drawing.Printing;

namespace GraphSynth
{
    public static partial class Program
    {
        #region Search Process - your code starts here.
        public static void runSearchProcess()
        {
            /* Here is where you code goes.
             * You have access to the previous fields and properties.
             * In general, I envision that you will declare three large objects
             * at the beginning of this function (a generation approach, an evaluation
             * approach, and a guidance approach) and initialize them with their
             * important values. */
            /*** here is an example ***/
            output(null, null, "making new generation method", "initializing generation with data");
            Generation.randomChoose GenerationApproach = new Generation.randomChoose(Program.seed,
                Program.rulesets, 50, false, Program.settings.recompileRules, Program.settings.execDir,
                Program.settings.compiledparamRules);

            SearchIO.output("making new evaluation", 2);
            Evaluation.EvaluateSwirls EvaluationApproach = new Evaluation.EvaluateSwirls();

            output("making new guidance", 2);
            Guidance.DoNothingButDisplay GuidanceApproach = new Guidance.DoNothingButDisplay();

            /*start search */
            iteration = 0;
            int iterMax = 50;
            int populationSize = 5;
            candidate[] candidates = new candidate[populationSize];

            do
            {
                iteration++;

                output("", null, "generate", "entering generation",
                    "entering the generation:" + GenerationApproach.GetType().ToString());
                candidates = GenerationApproach.GenerateArrayOfCandidates(populationSize);
                SearchIO.output("leaving generation", 3);

                output(null, null, "evaluate", "entering evaluation",
                    "entering the evaluation:" + EvaluationApproach.GetType().ToString());
                EvaluationApproach.assignSwirlSizeToAllCandidates(candidates);
                output("leaving evaluation", 3);

                output("and now guidance", 2);
                //object misc;
                //GuidanceApproach.displayBiggest(candidates, out misc);
                //miscObject = misc; 
                GuidanceApproach.displayBiggest(candidates);

                /* Some sleep time (25 ms) for the thread will ensure that the display thread 
                 * has time to update itself. This can also happen if the priority is set very
                 * low. Howver, this sleep statement does slow down the process. Comment out if 
                 * there are many iteration (> 10^5). */
                //Thread.Sleep(25);
                /* Additonally, it may be useful to force a garbage collection at the end of each 
                * iteration. Although, since it is time consuming, check to see how memory is 
                * increasing in TaskManager. If it is unaffected by this, then comment out. 
                * It is typical for GraphSynth to require to 50 MB. Essentially, the sleep 
                 statement should give the system time to garbage collect as well. */
                //GC.Collect();

            } while (!terminateRequest && iteration != iterMax); /* also check if population converged
                                                             * some additional boolean functions. */
            addAndShowGraphDisplay(candidates[0].graph, "candidate 0");
            addAndShowGraphDisplay(candidates[1].graph, "candidate 1");
            addAndShowGraphDisplay(candidates[2].graph, "candidate 2");
            addAndShowGraphDisplay(candidates[3].graph, "candidate 3");
            addAndShowGraphDisplay(candidates[4].graph, "candidate 4");
            candidate.saveToXml(candidates, "candidate", settings.outputDirectory);
        }
        #endregion
    }
}

