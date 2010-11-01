/* -----------------------------------------------
 * NuGenTabCloseButton.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Design;
using Genetibase.Controls.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// </summary>
	[Designer(typeof(NuGenTabCloseButtonDesigner))]
	[ToolboxItem(true)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenTabCloseButton : NuGenButtonBase
	{
		#region Properties.Protected.Overriden

		/*
		 * DefaultSize
		 */

		private Size _defaultSize = Size.Empty;

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				if (_defaultSize == Size.Empty)
				{
					_defaultSize = new Size(15, 15);
				}

				return _defaultSize;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Image imageToDraw = null;

			using (Pen pen = new Pen(this.BackColor))
			{
				g.DrawRectangle(pen, this.ClientRectangle);
			}

			switch (this.ButtonStateTracker.GetControlState(this))
			{
				case NuGenControlState.Hot:
				{
					imageToDraw = Resources.CloseButton_Hot;
					break;
				}
				case NuGenControlState.Pressed:
				{
					imageToDraw = Resources.CloseButton_Pressed;
					break;
				}
				default:
				{
					imageToDraw = Resources.CloseButton_Normal;
					break;
				}
			}

			if (this.Enabled)
			{
				g.DrawImageUnscaled(imageToDraw, this.DisplayRectangle.Location);
			}
			else
			{
				ControlPaint.DrawImageDisabled(g, imageToDraw, 0, 0, SystemColors.Control);
			}
		}

		/*
		 * SetBoundsCore
		 */

		/// <summary>
		/// Performs the work of setting the specified bounds of this control.
		/// </summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left"></see> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top"></see> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width"></see> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height"></see> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified"></see> values.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Width) == BoundsSpecified.Width)
			{
				width = this.DefaultSize.Width;
			}

			if ((specified & BoundsSpecified.Height) == BoundsSpecified.Height)
			{
				height = this.DefaultSize.Height;
			}

			base.SetBoundsCore(x, y, width, height, specified);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabCloseButton"/> class.
		/// </summary>
		public NuGenTabCloseButton()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabCloseButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// </param>
		public NuGenTabCloseButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
