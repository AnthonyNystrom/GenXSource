/* -----------------------------------------------
 * INuGenTabCloseButtonRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenTabCloseButton"/>.
	/// </summary>
	public interface INuGenTabCloseButtonRenderer
	{
		/// <summary>
		/// </summary>
		void DrawCloseButton(NuGenPaintParams paintParams);
	}
}
