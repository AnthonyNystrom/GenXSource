/* -----------------------------------------------
 * NuGenTrackBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.TrackBarInternals;
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
	partial class NuGenTrackBar
	{
		/// <summary>
		/// </summary>
		private sealed class TrackButton : NuGenButtonBase
		{
			#region Properties.Public

			/*
		 * Orientation
		 */

			private NuGenOrientationStyle _orientation;

			/// <summary>
			/// </summary>
			public NuGenOrientationStyle Orientation
			{
				get
				{
					return _orientation;
				}
				set
				{
					if (_orientation != value)
					{
						_orientation = value;

						int bufferWidth = this.Width;
						this.Width = this.Height;
						this.Height = bufferWidth;

						this.Invalidate();
					}
				}
			}

			/*
			 * TickStyle
			 */

			private TickStyle _tickStyle;

			/// <summary>
			/// </summary>
			public TickStyle TickStyle
			{
				get
				{
					return _tickStyle;
				}
				set
				{
					if (_tickStyle != value)
					{
						_tickStyle = value;
						this.Invalidate();
					}
				}
			}

			#endregion

			#region Properties.Protected.Overriden

			/*
			 * DefaultSize
			 */

			private static readonly Size _defaultSize = new Size(11, 22);

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

			private INuGenTrackBarRenderer _renderer;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			private INuGenTrackBarRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_renderer = this.ServiceProvider.GetService<INuGenTrackBarRenderer>();

						if (_renderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenTrackBarRenderer>();
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
			/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
			/// </summary>
			/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
			protected override void OnPaint(PaintEventArgs e)
			{
				Graphics g = e.Graphics;

				if (this.Orientation == NuGenOrientationStyle.Vertical)
				{
					NuGenControlPaint.Make90CCWGraphics(g, this.ClientRectangle);
				}

				NuGenTrackButtonPaintParams paintParams = new NuGenTrackButtonPaintParams(g);
				paintParams.Bounds = NuGenControlPaint.OrientationAgnosticRectangle(
					this.ClientRectangle,
					this.Orientation
				);
				paintParams.State = this.ButtonStateTracker.GetControlState();
				paintParams.Style = this.TickStyle;

				this.Renderer.DrawTrackButton(paintParams);
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="TrackButton"/> class.
			/// </summary>
			/// <param name="serviceProvider">Requires:<para/>
			/// <see cref="INuGenButtonStateService"/><para/>
			/// <see cref="INuGenTrackBarRenderer"/>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public TrackButton(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.BackColor = Color.Transparent;
			}

			#endregion
		}
	}
}
