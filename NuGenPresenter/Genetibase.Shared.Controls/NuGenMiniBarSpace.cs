/* -----------------------------------------------
 * NuGenMiniBarSpace.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenMiniBarSpace : NuGenMiniBarControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMiniBarSpace"/> class.
		/// </summary>
		public NuGenMiniBarSpace()
		{
			this.Width = 40;
		}

		/// <summary>
		/// </summary>
		/// <param name="mouse"></param>
		/// <param name="but"></param>
		/// <param name="act"></param>
		/// <returns></returns>
		public override NuGenMiniBarButtonState Action(Point mouse, MouseButtons but, NuGenMiniBarUpdateAction act)
		{
			return NState;
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		public override NuGenMiniBarButtonState NState
		{
			get
			{
				return NuGenMiniBarButtonState.Normal;
			}
		}

		/// <summary>
		/// </summary>
		protected override void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
			{
				width = Math.Min(200, Math.Max(2, width));
				base.SetBounds(x, y, width, height, specified);

				if (this.Owner != null)
				{
					this.Owner.MeasureButtons();
					this.Owner.Refresh();
				}
			}
			else
			{
				base.SetBounds(x, y, width, height, specified);
			}
		}
	}
}
