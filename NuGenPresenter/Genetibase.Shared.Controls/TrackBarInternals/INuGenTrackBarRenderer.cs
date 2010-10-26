/* -----------------------------------------------
 * INuGenTrackBarRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.TrackBarInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenTrackBarRenderer
	{
		/// <summary>
		/// </summary>
		void DrawTrackButton(NuGenTrackButtonPaintParams paintParams);
		
		/// <summary>
		/// </summary>
		void DrawTrack(NuGenTrackBarPaintParams paintParams);
	}
}
