using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Disassemble;

namespace Dile.UI
{
	public partial class NuGenQuickSearchPanel : NuGenBasePanel
	{
		private const int ItemTypeColumnWidth = 100;

		private NuGenQuickSearch quickFinder = null;
		private NuGenQuickSearch QuickFinder
		{
			get
			{
				return quickFinder;
			}
			set
			{
				quickFinder = value;
			}
		}

		private Dictionary<NuGenAssembly, ListViewGroup> foundAssemblies;
		private Dictionary<NuGenAssembly, ListViewGroup> FoundAssemblies
		{
			get
			{
				return foundAssemblies;
			}
			set
			{
				foundAssemblies = value;
			}
		}

		private ToolStripMenuItem displayItemMenuItem;
		private ToolStripMenuItem DisplayItemMenuItem
		{
			get
			{
				return displayItemMenuItem;
			}
			set
			{
				displayItemMenuItem = value;
			}
		}

		private ToolStripMenuItem locateInProjectExplorerMenuItem;
		private ToolStripMenuItem LocateInProjectExplorerMenuItem
		{
			get
			{
				return locateInProjectExplorerMenuItem;
			}
			set
			{
				locateInProjectExplorerMenuItem = value;
			}
		}

		public NuGenQuickSearchPanel()
		{
			InitializeComponent();

			foundItemsList.Initialize();

			DisplayItemMenuItem = new ToolStripMenuItem("Display item");
			DisplayItemMenuItem.Click += new EventHandler(DisplayItemMenuItem_Click);
			foundItemsList.ItemContextMenu.Items.Insert(0, DisplayItemMenuItem);

			LocateInProjectExplorerMenuItem = new ToolStripMenuItem("Locate in Project Explorer");
			LocateInProjectExplorerMenuItem.Click += new EventHandler(LocateInProjectExplorerMenuItem_Click);
			foundItemsList.ItemContextMenu.Items.Insert(1, LocateInProjectExplorerMenuItem);
		}

		private void DisplayItemMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeObject();
		}

		private void LocateInProjectExplorerMenuItem_Click(object sender, EventArgs e)
		{
			if (foundItemsList.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = foundItemsList.SelectedItems[0];
				NuGenTokenBase tokenObject = selectedItem.Tag as NuGenTokenBase;

				if (tokenObject != null)
				{
					//UIHandler.Instance.MainForm.ProjectExplorer.LocateTokenNode(tokenObject);
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

			foundItemsList.Items.Clear();
		}

		private void FoundItem(object sender, NuGenFoundItemEventArgs eventArgs)
		{
			if (sender != QuickFinder)
			{
				eventArgs.Cancel = true;
			}
			else
			{
				ListViewGroup assemblyGroup = (FoundAssemblies.ContainsKey(eventArgs.Assembly) ? FoundAssemblies[eventArgs.Assembly] : null);

				if (assemblyGroup == null)
				{
					assemblyGroup = new ListViewGroup(eventArgs.Assembly.Name);
					foundItemsList.Groups.Add(assemblyGroup);
					FoundAssemblies[eventArgs.Assembly] = assemblyGroup;
				}

				ListViewItem foundItem = new ListViewItem(new string[] { eventArgs.FoundTokenObject.Name, Convert.ToString(eventArgs.FoundTokenObject.ItemType) });
				foundItem.Group = assemblyGroup;
				foundItem.ToolTipText = eventArgs.FoundTokenObject.Name;
				foundItem.Tag = eventArgs.FoundTokenObject;
				foundItemsList.Items.Add(foundItem);
			}
		}

		public void ShowItems()
		{
			searchText.Text = string.Empty;
			ShowItems(string.Empty);
		}

		public void ShowItems(string searchText)
		{
			foundItemsList.Items.Clear();
			foundItemsList.Groups.Clear();
			FoundAssemblies = new Dictionary<NuGenAssembly, ListViewGroup>();

			QuickFinder = new NuGenQuickSearch(this, new FoundItem(FoundItem));
			QuickFinder.StartSearch(searchText);
		}

		private void settingsButton_Click(object sender, EventArgs e)
		{
			NuGenQuickSearchSettingsForm settingsForm = new NuGenQuickSearchSettingsForm();
			settingsForm.ShowDialog();
		}

		private void searchText_TextChanged(object sender, EventArgs e)
		{
			ShowItems(searchText.Text);
		}

		private void ShowCodeObject()
		{
			if (foundItemsList.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = foundItemsList.SelectedItems[0];
				NuGenIMultiLine codeObject = selectedItem.Tag as NuGenIMultiLine;

				if (codeObject != null)
				{
					NuGenUIHandler.Instance.ShowCodeObject(codeObject, new NuGenCodeObjectDisplayOptions());
				}
			}
		}

		private void foundItemsList_DoubleClick(object sender, EventArgs e)
		{
			ShowCodeObject();
		}

		private void foundItemsList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
			{
				ShowCodeObject();
			}
		}
		
		private void foundItemsList_Resize(object sender, EventArgs e)
		{
			itemTypeColumnHeader.Width = ItemTypeColumnWidth;
			itemNameColumnHeader.Width = foundItemsList.Width - ItemTypeColumnWidth;
		}

		private void QuickSearchPanel_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
			{
				searchText.Focus();
			}
		}

		private void searchText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Down)
			{
				foundItemsList.Focus();

				if (foundItemsList.FocusedItem == null && foundItemsList.Items.Count > 0)
				{
					foundItemsList.Items[0].Focused = true;
				}

				if (foundItemsList.FocusedItem != null)
				{
					foundItemsList.FocusedItem.Selected = true;
				}
			}
			else if (e.KeyData == Keys.Escape)
			{
				QuickFinder = null;
			}
		}
	}
}