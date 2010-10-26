/* -----------------------------------------------
 * NuGenDesignerRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// </summary>
	public static class NuGenDesignerRenderer
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void DrawAdornments(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Pen pen = new Pen(SystemColors.ControlDarkDark))
			{
				pen.DashStyle = DashStyle.Dash;
				g.DrawRectangle(pen, NuGenControlPaint.BorderRectangle(bounds));
			}
		}
	}
}
