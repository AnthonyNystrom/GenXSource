/* -----------------------------------------------
 * NuGenRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenRenderer
	{
		#region Methods.Elements

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawImage(NuGenImagePaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawImage(
				paintParams.Graphics,
				paintParams.Bounds,
				paintParams.State,
				paintParams.Image
			);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="image"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawImage(Graphics g, Rectangle bounds, NuGenControlState state, Image image)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			switch (state)
			{
				case NuGenControlState.Disabled:
				{
					g.DrawImage(
						image,
						bounds,
						0, 0, image.Width, image.Height,
						GraphicsUnit.Pixel,
						NuGenControlPaint.GetDesaturatedImageAttributes()
					);
					break;
				}
				default:
				{
					g.DrawImage(image, bounds);
					break;
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawText(NuGenTextPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawText(
				paintParams.Graphics,
				paintParams.Bounds,
				paintParams.State,
				paintParams.Text,
				paintParams.Font,
				paintParams.ForeColor,
				paintParams.TextAlign
			);
		}

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
		public void DrawText(
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			string text,
			Font font,
			Color foreColor,
			ContentAlignment textAlignment
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

			using (StringFormat sf = NuGenControlPaint.ContentAlignmentToStringFormat(textAlignment))
			{
				sf.HotkeyPrefix = HotkeyPrefix.Show;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap;

				this.DrawText(g, bounds, state, text, font, foreColor, sf);
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
		public virtual void DrawText(
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

			using (SolidBrush sb = new SolidBrush(state == NuGenControlState.Disabled ? SystemColors.ControlDark : foreColor))
			{
				g.DrawString(text, font, sb, bounds, sf);
			}
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRenderer"/> class.
		/// </summary>
		public NuGenRenderer()
		{
		}

		#endregion
	}
}
