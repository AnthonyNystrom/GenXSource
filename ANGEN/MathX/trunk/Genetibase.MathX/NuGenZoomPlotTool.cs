/* -----------------------------------------------
 * NuGenZoomPlotTool.cs
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
	public class NuGenZoomPlotTool : NuGenPlotToolBase
	{
		private int i_Mx, i_My;

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void MouseDown(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (sender == null)
				return;
			if (e.Button == MouseButtons.Left)
			{
				sender.Cursor = Cursors.Hand;
				i_Mx = e.X;
				i_My = e.Y;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void MouseMove(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (sender == null)
				return;
			if (e.Button == MouseButtons.Left)
			{
				Rectangle r_Gitter = sender.GridRectangle;
				r_Gitter.Offset(e.X - i_Mx, e.Y - i_My);
				i_Mx = e.X;
				i_My = e.Y;
				sender.GridRectangle = r_Gitter;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void MouseUp(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (sender == null)
				return;
			sender.Cursor = Cursors.Default;
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void MouseWheel(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (sender == null)
				return;
			sender.Cursor = Cursors.Cross;
			Rectangle r_Gitter = sender.GridRectangle;
			double dx = (double)(e.X - r_Gitter.X) / (double)(r_Gitter.Width),
				dy = (double)(e.Y - r_Gitter.Y) / (double)(r_Gitter.Height);
			if (e.Delta > 0)
			{
				if (r_Gitter.Width < 400 && r_Gitter.Height < 400)
				{
					r_Gitter.Width *= 2;
					r_Gitter.Height *= 2;
				}
			}
			else if (e.Delta < 0)
			{
				if (r_Gitter.Width > 5 && r_Gitter.Height > 5)
				{
					r_Gitter.Width /= 2;
					r_Gitter.Height /= 2;
				}
			}
			if (!sender.CenterAxis)
			{
				r_Gitter.X = (int)(e.X - dx * (double)(r_Gitter.Width));
				r_Gitter.Y = (int)(e.Y - dy * (double)(r_Gitter.Height));
			}
			sender.GridRectangle = r_Gitter;
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void TimerScroll(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			base.AutomaticMove(sender, e, false);
		}
	}
}
