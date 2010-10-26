/* -----------------------------------------------
 * NuGenSmoothDropDownRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.DropDownInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothDropDownRenderer : NuGenSmoothRenderer, INuGenDropDownRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawBorder(paintParams.Graphics, NuGenControlPaint.BorderRectangle(paintParams.Bounds), paintParams.State);
		}
		
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="paintParams"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void DrawDropDownBody(NuGenItemPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawItem(paintParams);
		}

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		public void DrawDropDownButton(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawScrollButton(paintParams);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDropDownRenderer"/> class.
		/// </summary>
		public NuGenSmoothDropDownRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
