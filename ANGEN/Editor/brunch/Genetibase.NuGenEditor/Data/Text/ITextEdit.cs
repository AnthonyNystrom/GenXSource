/* -----------------------------------------------
 * ITextEdit.cs
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
	public interface ITextEdit
	{
		/// <summary>
		/// </summary>
		void Apply();

		/// <summary>
		/// </summary>
		void Delete(Int32 startPosition, Int32 charsToDelete);

		/// <summary>
		/// </summary>
		void Insert(Int32 position, String text);

		/// <summary>
		/// </summary>
		void Insert(Int32 position, Char[] buffer, Int32 startIndex, Int32 length);

		/// <summary>
		/// </summary>
		void Replace(Int32 startPosition, Int32 charsToDelete, String replaceWith);
	}
}
