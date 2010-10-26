/* -----------------------------------------------
 * NuGenSmoothTextBoxServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.TextBoxInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenSmoothColorManager"/><para/>
	/// <see cref="INuGenTextBoxRenderer"/><para/>
	/// </summary>
	public class NuGenSmoothTextBoxServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * TextBoxRenderer
		 */

		private INuGenTextBoxRenderer _textBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenTextBoxRenderer TextBoxRenderer
		{
			get
			{
				if (_textBoxRenderer == null)
				{
					_textBoxRenderer = new NuGenSmoothTextBoxRenderer(this);
				}

				return _textBoxRenderer;
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

			if (serviceType == typeof(INuGenTextBoxRenderer))
			{
				Debug.Assert(this.TextBoxRenderer != null, "this.TextBoxRenderer != null");
				return this.TextBoxRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTextBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothTextBoxServiceProvider()
		{

		}
	}
}
