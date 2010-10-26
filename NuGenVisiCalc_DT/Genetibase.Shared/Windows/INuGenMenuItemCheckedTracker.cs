/* -----------------------------------------------
 * INuGenMenuItemCheckedTracker.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// If only one checked item is allowed in a group of menu items, create such a group using the
    /// <see cref="CreateGroup"/> method and specify currently checked item for the <see cref="ChangeChecked"/> method.
	/// </summary>
	public interface INuGenMenuItemCheckedTracker
	{
		/// <summary>
		/// </summary>
		void ChangeChecked(
			INuGenMenuItemGroup groupToContainCheckedMenuItem,
			ToolStripMenuItem menuItemToCheck
		);

		/// <summary>
		/// </summary>
		INuGenMenuItemGroup CreateGroup(ToolStripMenuItem[] menuItems);
	}
}
