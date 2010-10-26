/* -----------------------------------------------
 * NuGenControlReflectorServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.ControlReflectorInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenReflectedImageGenerator"/></para>
	/// </summary>
	public class NuGenControlReflectorServiceProvider : NuGenServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ImageGenerator
		 */

		private INuGenReflectedImageGenerator _imageGenerator;

		/// <summary>
		/// </summary>
		protected virtual INuGenReflectedImageGenerator ImageGenerator
		{
			get
			{
				if (_imageGenerator == null)
				{
					_imageGenerator = new NuGenReflectedImageGenerator();
				}

				return _imageGenerator;
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
			if (serviceType == typeof(INuGenReflectedImageGenerator))
			{
				Debug.Assert(this.ImageGenerator != null, "this.ImageGenerator != null");
				return this.ImageGenerator;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlReflectorServiceProvider"/> class.
		/// </summary>
		public NuGenControlReflectorServiceProvider()
		{

		}

		#endregion
	}
}
