/* -----------------------------------------------
 * ClassificationFormatMapSelector.cs
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
	public delegate IClassificationFormatMap ClassificationFormatMapSelector(String documentType);
}
