/* -----------------------------------------------
 * IAdornment.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Editor.Text.Adornment
{
	/// <summary>
	/// </summary>
	public interface IAdornment
	{
		/// <summary>
		/// </summary>
		TextSpan Span
		{
			get;
		}
	}
}
