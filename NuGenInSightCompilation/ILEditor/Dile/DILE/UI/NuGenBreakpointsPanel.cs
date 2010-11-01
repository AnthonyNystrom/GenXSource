using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Disassemble;
using Dile.UI.Debug;
using Janus.Windows.GridEX;

namespace Dile.UI
{
	public partial class NuGenBreakpointsPanel : NuGenBasePanel
	{
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

		private ToolStripMenuItem activateBreakpointMenuItem;
		private ToolStripMenuItem ActivateBreakpointMenuItem
		{
			get
			{
				return activateBreakpointMenuItem;
			}
			set
			{
				activateBreakpointMenuItem = value;
			}
		}

		private ToolStripMenuItem deactivateBreakpointMenuItem;
		private ToolStripMenuItem DeactivateBreakpointMenuItem
		{
			get
			{
				return deactivateBreakpointMenuItem;
			}
			set
			{
				deactivateBreakpointMenuItem = value;
			}
		}

		private ToolStripMenuItem removeBreakpointMenuItem;
		private ToolStripMenuItem RemoveBreakpointMenuItem
		{
			get
			{
				return removeBreakpointMenuItem;
			}
			set
			{
				removeBreakpointMenuItem = value;
			}
		}

		public NuGenBreakpointsPanel()
		{
			InitializeComponent();

			breakpointsGrid.Initialize();

			DisplayCodeMenuItem = new ToolStripMenuItem("Display breakpoint location");
			DisplayCodeMenuItem.Click += new EventHandler(DisplayCodeMenuItem_Click);
			breakpointsGrid.RowContextMenu.Items.Insert(0, DisplayCodeMenuItem);

			ActivateBreakpointMenuItem = new ToolStripMenuItem("Activate breakpoint");
			ActivateBreakpointMenuItem.Click += new EventHandler(ActivateBreakpointMenuItem_Click);
			breakpointsGrid.RowContextMenu.Items.Insert(1, ActivateBreakpointMenuItem);

			DeactivateBreakpointMenuItem = new ToolStripMenuItem("Deactivate breakpoint");
			DeactivateBreakpointMenuItem.Click += new EventHandler(DeactivateBreakpointMenuItem_Click);
			breakpointsGrid.RowContextMenu.Items.Insert(2, DeactivateBreakpointMenuItem);

			RemoveBreakpointMenuItem = new ToolStripMenuItem("Remove breakpoint");
			RemoveBreakpointMenuItem.Click += new EventHandler(RemoveBreakpointMenuItem_Click);
			breakpointsGrid.RowContextMenu.Items.Insert(3, RemoveBreakpointMenuItem);

			breakpointsGrid.RowContextMenu.Opening += new CancelEventHandler(RowContextMenu_Opening);
		}

		private void RowContextMenu_Opening(object sender, CancelEventArgs e)
		{
			if (breakpointsGrid.SelectedItems.Count == 1)
			{
                GridEXRow row = breakpointsGrid.SelectedItems[0].GetRow();
                NuGenBreakpointInformation breakpoint = NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value] as NuGenBreakpointInformation;
				bool breakpointActive = (breakpoint.State == BreakpointState.Active);

				ActivateBreakpointMenuItem.Visible = !breakpointActive;
				DeactivateBreakpointMenuItem.Visible = breakpointActive;
			}
		}

		private void DisplayCodeMenuItem_Click(object sender, EventArgs e)
		{
			if (breakpointsGrid.SelectedItems.Count == 1)
			{
                GridEXRow row = breakpointsGrid.SelectedItems[0].GetRow();
                NuGenBreakpointInformation breakpoint = NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value] as NuGenBreakpointInformation;

				if (breakpoint != null)
				{
					breakpoint.NavigateTo();
				}
			}
		}

		private void ActivateBreakpointMenuItem_Click(object sender, EventArgs e)
		{
			if (breakpointsGrid.SelectedItems.Count == 1)
			{
                GridEXRow row = breakpointsGrid.SelectedItems[0].GetRow();
                NuGenFunctionBreakpointInformation functionBreakpoint = NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value] as NuGenFunctionBreakpointInformation;

				if (functionBreakpoint != null)
				{
					ActivateBreakpoint(functionBreakpoint);
					NuGenUIHandler.Instance.UpdateBreakpoint(functionBreakpoint.MethodDefinition, functionBreakpoint);
				}
			}
		}

		private void DeactivateBreakpointMenuItem_Click(object sender, EventArgs e)
		{
			if (breakpointsGrid.SelectedItems.Count == 1)
			{
                GridEXRow row = breakpointsGrid.SelectedItems[0].GetRow();
                NuGenFunctionBreakpointInformation functionBreakpoint = NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value] as NuGenFunctionBreakpointInformation;

				if (functionBreakpoint != null)
				{
					DeactivateBreakpoint(functionBreakpoint);
					NuGenUIHandler.Instance.UpdateBreakpoint(functionBreakpoint.MethodDefinition, functionBreakpoint);
				}
			}
		}

		private void RemoveBreakpointMenuItem_Click(object sender, EventArgs e)
		{
			if (breakpointsGrid.SelectedItems.Count == 1)
			{
                GridEXRow row = breakpointsGrid.SelectedItems[0].GetRow();
                NuGenFunctionBreakpointInformation functionBreakpoint = NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value] as NuGenFunctionBreakpointInformation;

				if (functionBreakpoint != null)
				{
                    RowDelete(breakpointsGrid.SelectedItems[0].GetRow());
                    breakpointsGrid.SelectedItems[0].GetRow().Delete();
				}
			}
		}

		protected override bool IsDebugPanel()
		{
			return false;
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

            breakpointsGrid.ClearItems();
		}

		public void DisplayBreakpoints()
		{
			breakpointsGrid.BeginGridUpdate();
            breakpointsGrid.ClearItems();

			foreach (NuGenFunctionBreakpointInformation functionBreakpoint in NuGenProject.Instance.FunctionBreakpoints)
			{
				AddBreakpoint(functionBreakpoint);
			}

			breakpointsGrid.EndGridUpdate();
		}

        private GridEXRow test;
		public void AddBreakpoint(NuGenBreakpointInformation breakpoint)
		{
            breakpointsGrid.AddItem();
            GridEXRow row = breakpointsGrid.GetRow(breakpointsGrid.RowCount - 1);
            row.BeginEdit();
            test = row;

			row.Cells[0].Value = (breakpoint.State == BreakpointState.Active);
			row.Cells[1].Value = breakpoint.DisplayName;
			row.Cells[2].Value = breakpoint.OffsetValue;
            NuGenHelperFunctions.TaggedObjects.Add((String)row.Cells[1].Value + (String)row.Cells[2].Value, breakpoint);

            row.EndEdit();
		}

		private GridEXRow FindRow(NuGenBreakpointInformation breakpoint)
		{
            GridEXRow result = null;
			int index = 0;
			bool found = false;

			while (!found && index < breakpointsGrid.RowCount)
			{
                GridEXRow row = breakpointsGrid.GetRow(index++);

                if (breakpoint.CompareTo(NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value]) == 0)
				{
					result = row;
					found = true;
				}
			}

			return result;
		}

		public void DeactivateBreakpoint(NuGenBreakpointInformation breakpoint)
		{
			ToggleBreakpointActive(breakpoint, false);
		}

		public void ActivateBreakpoint(NuGenBreakpointInformation breakpoint)
		{
			ToggleBreakpointActive(breakpoint, true);
		}

		private void ToggleBreakpointActive(NuGenBreakpointInformation breakpoint, bool isActive)
		{
            GridEXRow row = FindRow(breakpoint);
            row.BeginEdit();
			breakpoint.State = (isActive ? BreakpointState.Active : BreakpointState.Inactive);

			if (row != null)
			{
				row.Cells[0].Value = isActive;
			}

            row.EndEdit();
			NuGenProject.Instance.IsSaved = false;
		}

		public void RemoveBreakpoint(NuGenBreakpointInformation breakpoint)
		{
            GridEXRow row = FindRow(breakpoint);
            RowDelete(row);
            row.Delete();			
			NuGenProject.Instance.IsSaved = false;
		}

		private void breakpointsGrid_CellDoubleClick(object sender, EventArgs e)
		{
			if (breakpointsGrid.CurrentRow != null)
			{
                GridEXRow row = breakpointsGrid.CurrentRow;
                NuGenBreakpointInformation breakpoint = NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value] as NuGenBreakpointInformation;

				breakpoint.NavigateTo();
			}
		}

		private void breakpointsGrid_CellContentClick(object sender, EventArgs e)
		{
            //SEE IF WE CAN LIVE WITHOUT THIS
            //if (e.ColumnIndex == 0)
            //{
            //    DataGridViewRow row = breakpointsGrid.Rows[e.RowIndex];
            //    DataGridViewCell cell = row.Cells[e.ColumnIndex];
            //    NuGenBreakpointInformation breakpoint = (NuGenBreakpointInformation)row.Tag;

            //    bool active = !((bool)cell.Value);
            //    breakpoint.State = (active ? BreakpointState.Active : BreakpointState.Inactive);
            //    cell.Value = active;

            //    NuGenFunctionBreakpointInformation functionBreakpoint = breakpoint as NuGenFunctionBreakpointInformation;

            //    if (functionBreakpoint != null)
            //    {
            //        NuGenUIHandler.Instance.UpdateBreakpoint(functionBreakpoint.MethodDefinition, functionBreakpoint);
            //    }

            //    NuGenProject.Instance.IsSaved = false;
            //}
		}

        private void RowDelete(GridEXRow row)
        {
            NuGenFunctionBreakpointInformation functionBreakpoint = NuGenHelperFunctions.TaggedObjects[(String)row.Cells[1].Value + (String)row.Cells[2].Value] as NuGenFunctionBreakpointInformation;

            if (functionBreakpoint != null)
            {
                RemoveBreakpoint(functionBreakpoint);
            }

            NuGenHelperFunctions.TaggedObjects.Remove((String)row.Cells[1].Value + (String)row.Cells[2].Value);
        }

        private static void RemoveBreakpoint(NuGenFunctionBreakpointInformation functionBreakpoint)
        {
            functionBreakpoint.State = BreakpointState.Removed;
            NuGenUIHandler.Instance.UpdateBreakpoint(functionBreakpoint.MethodDefinition, functionBreakpoint);
            NuGenProject.Instance.FunctionBreakpoints.Remove(functionBreakpoint);
            NuGenProject.Instance.IsSaved = false;
        }
	}
}