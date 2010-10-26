/* -----------------------------------------------
 * IAdornmentSurfaceHost.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Editor.Text.View;
using Genetibase.Windows.Controls.Editor.Text.AdornmentSurface;

namespace Genetibase.Windows.Controls.Editor.Text.AdornmentSurfaceManager
{
	/// <summary>
	/// </summary>
	public interface IAdornmentSurfaceHost
	{
		/// <summary>
		/// </summary>
		void AddAdornmentSurface(IAdornmentSurface adornmentSurface);
		/// <summary>
		/// </summary>
		IEditorView TextView
		{
			get;
		}
	}
}
