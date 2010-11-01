using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug;
using Dile.Metadata;
using Dile.UI.Debug;
using Dile.Disassemble;
using System.Threading;
using Janus.Windows.GridEX;


namespace Dile.UI
{
	public partial class NuGenThreadsPanel : NuGenBasePanel
	{
		private NuGenMethodDefinition getThreadNameMethod;
		private NuGenMethodDefinition GetThreadNameMethod
		{
			get
			{
				return getThreadNameMethod;
			}
			set
			{
				getThreadNameMethod = value;
			}
		}

		private bool hasSearchedForNameMethod = false;
		private bool HasSearchedForNameMethod
		{
			get
			{
				return hasSearchedForNameMethod;
			}
			set
			{
				hasSearchedForNameMethod = value;
			}
		}

		private string evaluatedThreadName;
		private string EvaluatedThreadName
		{
			get
			{
				return evaluatedThreadName;
			}
			set
			{
				evaluatedThreadName = value;
			}
		}

		private ToolStripMenuItem changeThreadMenuItem;
		private ToolStripMenuItem ChangeThreadMenuItem
		{
			get
			{
				return changeThreadMenuItem;
			}
			set
			{
				changeThreadMenuItem = value;
			}
		}

		public NuGenThreadsPanel()
		{
			InitializeComponent();
			NuGenSettings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Instance_DisplayHexaNumbersChanged);

			threadsGrid.Initialize();
			ChangeThreadMenuItem = new ToolStripMenuItem("Change current thread");
			ChangeThreadMenuItem.Click += new EventHandler(ChangeThreadMenuItem_Click);
			threadsGrid.RowContextMenu.Items.Insert(0, ChangeThreadMenuItem);
		}

		protected override bool IsDebugPanel()
		{
			return true;
		}

		private void ChangeThreadMenuItem_Click(object sender, EventArgs e)
		{
			if (threadsGrid.SelectedItems.Count == 1)
			{
                GridEXRow row = threadsGrid.SelectedItems[0].GetRow();
                ThreadWrapper thread = (ThreadWrapper)NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value];

				ChangeCurrentThread(thread);
			}
		}

		private void Instance_DisplayHexaNumbersChanged()
		{
			if (threadsGrid.RowCount != 0)
			{
                for (int i = 0; i < threadsGrid.RowCount; i++)
                {
                    GridEXCell idCell = threadsGrid.GetRow(i).Cells[0];
                    idCell.Value = NuGenHelperFunctions.FormatNumber(NuGenHelperFunctions.TaggedObjects[(int)idCell.Value]);
                }
			}
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			threadsGrid.ClearItems();
		}

		protected override void OnInitializePanel()
		{
			base.OnInitializePanel();

			ShowThreads();
		}

		public void Reset()
		{
			GetThreadNameMethod = null;
			HasSearchedForNameMethod = false;
		}

		private void FindGetThreadNameMethod(uint threadTypeToken, ModuleWrapper module)
		{
			NuGenTypeDefinition threadType = NuGenHelperFunctions.FindObjectByToken(threadTypeToken, module) as NuGenTypeDefinition;

			if (threadType != null)
			{
				NuGenProperty nameProperty = threadType.FindPropertyByName("Name");

				if (nameProperty != null)
				{
					GetThreadNameMethod = NuGenHelperFunctions.FindObjectByToken(nameProperty.GetterMethodToken, module) as NuGenMethodDefinition;
				}
			}

			HasSearchedForNameMethod = true;
		}

		private void GetThreadName(ThreadWrapper threadWrapper, ValueWrapper threadObject, NuGenFrameRefresher threadActiveFrameRefresher)
		{
			List<ModuleWrapper> modules = threadWrapper.FindModulesByName(GetThreadNameMethod.BaseTypeDefinition.ModuleScope.Assembly.FileName);

			if (modules.Count == 1)
			{
				ModuleWrapper module = modules[0];
				FunctionWrapper getThreadNameFunction = module.GetFunction(GetThreadNameMethod.Token);
				List<ValueWrapper> arguments = new List<ValueWrapper>(1);
				arguments.Add(threadObject);

				NuGenEvaluationHandler methodCaller = new NuGenEvaluationHandler(threadActiveFrameRefresher);
				NuGenBaseEvaluationResult evaluationResult = methodCaller.CallFunction(getThreadNameFunction, arguments);

				if (evaluationResult.IsSuccessful)
				{
					if (evaluationResult.Result != null && (CorElementType)evaluationResult.Result.ElementType == CorElementType.ELEMENT_TYPE_STRING)
					{
						ValueWrapper dereferencedResult = evaluationResult.Result.DereferenceValue();

						if (dereferencedResult != null)
						{
							EvaluatedThreadName = NuGenHelperFunctions.ShowEscapeCharacters(dereferencedResult.GetStringValue(), true);
						}
					}
				}
			}
		}

		private void ShowThreads()
		{
			threadsGrid.BeginGridUpdate();
			threadsGrid.ClearItems();
			List<ThreadWrapper> threads = NuGenDebugEventHandler.Instance.EventObjects.Controller.EnumerateThreads();

			foreach (ThreadWrapper thread in threads)
			{
				EvaluatedThreadName = "<no name>";
				ValueWrapper threadObject = null;
				ValueWrapper dereferencedObject = null;

				if (!HasSearchedForNameMethod)
				{
					threadObject = thread.GetObject();

					if (threadObject != null && !threadObject.IsNull())
					{
						dereferencedObject = threadObject.DereferenceValue();

						if (dereferencedObject != null)
						{
							ClassWrapper threadClass = dereferencedObject.GetClassInformation();
							uint threadTypeToken = threadClass.GetToken();
							ModuleWrapper module = threadClass.GetModule();

							FindGetThreadNameMethod(threadTypeToken, module);
						}
					}
				}

				if (HasSearchedForNameMethod)
				{
					if (GetThreadNameMethod == null)
					{
						EvaluatedThreadName = "<definition of the Thread class is not loaded>";
					}
					else
					{
						if (threadObject == null)
						{
							threadObject = thread.GetObject();

							if (threadObject != null && !threadObject.IsNull())
							{
								dereferencedObject = threadObject.DereferenceValue();
							}
						}

						if (dereferencedObject != null)
						{
							FrameWrapper threadActiveFrame = thread.GetActiveFrame();

							if (threadActiveFrame != null)
							{
								NuGenFrameRefresher threadActiveFrameRefresher = new NuGenFrameRefresher(thread, threadActiveFrame.ChainIndex, threadActiveFrame.FrameIndex, threadActiveFrame.IsActiveFrame);

								GetThreadName(thread, threadObject, threadActiveFrameRefresher);
							}
						}
					}
				}

                GridEXRow row = threadsGrid.AddItem();				

				uint threadID = thread.GetID();
				GridEXCell idCell = row.Cells[0];
                NuGenHelperFunctions.TaggedObjects.Add((int)idCell.Value, threadID);				
				idCell.Value = NuGenHelperFunctions.FormatNumber(threadID);
				row.Cells[1].Value = EvaluatedThreadName;

				AppDomainWrapper appDomain = thread.GetAppDomain();

				if (appDomain != null)
				{
					row.Cells[2].Value = appDomain.GetName();
				}

                NuGenHelperFunctions.TaggedObjects.Add((String)row.Cells[1].Value + (String)row.Cells[2].Value, thread);				
			}

			threadsGrid.EndGridUpdate();
		}

		private void threadsGrid_CellDoubleClick(object sender, RowActionEventArgs e)
		{
			if (threadsGrid.CurrentRow != null)
			{
                GridEXRow row = threadsGrid.CurrentRow;
                ThreadWrapper thread = (ThreadWrapper)NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value];

				ChangeCurrentThread(thread);
			}
		}

		private void ChangeCurrentThread(ThreadWrapper thread)
		{
			NuGenUIHandler.Instance.ClearDebugPanels(true);
			NuGenUIHandler.Instance.ClearUserWarning();
			NuGenUIHandler.Instance.ClearCodeDisplayers(true);

			NuGenDebugEventHandler.Instance.EventObjects.Thread = thread;
			NuGenDebugEventHandler.Instance.DisplayAllInformation();
		}
	}
}