/* -----------------------------------------------
 * INuGenImageListProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public interface INuGenImageListProvider
	{
		/// <summary>
		/// </summary>
		ImageList ImageList
		{
			get;
			set;
		}
	}
}
