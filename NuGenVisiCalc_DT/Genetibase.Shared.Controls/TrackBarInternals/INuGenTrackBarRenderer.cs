/* -----------------------------------------------
 * INuGenTrackBarRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
