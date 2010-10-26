/* -----------------------------------------------
 * INuGenStringProcessor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;

namespace Genetibase.Shared.Service
{
	/// <summary>
	/// Indicates that this class provides helper methods for string processing.
	/// </summary>
	public interface INuGenStringProcessor
	{
		/// <summary>
		/// Eats tail symbols of the line.
		/// </summary>
		/// <param name="stringToTrim"></param>
		/// <param name="stringFont"></param>
		/// <param name="maxLength"></param>
		/// <param name="grfx">Used to measure the specified string.</param>
		string EatLine(string stringToTrim, Font stringFont, float maxLength, Graphics grfx);

		/// <summary>
		/// Returns the first line in the specified text.
		/// </summary>
		string GetContentUntilCRLF(string textToExtractContentFrom);
	}
}
