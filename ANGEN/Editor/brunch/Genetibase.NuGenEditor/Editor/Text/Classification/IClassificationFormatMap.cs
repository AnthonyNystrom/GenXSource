/* -----------------------------------------------
 * IClassificationFormatMap.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Editor.Text.Classification
{
	/// <summary>
	/// </summary>
	public interface IClassificationFormatMap
	{
		/// <summary>
		/// </summary>
		TextFormattingRunProperties GetTextProperties(String classification);

		/// <summary>
		/// </summary>
		IEnumerable<String> ClassificationsList
		{
			get;
		}
	}
}
