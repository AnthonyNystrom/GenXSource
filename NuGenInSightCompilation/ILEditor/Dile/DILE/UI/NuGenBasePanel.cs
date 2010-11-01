using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.UI.Debug;
using System.Windows.Forms;

namespace Dile.UI
{
	public class NuGenBasePanel : UserControl
	{
		private MenuItem menuItem;
		public MenuItem MenuItem
		{
			get
			{
				return menuItem;
			}
			set
			{
				menuItem = value;
			}
		}

		private bool isDebugPanelInitialized;
		private bool IsDebugPanelInitialized
		{
			get
			{
				return isDebugPanelInitialized;
			}
			set
			{
				isDebugPanelInitialized = value;
			}
		}

		private bool isPanelCleared = true;
		private bool IsPanelCleared
		{
			get
			{
				return isPanelCleared;
			}
			set
			{
				isPanelCleared = value;
			}
		}

		private NoArgumentsDelegate onInitializePanelMethod;
		private NoArgumentsDelegate OnInitializePanelMethod
		{
			get
			{
				return onInitializePanelMethod;
			}
			set
			{
				onInitializePanelMethod = value;
			}
		}

		private NoArgumentsDelegate onClearPanelMethod;
		private NoArgumentsDelegate OnClearPanelMethod
		{
			get
			{
				return onClearPanelMethod;
			}
			set
			{
				onClearPanelMethod = value;
			}
		}

		private NuGenFrameRefresher activeFrameRefresher;
		protected NuGenFrameRefresher ActiveFrameRefresher
		{
			get
			{
				return activeFrameRefresher;
			}
			private set
			{
				activeFrameRefresher = value;
			}
		}

		private FrameWrapper activeFrame;
		protected FrameWrapper ActiveFrame
		{
			get
			{
				if (!IsFrameValid && ActiveFrameRefresher != null)
				{
					try
					{
						activeFrame = ActiveFrameRefresher.GetRefreshedValue();
						IsFrameValid = true;
					}
					catch
					{
					}
				}

				return activeFrame;
			}
			set
			{
				activeFrame = value;
			}
		}

		private bool isFrameValid;
		private bool IsFrameValid
		{
			get
			{
				return isFrameValid;
			}
			set
			{
				isFrameValid = value;
			}
		}

		public NuGenBasePanel()
		{
			if (IsDebugPanel())
			{
				RegisterToDebugEvents();
				NuGenAssemblyLoader.Instance.AssembliesLoaded += new AssembliesLoadedDelegate(AssemblyLoader_AssembliesLoaded);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool result = false;

			if (msg.Msg == NuGenConstants.WM_KEYDOWN || msg.Msg == NuGenConstants.WM_SYSKEYDOWN)
			{
				if (keyData == (Keys.Control | Keys.Tab))
				{
					NuGenUIHandler.Instance.DisplayDocumentSelector();
					result = true;
				}
			}

			if (!result)
			{
				result = base.ProcessCmdKey(ref msg, keyData);
			}

			return result;
		}

		protected virtual bool IsDebugPanel()
		{
			return false;
		}

		protected virtual bool UpdateWhenActiveFrameChanges()
		{
			return true;
		}

		protected virtual void OnClearPanel()
		{
		}

		private void RegisterToDebugEvents()
		{
			NuGenDebugEventHandler.Instance.ActiveFrameChanged += new ActiveFrameChangedDelegate(debugEventHandler_ActiveFrameChanged);
			NuGenDebugEventHandler.Instance.EvaluationComplete += new EvaluationCompleteDelegate(debugEventHandler_EvaluationComplete);
			NuGenDebugEventHandler.Instance.InvalidateDebugInformation += new InvalidateDebugInformationDelegate(debugEventHandler_InvalidateDebugInformation);			
			NuGenDebugEventHandler.Instance.ProcessExited += new ProcessExitedDelegate(debugEventHandler_ProcessExited);
            NuGenUIHandler.Instance.UpdateDebugInformation += new UpdateDebugInformationDelegate(debugEventHandler_UpdateDebugInformation);

			OnInitializePanelMethod = new NoArgumentsDelegate(OnInitializePanel);
			OnClearPanelMethod = new NoArgumentsDelegate(OnClearPanel);
		}

		private void debugEventHandler_EvaluationComplete(EvalWrapper evalWrapper)
		{
			IsFrameValid = false;
		}

        private delegate void ProcessExitedMethod();

		protected virtual void debugEventHandler_ProcessExited()
		{
            if (!IsPanelCleared)
            {
                IsDebugPanelInitialized = false;
                ActiveFrameRefresher = null;
                ActiveFrame = null;
                IsFrameValid = false;

                ClearPanel();
            }
		}

		private void AssemblyLoader_AssembliesLoaded(List<NuGenAssembly> assemblies, bool isProjectChanged)
		{
			if (!IsPanelCleared)
			{
				ClearPanel();
			}

			IsDebugPanelInitialized = false;

			if (Visible)
			{
				InitializePanel();
			}
		}

		private void debugEventHandler_ActiveFrameChanged(NuGenFrameRefresher newActiveFrameRefresher, FrameWrapper newActiveFrame)
		{
			ActiveFrameRefresher = newActiveFrameRefresher;
			ActiveFrame = newActiveFrame;
			IsFrameValid = true;

			if (UpdateWhenActiveFrameChanges())
			{
				if (Visible)
				{
					ClearPanel();
					InitializePanel();
				}
				else
				{
					IsPanelCleared = false;
					IsDebugPanelInitialized = false;
				}
			}
		}

		private void debugEventHandler_InvalidateDebugInformation()
		{
			if (!IsPanelCleared)
			{
				IsDebugPanelInitialized = false;
				ActiveFrameRefresher = null;
				ActiveFrame = null;
				IsFrameValid = false;

				ClearPanel();
			}
		}

		private void debugEventHandler_UpdateDebugInformation(NuGenFrameRefresher activeFrameRefresher, FrameWrapper activeFrame)
		{
			ActiveFrameRefresher = activeFrameRefresher;
			ActiveFrame = activeFrame;
			IsFrameValid = true;
			IsPanelCleared = false;

			if (Visible)
			{
				InitializePanel();
			}
			else
			{
				IsDebugPanelInitialized = false;
			}
		}

		public void ClearPanel()
		{
			if (!IsPanelCleared)
			{
				ForceClearPanel();
			}
		}

		public void ForceClearPanel()
		{
			try
			{
				if (InvokeRequired)
				{
					Invoke(OnClearPanelMethod);
				}
				else
				{
					OnClearPanel();
				}

				IsDebugPanelInitialized = false;
				IsPanelCleared = true;
			}
			catch (Exception exception)
			{
				NuGenUIHandler.Instance.DisplayUserWarning(string.Format("An error occurred while trying to clear the {0}: {1}", Text, exception.Message));
			}
		}

		private void InitializePanel()
		{
			if (!IsDebugPanelInitialized)
			{
				ForceReinitializePanel();
			}
		}

		private void ForceReinitializePanel()
		{
			try
			{
				switch (NuGenDebugEventHandler.Instance.State)
				{
					case DebuggerState.DebuggeePaused:
					case DebuggerState.DebuggeeSuspended:
					case DebuggerState.DebuggeeThrewException:
						if (InvokeRequired)
						{
							Invoke(OnInitializePanelMethod);
						}
						else
						{
							OnInitializePanel();
						}

						IsDebugPanelInitialized = true;
						IsPanelCleared = false;
						break;
				}
			}
			catch (Exception exception)
			{
				NuGenUIHandler.Instance.DisplayUserWarning(string.Format("An error occurred while trying to initialize the {0}: {1}", Text, exception.Message));
			}
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			InitializePanel();
		}

		protected virtual void OnInitializePanel()
		{
			if (!IsPanelCleared)
			{
				ClearPanel();
				IsPanelCleared = true;
			}
		}

		public override string ToString()
		{
            return this.Text;
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// BasePanel
			// 
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Name = "BasePanel";
			this.ResumeLayout(false);

		}
	}
}