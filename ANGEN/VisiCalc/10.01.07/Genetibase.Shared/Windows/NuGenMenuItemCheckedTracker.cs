/* -----------------------------------------------
 * NuGenMenuItemCheckedTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// If only one checked item is allowed in a group of menu items, create such a group using the
	/// <see cref="CreateGroup"/> method and specify currently checked item for the <see cref="ChangeChecked"/>
	/// method.
	/// </summary>
	public sealed class NuGenMenuItemCheckedTracker : INuGenMenuItemCheckedTracker
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="groupToContainCheckedMenuItem"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="menuItemToCheck"/> is <see langword="null"/>.</para>
		/// </exception>
		public void ChangeChecked(INuGenMenuItemGroup groupToContainCheckedMenuItem, ToolStripMenuItem menuItemToCheck)
		{
			if (groupToContainCheckedMenuItem == null)
			{
				throw new ArgumentNullException("groupToContainCheckedMenuItem");
			}

			if (menuItemToCheck == null)
			{
				throw new ArgumentNullException("menuItemToCheck");
			}

			if (!groupToContainCheckedMenuItem.Items.Contains(menuItemToCheck))
			{
				return;
			}

			if (_groups.ContainsKey(groupToContainCheckedMenuItem))
			{
				ToolStripMenuItem menuItem = _groups[groupToContainCheckedMenuItem];

                if (menuItem != menuItemToCheck)
                {
                    if (menuItem != null)
                    {
                        menuItem.Checked = false;
                    }

                    _groups[groupToContainCheckedMenuItem] = menuItemToCheck;
                    menuItemToCheck.Checked = true;
                }
			}
			else
			{
				_groups.Add(groupToContainCheckedMenuItem, menuItemToCheck);
				menuItemToCheck.Checked = true;
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="menuItems"/> is <see langword="null"/>.</para></exception>
		public INuGenMenuItemGroup CreateGroup(ToolStripMenuItem[] menuItems)
		{
			if (menuItems == null)
			{
				throw new ArgumentNullException("menuItems");
			}

			INuGenMenuItemGroup group = new NuGenMenuItemGroup();

			for (int i = 0; i < menuItems.Length; i++)
			{
				group.Items.Add(menuItems[i]);
			}

			return group;
		}

		private Dictionary<INuGenMenuItemGroup, ToolStripMenuItem> _groups;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMenuItemCheckedTracker"/> class.
		/// </summary>
		public NuGenMenuItemCheckedTracker()
		{
			_groups = new Dictionary<INuGenMenuItemGroup, ToolStripMenuItem>();
		}
	}
}
