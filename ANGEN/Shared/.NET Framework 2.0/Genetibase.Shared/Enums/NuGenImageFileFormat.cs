/* -----------------------------------------------
 * NuGenImageFileFormat.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Shared
{
	/// <summary>
	/// Defines the image file format.
	/// </summary>
	[Flags]
	public enum NuGenImageFileFormat
	{
		/// <summary>
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// </summary>
		Bmp = 0x0001,

		/// <summary>
		/// </summary>
		Emf = 0x0002,

		/// <summary>
		/// </summary>
		Exif = 0x0004,

		/// <summary>
		/// </summary>
		Gif = 0x0010,

		/// <summary>
		/// </summary>
		Jpeg = 0x0020,

		/// <summary>
		/// </summary>
		Png = 0x0040,

		/// <summary>
		/// </summary>
		Tiff = 0x0100,

		/// <summary>
		/// </summary>
		Wmf = 0x0200
	}
}
