using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class NuGenGeneralSettingsDisplayer : NuGenBaseSettingsDisplayer
	{
		private NumericUpDown recentAssembliesUpDown = null;
		private NumericUpDown RecentAssembliesUpDown
		{
			get
			{
				return recentAssembliesUpDown;
			}
			set
			{
				recentAssembliesUpDown = value;
			}
		}

		private NumericUpDown recentProjectsUpDown = null;
		private NumericUpDown RecentProjectsUpDown
		{
			get
			{
				return recentProjectsUpDown;
			}
			set
			{
				recentProjectsUpDown = value;
			}
		}

		private Label recentAssembliesDirectoryLabel = null;
		private Label RecentAssembliesDirectoryLabel
		{
			get
			{
				return recentAssembliesDirectoryLabel;
			}
			set
			{
				recentAssembliesDirectoryLabel = value;
			}
		}

		private Label recentProjectsDirectoryLabel = null;
		private Label RecentProjectsDirectoryLabel
		{
			get
			{
				return recentProjectsDirectoryLabel;
			}
			set
			{
				recentProjectsDirectoryLabel = value;
			}
		}

		private FolderBrowserDialog folderBrowser;
		private FolderBrowserDialog FolderBrowser
		{
			get
			{
				return folderBrowser;
			}
			set
			{
				folderBrowser = value;
			}
		}

		private NumericUpDown CreateRecentControls(TableLayoutPanel panel, string text, int value)
		{
			NumericUpDown result = null;

			Label label = new Label();
			label.Dock = DockStyle.Fill;
			label.Text = text;
			label.TextAlign = ContentAlignment.MiddleLeft;

			result = new NumericUpDown();
			result.Minimum = 0;
			result.Maximum = 25;
			result.Value = value;
			result.Width = 40;

			panel.Controls.Add(label);
			panel.Controls.Add(result);

			return result;
		}

		private Label CreateDefaultDirectoryControl(TableLayoutPanel panel, string text, string currentValue)
		{
			Label result = null;

			Label textLabel = new Label();
			textLabel.Dock = DockStyle.Fill;
			textLabel.Text = text;
			textLabel.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(textLabel);

			CheckBox enabledCheckBox = new CheckBox();
			enabledCheckBox.Checked = (currentValue != null && currentValue.Length > 0);
			panel.Controls.Add(enabledCheckBox);

			result = new Label();
			result.Dock = DockStyle.Fill;
			result.Text = currentValue;
			result.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(result);

			Button browseButton = new Button();
			browseButton.Enabled = enabledCheckBox.Checked;
			browseButton.Text = "Browse...";
			browseButton.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(browseButton);

			enabledCheckBox.CheckedChanged += new EventHandler(enabledCheckBox_CheckedChanged);
			enabledCheckBox.Tag = browseButton;

			browseButton.Click += new EventHandler(browseButton_Click);
			browseButton.Tag = result;

			return result;
		}

		private void CreateAssociateButton(TableLayoutPanel panel)
		{
			Label descriptionLabel = new Label();
			descriptionLabel.AutoSize = true;
			descriptionLabel.Text = "Associate this instance of DILE with the .dileproj extension.";
			panel.Controls.Add(descriptionLabel);

			Button associateButton = new Button();
			associateButton.AutoSize = true;
			associateButton.Text = "Associate";
			panel.Controls.Add(associateButton);
			associateButton.Click += new EventHandler(associateButton_Click);
		}

		private void CreateDeleteAssociationButton(TableLayoutPanel panel)
		{
			Label descriptionLabel = new Label();
			descriptionLabel.AutoSize = true;
			descriptionLabel.Text = "Delete association between any version of DILE and the .dileproj extension.";
			panel.Controls.Add(descriptionLabel);

			Button dissociateButton = new Button();
			dissociateButton.AutoSize = true;
			dissociateButton.Text = "Dissociate";
			panel.Controls.Add(dissociateButton);
			dissociateButton.Click += new EventHandler(dissociateButton_Click);
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			Button browseButton = (Button)sender;
			Label directoryLabel = (Label)browseButton.Tag;

			if (FolderBrowser == null)
			{
				FolderBrowser = new FolderBrowserDialog();
				FolderBrowser.ShowNewFolderButton = true;
			}

			FolderBrowser.SelectedPath = directoryLabel.Text;

			if (FolderBrowser.ShowDialog() == DialogResult.OK)
			{
				directoryLabel.Text = FolderBrowser.SelectedPath;
			}
		}

		private void enabledCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox enabledCheckBox = (CheckBox)sender;
			Button browseButton = (Button)enabledCheckBox.Tag;
			browseButton.Enabled = enabledCheckBox.Checked;

			if (!enabledCheckBox.Checked)
			{
				Label directoryLabel = (Label)browseButton.Tag;
				directoryLabel.Text = string.Empty;
			}
		}

		private void associateButton_Click(object sender, EventArgs e)
		{
			try
			{
				RegistryKey dileprojKey = Registry.ClassesRoot.CreateSubKey(".dileproj");
				dileprojKey.SetValue(null, "DILE.1");

				RegistryKey dileKey = Registry.ClassesRoot.CreateSubKey("DILE.1").CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command");
				Assembly currentAssembly = Assembly.GetExecutingAssembly();
				dileKey.SetValue(null, string.Format("{0} /l %1", currentAssembly.Location));

				MessageBox.Show(string.Format("This instance of DILE ({0}) has been successfully associated with the .dileproj extension.", currentAssembly.Location), "DILE association", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception exception)
			{
				MessageBox.Show("An exception occurred:\n\n" + exception.ToString(), "DILE association error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void dissociateButton_Click(object sender, EventArgs e)
		{
			try
			{
				Registry.ClassesRoot.DeleteSubKeyTree(".dileproj");
				Registry.ClassesRoot.DeleteSubKeyTree("DILE.1");
				
				MessageBox.Show(string.Format("The association between the .dileproj extension and DILE has been successfully deleted.", "DILE association", MessageBoxButtons.OK, MessageBoxIcon.Information));
			}
			catch (Exception exception)
			{
				MessageBox.Show("An exception occurred:\n\n" + exception.ToString(), "DILE association error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 2;
			panel.RowCount = 9;

			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));

			RecentAssembliesUpDown = CreateRecentControls(panel, "Number of recent assemblies shown", NuGenSettings.Instance.MaxRecentAssembliesCount);
			RecentProjectsUpDown = CreateRecentControls(panel, "Number of recent projects shown", NuGenSettings.Instance.MaxRecentProjectsCount);
			RecentAssembliesDirectoryLabel = CreateDefaultDirectoryControl(panel, "Default assembly directory", NuGenSettings.Instance.DefaultAssemblyDirectory);
			RecentProjectsDirectoryLabel = CreateDefaultDirectoryControl(panel, "Default project directory", NuGenSettings.Instance.DefaultProjectDirectory);
			CreateAssociateButton(panel);
			CreateDeleteAssociationButton(panel);
		}

		public override void ReadSettings()
		{
			NuGenSettings.Instance.MaxRecentAssembliesCount = Convert.ToInt32(RecentAssembliesUpDown.Value);
			NuGenSettings.Instance.MaxRecentProjectsCount = Convert.ToInt32(RecentProjectsUpDown.Value);
			NuGenSettings.Instance.DefaultAssemblyDirectory = RecentAssembliesDirectoryLabel.Text;
			NuGenSettings.Instance.DefaultProjectDirectory = RecentProjectsDirectoryLabel.Text;
		}

		public override string ToString()
		{
			return "General";
		}
	}
}