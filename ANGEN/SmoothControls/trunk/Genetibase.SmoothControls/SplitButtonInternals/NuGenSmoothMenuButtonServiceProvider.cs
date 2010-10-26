/* -----------------------------------------------
 * NuGenSmoothMenuButtonServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.SplitButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.SplitButtonInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenSplitButtonLayoutManager"/></para>
	/// <para><see cref="INuGenSplitButtonRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenToolStripRenderer"/></para>
	/// </summary>
	public class NuGenSmoothMenuButtonServiceProvider : NuGenSmoothSplitButtonServiceProvider
	{
		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is <see langword="null"/>.</exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenToolStripRenderer))
			{
				return NuGenSmoothServiceManager.ToolStripServiceProvider.GetService<INuGenToolStripRenderer>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothMenuButtonServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothMenuButtonServiceProvider()
		{
		}
	}
}
