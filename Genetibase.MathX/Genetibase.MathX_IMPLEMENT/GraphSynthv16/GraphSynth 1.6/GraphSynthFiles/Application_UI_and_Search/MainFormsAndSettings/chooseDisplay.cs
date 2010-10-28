using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GraphSynth.Representation;
using GraphSynth.Forms;
using GraphSynth.Generation;

namespace GraphSynth.Forms
{
    public class chooseViaHumanGui : RecognizeChooseApply
    {
        public override int choose(List<option> options, candidate cand)
        {
            SearchIO.output("There are " + options.Count.ToString() + " recognized locations.", 2);
            if (options.Count == 0)
            {
                SearchIO.output("Sorry there are no rules recognized.", 0);
                return int.MinValue;
            }
            else if (options.Count > Program.settings.maxRulesToDisplay)
            {
                SearchIO.output("Sorry there are too many rules to show.", 0);
                return int.MinValue;
            }
            else
            {
                SearchIO.output("Double-click on one to show the location.", 2);
                chooseDisplay choiceDisplay = new chooseDisplay();
                choiceDisplay.promptUser(options, (Boolean)(cand.recipe.Count == 0));
                return choiceDisplay.choice;
            }
        }

        public override double[] choose(option RC, candidate cand)
        { return null; }

        #region Constructors
        public chooseViaHumanGui(Boolean _display)
            : base(Program.seed, Program.rulesets, Program.settings.maxRulesToApply, _display,
            Program.settings.recompileRules, Program.settings.execDir, Program.settings.compiledparamRules) { }
        #endregion
    }
    public partial class chooseDisplay : Form
    {
        #region Fields
        List<option> rulesToDisplay = new List<option>();
        List<int> optionNumbers = new List<int>();
        public int choice = int.MinValue;
        System.Windows.Forms.Timer checkForStopTimer = new System.Windows.Forms.Timer();
        #endregion


        public chooseDisplay()
        {
            checkForStopTimer.Tick += new EventHandler(processTimer_Tick);
            checkForStopTimer.Interval = 500;
            checkForStopTimer.Start();
        }
        public void promptUser(List<option> RCs, Boolean hideUndo)
        {
            InitializeComponent();
            rulesToDisplay = RCs;

            string ruleNo, location;
            int option = 0;

            this.Text = "Choices from RuleSet #" + RCs[0].ruleSetIndex.ToString();

            for (int i = 0; i != rulesToDisplay.Count; i++)
            {
                option = i + 1;
                ruleNo = rulesToDisplay[i].ruleNumber.ToString();
                location = rulesToDisplay[i].location.ToString();
                recognizedRulesList.Items.Add(option.ToString() + ".\t" + ruleNo + "\t" + location);
                optionNumbers.Add(i);
            }
            if (hideUndo) this.undoButton.Enabled = false;
            ShowDialog();
        }


        private void showGraph_Click(object sender, EventArgs e)
        {
            SearchIO.addAndShowGraphDisplay(rulesToDisplay[recognizedRulesList.SelectedIndex].location.copy(),
                "Recognized Location " + recognizedRulesList.SelectedItem.ToString());
        }

        private void removeFromList_Click(object sender, EventArgs e)
        {
            int numToRemove = recognizedRulesList.CheckedIndices.Count;
            if (numToRemove == recognizedRulesList.Items.Count) 
            {
                MessageBox.Show("You cannot remove all possible options.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if                 (numToRemove == recognizedRulesList.Items.Count - 1)
            {
                int[] toRemove = new int[numToRemove];
                recognizedRulesList.CheckedIndices.CopyTo(toRemove, 0);
                for (int i = numToRemove; i != 0; i--)
                {
                    if (toRemove[i - 1] != optionNumbers.Count)
                    {
                        recognizedRulesList.Items.RemoveAt(toRemove[i - 1]);
                        optionNumbers.RemoveAt(toRemove[i - 1]);
                    }
                }
                if (DialogResult.Yes == MessageBox.Show(
                "You are removing all but one option [" +
                recognizedRulesList.Items[0].ToString() +
                "]. Would you like to apply this option?",
                "Apply Remaining Option?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    choice = optionNumbers[0];
                    this.Close();
                }
            }
            else
            {
                int[] toRemove = new int[numToRemove];
                recognizedRulesList.CheckedIndices.CopyTo(toRemove, 0);
                for (int i = numToRemove; i != 0; i--)
                {
                    if (toRemove[i - 1] != optionNumbers.Count)
                    {
                        recognizedRulesList.Items.RemoveAt(toRemove[i - 1]);
                        optionNumbers.RemoveAt(toRemove[i - 1]);
                    }
                }
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            int numChecked = recognizedRulesList.CheckedIndices.Count;
            checkForStopTimer.Stop();

            if (numChecked == 0)
            {
                MessageBox.Show("No Options Checked.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                checkForStopTimer.Start();
            }
            else if (numChecked == 1)
            {
                if (!Program.settings.confirmEachUserChoice ||
                                    (DialogResult.Yes == MessageBox.Show(
                                    "Apply Option: " + recognizedRulesList.CheckedItems[0].ToString() + "?",
                                    "Apply Option?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)))
                {
                    int[] toSaveVector = new int[numChecked];
                    recognizedRulesList.CheckedIndices.CopyTo(toSaveVector, 0);
                    choice = optionNumbers[toSaveVector[0]];
                    this.Close();
                }
                else checkForStopTimer.Start();
            }
            else if (DialogResult.Yes == MessageBox.Show(
                "You cannot apply all of these at the same time. Would you simply like to remove all unchecked Options?", "Remove Unchecked?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                int[] toSaveVector = new int[numChecked];
                recognizedRulesList.CheckedIndices.CopyTo(toSaveVector, 0);
                List<int> toSave = new List<int>(toSaveVector);
                for (int i = recognizedRulesList.Items.Count; i != 0; i--)
                {
                    if (!toSave.Contains(i - 1))
                    {
                        recognizedRulesList.Items.RemoveAt(i - 1);
                        optionNumbers.RemoveAt(i - 1);
                    }
                }
                checkForStopTimer.Start();
            }
            else checkForStopTimer.Start();
        }


        void processTimer_Tick(object sender, EventArgs e)
        {
            if (Program.terminateRequest)
            {
                recognizedRulesList.SetItemChecked(recognizedRulesList.Items.Count - 1, true);
                for (int i = 0; i != recognizedRulesList.Items.Count - 1; i++)
                    recognizedRulesList.SetItemChecked(i, false);
                applyButton_Click(sender, e);
            }


        }

        private void undo_Click(object sender, EventArgs e)
        {
                if ((!Program.settings.confirmEachUserChoice ||
                    (DialogResult.Yes == MessageBox.Show("Undo the last rule that was applied?",
                    "Undo Last Rule?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))))
                {
                    choice = -1;
                    this.Close();
                }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
                if ((!Program.settings.confirmEachUserChoice ||
                    (DialogResult.Yes == MessageBox.Show("Send Stop message to Generation Process?",
                    "Send Stop?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))))
                {
                    choice = int.MinValue;
                    this.Close();
                }
        }


    }
}