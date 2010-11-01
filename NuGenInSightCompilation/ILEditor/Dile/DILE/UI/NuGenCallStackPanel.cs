using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Controls;
using Dile.Disassemble;
using Dile.Debug;
using Dile.UI.Debug;
using System.Runtime.InteropServices;


namespace Dile.UI
{
	public partial class NuGenCallStackPanel : NuGenBasePanel
	{
		private ToolStripMenuItem addModuleMenuItem;
		private ToolStripMenuItem AddModuleMenuItem
		{
			get
			{
				return addModuleMenuItem;
			}
			set
			{
				addModuleMenuItem = value;
			}
		}

		private ToolStripMenuItem displayCodeMenuItem;
		private ToolStripMenuItem DisplayCodeMenuItem
		{
			get
			{
				return displayCodeMenuItem;
			}
			set
			{
				displayCodeMenuItem = value;
			}
		}

		private ToolStripMenuItem copyCallStackMenuItem;
		private ToolStripMenuItem CopyCallStackMenuItem
		{
			get
			{
				return copyCallStackMenuItem;
			}
			set
			{
				copyCallStackMenuItem = value;
			}
		}

		private ToolStripMenuItem displayCallStackMenuItem;
		private ToolStripMenuItem DisplayCallStackMenuItem
		{
			get
			{
				return displayCallStackMenuItem;
			}
			set
			{
				displayCallStackMenuItem = value;
			}
		}

		public NuGenCallStackPanel()
		{
			InitializeComponent();

			callStackView.Initialize();

			DisplayCallStackMenuItem = new ToolStripMenuItem("Display callstack as text");
			DisplayCallStackMenuItem.Click += new EventHandler(DisplayCallStackMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, DisplayCallStackMenuItem);

			CopyCallStackMenuItem = new ToolStripMenuItem("Copy callstack to clipboard");
			CopyCallStackMenuItem.Click += new EventHandler(CopyCallStackMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, CopyCallStackMenuItem);

			AddModuleMenuItem = new ToolStripMenuItem("Add module to project");
			AddModuleMenuItem.Click += new EventHandler(AddModuleMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, AddModuleMenuItem);

			DisplayCodeMenuItem = new ToolStripMenuItem("Display code");
			DisplayCodeMenuItem.Click += new EventHandler(DisplayCodeMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, DisplayCodeMenuItem);

			callStackView.ItemContextMenu.Opening += new CancelEventHandler(ItemContextMenu_Opening);
		}

		protected override bool IsDebugPanel()
		{
			return true;
		}

		protected override bool UpdateWhenActiveFrameChanges()
		{
			return false;
		}

		private void ShowCodeObject()
		{
			if (callStackView.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = callStackView.SelectedItems[0];
				NuGenFrameInformation frameInformation = selectedItem.Tag as NuGenFrameInformation;

				if (frameInformation != null)
				{
					frameInformation.RefreshFrame();
					List<NuGenBaseLineDescriptor> specialLines = null;
					NuGenCodeObjectDisplayOptions displayOptions = new NuGenCodeObjectDisplayOptions();

					if (frameInformation.IsActiveFrame)
					{
						if (frameInformation.IsExactLocation)
						{
							displayOptions.CurrentLine = new NuGenCurrentLine(frameInformation.Offset);
						}
						else
						{
							displayOptions.CurrentLine = new NuGenCallerLine(frameInformation.Offset);
						}
					}
					else
					{
						NuGenBaseLineDescriptor lineDescriptor = null;
						specialLines = new List<NuGenBaseLineDescriptor>(1);

						if (frameInformation.IsExactLocation)
						{
							lineDescriptor = new NuGenExactCallerLine(frameInformation.Offset);
						}
						else
						{
							lineDescriptor = new NuGenCallerLine(frameInformation.Offset);
						}

						specialLines.Add(lineDescriptor);
					}

					displayOptions.SpecialLinesToAdd = specialLines;

					NuGenUIHandler.Instance.ShowCodeObject(frameInformation.MethodDefinition, displayOptions);
					NuGenUIHandler.Instance.FrameChangedUpdate(frameInformation.Refresher, frameInformation.Frame, false);
				}
			}
		}

		private void callStackView_Resize(object sender, EventArgs e)
		{
			methodColumn.Width = callStackView.ClientSize.Width;
		}

		public void DisplayCallStack(List<FrameWrapper> callStack)
		{
			if (callStack != null)
			{
				callStackView.BeginUpdate();
				callStackView.Items.Clear();

				for (int index = 0; index < callStack.Count; index++)
				{
					FrameWrapper frame = callStack[index];
					ListViewItem item = new ListViewItem();
					bool isCodeAvailable = false;
					FunctionWrapper function = null;

					try
					{
						function = frame.GetFunction();
						isCodeAvailable = true;
					}
					catch(COMException comException)
					{
						//0x80131309 == CORDBG_E_CODE_NOT_AVAILABLE
						if ((uint)comException.ErrorCode == 0x80131309)
						{
							isCodeAvailable = false;
						}
						else
						{
							throw;
						}
					}

					if (isCodeAvailable)
					{
						ModuleWrapper module = function.GetModule();
						uint functionToken = function.GetToken();
						
						NuGenTokenBase tokenObject = NuGenHelperFunctions.FindObjectByToken(functionToken, module);
						NuGenMethodDefinition methodDefinition = tokenObject as NuGenMethodDefinition;

						if (methodDefinition != null)
						{
							bool activeFrame = (index == 0);
							NuGenFrameInformation frameInformation = new NuGenFrameInformation(NuGenDebugEventHandler.Instance.EventObjects.Thread, methodDefinition, activeFrame, frame);
							item.Tag = frameInformation;
							item.Text = string.Format("{0}::{1}", methodDefinition.BaseTypeDefinition.FullName, methodDefinition.DisplayName);

							if (!frameInformation.IsExactLocation)
							{
								item.Text += " - not exact offset";
							}
						}
						else
						{
							string moduleName = module.GetName();

							if (module.IsInMemory())
							{
								item.Tag = new NuGenMissingModule(module);
							}
							else
							{
								item.Tag = new NuGenMissingModule(moduleName);
							}

							item.Text = "Unknown method (perhaps a reference is not loaded). Module name: " + moduleName;
						}
					}

					if (!frame.IsILFrame())
					{
						if (isCodeAvailable)
						{
							item.Text = "Native frame, IP offset is not available (" + item.Text + ")";
						}
						else
						{
							item.Text = "Native frame, IP offset is not available (code is unavailable).";
						}
					}

					item.ToolTipText = item.Text;
					callStackView.Items.Add(item);
				}

				callStackView.EndUpdate();
			}
		}

		protected override void OnInitializePanel()
		{
			base.OnInitializePanel();

			if (NuGenDebugEventHandler.Instance.EventObjects.Thread != null)
			{
				List<FrameWrapper> callStack = NuGenDebugEventHandler.Instance.EventObjects.Thread.GetCallStack();
				DisplayCallStack(callStack);
			}
			else
			{
				callStackView.Items.Clear();
				NuGenUIHandler.Instance.DisplayUserWarning("Unable to retrieve the call stack because there is no current thread.");
			}
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			callStackView.Items.Clear();
		}

		private void callStackView_DoubleClick(object sender, EventArgs e)
		{
			ShowCodeObject();
		}

		private void callStackView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
			{
				ShowCodeObject();
			}
		}

		private string GetCallStackAsText()
		{
			StringBuilder result = new StringBuilder();

			for (int index = 0; index < callStackView.Items.Count; index++)
			{
				result.Append(callStackView.Items[index].Text);

				if (index < callStackView.Items.Count - 1)
				{
					result.Append("\r\n");
				}
			}

			return result.ToString();
		}

		private void DisplayCallStackMenuItem_Click(object sender, EventArgs e)
		{
			NuGenTextDisplayer.Instance.ShowText(GetCallStackAsText());
		}

		private void CopyCallStackMenuItem_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(GetCallStackAsText());
		}

		private void AddModuleMenuItem_Click(object sender, EventArgs e)
		{
			if (callStackView.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = callStackView.SelectedItems[0];
				NuGenMissingModule missingModule = selectedItem.Tag as NuGenMissingModule;

				if (missingModule != null)
				{
					missingModule.AddModuleToProject();
				}
			}
		}

		private void DisplayCodeMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeObject();
		}

		private void ItemContextMenu_Opening(object sender, CancelEventArgs e)
		{
			AddModuleMenuItem.Visible = false;
			DisplayCodeMenuItem.Visible = true;

			if (callStackView.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = callStackView.SelectedItems[0];

				if (selectedItem.Tag is NuGenMissingModule)
				{
					AddModuleMenuItem.Visible = true;
					DisplayCodeMenuItem.Visible = false;
				}
			}
		}
	}
}