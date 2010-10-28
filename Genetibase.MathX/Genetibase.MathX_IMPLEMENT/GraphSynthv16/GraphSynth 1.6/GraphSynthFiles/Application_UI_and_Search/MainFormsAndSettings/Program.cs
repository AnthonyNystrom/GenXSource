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
        #region The main entry point for the application.
        [STAThread]
        public static void Main()
        {
            /* Printing is done though the usual Console.Writeline, to a textbox. */
            console = new SearchIOToTextWriter();
            Console.SetOut(Program.console);
            

            /*** splash screen intro ***/
            /* runs on a parallel thread until all program data is loaded in.*/
            AboutGraphSynth introSplashWindow = new AboutGraphSynth();
            console.outputBox = introSplashWindow.richTextBox1;
            introSplashWindow.richTextBox1.Visible = true;
            introSplashWindow.closeButton.Visible = false;
            Thread introSplashThread = new Thread(new ThreadStart(introSplashWindow.ShowAsDialog));
            introSplashThread.Start();

            /* declare the main window that contains other windows and is the main place/thread that
             * all other routines are run from. */
            output("enabling visual styles");
            Application.EnableVisualStyles();
            output("starting main form");
            main = new mainForm();
            SearchIO.main = main;

            /*** here the default parameters from App.config are loaded in.*/
            output("Reading in settings file");
            settings = globalSettings.readInSettings(introSplashThread);
            if (settings != null)
            {
                SearchIO.defaultVerbosity = settings.defaultVerbosity;
                output("Default Verbosity set to " + settings.defaultVerbosity.ToString());
                SafeInvokeHelper.Invoke(introSplashWindow, "SendToBack", null);
                /* loadDefaults can be time consuming if there are many ruleSets/rules to load. */
                settings.loadDefaultSeedAndRuleSets();

                /* Okay, done with load-in, close self-promo */
                output("Closing Splash...", 3);
                if (introSplashThread.IsAlive) introSplashThread.Abort();

                /* set the console.Writeline to the outputTextBox in main since the splash screen 
                 * is now closed. */
                console.outputBox = main.outputTextBox;
                Application.Run(main);
            }
        }
        #endregion

        #region Fields
        /* The class globalSettings is in the IOandXML directory. These values
         * are loaded in from the App.config file. */
        public static globalSettings settings;
        /* this is the reference to the mainForm - the top/largest GraphSynth Window */
        public static mainForm main;
        /* this is the reference to SearchIO text that appears to the right of the mainform */
        public static SearchIOToTextWriter console;
        /* The seed graph (the top of the search tree) is stored in seed. */
        public static designGraph seed;
        /* an array of length Program.settings.numOfRuleSets
         * these can be sets in App.config or through the options at the top of the Design
         * pulldown menu. */
        public static ruleSet[] rulesets;
        #endregion

        #region Aliases for SearchIO
        /* The SearchIO is a new static class in the Representation DLL (under the directory
         * XMLandIO. Since moving to this DLL-centric approach, I have realized that many 
         * functions about the search process that used to simply be in Program were not 
         * inaccessible to methods written in Representation, Evaluation, Guidance,and GraphLayout. 
         * Thus, in order to be somewhat back-compatible and to make it simpler to write the 
         * search process methods in Program, the following aliases to SearchIO functions have
         * been created.*/
        public static void output(object message, int verbosityLimit)
        { SearchIO.output(message, verbosityLimit); }
        public static void output(object message0)
        { SearchIO.output(message0); }
        public static void output(object message0, object message1)
        { SearchIO.output(message0, message1, message1, message1, message1); }
        public static void output(object message0, object message1, object message2)
        { SearchIO.output(message0, message1, message2, message2, message2); }
        public static void output(object message0, object message1, object message2, object message3)
        { SearchIO.output(message0, message1, message2, message3, message3); }
        public static void output(object message0, object message1, object message2, object message3,
            object message4)
        { SearchIO.output(message0, message1, message2, message3, message4); }

        public static void addAndShowGraphDisplay(designGraph graph, string title)
        { SearchIO.addAndShowGraphDisplay(graph, title); }
        public static void addAndShowGraphDisplay(designGraph graph)
        { SearchIO.addAndShowGraphDisplay(graph, graph.name); }
        public static void addAndShowGraphDisplay(string title)
        { SearchIO.addAndShowGraphDisplay(null, title); }
        public static void addAndShowGraphDisplay()
        { SearchIO.addAndShowGraphDisplay(null, null); }

        public static int processNum
        { get { return SearchIO.processNum; } }
        public static Boolean terminateRequest
        { get { return SearchIO.terminateRequest; } }
        public static TimeSpan timeInterval
        { get { return SearchIO.timeInterval; } }

        /* These final two are still aliases but we take advantage of being in Program
         * to force an update of the searchProcessController. */
        /* in your search process assign the iteration property to your iterations
         * so that the searchProcessController's iteration counter is updated. */
        public static int iteration
        {
            set
            {
                SearchIO.iteration = value;

                /* Force update of searchProcessController with this crossThread invocation */
                object[] paramValues = { null, null };
                SafeInvokeHelper.Invoke(Program.main.searchControls[processNum],
                    "updateSPCDisplay", paramValues);
            }
            get { return SearchIO.iteration; } 
        }
        /* in your search process assign an important parameter that you would like
         * to track to the miscObject - for example, best objective function seen in 
         * search, level of search tree, number of candidates searched, number of 
         * candidates in population. */
        public static object miscObject
        {
            set
            {
                if (value == null) SearchIO.miscObject = "null";
                else SearchIO.miscObject = value;

                /* Force update of searchProcessController with this crossThread invocation */
                object[] paramValues = { null, null };
                SafeInvokeHelper.Invoke(Program.main.searchControls[processNum],
                    "updateSPCDisplay", paramValues);
            }
            get { return SearchIO.miscObject; } 
        }
        #endregion
    }
}
