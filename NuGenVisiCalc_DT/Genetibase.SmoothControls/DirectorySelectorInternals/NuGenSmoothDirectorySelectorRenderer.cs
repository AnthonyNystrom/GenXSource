/* -----------------------------------------------
 * NuGenSmoothDirectorySelectorRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.DirectorySelectorInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.DirectorySelectorInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothDirectorySelectorRenderer : NuGenSmoothRenderer, INuGenDirectorySelectorRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException"><para><paramref name="paintParams"/> is <see langword="null"/>.</para></exception>
		public void DrawDropDownButton(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			this.DrawScrollButtonBody(paintParams);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDirectorySelectorRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothDirectorySelectorRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
