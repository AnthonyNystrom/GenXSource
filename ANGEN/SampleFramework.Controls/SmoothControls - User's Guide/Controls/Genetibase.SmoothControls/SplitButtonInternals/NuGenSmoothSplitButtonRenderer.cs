/* -----------------------------------------------
 * NuGenSmoothSplitButtonRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.SplitButtonInternals;
using Genetibase.Shared.Drawing;
using Genetibase.SmoothControls.ButtonInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.SplitButtonInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothSplitButtonRenderer : NuGenSmoothButtonRenderer, INuGenSplitButtonRenderer
	{
		void INuGenButtonRenderer.DrawBackground(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBackground(
				paintParams.Graphics,
				Rectangle.Inflate(paintParams.Bounds, -2, -2),
				paintParams.State
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawArrow(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawScrollButtonBody(paintParams);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawSplitLine(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Rectangle bounds = paintParams.Bounds;

			this.DrawLine(
				paintParams.Graphics
				, NuGenControlPaint.RectTLCorner(bounds)
				, NuGenControlPaint.RectBLCorner(bounds)
				, paintParams.State
			);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSplitButtonRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothSplitButtonRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
