/* -----------------------------------------------
 * NuGenResolutionBlock.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExport;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExport
{
	/// <summary>
	/// Provides UI to specify resolution.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	internal partial class NuGenResolutionBlock : UserControl
	{
		#region Properties.Public

		/// <summary>
		/// Gets the resolution specified on the UI.
		/// </summary>
		public Size Resolution
		{
			get
			{
				return this.SizeTracker.Size;
			}
		}

		#endregion

		#region Properties.Protected

		private NuGenRatioSizeTracker _sizeTracker;

		/// <summary>
		/// Gets the <see cref="NuGenRatioSizeTracker"/> that is used to track resolution changes on the UI.
		/// </summary>
		protected NuGenRatioSizeTracker SizeTracker
		{
			get
			{
				return _sizeTracker;
			}
			set
			{
				_sizeTracker = value;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * SetResolutionFromImage
		 */

		/// <summary>
		/// Sets the initial resolution that is displayed on the UI.
		/// </summary>
		/// <param name="image">Specifies the <see cref="T:Image"/> to set the initial resolution upon.</param>
		public void SetResolutionFromImage(Image image)
		{
			Size size = Size.Empty;

			if (image != null)
			{
				size = image.Size;
			}

			this.SizeTracker = new NuGenRatioSizeTracker(size);
			this.SizeTracker.HeightChanged += sizeTracker_HeightChanged;
			this.SizeTracker.WidthChanged += sizeTracker_WidthChanged;
			this._widthNumeric.Value = size.Width;
			this._heightNumeric.Value = size.Height;

			this.Enabled = size != Size.Empty;
		}

		/*
		 * Verify
		 */

		/// <summary>
		/// Determines whether the data entered by the user is valid.
		/// </summary>
		/// <returns><see langword="true"/> if the data is valid; otherwise, <see langword="false"/>.</returns>
		public bool Verify()
		{
			if (this._widthNumeric.Value < 1)
			{
				MessageBox.Show(
					Properties.Resources.Argument_InvalidWidthResolution,
					Properties.Resources.Message_Alert,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation
				);

				this._widthNumeric.Select();
				return false;
			}

			if (this._heightNumeric.Value < 1)
			{
				MessageBox.Show(
					Properties.Resources.Argument_InvalidHeightResolution,
					Properties.Resources.Message_Alert,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation
				);

				this._heightNumeric.Select();
				return false;
			}

			return true;
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

			if (this.Enabled)
			{
				this.aspectRatioCheckBox.Enabled = true;
				this._widthNumeric.Enabled = true;
				this._heightNumeric.Enabled = true;
			}
			else
			{
				this.aspectRatioCheckBox.Enabled = false;
				this._widthNumeric.Enabled = false;
				this._heightNumeric.Enabled = false;
			}
		}

		#endregion

		#region Methods.Commands

		private void SetHeightCommand()
		{
			Debug.Assert(this.SizeTracker != null);
			this.SizeTracker.Height = (int)this._heightNumeric.Value;
		}

		private void SetWidthCommand()
		{
			Debug.Assert(this.SizeTracker != null);
			this.SizeTracker.Width = (int)this._widthNumeric.Value;
		}

		#endregion

		#region EventHandlers

		private void aspectRatioCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Debug.Assert(this.SizeTracker != null, "this.SizeTracker != null");
			this.SizeTracker.MaintainAspectRatio = this.aspectRatioCheckBox.Checked;
		}

		private void sizeTracker_WidthChanged(object sender, EventArgs e)
		{
			Debug.Assert(this.SizeTracker != null, "this.SizeTracker != null");
			this._widthNumeric.Value = this.SizeTracker.Width;
		}

		private void sizeTracker_HeightChanged(object sender, EventArgs e)
		{
			Debug.Assert(this.SizeTracker != null, "this.SizeTracker != null");
			this._heightNumeric.Value = this.SizeTracker.Height;
		}

		private void widthNumeric_KeyUp(object sender, KeyEventArgs e)
		{
			this.SetWidthCommand();
		}

		private void widthNumeric_ValueChanged(object sender, EventArgs e)
		{
			this.SetWidthCommand();	
		}

		private void heightNumeric_KeyUp(object sender, KeyEventArgs e)
		{
			this.SetHeightCommand();
		}

		private void heightNumeric_ValueChanged(object sender, EventArgs e)
		{
			this.SetHeightCommand();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenResolutionBlock"/> class.
		/// </summary>
		public NuGenResolutionBlock()
		{
			this.InitializeComponent();
			this.Enabled = false;
		}

		#endregion
	}
}
