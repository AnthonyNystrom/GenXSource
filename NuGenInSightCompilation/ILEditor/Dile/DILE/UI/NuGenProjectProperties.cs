using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration.UI;
using Dile.Disassemble;

namespace Dile.UI
{
	public partial class NuGenProjectProperties : Form
	{
		public NuGenProjectProperties()
		{
			InitializeComponent();
		}

		public DialogResult DisplaySettings()
		{
			List<NuGenBaseSettingsDisplayer> settingsDisplayers = new List<NuGenBaseSettingsDisplayer>(1);
			settingsDisplayers.Add(new NuGenGeneralProjectSettingsDisplayer());
			settingsDisplayers.Add(new NuGenProjectStartupSettingsDisplayer());
			settingsDisplayers.Add(new NuGenProjectExceptionSettingsDisplayer());
			settingsDisplayers.Add(new NuGenProjectDebuggingSettingsDisplayer());
			settingsDisplayerControl.DisplaySettings(settingsDisplayers);

			return ShowDialog();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			settingsDisplayerControl.UpdateSettings();
			NuGenProject.Instance.IsSaved = false;
			DialogResult = DialogResult.OK;
		}
	}
}