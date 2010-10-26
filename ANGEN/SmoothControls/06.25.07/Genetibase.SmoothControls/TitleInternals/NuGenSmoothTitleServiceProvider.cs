/* -----------------------------------------------
 * NuGenSmoothTitleServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TitleInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.TitleInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// </summary>
	public class NuGenSmoothTitleServiceProvider : NuGenImageControlServiceProvider
	{
		private INuGenTitleRenderer _titleRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenTitleRenderer TitleRenderer
		{
			get
			{
				if (_titleRenderer == null)
				{
					_titleRenderer = new NuGenSmoothTitleRenderer(this);
				}

				return _titleRenderer;
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

			if (serviceType == typeof(INuGenTitleRenderer))
			{
				Debug.Assert(this.TitleRenderer != null, "this.TitleRenderer != null");
				return this.TitleRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTitleServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothTitleServiceProvider()
		{
		}
	}
}
