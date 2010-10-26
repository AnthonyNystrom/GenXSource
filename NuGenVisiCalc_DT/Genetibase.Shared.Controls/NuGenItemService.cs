/* -----------------------------------------------
 * NuGenItemService.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides helper methods for combo box and list box items processing.
	/// </summary>
	public static class NuGenItemService
	{
		/*
		 * BuildItemPaintParams
		 */

		/// <summary>
		/// </summary>
		/// <param name="itemParams"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="itemParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		internal static NuGenItemPaintParams BuildItemPaintParams(NuGenItemParams itemParams)
		{
			if (itemParams == null)
			{
				throw new ArgumentNullException("itemParams");
			}

			NuGenItemPaintParams itemPaintParams = new NuGenItemPaintParams(itemParams.Graphics);
			itemPaintParams.BackgroundColor = itemParams.BackgroundColor;
			itemPaintParams.Bounds = itemParams.Bounds;
			itemPaintParams.ContentAlign = itemParams.RightToLeft == RightToLeft.No
				? ContentAlignment.MiddleLeft
				: ContentAlignment.MiddleRight
				;
			itemPaintParams.Font = itemParams.Font;
			itemPaintParams.ForeColor = itemParams.ForeColor;
			itemPaintParams.Image = itemParams.Image;
			itemPaintParams.Text = itemParams.Text;
			itemPaintParams.State = NuGenDrawItemStateTranslator.ToControlState(itemParams.State);

			return itemPaintParams;
		}

		/*
		 * GetImageBounds
		 */

		/// <summary>
		/// </summary>
		public static Rectangle GetImageBounds(Rectangle itemBounds, Image itemImage, ContentAlignment imageAlign)
		{
			if (itemImage != null)
			{
				if ((imageAlign & NuGenControlPaint.AnyRight) > 0)
				{
					return new Rectangle(
						itemBounds.Right - 3 - itemImage.Width,
						itemBounds.Top + (itemBounds.Height - itemImage.Height) / 2,
						itemImage.Width,
						itemImage.Height
					);
				}

				return new Rectangle(
					itemBounds.Left + 3,
					itemBounds.Top + (itemBounds.Height - itemImage.Height) / 2,
					itemImage.Width,
					itemImage.Height
				);
			}

			return Rectangle.Empty;
		}

		/*
		 * GetTextRectangle
		 */

		/// <summary>
		/// </summary>
		public static Rectangle GetTextBounds(Rectangle itemBounds, Rectangle imageBounds, ContentAlignment textAlign)
		{
			if ((textAlign & NuGenControlPaint.AnyRight) > 0)
			{
				if (!imageBounds.IsEmpty)
				{
					return new Rectangle(
						itemBounds.Left + 1,
						itemBounds.Top,
						itemBounds.Width - (itemBounds.Right - imageBounds.Left) - 1,
						itemBounds.Height
					);
				}

				return itemBounds;
			}

			return new Rectangle(
				imageBounds.Right + 1,
				itemBounds.Top,
				itemBounds.Width - imageBounds.Right - 1,
				itemBounds.Height
			);
		}

		/*
		 * GetItemSize
		 */

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="imageList">Can be <see langword="null"/>.</param>
		/// <param name="text"></param>
		/// <param name="itemTextFont"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="text"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="itemTextFont"/> is <see langword="null"/>.</para>
		/// </exception>
		public static Size GetItemSize(Graphics g, ImageList imageList, string text, Font itemTextFont)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			if (itemTextFont == null)
			{
				throw new ArgumentNullException("itemTextFont");
			}

			Size itemTextSize = g.MeasureString(text, itemTextFont).ToSize();

			int itemWidth = itemTextSize.Width + SystemInformation.VerticalScrollBarWidth + 10;
			int itemHeight = itemTextSize.Height;

			if (imageList != null)
			{
				itemWidth += imageList.ImageSize.Width;
				itemHeight = Math.Max(imageList.ImageSize.Height, itemHeight);
			}

			return new Size(itemWidth, itemHeight);
		}
	}
}
