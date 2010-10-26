/* -----------------------------------------------
 * NuGenSmoothAlignSelectorServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.RadioButtonInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.AlignSelectorInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenRadioButtonLayoutManager"/></para>
	/// <para><see cref="INuGenRadioButtonRenderer"/></para>
	/// </summary>
	public class NuGenSmoothAlignSelectorServiceProvider : NuGenSmoothRadioButtonServiceProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothAlignSelectorServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothAlignSelectorServiceProvider()
		{
		}
	}
}
