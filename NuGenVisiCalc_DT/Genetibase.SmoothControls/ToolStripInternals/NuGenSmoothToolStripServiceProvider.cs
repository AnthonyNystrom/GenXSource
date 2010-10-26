/* -----------------------------------------------
 * NuGenSmoothToolStripServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ToolStripInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.ToolStripInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenToolStripRenderer"/></para>
	/// </summary>
	public class NuGenSmoothToolStripServiceProvider : NuGenServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ToolStripRenderer
		 */

		private INuGenToolStripRenderer _toolStripRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenToolStripRenderer ToolStripRenderer
		{
			get
			{
				if (_toolStripRenderer == null)
				{
					_toolStripRenderer = new NuGenSmoothToolStripRenderer();
				}

				return _toolStripRenderer;
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
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenToolStripRenderer))
			{
				Debug.Assert(this.ToolStripRenderer != null, "this.ToolStripRenderer != null");
				return this.ToolStripRenderer;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolStripServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothToolStripServiceProvider()
		{

		}

		#endregion
	}
}
