/* -----------------------------------------------
 * NuGenSmoothPictureBoxLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.PictureBoxInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothPictureBoxLayoutManager : INuGenSmoothPictureBoxLayoutManager
	{
		private static readonly Padding _pictureBoxPadding = new Padding(1);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Padding GetInternalPictureBoxPadding()
		{
			return _pictureBoxPadding;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPictureBoxLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothPictureBoxLayoutManager()
		{
		}
	}
}
