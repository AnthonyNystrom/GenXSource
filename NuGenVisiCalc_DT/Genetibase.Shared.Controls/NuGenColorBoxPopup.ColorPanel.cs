/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenColorBoxPopup
	{
		private sealed class ColorPanel : NuGenPanel
		{
			#region Properties.Public

			/*
			 * DisplayColor
			 */

			private Color _displayColor;

			/// <summary>
			/// </summary>
			public Color DisplayColor
			{
				get
				{
					return _displayColor;
				}
				set
				{
					if (_displayColor != value)
					{
						_displayColor = value == Color.Transparent ? Color.White : value;
						this.Invalidate();
					}
				}
			}

			#endregion

			#region Properties.Protected.Overridden

			/*
			 * DefaultSize
			 */

			private static readonly Size _defaultSize = new Size(16, 16);

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
			 * ButtonStateTracker
			 */

			private INuGenButtonStateTracker _buttonStateTracker;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			private INuGenButtonStateTracker ButtonStateTracker
			{
				get
				{
					if (_buttonStateTracker == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						INuGenButtonStateService stateService = this.ServiceProvider.GetService<INuGenButtonStateService>();

						if (stateService == null)
						{
							throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
						}

						_buttonStateTracker = stateService.CreateStateTracker();
						Debug.Assert(_buttonStateTracker != null, "_buttonStateTracker != null");
					}

					return _buttonStateTracker;
				}
			}

			#endregion

			#region Methods.Protected.Overridden

			/*
			 * OnEnabledChanged
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
			/// </summary>
			/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
			protected override void OnEnabledChanged(EventArgs e)
			{
				this.ButtonStateTracker.Enabled(this.Enabled);
				base.OnEnabledChanged(e);
			}

			/*
			 * OnGotFocus
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
			/// </summary>
			/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
			protected override void OnGotFocus(EventArgs e)
			{
				this.ButtonStateTracker.GotFocus();
				base.OnGotFocus(e);
			}

			/*
			 * OnLostFocus
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
			/// </summary>
			/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
			protected override void OnLostFocus(EventArgs e)
			{
				this.ButtonStateTracker.LostFocus();
				base.OnLostFocus(e);
			}

			/*
			 * OnMouseDown
			 */

			/// <summary>
			/// Raises the mouse down event.
			/// </summary>
			/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
			protected override void OnMouseDown(MouseEventArgs e)
			{
				this.ButtonStateTracker.MouseDown();
				base.OnMouseDown(e);
			}

			/*
			 * OnMouseEnter
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"></see> event.
			/// </summary>
			/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
			protected override void OnMouseEnter(EventArgs e)
			{
				this.ButtonStateTracker.MouseEnter();
				this.Invalidate();

				base.OnMouseEnter(e);
			}

			/*
			 * OnMouseLeave
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
			/// </summary>
			/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
			protected override void OnMouseLeave(EventArgs e)
			{
				this.ButtonStateTracker.MouseLeave();
				this.Invalidate();

				base.OnMouseLeave(e);
			}

			/*
			 * OnMouseUp
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
			/// </summary>
			/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
			protected override void OnMouseUp(MouseEventArgs e)
			{
				this.ButtonStateTracker.MouseUp();
				base.OnMouseUp(e);
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
				Graphics g = e.Graphics;
				Rectangle bounds = this.ClientRectangle;
				NuGenControlState currentState = this.ButtonStateTracker.GetControlState();

				using (SolidBrush sb = new SolidBrush(_displayColor))
				{
					g.FillRectangle(sb, bounds);
				}

				NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(g);
				paintParams.Bounds = bounds;
				paintParams.DrawBorder = true;
				paintParams.State = currentState;

				this.Renderer.DrawBorder(paintParams);
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="ColorPanel"/> class.
			/// </summary>
			/// <param name="serviceProvider">Requires:<para/>
			/// 	<see cref="INuGenButtonStateService"/><para/>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public ColorPanel(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
			}

			#endregion
		}
	}
}
