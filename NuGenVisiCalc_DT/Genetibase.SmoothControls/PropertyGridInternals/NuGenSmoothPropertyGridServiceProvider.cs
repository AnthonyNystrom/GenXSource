/* -----------------------------------------------
 * NuGenSmoothPropertyGridServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PropertyGridInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.SmoothControls.PropertyGridInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenSmoothColorManager"/><para/>
	/// <see cref="INuGenPropertyGridRenderer"/><para/>
	/// </summary>
	public class NuGenSmoothPropertyGridServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * PropertyGridRenderer
		 */

		private INuGenPropertyGridRenderer _propertyGridRenderer;

		/// <summary>
		/// </summary>
		protected INuGenPropertyGridRenderer PropertyGridRenderer
		{
			get
			{
				if (_propertyGridRenderer == null)
				{
					_propertyGridRenderer = new NuGenSmoothPropertyGridRenderer(this);
				}

				return _propertyGridRenderer;
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

			if (serviceType == typeof(INuGenPropertyGridRenderer))
			{
				return this.PropertyGridRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPropertyGridServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothPropertyGridServiceProvider()
		{

		}

		#endregion
	}
}
