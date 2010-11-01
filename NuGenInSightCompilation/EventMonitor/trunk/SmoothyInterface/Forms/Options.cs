using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EventMonitor.Properties;
using SmoothyInterface.Enum;

namespace SmoothyInterface.Forms
{
	public partial class Options : Form
	{
		public Options()
		{
			InitializeComponent();
		}

		private void Options_Load(object sender, EventArgs e)
		{
			FillVisualStyle();			
		}

		private void FillVisualStyle()
		{
			cbEventDistinction.Items.Clear();

			string[] names = System.Enum.GetNames(typeof(VisualEventDistinctionType));
			
			for (int i = 0; i < names.Length; i++)
			{
				cbEventDistinction.Items.Add(names[i]);
			}

			cbEventDistinction.SelectedValue = Settings.Default.ColorMode;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SaveSettings();
			this.Close();
		}

		private void SaveSettings()
		{
			Settings.Default.ColorMode = cbEventDistinction.SelectedItem.ToString();
			Settings.Default.Save();
		}

		private void ShowColorDialogForRows(ref Panel panel)
		{
			colorDialog.Color = panel.BackColor;

			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				panel.BackColor = colorDialog.Color;
			}
		}

		private void btnChooseInformation_Click(object sender, EventArgs e)
		{
			ShowColorDialogForRows(ref panelColourInformation);
		}

		private void btnChooseWarning_Click(object sender, EventArgs e)
		{
			ShowColorDialogForRows(ref panelColorWarning);
		}

		private void btnChooseError_Click(object sender, EventArgs e)
		{
			ShowColorDialogForRows(ref panelColorError);
		}

		private void btnChooseSucces_Click(object sender, EventArgs e)
		{
			ShowColorDialogForRows(ref panelColorSuccessAudit);
		}

		private void btnFailureAudit_Click(object sender, EventArgs e)
		{
			ShowColorDialogForRows(ref panelColorFailureAudit);
        }

        private void uiGroupBox1_Click(object sender, EventArgs e)
        {

        }
	}
}