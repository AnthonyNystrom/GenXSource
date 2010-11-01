using System;
using System.Collections.Generic;
using System.Text;
using Dile.UI;
using System.ComponentModel;
using System.Windows.Forms;
using Janus.Windows.GridEX;

namespace Dile.Controls
{
	public class NuGenCustomDataGridView : GridEX
	{
		private ContextMenuStrip rowContextMenu;
		public ContextMenuStrip RowContextMenu
		{
			get
			{
				return rowContextMenu;
			}
			private set
			{
				rowContextMenu = value;
			}
		}

		public NuGenCustomDataGridView()
		{
            SelectionMode = Janus.Windows.GridEX.SelectionMode.SingleSelection;			
		}

		private void InitializeItemContextMenu()
		{
			RowContextMenu = new ContextMenuStrip();
			ToolStripMenuItem copyMenu = (ToolStripMenuItem)RowContextMenu.Items.Add("Copy to clipboard");
			ToolStripMenuItem displayTextMenu = (ToolStripMenuItem)RowContextMenu.Items.Add("Display text...");
            
            for (int index = 0; index < RootTable.Columns.Count; index++)
			{
                GridEXColumn columnHeader = RootTable.Columns[index];

				ToolStripItem copyChildMenu = copyMenu.DropDownItems.Add(columnHeader.Caption);
				copyChildMenu.Click += new EventHandler(copyMenu_Click);
                copyChildMenu.Tag = columnHeader;

				ToolStripItem displayTextChildMenu = displayTextMenu.DropDownItems.Add(columnHeader.Caption);
				displayTextChildMenu.Click += new EventHandler(displayTextMenu_Click);
                displayTextChildMenu.Tag = columnHeader;
			}

			if (RootTable.Columns.Count > 1)
			{
				copyMenu.DropDownItems.Add("-");
				displayTextMenu.DropDownItems.Add("-");

				ToolStripItem copyFullRowMenu = copyMenu.DropDownItems.Add("Full row");
				copyFullRowMenu.Click += new EventHandler(copyMenu_Click);

				ToolStripItem displayTextFullRowMenu = displayTextMenu.DropDownItems.Add("Full row");
				displayTextFullRowMenu.Click += new EventHandler(displayTextMenu_Click);
			}
		}

		private void copyMenu_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(GetChosenMenuText(sender));
		}

		private void displayTextMenu_Click(object sender, EventArgs e)
		{
			NuGenTextDisplayer.Instance.ShowText(GetChosenMenuText(sender));
		}

		private string GetChosenMenuText(object sender)
		{
			ToolStripItem menuItem = (ToolStripItem)sender;
			GridEXRow selectedRow = SelectedItems[0].GetRow();
			StringBuilder result = new StringBuilder();

			if (menuItem.Tag == null)
			{
				for (int index = 0; index < selectedRow.Cells.Count; index++)
				{
					GridEXCell cell = selectedRow.Cells[index];

					if (cell.Value != null)
					{
						result.Append(Convert.ToString(cell.Value));

						if (index < selectedRow.Cells.Count - 1)
						{
							result.Append("\t");
						}
					}
				}
			}
			else
			{
				GridEXColumn sourceColumn = (GridEXColumn)menuItem.Tag;
				GridEXCell cell = selectedRow.Cells[sourceColumn.Index];

				if (cell.Value != null)
				{
					result.Append(Convert.ToString(cell.Value));
				}
			}

			return result.ToString();
		}

		public void Initialize()
		{
			InitializeItemContextMenu();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
			{
				GridArea hitTest = HitTest(e.X, e.Y);

				if (hitTest == GridArea.Cell)
				{
                    //this.SelectedItems.Add(RowPositionFromPoint(e.X, e.Y));
					RowContextMenu.Show(this, e.Location);
				}
			}
		}

		public void BeginGridUpdate()
		{
            ColumnAutoResize = false;			
		}

		public void EndGridUpdate()
		{
            ColumnAutoResize = true;
            ColumnAutoSizeMode = ColumnAutoSizeMode.AllCells;
		}

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // NuGenCustomDataGridView
            // 
            this.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
	}
}