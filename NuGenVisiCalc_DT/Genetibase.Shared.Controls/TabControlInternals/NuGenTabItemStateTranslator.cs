/* -----------------------------------------------
 * NuGenTabItemStateTranslator.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public static class NuGenTabItemStateTranslator
	{
		/// <summary>
		/// </summary>
		public static TabItemState FromControlState(NuGenControlState ctrlState)
		{
			switch (ctrlState)
			{
				case NuGenControlState.Disabled:
				{
					return TabItemState.Disabled;
				}
				case NuGenControlState.Hot:
				{
					return TabItemState.Hot;
				}
				case NuGenControlState.Pressed:
				{
					return TabItemState.Selected;
				}
				default:
				{
					return TabItemState.Normal;
				}
			}
		}

		/// <summary>
		/// </summary>
		public static NuGenControlState ToControlState(TabItemState tabItemState)
		{
			switch (tabItemState)
			{
				case TabItemState.Disabled:
				{
					return NuGenControlState.Disabled;
				}
				case TabItemState.Hot:
				{
					return NuGenControlState.Hot;
				}
				case TabItemState.Selected:
				{
					return NuGenControlState.Pressed;
				}
				default:
				{
					return NuGenControlState.Normal;
				}
			}
		}
	}
}
