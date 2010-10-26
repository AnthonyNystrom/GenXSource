/* -----------------------------------------------
 * INuGenSmoothColorManager.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	public interface INuGenSmoothColorManager
	{
		/// <summary>
		/// </summary>
		Color GetBackgroundGradientBegin(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetBackgroundGradientEnd(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetBorderColor(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetShadowColorBottomBegin(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetShadowColorBottomEnd(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetShadowColorLeftBegin(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetShadowColorLeftEnd(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetShadowColorRightBegin(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetShadowColorRightEnd(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetShadowColorTopBegin(NuGenControlState state);
		
		/// <summary>
		/// </summary>
		Color GetShadowColorTopEnd(NuGenControlState state);
	}
}
