using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using SmoothyInterface.Forms;
using System.Reflection;
using EventMonitor.Properties;
using Janus.Windows.UI.Tab;

namespace SmoothyInterface
{
	public partial class NuGenEventMonitorPanel : UserControl
	{		

		public NuGenEventMonitorPanel()
		{
			InitializeComponent();
		}

		public void OpenClicked()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				string FileName = openFileDialog.FileName;
				// TODO: Add code here to open the file.
			}
		}

		public void SaveClicked()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				string FileName = saveFileDialog.FileName;
				// TODO: Add code here to save the current contents of the form to a file.
			}
		}
		
		private void BuildTree()
		{
			BuildMachineNode(System.Net.Dns.GetHostName());
		}

		private void BuildMachineNode(string hostName)
		{
			try
			{
				TreeNode rootNode = new TreeNode(hostName);
				rootNode.ImageIndex = 0;
				rootNode.SelectedImageIndex = 0;

				BuildEventLogNodes(rootNode);

				treeViewLogs.Nodes.Add(rootNode);
				rootNode.Expand();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to connect to computer " + hostName + " : " + ex.Message, "Smoothy", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BuildMachineNode(string hostName, EventLog[] logs)
		{
			try
			{
				TreeNode rootNode = new TreeNode(hostName);
				rootNode.ImageIndex = 0;
				rootNode.SelectedImageIndex = 0;

				BuildEventLogNodes(rootNode);

				treeViewLogs.Nodes.Add(rootNode);
				rootNode.Expand();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to connect to computer " + hostName + " : " + ex.Message, "Smoothy", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void treeViewLogs_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null)
			{
				// Check that this is not the root node
				if (e.Node.Tag != null)
				{
					
				}
			}
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{
			BuildTree();
		}

		private void treeViewLogs_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node != null)
			{
				PerformNodeActivation(e.Node);
			}
		}

		private void OpenChild(EventLog log)
		{
			this.Enabled = false;

			EventLogViewer viewer = FindChildForLog(log);

			if (viewer != null)
			{
				viewer.BringToFront();
				viewer.Focus();
			}
			else
			{
				EventLogViewer form = null;

				try
				{
					form = new EventLogViewer(log);

					if (!form.IsDisposed)
					{
                        UITabPage tab = new UITabPage(log.LogDisplayName);
                        tab.Controls.Add(form);
                        form.Dock = DockStyle.Fill;
                        this.uiTab1.TabPages.Add(tab);						
						form.Show();

						form.BringToFront();
						form.Focus();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to open Event Log : " + ex.Message, "Smoothy", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			this.Enabled = true;
		}

		private bool AreLogsTheSame(EventLog a, EventLog b)
		{
			return ((a.LogDisplayName == b.LogDisplayName) && (a.MachineName == b.MachineName));				
		}

		private EventLogViewer FindChildForLog(EventLog log)
		{
            foreach (UITabPage tab in uiTab1.TabPages)
            {
                foreach (Control c in tab.Controls)
                {
                    if (c is EventLogViewer)
                    {
                        EventLogViewer viewer = c as EventLogViewer;
                        if (AreLogsTheSame(viewer.Log, log))
                        {
                            return viewer;
                        }
                    }
                }               
            }

            return null;
		}

		public void OptionsClicked()
		{
			Options form = new Options();
			form.ShowDialog();
		}

		private void treeViewLogs_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				treeViewLogs.SelectedNode = e.Node;
			}
		}

		private void ClearLog(EventLog log)
		{
			log.Clear();

			EventLogViewer child = FindChildForLog(log);

			if (child != null)
			{
				child.BringToFront();
				child.Focus();
				child.RefreshGrid();
			}
		}

		private void clearEventLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = treeViewLogs.SelectedNode;

			if ((selectedNode != null) && (selectedNode.Tag != null))
			{
				EventLog log = selectedNode.Tag as EventLog;

				if (Settings.Default.ShowConfirmationsForCleaningEventLogs)
				{
					if (MessageBox.Show("Are you sure you want to clear this log?", "Smoothy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						ClearLog(log);
					}
				}
				else
				{
					ClearLog(log);
				}
			}			
		}

		private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClearAllEventLogs();
		}

		private void ClearAllEventLogs()
		{			
			if (MessageBox.Show("Are you sure you want to clear all of these eventlogs?", "Smoothy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				EventLog[] logs = System.Diagnostics.EventLog.GetEventLogs();

				for (int i = 0; i < logs.Length; i++)
				{
					ClearLog(logs[i]);
				}
			}
		}

		private void deleteEventLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete this log?", "Smoothy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				TreeNode selectedNode = treeViewLogs.SelectedNode;

				if ((selectedNode != null) && (selectedNode.Tag != null))
				{
					EventLog log = selectedNode.Tag as EventLog;
					EventLogViewer viewer = FindChildForLog(log);

					try {
						EventLog.Delete(log.LogDisplayName, log.MachineName);
					}
					catch (Exception ex) {
						MessageBox.Show("Could not delete log : " + ex.Message, "Smoothy", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}					
				}

				RefreshEventLogs(selectedNode.Parent.Parent);
			}
		}

		private void treeContextMenu_Opening(object sender, CancelEventArgs e)
		{
			TreeNode selectedNode = treeViewLogs.SelectedNode;

			if ((selectedNode != null) && (selectedNode.Tag != null))
			{
				EventLog log = selectedNode.Tag as EventLog;

				if (IsStandardLog(log))
				{
					if (Settings.Default.SafeGuardDeletionOfStandardLogs)
					{
						deleteEventLogToolStripMenuItem.Enabled = false;
					}
					else
					{
						deleteEventLogToolStripMenuItem.Enabled = true;
					}
				}
				else
				{
					deleteEventLogToolStripMenuItem.Enabled = true;
				}
			}
		}

		private bool IsStandardLog(EventLog log)
		{
			if ((log.LogDisplayName == "Application") ||
				(log.LogDisplayName == "System") ||
				(log.LogDisplayName == "Security"))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public void NewClicked()
		{
			if (treeViewLogs.SelectedNode != null) {
				if (treeViewLogs.SelectedNode.Parent != null)
				{
					// Pass in the machine name
					NewEventLog log = new NewEventLog(treeViewLogs.SelectedNode.Parent.Text);

					// Check if a log has actually been created
					if (log.ShowDialog() == DialogResult.OK)
					{
						// Refresh the event logs under the machine
						RefreshEventLogs(treeViewLogs.SelectedNode.Parent);
					}
				}
			}
		}

		private void RefreshEventLogs(TreeNode MachineNode)
		{
			MachineNode.Nodes.Clear();
			BuildEventLogNodes(MachineNode);
			MachineNode.Expand();
			MachineNode.FirstNode.Expand();
		}

		private void BuildEventLogNodes(TreeNode MachineNode)
		{			
			// Add the eventlog node
			TreeNode eventLogsNode = new TreeNode("Event Logs");
			eventLogsNode.ImageIndex = 2;
			eventLogsNode.SelectedImageIndex = 2;
			eventLogsNode.ContextMenuStrip = computerContextMenuStrip;
			MachineNode.Nodes.Add(eventLogsNode);

			EventLog[] eventLogs = System.Diagnostics.EventLog.GetEventLogs(MachineNode.Text);

			for (int i = 0; i < eventLogs.Length; i++)
			{
				TreeNode eventLogNode = new TreeNode(eventLogs[i].LogDisplayName, 1, 1);
				eventLogNode.Tag = eventLogs[i];
				eventLogNode.ContextMenuStrip = treeContextMenu;
				eventLogsNode.Nodes.Add(eventLogNode);
			}
		}

		public void ConnectClicked()
		{
			List<string> ignoreComputers = GetCurrentlyConnectedComputers();

			ConnectToComputer form = new ConnectToComputer(ignoreComputers);

			if (form.ShowDialog() == DialogResult.OK)
			{
				// Build the new computer node
				BuildMachineNode(form.MachineNameSpecified);

			}
		}

        public void OpenLog(object sender, EventArgs e)
        {
            TreeNode node = this.treeViewLogs.SelectedNode;
            try
            {
                // Check that this is not the root node
                if (node.Tag != null)
                {
                    if (node.Tag.GetType() == typeof(EventLog))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        EventLog log = node.Tag as EventLog;
                        OpenChild(log);
                    }
                }
            }
            catch { throw; }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

		private List<string> GetCurrentlyConnectedComputers()
		{
			List<string> ret = new List<string>();

			for (int i = 0; i < treeViewLogs.Nodes.Count; i++)
			{
				ret.Add(treeViewLogs.Nodes[i].Text);
			}

			return ret;
		}

		private void treeViewLogs_KeyUp(object sender, KeyEventArgs e)
		{
			if (treeViewLogs.SelectedNode != null)
			{
				if (e.KeyCode == Keys.Enter)
				{
					PerformNodeActivation(treeViewLogs.SelectedNode);
				}				
			}

			e.Handled = false;

		}

		private void PerformNodeActivation(TreeNode node)
		{
			try
			{
				// Check that this is not the root node
				if (node.Tag != null)
				{
					if (node.Tag.GetType() == typeof(EventLog))
					{
						this.Cursor = Cursors.WaitCursor;
						EventLog log = node.Tag as EventLog;
						OpenChild(log);
					}
				}
			}
			catch { throw; }
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void refreshEventLogsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = treeViewLogs.SelectedNode;

			if (selectedNode != null)
			{
				RefreshEventLogs(selectedNode.Parent);
			}
		}

        private void uiTab1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                uiTab1.TabPages.Remove(uiTab1.SelectedTab);
            }
        }

        private void uiTab1_TabClosed(object sender, TabEventArgs e)
        {
            uiTab1.TabPages.Remove(e.Page);
        }
	}
}
