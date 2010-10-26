/* -----------------------------------------------
 * NuGenSmoothSplitContainerRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SplitContainerInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.SmoothControls.SplitContainerInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothSplitContainerRenderer : NuGenSmoothRenderer, INuGenSplitContainerRenderer
	{
		#region INuGenSplitContainerRenderer Members

		/*
		 * DrawPanel
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawPanel(NuGenBorderPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			this.DrawBackground(paintParams);

			if (paintParams.DrawBorder)
			{
				this.DrawBorder(g, NuGenControlPaint.BorderRectangle(bounds), state);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSplitContainerRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		public NuGenSmoothSplitContainerRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
