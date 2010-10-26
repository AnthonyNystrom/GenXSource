/* -----------------------------------------------
 * NuGenSelectionPlotTool.cs
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
	public class NuGenSelectionPlotTool : NuGenPlotToolBase
	{
		private static readonly Cursor[] _curs = new Cursor[] { Cursors.Default, Cursors.SizeWE, Cursors.SizeWE, Cursors.SizeAll };
		
		private int ac, dx;

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void MouseDown(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (sender == null || e.Button != MouseButtons.Left)
				return;
			NuGenPlotPaintInterval selscr = sender.SelectionScreen;
			ac = Action(selscr, e.X);
			switch (ac)
			{
				case 1:
				case 3:
				dx = e.X - selscr.Start;
				break;
				case 2:
				dx = e.X - selscr.End;
				break;
				default:
				sender.SelectionScreen = new NuGenPlotPaintInterval(e.X, 0);
				dx = 0;
				ac = 2;
				break;
			}
			sender.Cursor = _curs[ac];
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void MouseMove(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (sender == null)
				return;
			DoSelection(sender, e);
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void MouseUp(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (sender == null)
				return;
			sender.Selection = NuGenVisiPlot2D.RightOrderInterval(sender.Selection);
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void TimerScroll(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (sender == null)
				return;
			sender.BeginUpdate();
			base.AutomaticMove(sender, e, true);
			DoSelection(sender, e);
			sender.EndUpdate();
		}

		private static int Action(NuGenPlotPaintInterval selscr, int x)
		{
			if (x > selscr.Start - 3 && x < selscr.Start + 3)
				return 1;
			else if (x > selscr.End - 3 && x < selscr.End + 3)
				return 2;
			else if (x > selscr.Start + 3 && x < selscr.End - 3)
				return 3;
			else
				return 0;
		}

		private void DoSelection(NuGenVisiPlot2D sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
			{
				ac = Action(
					sender.SelectionScreen, e.X);
				sender.Cursor = _curs[ac];
			}
			else
			{
				Rectangle r_Gitter = sender.GridRectangle;
				NuGenPlotPaintInterval sel = sender.SelectionScreen;
				switch (ac)
				{
					case 3:
					int w = sel.Width;
					sel.Start = e.X - dx;
					sel.Width = w;
					break;
					case 1:
					sel = new NuGenPlotPaintInterval(e.X - dx, sel.End);
					break;
					case 2:
					sel = new NuGenPlotPaintInterval(sel.Start, e.X - dx);
					break;
					case 0:
					sel.Start = sel.End = e.X - dx;
					break;
				}
				if (Control.ModifierKeys == Keys.Shift)
					sender.SetSelectionScreenRound(sel);
				else
					sender.SelectionScreen = sel;
			}
		}
	}
}
