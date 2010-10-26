/* -----------------------------------------------
 * NuGenPlotToolBase.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.MathX
{
	/// <summary>
	/// </summary>
	public abstract class NuGenPlotToolBase
	{
		/// <summary>
		/// </summary>
		public static NuGenPlotToolBase[] GetRightOrderArr()
		{
			return new NuGenPlotToolBase[] { new NuGenZoomPlotTool(), new NuGenSelectionPlotTool() };
		}

		/// <summary>
		/// </summary>
		public virtual void MouseDown(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			return;
		}

		/// <summary>
		/// </summary>
		public virtual void MouseMove(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			return;
		}

		/// <summary>
		/// </summary>
		public virtual void MouseWheel(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			return;
		}

		/// <summary>
		/// </summary>
		public virtual void MouseUp(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			return;
		}

		/// <summary>
		/// </summary>
		public virtual void TimerScroll(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			return;
		}

		/// <summary>
		/// </summary>
		protected void AutomaticMove(NuGenVisiPlot2D sender, MouseEventArgs e, bool invert)
		{
			if (sender == null)
				return;
			int dx = 0, dy = 0;
			if (e.X - sender.InnerMargin.X < 0)
				dx = Math.Max(-20, e.X - sender.InnerMargin.X);
			else if (e.X - sender.InnerMargin.Right > 0)
				dx = Math.Min(20, e.X - sender.InnerMargin.Right);

			if (e.Y - sender.InnerMargin.Y < 0)
				dy = Math.Max(-20, e.Y - sender.InnerMargin.Y);
			else if (e.Y - sender.InnerMargin.Bottom > 0)
				dy = Math.Min(20, e.Y - sender.InnerMargin.Bottom);

			if (invert)
			{
				dx = -dx;
				dy = -dy;
			}
			if (Control.ModifierKeys == Keys.Control)
			{
				dx = (int)(5.1 * (double)dx);
				dy = (int)(5.1 * (double)dy);
			}
			if (dx != 0 || dy != 0)
			{
				Rectangle r_Gitter = sender.GridRectangle;
				r_Gitter.X += dx;
				r_Gitter.Y += dy;
				sender.GridRectangle = r_Gitter;
			}
		}
	}
}
