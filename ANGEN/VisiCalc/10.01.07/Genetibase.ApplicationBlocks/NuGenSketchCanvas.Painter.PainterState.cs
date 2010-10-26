/* -----------------------------------------------
 * NuGenSketchCanvas.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenSketchCanvas
	{
		partial class Painter
		{
			private abstract class PainterState
			{
				private Painter _painter;

				protected Painter Painter
				{
					get
					{
						return _painter;
					}
				}

				public abstract void Clear();
				public abstract void MouseDown(MouseEventArgs e);
				public abstract void MouseMove(MouseEventArgs e);
				public abstract void MouseUp(MouseEventArgs e);
				public abstract void Paint(PaintEventArgs e);

				public PainterState(Painter painter)
				{
					Debug.Assert(painter != null, "painter != null");
					_painter = painter;
				}
			}
		}
	}
}
