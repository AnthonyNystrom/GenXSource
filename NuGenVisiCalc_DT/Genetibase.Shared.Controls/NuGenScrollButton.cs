/* -----------------------------------------------
 * NuGenScrollButton.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.ScrollBarInternals;
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
	[ToolboxItem(false)]
	[DefaultEvent("Click")]
	[DefaultProperty("Size")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenScrollButtonDesigner")]
	[NuGenSRDescription("Description_ScrollButton")]
	public class NuGenScrollButton : NuGenButtonBase
	{
		/*
		 * DoubleArrow
		 */

		private bool _doubleArrow;

		/// <summary>
		/// Gets or sets the value indicating whether to draw two scroll arrows with a little offset from
		/// its center position.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ScrollButton_DoubleArrow")]
		public bool DoubleArrow
		{
			get
			{
				return _doubleArrow;
			}
			set
			{
				if (_doubleArrow != value)
				{
					_doubleArrow = value;
					this.OnDoubleArrowChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _doubleArrowChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DoubleArrow"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ScrollButton_DoubleArrowChanged")]
		public event EventHandler DoubleArrowChanged
		{
			add
			{
				this.Events.AddHandler(_doubleArrowChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_doubleArrowChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenScrollButton.DoubleArrowChanged"/> event.
		/// </summary>
		protected virtual void OnDoubleArrowChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_doubleArrowChanged, e);
		}

		/*
		 * Style
		 */

		private NuGenScrollButtonStyle _style;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenScrollButtonStyle.Down)]
		[NuGenSRCategory("Category_Appearance")]
		public NuGenScrollButtonStyle Style
		{
			get
			{
				return _style;
			}
			set
			{
				if (_style != value)
				{
					_style = value;
					this.Invalidate();
				}
			}
		}

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(
			SystemInformation.VerticalScrollBarWidth,
			SystemInformation.HorizontalScrollBarHeight
		);

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

		private INuGenScrollBarRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenScrollBarRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenScrollBarRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenScrollBarRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle bounds = this.ClientRectangle;

			/* Translate e.Graphics to render different scroll-button styles. */

			switch (this.Style)
			{
				case NuGenScrollButtonStyle.Left:
				{
					bounds = new Rectangle(bounds.Top, bounds.Left, bounds.Height, bounds.Width);
					NuGenControlPaint.Make90CWGraphics(g, bounds);
					break;
				}
				case NuGenScrollButtonStyle.Right:
				{
					NuGenControlPaint.Make90CCWGraphics(g, bounds);
					bounds = new Rectangle(bounds.Top, bounds.Left, bounds.Height, bounds.Width);
					break;
				}
				case NuGenScrollButtonStyle.Up:
				{
					NuGenControlPaint.Make180CCWGraphics(g, bounds);
					break;
				}
			}

			if (
				bounds.Width > 0
				&& bounds.Height > 0
				)
			{
				NuGenPaintParams paintParams = new NuGenPaintParams(g);
				paintParams.Bounds = bounds;
				paintParams.State = this.ButtonStateTracker.GetControlState();

				if (this.DoubleArrow)
				{
					this.Renderer.DrawDoubleScrollButton(paintParams);
				}
				else
				{
					this.Renderer.DrawScrollButton(paintParams);
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScrollButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// <see cref="INuGenButtonStateService"/><para/>
		/// <see cref="INuGenScrollBarRenderer"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenScrollButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
