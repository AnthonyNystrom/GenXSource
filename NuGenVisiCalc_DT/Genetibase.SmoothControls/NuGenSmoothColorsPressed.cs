/* -----------------------------------------------
 * NuGenSmoothColorsPressed.cs
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
	public sealed class NuGenSmoothColorsPressed : INuGenSmoothColors
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
				return _shadowLeftBegin;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowLeftEnd
		{
			get
			{
				return _shadowLeftEnd;
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
				return _shadowTopBegin;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowTopEnd
		{
			get
			{
				return _shadowTopEnd;
			}
		}

		private static readonly Color _backgroundGradientBegin = Color.FromArgb(143, 175, 212);
		private static readonly Color _backgroundGradientEnd = Color.FromArgb(163, 190, 220);
		private static readonly Color _border = Color.FromArgb(111, 137, 176);
		private static readonly Color _shadowBottomBegin = Color.FromArgb(161, 189, 219);
		private static readonly Color _shadowBottomEnd = Color.FromArgb(183, 203, 227);
		private static readonly Color _shadowLeftBegin = Color.FromArgb(124, 159, 204);
		private static readonly Color _shadowLeftEnd = Color.FromArgb(143, 173, 211);
		private static readonly Color _shadowTopBegin = Color.FromArgb(124, 159, 204);
		private static readonly Color _shadowTopEnd = Color.FromArgb(143, 173, 211);
		private static readonly Color _shadowTransparent = Color.Transparent;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorsPressed"/> class.
		/// </summary>
		public NuGenSmoothColorsPressed()
		{
		}
	}
}
