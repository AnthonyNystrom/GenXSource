using System;
using System.Collections.Generic;
using System.Text;

using Dile.Controls;
using Dile.Debug;
using Dile.Disassemble;
using Dile.UI.Debug;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace Dile.UI
{
	public sealed class NuGenUIHandler
	{
		#region Singleton implementation
		private static readonly NuGenUIHandler instance = new NuGenUIHandler();
        private static Control invokee;
        public static Control Invokee
        {
            get
            {
                return invokee;
            }

            set
            {
                invokee = value;
            }
        }

		public static NuGenUIHandler Instance
		{
			get
			{
				return instance;
			}
		}
		#endregion

		public delegate void AssembliesLoadedDelegate(List<NuGenAssembly> loadedAssemblies, bool isProjectChanged);
        public delegate void OneIntParameterDelegate(int value);
        public delegate void StringParameterDelegate(string value, bool addElapsedTime);
        public delegate void NoParameterDelegate();
        public delegate void ExceptionParameterDelegate(Exception exception);
        public delegate void BoolParameterDelegate(bool value);
        public delegate void OneUIntParameterDelegate(uint value);
        public delegate void OneStringParameterDelegate(string value);
        public delegate void TwoStringParametersDelegate(string value1, string value2);
        public delegate void OneTokenObjectParameterDelegate(NuGenTokenBase tokenObject);
        public delegate void DebugEventParameterDelegate(NuGenDebugEventDescriptor debugEventDescriptor);
        public delegate void IMultilineParameterDelegate(NuGenIMultiLine codeObject, NuGenCodeObjectDisplayOptions options);
        public delegate void ValueListParameterDelegate(FrameWrapper frame, List<NuGenBaseValueRefresher> values);
        public delegate void ValueRefresherParameterDelegate(NuGenBaseValueRefresher valueRefresher);
        public delegate void ValueRefresherWrapperParameterDelegate(FrameWrapper frame, ValueWrapper valueWrapper, NuGenBaseValueRefresher valueRefresher);
        public delegate void ValueRefresherWrapperExpressionParameterDelegate(FrameWrapper frame, NuGenBaseValueRefresher valueRefresher, string initialExpression);
        public delegate void FrameRefresherParameterDelegate(NuGenFrameRefresher frameRefresher, FrameWrapper frame, bool calledByWatchPanel);
        public delegate void OneDebuggerStateParameterDelegate(DebuggerState newState);
        public delegate void OneBreakpointInformationParameterDelegate(NuGenBreakpointInformation breakpoint);
        public delegate void IMultiLineBreakpointParameterDelegate(NuGenIMultiLine codeObject, NuGenBreakpointInformation breakpointInformation);
        public delegate void StringArrayParameterDelegate(string[] fileNames);
        public delegate void CodeEditorFormParameterDelegate(NuGenCodeEditorForm codeEditor);
        public delegate void ModuleParameterDelegate(ModuleWrapper module);
        public delegate void ModuleArrayParameterDelegate(ModuleWrapper[] modules);
        public delegate void RaiseUpdateDebugInformationDelegate(NuGenFrameRefresher activeFrameRefresher, FrameWrapper activeFrame, DebuggerState newState);

		private readonly NuGenDocumentSelectorForm documentSelector = new NuGenDocumentSelectorForm();
		private NuGenDocumentSelectorForm DocumentSelector
		{
			get
			{
				return documentSelector;
			}
		}

        private Control callback;
        public Control Callback
        {
            get
            {
                return callback;
            }

            set
            {
                callback = value;
            }
        }

		private AssembliesLoadedDelegate assembliesLoadedMethod;
		public AssembliesLoadedDelegate AssembliesLoadedMethod
		{
			get
			{
				return assembliesLoadedMethod;
			}
			set
			{
				assembliesLoadedMethod = value;
			}
		}

		private OneIntParameterDelegate stepProgressBarMethod;
        public OneIntParameterDelegate StepProgressBarMethod
		{
			get
			{
				return stepProgressBarMethod;
			}
			set
			{
				stepProgressBarMethod = value;
			}
		}

		private OneIntParameterDelegate setProgressBarMaximumMethod;
        public OneIntParameterDelegate SetProgressBarMaximumMethod
		{
			get
			{
				return setProgressBarMaximumMethod;
			}
			set
			{
				setProgressBarMaximumMethod = value;
			}
		}

		private StringParameterDelegate setProgressTextMethod;
        public StringParameterDelegate SetProgressTextMethod
		{
			get
			{
				return setProgressTextMethod;
			}
			set
			{
				setProgressTextMethod = value;
			}
		}

		private NoParameterDelegate resetProgressBarMethod;
        public NoParameterDelegate ResetProgressBarMethod
		{
			get
			{
				return resetProgressBarMethod;
			}
			set
			{
				resetProgressBarMethod = value;
			}
		}

		private ExceptionParameterDelegate showExceptionMethod;
        public ExceptionParameterDelegate ShowExceptionMethod
		{
			get
			{
				return showExceptionMethod;
			}
			set
			{
				showExceptionMethod = value;
			}
		}

		private BoolParameterDelegate setProgressBarVisibleMethod;
        public BoolParameterDelegate SetProgressBarVisibleMethod
		{
			get
			{
				return setProgressBarVisibleMethod;
			}
			set
			{
				setProgressBarVisibleMethod = value;
			}
		}

		private DebugEventParameterDelegate displayOutputInformationMethod;
        public DebugEventParameterDelegate DisplayOutputInformationMethod
		{
			get
			{
				return displayOutputInformationMethod;
			}
			set
			{
				displayOutputInformationMethod = value;
			}
		}

		private OneStringParameterDelegate displayLogMessageMethod;
        public OneStringParameterDelegate DisplayLogMessageMethod
		{
			get
			{
				return displayLogMessageMethod;
			}
			set
			{
				displayLogMessageMethod = value;
			}
		}

		private BoolParameterDelegate clearDebugPanelsMethod;
        public BoolParameterDelegate ClearDebugPanelsMethod
		{
			get
			{
				return clearDebugPanelsMethod;
			}
			set
			{
				clearDebugPanelsMethod = value;
			}
		}

		private NoArgumentsDelegate clearOutputPanelMethod;
        public NoArgumentsDelegate ClearOutputPanelMethod
		{
			get
			{
				return clearOutputPanelMethod;
			}
			set
			{
				clearOutputPanelMethod = value;
			}
		}

		private IMultilineParameterDelegate showCodeObjectMethod;
        public IMultilineParameterDelegate ShowCodeObjectMethod
		{
			get
			{
				return showCodeObjectMethod;
			}
			set
			{
				showCodeObjectMethod = value;
			}
		}

		private ValueListParameterDelegate showLocalVariablesMethod;
        public ValueListParameterDelegate ShowLocalVariablesMethod
		{
			get
			{
				return showLocalVariablesMethod;
			}
			set
			{
				showLocalVariablesMethod = value;
			}
		}

		private ValueListParameterDelegate showArgumentsMethod;
        public ValueListParameterDelegate ShowArgumentsMethod
		{
			get
			{
				return showArgumentsMethod;
			}
			set
			{
				showArgumentsMethod = value;
			}
		}

		private ValueRefresherWrapperParameterDelegate showAutoObjectMethod;
        public ValueRefresherWrapperParameterDelegate ShowAutoObjectMethod
		{
			get
			{
				return showAutoObjectMethod;
			}
			set
			{
				showAutoObjectMethod = value;
			}
		}

		private ValueRefresherWrapperExpressionParameterDelegate showObjectInObjectViewerMethod;
        public ValueRefresherWrapperExpressionParameterDelegate ShowObjectInObjectViewerMethod
		{
			get
			{
				return showObjectInObjectViewerMethod;
			}
			set
			{
				showObjectInObjectViewerMethod = value;
			}
		}

		private BoolParameterDelegate clearCodeDisplayersMethod;
        public BoolParameterDelegate ClearCodeDisplayersMethod
		{
			get
			{
				return clearCodeDisplayersMethod;
			}
			set
			{
				clearCodeDisplayersMethod = value;
			}
		}

		private FrameRefresherParameterDelegate frameChangedUpdateMethod;
        public FrameRefresherParameterDelegate FrameChangedUpdateMethod
		{
			get
			{
				return frameChangedUpdateMethod;
			}
			set
			{
				frameChangedUpdateMethod = value;
			}
		}

		private OneStringParameterDelegate displayUserWarningMethod;
        public OneStringParameterDelegate DisplayUserWarningMethod
		{
			get
			{
				return displayUserWarningMethod;
			}
			set
			{
				displayUserWarningMethod = value;
			}
		}

		private NoParameterDelegate clearUserWarningMethod;
        public NoParameterDelegate ClearUserWarningMethod
		{
			get
			{
				return clearUserWarningMethod;
			}
			set
			{
				clearUserWarningMethod = value;
			}
		}

		private OneDebuggerStateParameterDelegate showDebuggerStateMethod;
        public OneDebuggerStateParameterDelegate ShowDebuggerStateMethod
		{
			get
			{
				return showDebuggerStateMethod;
			}
			set
			{
				showDebuggerStateMethod = value;
			}
		}

		private OneBreakpointInformationParameterDelegate addBreakpointMethod;
        public OneBreakpointInformationParameterDelegate AddBreakpointMethod
		{
			get
			{
				return addBreakpointMethod;
			}
			set
			{
				addBreakpointMethod = value;
			}
		}

		private OneBreakpointInformationParameterDelegate removeBreakpointMethod;
        public OneBreakpointInformationParameterDelegate RemoveBreakpointMethod
		{
			get
			{
				return removeBreakpointMethod;
			}
			set
			{
				removeBreakpointMethod = value;
			}
		}

		private OneBreakpointInformationParameterDelegate deactivateBreakpointMethod;
        public OneBreakpointInformationParameterDelegate DeactivateBreakpointMethod
		{
			get
			{
				return deactivateBreakpointMethod;
			}
			set
			{
				deactivateBreakpointMethod = value;
			}
		}

		private IMultiLineBreakpointParameterDelegate updateBreakpointMethod;
        public IMultiLineBreakpointParameterDelegate UpdateBreakpointMethod
		{
			get
			{
				return updateBreakpointMethod;
			}
			set
			{
				updateBreakpointMethod = value;
			}
		}

		private OneStringParameterDelegate showAssemblyMissingWarningMethod;
        public OneStringParameterDelegate ShowAssemblyMissingWarningMethod
		{
			get
			{
				return showAssemblyMissingWarningMethod;
			}
			set
			{
				showAssemblyMissingWarningMethod = value;
			}
		}

		private StringArrayDelegate addAssemblyMethod;
        public StringArrayDelegate AddAssemblyMethod
		{
			get
			{
				return addAssemblyMethod;
			}
			set
			{
				addAssemblyMethod = value;
			}
		}

		private CodeEditorFormParameterDelegate codeEditorActivatedMethod;
        public CodeEditorFormParameterDelegate CodeEditorActivatedMethod
		{
			get
			{
				return codeEditorActivatedMethod;
			}
			set
			{
				codeEditorActivatedMethod = value;
			}
		}

		private TwoStringParametersDelegate showMessageBoxMethod;
        public TwoStringParametersDelegate ShowMessageBoxMethod
		{
			get
			{
				return showMessageBoxMethod;
			}
			set
			{
				showMessageBoxMethod = value;
			}
		}

		private NoArgumentsDelegate removeUnnecessaryAssembliesMethod;
        public NoArgumentsDelegate RemoveUnnecessaryAssembliesMethod
		{
			get
			{
				return removeUnnecessaryAssembliesMethod;
			}
			set
			{
				removeUnnecessaryAssembliesMethod = value;
			}
		}

		private NoArgumentsDelegate closeDynamicModuleDocumentsMethod;
        public NoArgumentsDelegate CloseDynamicModuleDocumentsMethod
		{
			get
			{
				return closeDynamicModuleDocumentsMethod;
			}
			set
			{
				closeDynamicModuleDocumentsMethod = value;
			}
		}

		private ModuleParameterDelegate addModuleToPanelMethod;
        public ModuleParameterDelegate AddModuleToPanelMethod
		{
			get
			{
				return addModuleToPanelMethod;
			}
			set
			{
				addModuleToPanelMethod = value;
			}
		}

		private ModuleArrayParameterDelegate addModulesToPanelMethod;
        public ModuleArrayParameterDelegate AddModulesToPanelMethod
		{
			get
			{
				return addModulesToPanelMethod;
			}
			set
			{
				addModulesToPanelMethod = value;
			}
		}

		private NuGenUIHandler()
		{
		}

        internal void Initialize(Panel dockPanel)
		{
			DocumentSelector.DockPanel = dockPanel;
		}

		public void StepProgressBar(int incrementValue)
		{
            if(invokee.InvokeRequired)
                invokee.Invoke(StepProgressBarMethod, new object[] {incrementValue});                
            else
                StepProgressBarMethod(incrementValue);
		}

		public void SetProgressBarMaximum(int maximum)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(SetProgressBarMaximumMethod, new object[]{maximum});
			else
				SetProgressBarMaximumMethod(maximum);
		}

		public void SetProgressText(string text, bool addElapsedTime)
		{
             if(invokee.InvokeRequired)
				invokee.Invoke(SetProgressTextMethod, new object[]{text, addElapsedTime});
			else
				SetProgressTextMethod(text, addElapsedTime);
		}

		public void SetProgressText(string fileName, string text, bool addElapsedTime)
		{
			text = string.Format("{0}: {1} ", fileName, text);
			SetProgressText(text, addElapsedTime);
		}

		public void ResetProgressBar()
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ResetProgressBarMethod, new object[]{});
			else
				ResetProgressBarMethod();
		}

		public void ShowException(Exception exception)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ShowExceptionMethod, new object[]{exception});
			else
				ShowExceptionMethod(exception);
		}

		public void AssembliesLoaded(List<NuGenAssembly> loadedAssemblies, bool isProjectChanged)
		{
            if (invokee.InvokeRequired)
                invokee.Invoke(AssembliesLoadedMethod, new object[] { loadedAssemblies, isProjectChanged });
            else
                AssembliesLoadedMethod(loadedAssemblies, isProjectChanged);                                     
		}

		public void SetProgressBarVisible(bool visible)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(SetProgressBarVisibleMethod, new object[]{visible});
			else
				SetProgressBarVisibleMethod(visible);
		}

		public void DisplayOutputInformation(NuGenDebugEventDescriptor debugEventDescriptor)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(DisplayOutputInformationMethod, new object[]{debugEventDescriptor});
			else
				DisplayOutputInformationMethod(debugEventDescriptor);
		}

		public void DisplayLogMessage(string logMessage)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(DisplayLogMessageMethod, new object[]{logMessage});
			else
				DisplayLogMessageMethod(logMessage);
		}

		public void ShowCodeObject(NuGenIMultiLine codeObject, NuGenCodeObjectDisplayOptions options)
		{
            if (invokee.InvokeRequired)
                invokee.Invoke(ShowCodeObjectMethod, new object[] { codeObject, options });
            else
                ShowCodeObjectMethod(codeObject, options);
		}

		public void ClearDebugPanels()
		{
			ClearDebugPanels(false);
		}
		
		public void ClearDebugPanels(bool leaveThreads)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ClearDebugPanelsMethod, new object[]{leaveThreads});
			else
				ClearDebugPanelsMethod(leaveThreads);
		}

		public void ClearOutputPanel()
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ClearOutputPanelMethod, new object[]{});
			else
				ClearOutputPanelMethod();
		}

		public void ShowObjectInObjectViewer(FrameWrapper frame, NuGenBaseValueRefresher valueRefresher, string initialExpression)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ShowObjectInObjectViewerMethod, new object[]{frame, valueRefresher, initialExpression});
			else
				ShowObjectInObjectViewerMethod(frame, valueRefresher, initialExpression);
		}

		public void FrameChangedUpdate(NuGenFrameRefresher frameRefresher, FrameWrapper frame, bool calledByWatchPanel)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(FrameChangedUpdateMethod, new object[]{frameRefresher, frame, calledByWatchPanel});
			else
				FrameChangedUpdateMethod(frameRefresher, frame, calledByWatchPanel);
		}

		public void ClearCodeDisplayers(bool refreshDisplayers)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ClearCodeDisplayersMethod, new object[]{refreshDisplayers});
			else
				ClearCodeDisplayersMethod(refreshDisplayers);
		}

		public void DisplayUserWarning(string text)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(DisplayUserWarningMethod, new object[]{text});
			else
				DisplayUserWarningMethod(text);
		}

		public void ClearUserWarning()
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ClearUserWarningMethod, new object[]{});
			else
				ClearUserWarningMethod();
		}

		public void ShowDebuggerState(DebuggerState newState)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ShowDebuggerStateMethod, new object[]{newState});
			else
				ShowDebuggerStateMethod(newState);
		}

		public void AddBreakpoint(NuGenBreakpointInformation breakpoint)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(AddBreakpointMethod, new object[]{breakpoint});
			else
				AddBreakpointMethod(breakpoint);
		}

		public void RemoveBreakpoint(NuGenBreakpointInformation breakpoint)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(RemoveBreakpointMethod, new object[]{breakpoint});
			else
				RemoveBreakpointMethod(breakpoint);
		}

		public void DeactivateBreakpoint(NuGenBreakpointInformation breakpoint)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(DeactivateBreakpointMethod, new object[]{breakpoint});
			else
				DeactivateBreakpointMethod(breakpoint);
		}

		public void UpdateBreakpoint(NuGenIMultiLine codeObject, NuGenBreakpointInformation breakpointInformation)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(UpdateBreakpointMethod, new object[]{codeObject, breakpointInformation});
			else
				UpdateBreakpointMethod(codeObject, breakpointInformation);
		}

		public void ShowAssemblyMissingWarning(string assemblyPath)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ShowAssemblyMissingWarningMethod, new object[]{assemblyPath});
			else
				ShowAssemblyMissingWarningMethod(assemblyPath);
		}

		public void AddAssembly(string[] fileNames)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(AddAssemblyMethod, new object[]{fileNames});
			else
				AddAssemblyMethod(fileNames);
		}

		public void DisplayDocumentSelector()
		{
			if (!DocumentSelector.Visible)
			{
				//DocumentSelector.Display(MainForm, MainForm.ActiveCodeEditors);
			}
		}

		public void CodeEditorActivated(NuGenCodeEditorForm codeEditor)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(CodeEditorActivatedMethod, new object[]{codeEditor});
			else
				CodeEditorActivatedMethod(codeEditor);
		}

		public void ShowMessageBox(string caption, string text)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(ShowMessageBoxMethod, new object[]{caption, text});
			else
				ShowMessageBoxMethod(caption, text);
		}

		public void RemoveUnnecessaryAssemblies()
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(RemoveUnnecessaryAssembliesMethod, new object[]{});
			else
				RemoveUnnecessaryAssembliesMethod();
		}

		public void CloseDynamicModuleDocuments()
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(CloseDynamicModuleDocumentsMethod, new object[]{});
			else
				CloseDynamicModuleDocumentsMethod();
		}

		public void AddModuleToPanel(ModuleWrapper module)
		{
            if(invokee.InvokeRequired)
				invokee.Invoke(AddModuleToPanelMethod, new object[]{module});
			else
				AddModuleToPanelMethod(module);
		}

		public void AddModulesToPanel(ModuleWrapper[] modules)
		{
            AddModulesToPanel(modules);
		}

        public event UpdateDebugInformationDelegate UpdateDebugInformation;

		public void RaiseUpdateDebugInformation(NuGenFrameRefresher activeFrameRefresher, FrameWrapper activeFrame, DebuggerState newState)
		{
            if (invokee.InvokeRequired)
            {
                invokee.BeginInvoke(UpdateDebugInformation, new object[] { activeFrameRefresher, activeFrame });
                invokee.BeginInvoke(ShowDebuggerStateMethod, new object[] { newState });
            }
            else
            {                
                UpdateDebugInformation(activeFrameRefresher, activeFrame);
                ShowDebuggerStateMethod(newState);
            }
		}
	}
}