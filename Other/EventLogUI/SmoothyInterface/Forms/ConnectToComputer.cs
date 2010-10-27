using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EventMonitor.Properties;
using System.Collections.Specialized;
using System.Diagnostics;

namespace SmoothyInterface.Forms
{
	public partial class ConnectToComputer : Form
	{
		#region Globals

		private EventLog[] eventLogsRetrieved = null;
		private string machineName = null;
		private List<string> ignoreComputers = null;

		#endregion

		#region Construction

		public ConnectToComputer(List<string> ignoreComputers_)
		{
			#region Assertions

			Debug.Assert(ignoreComputers_ != null);

			#endregion

			InitializeComponent();

			ignoreComputers = ignoreComputers_;

			LoadComputers();
		}
				
		private void ConnectToComputer_Load(object sender, EventArgs e)
		{

		}

		#endregion

		#region Properties

		public EventLog[] EventLogsRetrieved
		{
			get
			{
				return eventLogsRetrieved;
			}
		}

		public string MachineNameSpecified
		{
			get
			{
				return machineName;
			}
		}

		#endregion

		#region Methods

		private void LoadComputers()
		{
			StringCollection computers = Settings.Default.RemoteComputers;

			if (computers != null)
			{
				for (int i = 0; i < computers.Count; i++)
				{
					dlComputerName.Items.Add(computers[i]);
				}
			}
		}

		#endregion

		#region Events

		private void dlComputerName_TextChanged(object sender, EventArgs e)
		{
			btnConnect.Enabled = (dlComputerName.Text != String.Empty);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;

				if (ignoreComputers.Contains(dlComputerName.Text))
				{
					MessageBox.Show("A connection to this computer is allready open.", "Smoothy", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{

					if (Settings.Default.RemoteComputers == null)
					{
						Settings.Default.RemoteComputers = new StringCollection();
					}

					EventLog[] logs = EventLog.GetEventLogs(dlComputerName.Text);					
					eventLogsRetrieved = logs;
					machineName = dlComputerName.Text;

					// Save the machine name in the user's config file
					if (!Settings.Default.RemoteComputers.Contains(machineName))
					{
						Settings.Default.RemoteComputers.Add(machineName);
						Settings.Default.Save();
					}

					this.DialogResult = DialogResult.OK;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to connect to remote computer : " + ex.Message, "Smoothy", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		#endregion

	}
}