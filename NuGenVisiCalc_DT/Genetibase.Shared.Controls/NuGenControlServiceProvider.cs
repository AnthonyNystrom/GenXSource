/* -----------------------------------------------
 * NuGenControlServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// </summary>
	public class NuGenControlServiceProvider : NuGenServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ButtonStateService
		 */

		private INuGenButtonStateService _buttonStateService;

		/// <summary>
		/// </summary>
		protected virtual INuGenButtonStateService ButtonStateService
		{
			get
			{
				if (_buttonStateService == null)
				{
					_buttonStateService = new NuGenButtonStateService();
				}

				return _buttonStateService;
			}
		}

		/*
		 * ControlStateService
		 */

		private INuGenControlStateService _controlStateService;

		/// <summary>
		/// </summary>
		protected virtual INuGenControlStateService ControlStateService
		{
			get
			{
				if (_controlStateService == null)
				{
					_controlStateService = new NuGenControlStateService();
				}

				return _controlStateService;
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

			if (serviceType == typeof(INuGenButtonStateService))
			{
				return this.ButtonStateService;
			}
			else if (serviceType == typeof(INuGenControlStateService))
			{
				return this.ControlStateService;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlServiceProvider"/> class.
		/// </summary>
		public NuGenControlServiceProvider()
		{

		}

		#endregion
	}
}
