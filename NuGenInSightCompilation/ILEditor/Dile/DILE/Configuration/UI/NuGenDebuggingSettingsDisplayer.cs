using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class NuGenDebuggingSettingsDisplayer : NuGenBaseSettingsDisplayer
	{
		private CheckBox enableLoadClassCheckBox = null;
		private CheckBox EnableLoadClassCheckBox
		{
			get
			{
				return enableLoadClassCheckBox;
			}
			set
			{
				enableLoadClassCheckBox = value;
			}
		}

		private CheckBox warnUnloadedAssemblyCheckBox = null;
		private CheckBox WarnUnloadedAssemblyCheckBox
		{
			get
			{
				return warnUnloadedAssemblyCheckBox;
			}
			set
			{
				warnUnloadedAssemblyCheckBox = value;
			}
		}

		private CheckBox stopOnExceptionCheckBox = null;
		private CheckBox StopOnExceptionCheckBox
		{
			get
			{
				return stopOnExceptionCheckBox;
			}
			set
			{
				stopOnExceptionCheckBox = value;
			}
		}

		private CheckBox stopOnlyOnUnhandledExceptionCheckBox = null;
		private CheckBox StopOnlyOnUnhandledExceptionCheckBox
		{
			get
			{
				return stopOnlyOnUnhandledExceptionCheckBox;
			}
			set
			{
				stopOnlyOnUnhandledExceptionCheckBox = value;
			}
		}

		private CheckBox stopOnMdaNotificationCheckBox = null;
		private CheckBox StopOnMdaNotificationCheckBox
		{
			get
			{
				return stopOnMdaNotificationCheckBox;
			}
			set
			{
				stopOnMdaNotificationCheckBox = value;
			}
		}

		private CheckBox displayHexaNumbers = null;
		private CheckBox DisplayHexaNumbers
		{
			get
			{
				return displayHexaNumbers;
			}
			set
			{
				displayHexaNumbers = value;
			}
		}

		private CheckBox detachOnQuit = null;
		private CheckBox DetachOnQuit
		{
			get
			{
				return detachOnQuit;
			}
			set
			{
				detachOnQuit = value;
			}
		}

		private Label funcEvalTimeoutLabel = null;
		private Label FuncEvalTimeoutLabel
		{
			get
			{
				return funcEvalTimeoutLabel;
			}
			set
			{
				funcEvalTimeoutLabel = value;
			}
		}

		private NumericUpDown funcEvalTimeoutUpDown = null;
		private NumericUpDown FuncEvalTimeoutUpDown
		{
			get
			{
				return funcEvalTimeoutUpDown;
			}
			set
			{
				funcEvalTimeoutUpDown = value;
			}
		}

		private Label funcEvalAbortTimeoutLabel = null;
		private Label FuncEvalAbortTimeoutLabel
		{
			get
			{
				return funcEvalAbortTimeoutLabel;
			}
			set
			{
				funcEvalAbortTimeoutLabel = value;
			}
		}

		private NumericUpDown funcEvalAbortTimeoutUpDown = null;
		private NumericUpDown FuncEvalAbortTimeoutUpDown
		{
			get
			{
				return funcEvalAbortTimeoutUpDown;
			}
			set
			{
				funcEvalAbortTimeoutUpDown = value;
			}
		}

		private void CreateEnableClassLoadCallbacksControls(TableLayoutPanel panel)
		{
			EnableLoadClassCheckBox = new CheckBox();
			EnableLoadClassCheckBox.Text = "Enable (un)load class callbacks during debugging?";
			EnableLoadClassCheckBox.Dock = DockStyle.Fill;
			EnableLoadClassCheckBox.CheckAlign = ContentAlignment.MiddleRight;
			EnableLoadClassCheckBox.Checked = NuGenSettings.Instance.IsLoadClassEnabled;

			panel.Controls.Add(EnableLoadClassCheckBox);
		}

		private void CreateWarnOfNotLoadedAssembliesControls(TableLayoutPanel panel)
		{
			WarnUnloadedAssemblyCheckBox = new CheckBox();
			WarnUnloadedAssemblyCheckBox.Text = "Warn if the debuggee loads an assembly which is not added to the DILE project?";
			WarnUnloadedAssemblyCheckBox.Dock = DockStyle.Fill;
			WarnUnloadedAssemblyCheckBox.CheckAlign = ContentAlignment.MiddleRight;
			WarnUnloadedAssemblyCheckBox.Checked = NuGenSettings.Instance.WarnUnloadedAssembly;

			panel.Controls.Add(WarnUnloadedAssemblyCheckBox);
		}

		private void CreateStopOnExceptionControls(TableLayoutPanel panel)
		{
			StopOnExceptionCheckBox = new CheckBox();
			StopOnExceptionCheckBox.Text = "Stop the debuggee when exception thrown?";
			StopOnExceptionCheckBox.Dock = DockStyle.Fill;
			StopOnExceptionCheckBox.CheckAlign = ContentAlignment.MiddleRight;
			StopOnExceptionCheckBox.Checked = NuGenSettings.Instance.StopOnException;
			StopOnExceptionCheckBox.CheckedChanged += new EventHandler(StopOnExceptionCheckBox_CheckedChanged);

			panel.Controls.Add(StopOnExceptionCheckBox);
		}

		private void CreateStopOnlyOnUnhandledExceptionControls(TableLayoutPanel panel)
		{
			StopOnlyOnUnhandledExceptionCheckBox = new CheckBox();
			StopOnlyOnUnhandledExceptionCheckBox.Text = "Stop the debuggee only when an unhandled exception thrown?";
			StopOnlyOnUnhandledExceptionCheckBox.Dock = DockStyle.Fill;
			StopOnlyOnUnhandledExceptionCheckBox.CheckAlign = ContentAlignment.MiddleRight;
			StopOnlyOnUnhandledExceptionCheckBox.Checked = NuGenSettings.Instance.StopOnlyOnUnhandledException;
			StopOnlyOnUnhandledExceptionCheckBox.Padding = new Padding(20, 0, 0, 0);

			panel.Controls.Add(StopOnlyOnUnhandledExceptionCheckBox);
		}

		private void CreateStopOnMdaNotificationControls(TableLayoutPanel panel)
		{
			StopOnMdaNotificationCheckBox = new CheckBox();
			StopOnMdaNotificationCheckBox.Text = "Stop the debuggee on MDA (Managed Debug Assistant) notification?";
			StopOnMdaNotificationCheckBox.Dock = DockStyle.Fill;
			StopOnMdaNotificationCheckBox.CheckAlign = ContentAlignment.MiddleRight;
			StopOnMdaNotificationCheckBox.Checked = NuGenSettings.Instance.StopOnMdaNotification;

			panel.Controls.Add(StopOnMdaNotificationCheckBox);
		}

		private void CreateDisplayHexaNumbersControls(TableLayoutPanel panel)
		{
			DisplayHexaNumbers = new CheckBox();
			DisplayHexaNumbers.Text = "Display numbers in hexadecimal format?";
			DisplayHexaNumbers.Dock = DockStyle.Fill;
			DisplayHexaNumbers.CheckAlign = ContentAlignment.MiddleRight;
			DisplayHexaNumbers.Checked = NuGenSettings.Instance.DisplayHexaNumbers;

			panel.Controls.Add(DisplayHexaNumbers);
		}

		private void CreateDetachOnQuitControls(TableLayoutPanel panel)
		{
			DetachOnQuit = new CheckBox();
			DetachOnQuit.Text = "Detach from debuggee when DILE is quit (rather than stopping the debuggee)?";
			DetachOnQuit.Dock = DockStyle.Fill;
			DetachOnQuit.CheckAlign = ContentAlignment.MiddleRight;
			DetachOnQuit.Checked = NuGenSettings.Instance.DetachOnQuit;

			panel.Controls.Add(DetachOnQuit);
		}

		private NumericUpDown CreateNumericUpDownControl(TableLayoutPanel panel, string text, int minimum, int maximum, int value)
		{
			NumericUpDown result = null;

			Label label = new Label();
			label.AutoSize = true;
			label.Dock = DockStyle.Left;
			label.Text = text;
			label.TextAlign = ContentAlignment.MiddleLeft;

			result = new NumericUpDown();
			result.Dock = DockStyle.Right;
			result.Minimum = minimum;
			result.Maximum = maximum;
			result.Value = value;
			result.Width = 40;

			panel.Controls.Add(label);
			panel.Controls.Add(result);

			return result;
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 1;
			panel.RowCount = 10;

			CreateEnableClassLoadCallbacksControls(panel);
			CreateStopOnExceptionControls(panel);
			CreateStopOnlyOnUnhandledExceptionControls(panel);
			CreateStopOnMdaNotificationControls(panel);
			CreateWarnOfNotLoadedAssembliesControls(panel);
			CreateDisplayHexaNumbersControls(panel);
			CreateDetachOnQuitControls(panel);
			FuncEvalTimeoutUpDown = CreateNumericUpDownControl(panel, "Time to wait for a function evaluation before aborting it (in seconds):", 5, 999, NuGenSettings.Instance.FuncEvalTimeout);
			FuncEvalAbortTimeoutUpDown = CreateNumericUpDownControl(panel, "Time to wait for successfully aborting a function evaluation before ignoring it (in seconds):", 5, 999, NuGenSettings.Instance.FuncEvalAbortTimeout);
		}

		public override void ReadSettings()
		{
			NuGenSettings.Instance.IsLoadClassEnabled = EnableLoadClassCheckBox.Checked;
			NuGenSettings.Instance.StopOnException = StopOnExceptionCheckBox.Checked;
			NuGenSettings.Instance.StopOnlyOnUnhandledException = StopOnlyOnUnhandledExceptionCheckBox.Checked;
			NuGenSettings.Instance.StopOnMdaNotification = StopOnMdaNotificationCheckBox.Checked;
			NuGenSettings.Instance.WarnUnloadedAssembly = WarnUnloadedAssemblyCheckBox.Checked;
			NuGenSettings.Instance.DisplayHexaNumbers = DisplayHexaNumbers.Checked;
			NuGenSettings.Instance.DetachOnQuit = DetachOnQuit.Checked;
			NuGenSettings.Instance.FuncEvalTimeout = Convert.ToInt32(FuncEvalTimeoutUpDown.Value);
			NuGenSettings.Instance.FuncEvalAbortTimeout = Convert.ToInt32(FuncEvalAbortTimeoutUpDown.Value);
		}

		private void StopOnExceptionCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			StopOnlyOnUnhandledExceptionCheckBox.Enabled = StopOnExceptionCheckBox.Checked;
		}

		public override string ToString()
		{
			return "Debugging";
		}
	}
}