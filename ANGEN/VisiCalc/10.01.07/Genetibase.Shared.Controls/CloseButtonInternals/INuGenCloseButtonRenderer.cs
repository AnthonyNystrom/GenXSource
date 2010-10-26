/* -----------------------------------------------
 * INuGenCloseButtonRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Shared.Controls.CloseButtonInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenCloseButton"/>.
	/// </summary>
	public interface INuGenCloseButtonRenderer
	{
		/// <summary>
		/// </summary>
		void DrawCloseButton(NuGenPaintParams paintParams);
	}
}
