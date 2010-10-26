/* -----------------------------------------------
 * NuGenSmoothDirectorySelectorServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.DirectorySelectorInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenDirectorySelectorRenderer"/></para>
	/// <para><see cref="INuGenToolStripRenderer"/></para>
	/// <para><see cref="INuGenToolTipLayoutManager"/></para>
	/// <para><see cref="INuGenToolTipRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothDirectorySelectorServiceProvider : NuGenControlServiceProvider
	{
		private INuGenDirectorySelectorRenderer _directorySelectorRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenDirectorySelectorRenderer DirectorySelectorRenderer
		{
			get
			{
				if (_directorySelectorRenderer == null)
				{
					_directorySelectorRenderer = new NuGenSmoothDirectorySelectorRenderer(this);
				}

				return _directorySelectorRenderer;
			}
		}

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

			if (serviceType == typeof(INuGenDirectorySelectorRenderer))
			{
				Debug.Assert(this.DirectorySelectorRenderer != null, "this.DirectorySelectorRenderer != null");
				return this.DirectorySelectorRenderer;
			}
			else if (serviceType == typeof(INuGenToolStripRenderer))
			{
				return NuGenSmoothServiceManager.ToolStripServiceProvider.GetService<INuGenToolStripRenderer>();
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenToolTipLayoutManager))
			{
				return NuGenSmoothServiceManager.ToolTipServiceProvider.GetService<INuGenToolTipLayoutManager>();
			}
			else if (serviceType == typeof(INuGenToolTipRenderer))
			{
				return NuGenSmoothServiceManager.ToolTipServiceProvider.GetService<INuGenToolTipRenderer>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDirectorySelectorServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothDirectorySelectorServiceProvider()
		{
		}
	}
}
