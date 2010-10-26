/* -----------------------------------------------
 * NuGenTransparencyEditorUI.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides UI to set transparency level.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	internal sealed class NuGenTransparencyEditorUI : UserControl
	{
		#region Properties.Public

		/// <summary>
		/// Gets the value of trapsparency set by user.
		/// </summary>
		public int GetValue
		{
			get
			{
				return _trackBar.Value;
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * InitializeTransparencyLabel
		 */

		/// <summary>
		/// </summary>
		/// <param name="labelToInitialize"></param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="labelToInitialize"/> is <see langword="null"/>.
		/// </exception>
		private void InitializeTransparencyLabel(Label labelToInitialize)
		{
			if (labelToInitialize == null)
			{
				throw new ArgumentNullException("labelToInitialize");
			}

			labelToInitialize.Dock = DockStyle.Bottom;
			labelToInitialize.Location = new Point(0, 40);
			labelToInitialize.Size = new Size(144, 16);
			labelToInitialize.TabIndex = 1;
			labelToInitialize.Text = Properties.Resources.Text_TransparencyEditor_transparencyLabel;
			labelToInitialize.TextAlign = ContentAlignment.TopCenter;
		}

		/*
		 * InitializeTrackBar
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="trackBarToInitialize"/> is <see langword="null"/>.
		/// </exception>
		private void InitializeTrackBar(TrackBar trackBarToInitialize)
		{
			if (trackBarToInitialize == null)
			{
				throw new ArgumentNullException("trackBarToInitialize");
			}

			trackBarToInitialize.Dock = DockStyle.Top;
			trackBarToInitialize.LargeChange = 10;
			trackBarToInitialize.Location = new Point(0, 0);
			trackBarToInitialize.Maximum = 100;
			trackBarToInitialize.Size = new Size(144, 45);
			trackBarToInitialize.TabIndex = 0;
			trackBarToInitialize.TickFrequency = 10;
			trackBarToInitialize.ValueChanged += this.trackBar_ValueChanged;
		}

		#endregion

		#region EventHandlers

		private void trackBar_ValueChanged(object sender, System.EventArgs e)
		{
			_transparencyLabel.Text = string.Format(CultureInfo.InvariantCulture, "{0}%", _trackBar.Value);
		}

		#endregion

		private Label _transparencyLabel = new Label();
		private TrackBar _trackBar = new TrackBar();

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTransparencyEditorUI"/> class.
		/// </summary>
		public NuGenTransparencyEditorUI()
		{
			this.SuspendLayout();

			this.InitializeTransparencyLabel(_transparencyLabel);
			this.InitializeTrackBar(_trackBar);

			_transparencyLabel.Parent = this;
			_trackBar.Parent = this;

			this.Size = new Size(144, 56);
			this.ResumeLayout(false);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTransparencyEditorUI"/> class.
		/// </summary>
		/// <param name="defaultTransparencyLevel">Specifies default transparency level.</param>
		public NuGenTransparencyEditorUI(int defaultTransparencyLevel)
			: this()
		{
			defaultTransparencyLevel = Math.Min(defaultTransparencyLevel, _trackBar.Maximum);
			defaultTransparencyLevel = Math.Max(defaultTransparencyLevel, _trackBar.Minimum);

			_trackBar.Value = defaultTransparencyLevel;
		}
	}
}
