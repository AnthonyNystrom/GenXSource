/* -----------------------------------------------
 * NuGenSmoothRoundedPanelRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.SmoothControls.Properties.Resources;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.RoundedPanelInternals;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.RoundedPanelInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothRoundedPanelRenderer : NuGenSmoothRenderer, INuGenRoundedPanelRenderer
	{
		void INuGenRoundedPanelRenderer.DrawBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			base.DrawRoundBackground(paintParams);

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			RectangleF ellipseBounds = new RectangleF(
				bounds.Left - bounds.Width * 0.2f
				, bounds.Top + bounds.Height * 0.6f
				, bounds.Width * 2
				, bounds.Height
			);

			if (ellipseBounds.Width > 0 && ellipseBounds.Height > 0)
			{
				Color beginColor = Color.FromArgb(60, this.ColorManager.GetBorderColor(state));
				Color endColor = Color.FromArgb(10, this.ColorManager.GetBackgroundGradientEnd(state));

				using (Brush brush = new LinearGradientBrush(ellipseBounds, beginColor, endColor, 0.0f))
				{
					g.SetClip(bounds, CombineMode.Replace);
					g.FillEllipse(brush, ellipseBounds);
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException"><para><paramref name="paintParams"/> is <see langword="null"/>.</para></exception>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				this.DrawRoundBorder(g, NuGenControlPaint.BorderRectangle(paintParams.Bounds), paintParams.State);
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="paintParams"/> is <see langword="null"/>.</para></exception>
		public void DrawShadow(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;

			int alpha = 0;
			Color baseColor = Color.Black;
			int alphaStep = 5;
			Padding deflatePadding = new Padding(1);

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				NuGenControlPaint.SetGraphicsVeryHighQuality(g);

				using (Pen pen = new Pen(Color.FromArgb(alpha, baseColor)))
				{
					for (int i = 0; i <= _shadowStepCount; i++)
					{
						NuGenControlPaint.DrawRoundRectangle(g, pen, bounds, _shadowRadius);
						pen.Color = Color.FromArgb(alpha += alphaStep, baseColor);
						bounds = NuGenControlPaint.DeflateRectangle(bounds, deflatePadding);

						if (i == 1)
						{
							alphaStep = 10;
						}
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <returns></returns>
		public Rectangle GetDisplayRectangle(Rectangle clientRectangle)
		{
			return Rectangle.Inflate(clientRectangle, -_shadowStepCount, -_shadowStepCount);
		}

		private const int _shadowStepCount = 5;
		private const int _shadowRadius = 6;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothRoundedPanelRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothRoundedPanelRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
