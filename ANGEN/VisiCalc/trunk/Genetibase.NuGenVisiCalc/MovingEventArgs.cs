/* -----------------------------------------------
 * MovingEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.NuGenVisiCalc
{
	internal class MovingEventArgs : EventArgs
	{
		private Rectangle _rectangle;

		public Rectangle Rectangle
		{
			get
			{
				return _rectangle;
			}

			set
			{
				_rectangle = value;
			}
		}

		public MovingEventArgs(Rectangle rect)
		{
			_rectangle = rect;
		}
	}
}
