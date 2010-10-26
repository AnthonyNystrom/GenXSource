/* -----------------------------------------------
 * NuGenSmoothTextBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.TextBoxInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenSmoothTextBox"/>.
	/// </summary>
	public sealed class NuGenSmoothTextBoxRenderer : NuGenSmoothRenderer, INuGenTextBoxRenderer
	{
		/*
		 * DrawBorder
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawBorder(NuGenBorderPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			if (paintParams.DrawBorder)
			{
				this.DrawBorder(
					paintParams.Graphics,
					NuGenControlPaint.BorderRectangle(paintParams.Bounds),
					paintParams.State
				);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTextBoxRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothTextBoxRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
