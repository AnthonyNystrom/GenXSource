/* -----------------------------------------------
 * NuGenPropertyGridRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.PropertyGridInternals
{
	/// <summary>
	/// Provides functionality to draw <see cref="NuGenPropertyGrid"/>.
	/// </summary>
	internal sealed class NuGenPropertyGridRenderer : INuGenPropertyGridRenderer
	{
		#region INuGenPropertyGridRenderer Members

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		public void DrawDocComment(NuGenPaintParams paintParams)
		{
			return;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyGridRenderer"/> class.
		/// </summary>
		public NuGenPropertyGridRenderer()
		{

		}

		#endregion
	}
}
