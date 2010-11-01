using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dile.Disassemble;
using Dile.UI.Debug;
using Dile.Debug;
using Dile.UI;
using System.IO;
using Janus.Windows.UI.Tab;

namespace NuGenInSightCompilation.UIAdapters
{
    [System.Diagnostics.DebuggerDisplay("ILEditor")]
    public partial class ILEditor : NuGenBasePanel
    {
        public delegate void SwitchTabsDelegate();
        public SwitchTabsDelegate SwitchTabs;

        private void CodeEditorAdded(NuGenCodeEditorForm displayer)
        {
            UITabPage newPage = new UITabPage();
            newPage.Name = displayer.CodeObject.HeaderText;                        
            newPage.Text = displayer.CodeObject.HeaderText;
            newPage.Controls.Add(displayer);
            displayer.Dock = DockStyle.Fill;
            uiTab1.TabPages.Add(newPage);
            uiTab1.SelectedTab = newPage;

            if (SwitchTabs != null)
            {
                SwitchTabs();
            }
            //UITabPage test = new UITabPage("Test");
            //uiTab1.TabPages.Add(test);            
        }

        public ILEditor()
        {
            InitializeComponent();
            uiTab1.ShowCloseButton = true;

            NuGenProject.Instance = new NuGenProject();
            NuGenProject.Instance.IsSaved = true;
            NuGenProject.Instance.Name = "New project";

            NuGenDebugEventHandler.Instance.StateChanged += new DebuggerStateChanged(Instance_StateChanged);
        }

        void Instance_StateChanged(DebuggerState oldState, DebuggerState newState)
        {
            NuGenUIHandler.Instance.ShowDebuggerState(newState);
        }

        public void CloseAll()
        {
            uiTab1.TabPages.Clear();
        }

        private Janus.Windows.UI.StatusBar.UIStatusBarPanel progressBar;
        private Janus.Windows.UI.StatusBar.UIStatusBarPanel readyLabel;

        public Janus.Windows.UI.StatusBar.UIStatusBarPanel ProgressBar
        {
            get
            {
                return progressBar;
            }

            set
            {
                progressBar = value;
            }
        }

        private bool wordWrap = true;
        public bool WordWrap
        {
            get
            {
                return wordWrap;
            }

            set
            {
                wordWrap = value;

                if (uiTab1.SelectedTab != null)
                {
                    foreach (Control c in uiTab1.SelectedTab.Controls)
                    {
                        if (c is NuGenCodeEditorForm)
                        {
                            ((NuGenCodeEditorForm)c).SetWordWrap(wordWrap);
                        }
                    }
                }
            }
        }

        public Janus.Windows.UI.StatusBar.UIStatusBarPanel ReadyLabel
        {
            get
            {
                return readyLabel;
            }

            set
            {
                readyLabel = value;
            }
        }

        private Janus.Windows.UI.StatusBar.UIStatusBar statusBar;

        public Janus.Windows.UI.StatusBar.UIStatusBar StatusBar
        {
            get
            {
                return statusBar;
            }

            set
            {
                statusBar = value;

                try
                {
                    progressBar = statusBar.Panels[0] as Janus.Windows.UI.StatusBar.UIStatusBarPanel;
                    readyLabel = statusBar.Panels[1] as Janus.Windows.UI.StatusBar.UIStatusBarPanel;
                }
                catch (Exception)
                {
                    System.Console.Error.Write("Tried to set the status bar to an improper type");
                }
            }
        }

        protected override bool IsDebugPanel()
        {
            return true;
        }

        private ExtendedDialogResult loadUnloadedAssembly;
        public ExtendedDialogResult LoadUnloadedAssembly
        {
            get
            {
                return loadUnloadedAssembly;
            }

            set
            {
                loadUnloadedAssembly = value;
            }
        }

        public NuGenCodeEditorForm ActiveCodeEditor
        {
            get
            {
                return uiTab1.SelectedTab.Controls[0] as NuGenCodeEditorForm;
            }
        }

        delegate void ShowDebuggerStateDelegate(DebuggerState state);

        protected override void debugEventHandler_ProcessExited()
        {
            base.debugEventHandler_ProcessExited();

            if (this.InvokeRequired)
            {
                ShowDebuggerStateDelegate del = this.ShowDebuggerState;
                this.Invoke(del, new object[] { DebuggerState.DebuggeeStopped });
            }
            else
            {
                ShowDebuggerState(DebuggerState.DebuggeeStopped);
            }
        }

        public void AttachToProcess()
        {
            NuGenAttachProcessDialog attachProcessDialog = new NuGenAttachProcessDialog();

            if (attachProcessDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.quickSearchPanel1.ClearPanel();
                    ClearCodeDisplayers(true);
                    NuGenBreakpointHandler.Instance.DeleteRemovedBreakpoints();
                    System.Diagnostics.Process debuggeeProcess = System.Diagnostics.Process.GetProcessById(Convert.ToInt32(attachProcessDialog.ProcessID));
                    string frameworkVersion = Debugger.GetVersionFromProcess(debuggeeProcess.Handle);

                    NuGenDebugEventHandler.Instance.Debugger.Initialize(NuGenDebugEventHandler.Instance, frameworkVersion);
                    NuGenDebugEventHandler.Instance.Debugger.DebugActiveProcess(attachProcessDialog.ProcessID, 0);
                    DisplayLogMessage("\n---------------Attached to debuggee--------------------\n");
                }
                catch (Exception exception)
                {
                    ShowException(exception);
                    DisplayUserWarning(exception.Message);
                }
            }
        }

        public void RunDebuggee()
        {
            if (NuGenDebugEventHandler.Instance.State != DebuggerState.DebuggeeStopped)
            {
                ClearDebugPanels(false);
                ClearCodeDisplayers(true);
                NuGenDebugEventHandler.Instance.ContinueProcess();
            }
            else if (!IsStartupExecutableSpecified())
            {
                DisplayUserWarning("No startup assembly/program is specified.");
            }
            else
            {
                try
                {
                    DisplayLogMessage("\n---------------Debuggee started--------------------\n");
                    LoadUnloadedAssembly = ExtendedDialogResult.None;
                    ClearDebugPanels(false);
                    ClearCodeDisplayers(true);
                    string executable = string.Empty;
                    string arguments = string.Empty;
                    string workingDirectory = string.Empty;

                    switch (NuGenProject.Instance.StartMode)
                    {
                        case ProjectStartMode.StartAssembly:
                            executable = NuGenProject.Instance.StartupAssembly.FullPath;
                            arguments = NuGenProject.Instance.AssemblyArguments;
                            workingDirectory = NuGenProject.Instance.AssemblyWorkingDirectory;
                            break;

                        case ProjectStartMode.StartProgram:
                            executable = NuGenProject.Instance.ProgramExecutable;
                            arguments = NuGenProject.Instance.ProgramArguments;
                            workingDirectory = NuGenProject.Instance.ProgramWorkingDirectory;
                            break;
                    }

                    if (string.IsNullOrEmpty(workingDirectory))
                    {
                        workingDirectory = Path.GetDirectoryName(executable);
                    }

                    if (string.IsNullOrEmpty(arguments))
                    {
                        arguments = string.Format("\"{0}\"", executable);
                    }
                    else
                    {
                        arguments = string.Format("\"{0}\" {1}", executable, arguments);
                    }

                    NuGenBreakpointHandler.Instance.DeleteRemovedBreakpoints();
                    string frameworkVersion = Debugger.GetRequestedRuntimeVersion(executable);

                    NuGenDebugEventHandler.Instance.Debugger.Initialize(NuGenDebugEventHandler.Instance, frameworkVersion);
                    NuGenDebugEventHandler.Instance.Debugger.CreateProcessA(executable, arguments, workingDirectory);                    
                }
                catch (Exception exception)
                {
                    ShowException(exception);
                    DisplayUserWarning("Exception occurred while trying to start the debuggee.");
                }
            }
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

        private void InitializeEvents()
        {
            NuGenUIHandler.Instance.AssembliesLoadedMethod = this.AssembliesLoaded;
            NuGenUIHandler.Instance.StepProgressBarMethod = this.StepProgressBar;
            NuGenUIHandler.Instance.SetProgressTextMethod = this.SetProgressText;
            NuGenUIHandler.Instance.SetProgressBarMaximumMethod = this.SetProgressBarMaximum;
		    NuGenUIHandler.Instance.ResetProgressBarMethod = this.ResetProgressBar;
		    NuGenUIHandler.Instance.ShowExceptionMethod = this.ShowException;
		    NuGenUIHandler.Instance.SetProgressBarVisibleMethod = this.SetProgressBarVisible;
		    NuGenUIHandler.Instance.DisplayOutputInformationMethod = this.DisplayOutputInformation;
		    NuGenUIHandler.Instance.DisplayLogMessageMethod = this.DisplayLogMessage;
		    NuGenUIHandler.Instance.ClearDebugPanelsMethod = this.ClearDebugPanels;
		    NuGenUIHandler.Instance.ClearOutputPanelMethod = this.ClearOutputPanel;
		    NuGenUIHandler.Instance.ShowCodeObjectMethod = this.ProjectExplorer.ShowCodeObject;
		    //UIHandler.Instance.ShowLocalVariablesMethod = this.ShowLocalVariables;
		    //UIHandler.Instance.ShowArgumentsMethod = this.ShowArguments;
		    //UIHandler.Instance.ShowAutoObjectMethod = this.ShowAutoObject;
		    NuGenUIHandler.Instance.ShowObjectInObjectViewerMethod = this.ShowObjectInObjectViewer;
		    NuGenUIHandler.Instance.ClearCodeDisplayersMethod = this.ClearCodeDisplayers;
		    NuGenUIHandler.Instance.FrameChangedUpdateMethod = this.FrameChangedUpdate;
		    NuGenUIHandler.Instance.DisplayUserWarningMethod = this.DisplayUserWarning;
		    NuGenUIHandler.Instance.ClearUserWarningMethod = this.ClearUserWarning;
		    NuGenUIHandler.Instance.ShowDebuggerStateMethod = this.ShowDebuggerState;
		    NuGenUIHandler.Instance.AddBreakpointMethod = this.AddBreakpoint;
		    NuGenUIHandler.Instance.RemoveBreakpointMethod = this.RemoveBreakpoint;
		    NuGenUIHandler.Instance.DeactivateBreakpointMethod = this.DeactivateBreakpoint;
		    NuGenUIHandler.Instance.UpdateBreakpointMethod = this.UpdateBreakpoint;
		    NuGenUIHandler.Instance.ShowAssemblyMissingWarningMethod = this.ShowAssemblyMissingWarning;
		    NuGenUIHandler.Instance.AddAssemblyMethod = this.AddAssembly;
		    NuGenUIHandler.Instance.CodeEditorActivatedMethod = this.CodeEditorActivated;
		    NuGenUIHandler.Instance.ShowMessageBoxMethod = this.ShowMessageBox;
		    NuGenUIHandler.Instance.RemoveUnnecessaryAssembliesMethod = this.RemoveUnnecessaryAssemblies;
		    NuGenUIHandler.Instance.CloseDynamicModuleDocumentsMethod = this.CloseDynamicModuleDocuments;
		    NuGenUIHandler.Instance.AddModuleToPanelMethod = this.AddModuleToPanel;
		    NuGenUIHandler.Instance.AddModulesToPanelMethod = this.AddModulesToPanel;		                    
        }

        private NuGenProjectExplorer explorer;
        public NuGenProjectExplorer ProjectExplorer
        {
            get
            {
                return explorer;
            }

            set
            {
                if (value == null)
                    return;

                explorer = value;                
                explorer.CodeEditorAdded = this.CodeEditorAdded;
                NuGenUIHandler.Instance.Callback = this;
                InitializeEvents();
            }
        }

        public delegate void DebuggerStateChangedDelegate(DebuggerState newState);
        private DebuggerStateChangedDelegate debuggerStateChanged;
        public DebuggerStateChangedDelegate DebuggerStateChanged
        {
            get
            {
                return debuggerStateChanged;
            }

            set
            {
                debuggerStateChanged = value;
            }
        }

        private void ResetPanels()
        {
            threadsPanel.Reset();
        }

        #region Callback Methods

        public void CodeEditorActivated(NuGenCodeEditorForm codeEditor)
        {
        }

        public void RemoveUnnecessaryAssemblies()
        {
            explorer.RemoveUnnecessaryAssemblies();
        }

        public void AssembliesLoaded(List<NuGenAssembly> loadedAssemblies, bool isProjectChanged)
        {
            informationPanel.AddElapsedTime();            
            informationPanel.AddInformation(readyLabel.Text);
            informationPanel.ResetCounter();
            //Application.DoEvents();

            foreach (NuGenAssembly assembly in loadedAssemblies)
            {
                ProjectExplorer.AddAssemblyToProject(assembly);
            }

            informationPanel.AddElapsedTime();
            ClearUserWarning();
            informationPanel.AddInformation(readyLabel.Text);
            informationPanel.AddInformation("\n\n");
            ResetPanels();

            if (NuGenDebugEventHandler.Instance.State == DebuggerState.DebuggeeStopped)
            {
              ShowDebuggerState(DebuggerState.DebuggeeStopped);              
            }
            else
            {
              ShowDebuggerState(NuGenDebugEventHandler.Instance.State);
              ClearDebugPanels(false);
              NuGenDebugEventHandler.Instance.DisplayCurrentCodeLocation();
            }

            GC.Collect(GC.MaxGeneration);

            if (isProjectChanged)
            {
              NuGenProject.Instance.IsSaved = false;
            }
        }

        public void StepProgressBar(int incrementValue)
        {
            progressBar.ProgressBarValue += incrementValue;            
        }

        public void SetProgressText(string text, bool addElapsedTime)
        {
            if (addElapsedTime)
            {
                informationPanel.AddElapsedTime();
                readyLabel.Text = text;
            }

            informationPanel.ResetCounter();
            informationPanel.AddInformation(text);
        }

        public void ResetProgressBar()
        {
            progressBar.ProgressBarValue = 0;
            informationPanel.Counter = null;
        }

        public void ShowException(Exception exception)
        {
            informationPanel.ResetCounter();
            informationPanel.AddException(exception);
            DisplayUserWarning("Exception occurred in DILE.");
        }

        public void SetProgressBarVisible(bool visible)
        {
            progressBar.Visible = visible;
        }

        public void DisplayOutputInformation(NuGenDebugEventDescriptor debugEventDescriptor)
        {
            outputPanel.AddEvent(debugEventDescriptor);
        }

        public void DisplayLogMessage(string logMessage)
        {
            //logMessagePanel.AddInformation(logMessage);
        }

        public void ClearDebugPanels(bool leaveThreads)
        {
            //ClearSpecialLines();
            ClearUserWarning();
        }

        public void ClearOutputPanel()
        {
            outputPanel.ClearPanel();
        }

        private NuGenObjectViewer objectViewer;

        public void ShowObjectInObjectViewer(FrameWrapper frame, NuGenBaseValueRefresher valueRefresher, string initialExpression)
        {
            if (objectViewer == null)
            {
            objectViewer = new NuGenObjectViewer();
            }

            objectViewer.Initialize();
            objectViewer.ShowValue(valueRefresher, frame, initialExpression);
        }

        public void ClearCodeDisplayers(bool refreshDisplayers)
        {
            foreach (NuGenCodeDisplayer codeDisplayer in ProjectExplorer.GetCodeDisplayers())
            {
                codeDisplayer.ClearCurrentLine();
                codeDisplayer.ClearSpecialLines();

                if (refreshDisplayers)
                {
                    codeDisplayer.Refresh();
                }
            }
        }

        public void FrameChangedUpdate(NuGenFrameRefresher frameRefresher, FrameWrapper frame, bool calledByWatchPanel)
        {
            NuGenDebugEventHandler.Instance.EventObjects.Frame = frame;
            NuGenDebugEventHandler.Instance.ChangeFrame(frameRefresher, NuGenDebugEventHandler.Instance.EventObjects.Frame);
        }

        public void DisplayUserWarning(string text)
        {            
            readyLabel.Text = text;
        }

        public void ClearUserWarning()
        {
            readyLabel.Text = "Ready";
        }

        public void ShowDebuggerState(DebuggerState newState)
        {
            DebuggerStateChanged(newState);
        }

        public void AddBreakpoint(NuGenBreakpointInformation breakpoint)
        {
            breakpointsPanel.AddBreakpoint(breakpoint);
        }

        public void RemoveBreakpoint(NuGenBreakpointInformation breakpoint)
        {
            breakpointsPanel.RemoveBreakpoint(breakpoint);
        }

        public void DeactivateBreakpoint(NuGenBreakpointInformation breakpoint)
        {
            breakpointsPanel.DeactivateBreakpoint(breakpoint);
        }

        public void UpdateBreakpoint(NuGenIMultiLine codeObject, NuGenBreakpointInformation breakpointInformation)
        {
            
        }

        public void ShowAssemblyMissingWarning(string assemblyPath)
        {
            if (!NuGenProject.Instance.IsAssemblyLoaded(assemblyPath))
            {
                bool loadAssembly = false;

                 if (LoadUnloadedAssembly == ExtendedDialogResult.YesToAll)
                 {
                     loadAssembly = true;
                 }
                 else if (LoadUnloadedAssembly != ExtendedDialogResult.NoToAll)
                 {
                     string message = string.Format("The following assembly is not loaded: {0}\r\n\r\nWould you like to load it now?\r\n\r\nNote: if an assembly is not loaded in DILE then during the debugging you might not see correct call stack/object values as DILE will have not have enough information about the debuggee.", assemblyPath);

                     NuGenExtendedMessageBox messageBox = new NuGenExtendedMessageBox();
                     messageBox.ShowMessage("DILE - Load assembly?", message);
                     LoadUnloadedAssembly = messageBox.ExtendedDialogResult;

                     if (LoadUnloadedAssembly == ExtendedDialogResult.Yes || LoadUnloadedAssembly == ExtendedDialogResult.YesToAll)
                     {
                         loadAssembly = true;
                     }
                 }

                if (loadAssembly)
                {
                    NuGenAssemblyLoader.Instance.Start(new string[] { assemblyPath });
                }
            }
        }

        public void AddAssembly(string[] fileNames)
        {
            ClearUserWarning();
            readyLabel.Text = string.Empty;
            progressBar.Visible = true;
            NuGenAssemblyLoader.Instance.Start(fileNames);
        }

        public void ShowMessageBox(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void CloseDynamicModuleDocuments()
        {
            /*
            while (index < ActiveCodeEditors.Count)
            {
                CodeEditorForm codeEditorForm = ActiveCodeEditors[index];

                if (codeEditorForm.CodeObject.IsInMemory)
                {
                    codeEditorForm.Hide();
                    codeEditorForm.Dispose();
                }
                else
                {
                    index++;
                }
            }*/
        }

        public void AddModuleToPanel(ModuleWrapper module)
        {            
            modulesPanel.AddModule(module);
        }

        public void AddModulesToPanel(ModuleWrapper[] modules)
        {
            modulesPanel.AddModules(modules);
        }

        public void SetProgressBarMaximum(int maximum)
        {
            progressBar.ProgressBarMaxValue = maximum;
        }

        #endregion

        public void SetBreakpointOnFirstInstruction()
        {
            if (NuGenProject.Instance.StartupAssembly != null)
            {
                bool entryMethodNotFound = false;

                if (NuGenProject.Instance.StartupAssembly.AllTokens.ContainsKey(NuGenProject.Instance.StartupAssembly.EntryPointToken))
                {
                    NuGenMethodDefinition entryMethod = NuGenProject.Instance.StartupAssembly.AllTokens[NuGenProject.Instance.StartupAssembly.EntryPointToken] as NuGenMethodDefinition;

                    if (entryMethod == null)
                    {
                        entryMethodNotFound = true;
                    }
                    else
                    {
                        NuGenBreakpointHandler.Instance.RunToCursor(entryMethod, 0, false);
                        RunDebuggee();
                    }
                }
                else
                {
                    entryMethodNotFound = true;
                }

                if (entryMethodNotFound)
                {
                    NuGenUIHandler.Instance.DisplayUserWarning("The entry method is not found in the startup assembly.");
                }
            }
        }

        public void AttachToProfiler(int processID)
        {
            this.quickSearchPanel1.ClearPanel();
            ClearCodeDisplayers(true);
            NuGenBreakpointHandler.Instance.DeleteRemovedBreakpoints();
            System.Diagnostics.Process debuggeeProcess = System.Diagnostics.Process.GetProcessById(processID);
            string frameworkVersion = Debugger.GetVersionFromProcess(debuggeeProcess.Handle);

            NuGenDebugEventHandler.Instance.Debugger.Initialize(NuGenDebugEventHandler.Instance, frameworkVersion);
            NuGenDebugEventHandler.Instance.Debugger.DebugActiveProcess((uint)processID, 0);
            DisplayLogMessage("\n---------------Attached to debuggee--------------------\n");
        }
    }
}
