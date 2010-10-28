using System;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;
using System.Collections.Generic;
using Netron.GraphLib;
using Netron.GraphLib.UI;
using GraphSynth.Representation;
using GraphSynth.Forms;

namespace GraphSynth
{
    public class globalSettings : ConfigurationSection
    {
        #region SETTINGS
        #region Directories
        public string execDir = Directory.GetCurrentDirectory() + "\\";
        [ConfigurationProperty("workingDir", DefaultValue = "..\\..\\..\\..\\")]
        public string wDir
        {
            get { return (string)this["workingDir"]; }
            set { this["workingDir"] = value; }
        }

        [ConfigurationProperty("outputDir", DefaultValue = "output")]
        public string oDir
        {
            get { return (string)this["outputDir"]; }
            set { this["outputDir"] = value; }
        }

        [ConfigurationProperty("inputDir", DefaultValue = "input")]
        public string iDir
        {
            get { return (string)this["inputDir"]; }
            set { this["inputDir"] = value; }
        }

        [ConfigurationProperty("rulesDir", DefaultValue = "rules")]
        public string rDir
        {
            get { return (string)this["rulesDir"]; }
            set { this["rulesDir"] = value; }
        }

        [ConfigurationProperty("localHelpDir")]
        public string hDir
        {
            get { return (string)this["localHelpDir"]; }
            set { this["localHelpDir"] = value; }
        }

        [ConfigurationProperty("onlineHelpURL", DefaultValue = "http://www.me.utexas.edu/~adl/graphsynth/")]
        public string onlineHelpURL
        {
            get { return (string)this["onlineHelpURL"]; }
            set { this["onlineHelpURL"] = value; }
        }
        #endregion

        #region Limits
        [ConfigurationProperty("maxRulesToDisplay", DefaultValue = "100")]
        public int maxRulesToDisplay
        {
            get { return (int)this["maxRulesToDisplay"]; }
            set { this["maxRulesToDisplay"] = value; }
        }

        [ConfigurationProperty("maxRulesToApply", DefaultValue = "100")]
        public int maxRulesToApply
        {
            get { return (int)this["maxRulesToApply"]; }
            set { this["maxRulesToApply"] = value; }
        }

        [ConfigurationProperty("numOfRuleSets", DefaultValue = "0")]
        public int numOfRuleSets
        {
            get { return (int)this["numOfRuleSets"]; }
            set { this["numOfRuleSets"] = value; }
        }
        #endregion

        #region Default Rules and Seed
        [ConfigurationProperty("compiledparamRules", DefaultValue = "compiledParamRules.dll")]
        public string compiledparamRules
        {
            get { return (string)this["compiledparamRules"]; }
            set { this["compiledparamRules"] = value; }
        }

        [ConfigurationProperty("defaultSeedFileName")]
        public string defaultSeedFileName
        {
            get { return (string)this["defaultSeedFileName"]; }
            set { this["defaultSeedFileName"] = value; }
        }


        public List<string> defaultRSFileNames = new List<string>();
        [ConfigurationProperty("defaultRuleSets")]
        public string stringOfRSFileNames
        {
            get { return (string)this["defaultRuleSets"]; }
            set { this["defaultRuleSets"] = value; }
        }
        private void setUpRuleSetList(string names)
        {
            defaultRSFileNames = StringCollectionConverter.convert(names);
            for (int i = defaultRSFileNames.Count; i != 0; i--)
            {
                if (defaultRSFileNames[i - 1].Equals("xml"))
                    defaultRSFileNames.RemoveAt(i - 1);
                else if (!defaultRSFileNames[i - 1].EndsWith(".xml"))
                    defaultRSFileNames[i - 1] += ".xml";
            }
            if (defaultRSFileNames.Count == 0) defaultRSFileNames.Add("");
        }
        #endregion

        #region Misc. Defaults
        [ConfigurationProperty("recompileRules", DefaultValue = "true")]
        public Boolean recompileRules
        {
            get { return (Boolean)this["recompileRules"]; }
            set { this["recompileRules"] = value; }
        }

        [ConfigurationProperty("searchControllerPlayOnStart", DefaultValue = "false")]
        public Boolean searchControllerPlayOnStart
        {
            get { return (Boolean)this["searchControllerPlayOnStart"]; }
            set { this["searchControllerPlayOnStart"] = value; }
        }

        [ConfigurationProperty("searchControllerInMain", DefaultValue = "true")]
        public Boolean searchControllerInMain
        {
            get { return (Boolean)this["searchControllerInMain"]; }
            set { this["searchControllerInMain"] = value; }
        }
        [ConfigurationProperty("getHelpFromOnline", DefaultValue = "false")]
        public Boolean getHelpFromOnline
        {
            get { return (Boolean)this["getHelpFromOnline"]; }
            set { this["getHelpFromOnline"] = value; }
        }
        [ConfigurationProperty("confirmEachUserChoice", DefaultValue = "true")]
        public Boolean confirmEachUserChoice
        {
            get { return (Boolean)this["confirmEachUserChoice"]; }
            set { this["confirmEachUserChoice"] = value; }
        }

        [ConfigurationProperty("defaultVerbosity", DefaultValue = "3")]
        public int defaultVerbosity
        {
            get { return (int)this["defaultVerbosity"]; }
            set { this["defaultVerbosity"] = value; }
        }
        #endregion

        #region Default Generation Procedure
        public nextGenerationSteps[] defaultGenSteps = new nextGenerationSteps[5];

        [ConfigurationProperty("generationAfterNormal", DefaultValue = "Loop")]
        public nextGenerationSteps generationAfterNormal
        {
            get { return (nextGenerationSteps)this["generationAfterNormal"]; }
            set
            { this["generationAfterNormal"] = value; }
        }
        [ConfigurationProperty("generationAfterChoice", DefaultValue = "Stop")]
        public nextGenerationSteps generationAfterChoice
        {
            get
            { return (nextGenerationSteps)this["generationAfterChoice"]; }
            set { this["generationAfterChoice"] = value; }
        }
        [ConfigurationProperty("generationAfterCycleLimit", DefaultValue = "Stop")]
        public nextGenerationSteps generationAfterCycleLimit
        {
            get { return (nextGenerationSteps)this["generationAfterCycleLimit"]; }
            set { this["generationAfterCycleLimit"] = value; }
        }
        [ConfigurationProperty("generationAfterNoRules", DefaultValue = "Stop")]
        public nextGenerationSteps generationAfterNoRules
        {
            get { return (nextGenerationSteps)this["generationAfterNoRules"]; }
            set { this["generationAfterNoRules"] = value; }
        }
        [ConfigurationProperty("generationAfterTriggerRule", DefaultValue = "Stop")]
        public nextGenerationSteps generationAfterTriggerRule
        {
            get { return (nextGenerationSteps)this["generationAfterTriggerRule"]; }
            set { this["generationAfterTriggerRule"] = value; }
        }
        [ConfigurationProperty("defaultLayoutAlgorithm", DefaultValue = "None")]
        public defaultLayoutAlg defaultLayoutAlgorithm
        {
            get { return (defaultLayoutAlg)this["defaultLayoutAlgorithm"]; }
            set { this["defaultLayoutAlgorithm"] = value; }
        }
        #endregion
        #endregion

        #region Read & Write Methods
        public static globalSettings readInSettings(System.Threading.Thread splashThread)
        {
            globalSettings settings;
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = @"GraphSynthSettings.config";
                // relative path names possible

                // Open another config file 
                Configuration config =
                   ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                   ConfigurationUserLevel.None);
                if (config.Sections["GraphSynthSettings"] == null) throw new FileNotFoundException();
                else settings = (globalSettings)config.GetSection("GraphSynthSettings");
                Program.output("GraphSynthSettings.config loaded successfully.");
            }

            catch (FileNotFoundException)
            {
                if (DialogResult.Yes ==
                MessageBox.Show("Welcome to GraphSynth. This very well might be your first time with this program, " +
                    "since no configuration file (\"GraphSynthSettings.config\") was found in the executing directory (" +
                    Directory.GetCurrentDirectory().ToString() + ".\n\nYou can create a new configuration file now (by clicking YES)" +
                    ", or exit, put an already established \"GraphSynthSettings.config\" in that directory and restart (by " +
                    "clicking NO).\n\nCreate a configuration file now?", "Welcome to GraphSynth", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    globalSettingsDisplay settingsDisplay = new globalSettingsDisplay();
                    settingsDisplay.applyButton.Enabled = false;
                    settingsDisplay.ShowDialog();
                    return globalSettings.readInSettings(splashThread);
                }
                else
                {
                splashThread.Abort();
                    Program.main.Close();
                }
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("The configuration file, \"GraphSynthSettings.config\" produced an error. " +
                    "The defaults will be loaded instead. Error :" + e.ToString(), "No config file.",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                settings = new globalSettings();
                settings.hDir = settings.tryToFindHelpDir();
            }
            return settings;
        }
        public void saveNewSettings(string filename)
        {
            stringOfRSFileNames = StringCollectionConverter.convert(defaultRSFileNames);
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = filename;  // relative path names possible

                // Open another config file 
                Configuration config =
                   ConfigurationManager.OpenMappedExeConfiguration(fileMap,
                   ConfigurationUserLevel.None);


                // You need to remove the old settings object before you can replace it
                config.Sections.Remove("GraphSynthSettings");
                // with an updated one
                config.Sections.Add("GraphSynthSettings", this);
                // Write the new configuration data to the XML file
                config.Save();

            }
            catch (Exception e)
            {
                MessageBox.Show("The configuration file did not save becuase of error: " + e.ToString(),
                    "Error Saving config file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void loadDefaultSeedAndRuleSets()
        {
            defaultGenSteps[0] = this.generationAfterNormal;
            defaultGenSteps[1] = this.generationAfterChoice;
            defaultGenSteps[2] = this.generationAfterCycleLimit;
            defaultGenSteps[3] = this.generationAfterNoRules;
            defaultGenSteps[4] = this.generationAfterTriggerRule;
            Program.output("default Generation Steps set");
            Program.rulesets = new ruleSet[numOfRuleSets];
            Program.output("There are " + numOfRuleSets.ToString() + " rulesets.");
            string filename = getOpenFilename(inputDirectory, defaultSeedFileName);
            if (filename != "")
            {
                Program.output("Seed graph : ");
                Program.seed = designGraph.openGraphFromXml(filename);
                Program.main.addAndShowGraphDisplay(Program.seed, "DefaultSeed: "
                    + Path.GetFileName(filename));
            }
            int i = 0;
            setUpRuleSetList(stringOfRSFileNames);
            foreach (string fn in defaultRSFileNames)
            {
                filename = getOpenFilename(rulesDirectory, fn);
                if (filename != "")
                {
                    Program.output("Rule Set #" + i.ToString());
                    Program.rulesets[i] = ruleSet.openRuleSetFromXml(filename, defaultGenSteps);
                    Program.rulesets[i].ruleSetIndex = i;
                    Program.main.addAndShowRuleSetDisplay(Program.rulesets[i], "Rule Set #" + i + " " +
                        Path.GetFileName(filename));
                }
                if (++i == numOfRuleSets) break;
            }
            if ((i != numOfRuleSets) && (numOfRuleSets > 10))
                MessageBox.Show("This GUI can only help you define rulesets 0, through 9. " +
                    "Rulesets 10 through " + (numOfRuleSets - 1) +
                    ", will need to be establish in the settings file or as part of Program.runSearchProcess().",
                    "Higher RuleSets", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private string getOpenFilename(string dir, string filename)
        {
            if (filename == "") return "";
            if (!filename.EndsWith(".xml")) filename += ".xml";
            if ((Path.IsPathRooted(filename)) && File.Exists(filename)) return filename;
            else if (File.Exists(dir + filename)) return filename = dir + filename;
            else if (filename.Length > 0)
                MessageBox.Show("File named " + filename + " not found.",
                    " File not found.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return "";
        }
        public string tryToFindHelpDir()
        {
            if (Directory.Exists(execDir + "..\\..\\Resources\\helpFiles\\"))
                return "..\\..\\Resources\\helpFiles\\";
            else if (Directory.Exists(execDir + "..\\Resources\\helpFiles\\"))
                return "..\\Resources\\helpFiles\\";
            else return "Resources\\helpFiles\\";
        }
        #endregion

        #region Directory Properties
        public string workingDirectory
        {
            get
            {
                string temp;
                if (Path.IsPathRooted(wDir)) temp = Path.GetFullPath(wDir);
                else temp = Path.GetFullPath(execDir + wDir);
                temp = temp.Replace("\\\\", "\\");
                if (!temp.EndsWith("\\")) temp += "\\";
                return temp;
            }
        }
        public string inputDirectory
        {
            get
            {
                string temp;
                if (Path.IsPathRooted(iDir)) temp = Path.GetFullPath(iDir);
                else temp = Path.GetFullPath(workingDirectory + iDir);
                temp = temp.Replace("\\\\", "\\");
                if (!temp.EndsWith("\\")) temp += "\\";
                return temp;
            }
        }
        public string outputDirectory
        {
            get
            {
                string temp;
                if (Path.IsPathRooted(oDir)) temp = Path.GetFullPath(oDir);
                else temp = Path.GetFullPath(workingDirectory + oDir);
                temp = temp.Replace("\\\\", "\\");
                if (!temp.EndsWith("\\")) temp += "\\";
                return temp;
            }
        }
        public string rulesDirectory
        {
            get
            {
                string temp;
                if (Path.IsPathRooted(rDir)) temp = Path.GetFullPath(rDir);
                else temp = Path.GetFullPath(workingDirectory + rDir);
                temp = temp.Replace("\\\\", "\\");
                if (!temp.EndsWith("\\")) temp += "\\";
                return temp;
            }
        }
        public string helpDirectory
        {
            get
            {
                string temp;
                if (Path.IsPathRooted(hDir)) temp = Path.GetFullPath(hDir);
                else temp = Path.GetFullPath(execDir + hDir);
                temp = temp.Replace("\\\\", "\\");
                if (!temp.EndsWith("\\")) temp += "\\";
                return temp;
            }
        }

        #endregion
        public globalSettings copyTo(globalSettings copyOfSettings)
        {

            if (Program.seed != null) copyOfSettings.defaultSeedFileName = Program.seed.name;
            copyOfSettings.compiledparamRules = this.compiledparamRules;
            copyOfSettings.confirmEachUserChoice = this.confirmEachUserChoice;
            copyOfSettings.defaultGenSteps = this.defaultGenSteps;
            copyOfSettings.defaultSeedFileName = this.defaultSeedFileName;
            copyOfSettings.defaultVerbosity = this.defaultVerbosity;
            copyOfSettings.execDir = this.execDir;
            copyOfSettings.generationAfterChoice = this.generationAfterChoice;
            copyOfSettings.generationAfterCycleLimit = this.generationAfterCycleLimit;
            copyOfSettings.generationAfterNormal = this.generationAfterNormal;
            copyOfSettings.generationAfterNoRules = this.generationAfterNoRules;
            copyOfSettings.generationAfterTriggerRule = this.generationAfterTriggerRule;
            copyOfSettings.getHelpFromOnline = this.getHelpFromOnline;
            copyOfSettings.iDir = this.iDir;
            copyOfSettings.hDir = this.hDir;
            copyOfSettings.maxRulesToApply = this.maxRulesToApply;
            copyOfSettings.maxRulesToDisplay = this.maxRulesToDisplay;
            copyOfSettings.numOfRuleSets = this.numOfRuleSets;
            copyOfSettings.onlineHelpURL = this.onlineHelpURL;
            copyOfSettings.oDir = this.oDir;
            copyOfSettings.recompileRules = this.recompileRules;
            copyOfSettings.rDir = this.rDir;
            copyOfSettings.searchControllerInMain = this.searchControllerInMain;
            copyOfSettings.searchControllerPlayOnStart = this.searchControllerPlayOnStart;
            copyOfSettings.defaultLayoutAlgorithm = this.defaultLayoutAlgorithm;
            copyOfSettings.wDir = this.wDir;
            for (int i = 0; i != copyOfSettings.numOfRuleSets; i++)
            {
                if (Program.rulesets[i] != null)
                    copyOfSettings.defaultRSFileNames.Add(Program.rulesets[i].name);
                else if ((Program.settings.defaultRSFileNames.Count > i) &&
                (Program.settings.defaultRSFileNames[i] != null))
                    copyOfSettings.defaultRSFileNames.Add(
                    Path.GetFileName(Program.settings.defaultRSFileNames[i]));
                else copyOfSettings.defaultRSFileNames.Add("");
            }
            return copyOfSettings;
        }

        #region Global Settings PropsBag
        /*** the property bag ***/
        private PropertyBag ruleSetPropsBag;
        [XmlIgnore]
        public PropertyBag Bag
        {
            get { return ruleSetPropsBag; }
        }
        /* Adds the basic properties of the shape */
        public void AddPropertiesToBag()
        {
            Bag.Properties.Add(new PropertySpec("# of Rule Sets", typeof(int),
                "Generation", "The number of rule Sets allocated for this run.",
                this.numOfRuleSets));
            Bag.Properties.Add(new PropertySpec("Max # Rules to Apply", typeof(int),
                "Generation", "The upper limit to the number of rules to apply on a single candidate.",
                this.maxRulesToApply));
            Bag.Properties.Add(new PropertySpec("Max # Rules to Display", typeof(int),
                "User Interface", "The maximum number of rules to show to the user.",
                this.maxRulesToDisplay));
            Bag.Properties.Add(new PropertySpec("Default Verbosity", typeof(System.Threading.ThreadPriority),
                "User Interface", "The default value for verbosity.",
                this.defaultVerbosity));
            Bag.Properties.Add(new PropertySpec("Recompile Rules", typeof(Boolean),
                "Generation", "When a rule set is loaded should the dynamic library be recreated? This might slow down loading process if true.",
                this.recompileRules));
            Bag.Properties.Add(new PropertySpec("Confirm Each User Choice", typeof(Boolean),
                "User Interface", "When true, a confirmation window will appear whenever apply is clicked.",
                this.confirmEachUserChoice));
            Bag.Properties.Add(new PropertySpec("Keep Search Controller in Main", typeof(Boolean),
                "Display", "The search controller box can be within the main window (true) or a separate window (false).",
                this.searchControllerInMain));
            Bag.Properties.Add(new PropertySpec("Search Controller AutoPlay", typeof(Boolean),
                "Display", "The search controller box will execute process on start (true) or wait until play has been clicked (false).",
                this.searchControllerPlayOnStart));
            Bag.Properties.Add(new PropertySpec("Default Layout Algorithm", typeof(defaultLayoutAlg),
                "Display", "If this is set to a specific algorithm, the process will be invoke when the window is entered.",
                this.defaultLayoutAlgorithm));
            Bag.Properties.Add(new PropertySpec("Get Help from Online", typeof(Boolean),
                "Display", "The help window can open a URL (true) or just reference the local help files (false).",
                this.getHelpFromOnline));
            Bag.Properties.Add(new PropertySpec("Default For GenStep - After Normal", typeof(nextGenerationSteps),
                "Generation", "The default for new rule sets - what to do after a normal cycle.",
                this.generationAfterNormal));
            Bag.Properties.Add(new PropertySpec("Default For GenStep - After Choice", typeof(nextGenerationSteps),
                "Generation", "The default for new rule sets - what to do after receving end choice.",
                this.generationAfterChoice));
            Bag.Properties.Add(new PropertySpec("Default For GenStep - After No Rules", typeof(nextGenerationSteps),
                "Generation", "The default for new rule sets - what to do after a no more rules exist.",
                this.generationAfterNoRules));
            Bag.Properties.Add(new PropertySpec("Default For GenStep - AfterCycle Limit", typeof(nextGenerationSteps),
                "Generation", "The default for new rule sets - what to do after a cycle limit has been reached.",
                this.generationAfterCycleLimit));
            Bag.Properties.Add(new PropertySpec("Default For GenStep - After Trigger", typeof(nextGenerationSteps),
                "Generation", "The default for new rule sets - what to do after a trigger rule is invoked.",
                this.generationAfterTriggerRule));

        }

        /* Allows the propertygrid to set new values */
        protected void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "# of Rule Sets": e.Value = this.numOfRuleSets; break;
                case "Max # Rules to Apply": e.Value = this.maxRulesToApply; break;
                case "Max # Rules to Display": e.Value = this.maxRulesToDisplay; break;
                case "Default Verbosity": e.Value = (System.Threading.ThreadPriority)this.defaultVerbosity; break;
                case "Recompile Rules": e.Value = this.recompileRules; break;
                case "Confirm Each User Choice": e.Value = this.confirmEachUserChoice; break;
                case "Keep Search Controller in Main": e.Value = this.searchControllerInMain; break;
                case "Search Controller AutoPlay": e.Value = this.searchControllerPlayOnStart; break;
                case "Get Help from Online": e.Value = this.getHelpFromOnline; break;
                case "Default Layout Algorithm": e.Value = this.defaultLayoutAlgorithm; break;
                case "Default For GenStep - After Normal": e.Value = this.generationAfterNormal; break;
                case "Default For GenStep - After Choice": e.Value = this.generationAfterChoice; break;
                case "Default For GenStep - After No Rules": e.Value = this.generationAfterNoRules; break;
                case "Default For GenStep - AfterCycle Limit": e.Value = this.generationAfterCycleLimit; break;
                case "Default For GenStep - After Trigger": e.Value = this.generationAfterTriggerRule; break;
            }
        }

        /* Allows the propertygrid to set new values. */
        protected void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "# of Rule Sets":
                    {
                        if ((int)e.Value > this.numOfRuleSets)
                            for (int i = this.numOfRuleSets; i != (int)e.Value; i++)
                                this.defaultRSFileNames.Add("");
                        else if ((int)e.Value < this.numOfRuleSets)
                            this.defaultRSFileNames.RemoveRange((int)e.Value,
                                (this.numOfRuleSets - (int)e.Value));
                        this.numOfRuleSets = (int)e.Value; break;
                    }
                case "Max # Rules to Apply": this.maxRulesToApply = (int)e.Value; break;
                case "Max # Rules to Display": this.maxRulesToDisplay = (int)e.Value; break;
                case "Default Verbosity": this.defaultVerbosity = (int)e.Value; break;
                case "Recompile Rules": this.recompileRules = (Boolean)e.Value; break;
                case "Confirm Each User Choice": this.confirmEachUserChoice = (Boolean)e.Value; break;
                case "Keep Search Controller in Main": this.searchControllerInMain = (Boolean)e.Value; break;
                case "Search Controller AutoPlay": this.searchControllerPlayOnStart = (Boolean)e.Value; break;
                case "Get Help from Online": this.getHelpFromOnline = (Boolean)e.Value; break;
                case "Default Layout Algorithm": this.defaultLayoutAlgorithm = (defaultLayoutAlg)e.Value; break;
                case "Default For GenStep - After Normal": this.generationAfterNormal = (nextGenerationSteps)e.Value; break;
                case "Default For GenStep - After Choice": this.generationAfterChoice = (nextGenerationSteps)e.Value; break;
                case "Default For GenStep - After No Rules": this.generationAfterNoRules = (nextGenerationSteps)e.Value; break;
                case "Default For GenStep - AfterCycle Limit": this.generationAfterCycleLimit = (nextGenerationSteps)e.Value; break;
                case "Default For GenStep - After Trigger": this.generationAfterTriggerRule = (nextGenerationSteps)e.Value; break;
            }
        }

        public void initPropertiesBag()
        {
            ruleSetPropsBag = new PropertyBag(this);
            this.AddPropertiesToBag();
            ruleSetPropsBag.GetValue += new PropertySpecEventHandler(GetPropertyBagValue);
            ruleSetPropsBag.SetValue += new PropertySpecEventHandler(SetPropertyBagValue);
        }
        #endregion




    }
}