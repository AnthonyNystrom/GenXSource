/* -----------------------------------------------
 * NuGenSmoothGroupBoxServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.GroupBoxInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.GroupBoxInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothGroupBoxServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * GroupBoxRenderer
		 */

		private INuGenGroupBoxRenderer _groupBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenGroupBoxRenderer GroupBoxRenderer
		{
			get
			{
				if (_groupBoxRenderer == null)
				{
					_groupBoxRenderer = new NuGenSmoothGroupBoxRenderer(this);
				}

				return _groupBoxRenderer;
			}
		}

		#endregion

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

			if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenGroupBoxRenderer))
			{
				Debug.Assert(this.GroupBoxRenderer != null, "this.GroupBoxRenderer != null");
				return this.GroupBoxRenderer;
			}

			return base.GetService(serviceType);
		}
		
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothGroupBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothGroupBoxServiceProvider()
		{

		}

		#endregion
	}
}
