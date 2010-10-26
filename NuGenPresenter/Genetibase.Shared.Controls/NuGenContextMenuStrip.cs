/* -----------------------------------------------
 * NuGenContextMenuStrip.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ToolStripInternals;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="ContextMenuStrip"/>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenContextMenuStrip : ContextMenuStrip
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenContextMenuStrip"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenToolStripRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="NuGenServiceNotFoundException"/>
		public NuGenContextMenuStrip(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			INuGenToolStripRenderer toolStripRenderer = serviceProvider.GetService<INuGenToolStripRenderer>();

			if (toolStripRenderer == null)
			{
				throw new NuGenServiceNotFoundException<INuGenToolStripRenderer>();
			}

			this.Renderer = toolStripRenderer.GetToolStripRenderer();
		}
	}
}
