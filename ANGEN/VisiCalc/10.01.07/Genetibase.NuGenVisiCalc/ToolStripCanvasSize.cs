/* -----------------------------------------------
 * ToolStripCanvasSize.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Genetibase.Shared.Controls;

namespace Genetibase.NuGenVisiCalc
{
	/// <summary>
	/// Represents <see cref="CanvasSizeControl"/> that <see cref="ToolStrip"/> and the inheritors can host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	internal sealed class ToolStripCanvasSize : NuGenToolStripControlHost
	{
		/*
		 * ApplySettings
		 */

		private static readonly Object _applySettings = new Object();

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

		private void OnApplySettings(EventArgs e)
		{
			Initiator.InvokeEventHandler(_applySettings, e);
		}

		/*
		 * CancelSettings
		 */

		private static readonly Object _cancelSettings = new Object();

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

		private void OnCancelSettings(EventArgs e)
		{
			Initiator.InvokeEventHandler(_cancelSettings, e);
		}

		/*
		 * CanvasSize
		 */

		public Size CanvasSize
		{
			get
			{
				return CanvasSizeCtrl.CanvasSize;
			}
			set
			{
				CanvasSizeCtrl.CanvasSize = value;
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
				return CanvasSizeCtrl.MaintainAspectRatio;
			}
			set
			{
				CanvasSizeCtrl.MaintainAspectRatio = value;
			}
		}

		/*
		 * CanvasSizeCtrl
		 */

		private CanvasSizeControl CanvasSizeCtrl
		{
			get
			{
				return (CanvasSizeControl)Control;
			}
		}

		public void InvokeApplyOperation()
		{
			CanvasSizeCtrl.InvokeApplyOperation();
		}

		public void InvokeCancelOperation()
		{
			CanvasSizeCtrl.InvokeCancelOperation();
		}

		/*
		 * OnSubscribeControlEvents
		 */

		protected override void OnSubscribeControlEvents(Control control)
		{
			base.OnSubscribeControlEvents(control);

			CanvasSizeControl canvasSizeCtrl = (CanvasSizeControl)control;
			canvasSizeCtrl.ApplySettings += _canvasSizeCtrl_ApplySettings;
			canvasSizeCtrl.CancelSettings += _canvasSizeCtrl_CancelSettings;
		}

		/*
		 * OnUnsubscribeControlEvents
		 */

		protected override void OnUnsubscribeControlEvents(Control control)
		{
			base.OnUnsubscribeControlEvents(control);

			CanvasSizeControl canvasSizeCtrl = (CanvasSizeControl)control;
			canvasSizeCtrl.ApplySettings -= _canvasSizeCtrl_ApplySettings;
			canvasSizeCtrl.CancelSettings -= _canvasSizeCtrl_CancelSettings;
		}

		private void _canvasSizeCtrl_ApplySettings(Object sender, EventArgs e)
		{
			OnApplySettings(e);
		}

		private void _canvasSizeCtrl_CancelSettings(Object sender, EventArgs e)
		{
			OnCancelSettings(e);
		}

		private static Control CreateControlInstance()
		{
			CanvasSizeControl canvasSizeCtrl = new CanvasSizeControl();
			canvasSizeCtrl.Size = new Size(170, 100);
			canvasSizeCtrl.MinimumSize = canvasSizeCtrl.Size;

			return canvasSizeCtrl;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolStripCanvasSize"/> class.
		/// </summary>
		public ToolStripCanvasSize()
			: base(CreateControlInstance())
		{
			AutoSize = false;
		}
	}
}
