/* -----------------------------------------------
 * NuGenSmoothUnitsSpinServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.SpinInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.UnitsSpinInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenInt32ValueConverter"/></para>
	///	<para><see cref="INuGenMeasureUnitsValueConverter"/></para>
	/// <para><see cref="INuGenSpinRenderer"/></para>
	/// </summary>
	public class NuGenSmoothUnitsSpinServiceProvider : NuGenSmoothSpinServiceProvider
	{
		private INuGenMeasureUnitsValueConverter _measureUnitsValueConverter;

		/// <summary>
		/// </summary>
		protected virtual INuGenMeasureUnitsValueConverter MeasureUnitsValueConverter
		{
			get
			{
				if (_measureUnitsValueConverter == null)
				{
					_measureUnitsValueConverter = new NuGenMeasureUnitsValueConverter();
				}

				return _measureUnitsValueConverter;
			}
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		protected override INuGenInt32ValueConverter Int32ValueConverter
		{
			get
			{
				return this.MeasureUnitsValueConverter;
			}
		}

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

			if (serviceType == typeof(INuGenMeasureUnitsValueConverter))
			{
				Debug.Assert(this.MeasureUnitsValueConverter != null, "this.MeasureUnitsValueConverter != null");
				return this.MeasureUnitsValueConverter;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothUnitsSpinServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothUnitsSpinServiceProvider()
		{
		}
	}
}
