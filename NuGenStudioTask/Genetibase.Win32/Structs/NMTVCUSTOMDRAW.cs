/* -----------------------------------------------
 * NMTVCUSTOMDRAW.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains information specific to an NM_CUSTOMDRAW (tree view) notification message sent by a tree-view control.
	/// </summary>
	public struct NMTVCUSTOMDRAW
	{
		/// <summary>
		/// NMCUSTOMDRAW structure that contains general custom draw information.
		/// </summary>
		public NMCUSTOMDRAW nmcd;

		/// <summary>
		/// COLORREF value representing the color that will be used to display text foreground in the list-view control.
		/// </summary>
		public uint clrText;

		/// <summary>
		/// COLORREF value representing the color that will be used to display text background in the list-view control. 
		/// </summary>
		public uint clrTextBk;

		/// <summary>
		/// Zero-based level of the item being drawn. The root item is at level zero, a child of the root item is at level one, and so on.
		/// </summary>
		public int iSubItem;
	}
}
