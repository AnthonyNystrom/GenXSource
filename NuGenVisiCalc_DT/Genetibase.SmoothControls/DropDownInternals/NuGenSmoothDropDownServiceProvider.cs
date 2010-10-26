/* -----------------------------------------------
 * NuGenSmoothDropDownServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.DropDownInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenDropDownRenderer"/></para>
	/// <para><see cref="INuGenPanelRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// </summary>
	public class NuGenSmoothDropDownServiceProvider : NuGenImageControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * DropDownRenderer
		 */

		private INuGenDropDownRenderer _dropDownRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenDropDownRenderer DropDownRenderer
		{
			get
			{
				if (_dropDownRenderer == null)
				{
					_dropDownRenderer = new NuGenSmoothDropDownRenderer(this);
				}

				return _dropDownRenderer;
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

			if (serviceType == typeof(INuGenDropDownRenderer))
			{
				Debug.Assert(this.DropDownRenderer != null, "this.DropDownRenderer != null");
				return this.DropDownRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenPanelRenderer))
			{
				return NuGenSmoothServiceManager.PanelServiceProvider.GetService<INuGenPanelRenderer>();
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDropDownServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothDropDownServiceProvider()
		{
		}

		#endregion
	}
}
