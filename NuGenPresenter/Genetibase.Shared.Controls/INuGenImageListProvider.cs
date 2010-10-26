/* -----------------------------------------------
 * INuGenImageListProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
