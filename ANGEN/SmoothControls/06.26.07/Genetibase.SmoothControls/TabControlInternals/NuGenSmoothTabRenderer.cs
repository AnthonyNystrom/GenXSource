/* -----------------------------------------------
 * NuGenSmoothTabRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.SmoothControls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothTabRenderer : NuGenSmoothRenderer, INuGenTabRenderer
	{
		/*
		 * DrawTabBody
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawTabBody(NuGenTabBodyPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;

			this.DrawBackground(g, bounds, NuGenControlState.Normal);
			this.DrawBorder(g, NuGenControlPaint.BorderRectangle(bounds), NuGenControlState.Normal);
		}

		/*
		 * DrawTabButton
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawTabButton(NuGenTabButtonPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState currentState = NuGenControlState.Normal;

			/* Helps to draw background properly if the BackColor property is set to Color.Transparent. */
			g.FillRectangle(SystemBrushes.Control, new Rectangle(0, 0, bounds.Width, 2));

			switch (paintParams.State)
			{
				case TabItemState.Disabled:
				{
					currentState = NuGenControlState.Disabled;
					break;
				}
				case TabItemState.Hot:
				{
					currentState = NuGenControlState.Hot;
					break;
				}
				case TabItemState.Selected:
				{
					currentState = NuGenControlState.Pressed;
					break;
				}
				default:
				{
					currentState = NuGenControlState.Normal;
					break;
				}
			}

			Rectangle tweakedRectangle = NuGenControlPaint.BorderRectangle(bounds);

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				this.DrawHalfRoundBackground(g, tweakedRectangle, currentState);
				this.DrawHalfRoundBorder(g, tweakedRectangle, currentState);
			}

			this.DrawText(
				g, 
				paintParams.TextBounds, 
				currentState, 
				paintParams.Text, 
				paintParams.Font, 
				paintParams.ForeColor, 
				System.Drawing.ContentAlignment.MiddleLeft
			);

			if (paintParams.Image != null)
			{
				this.DrawImage(
					g, 
					paintParams.ImageBounds, 
					currentState, 
					paintParams.Image
				);
			}
		}

		/*
		 * DrawTabPage
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawTabPage(NuGenTabPagePaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBackground(
				paintParams.Graphics,
				paintParams.Bounds,
				NuGenControlState.Normal
			);
		}

		/*
		 * GetPadding
		 */

		private static readonly Padding _padding = new Padding(1, 1, 2, 2);

		/// <summary>
		/// </summary>
		/// <param name="flatStyle"></param>
		/// <returns></returns>
		public Padding GetPadding(FlatStyle flatStyle)
		{
			return _padding;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTabRenderer"/> class.
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
		public NuGenSmoothTabRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
