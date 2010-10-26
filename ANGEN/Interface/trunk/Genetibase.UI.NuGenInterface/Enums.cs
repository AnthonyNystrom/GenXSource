/* -----------------------------------------------
 * Enums.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.UI.NuGenInterface
{
	/*
	 * NuGenButtonColor
	 */

	/// <summary>
	/// Defines colors assigned to a button.
	/// </summary>
	public enum NuGenButtonColor
	{
		/// <summary>
		/// Blue button background.
		/// </summary>
		Blue,
		
		/// <summary>
		/// Blue button background is highlighted.
		/// </summary>
		BlueHighlighted,
		
		/// <summary>
		/// Orange button background.
		/// </summary>
		Orange,
		
		/// <summary>
		/// Orange button background is highlighted.
		/// </summary>
		OrangeHighlighted,
		
		/// <summary>
		/// Magenta button background.
		/// </summary>
		Magenta,
		
		/// <summary>
		/// Magenta button background is highlighted.
		/// </summary>
		MagentaHighlighted
	}

	/*
	 * NuGenColorScheme
	 */

	/// <summary>
	/// Defines color schemes supported by a renderer.
	/// </summary>
	public enum NuGenColorScheme
	{
		/// <summary>
		/// Imitates Microsoft Office 2007 Windows XP (blue) style.
		/// </summary>
		Blue,

		/// <summary>
		/// Imitates Microsoft Office 2007 Windows Vista (gray) style.
		/// </summary>
		Gray
	}

	/*
	 * NuGenTabColor
	 */

	/// <summary>
	/// Defines colors assigned to ribbon items.
	/// </summary>
	public enum NuGenRibbonTabColor
	{
		/// <summary>
		/// </summary>
		Default,
		
		/// <summary>
		/// </summary>
		Magenta,
		
		/// <summary>
		/// </summary>
		Orange,
		
		/// <summary>
		/// </summary>
		Green
	}

	/// <summary>
	/// Defines colors assigned to ribbon groups.
	/// </summary>
	public enum NuGenRibbonTabGroupColor
	{
		/// <summary>
		/// </summary>
		Default,
		
		/// <summary>
		/// </summary>
		Magenta,
		
		/// <summary>
		/// </summary>
		Orange,
		
		/// <summary>
		/// </summary>
		Green
	}
}
