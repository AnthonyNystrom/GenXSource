/* -----------------------------------------------
 * NuGenSmoothGroupBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.GroupBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.GroupBoxInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenSmoothGroupBox"/>.
	/// </summary>
	public sealed class NuGenSmoothGroupBoxRenderer : NuGenSmoothRenderer, INuGenGroupBoxRenderer
	{
		#region INuGenGroupBoxRenderer Members

		/*
		 * DrawFrame
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawFrame(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				this.DrawRoundBorder(g, bounds, state);
			}
		}

		/*
		 * DrawLabel
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawLabel(NuGenTextPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			this.DrawBackground(paintParams);
			this.DrawBorder(paintParams.Graphics, paintParams.Bounds, paintParams.State);

			this.DrawText(
				g, 
				bounds, 
				state, 
				paintParams.Text, 
				paintParams.Font, 
				paintParams.ForeColor, 
				paintParams.TextAlign);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothGroupBoxRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothGroupBoxRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
