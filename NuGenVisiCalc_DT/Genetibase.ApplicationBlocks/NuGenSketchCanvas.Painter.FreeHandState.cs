/* -----------------------------------------------
 * NuGenSketchCanvas.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenSketchCanvas
	{
		partial class Painter
		{
			private sealed class FreeHandState : PainterState
			{
				public override void Clear()
				{
					_firgures.Clear();
					_currentFigure.Clear();
					_isDrawing = false;
					this.Painter._sketchCanvas.Invalidate();
				}

				public override void MouseDown(MouseEventArgs e)
				{
					if (e.Button == MouseButtons.Left)
					{
						_isDrawing = true;
						_currentFigure.Clear();
						_currentFigure.Add(e.Location);
					}
				}

				public override void MouseMove(MouseEventArgs e)
				{
					if (_isDrawing)
					{
						_currentFigure.Add(e.Location);

						using (Graphics g = Graphics.FromHwnd(this.Painter._sketchCanvas._hWnd))
						using (Pen pen = this.Painter._sketchCanvas.CreatePen())
						{
							using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
							{
								this.Painter._sketchCanvas.InitializeGraphics(g);

								int figureCount = _currentFigure.Count;
								g.DrawLine(pen, _currentFigure[figureCount - 1], _currentFigure[figureCount - 2]);
							}
						}
					}
				}

				public override void MouseUp(MouseEventArgs e)
				{
					_firgures.Add(_currentFigure.ToArray());
					_isDrawing = false;
				}

				public override void Paint(PaintEventArgs e)
				{
					Graphics g = e.Graphics;

					using (NuGenGrfxMode mode = new NuGenGrfxMode(g))
					using (Pen pen = this.Painter._sketchCanvas.CreatePen())
					{
						this.Painter._sketchCanvas.InitializeGraphics(g);

						foreach (Point[] figure in _firgures)
						{
							if (figure.Length > 1)
							{
								g.DrawLines(pen, figure);
							}
						}
					}
				}

				private bool _isDrawing;
				private List<Point> _currentFigure = new List<Point>();
				private List<Point[]> _firgures = new List<Point[]>();

				public FreeHandState(Painter painter)
					: base(painter)
				{
				}
			}
		}
	}
}
