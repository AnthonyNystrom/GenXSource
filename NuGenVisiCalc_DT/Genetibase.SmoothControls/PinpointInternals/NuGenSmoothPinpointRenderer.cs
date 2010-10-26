/* -----------------------------------------------
 * NuGenSmoothPinpointRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PinpointInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Genetibase.Shared.Drawing;

namespace Genetibase.SmoothControls.PinpointInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothPinpointRenderer : NuGenSmoothRenderer, INuGenPinpointRenderer
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="paintParams"/> is <see langword="null"/>.</exception>
		public void DrawFisheyeExpander(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;
			NuGenControlState expanderState = NuGenControlState.Normal;

			if (state == NuGenControlState.Hot)
			{
				using (SolidBrush sb = new SolidBrush(Color.FromArgb(30, this.ColorManager.GetBorderColor(expanderState))))
				{
					g.FillRectangle(sb, bounds);
				}

				this.DrawBorder(g, bounds, expanderState);
			}
			else if (state == NuGenControlState.Normal)
			{
				this.DrawBorder(g, NuGenControlPaint.BorderRectangle(bounds), expanderState);
			}

			NuGenPaintParams arrowPaintParams = new NuGenPaintParams(paintParams);
			arrowPaintParams.State = expanderState;
			arrowPaintParams.Bounds = Rectangle.FromLTRB(
				bounds.Left
				, bounds.Bottom - 20
				, bounds.Right
				, bounds.Bottom
			);

			this.DrawScrollButtonBody(arrowPaintParams);

			arrowPaintParams.Bounds = Rectangle.FromLTRB(
				bounds.Left
				, bounds.Top
				, bounds.Right
				, bounds.Top + 30
			);

			g.RotateTransform(180);
			g.TranslateTransform(-(bounds.Width * 2 + bounds.Left + 3), -(24 + bounds.Y * 2));
			this.DrawScrollButtonBody(arrowPaintParams);
			g.ResetTransform();
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="paintParams"/> is <see langword="null"/>.</exception>
		public void DrawSelectionFrame(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			this.DrawBackground(paintParams);
			this.DrawBorder(g, NuGenControlPaint.BorderRectangle(bounds), state);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPinpointRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothPinpointRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
