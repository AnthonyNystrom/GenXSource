/* -----------------------------------------------
 * NUGenListBoxServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.ListBoxInternals
{
	/// <summary>
	/// </summary>
	public class NuGenListBoxServiceProvider : NuGenControlServiceProvider
	{
		private INuGenImageListService _imageListService;

		/// <summary>
		/// </summary>
		public virtual INuGenImageListService ImageListService
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

		private INuGenListBoxRenderer _listBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenListBoxRenderer ListBoxRenderer
		{
			get
			{
				if (_listBoxRenderer == null)
				{
					_listBoxRenderer = new NuGenListBoxRenderer();
				}

				return _listBoxRenderer;
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

			if (serviceType == typeof(INuGenImageListService))
			{
				return this.ImageListService;
			}
			else if (serviceType == typeof(INuGenListBoxRenderer))
			{
				return this.ListBoxRenderer;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenListBoxServiceProvider"/> class.
		/// </summary>
		public NuGenListBoxServiceProvider()
		{
		}
	}
}
