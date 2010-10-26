/* -----------------------------------------------
 * NuGenSmoothColorsFocused.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothColorsFocused : INuGenSmoothColors
	{
		/// <summary>
		/// </summary>
		public Color BackgroundGradientBegin
		{
			get
			{
				return _normalColors.BackgroundGradientBegin;
			}
		}

		/// <summary>
		/// </summary>
		public Color BackgroundGradientEnd
		{
			get
			{
				return _normalColors.BackgroundGradientEnd;
			}
		}

		/// <summary>
		/// </summary>
		public Color Border
		{
			get
			{
				return _normalColors.Border;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowBottomBegin
		{
			get
			{
				return _shadow;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowBottomEnd
		{
			get
			{
				return _shadow;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowLeftBegin
		{
			get
			{
				return _shadow;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowLeftEnd
		{
			get
			{
				return _shadow;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowRightBegin
		{
			get
			{
				return _shadow;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowRightEnd
		{
			get
			{
				return _shadow;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowTopBegin
		{
			get
			{
				return _shadow;
			}
		}

		/// <summary>
		/// </summary>
		public Color ShadowTopEnd
		{
			get
			{
				return _shadow;
			}
		}

		private INuGenSmoothColors _normalColors;
		private static readonly Color _shadow = Color.FromArgb(155, 188, 229);

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorsFocused"/> class.
		/// </summary>
		public NuGenSmoothColorsFocused()
		{
			_normalColors = new NuGenSmoothColorsNormal();
		}
	}
}
