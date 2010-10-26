/* -----------------------------------------------
 * NuGenSmoothSplitContainerServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SplitContainerInternals;
using Genetibase.Shared.Windows;

using System;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.SplitContainerInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenSmoothColorManager"/><para/>
	/// <see cref="INuGenSplitContainerRenderer"/><para/>
	/// </summary>
	public class NuGenSmoothSplitContainerServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * SplitContainerRenderer
		 */

		private INuGenSplitContainerRenderer _splitContainerRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenSplitContainerRenderer SplitContainerRenderer
		{
			get
			{
				if (_splitContainerRenderer == null)
				{
					_splitContainerRenderer = new NuGenSmoothSplitContainerRenderer(this);
				}

				return _splitContainerRenderer;
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

			if (serviceType == typeof(INuGenSplitContainerRenderer))
			{
				Debug.Assert(this.SplitContainerRenderer != null, "this.SplitContainerRenderer != null");
				return this.SplitContainerRenderer;
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
		/// Initializes a new instance of the <see cref="NuGenSmoothSplitContainerServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothSplitContainerServiceProvider()
		{
		}

		#endregion
	}
}
