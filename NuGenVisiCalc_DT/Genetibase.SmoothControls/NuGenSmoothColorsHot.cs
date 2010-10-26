/* -----------------------------------------------
 * NuGenSmoothColorsHot.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothColorsHot : INuGenSmoothColors
	{
		/// <summary>
		/// </summary>
		public Color BackgroundGradientBegin
		{
			get
			{
				return _backgroundGradientBegin;
			}
		}

		/// <summary>
		/// </summary>
		public Color BackgroundGradientEnd
		{
			get
			{
				return _backgroundGradientEnd;
			}
		}

		/// <summary>
		/// </summary>
		public Color Border
		{
			get
			{
				return _border;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowBottomBegin
		{
			get
			{
				return _shadowBottomBegin;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowBottomEnd
		{
			get
			{
				return _shadowBottomEnd;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowLeftBegin
		{
			get
			{
				return _shadowTransparent;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowLeftEnd
		{
			get
			{
				return _shadowTransparent;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowRightBegin
		{
			get
			{
				return _shadowTransparent;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowRightEnd
		{
			get
			{
				return _shadowTransparent;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowTopBegin
		{
			get
			{
				return _shadowTransparent;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowTopEnd
		{
			get
			{
				return _shadowTransparent;
			}
		}

		private static readonly Color _backgroundGradientBegin = Color.FromArgb(177, 206, 239);
		private static readonly Color _backgroundGradientEnd = Color.FromArgb(155, 188, 229);
		private static readonly Color _border = Color.FromArgb(111, 137, 176);
		private static readonly Color _shadowBottomBegin = Color.FromArgb(136, 175, 225);
		private static readonly Color _shadowBottomEnd = Color.FromArgb(116, 161, 220);
		private static readonly Color _shadowTransparent = Color.Transparent;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorsHot"/> class.
		/// </summary>
		public NuGenSmoothColorsHot()
		{
		}
	}
}
