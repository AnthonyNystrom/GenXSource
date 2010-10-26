/* -----------------------------------------------
 * ITextReader.cs
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
	public interface ITextReader
	{
		/// <summary>
		/// </summary>
		String DocumentType
		{
			get;
		}

		/// <summary>
		/// </summary>
		Char this[Int32 position]
		{
			get;
		}

		/// <summary>
		/// </summary>
		Int32 Length
		{
			get;
		}

		/// <summary>
		/// </summary>
		Int32 LineCount
		{
			get;
		}

		/// <summary>
		/// </summary>
		ITextVersion Version
		{
			get;
		}

		/// <summary>
		/// </summary>
		void CopyTo(Int32 sourceIndex, Char[] destination, Int32 destinationIndex, Int32 count);

		/// <summary>
		/// </summary>
		String GetText();

		/// <summary>
		/// </summary>
		String GetText(Int32 startIndex);

		/// <summary>
		/// </summary>
		String GetText(Int32 startIndex, Int32 length);

		/// <summary>
		/// </summary>
		Char[] ToCharArray();

		/// <summary>
		/// </summary>
		Char[] ToCharArray(Int32 startIndex, Int32 length);
	}
}
