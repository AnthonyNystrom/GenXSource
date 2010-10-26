/* -----------------------------------------------
 * NuGenSmoothFontBoxServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.ComboBoxInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.FontBoxInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenComboBoxRenderer"/><para/>
	/// <see cref="INuGenImageListService"/><para/>
	/// <see cref="INuGenFontFamiliesProvider"/><para/>
	/// <see cref="INuGenSmoothColorManager"/><para/>
	/// </summary>
	public class NuGenSmoothFontBoxServiceProvider : NuGenSmoothComboBoxServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * FontFamiliesProvider
		 */

		private INuGenFontFamiliesProvider _fontFamiliesProvider;

		/// <summary>
		/// </summary>
		protected virtual INuGenFontFamiliesProvider FontFamiliesProvider
		{
			get
			{
				if (_fontFamiliesProvider == null)
				{
					_fontFamiliesProvider = new NuGenFontFamiliesProvider();
				}

				return _fontFamiliesProvider;
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

			if (serviceType == typeof(INuGenFontFamiliesProvider))
			{
				Debug.Assert(this.FontFamiliesProvider != null, "this.FontFamiliesProvider != null");
				return this.FontFamiliesProvider;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothFontBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothFontBoxServiceProvider()
		{

		}

		#endregion
	}
}
