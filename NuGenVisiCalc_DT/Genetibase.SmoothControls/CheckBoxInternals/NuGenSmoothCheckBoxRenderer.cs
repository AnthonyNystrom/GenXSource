/* -----------------------------------------------
 * NuGenSmoothCheckBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.SmoothControls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.CheckBoxInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothCheckBoxRenderer : NuGenSmoothRenderer, INuGenCheckBoxRenderer
	{
		#region INuGenCheckBoxRenderer Members

		/*
		 * DrawCheckBox
		 */

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawCheckBox(NuGenCheckBoxPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			Graphics g = paintParams.Graphics;
			Rectangle bounds = paintParams.Bounds;
			NuGenControlState currentState = paintParams.State;
			CheckState checkState = paintParams.CheckState;

			if (checkState == CheckState.Indeterminate && currentState != NuGenControlState.Disabled)
			{
				this.DrawBackground(g, bounds, NuGenControlState.Pressed);
			}
			else
			{
				this.DrawBackground(paintParams);
			}

			this.DrawBorder(g, bounds, currentState);

			if (checkState == CheckState.Checked)
			{
				this.DrawCheck(g, bounds, currentState);
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * DrawCheck
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///		Border should return an array containing at least 1 element.
		/// </para>
		/// </exception>
		private void DrawCheck(Graphics g, Rectangle bounds, NuGenControlState state)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			int x = bounds.X;
			int y = bounds.Y;

			x += 3;
			y += 3;

			Point p1 = new Point(x, y + 2);
			Point p2 = new Point(x + 2, y + 4);
			Point p3 = new Point(x + 6, y);

			using (Pen pen = this.GetBorderPen(state))
			{
				g.DrawLine(pen, p1, p2);
				g.DrawLine(pen, p2, p3);

				p1.Y++;
				p2.Y++;
				p3.Y++;

				g.DrawLine(pen, p1, p2);
				g.DrawLine(pen, p2, p3);

				p1.Y++;
				p2.Y++;
				p3.Y++;

				g.DrawLine(pen, p1, p2);
				g.DrawLine(pen, p2, p3);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCheckBoxRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothCheckBoxRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
