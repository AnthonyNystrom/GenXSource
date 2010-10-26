/* -----------------------------------------------
 * NuGenSmoothServiceProvider.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothServiceProvider : NuGenServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * SmoothColorManager
		 */

		private INuGenSmoothColorManager _smoothColorManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenSmoothColorManager SmoothColorManager
		{
			get
			{
				if (_smoothColorManager == null)
				{
					_smoothColorManager = new NuGenSmoothColorManager();
				}

				return _smoothColorManager;
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
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceType"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			
			if (serviceType == typeof(INuGenSmoothColorManager))
			{
				Debug.Assert(this.SmoothColorManager != null, "this.SmoothColorManager != null");
				return this.SmoothColorManager;
			}

			return null;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothServiceProvider()
		{
		}
	}
}
