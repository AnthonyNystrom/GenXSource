/* -----------------------------------------------
 * NuGenSmoothPanelExRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.SmoothControls.Properties.Resources;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelExInternals;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.PanelExInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothPanelExRenderer : NuGenSmoothRenderer, INuGenPanelExRenderer
	{
		void INuGenPanelExRenderer.DrawBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			base.DrawBackground(paintParams);
			RendererUtils.DrawBackground(this.ServiceProvider, paintParams);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBorder(
				paintParams.Graphics
				, NuGenControlPaint.BorderRectangle(paintParams.Bounds)
				, paintParams.State
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
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
						g.DrawRectangle(pen, bounds);
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
		public Rectangle GetDisplayRectangle(Rectangle clientRectangle, bool drawShadow)
		{
			if (drawShadow)
			{
				return Rectangle.Inflate(clientRectangle, -_shadowStepCount, -_shadowStepCount);
			}

			return clientRectangle;
		}

		private const int _shadowStepCount = 5;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPanelExRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothPanelExRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
