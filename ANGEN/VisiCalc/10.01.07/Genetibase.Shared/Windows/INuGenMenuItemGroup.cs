/* -----------------------------------------------
 * INuGenMenuItemGroup.cs
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
	public interface INuGenMenuItemGroup
	{
		/// <summary>
		/// </summary>
		IList<ToolStripMenuItem> Items
		{
			get;
		}
	}
}
