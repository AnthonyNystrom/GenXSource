using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SmoothyInterface.Forms
{
	public partial class NewEventLog : Form
	{
		string machineName;

		public NewEventLog(string machineName_)
		{
			InitializeComponent();
			machineName = machineName_;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			if (ValidateUserInput())
			{
				try
				{
					if (EventLogExists(tbEventLogName.Text)) {
						MessageBox.Show("This log allready exists on " + machineName + ".", "Smoothy", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					}
					else {
						EventSourceCreationData data = new EventSourceCreationData(tbSourceName.Text, tbEventLogName.Text);
						data.MachineName = machineName;

						EventLog.CreateEventSource(data);

						this.DialogResult = DialogResult.OK;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to create event log : " + ex.Message, "Smoothy", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private bool EventLogExists(string logName)
		{
			EventLog[] logs = EventLog.GetEventLogs(machineName);

			for (int i = 0; i < logs.Length; i++) {
				if (logs[i].LogDisplayName == logName)
				{
					return true;
				}
			}

			return false;
		}

		private bool ValidateUserInput()
		{
			if (tbEventLogName.Text == String.Empty)
			{
				errorProvider.SetError(tbEventLogName, "Event Log name is mandatory.");
				return false;
			}

			if (tbSourceName.Text == String.Empty)
			{
				errorProvider.SetError(tbSourceName, "Source name is mandatory.");
				return false;
			}

			return true;
		}
	}
}