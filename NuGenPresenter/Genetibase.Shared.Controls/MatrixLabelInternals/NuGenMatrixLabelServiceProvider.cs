/* -----------------------------------------------
 * NuGenMatrixLabelServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.MatrixLabelInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenMatrixBuilder"/></para>
	/// </summary>
	public class NuGenMatrixLabelServiceProvider : NuGenControlServiceProvider
	{
		private INuGenMatrixBuilder _matrixBuilder;

		/// <summary>
		/// </summary>
		protected virtual INuGenMatrixBuilder MatrixBuilder
		{
			get
			{
				if (_matrixBuilder == null)
				{
					_matrixBuilder = new NuGenMatrixBuilder();
				}

				return _matrixBuilder;
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

			if (serviceType == typeof(INuGenMatrixBuilder))
			{
				Debug.Assert(this.MatrixBuilder != null, "this.MatrixBuilder != null");
				return this.MatrixBuilder;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMatrixLabelServiceProvider"/> class.
		/// </summary>
		public NuGenMatrixLabelServiceProvider()
		{
		}
	}
}
