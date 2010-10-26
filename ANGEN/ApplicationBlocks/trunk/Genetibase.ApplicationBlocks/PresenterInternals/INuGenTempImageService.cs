/* -----------------------------------------------
 * INuGenTempImageService.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.ApplicationBlocks.PresenterInternals
{
	/// <summary>
	/// Provides methods to work with temporary images.
	/// </summary>
	public interface INuGenTempImageService : IDisposable
	{
		/// <summary>
		/// </summary>
		IList<Image> GetTempImageCollection();

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="imageToSave"/> is <see langword="null"/>.</para>
		/// </exception>
		void SaveTempImage(Image imageToSave);
	}
}
