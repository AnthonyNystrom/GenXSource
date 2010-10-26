/* -----------------------------------------------
 * NuGenSmoothToolTipRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Genetibase.SmoothControls.ToolTipInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothToolTipRenderer : NuGenSmoothRenderer, INuGenToolTipRenderer
	{
		#region Declarations.Fields

		private readonly Color[] _shadowColors;
		private readonly int _shadowColorsCount;

		#endregion

		#region INuGenToolTipRenderer Members

		/*
		 * DrawBackground
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public new void DrawBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				this.DrawRoundBackground(g, bounds, state);
			}
		}

		/*
		 * DrawBevel
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawBevel(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Point pt1 = paintParams.Bounds.Location;
			Point pt2 = (Point)paintParams.Bounds.Size;
			NuGenControlState state = paintParams.State;

			using (Pen topBevelPen = this.GetBorderPen(state))
			using (Pen bottomBevelPen = new Pen(this.ColorManager.GetBackgroundGradientBegin(state)))
			{
				g.DrawLine(topBevelPen, pt1, pt2);

				pt1.Offset(0, 1);
				pt2.Offset(0, 1);

				g.DrawLine(bottomBevelPen, pt1, pt2);
			}
		}

		/*
		 * DrawBorder
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				this.DrawRoundBorder(g, NuGenControlPaint.BorderRectangle(bounds), state);
			}
		}

		/*
		 * DrawShadow
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawShadow(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;

				for (int i = 0; i < _shadowColorsCount; i++)
				{
					using (Pen pen = new Pen(_shadowColors[i]))
					{
						NuGenControlPaint.DrawRoundRectangle(g, pen, bounds, _shadowColorsCount - i);
					}

					bounds.Inflate(-1, -1);
				}
			}
		}

		/*
		 * DrawHeaderText
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException"><paramref name="paintParams"/> is <see langword="null"/>.</exception>
		public void DrawHeaderText(NuGenTextPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawText(paintParams);
		}

		/*
		 * DrawText
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException"><paramref name="paintParams"/> is <see langword="null"/>.</exception>
		public new void DrawText(NuGenTextPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

				using (StringFormat sf = new StringFormat())
				{
					sf.Trimming = StringTrimming.EllipsisCharacter;

					base.DrawText(
						paintParams.Graphics,
						paintParams.Bounds,
						paintParams.State,
						paintParams.Text,
						paintParams.Font,
						paintParams.ForeColor,
						sf
					);
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolTipRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothToolTipRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_shadowColors = new Color[] {
				Color.FromArgb(14,Color.Black),
				Color.FromArgb(43,Color.Black),
				Color.FromArgb(84,Color.Black),
				Color.FromArgb(113,Color.Black),
				Color.FromArgb(128,Color.Black)
			};

			_shadowColorsCount = _shadowColors.Length;
		}

		#endregion
	}
}
