/* -----------------------------------------------
 * NuGenSmoothPinpointServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Shared.Controls.PinpointInternals;

namespace Genetibase.SmoothControls.PinpointInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenPinpointLayoutManager"/></para>
	/// <para><see cref="INuGenPinpointRenderer"/></para>
	/// </summary>
	public class NuGenSmoothPinpointServiceProvider : NuGenControlServiceProvider
	{
		private INuGenPinpointLayoutManager _pinpointLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenPinpointLayoutManager PinpointLayoutManager
		{
			get
			{
				if (_pinpointLayoutManager == null)
				{
					_pinpointLayoutManager = new NuGenSmoothPinpointLayoutManager();
				}

				return _pinpointLayoutManager;
			}
		}

		private INuGenPinpointRenderer _pinpointRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenPinpointRenderer PinpointRenderer
		{
			get
			{
				if (_pinpointRenderer == null)
				{
					_pinpointRenderer = new NuGenSmoothPinpointRenderer(this);
				}

				return _pinpointRenderer;
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

			if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenPinpointRenderer))
			{
				Debug.Assert(this.PinpointRenderer != null, "this.PinpointRenderer != null");
				return this.PinpointRenderer;
			}
			else if (serviceType == typeof(INuGenPinpointLayoutManager))
			{
				Debug.Assert(this.PinpointLayoutManager != null, "this.PinpointLayoutManager != null");
				return this.PinpointLayoutManager;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPinpointServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothPinpointServiceProvider()
		{
		}
	}
}
