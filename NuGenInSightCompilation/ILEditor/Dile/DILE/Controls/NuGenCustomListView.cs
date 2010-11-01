using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using System.Windows.Forms;

namespace Dile.Controls
{
	public class NuGenCustomListView : ListView
	{
		private ContextMenuStrip itemContextMenu;
		public ContextMenuStrip ItemContextMenu
		{
			get
			{
				return itemContextMenu;
			}
			private set
			{
				itemContextMenu = value;
			}
		}

		public NuGenCustomListView()
		{
			FullRowSelect = true;
		}

		private void InitializeItemContextMenu()
		{
			ItemContextMenu = new ContextMenuStrip();
			ToolStripMenuItem copyMenu = (ToolStripMenuItem)ItemContextMenu.Items.Add("Copy to clipboard");
			ToolStripMenuItem displayTextMenu = (ToolStripMenuItem)ItemContextMenu.Items.Add("Display text...");
			
			for (int index = 0; index < Columns.Count; index++)
			{
				ColumnHeader columnHeader = Columns[index];

				ToolStripItem copyChildMenu = copyMenu.DropDownItems.Add(columnHeader.Text);
				copyChildMenu.Click += new EventHandler(copyMenu_Click);
				copyChildMenu.Tag = columnHeader;

				ToolStripItem displayTextChildMenu = displayTextMenu.DropDownItems.Add(columnHeader.Text);
				displayTextChildMenu.Click += new EventHandler(displayTextMenu_Click);
				displayTextChildMenu.Tag = columnHeader;
			}

			if (Columns.Count > 1)
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
			ListViewItem currentItem = SelectedItems[0];
			StringBuilder result = new StringBuilder();

			if (menuItem.Tag == null)
			{
				for (int index = 0; index < currentItem.SubItems.Count; index++)
				{
					result.Append(currentItem.SubItems[index].Text);

					if (index < currentItem.SubItems.Count - 1)
					{
						result.Append("\t");
					}
				}
			}
			else
			{
				ColumnHeader sourceColumn = (ColumnHeader)menuItem.Tag;
				result.Append(currentItem.SubItems[sourceColumn.Index].Text);
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
				ListViewItem currentItem = GetItemAt(e.X, e.Y);

				if (currentItem != null)
				{
					currentItem.Selected = true;
					ItemContextMenu.Show(this, e.Location);
				}
			}
		}
	}
}