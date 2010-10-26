/* -----------------------------------------------
 * NuGenSmoothPropertyGridRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PropertyGridInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.SmoothControls.PropertyGridInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenSmoothPropertyGrid"/>.
	/// </summary>
	public sealed class NuGenSmoothPropertyGridRenderer : NuGenSmoothRenderer, INuGenPropertyGridRenderer
	{
		#region INuGenPropertyGridRenderer Members

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawDocComment(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			this.DrawBackground(paintParams);
			this.DrawBorder(g, NuGenControlPaint.BorderRectangle(bounds), state);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPropertyGridRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		public NuGenSmoothPropertyGridRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
