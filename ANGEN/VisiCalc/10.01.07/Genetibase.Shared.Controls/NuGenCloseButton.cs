/* -----------------------------------------------
 * NuGenCloseButton.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.CloseButtonInternals;
using Genetibase.Shared.Controls.ComponentModel;
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
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenCloseButton), "Resources.NuGenIcon.png")]
	[DefaultEvent("Click")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenCloseButtonDesigner")]
	[NuGenSRDescription("Description_CloseButton")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenCloseButton : NuGenButtonBase
	{
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

		/*
		 * Renderer
		 */

		private INuGenCloseButtonRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenCloseButtonRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					_renderer = this.ServiceProvider.GetService<INuGenCloseButtonRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenCloseButtonRenderer>();
					}
				}

				return _renderer;
			}
		}

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
			
			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.ButtonStateTracker.GetControlState();
			
			this.Renderer.DrawCloseButton(paintParams);
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCloseButton"/> class.
		/// </summary>
		public NuGenCloseButton()
			: this(NuGenServiceManager.TabControlServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCloseButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateService"/><para/>
		/// <see cref="INuGenCloseButtonRenderer"/><para/>
		/// </param>
		public NuGenCloseButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.BackColor = Color.Transparent;
		}
	}
}
