using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommandManager
{
	public partial class MainForm : Form
	{
		private void _commandManager_ApplicationCommandUpdate(object sender, NuGenApplicationCommandEventArgs e)
		{
			Control owner = sender as Control;
			NuGenApplicationCommand command = e.ApplicationCommand;
			ToolStripItem item = e.Item as ToolStripItem;

			switch (command.ApplicationCommandName)
			{
				case "New":
				{
					e.ApplicationCommand.Enabled = _enableNewCheckBox.Checked;
					break;
				}
				case "FileCopy":
				{
					e.ApplicationCommand.Enabled = _enableGlobalCopyCheckBox.Checked;
					break;
				}
				case "ContextCopy":
				{
					e.ApplicationCommand.Enabled = _enableLocalCopyCheckBox.Checked;
					break;
				}
				case "Save":
				{
					e.ApplicationCommand.Visible = _showSaveCheckBox.Checked;
					break;
				}
				case "TrackBar":
				{
					_progressBar.Value = _trackBar.Value;
					break;
				}
			}
		}

		public MainForm()
		{
			this.InitializeComponent();
			this.SetStyle(ControlStyles.Opaque, true);
		}
	}
}
