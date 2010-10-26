/* -----------------------------------------------
 * NuGenSmoothScrollBarServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.ScrollBarInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenScrollBarRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenValueTrackerService"/></para>
	/// </summary>
	public class NuGenSmoothScrollBarServiceProvider : NuGenControlServiceProvider
	{
		private INuGenScrollBarRenderer _scrollBarRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenScrollBarRenderer ScrollBarRenderer
		{
			get
			{
				if (_scrollBarRenderer == null)
				{
					_scrollBarRenderer = new NuGenSmoothScrollBarRenderer(this);
				}

				return _scrollBarRenderer;
			}
		}

		private INuGenValueTrackerService _valueTrackerService;

		/// <summary>
		/// </summary>
		protected virtual INuGenValueTrackerService ValueTrackerService
		{
			get
			{
				if (_valueTrackerService == null)
				{
					_valueTrackerService = new NuGenValueTrackerService();
				}
				
				return _valueTrackerService;
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

			if (serviceType == typeof(INuGenScrollBarRenderer))
			{
				Debug.Assert(this.ScrollBarRenderer != null, "this.ScrollBarRenderer != null");
				return this.ScrollBarRenderer;
			}
			else if (serviceType == typeof(INuGenValueTrackerService))
			{
				Debug.Assert(this.ValueTrackerService != null, "this.ValueTrackerService != null");
				return this.ValueTrackerService;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothScrollBarServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothScrollBarServiceProvider()
		{

		}
	}
}
