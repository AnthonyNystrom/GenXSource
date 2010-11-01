/* -----------------------------------------------
 * NuGenSpin.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenSpinBase
	{
		/// <summary>
		/// </summary>
		protected sealed class SpinButton : NuGenButtonBase
		{
			#region Properties.Public

			/*
			 * Style
			 */

			private NuGenSpinButtonStyle _style = NuGenSpinButtonStyle.Up;

			/// <summary>
			/// </summary>
			public NuGenSpinButtonStyle Style
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

			private static readonly Size _defaultSize = new Size(16, 10);

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

			private INuGenSpinRenderer _renderer;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			private INuGenSpinRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_renderer = this.ServiceProvider.GetService<INuGenSpinRenderer>();

						if (_renderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenSpinRenderer>();
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
				this.Renderer.DrawSpinButton(
					new NuGenSpinButtonPaintParams(
						this,
						e.Graphics,
						this.ClientRectangle,
						this.ButtonStateTracker.GetControlState(),
						this.Style
					)
				);
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="SpinButton"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// Requires:<para/>
			/// <see cref="INuGenButtonStateTracker"/><para/>
			/// <see cref="INuGenSpinRenderer"/><para/>
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
			/// </exception>
			public SpinButton(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			}

			#endregion
		}
	}
}
