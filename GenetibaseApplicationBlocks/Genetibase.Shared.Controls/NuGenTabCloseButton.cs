/* -----------------------------------------------
 * NuGenTabCloseButton.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.Design;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[Designer(typeof(NuGenTabCloseButtonDesigner))]
	[ToolboxItem(true)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenTabCloseButton : NuGenButtonBase
	{
		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(15, 15);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Renderer
		 */

		private INuGenTabCloseButtonRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTabCloseButtonRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					_renderer = this.ServiceProvider.GetService<INuGenTabCloseButtonRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTabCloseButtonRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			this.Renderer.DrawCloseButton(
				new NuGenPaintParams(this, e.Graphics, this.ClientRectangle, this.ButtonStateTracker.GetControlState())
			);
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
			: this(NuGenServiceManager.TabControlServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabCloseButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateService"/><para/>
		/// <see cref="INuGenTabCloseButtonRenderer"/><para/>
		/// </param>
		public NuGenTabCloseButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.BackColor = Color.Transparent;
		}

		#endregion
	}
}
