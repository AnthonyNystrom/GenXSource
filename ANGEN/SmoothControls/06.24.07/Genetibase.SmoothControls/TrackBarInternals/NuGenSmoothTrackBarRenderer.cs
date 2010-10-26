/* -----------------------------------------------
 * NuGenSmoothTrackBarRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.TrackBarInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothTrackBarRenderer : NuGenSmoothRenderer, INuGenTrackBarRenderer
	{
		#region Declarations.Consts

		private const int _arcSize = 4;

		#endregion

		#region INuGenTrackBarRenderer.DrawTrackButton

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawTrackButton(NuGenTrackButtonPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;
			TickStyle tickStyle = paintParams.Style;

			Rectangle borderRectangle = NuGenControlPaint.BorderRectangle(bounds);

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;

				using (GraphicsPath gp = NuGenSmoothTrackBarRenderer.GetGraphicsPath(borderRectangle, tickStyle))
				{
					/* Background */

					using (
							Brush brush = this.GetBackgroundBrush(
								borderRectangle,
								state == NuGenControlState.Normal || state == NuGenControlState.Focused
									? NuGenControlState.Hot
									: state
							)
						)
					{
						g.FillPath(brush, gp);
					}

					/* Body */

					Rectangle bodyRectangle = Rectangle.Inflate(borderRectangle, 0, -3);

					using (GraphicsPath bgp = NuGenSmoothTrackBarRenderer.GetBodyGraphicsPath(borderRectangle, tickStyle))
					using (Brush brush = this.GetBackgroundBrush(bodyRectangle, state))
					{
						g.FillPath(brush, bgp);
					}

					/* Border */

					using (Pen pen = this.GetBorderPen(state))
					{
						g.DrawPath(pen, gp);
					}
				}
			}
		}

		#endregion

		#region INuGenTrackBarRenderer.DrawTrack

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawTrack(NuGenTrackBarPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;
			TickStyle tickStyle = paintParams.TickStyle;
			INuGenValueTracker valueTracker = paintParams.ValueTracker;

			/* Track */

			Rectangle valueRect = new Rectangle(
				bounds.Left,
				bounds.Top,
				(int)(NuGenSmoothTrackBarRenderer.GetStep(bounds, valueTracker) * (valueTracker.Value - valueTracker.Minimum)),
				bounds.Height
			);

			if (
				valueRect.Width > 0
				&& valueRect.Height > 0
				)
			{
				this.DrawBackground(
					g,
					valueRect,
					state == NuGenControlState.Normal || state == NuGenControlState.Focused ? NuGenControlState.Hot : state
				);
			}

			this.DrawBorder(g, bounds, state);

			/* TickLines */

			if ((tickStyle & TickStyle.BottomRight) > 0)
			{
				this.DrawTickLines(
					g,
					NuGenSmoothTrackBarRenderer.GetTickLinesBounds(bounds, TickStyle.BottomRight),
					state,
					TickStyle.BottomRight,
					valueTracker
				);
			}

			if ((tickStyle & TickStyle.TopLeft) > 0)
			{
				this.DrawTickLines(
					g,
					NuGenSmoothTrackBarRenderer.GetTickLinesBounds(bounds, TickStyle.TopLeft),
					state,
					TickStyle.TopLeft,
					valueTracker
				);
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * GetStep
		 */

		private static float GetStep(Rectangle bounds, INuGenValueTracker valueTracker)
		{
			Debug.Assert(valueTracker != null, "valueTracker != null");

			float interval = valueTracker.Maximum - valueTracker.Minimum;
			return (float)bounds.Width / interval;
		}

		#endregion

		#region Methods.Private.Track

		/*
		 * DrawLargeTickLine
		 */

		private void DrawLargeTickLine(
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			float offset,
			TickStyle tickStyle
			)
		{
			Debug.Assert(g != null, "g != null");

			int y1 = bounds.Top;
			int y2 = bounds.Bottom;

			if (tickStyle == TickStyle.TopLeft)
			{
				y1 = bounds.Top;
				y2 = bounds.Bottom;
			}

			this.DrawTickLine(g, offset, y1, y2, state);
		}

		/*
		 * DrawSmallTickLine
		 */

		private void DrawSmallTickLine(
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			float offset,
			TickStyle tickStyle
			)
		{
			Debug.Assert(g != null, "g != null");

			int y1 = bounds.Top;
			int y2 = bounds.Bottom - 1;

			if (tickStyle == TickStyle.TopLeft)
			{
				y1 = bounds.Top + 1;
				y2 = bounds.Bottom;
			}

			this.DrawTickLine(g, offset, y1, y2, state);
		}

		/*
		 * DrawTickLine
		 */

		private void DrawTickLine(Graphics g, float offset, int y1, int y2, NuGenControlState state)
		{
			Debug.Assert(g != null, "g != null");

			this.DrawLine(
				g,
				new PointF(offset, y1),
				new PointF(offset, y2),
				state
			);
		}

		/*
		 * DrawTickLines
		 */

		private void DrawTickLines(
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			TickStyle tickStyle,
			INuGenValueTracker valueTracker
			)
		{
			Debug.Assert(g != null, "g != null");
			Debug.Assert(valueTracker != null, "valueTracker != null");

			float step = NuGenSmoothTrackBarRenderer.GetStep(bounds, valueTracker) * valueTracker.SmallChange;
			float currentOffset = bounds.Left;

			this.DrawLargeTickLine(g, bounds, state, currentOffset, tickStyle);

			while ((currentOffset += step) < bounds.Right)
			{
				this.DrawSmallTickLine(g, bounds, state, currentOffset, tickStyle);
			}

			this.DrawLargeTickLine(g, bounds, state, bounds.Right, tickStyle);
		}

		/*
		 * GetTickLinesBounds
		 */

		private static Rectangle GetTickLinesBounds(Rectangle trackBounds, TickStyle tickStyle)
		{
			Rectangle rect = Rectangle.Empty;
			int tickLinesHeight = trackBounds.Height * 3;

			if (tickStyle == TickStyle.BottomRight)
			{
				rect = new Rectangle(
					trackBounds.Left,
					trackBounds.Bottom + tickLinesHeight,
					trackBounds.Width,
					trackBounds.Height
				);
			}
			else if (tickStyle == TickStyle.TopLeft)
			{
				rect = new Rectangle(
					trackBounds.Left,
					trackBounds.Top - trackBounds.Height - tickLinesHeight,
					trackBounds.Width,
					trackBounds.Height
				);
			}

			return rect;
		}

		#endregion

		#region Methods.Private.TrackButton

		/*
		 * GetBodyGraphicsPath
		 */

		private static GraphicsPath GetBodyGraphicsPath(Rectangle bounds, TickStyle tickStyle)
		{
			GraphicsPath gp = new GraphicsPath();

			switch (tickStyle)
			{
				case TickStyle.TopLeft:
				{
					float rectHeight = bounds.Bottom - NuGenSmoothTrackBarRenderer.GetAsymmetricSliderHeight(bounds) + 3;
					Point topCenter = NuGenControlPaint.RectTCCorner(bounds);
					topCenter.Offset(0, 3);

					gp.AddLine(bounds.Left, rectHeight, topCenter.X, topCenter.Y);
					gp.AddLine(topCenter.X, topCenter.Y, bounds.Right, rectHeight);
					gp.AddLine(bounds.Right, bounds.Bottom - 4, bounds.Left, bounds.Bottom - 4);

					break;
				}
				case TickStyle.BottomRight:
				{
					float rectHeight = NuGenSmoothTrackBarRenderer.GetAsymmetricSliderHeight(bounds) - 3;
					Point bottomCenter = NuGenControlPaint.RectBCCorner(bounds);
					bottomCenter.Offset(0, -3);

					gp.AddLine(bounds.Left, bounds.Top + 4, bounds.Right, bounds.Top + 4);
					gp.AddLine(bounds.Right, rectHeight, bottomCenter.X, bottomCenter.Y);
					gp.AddLine(bottomCenter.X, bottomCenter.Y, bounds.Left, rectHeight);

					break;
				}
				default:
				{
					gp.AddRectangle(Rectangle.Inflate(bounds, 0, -3));
					break;
				}
			}

			gp.CloseFigure();
			return gp;
		}

		/*
		 * GetGraphicsPath
		 */

		private static GraphicsPath GetGraphicsPath(Rectangle bounds, TickStyle tickStyle)
		{
			switch (tickStyle)
			{
				case TickStyle.TopLeft:
				{
					return NuGenSmoothTrackBarRenderer.GetTopLeftGraphicsPath(bounds);
				}
				case TickStyle.BottomRight:
				{
					return NuGenSmoothTrackBarRenderer.GetBottomRightGraphicsPath(bounds);
				}
				default:
				{
					return NuGenControlPaint.GetRoundRectangleGraphicsPath(bounds, 2);
				}
			}
		}

		/*
		 * GetBottomRightGraphicsPath
		 */

		private static GraphicsPath GetBottomRightGraphicsPath(Rectangle bounds)
		{
			GraphicsPath gp = new GraphicsPath();

			gp.AddArc(bounds.Left, bounds.Top, _arcSize, _arcSize, 180, 90);
			gp.AddArc(bounds.Right - _arcSize, bounds.Top, _arcSize, _arcSize, 270, 90);

			Point bottomCenter = NuGenControlPaint.RectBCCorner(bounds);
			float rectHeight = NuGenSmoothTrackBarRenderer.GetAsymmetricSliderHeight(bounds);

			gp.AddLine(bounds.Right, rectHeight, bottomCenter.X, bottomCenter.Y);
			gp.AddLine(bottomCenter.X, bottomCenter.Y, bounds.Left, rectHeight);

			gp.CloseFigure();
			return gp;
		}

		/*
		 * GetTopLeftGraphicsPath
		 */

		private static GraphicsPath GetTopLeftGraphicsPath(Rectangle bounds)
		{
			GraphicsPath gp = new GraphicsPath();

			Point topCenter = NuGenControlPaint.RectTCCorner(bounds);
			float rectHeight = bounds.Bottom - NuGenSmoothTrackBarRenderer.GetAsymmetricSliderHeight(bounds);

			gp.AddLine(bounds.Left, rectHeight, topCenter.X, topCenter.Y);
			gp.AddLine(topCenter.X, topCenter.Y, bounds.Right, rectHeight);

			gp.AddArc(bounds.Right - _arcSize, bounds.Bottom - _arcSize, _arcSize, _arcSize, 0, 90);
			gp.AddArc(bounds.Left, bounds.Bottom - _arcSize, _arcSize, _arcSize, 90, 90);

			gp.CloseFigure();
			return gp;
		}

		/*
		 * GetAsymmetricSliderHeight
		 */

		private static float GetAsymmetricSliderHeight(Rectangle bounds)
		{
			return bounds.Top + bounds.Height * 0.73f;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTrackBarRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothTrackBarRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
