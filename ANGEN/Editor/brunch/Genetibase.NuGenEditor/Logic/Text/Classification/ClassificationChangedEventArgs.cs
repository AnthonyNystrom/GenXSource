/* -----------------------------------------------
 * ClassificationChangedEventArgs.cs
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
	public class ClassificationChangedEventArgs : EventArgs
	{
		private VersionedTextSpan _changeSpan;

		/// <summary>
		/// </summary>
		public ClassificationChangedEventArgs(VersionedTextSpan changeSpan)
		{
			_changeSpan = changeSpan;
		}

		/// <summary>
		/// </summary>
		public VersionedTextSpan ChangeSpan
		{
			get
			{
				return _changeSpan;
			}
		}
	}


}
