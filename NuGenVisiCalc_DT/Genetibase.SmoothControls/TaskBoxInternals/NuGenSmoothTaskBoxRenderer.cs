/* -----------------------------------------------
 * NuGenSmoothTaskBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TaskBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.TaskBoxInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothTaskBoxRenderer : NuGenSmoothRenderer, INuGenTaskBoxRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBorder(paintParams.Graphics, paintParams.Bounds, paintParams.State);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawCollapseButton(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawScrollButton(paintParams);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawHeader(NuGenItemPaintParams paintParams)
		{
			this.DrawItemBody(paintParams);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTaskBoxRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothTaskBoxRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
