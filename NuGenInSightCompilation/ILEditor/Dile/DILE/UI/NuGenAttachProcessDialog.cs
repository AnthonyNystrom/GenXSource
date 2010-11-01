using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Debug;
using System.Diagnostics;

namespace Dile.UI
{
	public partial class NuGenAttachProcessDialog : Form
	{
		private uint processID = 0;
		public uint ProcessID
		{
			get
			{
				return processID;
			}
			private set
			{
				processID = value;
			}
		}

		private ToolStripMenuItem attachProcessMenuItem;
		private ToolStripMenuItem AttachProcessMenuItem
		{
			get
			{
				return attachProcessMenuItem;
			}
			set
			{
				attachProcessMenuItem = value;
			}
		}

		private ToolStripMenuItem refreshProcessesMenuItem;
		private ToolStripMenuItem RefreshProcessesMenuItem
		{
			get
			{
				return refreshProcessesMenuItem;
			}
			set
			{
				refreshProcessesMenuItem = value;
			}
		}

		public NuGenAttachProcessDialog()
		{
			InitializeComponent();

			managedProcessesGrid.Initialize();

			AttachProcessMenuItem = new ToolStripMenuItem("Attach to process");
			AttachProcessMenuItem.Click += new EventHandler(AttachProcessMenuItem_Click);
			managedProcessesGrid.RowContextMenu.Items.Insert(0, AttachProcessMenuItem);

			RefreshProcessesMenuItem = new ToolStripMenuItem("Refresh processes");
			RefreshProcessesMenuItem.Click += new EventHandler(RefreshProcessesMenuItem_Click);
			managedProcessesGrid.RowContextMenu.Items.Insert(1, RefreshProcessesMenuItem);
		}

		private void SetProcessID()
		{
			if (managedProcessesGrid.CurrentRow != null)
			{
				ProcessID = uint.Parse(managedProcessesGrid.CurrentRow.Cells[0].Text);

				DialogResult = DialogResult.OK;
			}
		}

		private void AttachProcessDialog_Load(object sender, EventArgs e)
		{
			ShowProcesses();
		}

		private void ShowProcesses()
		{
			managedProcessesGrid.BeginGridUpdate();
            managedProcessesGrid.ClearItems();
			ProcessEnumerator processEnumerator = new ProcessEnumerator();
			List<ProcessInformation> managedProcesses = processEnumerator.GetManagedProcesses();
			uint dileProcessID = Convert.ToUInt32(Process.GetCurrentProcess().Id);

			foreach (ProcessInformation processInformation in managedProcesses)
			{
				if (processInformation.ID != dileProcessID)
				{
					Process process = Process.GetProcessById(Convert.ToInt32(processInformation.ID));
					string processFramework = Dile.Debug.Debugger.GetVersionFromProcess(process.Handle);

                    managedProcessesGrid.AddItem(new object[] { processInformation.ID, processInformation.Name, processFramework });
				}
			}

			managedProcessesGrid.EndGridUpdate();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			SetProcessID();
		}

		private void managedProcessesGrid_CellDoubleClick(object sender, EventArgs e)
		{
			SetProcessID();
		}

		private void AttachProcessMenuItem_Click(object sender, EventArgs e)
		{
			SetProcessID();
		}

		private void RefreshProcessesMenuItem_Click(object sender, EventArgs e)
		{
			ShowProcesses();
		}

		private void refreshButton_Click(object sender, EventArgs e)
		{
			ShowProcesses();
		}

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	}
}