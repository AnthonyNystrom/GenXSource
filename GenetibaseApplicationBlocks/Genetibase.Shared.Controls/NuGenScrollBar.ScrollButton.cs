/* -----------------------------------------------
 * NuGenScrollBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
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
	partial class NuGenScrollBar
	{
		/// <summary>
		/// </summary>
		private sealed class ScrollButton : NuGenButtonBase
		{
			#region Properties.Public

			/*
		 * Style
		 */

			private NuGenScrollButtonStyle _style;

			/// <summary>
			/// </summary>
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

			#endregion

			#region Properties.Protected.Overridden

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

			#endregion

			#region Properties.Services

			/*
		 * Renderer
		 */

			private INuGenScrollBarRenderer _renderer;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			private INuGenScrollBarRenderer Renderer
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

			#endregion

			#region Methods.Protected.Overriden

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
					this.Renderer.DrawScrollButton(new NuGenPaintParams(
						this,
						g,
						bounds,
						this.ButtonStateTracker.GetControlState())
					);
				}
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="ScrollButton"/> class.
			/// </summary>
			/// <param name="serviceProvider">Requires:<para/>
			/// <see cref="INuGenButtonStateService"/><para/>
			/// <see cref="INuGenScrollBarRenderer"/><para/>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public ScrollButton(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
			}

			#endregion
		}
	}
}
