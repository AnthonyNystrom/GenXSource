/* -----------------------------------------------
 * NuGenImageControlServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// </summary>
	public class NuGenImageControlServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ControlImageRenderer
		 */

		private INuGenControlImageManager _controlImageManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenControlImageManager ControlImageManager
		{
			get
			{
				if (_controlImageManager == null)
				{
					_controlImageManager = new NuGenControlImageManager();
				}

				return _controlImageManager;
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
			if (serviceType == typeof(INuGenControlImageManager))
			{
				Debug.Assert(this.ControlImageManager != null, "this.ControlImageManager != null");
				return this.ControlImageManager;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImageControlServiceProvider"/> class.
		/// </summary>
		public NuGenImageControlServiceProvider()
		{

		}

		#endregion
	}
}
