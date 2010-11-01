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
	/// <see cref="CreateGroup"/> method and specify currently checked item for the <see cref="CheckedChanged"/>
	/// method.
	/// </summary>
	public class NuGenMenuItemCheckedTracker : INuGenMenuItemCheckedTracker
	{
		#region INuGenMenuItemCheckedTracker Members

		/*
		 * CheckedChanged
		 */

		/// <summary>
		/// </summary>
		/// <param name="groupToContainCheckedMenuItem"></param>
		/// <param name="checkedMenuItem"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="groupToContainCheckedMenuItem"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="checkedMenuItem"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void CheckedChanged(INuGenMenuItemGroup groupToContainCheckedMenuItem, ToolStripMenuItem checkedMenuItem)
		{
			if (groupToContainCheckedMenuItem == null)
			{
				throw new ArgumentNullException("groupToContainCheckedMenuItem");
			}

			if (checkedMenuItem == null)
			{
				throw new ArgumentNullException("checkedMenuItem");
			}

			if (!groupToContainCheckedMenuItem.Items.Contains(checkedMenuItem))
			{
				return;
			}

			if (this.Groups.ContainsKey(groupToContainCheckedMenuItem))
			{
				ToolStripMenuItem menuItem = this.Groups[groupToContainCheckedMenuItem];

				if (menuItem != checkedMenuItem)
				{
					if (menuItem != null)
					{
						menuItem.Checked = false;
					}

					this.Groups[groupToContainCheckedMenuItem] = checkedMenuItem;
				}
			}
			else
			{
				this.Groups.Add(groupToContainCheckedMenuItem, checkedMenuItem);
			}
		}

		/*
		 * CreateGroup
		 */

		/// <summary>
		/// </summary>
		/// <param name="menuItems"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="menuItems"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
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

		#endregion

		#region Properties.Protected

		/*
		 * Groups
		 */

		private Dictionary<INuGenMenuItemGroup, ToolStripMenuItem> _groups = null;

		/// <summary>
		/// </summary>
		protected Dictionary<INuGenMenuItemGroup, ToolStripMenuItem> Groups
		{
			get
			{
				if (_groups == null)
				{
					_groups = new Dictionary<INuGenMenuItemGroup, ToolStripMenuItem>();
				}

				return _groups;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMenuItemCheckedTracker"/> class.
		/// </summary>
		public NuGenMenuItemCheckedTracker()
		{

		}

		#endregion
	}
}
