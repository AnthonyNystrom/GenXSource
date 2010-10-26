/* -----------------------------------------------
 * NuGenSmoothButtonRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.ButtonInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothButtonRenderer : NuGenSmoothRenderer, INuGenButtonRenderer
	{
		void INuGenButtonRenderer.DrawBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBackground(
				paintParams.Graphics,
				Rectangle.Inflate(paintParams.Bounds, -2, -2),
				paintParams.State
			);
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
					NuGenSmoothButtonRenderer.GetBorderRectangle(paintParams.Bounds),
					paintParams.State
				);
			}
		}

		/*
		 * DrawShadow
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		public void DrawShadow(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawShadow(paintParams.Graphics, NuGenSmoothButtonRenderer.GetBorderRectangle(paintParams.Bounds), paintParams.State);
		}

		private static Rectangle GetBorderRectangle(Rectangle bounds)
		{
			return new Rectangle(
				bounds.Left + 1,
				bounds.Top + 1,
				bounds.Width - 3,
				bounds.Height - 3
			);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothButtonRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothButtonRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
