/* -----------------------------------------------
 * INuGenSmoothColors.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	public interface INuGenSmoothColors
	{
		/// <summary>
		/// </summary>
		Color BackgroundGradientBegin
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color BackgroundGradientEnd
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color Border
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color ShadowBottomBegin
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color ShadowBottomEnd
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color ShadowLeftBegin
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color ShadowLeftEnd
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color ShadowRightBegin
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color ShadowRightEnd
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color ShadowTopBegin
		{
			get;
		}

		/// <summary>
		/// </summary>
		Color ShadowTopEnd
		{
			get;
		}
	}
}
