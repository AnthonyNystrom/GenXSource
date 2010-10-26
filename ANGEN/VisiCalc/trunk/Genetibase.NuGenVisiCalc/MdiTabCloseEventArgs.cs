/* -----------------------------------------------
 * MdiTabCloseEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed class MdiTabCloseEventArgs : EventArgs
	{
		private Boolean _cancel;

		public Boolean Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel = value;
			}
		}

		private Canvas _canvas;

		public Canvas Canvas
		{
			get
			{
				return _canvas;
			}
		}

		public MdiTabCloseEventArgs(Canvas canvas)
		{
			_canvas = canvas;
		}
	}
}
