/* -----------------------------------------------
 * NuGenSmoothPopupContainerServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.PanelInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.PopupContainerInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// </summary>
	public class NuGenSmoothPopupContainerServiceProvider : NuGenServiceProvider
	{
		#region Methods.Protected.Overridden

		/*
		 * GetService
		 */

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

			if (serviceType == typeof(INuGenPanelRenderer))
			{
				return NuGenSmoothServiceManager.PanelServiceProvider.GetService<INuGenPanelRenderer>();
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPopupContainerServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothPopupContainerServiceProvider()
		{

		}

		#endregion
	}
}
