/* -----------------------------------------------
 * INuGenSmoothSpinRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.SmoothControls.SpinInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothSpinRenderer : NuGenSmoothRenderer, INuGenSpinRenderer
	{
		#region INuGenSpinRenderer Members

		/*
		 * DrawBorder
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBorder(
				paintParams.Graphics,
				NuGenControlPaint.BorderRectangle(paintParams.Bounds),
				paintParams.State
			);
		}

		/*
		 * DrawSpinButton
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawSpinButton(NuGenSpinButtonPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;
			NuGenSpinButtonStyle style = paintParams.Style;

			this.DrawBackground(paintParams);
			this.DrawBorder(g, NuGenControlPaint.BorderRectangle(bounds), state);
			this.DrawSpinArrow(g, state, style);
		}

		#endregion

		#region Methods.Private

		/*
		 * DrawSpinArrow
		 */

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="state"></param>
		/// <param name="style"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		private void DrawSpinArrow(Graphics g, NuGenControlState state, NuGenSpinButtonStyle style)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Pen pen = this.GetBorderPen(state))
			{
				Point p1 = Point.Empty;
				Point p2 = Point.Empty;
				Point p3 = Point.Empty;

				switch (style)
				{
					case NuGenSpinButtonStyle.Down:
					{
						p1 = new Point(4, 3);
						p2 = new Point(8, 7);
						p3 = new Point(12, 3);

						for (int i = 0; i < 3; i++)
						{
							p1.X++;
							p2.Y--;
							p3.X--;

							g.DrawLine(pen, p1, p2);
							g.DrawLine(pen, p2, p3);
						}

						break;
					}
					default:
					{
						p1 = new Point(4, 6);
						p2 = new Point(8, 2);
						p3 = new Point(12, 6);

						for (int i = 0; i < 3; i++)
						{
							p1.X++;
							p2.Y++;
							p3.X--;

							g.DrawLine(pen, p1, p2);
							g.DrawLine(pen, p2, p3);
						}

						break;
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSpinRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		public NuGenSmoothSpinRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
