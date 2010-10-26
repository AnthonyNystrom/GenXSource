/* -----------------------------------------------
 * ITextVersion.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Data.Text
{
	/// <summary>
	/// </summary>
	public interface ITextVersion
	{
		/// <summary>
		/// </summary>
		TextChange Change
		{
			get;
		}

		/// <summary>
		/// </summary>
		ITextVersion Next
		{
			get;
		}
	}
}
