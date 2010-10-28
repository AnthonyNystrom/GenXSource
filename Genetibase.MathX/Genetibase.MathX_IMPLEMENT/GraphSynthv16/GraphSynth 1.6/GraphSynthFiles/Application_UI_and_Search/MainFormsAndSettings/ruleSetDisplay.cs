using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GraphSynth.Representation;

namespace GraphSynth.Forms
{
    public partial class ruleSetDisplay : Form
    {
        public ruleSet ruleset;

        public ruleSetDisplay(ruleSet rules, string formTitle)
        {
            this.MdiParent = Program.main;
            InitializeComponent();

            ruleset = rules;
            this.Text = formTitle;

            rules.initPropertiesBag();
            this.propertyGrid1.SelectedObject = rules.Bag;
            this.GotFocus += new EventHandler(ruleSetDisplay_Enter);
            this.Enter += new EventHandler(ruleSetDisplay_Enter);
        }

        void ruleSetDisplay_Enter(object sender, EventArgs e)
        {
            this.checkedListBox1.Items.Clear();
            for (int i = 1; i <= ruleset.rules.Count; i++)
                this.checkedListBox1.Items.Add(i.ToString() + ". " + ruleset.ruleFileNames[(i - 1)]);
        }

        private void saveAndCloseButton_Click(object sender, EventArgs e)
        {
            Program.main.saveCurrentItem_Click(sender, e);
        }

        private void removeCheckedButton_Click(object sender, EventArgs e)
        {
            int numToRemove = this.checkedListBox1.CheckedIndices.Count;
            int[] toRemove = new int[numToRemove];
            this.checkedListBox1.CheckedIndices.CopyTo(toRemove, 0);
            for (int i = numToRemove; i != 0; i--)
            {
                this.checkedListBox1.Items.RemoveAt(toRemove[i - 1]);
                ruleset.rules.RemoveAt(toRemove[i - 1]);
                ruleset.ruleFileNames.RemoveAt(toRemove[i - 1]);
            }
            /* now re-number the list. */
            string itemString;
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 1; i <= this.checkedListBox1.Items.Count; i++)
            {
                itemString = (string)this.checkedListBox1.Items[(i - 1)];
                itemString = itemString.TrimStart(digits);
                this.checkedListBox1.Items[(int)(i - 1)] = i.ToString() + itemString;
            }
        }

        private void addRuleButton_Click(object sender, EventArgs e)
        {
            string[] filenames;
            string justFileName;
            int ruleNumber;

            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.Title = "Add rules";
            fileChooser.InitialDirectory = Program.settings.rulesDirectory;
            fileChooser.Filter = "file (*.xml)|*.xml";
            fileChooser.Multiselect = true;
            DialogResult result = fileChooser.ShowDialog();
            if (result == DialogResult.Cancel) return;
            filenames = fileChooser.FileNames;

            if (filenames.GetLength(0) == 0)
                MessageBox.Show("Error in rule loading. RuleSet unchanged",
                    "Error in rule loading.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                foreach (string filename in filenames)
                {
                    try
                    {
                        ruleset.Add(grammarRule.openRuleFromXml(filename));
                        justFileName = Path.GetFileName(filename);
                        ruleNumber = this.checkedListBox1.Items.Count + 1;
                        this.checkedListBox1.Items.Add(ruleNumber.ToString() + ". " + justFileName);
                        ruleset.ruleFileNames.Add(justFileName);
                    }
                    catch
                    {
                        MessageBox.Show("Error in loading rule: " + filename,
                            "Error in rule loading.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void showRule_Click(object sender, EventArgs e)
        {
            try
            {
                grammarRuleDisplay ruleDisplay =
                    new grammarRuleDisplay(ruleset.rules[this.checkedListBox1.SelectedIndex],
                    this.checkedListBox1.Items[this.checkedListBox1.SelectedIndex].ToString());
                ruleDisplay.Show();
            }
            catch
            {
                MessageBox.Show("Please save RuleSet first before displaying rule.", "Rule Set not Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void clearAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i != checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, false);
        }
        private void checkAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i != checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, true);
        }

        private void moveCheckedUp_Click(object sender, EventArgs e)
        {
            string tempString = null;
            grammarRule tempRule = null;
            int numToMoveUp = this.checkedListBox1.CheckedIndices.Count;
            int[] toMoveUp = new int[numToMoveUp];
            this.checkedListBox1.CheckedIndices.CopyTo(toMoveUp, 0);
            for (int i = 0; i != numToMoveUp; i++)
            {
                if ((toMoveUp[i] != 0) &&
                    (!this.checkedListBox1.GetItemChecked(toMoveUp[i] - 1)))
                {
                    tempString = (string)this.checkedListBox1.Items[toMoveUp[i] - 1];
                    this.checkedListBox1.Items[toMoveUp[i] - 1] = this.checkedListBox1.Items[toMoveUp[i]];
                    this.checkedListBox1.Items[toMoveUp[i]] = tempString;
                    this.checkedListBox1.SetItemChecked(toMoveUp[i] - 1, true);
                    this.checkedListBox1.SetSelected(toMoveUp[i] - 1, false);
                    this.checkedListBox1.SetItemChecked(toMoveUp[i], false);
                    this.checkedListBox1.SetSelected(toMoveUp[i], false);
                    tempRule = ruleset.rules[toMoveUp[i]-1];
                    ruleset.rules[toMoveUp[i] - 1] = ruleset.rules[toMoveUp[i]];
                    ruleset.rules[toMoveUp[i]] = tempRule;
                    tempString = ruleset.ruleFileNames[toMoveUp[i] - 1];
                    ruleset.ruleFileNames[toMoveUp[i] - 1] = ruleset.ruleFileNames[toMoveUp[i]];
                    ruleset.ruleFileNames[toMoveUp[i]] = tempString;
                }
            }
            /* now re-number the list. */
            string itemString;
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 1; i <= this.checkedListBox1.Items.Count; i++)
            {
                itemString = (string)this.checkedListBox1.Items[(i - 1)];
                itemString = itemString.TrimStart(digits);
                this.checkedListBox1.Items[(int)(i - 1)] = i.ToString() + itemString;
            }
        }

        private void moveCheckedDown_Click(object sender, EventArgs e)
        {
            string tempString = null;
            grammarRule tempRule = null;
            int numToMoveDown = this.checkedListBox1.CheckedIndices.Count;
            int[] toMoveDown = new int[numToMoveDown];
            this.checkedListBox1.CheckedIndices.CopyTo(toMoveDown, 0);
            for (int i = numToMoveDown - 1; i>=0 ; i--)
            {
                if ((toMoveDown[i] != this.checkedListBox1.Items.Count-1) &&
                    (!this.checkedListBox1.GetItemChecked(toMoveDown[i] + 1)))
                {
                    tempString = (string)this.checkedListBox1.Items[toMoveDown[i] + 1];
                    this.checkedListBox1.Items[toMoveDown[i] + 1] = this.checkedListBox1.Items[toMoveDown[i]];
                    this.checkedListBox1.Items[toMoveDown[i]] = tempString;
                    this.checkedListBox1.SetItemChecked(toMoveDown[i] + 1, true);
                    this.checkedListBox1.SetSelected(toMoveDown[i] + 1, true);
                    this.checkedListBox1.SetItemChecked(toMoveDown[i], false);
                    this.checkedListBox1.SetSelected(toMoveDown[i], false);
                    tempRule = ruleset.rules[toMoveDown[i] + 1];
                    ruleset.rules[toMoveDown[i] + 1] = ruleset.rules[toMoveDown[i]];
                    ruleset.rules[toMoveDown[i]] = tempRule;
                    tempString = ruleset.ruleFileNames[toMoveDown[i] + 1];
                    ruleset.ruleFileNames[toMoveDown[i] + 1] = ruleset.ruleFileNames[toMoveDown[i]];
                    ruleset.ruleFileNames[toMoveDown[i]] = tempString;
                }
            }
            /* now re-number the list. */
            string itemString;
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 1; i <= this.checkedListBox1.Items.Count; i++)
            {
                itemString = (string)this.checkedListBox1.Items[(i - 1)];
                itemString = itemString.TrimStart(digits);
                this.checkedListBox1.Items[(int)(i - 1)] = i.ToString() + itemString;
            }
        }

    }
}