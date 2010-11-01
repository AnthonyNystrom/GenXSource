using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Controls;
using Dile.Debug;
using Dile.Disassemble;
using Dile.Disassemble.ILCodes;
using Dile.UI.Debug;
using System.IO;
using System.Runtime.InteropServices;


namespace Dile.UI
{
	public partial class NuGenCodeEditorForm : Panel
	{
		public NuGenIMultiLine CodeObject
		{
			get
			{
				return ilEditor.CodeObject;
			}
		}

		public NuGenBaseLineDescriptor CurrentLine
		{
			get
			{
				return ilEditor.CurrentLine;
			}
			set
			{
				ilEditor.CurrentLine = value;
			}
		}

		public NuGenCodeEditorForm()
		{
			InitializeComponent();
		}

		public void AddSpecialLines(List<NuGenBaseLineDescriptor> specialLines)
		{
			if (specialLines != null)
			{
				ilEditor.SpecialLines.AddRange(specialLines);
			}
		}

		public void CopySettings(NuGenCodeEditorForm destinationForm)
		{
			destinationForm.ilEditor.SpecialLines.AddRange(ilEditor.SpecialLines);
			destinationForm.CurrentLine = CurrentLine;
		}

		public void ShowCodeObject(NuGenIMultiLine codeObject)
		{
			Text = codeObject.HeaderText;			
			ilEditor.ShowCodeObject(codeObject);
		}

		public void SetWordWrap(bool enabled)
		{
			ilEditor.WordWrap = enabled;

			if (ilEditor.CodeObject != null)
			{
				ilEditor.Text = ilEditor.Text;
			}
		}

		private NuGenProjectExplorer projectExplorer;
		public NuGenProjectExplorer ProjectExplorer
		{
			get
			{
				return projectExplorer;
			}
			set
			{
				projectExplorer = value;
			}
		}

		public void RefreshEditorControl(bool forceFormatRefresh)
		{
			ilEditor.RefreshControl(forceFormatRefresh);
		}

		internal void RefreshEditorControl(bool forceFormatRefresh, uint scrollToOffset)
		{
			ilEditor.RefreshControl(forceFormatRefresh, scrollToOffset);
		}

		private void locateInProjectExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ProjectExplorer.LocateTokenNode((NuGenTokenBase)CodeObject);
		}

		public void ClearSpecialLines()
		{
			ilEditor.SpecialLines.Clear();
		}

		public void UpdateBreakpoint(NuGenFunctionBreakpointInformation breakpoint)
		{
			ilEditor.UpdateBreakpoint(breakpoint);
		}

		public void SetBreakpointAtSelection()
		{
			ilEditor.SetBreakpointAtSelection();
		}

		public void SetRunToCursorAtSelection()
		{
			ilEditor.SetRunToCursorAtSelection();
		}

		public void UpdateFont(Font font)
		{
			Font = font;
			ilEditor.Font = font;
			ilEditor.Text = ilEditor.Text;
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

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			setIPToolStripMenuItem.Visible = false;

			if (NuGenDebugEventHandler.Instance.EventObjects.Frame != null && NuGenDebugEventHandler.Instance.EventObjects.Frame.IsActiveFrame && CurrentLine != null)
			{
				NuGenMethodDefinition displayedMethod = CodeObject as NuGenMethodDefinition;

				if (displayedMethod != null)
				{
					FunctionWrapper currentFunction = NuGenDebugEventHandler.Instance.EventObjects.Frame.GetFunction();
					uint currentFunctionToken = currentFunction.GetToken();

					if (displayedMethod.Token == currentFunctionToken)
					{
						ModuleWrapper currentModule = currentFunction.GetModule();
						bool isInMemoryCurrentModule = currentModule.IsInMemory();
						string currentModuleName = string.Empty;

						if (isInMemoryCurrentModule)
						{
							currentModuleName = currentModule.GetNameFromMetaData();
						}
						else
						{
							currentModuleName = currentModule.GetName();

							try
							{
								currentModuleName = Path.GetFileNameWithoutExtension(currentModuleName);
							}
							catch
							{
							}
						}

						currentModuleName = currentModuleName.ToLower();

						if ((isInMemoryCurrentModule && displayedMethod.BaseTypeDefinition.ModuleScope.Name.ToLower() == currentModuleName) || (!isInMemoryCurrentModule && Path.GetFileNameWithoutExtension(displayedMethod.BaseTypeDefinition.ModuleScope.Assembly.FullPath).ToLower() == currentModuleName))
						{
							NuGenBaseILCode currentILCode = ilEditor.GetILCodeAtMouseCursor();
							setIPToolStripMenuItem.Visible = true;

							if (currentILCode != null)
							{
								int hResult = NuGenDebugEventHandler.Instance.EventObjects.Frame.CanSetIP(Convert.ToUInt32(currentILCode.Offset));

								if (hResult == 0)
								{
									setIPToolStripMenuItem.Enabled = true;
									setIPToolStripMenuItem.Tag = currentILCode;
								}
								else
								{
									COMException comException = Marshal.GetExceptionForHR(hResult) as COMException;

									if (comException != null)
									{
										NuGenUIHandler.Instance.DisplayUserWarning(Marshal.GetExceptionForHR(hResult).Message);
									}

									setIPToolStripMenuItem.Enabled = false;
								}
							}
							else
							{
								setIPToolStripMenuItem.Enabled = false;
							}
						}
					}
				}
			}
		}

		private void setIPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NuGenBaseILCode currentILCode = setIPToolStripMenuItem.Tag as NuGenBaseILCode;

			if (currentILCode != null)
			{
				try
				{
					NuGenDebugEventHandler.Instance.EventObjects.Frame.SetIP(Convert.ToUInt32(currentILCode.Offset));
					NuGenDebugEventHandler.Instance.DisplayAllInformation();
				}
				catch (Exception exception)
				{
					NuGenUIHandler.Instance.ShowException(exception);
					NuGenUIHandler.Instance.DisplayUserWarning(exception.Message);
				}
			}
		}

		private void CodeEditorForm_Enter(object sender, EventArgs e)
		{
			NuGenUIHandler.Instance.CodeEditorActivated(this);
		}
	}
}