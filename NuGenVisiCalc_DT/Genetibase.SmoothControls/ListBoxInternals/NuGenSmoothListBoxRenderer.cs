/* -----------------------------------------------
 * NuGenSmoothListBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.ListBoxInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothListBoxRenderer : NuGenSmoothRenderer, INuGenListBoxRenderer
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothListBoxRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenSmoothColorManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothListBoxRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
