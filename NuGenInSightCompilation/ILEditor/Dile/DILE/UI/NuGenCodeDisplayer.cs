using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Controls;
using Dile.Debug;
using Dile.Disassemble;
using Dile.UI.Debug;
using System.Windows.Forms;


namespace Dile.UI
{
    [Serializable]
	public class NuGenCodeDisplayer
	{
		private NuGenIMultiLine codeObject;
		public NuGenIMultiLine CodeObject
		{
			get
			{
				return codeObject;
			}
			set
			{
				codeObject = value;
			}
		}

		private NuGenCodeEditorForm window;
		internal NuGenCodeEditorForm Window
		{
			get
			{
				return window;
			}
			private set
			{
				window = value;
			}
		}

        private NuGenProjectExplorer.CodeDisplayerAddedDelegate codeDisplayerAdded;
        public NuGenProjectExplorer.CodeDisplayerAddedDelegate CodeDisplayerAdded
		{
			get
			{
                return codeDisplayerAdded;
			}
			set
			{
                codeDisplayerAdded = value;
			}
		}

        internal NuGenCodeDisplayer(NuGenProjectExplorer.CodeDisplayerAddedDelegate codeDisplayerAdded, NuGenIMultiLine codeObject, NuGenCodeEditorForm window)
		{
            CodeDisplayerAdded = codeDisplayerAdded;
			CodeObject = codeObject;
			Window = window;
		}

		public void ShowCodeObject(NuGenCodeObjectDisplayOptions options)
		{
			bool initialized = true;

			try
			{
				initialized = (CodeObject.CodeLines != null);

				if (!initialized)
				{
					NuGenUIHandler.Instance.ResetProgressBar();
					NuGenUIHandler.Instance.SetProgressBarMaximum(2);
					NuGenUIHandler.Instance.SetProgressBarVisible(true);
					NuGenUIHandler.Instance.SetProgressText("Creating text representation of the object... ", true);
					CodeObject.Initialize();
					NuGenUIHandler.Instance.StepProgressBar(1);

					NuGenUIHandler.Instance.SetProgressText("Displaying the text... ", true);
					Window.ShowCodeObject(codeObject);
                   
                    //Check this : PETETODO
					//Window.Controls.Add(DockPanel);
                    CodeDisplayerAdded(Window);

					NuGenUIHandler.Instance.SetProgressText("Ready\n\n", true);
				}

				if (Window.IsDisposed)
				{
					NuGenCodeEditorForm newWindow = new NuGenCodeEditorForm();
					newWindow.UpdateFont(NuGenSettings.Instance.CodeEditorFont.Font);
					Window.CopySettings(newWindow);
					Window = newWindow;

					Window.ShowCodeObject(codeObject);
					Window.CurrentLine = options.CurrentLine;

                    //PETETODO : Check this
					//Window.Controls.Add(DockPanel);
                    //If initialized == true then it has already been added
                    if (initialized)
                    {
                        CodeDisplayerAdded(Window);
                    }
				}
				else
				{
					if (Window.Visible)
					{
						Window.CurrentLine = options.CurrentLine;
					}
					else
					{
						Window.ShowCodeObject(codeObject);
						Window.CurrentLine = options.CurrentLine;

						//Window.Controls.Add(DockPanel);
                        //If initialized == true then it has already been added
                        if (initialized)
                        {
                            CodeDisplayerAdded(Window);
                        }
					}					
				}

				Window.AddSpecialLines(options.SpecialLinesToAdd);				

				if (options.IsNavigateSet)
				{
					Window.RefreshEditorControl(true, options.NavigateToOffset);
				}
				else
				{
					Window.RefreshEditorControl(true);
				}
			}
			catch (Exception exception)
			{
				NuGenUIHandler.Instance.ShowException(exception);
			}
			finally
			{
				if (!initialized)
				{
					NuGenUIHandler.Instance.SetProgressBarVisible(false);
				}
			}
		}

		public void ClearSpecialLines()
		{
			if (!Window.IsDisposed)
			{
				Window.ClearSpecialLines();
			}
		}

		public void ClearCurrentLine()
		{
			if (!Window.IsDisposed)
			{
				Window.CurrentLine = null;
			}
		}

		public void Refresh()
		{
			if (!Window.IsDisposed)
			{
				Window.RefreshEditorControl(true);
			}
		}

		public void UpdateBreakpoint(NuGenBreakpointInformation breakpointInformation)
		{
			NuGenFunctionBreakpointInformation functionBreakpointInformation = breakpointInformation as NuGenFunctionBreakpointInformation;

			if (!Window.IsDisposed && Window.Visible && functionBreakpointInformation != null)
			{
				Window.UpdateBreakpoint(functionBreakpointInformation);
			}
		}
	}
}