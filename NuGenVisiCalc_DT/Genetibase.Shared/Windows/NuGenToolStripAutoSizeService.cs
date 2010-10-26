/* -----------------------------------------------
 * NuGenToolStripAutoSizeService.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Reflection;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public class NuGenToolStripAutoSizeService : INuGenToolStripAutoSizeService
	{
		/// <summary>
		/// </summary>
		/// <param name="targetItem"></param>
		/// <param name="minWidth"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetItem"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void SetNewWidth(ToolStripItem targetItem, int minWidth)
		{
			if (targetItem == null)
			{
				throw new ArgumentNullException("targetItem");
			}

			if (targetItem.Owner == null)
			{
				return;
			}

			ToolStrip toolStrip = targetItem.Owner;
			int itemsWidth = 0;

			foreach (ToolStripItem item in toolStrip.Items)
			{
				if (
					item != targetItem
					)
				{
					itemsWidth += item.Width + 1;
				}
			}

			targetItem.Width = Math.Max(minWidth, toolStrip.Width - itemsWidth);
			NuGenInvoker invoker = new NuGenInvoker(toolStrip);
			invoker.Methods["OnSizeChanged"].Invoke(EventArgs.Empty);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGenToolStripAutoSizeService"/> class.
        /// </summary>
        public NuGenToolStripAutoSizeService()
        {
        }
	}
}
