/* -----------------------------------------------
 * NuGenScrollBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenScrollBar
	{
		/// <summary>
		/// </summary>
		private sealed class ScrollTrack : ScrollPart
		{
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
				base.OnPaint(e);

				if (
					this.ClientRectangle.Width > 0
					&& this.ClientRectangle.Height > 0
					)
				{
					this.Renderer.DrawScrollTrack(new NuGenPaintParams(
						this,
						e.Graphics,
						NuGenControlPaint.OrientationAgnosticRectangle(this.ClientRectangle, this.Orientation),
						this.ButtonStateTracker.GetControlState())
					);
				}
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="ScrollTrack"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// Requires:<para/>
			/// <see cref="INuGenButtonStateService"/><para/>
			/// <see cref="INuGenScrollBarRenderer"/><para/>
			/// </param>
			public ScrollTrack(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
			}

			#endregion
		}
	}
}
