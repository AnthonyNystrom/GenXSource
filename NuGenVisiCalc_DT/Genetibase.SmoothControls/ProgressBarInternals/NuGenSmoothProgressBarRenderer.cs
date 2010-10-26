/* -----------------------------------------------
 * NuGenSmoothProgressBarRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.ProgressBarInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothProgressBarRenderer : NuGenSmoothRenderer, INuGenProgressBarRenderer
	{
		/*
		 * DrawBackground
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException"><paramref name="paintParams"/> is <see langword="null"/>.</exception>
		public new void DrawBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			base.DrawRoundBackground(paintParams);
		}

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

			Graphics g = paintParams.Graphics;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;

				this.DrawRoundBorder(
					g,
					NuGenControlPaint.BorderRectangle(paintParams.Bounds),
					paintParams.State
				);
			}
		}

		/*
		 * DrawForeground
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawForeground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;

			if (bounds.Height > 0 && bounds.Width > 0)
			{
				using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					this.DrawBackground(paintParams.Graphics, bounds, NuGenControlState.Pressed);
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothProgressBarRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenProgressBarLayoutManager"/></para>
		/// <para><see cref="INuGenSmoothColorManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothProgressBarRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
