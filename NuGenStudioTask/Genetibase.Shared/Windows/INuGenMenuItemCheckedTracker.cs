/* -----------------------------------------------
 * INuGenMenuItemCheckedTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// If only one checked item is allowed in a group of menu items, create such a group using the
	/// <see cref="CreateGroup"/> method and specify currently checked item for the <see cref="CheckedChanged"/>
	/// method.
	/// </summary>
	public interface INuGenMenuItemCheckedTracker
	{
		/// <summary>
		/// </summary>
		/// <param name="groupToContainCheckedMenuItem"></param>
		/// <param name="checkedMenuItem"></param>
		void CheckedChanged(
			INuGenMenuItemGroup groupToContainCheckedMenuItem,
			ToolStripMenuItem checkedMenuItem
		);

		/// <summary>
		/// </summary>
		/// <param name="menuItems"></param>
		/// <returns></returns>
		INuGenMenuItemGroup CreateGroup(ToolStripMenuItem[] menuItems);
	}
}
