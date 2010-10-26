/* -----------------------------------------------
 * NuGenColorUtility.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Genetibase.Shared.Controls.Drawing
{
	internal static class NuGenColorUtility
	{
		private static HatchBrush _hbrs = new HatchBrush(
			HatchStyle.LargeCheckerBoard, Color.Silver, Color.White);
		public static HatchBrush CheckerBrush
		{
			get
			{
				return _hbrs;
			}
		}
	}
}
