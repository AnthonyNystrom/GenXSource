/* -----------------------------------------------
 * NuGenComboBoxServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.ComboBoxInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenComboBoxRenderer"/></para>
	/// <para><see cref="INuGenImageListService"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothComboBoxServiceProvider : NuGenControlServiceProvider
	{
		/*
		 * ComboBoxRenderer
		 */

		private INuGenComboBoxRenderer _comboBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenComboBoxRenderer ComboBoxRenderer
		{
			get
			{
				if (_comboBoxRenderer == null)
				{
					_comboBoxRenderer = new NuGenSmoothComboBoxRenderer(this);
				}

				return _comboBoxRenderer;
			}
		}

		/*
		 * ImageListService
		 */

		private INuGenImageListService _imageListService;

		/// <summary>
		/// </summary>
		protected virtual INuGenImageListService ImageListService
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
		 * Int32ValueConverter
		 */

		private INuGenInt32ValueConverter _int32ValueConverter;

		/// <summary>
		/// </summary>
		protected virtual INuGenInt32ValueConverter Int32ValueConverter
		{
			get
			{
				if (_int32ValueConverter == null)
				{
					_int32ValueConverter = new NuGenInt32ValueConverter();
				}

				return _int32ValueConverter;
			}
		}

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

			if (serviceType == typeof(INuGenImageListService))
			{
				Debug.Assert(this.ImageListService != null, "this.ImageListService != null");
				return this.ImageListService;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenInt32ValueConverter))
			{
				Debug.Assert(this.Int32ValueConverter != null, "this.Int32ValueConverter != null");
				return this.Int32ValueConverter;
			}
			else if (serviceType == typeof(INuGenComboBoxRenderer))
			{
				Debug.Assert(this.ComboBoxRenderer != null, "this.ComboBoxRenderer != null");
				return this.ComboBoxRenderer;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothComboBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothComboBoxServiceProvider()
		{
		}
	}
}
