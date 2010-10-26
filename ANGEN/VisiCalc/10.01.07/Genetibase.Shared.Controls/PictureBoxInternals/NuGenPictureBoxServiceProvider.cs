/* -----------------------------------------------
 * NuGenPictureBoxServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.Shared.Controls.PictureBoxInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenPictureBoxRenderer"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// </summary>
	public class NuGenPictureBoxServiceProvider : NuGenControlServiceProvider
	{
		/*
		 * PictureBoxRenderer
		 */

		private INuGenPictureBoxRenderer _pictureBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenPictureBoxRenderer PictureBoxRenderer
		{
			get
			{
				if (_pictureBoxRenderer == null)
				{
					_pictureBoxRenderer = new NuGenPictureBoxRenderer();
				}

				return _pictureBoxRenderer;
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
			if (serviceType == typeof(INuGenPictureBoxRenderer))
			{
				Debug.Assert(this.PictureBoxRenderer != null, "this.Renderer != null");
				return this.PictureBoxRenderer;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPictureBoxServiceProvider"/> class.
		/// </summary>
		public NuGenPictureBoxServiceProvider()
		{
		}
	}
}
