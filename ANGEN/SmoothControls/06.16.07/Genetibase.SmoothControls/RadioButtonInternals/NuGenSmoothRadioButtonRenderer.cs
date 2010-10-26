/* -----------------------------------------------
 * NuGenSmoothRadioButtonRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.RadioButtonInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothRadioButtonRenderer : NuGenSmoothRenderer, INuGenRadioButtonRenderer
	{
		#region INuGenRadioButtonRenderer Members

		/*
		 * DrawRadioButton
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawRadioButton(NuGenRadioButtonPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState state = paintParams.State;

			this.DrawEllipseBackground(g, bounds, state);

			using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				this.DrawEllipseBorder(g, bounds, state);
			}

			if (paintParams.Checked)
			{
				this.DrawRadio(g, bounds, state);
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * DrawRadio
		 */

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <param name="state"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		private void DrawRadio(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			Debug.Assert(this.ColorManager != null, "this.ColorManager != null");
			bounds.Inflate(-3, -3);
			using (SolidBrush sb = new SolidBrush(this.ColorManager.GetBorderColor(state)))
			{
				using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.FillEllipse(sb, bounds);
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothRadioButtonRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothRadioButtonRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
