/* -----------------------------------------------
 * NuGenSmoothColorManager.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.SmoothControls.Properties;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Provides methods for colors easy access.
	/// </summary>
	public sealed class NuGenSmoothColorManager : INuGenSmoothColorManager
	{
		#region INuGenSmoothColorManager Members

		/// <summary>
		/// </summary>
		public Color GetBackgroundGradientBegin(NuGenControlState state)
		{
			return this.Colors[state].BackgroundGradientBegin;
		}

		/// <summary>
		/// </summary>
		public Color GetBackgroundGradientEnd(NuGenControlState state)
		{
			return this.Colors[state].BackgroundGradientEnd;
		}

		/// <summary>
		/// </summary>
		public Color GetBorderColor(NuGenControlState state)
		{
			return this.Colors[state].Border;
		}

		/// <summary>
		/// </summary>
		public Color GetShadowColorBottomBegin(NuGenControlState state)
		{
			return this.Colors[state].ShadowBottomBegin;
		}

		/// <summary>
		/// </summary>
		public Color GetShadowColorBottomEnd(NuGenControlState state)
		{
			return this.Colors[state].ShadowBottomEnd;
		}

		/// <summary>
		/// </summary>
		public Color GetShadowColorLeftBegin(NuGenControlState state)
		{
			return this.Colors[state].ShadowLeftBegin;
		}

		/// <summary>
		/// </summary>
		public Color GetShadowColorLeftEnd(NuGenControlState state)
		{
			return this.Colors[state].ShadowLeftEnd;
		}

		/// <summary>
		/// </summary>
		public Color GetShadowColorRightBegin(NuGenControlState state)
		{
			return this.Colors[state].ShadowRightBegin;
		}

		/// <summary>
		/// </summary>
		public Color GetShadowColorRightEnd(NuGenControlState state)
		{
			return this.Colors[state].ShadowRightEnd;
		}

		/// <summary>
		/// </summary>
		public Color GetShadowColorTopBegin(NuGenControlState state)
		{
			return this.Colors[state].ShadowTopBegin;
		}

		/// <summary>
		/// </summary>
		public Color GetShadowColorTopEnd(NuGenControlState state)
		{
			return this.Colors[state].ShadowTopEnd;
		}

		#endregion

		#region Properties.Private

		/*
		 * Colors
		 */

		private Dictionary<NuGenControlState, INuGenSmoothColors> _colors;

		private static readonly INuGenSmoothColors _disabledColors = new NuGenSmoothColorsDisabled();
		private static readonly INuGenSmoothColors _focusedColors = new NuGenSmoothColorsFocused();
		private static readonly INuGenSmoothColors _hotColors = new NuGenSmoothColorsHot();
		private static readonly INuGenSmoothColors _normalColors = new NuGenSmoothColorsNormal();
		private static readonly INuGenSmoothColors _pressedColors = new NuGenSmoothColorsPressed();

		/// <summary>
		/// </summary>
		private Dictionary<NuGenControlState, INuGenSmoothColors> Colors
		{
			get
			{
				if (_colors == null)
				{
					_colors = new Dictionary<NuGenControlState, INuGenSmoothColors>();

					_colors.Add(NuGenControlState.Disabled, _disabledColors);
					_colors.Add(NuGenControlState.Focused, _focusedColors);
					_colors.Add(NuGenControlState.Hot, _hotColors);
					_colors.Add(NuGenControlState.Normal, _normalColors);
					_colors.Add(NuGenControlState.Pressed, _pressedColors);
				}

				return _colors;
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorManager"/> class.
		/// </summary>
		public NuGenSmoothColorManager()
		{
		}
	}
}
