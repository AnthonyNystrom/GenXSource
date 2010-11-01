using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration.UI;

namespace Dile.Controls
{
	public partial class NuGenSettingsDisplayerControl : UserControl
	{
		public NuGenSettingsDisplayerControl()
		{
			InitializeComponent();
		}

		public void DisplaySettings(List<NuGenBaseSettingsDisplayer> settingsDisplayers)
		{
			try
			{
				settingCategoriesList.BeginUpdate();
				settingCategoriesList.Items.Clear();

				for (int index = 0; index < settingsDisplayers.Count; index++)
				{
					settingCategoriesList.Items.Add(settingsDisplayers[index]);
				}

				settingCategoriesList.SelectedIndex = 0;
			}
			finally
			{
				settingCategoriesList.EndUpdate();
			}
		}

		public void UpdateSettings()
		{
			foreach (NuGenBaseSettingsDisplayer displayer in settingCategoriesList.Items)
			{
				if (displayer.IsInitialized)
				{
					displayer.ReadSettings();
				}
			}
		}

		private void settingCategoriesList_SelectedIndexChanged(object sender, EventArgs e)
		{
			NuGenBaseSettingsDisplayer displayer = settingCategoriesList.SelectedItem as NuGenBaseSettingsDisplayer;

			if (displayer != null)
			{
				settingsPanel.Controls.Clear();
				settingsPanel.RowStyles.Clear();
				settingsPanel.ColumnStyles.Clear();
				displayer.DisplayControls(settingsPanel);
			}
		}
	}
}
