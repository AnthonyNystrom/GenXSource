/* -----------------------------------------------
 * NuGenSmoothListBoxServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.ListBoxInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenListBoxRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenImageListService"/></para>
	/// </summary>
	public class NuGenSmoothListBoxServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ImageListService
		 */

		private INuGenImageListService _imageListService;

		/// <summary>
		/// </summary>
		protected INuGenImageListService ImageListService
		{
			get
			{
				if (_imageListService == null)
				{
					_imageListService = new NuGenImageListService();
				}

				return _imageListService;
			}
		}

		/*
		 * ListBoxRenderer
		 */

		private INuGenListBoxRenderer _listBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenListBoxRenderer ListBoxRenderer
		{
			get
			{
				if (_listBoxRenderer == null)
				{
					_listBoxRenderer = new NuGenSmoothListBoxRenderer(this);
				}

				return _listBoxRenderer;
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
			else if (serviceType == typeof(INuGenImageListService))
			{
				Debug.Assert(this.ImageListService != null, "this.ImageListService != null");
				return this.ImageListService;
			}
			else if (serviceType == typeof(INuGenListBoxRenderer))
			{
				Debug.Assert(this.ListBoxRenderer != null, "this.ListBoxRenderer != null");
				return this.ListBoxRenderer;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothListBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothListBoxServiceProvider()
		{

		}

		#endregion
	}
}
