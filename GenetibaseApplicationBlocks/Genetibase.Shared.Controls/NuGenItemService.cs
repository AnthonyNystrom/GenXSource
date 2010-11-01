/* -----------------------------------------------
 * NuGenItemService.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

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
	internal static class NuGenItemService
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
		public static NuGenItemPaintParams BuildItemPaintParams(NuGenItemParams itemParams)
		{
			if (itemParams == null)
			{
				throw new ArgumentNullException("itemParams");
			}

			NuGenControlState currentState = NuGenControlState.Normal;

			if ((itemParams.State & DrawItemState.Selected) > 0)
			{
				currentState = NuGenControlState.Hot;
			}

			NuGenItemPaintParams itemPaintParams = new NuGenItemPaintParams(
				itemParams.Sender,
				itemParams.Graphics,
				itemParams.Bounds,
				currentState
			);

			itemPaintParams.ContentAlign = itemParams.RightToLeft == RightToLeft.No
				? ContentAlignment.MiddleLeft
				: ContentAlignment.MiddleRight
				;
			itemPaintParams.Font = itemParams.Font;
			itemPaintParams.ForeColor = itemParams.ForeColor;
			itemPaintParams.Image = itemParams.Image;
			itemPaintParams.Text = itemParams.Text;

			return itemPaintParams;
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
