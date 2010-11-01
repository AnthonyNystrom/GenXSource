using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public partial class NuGenSettingsDialog : Form
	{
		public NuGenSettingsDialog()
		{
			InitializeComponent();
		}

		public DialogResult DisplaySettings()
		{
			List<NuGenBaseSettingsDisplayer> settingsDisplayers = new List<NuGenBaseSettingsDisplayer>(4);
			settingsDisplayers.Add(new NuGenDebuggingSettingsDisplayer());
			settingsDisplayers.Add(new NuGenDisplayedDebuggingEventsDisplayer());
			settingsDisplayerControl.DisplaySettings(settingsDisplayers);

			return ShowDialog();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			settingsDisplayerControl.UpdateSettings();
			NuGenSettings.Instance.SettingsUpdated();

			DialogResult = DialogResult.OK;
		}
	}
}