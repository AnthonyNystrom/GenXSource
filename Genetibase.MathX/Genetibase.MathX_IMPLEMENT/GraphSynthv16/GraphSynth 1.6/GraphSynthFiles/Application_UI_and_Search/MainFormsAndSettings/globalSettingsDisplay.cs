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
    public partial class globalSettingsDisplay : Form
    {
        public globalSettings newSettings = new globalSettings();

        public globalSettingsDisplay()
        {
            InitializeComponent();

            if (Program.settings != null) newSettings = Program.settings.copyTo(newSettings);

          newSettings.hDir = newSettings.tryToFindHelpDir();
            refreshTextBoxes();
            newSettings.initPropertiesBag();
            this.propertyGrid1.SelectedObject = newSettings.Bag;
        }

        private void refreshTextBoxes()
        {
            this.wDirText.Text = newSettings.wDir;
            this.iDirText.Text = newSettings.iDir;
            this.oDirText.Text = newSettings.oDir;
            this.rDirText.Text = newSettings.rDir;
            this.helpDirText.Text = newSettings.hDir;
            this.helpURLText.Text = newSettings.onlineHelpURL;
            this.seedText.Text = newSettings.defaultSeedFileName;
            this.compiledRulesText.Text = newSettings.compiledparamRules;
            for (int i = 0; ((i < newSettings.numOfRuleSets) && (i < 10)); i++)
                RSText[i].Text = newSettings.defaultRSFileNames[i];
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            newSettings.stringOfRSFileNames
                = StringCollectionConverter.convert(newSettings.defaultRSFileNames);
            globalSettings tempSettings = Program.settings;
            Program.settings = newSettings;
            SearchIO.defaultVerbosity = newSettings.defaultVerbosity;
            try { newSettings.loadDefaultSeedAndRuleSets(); }
            catch (Exception ee)
            {
                if (DialogResult.Yes == MessageBox.Show("Settings did not work because of the following error: " + ee.ToString() +
                    " Revert to Previous Settings?", "Error in Settings. Revert back?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    Program.settings = tempSettings;
                return;
            }
            cancelButton_Click(sender, e);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.Title = "Save settings as ...";
            fileChooser.InitialDirectory = newSettings.execDir;
            fileChooser.Filter = "settings file (*.config)|*.config";
            fileChooser.FileName = "GraphSynthSettings";
            DialogResult result = fileChooser.ShowDialog();
            string filename;
            fileChooser.CheckFileExists = false;
            if (result == DialogResult.Cancel) return;
            filename = fileChooser.FileName;
            if (filename == "" || filename == null)
                MessageBox.Show("Invalid file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            newSettings.saveNewSettings(filename);
            this.applyButton.Enabled = true;
        }

        private void browseWorkingDirButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Set a working directory for GraphSynth (input, output, " +
                "rules, and help directories will be set relative to this).";
            folderBrowser.SelectedPath = newSettings.workingDirectory;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                //newSettings.checkWorkingDirAndUpdateOthers(folderBrowser.SelectedPath);
                newSettings.wDir = folderBrowser.SelectedPath;
                refreshTextBoxes();
            }
        }
        void wDirText_Leave(object sender, System.EventArgs e)
        {
            newSettings.wDir = this.wDirText.Text;
            refreshTextBoxes();
        }

        private void browseOutputDirButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Set the output directory for GraphSynth.";
            folderBrowser.SelectedPath = newSettings.workingDirectory;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                newSettings.oDir = folderBrowser.SelectedPath;
                newSettings.oDir = newSettings.oDir.Replace(newSettings.workingDirectory, "");
                refreshTextBoxes();
            }
        }
        void oDirText_Leave(object sender, System.EventArgs e)
        {
            newSettings.oDir = this.oDirText.Text;
            refreshTextBoxes();
        }


        private void browseInputDirButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Set the input directory for GraphSynth.";
            folderBrowser.SelectedPath = newSettings.workingDirectory;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                newSettings.iDir = folderBrowser.SelectedPath;
                newSettings.iDir = newSettings.iDir.Replace(newSettings.workingDirectory, "");
                refreshTextBoxes();
            }
        }
        void iDirText_Leave(object sender, System.EventArgs e)
        {
            newSettings.iDir = this.iDirText.Text;
            refreshTextBoxes();
        }

        private void browseRulesDirButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Set the rules directory for GraphSynth.";
            folderBrowser.SelectedPath = newSettings.workingDirectory;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                newSettings.rDir = folderBrowser.SelectedPath;
                newSettings.rDir = newSettings.rDir.Replace(newSettings.workingDirectory, "");
                refreshTextBoxes();
            }
        }
        void rDirText_Leave(object sender, System.EventArgs e)
        {
            newSettings.rDir = this.rDirText.Text;
            refreshTextBoxes();
        }

        private void browseHelpDirButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Set the help directory for GraphSynth.";
            folderBrowser.SelectedPath = newSettings.execDir;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                newSettings.hDir = folderBrowser.SelectedPath;
                newSettings.hDir = newSettings.hDir.Replace(newSettings.execDir, "");
                refreshTextBoxes();
            }
        }
        void helpDirText_Leave(object sender, System.EventArgs e)
        {
            newSettings.hDir = this.helpDirText.Text;
            refreshTextBoxes();
        }

        private void browseHelpURLButton_Click(object sender, EventArgs e)
        {
            this.helpURLText.Text = Clipboard.GetText();
            newSettings.onlineHelpURL = this.helpURLText.Text;
            refreshTextBoxes();
        }
        void helpURLText_Leave(object sender, System.EventArgs e)
        {
            newSettings.onlineHelpURL = this.helpURLText.Text;
            refreshTextBoxes();
        }

        private void RefreshSeedRulesTab(object sender, EventArgs e)
        {
            for (int i = 0; i != 10; i++)
            {
                if (i < newSettings.numOfRuleSets)
                {
                    this.RSText[i].Visible = true;
                    this.RSbutton[i].Visible = true;
                    this.RSbutton[i].BringToFront();
                    this.RSText[i].Text = Path.GetFileName(newSettings.defaultRSFileNames[i]);
                }
                else
                {
                    this.RSbutton[i].Visible = false;
                    this.RSText[i].Visible = false;
                }
            }
        }





        void compiledRulesText_Leave(object sender, System.EventArgs e)
        {
            newSettings.compiledparamRules = this.compiledRulesText.Text;
            refreshTextBoxes();
        }


        private void browseDefSeedButton_Click(object sender, EventArgs e)
        {
            newSettings.defaultSeedFileName = Path.GetFileName(Program.main.getOpenFilename(newSettings.inputDirectory));
            newSettings.defaultSeedFileName.Replace(newSettings.inputDirectory, "");
            refreshTextBoxes();
        }

        private void browseCompiledDLLButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileChooser = new OpenFileDialog();
                fileChooser.CheckFileExists = false;
                fileChooser.Title = "Open or type the name of a '.dll'";
                fileChooser.InitialDirectory = newSettings.rulesDirectory;
                fileChooser.Filter = "(dll files)|*.dll";
                DialogResult result = fileChooser.ShowDialog();
                if (result != DialogResult.Cancel)
                {
                    newSettings.compiledparamRules = Path.GetFileName(fileChooser.FileName);
                    newSettings.compiledparamRules.Replace(newSettings.rulesDirectory, "");
                    refreshTextBoxes();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RSbutton_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (!sender.Equals(this.RSbutton[i]))
                i++;
            newSettings.defaultRSFileNames[i] = Path.GetFileName(Program.main.getOpenFilename(newSettings.rulesDirectory));
            newSettings.defaultRSFileNames[i].Replace(newSettings.rulesDirectory, "");
            refreshTextBoxes();
        }


    }
}