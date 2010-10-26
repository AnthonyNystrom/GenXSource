/* -----------------------------------------------
 * NuGenListBoxRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.ListBoxInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenListBoxRenderer : NuGenRenderer, INuGenListBoxRenderer
	{
		#region INuGenItemRenderer Members

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawItem(NuGenItemPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;
			ContentAlignment align = paintParams.ContentAlign;
			Image image = paintParams.Image;
			string text = paintParams.Text;

			/* Background */

			NuGenListBoxRenderer.DrawItemBackground(g, bounds, state, paintParams.BackgroundColor);

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

		#endregion

		#region Methods.Private

		private static void DrawItemBackground(Graphics g, Rectangle bounds, NuGenControlState state, Color defaultBackColor)
		{
			Debug.Assert(g != null, "g != null");
			Color backColor;

			if (
				state == NuGenControlState.Normal
				|| state == NuGenControlState.Disabled
				)
			{
				backColor = defaultBackColor;
			}
			else
			{
				backColor = SystemColors.Highlight;
			}

			using (SolidBrush sb = new SolidBrush(backColor))
			{
				g.FillRectangle(sb, bounds);
			}
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenListBoxRenderer"/> class.
		/// </summary>
		public NuGenListBoxRenderer()
		{
		}

		#endregion
	}
}
