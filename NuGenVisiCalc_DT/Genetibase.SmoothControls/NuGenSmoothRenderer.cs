/* -----------------------------------------------
 * NuGenSmoothRenderer.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothRenderer : NuGenRenderer
	{
		#region Properties.Services

		/*
		 * ColorManager
		 */

		private INuGenSmoothColorManager _colorManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenSmoothColorManager ColorManager
		{
			get
			{
				if (_colorManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_colorManager = this.ServiceProvider.GetService<INuGenSmoothColorManager>();

					if (_colorManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenSmoothColorManager>();
					}
				}

				return _colorManager;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Background

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawBackground(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (bounds.Width > 0 && bounds.Height > 0)
			{
				using (Brush brush = this.GetBackgroundBrush(bounds, state))
				{
					g.FillRectangle(brush, bounds);
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBackground(paintParams.Graphics, paintParams.Bounds, paintParams.State);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawEllipseBackground(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Brush brush = this.GetBackgroundBrush(bounds, state))
			{
				g.FillEllipse(brush, bounds);
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawRoundBackground(Graphics g, Rectangle bounds, NuGenControlState state, NuGenRoundRectangleStyle style)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (bounds.Width > 0 && bounds.Height > 0)
			{
				using (Brush brush = this.GetBackgroundBrush(bounds, state))
				using (GraphicsPath gp = NuGenControlPaint.GetRoundRectangleGraphicsPath(bounds, _roundRectangleRadius, style))
				{
					g.FillPath(brush, gp);
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawRoundBackground(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			DrawRoundBackground(g, bounds, state, NuGenRoundRectangleStyle.Default);	
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawRoundBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			DrawRoundBackground(paintParams.Graphics, paintParams.Bounds, paintParams.State);
		}

		#endregion

		#region Methods.Border

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <param name="state"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawBorder(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Pen pen = this.GetBorderPen(state))
			{
				g.DrawRectangle(pen, bounds);
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawEllipseBorder(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Pen pen = this.GetBorderPen(state))
			{
				g.DrawEllipse(pen, bounds);
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawRoundBorder(Graphics g, Rectangle bounds, NuGenControlState state, NuGenRoundRectangleStyle style)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Pen pen = this.GetBorderPen(state))
			{
				NuGenControlPaint.DrawRoundRectangle(g, pen, bounds, _roundRectangleRadius, style);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <param name="state"></param>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawRoundBorder(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			DrawRoundBorder(g, bounds, state, NuGenRoundRectangleStyle.Default);
		}

		#endregion

		#region Methods.Controls

		/// <summary>
		/// Draws only image and text without background (internally called by <see cref="DrawItem"/>
		/// after background is drawn).
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawItemBody(NuGenItemPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Rectangle bounds = paintParams.Bounds;
			ContentAlignment align = paintParams.ContentAlign;
			Graphics g = paintParams.Graphics;
			Image image = paintParams.Image;
			NuGenControlState state = paintParams.State;
			string text = paintParams.Text;

			/* Image */

			Rectangle imageBounds = NuGenItemService.GetImageBounds(bounds, image, align);

			if (image != null)
			{
				this.DrawImage(g, imageBounds, state, image);
			}

			/* Text */

			if (text != null)
			{
				using (StringFormat sf = NuGenControlPaint.ContentAlignmentToStringFormat(align))
				{
					sf.FormatFlags = StringFormatFlags.NoWrap;
					sf.Trimming = StringTrimming.EllipsisCharacter;
					this.DrawText(
						g,
						NuGenItemService.GetTextBounds(bounds, imageBounds, align),
						state,
						text,
						paintParams.Font,
						paintParams.ForeColor,
						sf
					);
				}
			}
		}

		/// <summary>
		/// Draws background, image, and text (utilized for e.g. drawing list box items).
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawItem(NuGenItemPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawItemBackground(
				paintParams.Graphics
				, paintParams.Bounds
				, paintParams.State
				, paintParams.BackgroundColor
			);

			this.DrawItemBody(paintParams);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawScrollButton(NuGenPaintParams paintParams)
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
			this.DrawScrollButtonBody(paintParams);
		}

		/// <summary>
		/// Internally called by <see cref="DrawScrollButton"/> to draw the arrow itself.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawScrollButtonBody(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			using (Pen pen = this.GetBorderPen(state))
			{
				pen.Width = 2f;

				int width = 8;
				int height = 4;

				Rectangle arrowBounds = new Rectangle(
					bounds.Left + bounds.Width / 2 - width / 2,
					bounds.Top + bounds.Height / 2 - height / 2,
					width,
					height
				);

				Point p1 = new Point(arrowBounds.Left, arrowBounds.Top);
				Point p2 = new Point(arrowBounds.Left + arrowBounds.Width / 2, arrowBounds.Bottom);
				Point p3 = new Point(arrowBounds.Right, arrowBounds.Top);

				g.DrawLines(pen, new Point[] { p1, p2, p3 });
			}
		}

		#endregion

		#region Methods.Elements

		/// <summary>
		/// Draws an arc representing a portion of an ellipse specified by a pair of coordinates, a width, and a height.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawArc(
			Graphics g,
			int x,
			int y,
			int width,
			int height,
			int startAngle,
			int sweepAngle,
			NuGenControlState state
		)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Pen pen = this.GetBorderPen(state))
			{
				g.DrawArc(pen, x, y, width, height, startAngle, sweepAngle);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="state"></param>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawLine(Graphics g, Point p1, Point p2, NuGenControlState state)
		{
			this.DrawLine(g, (PointF)p1, (PointF)p2, state);
		}

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="state"></param>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawLine(Graphics g, PointF p1, PointF p2, NuGenControlState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Pen pen = this.GetBorderPen(state))
			{
				g.DrawLine(pen, p1, p2);
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="font"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="sf"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public override void DrawText(
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			string text,
			Font font,
			Color foreColor,
			StringFormat sf
			)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (font == null)
			{
				throw new ArgumentNullException("font");
			}

			if (sf == null)
			{
				throw new ArgumentNullException("sf");
			}

			using (SolidBrush sb = new SolidBrush(state == NuGenControlState.Disabled ? this.ColorManager.GetBorderColor(state) : foreColor))
			{
				g.DrawString(text, font, sb, bounds, sf);
			}
		}

		#endregion

		#region Methods.Shadow

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawShadow(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			/* Bottom */

			using (Pen pen = new Pen(this.ColorManager.GetShadowColorBottomBegin(state)))
			{
				g.DrawLine(pen, bounds.Left + 1, bounds.Bottom - 2, bounds.Right - 1, bounds.Bottom - 2);
			}

			using (Pen pen = new Pen(this.ColorManager.GetShadowColorBottomEnd(state)))
			{
				g.DrawLine(pen, bounds.Left + 2, bounds.Bottom - 1, bounds.Right - 2, bounds.Bottom - 1);
			}

			/* Top */

			using (Pen pen = new Pen(this.ColorManager.GetShadowColorTopBegin(state)))
			{
				g.DrawLine(pen, bounds.Left + 2, bounds.Top + 1, bounds.Right - 2, bounds.Top + 1);
			}

			using (Pen pen = new Pen(this.ColorManager.GetShadowColorTopEnd(state)))
			{
				g.DrawLine(pen, bounds.Left + 1, bounds.Top + 2, bounds.Right - 1, bounds.Top + 2);
			}

			/* Left */

			using (Pen pen = new Pen(this.ColorManager.GetShadowColorLeftBegin(state)))
			{
				g.DrawLine(pen, bounds.Left + 1, bounds.Top + 3, bounds.Left + 1, bounds.Bottom - 3);
			}

			using (Pen pen = new Pen(this.ColorManager.GetShadowColorLeftEnd(state)))
			{
				g.DrawLine(pen, bounds.Left + 2, bounds.Top + 3, bounds.Left + 2, bounds.Bottom - 3);
			}

			/* Right */

			using (Pen pen = new Pen(this.ColorManager.GetShadowColorRightBegin(state)))
			{
				g.DrawLine(pen, bounds.Right - 2, bounds.Top + 3, bounds.Right - 2, bounds.Bottom - 3);
			}

			using (Pen pen = new Pen(this.ColorManager.GetShadowColorRightEnd(state)))
			{
				g.DrawLine(pen, bounds.Right - 1, bounds.Top + 3, bounds.Right - 1, bounds.Bottom - 3);
			}
		}

		#endregion

		#region Methods.Drawing

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Brush GetBackgroundBrush(Rectangle bounds, NuGenControlState state)
		{
			Debug.Assert(this.ColorManager != null, "this.ColorManager != null");
			return new LinearGradientBrush(
				bounds, 
				this.ColorManager.GetBackgroundGradientBegin(state),
				this.ColorManager.GetBackgroundGradientEnd(state),
				LinearGradientMode.Vertical
			);
		}

		/// <summary>
		/// </summary>
		public Pen GetBorderPen(NuGenControlState state)
		{
			Debug.Assert(this.ColorManager != null, "this.ColorManager != null");
			return new Pen(this.ColorManager.GetBorderColor(state));
		}

		#endregion

		#region Methods.Private

		/*
		 * DrawBackground
		 */

		/// <summary>
		/// </summary>
		private void DrawItemBackground(
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			Color defaultBackColor
			)
		{
			Debug.Assert(g != null, "g != null");
			Rectangle borderBounds = new Rectangle(
				bounds.Left,
				bounds.Top,
				bounds.Width - 1,
				bounds.Height
			);

			if (state == NuGenControlState.Normal
				|| state == NuGenControlState.Disabled
				)
			{
				using (SolidBrush sb = new SolidBrush(defaultBackColor))
				using (Pen pen = new Pen(sb))
				{
					g.FillRectangle(sb, bounds);
					g.DrawRectangle(pen, borderBounds);
				}
			}
			else
			{
				this.DrawBackground(g, bounds, state);
				this.DrawBorder(g, borderBounds, state);
			}
		}

		#endregion

		private const int _roundRectangleRadius = 3;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothRenderer"/> class.
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
		public NuGenSmoothRenderer(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
		}
	}
}
