/* -----------------------------------------------
 * BLENDFUNCTION.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Controls blending by specifying the blending functions for source and destination bitmaps.
	/// </summary>
	public struct BLENDFUNCTION
	{
		/// <summary>
		/// Specifies the source blend operation. Currently, the only source and destination blend 
		/// operation that has been defined is AC_SRC_OVER. 
		/// </summary>
		public Byte BlendOp;

		/// <summary>
		/// Must be zero.
		/// </summary>
		public Byte BlendFlags;

		/// <summary>
		/// Specifies an alpha transparency value to be used on the entire source bitmap. The 
		/// SourceConstantAlpha value is combined with any per-pixel alpha values in the source bitmap. 
		/// If you set SourceConstantAlpha to 0, it is assumed that your image is transparent. Set the 
		/// SourceConstantAlpha value to 255 (opaque) when you only want to use per-pixel alpha values.
		/// </summary>
		public Byte SourceConstantAlpha;

		/// <summary>
		/// This member controls the way the source and destination bitmaps are interpreted.
		/// </summary>
		public Byte AlphaFormat;
	}
}
