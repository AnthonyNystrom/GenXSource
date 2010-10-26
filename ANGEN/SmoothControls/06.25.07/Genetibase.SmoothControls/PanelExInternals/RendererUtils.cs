/* -----------------------------------------------
 * RendererUtils.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.SmoothControls.PanelExInternals
{
	internal static class RendererUtils
	{
		/// <summary>
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenSmoothColorManager"/></para>
		/// </param>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="NuGenServiceNotFoundException"/>
		public static void DrawBackground(INuGenServiceProvider serviceProvider, NuGenPaintParams paintParams)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

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
				INuGenSmoothColorManager colorManager = serviceProvider.GetService<INuGenSmoothColorManager>();

				if (colorManager == null)
				{
					throw new NuGenServiceNotFoundException<INuGenSmoothColorManager>();
				}

				Color beginColor = Color.FromArgb(60, colorManager.GetBorderColor(state));
				Color endColor = Color.FromArgb(10, colorManager.GetBackgroundGradientEnd(state));

				using (Brush brush = new LinearGradientBrush(ellipseBounds, beginColor, endColor, 0.0f))
				{
					g.SetClip(bounds, CombineMode.Replace);
					g.FillEllipse(brush, ellipseBounds);
				}
			}
		}
	}
}
