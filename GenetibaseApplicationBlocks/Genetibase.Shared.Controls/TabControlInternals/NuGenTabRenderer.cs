/* -----------------------------------------------
 * NuGenTabRenderer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;
using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Drawing2D;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// </summary>
	internal sealed class NuGenTabRenderer : INuGenTabRenderer
	{
		#region Declarations.Fields

		private static readonly int _penWidth = 1;
		private static readonly Size _leftNotchSize = new Size(3, 3);
		private static readonly Size _rightNotchSize = new Size(1, 1);
		private static readonly Padding _emptyPadding = new Padding(0);
		private static readonly Padding _flatPadding = new Padding(1, 1, 2, 2);
		private static readonly Padding _systemPadding = new Padding(1, 1, 3, 3);

		#endregion

		#region INuGenTabRenderer Members

		/*
		 * DrawTabBody
		 */

		/// <summary>
		/// </summary>
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

			Debug.Assert(paintParams.Graphics != null, "tabPageParams.Graphics != null");

			switch (paintParams.FlatStyle)
			{
				case FlatStyle.Flat:
				case FlatStyle.Popup:
				{
					if (Application.RenderWithVisualStyles)
					{
						this.DrawTabBodyFlatUsingVisualStyles(paintParams.Graphics, paintParams.Bounds);
					}
					else
					{
						this.DrawTabBodyFlat(paintParams.Graphics, paintParams.Bounds);
					}
					break;
				}
				case FlatStyle.Standard:
				case FlatStyle.System:
				{
					if (Application.RenderWithVisualStyles)
					{
						this.DrawTabBodyUsingVisualStyles(paintParams.Graphics, paintParams.Bounds);
					}
					else
					{
						this.DrawTabBodyNotched(paintParams.Graphics, paintParams.Bounds);
					}
					break;
				}
			}
		}

		/*
		 * DrawTabButton
		 */

		/// <summary>
		/// </summary>
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
			Debug.Assert(g != null, "g != null");

			if (Application.RenderWithVisualStyles)
			{
				this.DrawTabButtonBackgroundUsingVisualStyles(g, paintParams.Bounds, paintParams.State);
			}
			else
			{
				this.DrawTabButtonBackground(g, paintParams.Bounds, paintParams.State);
			}

			Rectangle textBounds = paintParams.TextBounds;

			if (textBounds.Width > 10)
			{
				Debug.Assert(paintParams.Font != null, "tabItemParams.Font != null");
				this.DrawTabButtonText(
					g,
					paintParams.Text,
					textBounds,
					paintParams.Font,
					paintParams.ForeColor,
					paintParams.State != TabItemState.Disabled
				);
			}

			Image tabItemImage = paintParams.Image;

			if (tabItemImage != null)
			{
				this.DrawTabButtonImage(
					g,
					tabItemImage,
					paintParams.ImageBounds,
					paintParams.State != TabItemState.Disabled
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

			Debug.Assert(paintParams.Graphics != null, "paintParams.Graphics != null");

			if (Application.RenderWithVisualStyles)
			{
				this.DrawTabPageBackgroundUsingVisualStyles(paintParams.Graphics, paintParams.Bounds);
			}
			else
			{
				this.DrawTabPageBackground(paintParams.Graphics, paintParams.Bounds);
			}
		}

		/*
		 * GetPadding
		 */

		/// <summary>
		/// </summary>
		/// <param name="flatStyle"></param>
		/// <returns></returns>
		public Padding GetPadding(FlatStyle flatStyle)
		{
			switch (flatStyle)
			{
				case FlatStyle.Flat:
				case FlatStyle.Popup:
				{
					return _flatPadding;
				}
				case FlatStyle.Standard:
				case FlatStyle.System:
				{
					return _systemPadding;
				}
			}

			return _emptyPadding;
		}

		#endregion

		#region Properties.Private

		private VisualStyleRenderer _tabBodyVisualStyleRenderer = null;

		/// <summary>
		/// </summary>
		private VisualStyleRenderer TabBodyVisualStyleRenderer
		{
			get
			{
				if (_tabBodyVisualStyleRenderer == null)
				{
					_tabBodyVisualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
				}

				return _tabBodyVisualStyleRenderer;
			}
		}

		#endregion

		#region Methods.Draw.TabBody

		/*
		 * DrawTabBodyFlat
		 */

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		private void DrawTabBodyFlat(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (
				bounds.Height > 0
				&& bounds.Width > 0
				)
			{
				ControlPaint.DrawButton(g, bounds, ButtonState.Flat);
			}
		}

		/*
		 * DrawTabBodyNotched
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		private void DrawTabBodyNotched(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (
				bounds.Height > 0
				&& bounds.Width > 0
				)
			{
				ControlPaint.DrawButton(g, bounds, ButtonState.Normal);
			}
		}

		/*
		 * DrawTabBodyFlatUsingVisualStyles
		 */

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///		Visual styles are not currently available.
		/// </para>
		/// </exception>
		private void DrawTabBodyFlatUsingVisualStyles(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (!VisualStyleInformation.IsEnabledByUser)
			{
				throw new InvalidOperationException(Resources.InvalidOperation_VisualStyles);
			}

			if (
				bounds.Height > 0
				&& bounds.Width > 0
				)
			{
				Debug.Assert(this.TabBodyVisualStyleRenderer != null, "this.TabBodyVisualStyleRenderer != null");
				this.TabBodyVisualStyleRenderer.DrawBackground(g, bounds);
				this.TabBodyVisualStyleRenderer.DrawEdge(
					g,
					bounds,
					Edges.Bottom | Edges.Left | Edges.Right | Edges.Top,
					EdgeStyle.Etched,
					EdgeEffects.None
				);
			}
		}

		/*
		 * DrawTabBodyUsingVisualStyles
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///		Visual styles are not currently available.
		/// </para>
		/// </exception>
		private void DrawTabBodyUsingVisualStyles(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (!VisualStyleInformation.IsEnabledByUser)
			{
				throw new InvalidOperationException(Resources.InvalidOperation_VisualStyles);
			}

			if (
				bounds.Height > 0
				&& bounds.Width > 0
				)
			{
				TabRenderer.DrawTabPage(g, bounds);
			}
		}

		#endregion

		#region Methods.Draw.TabButton

		/*
		 * DrawTabButtonBackground
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		private void DrawTabButtonBackground(Graphics g, Rectangle bounds, TabItemState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			Rectangle tweakedRectangle = new Rectangle(
				bounds.Left,
				bounds.Top,
				bounds.Width - _penWidth,
				bounds.Height - _penWidth
			);

			Pen bottomPen = null;
			Pen leftTopPen = null;
			Pen rightPen = null;
			Pen rightShadowPen = null;

			switch (state)
			{
				case TabItemState.Disabled:
				{
					bottomPen = new Pen(SystemColors.ControlLightLight);
					leftTopPen = new Pen(SystemColors.ControlDark);
					rightPen = new Pen(SystemColors.ControlDark);
					rightShadowPen = new Pen(SystemColors.Control);
					break;
				}
				case TabItemState.Selected:
				{
					bottomPen = new Pen(SystemColors.Control);
					leftTopPen = new Pen(SystemColors.ControlLightLight);
					rightPen = new Pen(SystemColors.ControlDarkDark);
					rightShadowPen = new Pen(SystemColors.ControlDark);
					break;
				}
				default:
				{
					bottomPen = new Pen(SystemColors.ControlLightLight);
					leftTopPen = new Pen(SystemColors.ControlLightLight);
					rightPen = new Pen(SystemColors.ControlDarkDark);
					rightShadowPen = new Pen(SystemColors.ControlDark);
					break;
				}
			}

			try
			{
				g.DrawLines(leftTopPen, this.GetLeftTopNotchedPolygon(tweakedRectangle));
				g.DrawLines(rightPen, this.GetRightNotchedPolygon(tweakedRectangle));
				g.DrawLines(rightShadowPen, this.GetRightShadowNotchedPolygon(tweakedRectangle));
				g.DrawLine(
					bottomPen,
					NuGenControlPaint.RectBLCorner(tweakedRectangle),
					NuGenControlPaint.RectBRCorner(tweakedRectangle)
				);
			}
			finally
			{
				if (bottomPen != null)
				{
					bottomPen.Dispose();
				}

				if (leftTopPen != null)
				{
					leftTopPen.Dispose();
				}

				if (rightPen != null)
				{
					rightPen.Dispose();
				}
			}
		}

		/*
		 * DrawTabButtonBackgroundUsingVisualStyles
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///		Visual styles are not currently available.
		/// </para>
		/// </exception>
		private void DrawTabButtonBackgroundUsingVisualStyles(
			Graphics g,
			Rectangle bounds,
			TabItemState state
			)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (!VisualStyleInformation.IsEnabledByUser)
			{
				throw new InvalidOperationException(Resources.InvalidOperation_VisualStyles);
			}

			TabRenderer.DrawTabItem(g, bounds, state);
		}


		/*
		 * DrawTabButtonImage
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="imageToDraw"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		private void DrawTabButtonImage(Graphics g, Image imageToDraw, Rectangle imageBounds, bool enabled)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (imageToDraw == null)
			{
				throw new ArgumentNullException("imageToDraw");
			}

			if (enabled)
			{
				g.DrawImage(imageToDraw, imageBounds);
			}
			else
			{
				g.DrawImage(
					imageToDraw,
					imageBounds,
					0, 0, imageBounds.Width, imageBounds.Height,
					GraphicsUnit.Pixel,
					NuGenControlPaint.GetGrayscaleImageAttributes()
				);
			}
		}

		/*
		 * DrawTabButtonText
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="text"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="font"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		private void DrawTabButtonText(
			Graphics g,
			string text,
			Rectangle textBounds,
			Font font,
			Color foreColor,
			bool enabled
			)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			if (font == null)
			{
				throw new ArgumentNullException("font");
			}

			using (SolidBrush sb = new SolidBrush(enabled ? foreColor : SystemColors.ControlDark))
			using (StringFormat sf = new StringFormat())
			{
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				sf.LineAlignment = StringAlignment.Center;

				g.DrawString(text, font, sb, textBounds, sf);
			}
		}

		#endregion

		#region Methods.Draw.TabPage

		/*
		 * DrawTabPageBackground
		 */

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		private void DrawTabPageBackground(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (bounds == Rectangle.Empty)
			{
				return;
			}

			if (
				bounds.Height > 0
				&& bounds.Width > 0
				)
			{
				using (SolidBrush sb = new SolidBrush(SystemColors.Control))
				{
					g.FillRectangle(sb, bounds);
				}
			}
		}

		/*
		 * DrawTabPageBackgroundUsingVisualStyles
		 */

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		/// <exception cref="InvalidOperationException">
		///		Visual styles are not currently available.
		/// </exception>
		private void DrawTabPageBackgroundUsingVisualStyles(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (bounds == Rectangle.Empty)
			{
				return;
			}

			if (!VisualStyleInformation.IsEnabledByUser)
			{
				throw new InvalidOperationException(Resources.InvalidOperation_VisualStyles);
			}

			if (
				bounds.Height > 0
				&& bounds.Width > 0
				)
			{
				this.TabBodyVisualStyleRenderer.DrawBackground(g, bounds);
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * GetLeftTopNotchedPolygon
		 */

		/// <summary>
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		private Point[] GetLeftTopNotchedPolygon(Rectangle bounds)
		{
			return new Point[] {
				NuGenControlPaint.RectBLCorner(bounds),
				new Point(bounds.Left, bounds.Top + _leftNotchSize.Height),
				new Point(bounds.Left + _leftNotchSize.Width, bounds.Top),
				new Point(bounds.Right - _rightNotchSize.Width - _penWidth, bounds.Top)
			};
		}

		/*
		 * GetRightNotchedPolygon
		 */

		/// <summary>
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		private Point[] GetRightNotchedPolygon(Rectangle bounds)
		{
			return new Point[] {
				new Point(bounds.Right - _rightNotchSize.Width, bounds.Top),
				new Point(bounds.Right, bounds.Top + _rightNotchSize.Height),
				NuGenControlPaint.RectBRCorner(bounds)
			};
		}

		/*
		 * GetRightShadowNotchedPolygon
		 */

		/// <summary>
		/// </summary>
		/// <param name="bounds"></param>
		/// <returns></returns>
		private Point[] GetRightShadowNotchedPolygon(Rectangle bounds)
		{
			int x = bounds.Right - _rightNotchSize.Width;

			return new Point[] {
				new Point(x, bounds.Top + _rightNotchSize.Height),
				new Point(x, bounds.Bottom)
			};
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabRenderer"/> class.
		/// </summary>
		public NuGenTabRenderer()
		{
		}

		#endregion
	}
}
