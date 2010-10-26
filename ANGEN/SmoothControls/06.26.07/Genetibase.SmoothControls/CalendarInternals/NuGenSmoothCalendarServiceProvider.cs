/* -----------------------------------------------
 * NuGenSmoothCalendarServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.CalendarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.CalendarInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenCalendarRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothCalendarServiceProvider : NuGenControlServiceProvider
	{
		private INuGenCalendarRenderer _calendarRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenCalendarRenderer CalendarRenderer
		{
			get
			{
				if (_calendarRenderer == null)
				{
					_calendarRenderer = new NuGenSmoothCalendarRenderer(this);
				}

				return _calendarRenderer;
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

			if (serviceType == typeof(INuGenCalendarRenderer))
			{
				return this.CalendarRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalendarServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothCalendarServiceProvider()
		{
		}
	}
}
