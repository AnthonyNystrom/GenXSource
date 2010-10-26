/* -----------------------------------------------
 * NuGenMenuItemGroup.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public class NuGenMenuItemGroup : INuGenMenuItemGroup
	{
		private IList<ToolStripMenuItem> _items;

		/// <summary>
		/// </summary>
		public IList<ToolStripMenuItem> Items
		{
			get
			{
				if (_items == null)
				{
					_items = new List<ToolStripMenuItem>();
				}

				return _items;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMenuItemGroup"/> class.
		/// </summary>
		public NuGenMenuItemGroup()
		{

		}
	}
}
