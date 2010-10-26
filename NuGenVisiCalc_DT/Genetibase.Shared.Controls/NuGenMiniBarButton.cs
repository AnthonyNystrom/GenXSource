/* -----------------------------------------------
 * NuGenMiniBarButton.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
	public class NuGenMiniBarButton : NuGenMiniBarControl
	{
		#region variablen
		private Bitmap _glyph;
		private bool _click;
		#endregion

		/// <summary>
		/// </summary>
		/// <param name="mouse"></param>
		/// <param name="but"></param>
		/// <param name="act"></param>
		/// <returns></returns>
		public override NuGenMiniBarButtonState Action(Point mouse, MouseButtons but, NuGenMiniBarUpdateAction act)
		{
			if (this.ClientRectangle.Contains(mouse))
			{
				if (act == NuGenMiniBarUpdateAction.MouseDown)
					_click = true;
				else if (act == NuGenMiniBarUpdateAction.MouseUp && _click)
				{
					if (this.Owner != null)
						this.Owner.ClickButton(this);
					_click = false;
				}
				return _click ? NuGenMiniBarButtonState.Pressed : NuGenMiniBarButtonState.Hot;
			}
			else
				return NuGenMiniBarButtonState.Normal;
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
		[DefaultValue(null)]
		public Bitmap Glyph
		{
			get
			{
				return _glyph;
			}
			set
			{
				_glyph = value;
				if (this.Owner != null)
					this.Owner.Refresh();
			}
		}
	}
}
