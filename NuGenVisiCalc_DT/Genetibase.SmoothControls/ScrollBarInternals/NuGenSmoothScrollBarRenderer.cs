/* -----------------------------------------------
 * NuGenSmoothScrollBarRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.SmoothControls.ScrollBarInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenSmoothScrollBar"/>.
	/// </summary>
	public sealed class NuGenSmoothScrollBarRenderer : NuGenSmoothRenderer, INuGenScrollBarRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			base.DrawBorder(
				paintParams.Graphics
				, NuGenControlPaint.BorderRectangle(paintParams.Bounds)
				, paintParams.State
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawDoubleScrollButton(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBackground(paintParams);
			this.DrawBorder(paintParams);

			Rectangle bounds = paintParams.Bounds;

			paintParams.Bounds = new Rectangle(
				bounds.Left
				, bounds.Top - 3
				, bounds.Width
				, bounds.Height
			);
			this.DrawScrollButtonBody(paintParams);

			paintParams.Bounds = new Rectangle(
				bounds.Left
				, bounds.Top + 3
				, bounds.Width
				, bounds.Height
			);
			this.DrawScrollButtonBody(paintParams);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawScrollTrack(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			Color borderColor = this.ColorManager.GetBorderColor(NuGenControlState.Normal);
			Color bkgndColor;

			switch (state)
			{
				case NuGenControlState.Pressed:
				{
					bkgndColor = Color.FromArgb(130, borderColor);
					break;
				}
				case NuGenControlState.Hot:
				{
					bkgndColor = Color.FromArgb(90, borderColor);
					break;
				}
				default:
				{
					bkgndColor = Color.FromArgb(50, borderColor);
					break;
				}
			}

			using (SolidBrush sb = new SolidBrush(bkgndColor))
			{
				g.FillRectangle(sb, bounds);
			}
			
			Rectangle borderRectangle = NuGenControlPaint.BorderRectangle(bounds);

			this.DrawLine(
				g,
				NuGenControlPaint.RectTLCorner(borderRectangle),
				NuGenControlPaint.RectTRCorner(borderRectangle),
				NuGenControlState.Normal
			);
			this.DrawLine(
				g,
				NuGenControlPaint.RectBLCorner(borderRectangle),
				NuGenControlPaint.RectBRCorner(borderRectangle),
				NuGenControlState.Normal
			);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawSizeBox(NuGenPaintParams paintParams)
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

			if (bounds.Width > 12)
			{
				int oneGripY = bounds.Top + bounds.Height / 2 - _gripLineHeight / 2;
				int twoGripY = bounds.Top + bounds.Height / 2 + _gripLineHeight / 2;

				int oneGripX = bounds.Left + bounds.Width / 2;
				int twoGripX = oneGripX - _gripLineOffset;
				int threeGripX = oneGripX + _gripLineOffset;

				Point p11 = new Point(oneGripX, oneGripY);
				Point p12 = new Point(oneGripX, twoGripY);

				Point p21 = new Point(twoGripX, oneGripY);
				Point p22 = new Point(twoGripX, twoGripY);

				Point p31 = new Point(threeGripX, oneGripY);
				Point p32 = new Point(threeGripX, twoGripY);

				this.DrawLine(g, p11, p12, state);
				this.DrawLine(g, p21, p22, state);
				this.DrawLine(g, p31, p32, state);
			}
		}

		private const int _gripLineOffset = 3;
		private const int _gripLineHeight = 6;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothScrollBarRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothScrollBarRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
