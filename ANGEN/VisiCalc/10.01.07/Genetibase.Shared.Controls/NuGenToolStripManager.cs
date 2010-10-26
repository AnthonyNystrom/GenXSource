/* -----------------------------------------------
 * NuGenToolStripManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Sets the injected renderer for <see cref="ToolStripManager"/>.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenToolStripManager : Component
	{
		private ToolStripRenderer _oldRenderer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolStripManager"/> class.
		/// </summary>
		/// <param name="renderer"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="renderer"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenToolStripManager(ToolStripRenderer renderer)
		{
			if (renderer == null)
			{
				throw new ArgumentNullException("renderer");
			}

			_oldRenderer = ToolStripManager.Renderer;
			ToolStripManager.Renderer = renderer;
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ToolStripManager.Renderer = _oldRenderer;
			}

			base.Dispose(disposing);
		}
	}
}
