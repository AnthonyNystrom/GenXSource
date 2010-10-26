/* -----------------------------------------------
 * NuGenSketchCanvas.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

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
		private sealed partial class Painter
		{
			#region Methods.Public

			public void Clear()
			{
				_state.Clear();
			}

			public void MouseDown(MouseEventArgs e)
			{
				_state.MouseDown(e);
			}

			public void MouseMove(MouseEventArgs e)
			{
				_state.MouseMove(e);
			}

			public void MouseUp(MouseEventArgs e)
			{
				_state.MouseUp(e);
			}

			public void Paint(PaintEventArgs e)
			{
				_state.Paint(e);
			}

			#endregion

			#region Methods.States

			private FreeHandState _freeHandState;

			private PainterState GetFreeHandState()
			{
				if (_freeHandState == null)
				{
					_freeHandState = new FreeHandState(this);
				}

				return _freeHandState;
			}

			#endregion

			private NuGenSketchCanvas _sketchCanvas;
			private PainterState _state;

			public Painter(NuGenSketchCanvas sketchCanvas)
			{
				_sketchCanvas = sketchCanvas;
				_state = this.GetFreeHandState();
			}
		}
	}
}
