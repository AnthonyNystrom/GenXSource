using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Netron.GraphLib;
using GraphSynth.Representation;

namespace GraphSynth.Forms
{
    public partial class searchProcessController : Form
    {
        #region Fields
        Thread searchThread;
        public System.Windows.Forms.Timer processTimer = new System.Windows.Forms.Timer();
        long startTime, abortedTime;
        #endregion

        #region Constructors
        public searchProcessController(int processNum)
        {
            if (Program.settings.searchControllerInMain)
                this.MdiParent = Program.main;
            InitializeComponent();
            searchThread = new Thread(new ThreadStart(Program.runSearchProcess));
            this.Text = "Search Process #" + processNum.ToString();
            searchThread.Name = "S" + processNum.ToString() + "> ";
            priorityComboBox.SelectedIndex = 0;
            SearchIO.setVerbosity(searchThread.Name, Program.settings.defaultVerbosity);
            verbosityComboBox.SelectedIndex = Program.settings.defaultVerbosity;
            processTimer.Tick += new EventHandler(updateSPCDisplay);
            processTimer.Interval = getIntervalFromVerbosity();
        }

        /* this is the one invoked by the test generation runs:
         * Recognize-->User Choose--Apply
         * Recognize-->Human Choose--Apply */
        public searchProcessController(GraphSynth.Generation.RecognizeChooseApply generation)
        {
            if (Program.settings.searchControllerInMain)
                this.MdiParent = Program.main;
            InitializeComponent();
            searchThread = new Thread(new ThreadStart(generation.runGUIOrRandomTest));
            searchThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            this.Text = generation.GetType().ToString();
            searchThread.Name = "Test> ";
            priorityComboBox.SelectedIndex = 0;
            SearchIO.setVerbosity(searchThread.Name, Program.settings.defaultVerbosity);
            verbosityComboBox.SelectedIndex = Program.settings.defaultVerbosity;
            processTimer.Tick += new EventHandler(updateSPCDisplay);
            processTimer.Interval = getIntervalFromVerbosity();
            this.stopButton.Enabled = false;
        }

        #endregion

        public void updateSPCDisplay(object sender, EventArgs e)
        {
            ThreadState currentState = searchThread.ThreadState;
            /* the ugly truth is that the threadState can change during the following
             * conditions. To avoid problems we set up a local variable first. */
            iterationBox.Text = SearchIO.getIteration(searchThread.Name).ToString();
            miscBox.Text = SearchIO.getMiscObject(searchThread.Name);
            verbosityComboBox.SelectedIndex = SearchIO.getVerbosity(searchThread.Name);
            processTimer.Interval = getIntervalFromVerbosity();
            try { priorityComboBox.SelectedIndex = (int)searchThread.Priority; }
            catch { }
            if ((currentState == ThreadState.AbortRequested) ||
                (currentState == ThreadState.StopRequested) ||
                (currentState == ThreadState.SuspendRequested))
            {
                this.playButton.Enabled = false;
                this.pauseButton.Enabled = false;
                this.stopButton.Enabled = false;
                this.abortButton.Enabled = true;
                this.ControlBox = false;
                this.priorityComboBox.Enabled = true;
                this.verbosityComboBox.Enabled = true;
                updateTimeDisplay();
            }
            else if (currentState == ThreadState.Suspended)
            {
                this.playButton.Enabled = true;
                this.pauseButton.Enabled = false;
                this.stopButton.Enabled = false;
                this.abortButton.Enabled = true;
                this.ControlBox = false;
                this.priorityComboBox.Enabled = true;
                this.verbosityComboBox.Enabled = true;
            }
            else if ((currentState == ThreadState.Running) ||
                (currentState == ThreadState.WaitSleepJoin))
            {
                this.playButton.Enabled = false;
                this.pauseButton.Enabled = true;
                this.stopButton.Enabled = true;
                this.abortButton.Enabled = true;
                this.ControlBox = false;
                this.priorityComboBox.Enabled = true;
                this.verbosityComboBox.Enabled = true;
                updateTimeDisplay();
            }
            else if ((currentState == ThreadState.Stopped) ||
                (currentState == ThreadState.Aborted))
            {
                if (abortedTime != 0)
                {
                    long test = DateTime.Now.Ticks - abortedTime;
                    if (test > 50000000)
                    /* after 5 seconds(?) close the search process window */
                    {
                        this.processTimer.Stop();
                        this.Close();
                    }
                }
                else
                {
                    this.playButton.Enabled = false;
                    this.pauseButton.Enabled = false;
                    this.stopButton.Enabled = false;
                    this.abortButton.Enabled = false;
                    this.ControlBox = true;
                    this.priorityComboBox.Enabled = false;
                    this.verbosityComboBox.Enabled = false;
                    abortedTime = DateTime.Now.Ticks;
                }
            }
        }

        public void playButton_Click(object sender, EventArgs e)
        {
            if (searchThread.ThreadState == ThreadState.Unstarted)
            {
                searchThread.Start();
                processTimer.Start();
                startTime = DateTime.Now.Ticks;
            }
            else if ((searchThread.ThreadState == ThreadState.Running) ||
                (searchThread.ThreadState == ThreadState.Suspended) ||
                (searchThread.ThreadState == ThreadState.WaitSleepJoin))
            {
                searchThread.Resume();
                processTimer.Enabled = true;
                startTime = DateTime.Now.Ticks - 
                    SearchIO.getTimeInterval(searchThread.Name).Ticks;
            }
            else SearchIO.output("Cannot (re-)start thread because it is " +
                searchThread.ThreadState.ToString(), 2);
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if ((searchThread.ThreadState == ThreadState.Running) ||
                (searchThread.ThreadState == ThreadState.WaitSleepJoin))
            {
                searchThread.Suspend();
            }
            else SearchIO.output("Cannot pause thread because it is " +
               searchThread.ThreadState.ToString(), 2);
        }
        private void stopButton_Click(object sender, EventArgs e)
        {
            if (searchThread.ThreadState != ThreadState.Suspended)
            {
                SearchIO.setTerminationRequest(searchThread.Name);
                    SearchIO.output("A stop request has been sent to your search process.", 1);
            }
            else SearchIO.output("Cannot stop thread because it is currently paused.", 2);
        }
        private void hardStopButton_Click(object sender, EventArgs e)
        {
            if (searchThread.ThreadState == ThreadState.Suspended)
                searchThread.Resume();
            if ((searchThread.ThreadState == ThreadState.Running) ||
                (searchThread.ThreadState == ThreadState.WaitSleepJoin) ||
                (searchThread.ThreadState == ThreadState.AbortRequested) ||
                (searchThread.ThreadState == ThreadState.StopRequested) ||
                (searchThread.ThreadState == ThreadState.SuspendRequested))
            {
                searchThread.Abort();
                searchThread.Join();
            }
            else SearchIO.output("Cannot hard-stop thread because it is " +
               searchThread.ThreadState.ToString(), 2);
        }

        private void priorityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchThread.Priority = (ThreadPriority)priorityComboBox.SelectedIndex;
        }

        private void verbosityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchIO.setVerbosity(searchThread.Name, verbosityComboBox.SelectedIndex);
        }


        public int getIntervalFromVerbosity()
        {
            /* this is a very subjective little function. Basically, we want to 
             * update the searchProcessController more often for high verbosity
             * systems. The time it returns is in milliseconds, and so 2 corresponds
             * to an update once a second (1000 milliseconds). */
            switch (SearchIO.getVerbosity(searchThread.Name))
            {
                case 0: return 2500;
                case 1: return 1000;
                case 2: return 500;
                case 3: return 100;
                case 4: return 50;
            }
            return 500;
        }
        void updateTimeDisplay()
        {
            SearchIO.setTimeInterval(searchThread.Name,
                new TimeSpan(DateTime.Now.Ticks - startTime));
            this.timeDisplay.Text = SearchIO.getTimeInterval(searchThread.Name).ToString();
            if (this.timeDisplay.Text.Length == 8)
                this.timeDisplay.Text = this.timeDisplay.Text + ".000";
            else if (this.timeDisplay.Text.Length < 12)
                this.timeDisplay.Text = this.timeDisplay.Text.PadRight(12, '0');
            if (this.timeDisplay.Text.StartsWith("00:"))
                this.timeDisplay.Text = this.timeDisplay.Text.Substring(3, 9);
            else
            {
                this.timeDisplay.Text = this.timeDisplay.Text.Substring(0, 11);
                this.timeText.Text = "time: (hh:mm:ss.ss):";
            }
        }


    }
}