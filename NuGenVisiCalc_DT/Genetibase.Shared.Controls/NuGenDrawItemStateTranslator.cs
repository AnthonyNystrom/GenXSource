/* -----------------------------------------------
 * NuGenDrawItemStateTranslator.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public static class NuGenDrawItemStateTranslator
	{
		/// <summary>
		/// </summary>
		public static NuGenControlState ToControlState(DrawItemState itemState)
		{
			if (
				(itemState & DrawItemState.Disabled) > 0
				|| (itemState & DrawItemState.Grayed) > 0
				)
			{
				return NuGenControlState.Disabled;
			}
			else if (
				(itemState & DrawItemState.HotLight) > 0
				|| (itemState & DrawItemState.Inactive) > 0
				)
			{
				return NuGenControlState.Hot;
			}
			else if (
				(itemState & DrawItemState.Selected) > 0
				)
			{
				return NuGenControlState.Pressed;
			}
			else
			{
				return NuGenControlState.Normal;
			}
		}
	}
}
