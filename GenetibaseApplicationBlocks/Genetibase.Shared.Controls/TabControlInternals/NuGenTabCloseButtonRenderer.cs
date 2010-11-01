/* -----------------------------------------------
 * NuGenTabCloseButtonRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenTabCloseButton"/>.
	/// </summary>
	internal sealed class NuGenTabCloseButtonRenderer : INuGenTabCloseButtonRenderer
	{
		#region INuGenTabCloseButtonRenderer Members

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
					imageToDraw = Resources.CloseButton_Hot;
					break;
				}
				case NuGenControlState.Pressed:
				{
					imageToDraw = Resources.CloseButton_Pressed;
					break;
				}
				default:
				{
					imageToDraw = Resources.CloseButton_Normal;
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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabCloseButtonRenderer"/> class.
		/// </summary>
		public NuGenTabCloseButtonRenderer()
		{
		}

		#endregion
	}
}
