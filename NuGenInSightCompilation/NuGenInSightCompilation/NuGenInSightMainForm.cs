using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Janus.Windows.UI.Tab;
using Dile.UI;
using System.IO;
using Dile.Disassemble;
using Dile.UI.Debug;
using Dile.Debug;
using System.Xml.Serialization;
using Dile.Configuration;
using System.Xml;
using RegistryMonitorUI;
using Janus.Windows.Ribbon;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using Dile;
using Microsoft.Win32;
using System.Threading;
using Genetibase.Debug;

namespace NuGenInSightCompilation
{
    [System.Diagnostics.DebuggerDisplay("Form1")]
    public partial class NuGenInSightMainForm : Form
    {
        Thread splashThread;
        public NuGenInSightMainForm()
        {            
            splashThread = new Thread(new ThreadStart(delegate() { new Splash().ShowDialog(); }));
            splashThread.Start();    

            Cursor.Current = Cursors.AppStarting;
            InitializeComponent();
            ShowInTaskbar = false;
            ilEditor1.Visible = true;
            ilEditor1.CreateControl();
            ilEditor1.StatusBar = this.uiStatusBar1;
            ribbon1.SelectedTabChanged += new Janus.Windows.Ribbon.TabEventHandler(ribbon1_SelectedTabChanged);
            ilEditor1.ProjectExplorer = ProjectExplorer;
            NuGenUIHandler.Invokee = this;
            ProjectExplorer.AddAssemblyDelegate = this.AddAssembly;            
            ilEditor1.DebuggerStateChanged = this.DebuggerStateChanged;
            ShowDebuggeeStoppedState();
            WindowState = FormWindowState.Maximized;

            realTimePanel1.GraphChanged += new NuGenRealTime.RealTimePanel.GraphChangedDelegate(realTimePanel1_GraphChanged);

            FormClosing += new FormClosingEventHandler(NuGenInSightMainForm_FormClosing);
            PopulateComboBox();

            this.Load += new EventHandler(NuGenInSightMainForm_Load);

            label16.Text = System.Environment.OSVersion.ToString();

            RegistryKey Rkey = Registry.LocalMachine;
            Rkey = Rkey.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
            label15.Text = (string)Rkey.GetValue("ProcessorNameString");

            Rkey = Registry.LocalMachine;
            Rkey = Rkey.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
            label18.Text = (string)Rkey.GetValue("~MHZ").ToString() + " MHZ";

            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            label17.Text = ramCounter.NextValue().ToString() + " MBytes";

            ilEditor1.SwitchTabs = delegate()
            {
                ribbon1.SelectedTab = ilRibbonTab;
            };
        }

        void NuGenInSightMainForm_Load(object sender, EventArgs e)
        {
            if (NuGenInSightCompilation.Properties.Settings.Default.RecentProjects == null)
            {
                NuGenInSightCompilation.Properties.Settings.Default.RecentProjects = new System.Collections.ArrayList();
            }
            else
            {
                foreach (String project in NuGenInSightCompilation.Properties.Settings.Default.RecentProjects)
                {
                    AddRecent(project);
                }
            }

            timer1.Start();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        private delegate void OpacityDelegate();
        OpacityDelegate opacDel;

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);

            opacDel = delegate()
            {
                this.ShowInTaskbar = true;
            };

            try
            {
                this.Invoke(opacDel);
            }
            catch { }

            Thread.Sleep(1000);

            opacDel = delegate()
            { 
              this.Opacity = 100;
              Cursor.Current = Cursors.Default;
              splashThread.Abort();                
            };

            try
            {
                this.Invoke(opacDel);
            }
            catch { }
        }

        void NuGenInSightMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        void realTimePanel1_GraphChanged(string[] instances, string currentInstance)
        {
            comboBoxCommand2.ComboBox.Items.Clear();

            foreach (String str in instances)
            {
                comboBoxCommand2.ComboBox.Items.Add(str);
            }

            comboBoxCommand2.ComboBox.SelectedValue = currentInstance;
        }

        private void PopulateComboBox()
        {
            this.comboBoxCommand1.ComboBox.Items.Clear();

            this.comboBoxCommand1.ComboBox.Items.Add("Project");
            this.comboBoxCommand1.ComboBox.Items.Add("_Total");

            foreach (Process proc in Process.GetProcesses())
            {
                this.comboBoxCommand1.ComboBox.Items.Add(proc.ProcessName);
            }  
        }

        private void AddAssembly()
        {
            newAssemblyButton_Click(null, null);
        }

        private void DebuggerStateChanged(DebuggerState newState)
        {
			switch (newState)
			{
				case DebuggerState.DebuggeeRunning:                    
					runButton.Enabled = false;
                    buttonCommand34.Enabled = false;
					pauseButton.Enabled = true;
					stopButton.Enabled = true;
					runToCursorButton.Enabled = true;                    
					stepOverButtonCommand.Enabled = false;
					stepIntoButtonCommand.Enabled = false;
					stepOutButtonCommand.Enabled = false;
                    uiStatusBar1.Panels[2].Text = attach ? "Attached" : "Process Running";
					break;

				case DebuggerState.DebuggeeStopped:
					ShowDebuggeeStoppedState();
                    uiStatusBar1.Panels[2].Text = detach ? "Detached" : "Process Stopped";
					//doNotStopButton.Enabled = false;
					//doNotStopHereButton.Enabled = false;
					//CloseDynamicModuleDocuments();
					break;

				case DebuggerState.DebuggeePaused:
				case DebuggerState.DebuggeeSuspended:
					runButton.Enabled = true;
                    buttonCommand34.Enabled = true;
					pauseButton.Enabled = false;
					stopButton.Enabled = true;
					runToCursorButton.Enabled = true;
					stepOverButtonCommand.Enabled = true;
					stepIntoButtonCommand.Enabled = true;
					stepOutButtonCommand.Enabled = true;
                    uiStatusBar1.Panels[2].Text = "Process Suspended";
					break;

				case DebuggerState.EvaluatingExpression:
					runButton.Enabled = false;
                    buttonCommand34.Enabled = false;
					pauseButton.Enabled = false;
					stopButton.Enabled = false;
					runToCursorButton.Enabled = false;
					stepOverButtonCommand.Enabled = false;
					stepIntoButtonCommand.Enabled = false;
					stepOutButtonCommand.Enabled = false;
                    uiStatusBar1.Panels[2].Text = attach ? "Attached" : "Process Running";
					//objectViewerMenu.Enabled = false;
					break;

				case DebuggerState.DebuggeeThrewException:
					runButton.Enabled = true;
                    buttonCommand34.Enabled = true;
					pauseButton.Enabled = false;
					stopButton.Enabled = true;
					runToCursorButton.Enabled = true;
					stepOverButtonCommand.Enabled = true;
					stepIntoButtonCommand.Enabled = true;
					stepOutButtonCommand.Enabled = true;
                    uiStatusBar1.Panels[2].Text = "Exception";
					break;
			}
		}

        private void ShowDebuggeeStoppedState()
        {
            buttonCommand34.Enabled = runButton.Enabled = IsStartupExecutableSpecified();            
            pauseButton.Enabled = false;
            stopButton.Enabled = false;
            runToCursorButton.Enabled = runButton.Enabled;
            stepOverButtonCommand.Enabled = runButton.Enabled;
            stepIntoButtonCommand.Enabled = runButton.Enabled;
            stepOutButtonCommand.Enabled = runButton.Enabled;
            uiStatusBar1.Panels[2].Text = detach ? "Detached" : "Process Stopped";
        }

        private static bool IsStartupExecutableSpecified()
        {
            bool result = false;

            switch (NuGenProject.Instance.StartMode)
            {
                case ProjectStartMode.StartAssembly:
                    result = (NuGenProject.Instance.StartupAssembly != null);
                    break;

                case ProjectStartMode.StartProgram:
                    result = !string.IsNullOrEmpty(NuGenProject.Instance.ProgramExecutable);
                    break;
            }
            return result;
        }

        void ribbon1_SelectedTabChanged(object sender, Janus.Windows.Ribbon.TabEventArgs e)
        {
            uiTab1.SelectedTab = GetTab(e.Tab);
        }

        public UITabPage GetTab(Janus.Windows.Ribbon.RibbonTab tab)
        {
            switch (tab.Name)
            {
                case "homeRibbonTab": return homeTab;
                case "ilRibbonTab": return ilTab;                
                case "eventLogRibbonTab": return eventLogTab;
                case "metersRibbonTab": return metersTab;
            }

            return null;
        }
        
        public NuGenProjectExplorer ProjectExplorer
        {
            get
            {
                return projectExplorer;
            }
        }

        private NuGenProject projectToLoad;
        public NuGenProject ProjectToLoad
        {
            get
            {
                return projectToLoad;
            }

            set
            {
                projectToLoad = value;
            }
        }

        public void OpenProject(string projectFilePath, bool addToConfiguration)
        {
            NuGenProject.Instance.Assemblies.Clear();
            projectExplorer.ProjectElements.Nodes.Clear();

            try
            {
                using (Stream stream = File.Open(projectFilePath, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    NuGenProject project = (NuGenProject)bformatter.Deserialize(stream);
                    stream.Close();

                    string[] fileNames = new string[project.AssembliesStrings.Count];

                    for (int index = 0; index < project.AssembliesStrings.Count; index++)
                    {
                        fileNames[index] = project.AssembliesStrings[index];
                    }

                    NuGenProject.Instance.Name = project.Name;
                    ilEditor1.AddAssembly(fileNames);

                    NuGenAssemblyLoader.Instance.Start(fileNames);

                    if (addToConfiguration)
                    {
                        NuGenSettings.Instance.AddProject(projectFilePath);
                    }
                }
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.InnerException is XmlException)
                {
                    //informationPanel.AddInformation(string.Format("'{0}' is not a valid xml file.\n", projectFilePath));
                }
                else
                {
                    //informationPanel.AddInformation(string.Format("Could not load '{0}' file. (exception message: '{1}')\n", projectFilePath, invalidOperationException.Message));
                }

                ProjectToLoad = null;
            }
            catch (Exception)
            {
                //informationPanel.AddInformation(string.Format("Could not load '{0}' file. (exception message: '{1}')\n", projectFilePath, exception.Message));

                ProjectToLoad = null;
            }
        }

        private string[] assembliesToLoad;
        public string[] AssembliesToLoad
        {
            get
            {
                return assembliesToLoad;
            }

            set
            {
                assembliesToLoad = value;
            }
        }
        

        private void newAssemblyButton_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {                
                NuGenAssemblyLoader.Instance.Start(ofd.FileNames);                
            }         
        }

        private void regMonClick(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {                        
            NuGenRegistryMonitorMainForm f = new NuGenRegistryMonitorMainForm();
            AddTabbedForm("Registry Monitor", f);
        }

        #region ILEditor Stuff

        private void runButtonClicked(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            attach = false;
            ilEditor1.RunDebuggee();            
        }

        private void stepIntoClicked(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            if (NuGenDebugEventHandler.Instance.State == DebuggerState.DebuggeeStopped)
            {
                this.ilEditor1.SetBreakpointOnFirstInstruction();
            }
            else
            {
                NuGenBreakpointHandler.Instance.Step(StepType.StepIn);
            }
        }

        private void stepOverClicked(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            if (NuGenDebugEventHandler.Instance.State == DebuggerState.DebuggeeStopped)
            {
                ilEditor1.SetBreakpointOnFirstInstruction();
            }
            else
            {
                NuGenBreakpointHandler.Instance.Step(StepType.StepOver);
            }
        }

        private void stepOutClicked(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            if (NuGenDebugEventHandler.Instance.State == DebuggerState.DebuggeeStopped)
            {
                ilEditor1.SetBreakpointOnFirstInstruction();
            }
            else
            {
                NuGenBreakpointHandler.Instance.Step(StepType.StepOut);
            }
        }

        private bool detach;
        private void detachClick(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            detach = true;
            NuGenDebugEventHandler.Instance.Detach();
        }

        private void stopClicked(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            detach = false;
            StopDebuggee();
        }

        private void StopDebuggee()
        {
            try
            {
                NuGenDebugEventHandler.Instance.RefreshAndSuspendProcess();
                NuGenDebugEventHandler.Instance.EventObjects.Process.Stop();
            }
            catch (Exception)
            {
                //ShowException(exception);
            }

            ShowDebuggeeStoppedState();
        }

        private void pauseClicked(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            PauseDebuggee();
        }

        private void PauseDebuggee()
        {
            try
            {
                NuGenDebugEventHandler.Instance.RefreshAndSuspendProcess();
                List<ThreadWrapper> debugeeThreads = NuGenDebugEventHandler.Instance.EventObjects.Controller.EnumerateThreads();

                if (debugeeThreads.Count > 0)
                {
                    NuGenDebugEventHandler.Instance.EventObjects.Thread = debugeeThreads[0];

                    NuGenDebugEventHandler.Instance.DisplayAllInformation(DebuggerState.DebuggeePaused);
                }
            }
            catch (Exception)
            {
                //ShowException(exception);
            }
        }

        private void toggleBreakpointClicked(object sender, CommandEventArgs e)
        {
            NuGenCodeEditorForm codeEditor = ilEditor1.ActiveCodeEditor as NuGenCodeEditorForm;

            if (codeEditor != null)
            {
                codeEditor.SetBreakpointAtSelection();
            }
        }

        private void runToCursorClicked(object sender, CommandEventArgs e)
        {
            attach = false;
            NuGenCodeEditorForm codeEditor = ilEditor1.ActiveCodeEditor as NuGenCodeEditorForm;

            if (codeEditor != null)
            {
                codeEditor.SetRunToCursorAtSelection();
                ilEditor1.RunDebuggee();
            }            
        }

        private bool attach;
        private void AttachButtonClicked(object sender, CommandEventArgs e)
        {
            attach = true;
            ilEditor1.AttachToProcess();
        }
        #endregion

        private void comboBoxCommand1_ComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            if (comboBoxCommand1.ComboBox.SelectedItem.Text.Equals("Project"))
            {
                String proj = ProjectInstance();

                if (proj != null)
                {
                    realTimePanel1.Instance = proj;
                }
            }
            else
            {
                realTimePanel1.Instance = comboBoxCommand1.ComboBox.SelectedItem.Text;
            }
        }

        private String ProjectInstance()
        {
            if (NuGenProject.Instance.StartupAssembly == null)
                return null;

            if(Process.GetProcessesByName("NuGenProject.Instance.StartupAssembly").Length == 0)
            {
                DialogResult res = MessageBox.Show("Project process not executing.  Would you like to run it now?", "Project", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    Process proc = new Process();
                    proc.StartInfo = new ProcessStartInfo(NuGenProject.Instance.StartupAssembly.FullPath);
                    proc.Start();
                }
                else
                {
                    return "";
                }
            }
            
            return NuGenProject.Instance.StartupAssembly.Name;
        }

        private void buttonCommand25_Click(object sender, CommandEventArgs e)
        {
            realTimePanel1.ShowHome();
        }

        private void comboBoxCommand2_Click(object sender, EventArgs e)
        {            
            comboBoxCommand2.ComboBox.Items.Clear();

            foreach (String instance in realTimePanel1.GraphInstances)
            {
                comboBoxCommand2.ComboBox.Items.Add(instance);
            }
        }

        private void comboBoxCommand2_ComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            realTimePanel1.GraphInstance = comboBoxCommand2.ComboBox.SelectedItem.Text;
        }

        private void comboBoxCommand1_ComboBox_MouseEnter(object sender, EventArgs e)
        {
            PopulateComboBox();
        }

        private void buttonCommand29_Click(object sender, CommandEventArgs e)
        {
            nuGenEventMonitorPanel1.OpenClicked();
        }

        private void buttonCommand28_Click(object sender, CommandEventArgs e)
        {
            nuGenEventMonitorPanel1.NewClicked();
        }

        private void buttonCommand30_Click(object sender, CommandEventArgs e)
        {
            nuGenEventMonitorPanel1.SaveClicked();
        }

        private void buttonCommand26_Click(object sender, CommandEventArgs e)
        {
            nuGenEventMonitorPanel1.ConnectClicked();
        }

        private void buttonCommand27_Click(object sender, CommandEventArgs e)
        {
            nuGenEventMonitorPanel1.OptionsClicked();
        }

        private IDictionary<String, Form> tabToFormMap = new Dictionary<String, Form>();

        private void buttonCommand7_Click(object sender, CommandEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            GetHardwareInfo.WMIMainForm form = new GetHardwareInfo.WMIMainForm();
            AddTabbedForm("Hardware Information", form);
            Cursor = Cursors.Default;
        }

        private void AddTabbedForm(String name, Form form)
        {
            int nPages = 0;

            foreach (UITabPage pg in uiTab2.TabPages)
            {
                if (pg.Text.StartsWith(name))
                {
                    nPages++;
                }
            }

            if (nPages > 0)
            {
                name = name +" View - " + (nPages + 1).ToString();
            }

            if (tabToFormMap.ContainsKey(name))
            {
                tabToFormMap[name].Dispose();
                tabToFormMap.Remove(name);
            }

            tabToFormMap[name] = form;

            //form.Show();
            UITabPage page = new UITabPage(name);            
            Control[] ctrls = new Control[tabToFormMap[name].Controls.Count];
            tabToFormMap[name].Controls.CopyTo(ctrls, 0);

            page.Controls.AddRange(ctrls);

            uiTab2.TabPages.Add(page);
            uiTab2.SelectedTab = page;
        }

        private void buttonCommand5_Click(object sender, CommandEventArgs e)
        {
            Genetibase.Debug.NuGenPathWatcherMainForm form = new Genetibase.Debug.NuGenPathWatcherMainForm();
            AddTabbedForm("Path Watcher", form);
        }

        private void buttonCommand9_Click(object sender, CommandEventArgs e)
        {
            Genetibase.Debug.frmProcess form = new Genetibase.Debug.frmProcess();
            AddTabbedForm("PExplore", form);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewProject();
        }

        private void NewProject()
        {
            if (projectExplorer.ProjectElements.Nodes.Count != 0)
            {
                DialogResult res = MessageBox.Show("Would you like to save the current project?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    SaveProject();
                }
                else if (res == DialogResult.Cancel)
                {
                    return;
                }
            }

            ProjectNameForm form = new ProjectNameForm();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();


            projectExplorer.ProjectElements.Nodes.Clear();

            //NuGenProject.Instance = new NuGenProject();
            NuGenProject.Instance.Name = form.ProjectName;
            NuGenProject.Instance.Assemblies.Clear();
        }

        private void buttonCommand4_Click(object sender, CommandEventArgs e)
        {
            SaveProject();
        }

        private void SaveProject()
        {
            if (NuGenProject.Instance.Name == null)
            {
                return;
            }

            if (File.Exists(NuGenProject.Instance.Name + ".ngt"))
            {
                DialogResult res = MessageBox.Show("Overwrite existing file?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (res == DialogResult.No)
                {
                    return;
                }                
            }

            Stream stream = File.Open(NuGenProject.Instance.Name + ".ngt", FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, NuGenProject.Instance);
            stream.Close();

            if (!Properties.Settings.Default.RecentProjects.Contains(Directory.GetCurrentDirectory() + "\\" + NuGenProject.Instance.Name + ".ngt"))
            {
                Properties.Settings.Default.RecentProjects.Add(Directory.GetCurrentDirectory() + "\\" + NuGenProject.Instance.Name + ".ngt");
                AddRecent(Directory.GetCurrentDirectory() + "\\" + NuGenProject.Instance.Name + ".ngt");
            }
        }

        private void buttonCommand3_Click(object sender, CommandEventArgs e)
        {
            NewProject();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenProject();
        }

        private void OpenProject()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() != DialogResult.Cancel)
            {
                OpenProject(dlg.FileName, true);
            
                if(!Properties.Settings.Default.RecentProjects.Contains(dlg.FileName))
                {
                    Properties.Settings.Default.RecentProjects.Add(dlg.FileName);
                    AddRecent(dlg.FileName);                    
                }
            }
        }

        private void AddRecent(string filename)
        {
            LinkLabel label = new LinkLabel();
            label.Text =  filename.Substring(filename.LastIndexOf("\\") + 1).Replace(".ngt", "").Replace("NGT", "");
            label.Click += new EventHandler(delegate(object o, EventArgs e)
            {
                if (projectExplorer.ProjectElements.Nodes.Count != 0)
                {
                    DialogResult res = MessageBox.Show("Would you like to save the current project?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (res == DialogResult.Yes)
                    {
                        SaveProject();
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                if(!File.Exists(filename))
                {
                    DialogResult res = MessageBox.Show("Could not find the file " + filename + "\nWould you like to remove it from the list?", "File missing", MessageBoxButtons.YesNo);
                    if(res == DialogResult.Yes)
                    {
                        Properties.Settings.Default.RecentProjects.Remove(filename);
                        panel5.Controls.Remove(label);
                    }

                    return;
                }

                OpenProject(filename, true);
            });

            if (panel5.Controls.Count == 12)
            {
                panel5.Controls.RemoveAt(0);
                Properties.Settings.Default.RecentProjects.RemoveAt(0);
            }

            panel5.Controls.Add(label);
            label.Dock = DockStyle.Top;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uiTab2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && uiTab2.SelectedTab.Text != "Welcome")
            {
                uiTab2.TabPages.Remove(uiTab2.SelectedTab);
            }
            else if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(uiTab2.PointToScreen(e.Location));
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uiTab2.TabPages.Remove(uiTab2.SelectedTab);
        }

        private void openSeparateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabToFormMap[uiTab2.SelectedTab.Text].Controls.Clear();

            Control[] ctrls = new Control[uiTab2.SelectedTab.Controls.Count];
            uiTab2.SelectedTab.Controls.CopyTo(ctrls, 0);

            tabToFormMap[uiTab2.SelectedTab.Text].Controls.AddRange(ctrls);
            tabToFormMap[uiTab2.SelectedTab.Text].Show();

            uiTab2.SelectedTab.Controls.Clear();
            uiTab2.TabPages.Remove(uiTab2.SelectedTab);
        }

        private void buttonCommand11_Click(object sender, CommandEventArgs e)
        {
            Dile.Configuration.UI.NuGenSettingsDialog settingsDialog = new Dile.Configuration.UI.NuGenSettingsDialog();

            if (settingsDialog.DisplaySettings() == DialogResult.OK)
            {
                Dile.Configuration.NuGenSettings.SaveConfiguration();                
            }
        }

        private void buttonCommand12_Click(object sender, CommandEventArgs e)
        {
            NuGenProjectProperties properties = new NuGenProjectProperties();

            if (properties.DisplaySettings() == DialogResult.OK)
            {
                projectExplorer.ProjectElements.Nodes[0].Text = NuGenHelperFunctions.TruncateText(NuGenProject.Instance.Name);
                NuGenUIHandler.Instance.ShowDebuggerState(DebuggerState.DebuggeeStopped);
            }
        }

        private void buttonCommand13_Click(object sender, CommandEventArgs e)
        {
            ilEditor1.CloseAll();
        }

        private void buttonCommand14_Click(object sender, CommandEventArgs e)
        {
            ilEditor1.WordWrap = !ilEditor1.WordWrap;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label9.Text = ProjectExplorer.StartupAssembly;

            PerformanceCounter counter;
            counter = new PerformanceCounter("System", "Processes");
            this.label10.Text = counter.NextValue().ToString();

            if (label9.Text == "Not Set")
            {                
                label13.Text = "N/A";
                label14.Text = "N/A";
            }
            else
            {
                try
                {
                    counter = new PerformanceCounter("Process", "% Processor Time");
                    counter.InstanceName = ProjectExplorer.StartupAssembly.Substring(0, ProjectExplorer.StartupAssembly.IndexOf("."));
                    this.label13.Text = counter.NextValue().ToString();

                    counter = new PerformanceCounter("Process", "Working Set");
                    counter.InstanceName = ProjectExplorer.StartupAssembly.Substring(0, ProjectExplorer.StartupAssembly.IndexOf("."));
                    this.label14.Text = counter.NextValue().ToString();
                }
                catch
                {
                    label13.Text = "N/A";
                    label14.Text = "N/A";
                }
            }
        }

        private void uiTab2_ClosingTab(object sender, Janus.Windows.UI.Tab.TabCancelEventArgs e)
        {
            if (e.Page.Text == "Welcome")
            {
                e.Cancel = true;
            }
        }

        private void exit_click(object sender, CommandEventArgs e)
        {
            Application.Exit();
        }

        private void dropDownCommand10_Click(object sender, CommandEventArgs e)
        {
            GetHardwareInfo.WMIMainForm form = new GetHardwareInfo.WMIMainForm();
            form.Show();
        }

        private void dropDownCommand11_Click(object sender, CommandEventArgs e)
        {
            NuGenPathWatcherMainForm form = new NuGenPathWatcherMainForm();
            form.Show();
        }

        private void dropDownCommand12_Click(object sender, CommandEventArgs e)
        {
            frmProcess form = new frmProcess();
            form.Show();
        }

        private void dropDownCommand13_Click(object sender, CommandEventArgs e)
        {
            NuGenRegistryMonitorMainForm form = new NuGenRegistryMonitorMainForm();
            form.Show();
        }

        private void NuGenInSightMainForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            detachClick(sender, null);
        }
    }
}