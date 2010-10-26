/* -----------------------------------------------
 * NuGenPrintPreview.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenPrintPreview
	{
		private sealed class ZoomMenuItem : ToolStripMenuItem
		{
			private bool _autoZoom;

			public bool AutoZoom
			{
				get
				{
					return _autoZoom;
				}
			}

			private float _zoomFactor;

			public float ZoomFactor
			{
				get
				{
					return _zoomFactor;
				}
			}

			public ZoomMenuItem(string text, bool autoZoom)
				: this(text, 1)
			{
				_autoZoom = autoZoom;
			}

			public ZoomMenuItem(string text, float zoomFactor)
				: base(text)
			{
				_zoomFactor = zoomFactor;
			}
		}
	}
}
