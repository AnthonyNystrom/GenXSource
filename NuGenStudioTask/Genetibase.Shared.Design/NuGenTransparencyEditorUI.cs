/* -----------------------------------------------
 * NuGenTransparencyEditorUI.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides UI to set transparency level.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	public class NuGenTransparencyEditorUI : UserControl
	{
		#region Properties.Public

		/// <summary>
		/// Gets the value of trapsparency set by user.
		/// </summary>
		public int GetValue
		{
			get
			{
				return this.TrackBar.Value;
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * Label
		 */

		private Label _transparencyLabel = null;

		/// <summary>
		/// </summary>
		protected Label TransparencyLabel
		{
			get
			{
				if (_transparencyLabel == null)
				{
					_transparencyLabel = new Label();
				}

				return _transparencyLabel;
			}
		}

		/*
		 * TrackBar
		 */

		private TrackBar _trackBar = null;

		/// <summary>
		/// </summary>
		protected TrackBar TrackBar
		{
			get
			{
				if (_trackBar == null)
				{
					_trackBar = new TrackBar();
				}

				return _trackBar;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * InitializeTransparencyLabel
		 */

		/// <summary>
		/// </summary>
		/// <param name="labelToInitialize"></param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="labelToInitialize"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void InitializeTransparencyLabel(Label labelToInitialize)
		{
			if (labelToInitialize == null)
			{
				throw new ArgumentNullException("labelToInitialize");
			}

			labelToInitialize.Dock = DockStyle.Bottom;
			labelToInitialize.Location = new Point(0, 40);
			labelToInitialize.Size = new Size(144, 16);
			labelToInitialize.TabIndex = 1;
			labelToInitialize.Text = "0%";
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
		protected virtual void InitializeTrackBar(TrackBar trackBarToInitialize)
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
			this.TransparencyLabel.Text = string.Format("{0}%", this.TrackBar.Value);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTransparencyEditorUI"/> class.
		/// </summary>
		public NuGenTransparencyEditorUI()
		{
			this.SuspendLayout();

			Debug.Assert(this.TransparencyLabel != null, "this.TransparencyLabel != null");
			Debug.Assert(this.TrackBar != null, "this.TrackBar != null");

			this.InitializeTransparencyLabel(this.TransparencyLabel);
			this.InitializeTrackBar(this.TrackBar);

			this.TransparencyLabel.Parent = this;
			this.TrackBar.Parent = this;

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
			defaultTransparencyLevel = Math.Min(defaultTransparencyLevel, this.TrackBar.Maximum);
			defaultTransparencyLevel = Math.Max(defaultTransparencyLevel, this.TrackBar.Minimum);

			this.TrackBar.Value = defaultTransparencyLevel;
		}

		#endregion
	}
}
