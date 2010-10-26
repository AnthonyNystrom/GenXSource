/* -----------------------------------------------
 * IClassifier.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Windows.Controls.Data.Text;

namespace Genetibase.Windows.Controls.Logic.Text.Classification
{
	/// <summary>
	/// </summary>
	public interface IClassifier
	{
		/// <summary>
		/// </summary>
		event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
		/// <summary>
		/// </summary>
		IList<ClassificationSpan> GetClassificationSpans(VersionedTextSpan span);
	}
}
