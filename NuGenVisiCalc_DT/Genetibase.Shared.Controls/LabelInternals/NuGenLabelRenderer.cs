/* -----------------------------------------------
 * NuGenLabelRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Drawing;

namespace Genetibase.Shared.Controls.LabelInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenLabel"/>.
	/// </summary>
	public sealed class NuGenLabelRenderer : INuGenLabelRenderer
	{
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

			paintParams.Graphics.DrawImage(paintParams.Image, paintParams.Bounds);
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

			using (SolidBrush sb = new SolidBrush(paintParams.ForeColor))
			using (StringFormat sf = NuGenControlPaint.ContentAlignmentToStringFormat(paintParams.TextAlign))
			{
				paintParams.Graphics.DrawString(paintParams.Text, paintParams.Font, sb, paintParams.Bounds, sf);
			}
		}
	}
}
