/* -----------------------------------------------
 * NuGenSmoothColorsNormal.cs
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
	public sealed class NuGenSmoothColorsNormal : INuGenSmoothColors
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
				return _shadowTransparent;
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
				return _shadowRightEnd;
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

		private static readonly Color _backgroundGradientBegin = Color.FromArgb(240, 242, 236);
		private static readonly Color _backgroundGradientEnd = Color.FromArgb(222, 224, 218);
		private static readonly Color _border = Color.FromArgb(128, 128, 128);
		private static readonly Color _shadowBottomEnd = Color.FromArgb(188, 190, 184);
		private static readonly Color _shadowRightEnd = Color.FromArgb(188, 190, 184);
		private static readonly Color _shadowTransparent = Color.Transparent;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorsNormal"/> class.
		/// </summary>
		public NuGenSmoothColorsNormal()
		{
		}
	}
}
