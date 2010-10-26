/* -----------------------------------------------
 * ColorEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace FakeIconEditor
{
	internal sealed class ColorEventArgs : EventArgs
	{
		private Color _color;

		public Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
			}
		}

		public ColorEventArgs(Color color)
		{
			_color = color;
		}
	}
}
