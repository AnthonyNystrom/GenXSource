/* -----------------------------------------------
 * NuGenPropertyGridServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.PropertyGridInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenPropertyGridRenderer"/><para/>
	/// </summary>
	public class NuGenPropertyGridServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * PropertyGridRenderer
		 */

		private INuGenPropertyGridRenderer _propertyGridRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenPropertyGridRenderer PropertyGridRenderer
		{
			get
			{
				if (_propertyGridRenderer == null)
				{
					_propertyGridRenderer = new NuGenPropertyGridRenderer();
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
				Debug.Assert(this.PropertyGridRenderer != null, "this.PropertyGridRenderer != null");
				return this.PropertyGridRenderer;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyGridServiceProvider"/> class.
		/// </summary>
		public NuGenPropertyGridServiceProvider()
		{

		}

		#endregion
	}
}
