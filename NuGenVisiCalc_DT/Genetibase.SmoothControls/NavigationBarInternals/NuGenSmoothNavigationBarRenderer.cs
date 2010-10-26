/* -----------------------------------------------
 * NuGenSmoothNavigationBarRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.NavigationBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.SmoothControls.NavigationBarInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothNavigationBarRenderer : NuGenSmoothRenderer, INuGenNavigationBarRenderer
	{
		#region INuGenNavigationBarRenderer Members

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

			this.DrawBorder(
				paintParams.Graphics,
				NuGenControlPaint.BorderRectangle(paintParams.Bounds),
				paintParams.State
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawButtonBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Rectangle borderRectangle = paintParams.Bounds;
			borderRectangle.Width--;
			this.DrawBorder(paintParams.Graphics, borderRectangle, paintParams.State);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawDropDownArrow(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			int x = bounds.Left + bounds.Width / 2;
			int y = bounds.Top + bounds.Height / 2 - 3;

			Point[] arrowPoints = new Point[] {
				new Point(x - 3, y),
				new Point(x + 2, y),
				new Point(x, y + 3)
			};

			using (SolidBrush sb = new SolidBrush(this.ColorManager.GetBorderColor(state)))
			{
				PixelOffsetMode oldPixelOffsetMode = g.PixelOffsetMode;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				g.FillPolygon(sb, arrowPoints);
				g.PixelOffsetMode = oldPixelOffsetMode;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawGrip(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBackground(paintParams);

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			Rectangle borderRectangle = bounds;
			borderRectangle.Width--;
			this.DrawBorder(g, borderRectangle, state);

			int handleHeight = 2;
			int handleWidth = 2;
			int handleOffset = 2;
			int shadowOffset = 1;

			Point[] handles = new Point[9];
			handles[4] = new Point(
				borderRectangle.Left + (borderRectangle.Width - handleWidth) / 2,
				borderRectangle.Top + (borderRectangle.Height - handleHeight) / 2
			);

			for (int i = 3; i > -1; i--)
			{
				handles[i] = new Point(
					handles[i + 1].X - handleWidth - handleOffset,
					handles[i + 1].Y
				);
			}

			for (int i = 5; i < 9; i++)
			{
				handles[i] = new Point(
					handles[i - 1].X + handleWidth + handleOffset,
					handles[i - 1].Y
				);
			}

			using (SolidBrush sb = new SolidBrush(this.ColorManager.GetBorderColor(state)))
			{
				for (int i = 0; i < handles.Length; i++)
				{
					g.FillRectangle(sb, new Rectangle(handles[i], new Size(handleWidth, handleHeight)));
				}
			}

			Point[] shadows = handles;

			for (int i = 0; i < shadows.Length; i++)
			{
				shadows[i].X += shadowOffset;
				shadows[i].Y += shadowOffset;
			}

			using (SolidBrush sb = new SolidBrush(this.ColorManager.GetBackgroundGradientBegin(state)))
			{
				for (int i = 0; i < shadows.Length; i++)
				{
					g.FillRectangle(sb, new Rectangle(shadows[i], new Size(handleWidth, handleHeight)));
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawLargeButtonBody(NuGenItemPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Font font = paintParams.Font;

			if (font != null)
			{
				paintParams.Font = new Font(font, FontStyle.Bold);
			}

			this.DrawItem(paintParams);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawNavigationPaneBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = NuGenControlPaint.BorderRectangle(paintParams.Bounds);
			NuGenControlState state = paintParams.State;

			Point topLeftCorner = NuGenControlPaint.RectTLCorner(bounds);
			Point bottomLeftCorner = NuGenControlPaint.RectBLCorner(bounds);
			Point topRightCorner = NuGenControlPaint.RectTRCorner(bounds);
			Point bottomRightCorner = NuGenControlPaint.RectBRCorner(bounds);

			this.DrawLine(g, topLeftCorner, bottomLeftCorner, state);
			this.DrawLine(g, topRightCorner, bottomRightCorner, state);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawSmallButtonBody(NuGenImagePaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			Image image = paintParams.Image;

			if (image != null)
			{
				this.DrawImage(g, NuGenSmoothNavigationBarRenderer.GetSmallImageBounds(bounds), state, image);
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * DrawItem
		 */

		private new void DrawItem(NuGenItemPaintParams paintParams)
		{
			Debug.Assert(paintParams != null, "paintParams != null");

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;
			Image image = paintParams.Image;
			ContentAlignment align = paintParams.ContentAlign;
			string text = paintParams.Text;

			Rectangle imageBounds = Rectangle.Empty;

			if (image != null)
			{
				imageBounds = NuGenSmoothNavigationBarRenderer.GetLargeImageBounds(bounds, image, align);
				this.DrawImage(g, imageBounds, state, image);
			}

			Rectangle textBounds = NuGenSmoothNavigationBarRenderer.GetTextRectangle(bounds, imageBounds, align);

			if (text != null)
			{
				using (StringFormat sf = NuGenControlPaint.ContentAlignmentToStringFormat(align))
				{
					sf.FormatFlags = StringFormatFlags.NoWrap;
					sf.Trimming = StringTrimming.EllipsisCharacter;
					this.DrawText(
						g,
						textBounds,
						state,
						text,
						paintParams.Font,
						paintParams.ForeColor,
						sf
					);
				}
			}
		}

		/*
		 * GetLargeImageBounds
		 */

		private static Rectangle GetLargeImageBounds(Rectangle itemBounds, Image itemImage, ContentAlignment imageAlign)
		{
			if (itemImage != null)
			{
				return new Rectangle(
					((imageAlign & NuGenControlPaint.AnyRight) > 0)
						? itemBounds.Right - (_offset * 2 + _largeImageSize)
						: itemBounds.Left + _offset * 2
						,
					itemBounds.Top + (itemBounds.Height - _largeImageSize) / 2,
					_largeImageSize,
					_largeImageSize
				);
			}

			return Rectangle.Empty;
		}

		/*
		 * GetSmallImageBounds
		 */

		private static Rectangle GetSmallImageBounds(Rectangle itemBounds)
		{
			return new Rectangle(itemBounds.Left + (itemBounds.Width - _smallImageSize) / 2,
				itemBounds.Top + (itemBounds.Height - _smallImageSize) / 2,
				_smallImageSize,
				_smallImageSize
			);
		}

		/*
		 * GetTextBounds
		 */

		private static Rectangle GetTextRectangle(Rectangle itemBounds, Rectangle imageBounds, ContentAlignment textAlign)
		{
			if ((textAlign & NuGenControlPaint.AnyRight) > 0)
			{
				if (!imageBounds.IsEmpty)
				{
					return Rectangle.FromLTRB(
						itemBounds.Left,
						itemBounds.Top,
						imageBounds.Left - _offset,
						itemBounds.Height
					);
				}

				return itemBounds;
			}

			return Rectangle.FromLTRB(
				imageBounds.Right + _offset,
				itemBounds.Top,
				itemBounds.Right,
				itemBounds.Bottom
			);
		}

		#endregion

		private const int _largeImageSize = 24;
		private const int _smallImageSize = 16;
		private const int _offset = 5;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothNavigationBarRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothNavigationBarRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}		
	}
}
