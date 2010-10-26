/* -----------------------------------------------
 * INuGenRibbonFormProperties.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// Provides Ribbon form parameters.
	/// </summary>
	public interface INuGenRibbonFormProperties
	{
		/// <summary>
		/// </summary>
		int BottomLeftCornerSize
		{
			get;
		}

		/// <summary>
		/// </summary>
		int BottomRightCornerSize
		{
			get;
		}

		/// <summary>
		/// </summary>
		int DisplayRectangleReductionBottom
		{
			get;
		}

		/// <summary>
		/// </summary>
		int DisplayRectangleReductionHorizontal
		{
			get;
		}

		/// <summary>
		/// </summary>
		int DisplayRectangleReductionTop
		{
			get;
		}

		/// <summary>
		/// </summary>
		int TopLeftCornerSize
		{
			get;
		}

		/// <summary>
		/// </summary>
		int TopRightCornerSize
		{
			get;
		}
	}
}
