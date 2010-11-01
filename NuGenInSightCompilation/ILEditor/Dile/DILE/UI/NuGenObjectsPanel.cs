using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug;
using Dile.Debug.Expressions;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using Janus.Windows.GridEX;


namespace Dile.UI
{
	public partial class NuGenObjectsPanel : NuGenBasePanel
	{
		private ObjectsPanelMode mode;
		public ObjectsPanelMode Mode
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;

				ChangeMode();
			}
		}

		public string ColumnHeader
		{
			get
			{
				return objectsGrid.RootTable.Columns[0].Caption;
			}
			set
			{
				objectsGrid.RootTable.Columns[0].Caption = value;
			}
		}

		private NuGenEvaluationContext evaluationContext;
		private NuGenEvaluationContext EvaluationContext
		{
			get
			{
				return evaluationContext;
			}
			set
			{
				evaluationContext = value;
			}
		}

		private NuGenValueDisplayer valueDisplayer;
		private NuGenValueDisplayer ValueDisplayer
		{
			get
			{
				return valueDisplayer;
			}
			set
			{
				valueDisplayer = value;
			}
		}

		private ToolStripMenuItem displayInViewerMenuItem;
		private ToolStripMenuItem DisplayInViewerMenuItem
		{
			get
			{
				return displayInViewerMenuItem;
			}
			set
			{
				displayInViewerMenuItem = value;
			}
		}

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

		public NuGenObjectsPanel()
		{
			InitializeComponent();
			NuGenSettings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Instance_DisplayHexaNumbersChanged);

			objectsGrid.Initialize();

			AddModuleMenuItem = new ToolStripMenuItem("Add module to project");
			AddModuleMenuItem.Click += new EventHandler(AddModuleMenuItem_Click);
			objectsGrid.RowContextMenu.Items.Insert(0, AddModuleMenuItem);

			DisplayInViewerMenuItem = new ToolStripMenuItem("Display in Object Viewer...");
			DisplayInViewerMenuItem.Click += new EventHandler(DisplayInViewerMenuItem_Click);
			objectsGrid.RowContextMenu.Items.Insert(1, DisplayInViewerMenuItem);

			objectsGrid.RowContextMenu.Opening += new CancelEventHandler(RowContextMenu_Opening);
		}

		private void ChangeMode()
		{
			switch(Mode)
			{
				case ObjectsPanelMode.Arguments:
					this.objectsGrid.RootTable.Columns[0].Caption = "Argument";					
					break;

				case ObjectsPanelMode.AutoObjects:
					this.objectsGrid.RootTable.Columns[0].Caption = "Expression";					
					break;

				case ObjectsPanelMode.LocalVariables:
					this.objectsGrid.RootTable.Columns[0].Caption = "Local variable";
					break;

				case ObjectsPanelMode.Watch:
					this.objectsGrid.RootTable.Columns[0].Caption = "Expression";					
					break;
			}

			bool watchPanel = (Mode == ObjectsPanelMode.Watch);		
		}

		private void RowContextMenu_Opening(object sender, CancelEventArgs e)
		{
            e.Cancel = objectsGrid.EditTextBox != null;			

			if (objectsGrid.EditTextBox != null)
			{
				AddModuleMenuItem.Visible = false;
				DisplayInViewerMenuItem.Visible = false;

				if (objectsGrid.SelectedItems.Count == 1)
				{
					GridEXRow selectedRow = objectsGrid.SelectedItems[0].GetRow();
					NuGenIValueFormatter valueFormatter = NuGenHelperFunctions.TaggedObjects[(int)selectedRow.Cells[2].Value] as NuGenIValueFormatter;

					if (valueFormatter != null)
					{
						if (valueFormatter is NuGenMissingModuleFormatter)
						{
							AddModuleMenuItem.Visible = true;
						}

						DisplayInViewerMenuItem.Visible = true;
					}
				}
			}
		}

		private void AddModuleMenuItem_Click(object sender, EventArgs e)
		{
			if (objectsGrid.SelectedItems.Count == 1)
			{
				GridEXRow selectedRow = objectsGrid.SelectedItems[0].GetRow();
				NuGenMissingModuleFormatter missingModuleFormatter = NuGenHelperFunctions.TaggedObjects[(int)selectedRow.Cells[2].Value] as NuGenMissingModuleFormatter;

				if (missingModuleFormatter != null)
				{
					missingModuleFormatter.MissingModule.AddModuleToProject();
				}
			}
		}

		private void DisplayInViewerMenuItem_Click(object sender, EventArgs e)
		{
			if (objectsGrid.SelectedItems.Count == 1)
			{
				GridEXRow selectedRow = objectsGrid.SelectedItems[0].GetRow();
				NuGenIValueFormatter valueFormatter = NuGenHelperFunctions.TaggedObjects[(int)selectedRow.Cells[2].Value] as NuGenIValueFormatter;

				if (valueFormatter != null)
				{
					NuGenUIHandler.Instance.ShowObjectInObjectViewer(ActiveFrame, valueFormatter.ValueRefresher, (string)selectedRow.Cells[0].Value);
				}
			}
		}

		protected override bool IsDebugPanel()
		{
			return true;
		}

		private void Instance_DisplayHexaNumbersChanged()
		{
            for (int i = 0; i < objectsGrid.RowCount; i++)
            {
                GridEXRow row = objectsGrid.GetRow(i);

                NuGenIValueFormatter valueFormatter = NuGenHelperFunctions.TaggedObjects[(int)row.Cells[2].Value] as NuGenIValueFormatter;

                if (valueFormatter != null)
                {
                    row.Cells[1].Value = valueFormatter.GetFormattedString(NuGenSettings.Instance.DisplayHexaNumbers);
                }
            }		
		}

        private int id = 0;
		private void AddValueFormatter(NuGenIValueFormatter valueFormatter)
		{
			GridEXRow addedRow = objectsGrid.AddItem(valueFormatter.Name, valueFormatter.GetFormattedString(NuGenSettings.Instance.DisplayHexaNumbers), id++);

            NuGenHelperFunctions.TaggedObjects.Add(id, valueFormatter);            
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			if (Mode == ObjectsPanelMode.Watch)
			{
                for (int i = 0; i < objectsGrid.RowCount; i++)
                {
                    objectsGrid.GetRow(i).Cells[1].Value = string.Empty;
                }				
			}
			else
			{
                objectsGrid.ClearItems();
			}
		}

		protected override void OnInitializePanel()
		{
			base.OnInitializePanel();

			if (NuGenDebugEventHandler.Instance.EventObjects.Thread != null)
			{
				if (ActiveFrame == null)
				{
					NuGenUIHandler.Instance.DisplayUserWarning("No stack frame information is available at the location.");
				}
				else if (!ActiveFrame.IsILFrame() && Mode != ObjectsPanelMode.Watch)
				{
					switch(Mode)
					{
						case ObjectsPanelMode.Arguments:
							NuGenUIHandler.Instance.DisplayUserWarning("The current frame is native therefore arguments are not available.");
							break;

						case ObjectsPanelMode.AutoObjects:
							NuGenUIHandler.Instance.DisplayUserWarning("The current frame is native therefore auto objects are not available.");
							break;

						case ObjectsPanelMode.LocalVariables:
							NuGenUIHandler.Instance.DisplayUserWarning("The current frame is native therefore local variables are not available.");
							break;
					}
				}
				else
				{
					EvalWrapper evalWrapper = NuGenDebugEventHandler.Instance.EventObjects.Thread.CreateEval();
					EvaluationContext = new NuGenEvaluationContext(NuGenDebugEventHandler.Instance.EventObjects.Process, new NuGenEvaluationHandler(ActiveFrameRefresher), evalWrapper, NuGenDebugEventHandler.Instance.EventObjects.Thread);
					ValueDisplayer = new NuGenValueDisplayer(EvaluationContext);

					switch (Mode)
					{
						case ObjectsPanelMode.Arguments:
							DisplayArguments();
							break;

						case ObjectsPanelMode.AutoObjects:
							DisplayCurrentException();
							break;

						case ObjectsPanelMode.LocalVariables:
							DisplayLocalVariables();
							break;

						case ObjectsPanelMode.Watch:
							DisplayWatchExpressions();
							break;
					}
				}
			}
		}

		private void DisplayArguments()
		{
			try
			{
				uint argumentCount = ActiveFrame.GetArgumentCount();
				List<NuGenBaseValueRefresher> arguments = new List<NuGenBaseValueRefresher>(Convert.ToInt32(argumentCount));

				for (uint index = 0; index < argumentCount; index++)
				{
					NuGenArgumentValueRefresher refresher = new NuGenArgumentValueRefresher(string.Concat("A_", index), ActiveFrameRefresher, index);

					arguments.Add(refresher);
				}

				ShowObjects(arguments);
			}
			catch (Exception exception)
			{
				NuGenUIHandler.Instance.ShowException(exception);
			}
		}

		public void DisplayLocalVariables()
		{
			try
			{
				uint localVariableCount = ActiveFrame.GetLocalVariableCount();
				List<NuGenBaseValueRefresher> localVariables = new List<NuGenBaseValueRefresher>(Convert.ToInt32(localVariableCount));

				for (uint index = 0; index < localVariableCount; index++)
				{
					NuGenLocalVariableRefresher refresher = new NuGenLocalVariableRefresher(string.Concat("V_", index), ActiveFrameRefresher, index);

					localVariables.Add(refresher);
				}

				ShowObjects(localVariables);
			}
			catch (Exception exception)
			{
				NuGenUIHandler.Instance.ShowException(exception);
			}
		}

		private void DisplayCurrentException()
		{
			try
			{
				if (ActiveFrame.IsILFrame())
				{
					ValueWrapper exceptionObject = NuGenDebugEventHandler.Instance.EventObjects.Thread.GetCurrentException();

					if (exceptionObject != null)
					{
						NuGenExceptionValueRefresher valueRefresher = new NuGenExceptionValueRefresher(NuGenConstants.CurrentExceptionName, NuGenDebugEventHandler.Instance.EventObjects.Thread);

						ShowObject(valueRefresher);
					}
				}
			}
			catch (Exception exception)
			{
				NuGenUIHandler.Instance.ShowException(exception);
			}
		}

		private void DisplayWatchExpression(GridEXRow watchRow)
		{
			NuGenIValueFormatter watchValueFormatter = null;
			NuGenExpressionValueRefresher expressionRefresher = null;

			try
			{
				NuGenParser parser = new NuGenParser();
				string watchExpression = (string)watchRow.Cells[0].Value;
				List<NuGenBaseExpression> expressions = parser.Parse(watchExpression);
				expressionRefresher = new NuGenExpressionValueRefresher(expressions, ActiveFrameRefresher, EvaluationContext.EvaluationHandler, watchExpression);

				watchValueFormatter = ValueDisplayer.CreateSimpleFormatter(expressionRefresher.GetRefreshedValue());
			}
			catch (NuGenParserException parserException)
			{
				watchValueFormatter = new NuGenErrorValueFormatter("Parser exception", parserException.Message);
			}
			catch (NuGenEvaluationException evaluationException)
			{
				watchValueFormatter = new NuGenErrorValueFormatter("Evaluation exception", evaluationException.Message);
			}
			catch (NuGenEvaluationHandlerException evaluationHandlerException)
			{
				watchValueFormatter = new NuGenErrorValueFormatter("Evaluation running exception", evaluationHandlerException.Message);
			}
			catch (NugenMissingModuleException missingModuleException)
			{
				watchValueFormatter = new NuGenMissingModuleFormatter(missingModuleException.MissingModule);
			}
			catch (InvalidOperationException invalidOperationException)
			{
				watchValueFormatter = new NuGenErrorValueFormatter("Evaluation exception", invalidOperationException.Message);
			}
			catch (Exception exception)
			{
				watchValueFormatter = new NuGenErrorValueFormatter("Unexpected exception", exception.Message);
			}

			if (watchValueFormatter != null)
			{
				watchValueFormatter.ValueRefresher = expressionRefresher;
				watchRow.Cells[1].Value = watchValueFormatter.GetFormattedString(NuGenSettings.Instance.DisplayHexaNumbers);
                NuGenHelperFunctions.TaggedObjects[(int)watchRow.Cells[2].Value] = watchValueFormatter;				
			}
		}

		private void DisplayWatchExpressions()
		{
			for (int index = 0; index < objectsGrid.RowCount; index++)
			{
				GridEXRow watchRow = objectsGrid.GetRow(index);
                if(watchRow.Cells.Count == 0)
                {
					DisplayWatchExpression(watchRow);
				}
			}
		}

		private void ShowObject(NuGenBaseValueRefresher refresher)
		{
			List<NuGenBaseValueRefresher> objects = new List<NuGenBaseValueRefresher>(1);
			objects.Add(refresher);

			ShowObjects(objects);
		}

		private void ShowObjects(List<NuGenBaseValueRefresher> objects)
		{
			for(int index = 0; index < objects.Count; index++)
			{
				NuGenBaseValueRefresher valueRefresher = objects[index];
				NuGenIValueFormatter valueFormatter = null;

				try
				{
					NuGenDebugExpressionResult debugValue = new NuGenDebugExpressionResult(evaluationContext, valueRefresher.GetRefreshedValue());
					valueFormatter = ValueDisplayer.CreateSimpleFormatter(debugValue);
				}
				catch (Exception exception)
				{
					valueFormatter = new NuGenStringValueFormatter(exception.ToString());
				}

				valueFormatter.Name = valueRefresher.Name;
				valueFormatter.ValueRefresher = valueRefresher;

				AddValueFormatter(valueFormatter);
			}
		}

		private void objectsGrid_CellDoubleClick(object sender, RowActionEventArgs e)
		{
			if (objectsGrid.CurrentRow != null)
			{
				NuGenIValueFormatter valueFormatter = NuGenHelperFunctions.TaggedObjects[(int)objectsGrid.CurrentRow.Cells[2].Value] as NuGenIValueFormatter;

				if (valueFormatter != null)
				{
					NuGenUIHandler.Instance.ShowObjectInObjectViewer(ActiveFrame, valueFormatter.ValueRefresher, (string)objectsGrid.CurrentRow.Cells[0].Value);
				}
			}
		}

		private void objectsGrid_CellEndEdit(object sender, ColumnActionEventArgs e)
		{
			if (NuGenDebugEventHandler.Instance.EventObjects.Thread != null && ActiveFrame != null && objectsGrid.SelectedItems[0].GetRow().RowIndex < objectsGrid.RowCount)
			{
				try
				{
					DisplayWatchExpression(objectsGrid.SelectedItems[0].GetRow());
				}
				catch (Exception exception)
				{
					NuGenUIHandler.Instance.ShowException(exception);
					NuGenUIHandler.Instance.DisplayUserWarning(exception.Message);
				}
				finally
				{
					try
					{
						ActiveFrame = ActiveFrameRefresher.GetRefreshedValue();
						NuGenDebugEventHandler.Instance.EventObjects.Frame = ActiveFrame;
					}
					catch (Exception exception)
					{
						NuGenUIHandler.Instance.ShowException(exception);
						NuGenUIHandler.Instance.DisplayUserWarning(exception.Message);
					}
				}
			}
		}
	}
}