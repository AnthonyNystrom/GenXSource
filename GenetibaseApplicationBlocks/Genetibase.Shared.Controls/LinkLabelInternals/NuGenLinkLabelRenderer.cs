/* -----------------------------------------------
 * NuGenLinkLabelRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Genetibase.Shared.Controls.LinkLabelInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenLinkLabel"/>.
	/// </summary>
	internal sealed class NuGenLinkLabelRenderer : INuGenLinkLabelRenderer
	{
		#region INuGenLinkLabelRenderer Members

		/*
		 * DrawImage
		 */

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

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			g.DrawImage(paintParams.Image, bounds);
		}

		/*
		 * DrawText
		 */

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

			using (SolidBrush sb = new SolidBrush(paintParams.ForeColor))
			using (StringFormat sf = NuGenControlPaint.ContentAlignmentToStringFormat(paintParams.TextAlign))
			{
				paintParams.Graphics.DrawString(paintParams.Text, paintParams.Font, sb, paintParams.Bounds, sf);
			}
		}

		#endregion
	}
}
