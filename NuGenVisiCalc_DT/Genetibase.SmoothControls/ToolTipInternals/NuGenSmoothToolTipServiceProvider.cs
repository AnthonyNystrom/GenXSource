/* -----------------------------------------------
 * NuGenSmoothToolTipServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ToolTipInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.ToolTipInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenToolTipLayoutManager"/></para>
	/// <para><see cref="INuGenToolTipRenderer"/></para>
	/// </summary>
	public class NuGenSmoothToolTipServiceProvider : NuGenServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ToolTipLayoutManager
		 */

		private INuGenToolTipLayoutManager _toolTipLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenToolTipLayoutManager ToolTipLayoutManager
		{
			get
			{
				if (_toolTipLayoutManager == null)
				{
					_toolTipLayoutManager = new NuGenToolTipLayoutManager();
				}

				return _toolTipLayoutManager;
			}
		}

		/*
		 * ToolTipRenderer
		 */

		private INuGenToolTipRenderer _toolTipRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenToolTipRenderer ToolTipRenderer
		{
			get
			{
				if (_toolTipRenderer == null)
				{
					_toolTipRenderer = new NuGenSmoothToolTipRenderer(this);
				}

				return _toolTipRenderer;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

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

			if (serviceType == typeof(INuGenToolTipRenderer))
			{
				Debug.Assert(this.ToolTipRenderer != null, "this.ToolTipRenderer != null");
				return this.ToolTipRenderer;
			}
			else if (serviceType == typeof(INuGenToolTipLayoutManager))
			{
				Debug.Assert(this.ToolTipLayoutManager != null, "this.ToolTipLayoutManager != null");
				return this.ToolTipLayoutManager;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolTipServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothToolTipServiceProvider()
		{
		}
	}
}
