/* -----------------------------------------------
 * CanvasSizeControl.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.ApplicationBlocks;
using Genetibase.SmoothControls;
using Genetibase.Shared.ComponentModel;

namespace Genetibase.NuGenVisiCalc
{
	/// <summary>
	/// Provides UI to set canvas size.
	/// </summary>
	[System.ComponentModel.DesignerCategory("UserControl")]
	internal sealed partial class CanvasSizeControl : UserControl
	{
		/*
		 * ApplySettings
		 */

		private static readonly Object _applySettings = new Object();

		/// <summary>
		/// Occurs when the user decides to apply the settings.
		/// </summary>
		public event EventHandler ApplySettings
		{
			add
			{
				Events.AddHandler(_applySettings, value);
			}
			remove
			{
				Events.RemoveHandler(_applySettings, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.NuGenVisiCalc.CanvasSizeControl.ApplySettings"/> event.
		/// </summary>
		/// <param name="e"></param>
		private void OnApplySettings(EventArgs e)
		{
			Initiator.InvokeEventHandler(_applySettings, e);
		}

		/*
		 * CancelSettings
		 */

		private static readonly Object _cancelSettings = new Object();

		/// <summary>
		/// Occurs when the user decided to cancel the settings.
		/// </summary>
		public event EventHandler CancelSettings
		{
			add
			{
				Events.AddHandler(_cancelSettings, value);
			}
			remove
			{
				Events.RemoveHandler(_cancelSettings, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.NuGenVisiCalc.CanvasSizeControl.CancelSettings"/> event.
		/// </summary>
		/// <param name="e"></param>
		private void OnCancelSettings(EventArgs e)
		{
			Initiator.InvokeEventHandler(_cancelSettings, e);
		}

		/*
		 * CanvasSize
		 */

		/// <summary>
		/// Determines the size we rollback to when the user clicks cancel.
		/// </summary>
		private Size _rollbackSize;

		/// <summary>
		/// </summary>
		public Size CanvasSize
		{
			get
			{
				return _sizeTracker.Size;
			}
			set
			{
				_rollbackSize = value; // Preserve rollback size.
				_heightSpin.Value = value.Height;
				_widthSpin.Value = value.Width;
			}
		}

		/*
		 * MaintainAspectRatio
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to maintain the aspect ratio.
		/// </summary>
		public Boolean MaintainAspectRatio
		{
			get
			{
				return _maintainAspectRatioCheckBox.Checked;
			}
			set
			{
				_maintainAspectRatioCheckBox.Checked = value;
			}
		}

		/*
		 * InvokeApplyOperation
		 */

		/// <summary>
		/// As if the user clicked Ok.
		/// </summary>
		public void InvokeApplyOperation()
		{
			CanvasSize = _sizeTracker.Size;
			OnApplySettings(EventArgs.Empty);
		}

		/*
		 * InvokeCancelOperation
		 */

		/// <summary>
		/// As if the user clicked Cancel.
		/// </summary>
		public void InvokeCancelOperation()
		{
			CanvasSize = _rollbackSize;
			OnCancelSettings(EventArgs.Empty);
		}

		#region Properties.Services

		private INuGenEventInitiatorService _initiator;

		private INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, Events);
				}

				return _initiator;
			}
		}

		#endregion

		#region EventHandlers

		private void _okLinkLabel_Click(object sender, EventArgs e)
		{
			InvokeApplyOperation();
		}

		private void _cancelLinkLabel_Click(object sender, EventArgs e)
		{
			InvokeCancelOperation();
		}

		private void _heightSpin_ValueChanged(Object sender, EventArgs e)
		{
			_sizeTracker.Height = _heightSpin.Value;
		}

		private void _maintainAspectRatioCheckBox_CheckedChanged(Object sender, EventArgs e)
		{
			_sizeTracker.MaintainAspectRatio = _maintainAspectRatioCheckBox.Checked;

			if (_maintainAspectRatioCheckBox.Checked)
			{
				_sizeTracker.UpdateRatio();
			}
		}

		private void _sizeTracker_WidthChanged(Object sender, EventArgs e)
		{
			Int32 newWidth = _sizeTracker.Width;
			_widthSpin.Value = Math.Min(_widthSpin.Maximum, Math.Max(_widthSpin.Minimum, newWidth));
		}

		private void _sizeTracker_HeightChanged(Object sender, EventArgs e)
		{
			Int32 newHeight = _sizeTracker.Height;
			_heightSpin.Value = Math.Min(_heightSpin.Maximum, Math.Max(_heightSpin.Minimum, newHeight));
		}

		private void _widthSpin_ValueChanged(Object sender, EventArgs e)
		{
			_sizeTracker.Width = _widthSpin.Value;
		}

		#endregion

		private NuGenRatioSizeTracker _sizeTracker;

		/// <summary>
		/// Initializes a new instance of the <see cref="CanvasSizeControl"/> class.
		/// </summary>
		public CanvasSizeControl()
		{
			InitializeComponent();
			NuGenSmoothColorTable colorTable = new NuGenSmoothColorTable();
			BackColor = colorTable.ToolStripPanelGradientEnd;
			_sizeTracker = new NuGenRatioSizeTracker(new Size(_widthSpin.Value, _heightSpin.Value));
			_sizeTracker.HeightChanged += _sizeTracker_HeightChanged;
			_sizeTracker.WidthChanged += _sizeTracker_WidthChanged;
		}
	}
}
