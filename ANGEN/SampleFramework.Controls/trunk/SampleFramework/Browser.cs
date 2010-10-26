/* -----------------------------------------------
 * Browser.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using config = SampleFramework.Properties.Settings;
using res = SampleFramework.Properties.Resources;

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.Shared.Controls;
using System.IO;

namespace SampleFramework
{
	internal sealed partial class Browser : Form
	{
		#region Properties.Services

		private INuGenServiceProvider _serviceProvider;

		private INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		private ISamplesManager _samplesManager;

		private ISamplesManager SamplesManager
		{
			get
			{
				if (_samplesManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_samplesManager = this.ServiceProvider.GetService<ISamplesManager>();

					if (_samplesManager == null)
					{
						throw new NuGenServiceNotFoundException<ISamplesManager>();
					}
				}

				return _samplesManager;
			}
		}

		private INuGenWindowStateTracker _wndStateTracker;

		private INuGenWindowStateTracker WndStateTracker
		{
			get
			{
				if (_wndStateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_wndStateTracker = this.ServiceProvider.GetService<INuGenWindowStateTracker>();

					if (_wndStateTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenWindowStateTracker>();
					}
				}

				return _wndStateTracker;
			}
		}

		#endregion

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			config.Default.Location = this.WndStateTracker.GetLocation(this);
			config.Default.Size = this.WndStateTracker.GetSize(this);
			config.Default.WindowState = this.WndStateTracker.GetWindowState(this);
			config.Default.Save();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.Location = config.Default.Location;
			this.Size = config.Default.Size;
			this.Text = res.Text_Browser;
			this.WindowState = config.Default.WindowState;

			this.SamplesManager.PopulateSampleTree(
				this.ServiceProvider
				, _samplesTreeView
				, 0, 1, 2
			);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.WndStateTracker.SetWindowState(this);
		}

		#region EventHandlers.Toolbar

		private void _runButton_Click(object sender, EventArgs e)
		{
			Debug.Assert(_exePath != null, "_exePath != null");
			Process.Start(_exePath);
		}

		private void _csSampleButton_Click(object sender, EventArgs e)
		{
			Debug.Assert(_csProjectPath != null, "_csProjectPath != null");
			Process.Start(_csProjectPath);
		}

		private void _vbSampleButton_Click(object sender, EventArgs e)
		{
			Debug.Assert(_vbProjectPath != null, "_vbProjectPath != null");
			Process.Start(_vbProjectPath);
		}

		private void _browseButton_Click(object sender, EventArgs e)
		{
			Debug.Assert(_samplePath != null, "_samplePath != null");
			Process.Start(_samplePath);
		}

		#endregion

		private void _samplesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.SuspendLayout();

			NuGenTreeNode treeNode = e.Node as NuGenTreeNode;

			if (treeNode != null)
			{
				SampleDescriptor sampleDescriptor = treeNode.Tag as SampleDescriptor;

				if (sampleDescriptor != null)
				{
					_runButton.Enabled = sampleDescriptor.ExeIsAvailable;
					_csSampleButton.Enabled = sampleDescriptor.CsProjectIsAvailable;
					_vbSampleButton.Enabled = sampleDescriptor.VbProjectIsAvailable;
					_browseButton.Enabled = sampleDescriptor.SamplePath != null;

					_exePath = sampleDescriptor.ExePath;
					_csProjectPath = sampleDescriptor.CsProjectPath;
					_vbProjectPath = sampleDescriptor.VbProjectPath;
					_samplePath = sampleDescriptor.SamplePath;

					if (sampleDescriptor.DescriptionIsAvailable)
					{
						using (StreamReader sr = new StreamReader(sampleDescriptor.DescriptionPath))
						{
							try
							{
								_descriptionTextBox.Text = sr.ReadToEnd();
							}
							catch (IOException)
							{
								_descriptionTextBox.Text = "";
							}
							catch (OutOfMemoryException)
							{
								_descriptionTextBox.Text = "";
							}
						}
					}

					if (sampleDescriptor.ImageIsAvailable)
					{
						if (_imagePictureBox.Image != null)
						{
							_imagePictureBox.Image.Dispose();
							_imagePictureBox.Image = null;
						}

						_imagePictureBox.Image = Image.FromFile(sampleDescriptor.ImagePath);
					}
				}
			}

			this.ResumeLayout();
		}

		private string _exePath;
		private string _csProjectPath;
		private string _vbProjectPath;
		private string _samplePath;

		/// <summary>
		/// Initializes a new instance of the <see cref="Browser"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenWindowStateTracker"/></para>
		/// <para><see cref="ISampleFolder"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public Browser(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.InitializeComponent();

			_runButton.Text = res.Text_Browser_runButton;
			_runButton.ToolTipText = res.Tooltip_Browser_runButton;

			_csSampleButton.Text = res.Text_Browser_csSampleButton;
			_csSampleButton.ToolTipText = res.Tooltip_Browser_csSampleButton;

			_vbSampleButton.Text = res.Text_Browser_vbSampleButton;
			_vbSampleButton.ToolTipText = res.Tooltip_Browser_vbSampleButton;

			_browseButton.Text = res.Text_Browser_browseButton;
			_browseButton.ToolTipText = res.Tooltip_Browser_browseButton;

			_docButton.Text = res.Text_Browser_docButton;
			_docButton.ToolTipText = res.Tooltip_Browser_docButton;
		}
	}
}
