/* -----------------------------------------------
 * NuGenToolTipLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.ToolTipInternals
{
	/// <summary>
	/// </summary>
	public partial class NuGenToolTipLayoutManager : INuGenToolTipLayoutManager
	{
		#region INuGenToolTipLayoutManager.BuildLayoutDescriptor

		private const int _remarksImageOffset = 8;

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="tooltipInfo"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="headerFont"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="textFont"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenToolTipLayoutDescriptor BuildLayoutDescriptor(
			Graphics g,
			NuGenToolTipInfo tooltipInfo,
			Font headerFont,
			Font textFont
			)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (tooltipInfo == null)
			{
				throw new ArgumentNullException("tooltipInfo");
			}

			if (headerFont == null)
			{
				throw new ArgumentNullException("headerFont");
			}

			if (textFont == null)
			{
				throw new ArgumentNullException("textFont");
			}

			NuGenToolTipLayoutDescriptor descriptor = new NuGenToolTipLayoutDescriptor();

			Size tooltipSize = tooltipInfo.IsCustomSize
				? tooltipInfo.CustomSize
				: this.GetAutoSize(g, tooltipInfo, headerFont, textFont)
				;

			Rectangle rect = new Rectangle(new Point(0, 0), tooltipSize);

			Padding headerPadding = _headerPadding;
			Padding remarksHeaderPadding = _remarksHeaderPadding;
			Padding textPadding = NuGenToolTipLayoutManager.GetTextPadding(tooltipInfo);
			Padding imagePadding = _imagePadding;

			/* Header */

			if (tooltipInfo.IsHeaderVisible)
			{
				Rectangle headerRect = new Rectangle(
					rect.X + headerPadding.Left,
					rect.Y + headerPadding.Top,
					rect.Width - headerPadding.Horizontal,
					rect.Height - headerPadding.Vertical
				);

				descriptor.HeaderBounds = headerRect;
				Size headerSize = g.MeasureString(tooltipInfo.Header, headerFont, headerRect.Width).ToSize();
				headerSize.Width += headerPadding.Horizontal;
				headerSize.Height += headerPadding.Vertical;
				rect.Y += headerSize.Height;
				rect.Height -= headerSize.Height;
			}

			/* Remarks */

			if (tooltipInfo.IsRemarksHeaderVisible && rect.Width > 0 && rect.Height > 0)
			{
				if (tooltipInfo.IsRemarksVisible)
				{
					int offset = remarksHeaderPadding.Left;

					if (tooltipInfo.IsRemarksImageVisible)
					{
						offset += tooltipInfo.RemarksImage.Width + imagePadding.Horizontal;
					}

					int remarksTextWidth = rect.Width - offset;
					Size remarksSize = g.MeasureString(tooltipInfo.Remarks, textFont, remarksTextWidth).ToSize();

					Rectangle remarksRect = new Rectangle(
						rect.X + offset,
						rect.Bottom - remarksSize.Height - textPadding.Vertical,
						remarksTextWidth,
						remarksSize.Height
					);

					descriptor.RemarksBounds = remarksRect;
					rect.Height -= (remarksRect.Height + textPadding.Vertical);
				}

				Size remarksHeaderSize = g.MeasureString(tooltipInfo.RemarksHeader, headerFont, rect.Width - remarksHeaderPadding.Horizontal).ToSize();
				Image remarksImage = tooltipInfo.RemarksImage;

				if (tooltipInfo.IsRemarksImageVisible && remarksImage.Height > remarksHeaderSize.Height)
				{
					remarksHeaderSize.Height = remarksImage.Height;
				}

				Rectangle remarksHeaderRect = new Rectangle(
					rect.X + remarksHeaderPadding.Left,
					rect.Bottom - remarksHeaderSize.Height - remarksHeaderPadding.Bottom,
					rect.Width - remarksHeaderPadding.Horizontal,
					remarksHeaderSize.Height
				);

				int bevelTop = remarksHeaderRect.Y - remarksHeaderPadding.Top - 1;
				descriptor.BevelBounds = new Rectangle(headerPadding.Horizontal, bevelTop, tooltipSize.Width - headerPadding.Horizontal, bevelTop);

				if (remarksImage != null)
				{
					descriptor.RemarksImageBounds = new Rectangle(
						remarksHeaderRect.X,
						remarksHeaderRect.Y + (remarksHeaderRect.Height - remarksImage.Height) / 2,
						remarksImage.Width,
						remarksImage.Height
					);

					remarksHeaderRect.X += (remarksImage.Width + _remarksImageOffset);
					remarksHeaderRect.Width -= (remarksImage.Width + _remarksImageOffset);
				}

				descriptor.RemarksHeaderBounds = remarksHeaderRect;

				remarksHeaderSize.Width += remarksHeaderPadding.Horizontal;
				remarksHeaderSize.Height += remarksHeaderPadding.Vertical;

				rect.Height -= remarksHeaderSize.Height;
			}

			/* Image */

			Image image = tooltipInfo.Image;

			if (tooltipInfo.IsImageVisible)
			{
				Rectangle imageRect = new Rectangle(
					rect.X + imagePadding.Left,
					rect.Y + imagePadding.Top,
					image.Width,
					image.Width
				);

				descriptor.ImageBounds = imageRect;

				rect.X += (imagePadding.Horizontal + image.Width);
				rect.Width -= (imagePadding.Horizontal + image.Width);
			}

			/* Text */

			if (tooltipInfo.IsTextVisible && rect.Width > 0 && rect.Height > 0)
			{
				Rectangle textRect = new Rectangle(
					rect.X + textPadding.Left,
					rect.Y + textPadding.Top,
					rect.Width - textPadding.Horizontal,
					rect.Height - textPadding.Vertical
				);

				descriptor.TextBounds = textRect;
			}

			Size shadowSize = this.GetShadowSize();
			descriptor.TooltipSize = new Size(tooltipSize.Width + shadowSize.Width, tooltipSize.Height + shadowSize.Height);

			return descriptor;
		}

		/*
		 * GetAutoSize
		 */

		private Size GetAutoSize(Graphics g, NuGenToolTipInfo tooltipInfo, Font headerFont, Font textFont)
		{
			Debug.Assert(tooltipInfo != null, "tooltipInfo != null");

			Padding headerPadding = _headerPadding;
			Padding remarksPadding = _remarksHeaderPadding;
			Padding textPadding = NuGenToolTipLayoutManager.GetTextPadding(tooltipInfo);
			Padding imagePadding = _imagePadding;

			Size minimumTooltipSize = this.GetMinimumTooltipSize();
			Size size = Size.Empty;

			if (tooltipInfo.IsHeaderVisible)
			{
				Size headerSize = g.MeasureString(tooltipInfo.Header, headerFont).ToSize();

				headerSize.Width += (headerPadding.Horizontal * 2);
				headerSize.Height += (headerPadding.Vertical + headerPadding.Top);

				if (headerSize.Width > size.Width)
				{
					size.Width = headerSize.Width;
				}

				size.Height += headerSize.Height;
			}

			if (tooltipInfo.IsRemarksHeaderVisible)
			{
				Size remarksHeaderSize = g.MeasureString(tooltipInfo.RemarksHeader, headerFont).ToSize();

				remarksHeaderSize.Width += 2;
				remarksHeaderSize.Height += 2;

				Image remarksImage = tooltipInfo.RemarksImage;

				if (tooltipInfo.IsRemarksImageVisible)
				{
					remarksHeaderSize.Width += (remarksImage.Width + _remarksImageOffset);

					if (remarksImage.Height > remarksHeaderSize.Height)
					{
						remarksHeaderSize.Height = remarksImage.Height;
					}
				}

				remarksHeaderSize.Width += remarksPadding.Horizontal;
				remarksHeaderSize.Height += remarksPadding.Vertical;

				if (remarksHeaderSize.Width > size.Width)
				{
					size.Width = remarksHeaderSize.Width;
				}

				size.Height += remarksHeaderSize.Height;

				if (tooltipInfo.IsRemarksVisible)
				{
					int textArea = size.Width;

					if (textArea < minimumTooltipSize.Width)
					{
						textArea = minimumTooltipSize.Width;
					}

					if (remarksImage != null)
					{
						textArea -= remarksImage.Width;
					}

					Size textSize = g.MeasureString(tooltipInfo.Remarks, textFont, textArea).ToSize();

					if (textSize.Height > textSize.Width * 1.75)
					{
						textArea = (int)(textSize.Height * 0.75);
						textSize = g.MeasureString(tooltipInfo.Remarks, textFont, textArea).ToSize();
					}

					textSize.Width += textPadding.Horizontal;
					textSize.Height += textPadding.Vertical;

					if (textSize.Width > size.Width)
					{
						size.Width = textSize.Width;
					}

					size.Height += textSize.Height;
				}
			}

			if (tooltipInfo.IsTextVisible)
			{
				int textArea = size.Width;

				if (textArea < minimumTooltipSize.Width)
				{
					textArea = minimumTooltipSize.Width;
				}

				Size textSize = g.MeasureString(tooltipInfo.Text, textFont, textArea).ToSize();

				textSize.Width += (textPadding.Horizontal + textPadding.Right);
				textSize.Height += (textPadding.Vertical);

				if (tooltipInfo.IsImageVisible)
				{
					Image image = tooltipInfo.Image;
					Debug.Assert(image != null, "image != null");

					textSize.Width += image.Width + imagePadding.Horizontal;

					if (image.Height + imagePadding.Vertical > textSize.Height)
					{
						textSize.Height = image.Height + imagePadding.Vertical;
					}
				}

				if (textSize.Width > size.Width)
				{
					size.Width = textSize.Width;
				}

				size.Height += textSize.Height;
			}
			else
			{
				if (tooltipInfo.IsImageVisible)
				{
					Image image = tooltipInfo.Image;
					Debug.Assert(image != null, "image != null");

					if (image.Width + imagePadding.Horizontal > size.Width)
					{
						size.Width = image.Width + imagePadding.Horizontal;
					}

					size.Height += image.Height + imagePadding.Vertical;
				}
			}

			if (size.Width < minimumTooltipSize.Width)
			{
				size.Width = minimumTooltipSize.Width;
			}

			if (size.Height < minimumTooltipSize.Height)
			{
				size.Height = minimumTooltipSize.Height;
			}

			return size;
		}

		private static Padding GetTextPadding(NuGenToolTipInfo tooltipInfo)
		{
			Debug.Assert(tooltipInfo != null, "tooltipInfo != null");

			if (!tooltipInfo.IsHeaderVisible && !tooltipInfo.IsRemarksHeaderVisible)
			{
				return new Padding(1);
			}

			return new Padding(14, 6, 4, 4);
		}

		#endregion

		#region INuGenToolTipLayoutManager.MinimumTooltipSize

		private Size _minimumTooltipSize;

		/// <summary>
		/// </summary>
		public Size GetMinimumTooltipSize()
		{
			return _minimumTooltipSize;
		}

		/// <summary>
		/// </summary>
		public void SetMinimumTooltipSize(Size value)
		{
			_minimumTooltipSize = value;
		}

		#endregion

		#region INuGenToolTipLayoutManager.GetShadowSize

		private static readonly Size _shadowSize = new Size(4, 4);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Size GetShadowSize()
		{
			return _shadowSize;
		}

		#endregion

		#region INuGenToolTipLayoutManager.GetToolTipLocation

		private const int _tooltipOffset = 3;

		/// <summary>
		/// </summary>
		/// <param name="targetControl">If <see langword="null"/> tooltip location is calculated according to <paramref name="cursorPosition"/> and <paramref name="cursorSize"/> only.</param>
		/// <param name="cursorPosition">Screen coordinates expected.</param>
		/// <param name="cursorSize"></param>
		/// <param name="placeBelowControl"></param>
		/// <returns></returns>
		public Point GetToolTipLocation(Control targetControl, Point cursorPosition, Size cursorSize, bool placeBelowControl)
		{
			Point tooltipLocation;

			if (targetControl != null && placeBelowControl)
			{
				Rectangle ctrlScreenBounds = targetControl.RectangleToScreen(targetControl.ClientRectangle);
				tooltipLocation = new Point(cursorPosition.X, ctrlScreenBounds.Bottom + _tooltipOffset);
			}
			else
			{
				tooltipLocation = cursorPosition;
				tooltipLocation.Offset(cursorSize.Width / 2, cursorSize.Height / 2);
			}

			return tooltipLocation;
		}

		#endregion

		private static readonly Padding _headerPadding = new Padding(6, 6, 2, 2);
		private static readonly Padding _imagePadding = new Padding(6);
		private static readonly Padding _remarksHeaderPadding = new Padding(6, 6, 4, 6);

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolTipLayoutManager"/> class.
		/// </summary>
		public NuGenToolTipLayoutManager()
		{
		}
	}
}
