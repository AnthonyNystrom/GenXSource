using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using Netron.GraphLib;

namespace GraphSynth.Representation
{
    public partial class ruleSet
    {
        #region Fields
        private FileSystemWatcher watch;
        private Boolean FileChangedBypass = false;
        private string rulesDir;
        public nextGenerationSteps generationAfterNormal
        {
            get { return nextGenerationStep[0]; }
            set { nextGenerationStep[0] = (nextGenerationSteps)value; }
        }
        public nextGenerationSteps generationAfterChoice
        {
            get { return nextGenerationStep[1]; }
            set { nextGenerationStep[1] = (nextGenerationSteps)value; }
        }
        public nextGenerationSteps generationAfterCycleLimit
        {
            get { return nextGenerationStep[2]; }
            set { nextGenerationStep[2] = (nextGenerationSteps)value; }
        }
        public nextGenerationSteps generationAfterNoRules
        {
            get { return nextGenerationStep[3]; }
            set { nextGenerationStep[3] = (nextGenerationSteps)value; }
        }
        public nextGenerationSteps generationAfterTriggerRule
        {
            get { return nextGenerationStep[4]; }
            set { nextGenerationStep[4] = (nextGenerationSteps)value; }
        }
        #endregion

        #region IO Methods
        public static void saveRuleSetToXml(string filename, ruleSet rules)
        {
            StreamWriter ruleWriter = null;
            try
            {
                ruleWriter = new StreamWriter(filename);
                XmlSerializer ruleSerializer = new XmlSerializer(typeof(ruleSet));
                ruleSerializer.Serialize(ruleWriter, rules);
            }
            catch (Exception ioe)
            {
                MessageBox.Show(ioe.ToString(), "XML Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (ruleWriter != null) ruleWriter.Close();
            }
        }

        public static ruleSet openRuleSetFromXml(string filename, nextGenerationSteps[] defaultGenSteps)
        {
            ruleSet newRuleSet = null;
            StreamReader ruleReader = null;
            try
            {
                ruleReader = new StreamReader(filename);
                XmlSerializer ruleDeserializer = new XmlSerializer(typeof(ruleSet));
                newRuleSet = (ruleSet)ruleDeserializer.Deserialize(ruleReader);
                SearchIO.output(Path.GetFileName(filename) + " successfully loaded.");
                newRuleSet.rulesDir = Path.GetDirectoryName(filename) + "\\";
                newRuleSet.loadRulesFromFileNames();
                newRuleSet.initializeFileWatcher(newRuleSet.rulesDir);
                if (newRuleSet.name == null)
                    newRuleSet.name = Path.GetFileName(filename).TrimEnd(new char[] { '.', 'x', 
                        'm', 'l' });

                for (int i = 0; i != 5; i++)
                {
                    if (newRuleSet.nextGenerationStep[i] == nextGenerationSteps.Unspecified)
                        newRuleSet.nextGenerationStep[i] = defaultGenSteps[i];
                }
            }
            catch (Exception ioe)
            {
                MessageBox.Show(ioe.ToString(), "XML Serialization Error", MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
            }
            finally
            {
                if (ruleReader != null) ruleReader.Close();
            }

            return newRuleSet;
        }
        public void loadRulesFromFileNames()
        {
            rules = new List<grammarRule>();
            foreach (string ruleName in ruleFileNames)
            {
                SearchIO.output("Loading " + ruleName);
                rules.Add(grammarRule.openRuleFromXml(rulesDir + ruleName));
            }
        }
        #endregion

        #region Load and compile Source Files
        public static void loadAndCompileSourceFiles(ruleSet[] rulesets, Boolean recompileRules, 
            string compiledparamRules, string execDir)
        {
            object compiledFunctions = null;
            List<string> allSourceFiles = new List<string>();
            CompilerResults cr = null;
            string rulesDirectory = rulesets[0].rulesDir;

            if (compiledFunctionsAlreadyLoaded(rulesets)) return;
            try
            {
                if (recompileRules)
                {
                    if (FindSourceFiles(rulesets, allSourceFiles, rulesDirectory))
                    {
                        if (CompileSourceFiles(rulesets, allSourceFiles, out cr,
                            rulesDirectory, execDir, compiledparamRules))
                        {
                            System.Reflection.Assembly a = cr.CompiledAssembly;
                            compiledFunctions = a.CreateInstance("GraphSynth.ParamRules.ParamRules");
                        }
                    }
                }
                else
                {
                    /* load .dll since compilation crashed */
                    string[] filename = Directory.GetFiles(rulesDirectory, "*"
                        + compiledparamRules + "*");
                    if (filename.GetLength(0) > 1)
                        throw new Exception("More than one compiled library (*.dll) similar to "
                            + compiledparamRules + "in" + rulesDirectory);
                    else if (filename.GetLength(0) == 0)
                        throw new Exception("Compiled library: "
                            + compiledparamRules + "not found in" + rulesDirectory);
                    else
                        compiledFunctions = Assembly.Load(filename[0]);
                }
                if (compiledFunctions != null)
                {
                    Type t = compiledFunctions.GetType();
                    foreach (ruleSet set in rulesets)
                        foreach (grammarRule rule in set.rules)
                        {
                            rule.DLLofFunctions = compiledFunctions;
                            foreach (string function in rule.recognizeFunctions)
                                rule.recognizeFuncs.Add(t.GetMethod(function));
                            foreach (string function in rule.applyFunctions)
                                rule.applyFuncs.Add(t.GetMethod(function));
                        }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Compilation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static Boolean FindSourceFiles(ruleSet[] rulesets, List<string> allSourceFiles,
            string rulesDirectory)
        {
            Boolean filesFound = true;

            foreach (ruleSet a in rulesets)
            {
                foreach (string file in a.recognizeSourceFiles)
                {
                    if (File.Exists(rulesDirectory + file))
                        allSourceFiles.Add(rulesDirectory + file);
                    else
                    {
                        MessageBox.Show("Missing source file: " + file +
                            ". Cancelling compilation of C# recognize source file.",
                            "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        filesFound = false;
                        break;
                    }
                }
                foreach (string file in a.applySourceFiles)
                {
                    if (File.Exists(rulesDirectory + file))
                        allSourceFiles.Add(rulesDirectory + file);
                    else
                    {
                        MessageBox.Show("Missing source file: " + file +
                            ". Cancelling compilation of C# apply source file.",
                            "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        filesFound = false;
                        break;
                    }
                }
            }
            if (allSourceFiles.Count == 0) filesFound = false;
            return filesFound;
        }

        public static Boolean CompileSourceFiles(ruleSet[] rulesets, List<string> allSourceFiles,
            out CompilerResults cr, string rulesDir, string execDir, string compiledparamRules)
        {
            cr = null;
            try
            {
                CSharpCodeProvider c = new CSharpCodeProvider();
               // c.CreateCompiler();
//                ICodeCompiler icc = c.CreateCompiler();
                CompilerParameters cp = new CompilerParameters();

                cp.ReferencedAssemblies.Add("system.dll");
                cp.ReferencedAssemblies.Add("system.xml.dll");
                cp.ReferencedAssemblies.Add("system.data.dll");
                cp.ReferencedAssemblies.Add("system.windows.forms.dll");
                cp.ReferencedAssemblies.Add(execDir + "GraphSynth.exe");
                cp.ReferencedAssemblies.Add(execDir + "Representation.dll");

                cp.CompilerOptions = "/t:library";
                cp.GenerateInMemory = true;
                cp.OutputAssembly = rulesDir + compiledparamRules;
                string[] allSourceFilesArray = allSourceFiles.ToArray();

                cr = c.CompileAssemblyFromFile(cp, allSourceFilesArray);

                //cr = icc.CompileAssemblyFromFileBatch(cp, allSourceFilesArray);
                if (cr.Errors.Count > 0) throw new Exception();
                else return true;
            }
            catch
            {
                MessageBox.Show("Error Compiling C# recognize and apply source files.",
                    "Compilation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreach (CompilerError e in cr.Errors)
                    SearchIO.output(e.ToString(), 1);
                return false;
            }
        }

        private static Boolean compiledFunctionsAlreadyLoaded(ruleSet[] rulesets)
        {
            foreach (ruleSet set in rulesets)
                if (set != null)
                {
                    foreach (grammarRule rule in set.rules)
                        if (rule.recognizeFuncs.Count + rule.applyFuncs.Count !=
                            rule.recognizeFunctions.Count + rule.applyFunctions.Count)
                            return false;
                }
            return true;
        }
        #endregion

        #region FileWatch
        /* There is no way to prevent watching all the *.xml files in the rulesDirectory -
         * it seems that filter is limited to this. That's okay, in each of the events that is
         * triggered we first see if it is important to tell the user. */
        public void initializeFileWatcher(string rulesDir)
        {
            watch = new FileSystemWatcher();
            watch.Path = rulesDir;
            watch.Changed += new FileSystemEventHandler(watch_Changed);
            watch.Created += new FileSystemEventHandler(watch_Created);
            watch.Deleted += new FileSystemEventHandler(watch_Deleted);
            watch.Renamed += new RenamedEventHandler(watch_Renamed);
            watch.EnableRaisingEvents = true;
            watch.IncludeSubdirectories = true;
            watch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                | NotifyFilters.FileName;

        }

        void watch_Renamed(object sender, RenamedEventArgs e)
        {
            string oldName = Path.GetFileName(e.OldFullPath);
            string newName = Path.GetFileName(e.FullPath);
            if (ruleFileNames.Contains(oldName))
            {
                DialogResult res = MessageBox.Show("It appears that you have renamed a rule that was a member of ruleset: "
                + name + ". Would you like to keep the rule in this set?", "Rule Set Rule Renamed.", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    FileChangedBypass = true;
                    int i = ruleFileNames.FindIndex(delegate(string a)
                        { return a.Equals(oldName); });
                    ruleFileNames[i] = newName;
                    loadRulesFromFileNames();
                }
            }
        }

        void watch_Deleted(object sender, FileSystemEventArgs e)
        {
            if (ruleFileNames.Contains(e.Name))
            {
                DialogResult res = MessageBox.Show("It appears that you have deleted a rule that was a member of ruleset: "
                + name + ". Would you like to delete the rule from the set?", "Rule Set Rule Deleted.", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    FileChangedBypass = true;
                    int i = ruleFileNames.FindIndex(delegate(string a)
                        { return a.Equals(e.Name); });
                    ruleFileNames.RemoveAt(i);
                    loadRulesFromFileNames();
                }
            }
        }

        void watch_Created(object sender, FileSystemEventArgs e)
        {
            if (ruleFileNames.Contains(e.Name))
            {
                DialogResult res = MessageBox.Show("It appears that you have created a rule that was a member of ruleset: "
                + name + ". Would you like to load it into the rule set?", "Rule Set Rule Created.", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    FileChangedBypass = true;
                    loadRulesFromFileNames();
                }
            }
        }


        void watch_Changed(object sender, FileSystemEventArgs e)
        {
            /* an annoying this happens when the user says yes to any of the above three
             * events - this event is automatically triggered. To bypass this we use the
             * boolean, FileChangedBypass. */
            if (FileChangedBypass) FileChangedBypass = false;
            else if (ruleFileNames.Contains(e.Name))
            {
                DialogResult res = MessageBox.Show("It appears that you have changed a rule that was a member of ruleset: "
                + name + ". Would you like to re-load it into the rule set?", "Rule Set Rule Changed.", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                {
                    FileChangedBypass = true;
                    if (res == DialogResult.Yes)
                    {
                        loadRulesFromFileNames();
                    }
                }
            }
        }
        #endregion


        #region ruleSetPropsBag
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
            Bag.Properties.Add(new PropertySpec("Name", typeof(string),
                "General RuleSet Attributes", "A decriptive name for the rule set.",
                this.name));
            Bag.Properties.Add(new PropertySpec("triggerRule #", typeof(int),
                "General RuleSet Attributes", "If the generation process should stop/change at a particular rule, indicate " +
                "the rule # her. Otherwise, set to -1.", this.triggerRuleNum));
            Bag.Properties.Add(new PropertySpec("choiceMethod", typeof(choiceMethods), "General RuleSet Attributes",
                "Are rules chosen following a design agent or invoked automatically when found to be recognized (requires confluence)?",
                this.choiceMethod));
            Bag.Properties.Add(new PropertySpec("interim candidates are", typeof(candidatesAre), "General RuleSet Attributes",
                "Does the execution of any rule from this set create a FEASIBLE solution, or are they DEVELOPING steps towards" +
                " a feasible solution?", this.interimCandidates));
            Bag.Properties.Add(new PropertySpec("final candidates are", typeof(candidatesAre), "General RuleSet Attributes",
                "After the execution of the entire rule set - are the graphs FEASIBLE candidates, or are they still DEVELOPING?"
                , this.finalCandidates));
            Bag.Properties.Add(new PropertySpec("after normal cycle", typeof(nextGenerationSteps), "General RuleSet Attributes",
                "After a proper Recognize->Choose->Apply loop, what should happen?",
                this.nextGenerationStep[0]));
            Bag.Properties.Add(new PropertySpec("choice sends a STOP", typeof(nextGenerationSteps), "General RuleSet Attributes",
                "If choice sends a -1 instead of a valid option number, then what should happen?",
                this.nextGenerationStep[1]));
            Bag.Properties.Add(new PropertySpec("number of calls reached", typeof(nextGenerationSteps), "General RuleSet Attributes",
                "After a set number of calls, what should happen?", this.nextGenerationStep[2]));
            Bag.Properties.Add(new PropertySpec("no rules recognized", typeof(nextGenerationSteps), "General RuleSet Attributes",
                "If no rules are recognized, what should happen next?", this.nextGenerationStep[3]));
            Bag.Properties.Add(new PropertySpec("trigger rule invoked", typeof(nextGenerationSteps), "General RuleSet Attributes",
                "If the trigger rule is encountered (if not set to -1), what should happen next?", this.nextGenerationStep[4]));
            Bag.Properties.Add(new PropertySpec("Recognize Source Files", typeof(List<string>), "General RuleSet Attributes",
                "What C# files contain the rules parametric recognize routines?", this.recognizeSourceFiles,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
            Bag.Properties.Add(new PropertySpec("Apply Source Files", typeof(List<string>), "General RuleSet Attributes",
                "What C# files contain the rules parametric apply routines?", this.applySourceFiles,
                typeof(System.Drawing.Design.UITypeEditor), typeof(StringCollectionConverter)));
        }

        /* Allows the propertygrid to set new values */
        protected void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Name": e.Value = this.name; break;
                case "choiceMethod": e.Value = this.choiceMethod; break;
                case "triggerRule #":
                    {
                        if ((this.triggerRuleNum < 0) || (this.triggerRuleNum >= this.rules.Count))
                            e.Value = -1;
                        else e.Value = triggerRuleNum + 1;
                    }
                    break;
                case "after normal cycle": e.Value = (nextGenerationSteps)this.nextGenerationStep[0]; break;
                case "choice sends a STOP": e.Value = (nextGenerationSteps)this.nextGenerationStep[1]; break;
                case "number of calls reached": e.Value = (nextGenerationSteps)this.nextGenerationStep[2]; break;
                case "no rules recognized": e.Value = (nextGenerationSteps)this.nextGenerationStep[3]; break;
                case "trigger rule invoked": e.Value = (nextGenerationSteps)this.nextGenerationStep[4]; break;
                case "Recognize Source Files": e.Value = this.recognizeSourceFiles; break;
                case "Apply Source Files": e.Value = this.applySourceFiles; break;
                case "interim candidates are": e.Value = this.interimCandidates; break;
                case "final candidates are": e.Value = this.finalCandidates; break;
            }
        }

        /* Allows the propertygrid to set new values. */
        protected void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Name": this.name = (string)e.Value; break;
                case "choiceMethod": this.choiceMethod = (choiceMethods)e.Value; break;
                case "Recognize Source Files":
                    {
                        this.recognizeSourceFiles = (List<string>)e.Value;
                        for (int i = recognizeSourceFiles.Count; i != 0; i--)
                        {
                            if (recognizeSourceFiles[i - 1].Equals("cs"))
                                recognizeSourceFiles.RemoveAt(i - 1);
                            else if (!recognizeSourceFiles[i - 1].EndsWith(".cs"))
                                recognizeSourceFiles[i - 1] += ".cs";
                        }
                        checkForSourceFiles();
                        break;
                    }
                case "Apply Source Files":
                    {
                        this.applySourceFiles = (List<string>)e.Value;
                        for (int i = applySourceFiles.Count; i != 0; i--)
                        {
                            if (applySourceFiles[i - 1].Equals("cs"))
                                applySourceFiles.RemoveAt(i - 1);
                            else if (!applySourceFiles[i - 1].EndsWith(".cs"))
                                applySourceFiles[i - 1] += ".cs";
                        }
                        checkForSourceFiles();
                        break;
                    }
                case "triggerRule #":
                    {
                        if (((int)e.Value > rules.Count) || ((int)e.Value < 0))
                            triggerRuleNum = -1;
                        else triggerRuleNum = (int)e.Value - 1;
                        break;
                    }
                case "after normal cycle": this.nextGenerationStep[0] = (nextGenerationSteps)e.Value; break;
                case "choice sends a STOP": this.nextGenerationStep[1] = (nextGenerationSteps)e.Value; break;
                case "number of calls reached": this.nextGenerationStep[2] = (nextGenerationSteps)e.Value; break;
                case "no rules recognized": this.nextGenerationStep[3] = (nextGenerationSteps)e.Value; break;
                case "trigger rule invoked": this.nextGenerationStep[4] = (nextGenerationSteps)e.Value; break;
                case "interim candidates are": this.interimCandidates = (candidatesAre)e.Value; break;
                case "final candidates are": this.finalCandidates = (candidatesAre)e.Value; break;
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

        #region source file functions
        private void checkForSourceFiles()
        {
            string file;
            for (int i = 0; i != recognizeSourceFiles.Count; i++)
            {
                file = recognizeSourceFiles[i];
                if (!File.Exists(rulesDir + file))
                {
                    DialogResult result =
                    MessageBox.Show("Source File " + rulesDir + file
                    + " not found. Would you like to create it?", "File not found.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) createRulesSourceFile(rulesDir + file);
                }
            }
            for (int i = 0; i != applySourceFiles.Count; i++)
            {
                file = applySourceFiles[i];
                if (!File.Exists(rulesDir + file))
                {
                    DialogResult result =
                    MessageBox.Show("Source File " + rulesDir + file
                    + " not found. Would you like to create it?", "File not found.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) createRulesSourceFile(rulesDir + file);
                }
            }
        }

        private static void createRulesSourceFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter w = new StreamWriter(fs, Encoding.UTF8);

            w.WriteLine("using System;\nusing System.Collections.Generic;");
            w.WriteLine("using GraphSynth;\nusing GraphSynth.Representation;");
            w.WriteLine("\nnamespace GraphSynth.ParamRules\n{");
            w.WriteLine("public partial class ParamRules\n{");
            w.WriteLine("/* here are parametric rules written as part of the ruleSet.");
            w.WriteLine("* these are compiled at runtime into a .dll indicated in the");
            w.WriteLine("* App.config file. */");
            w.WriteLine("#region Parametric Recognition Rules");
            w.WriteLine("/* Parametric recognition rules receive as input:");
            w.WriteLine("* 1. the left hand side of the rule (L)");
            w.WriteLine("* 2. the entire host graph (host)");
            w.WriteLine("* 3. the location of the nodes in the host that L matches to (locatedNodes).");
            w.WriteLine("* 4. the location of the arcs in the host that L matches to (locatedArcs). */");
            w.WriteLine("#endregion\n\n");
            w.WriteLine("#region Parametric Application Rules");
            w.WriteLine("/* Parametric application rules receive as input:");
            w.WriteLine("* 1. the location designGraph indicating the nodes&arcs of host that match with L (Lmapping)");
            w.WriteLine("* 2. the entire host graph (host)");
            w.WriteLine("* 3. the location of the nodes in the host that R matches to (Rmapping).");
            w.WriteLine("* 4. the parameters chosen by an agent for instantiating elements of Rmapping or host (parameters). */");
            w.WriteLine("#endregion\n\n");
            w.WriteLine("}\n}");
            w.Flush();
            w.Close();
            fs.Close();
        }
        #endregion
    }
}
