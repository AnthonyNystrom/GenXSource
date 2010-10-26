/* -----------------------------------------------
 * NuGenCloseButtonRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CloseButtonInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenCloseButton"/>.
	/// </summary>
	internal sealed class NuGenCloseButtonRenderer : INuGenCloseButtonRenderer
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawCloseButton(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;
			Image imageToDraw = null;

			switch (state)
			{
				case NuGenControlState.Hot:
				{
					imageToDraw = Resources.Image_CloseButton_Hot;
					break;
				}
				case NuGenControlState.Pressed:
				{
					imageToDraw = Resources.Image_CloseButton_Pressed;
					break;
				}
				default:
				{
					imageToDraw = Resources.Image_CloseButton_Normal;
					break;
				}
			}

			if (state == NuGenControlState.Disabled)
			{
				ControlPaint.DrawImageDisabled(g, imageToDraw, bounds.X, bounds.Y, SystemColors.Control);
			}
			else
			{
				g.DrawImageUnscaled(imageToDraw, bounds.X, bounds.Y);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCloseButtonRenderer"/> class.
		/// </summary>
		public NuGenCloseButtonRenderer()
		{
		}
	}
}
