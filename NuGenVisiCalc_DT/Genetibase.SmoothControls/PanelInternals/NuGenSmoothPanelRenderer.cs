/* -----------------------------------------------
 * NuGenSmoothPanelRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.SmoothControls.PanelInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothPanelRenderer : NuGenSmoothRenderer, INuGenPanelRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawBorder(NuGenBorderPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			if (paintParams.DrawBorder)
			{
				this.DrawBorder(
					paintParams.Graphics
					, NuGenControlPaint.BorderRectangle(paintParams.Bounds)
					, paintParams.State
				);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawExtendedBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBackground(paintParams);
			Color borderColor = this.ColorManager.GetBorderColor(paintParams.State);
			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;

			int ellipseWidth = 2 * bounds.Width;
			int ellipseHeight = 2 * bounds.Height;

			int ellipseStart = 0;
			int ellipseStep = 50;

			using (SolidBrush sb = new SolidBrush(Color.FromArgb(20, borderColor)))
			{
				g.FillEllipse(sb, ellipseStart, ellipseStart, ellipseWidth, ellipseHeight);
				g.FillEllipse(sb, ellipseStart + ellipseStep, ellipseStart + ellipseStep, ellipseWidth, ellipseHeight);
				g.FillEllipse(sb, ellipseStart + ellipseStep * 2, ellipseStart + ellipseStep * 2, ellipseWidth, ellipseHeight);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPanelRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenSmoothPanelRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
