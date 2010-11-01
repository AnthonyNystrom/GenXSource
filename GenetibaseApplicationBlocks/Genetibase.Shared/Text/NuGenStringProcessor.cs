/* -----------------------------------------------
 * NuGenStringProcessor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Text
{
	/// <summary>
	/// Provides helper methods for string processing.
	/// </summary>
	public sealed class NuGenStringProcessor : INuGenStringProcessor
	{
		#region Declarations.Consts

		private const string EAT_TRAILER = "...";

		#endregion

		#region Methods.Private

		/// <summary>
		/// Replaces the trailer symbols of the specified line with "...".
		/// </summary>		
		private string EaterSubRoutine(string stringToTrim, Font stringFont, float maxLength, Graphics grfx)
		{
			Debug.Assert(stringFont != null, "stringFont != null");
			Debug.Assert(grfx != null, "grfx != null");

			if (stringToTrim == null)
			{
				return "";
			}

			if (maxLength <= grfx.MeasureString(EAT_TRAILER, stringFont).Width)
				return "";

			// Maybe the input string is already of necessary length.
			if (grfx.MeasureString(stringToTrim, stringFont).Width < maxLength)
			{
				return stringToTrim;
			}
			else
			{
				if (stringToTrim.Length < 4)
					stringToTrim = "";
				else
					stringToTrim = stringToTrim.Remove(stringToTrim.Length - 4, 1);

				return this.EaterSubRoutine(stringToTrim, stringFont, maxLength, grfx);
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * EatLine
		 */

		/// <summary>
		/// Eats tail symbols of the line.
		/// </summary>
		/// <param name="stringToTrim"></param>
		/// <param name="stringFont"></param>
		/// <param name="maxLength"></param>
		/// <param name="grfx">Used to measure the specified string.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="stringFont"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="grfx"/> is <see langword="null"/>.
		/// </exception>
		public string EatLine(string stringToTrim, Font stringFont, float maxLength, Graphics grfx)
		{
			if (stringFont == null)
			{
				throw new ArgumentNullException("stringFont");
			}

			if (grfx == null)
			{
				throw new ArgumentNullException("grfx");
			}

			if (stringToTrim == null)
			{
				return "";
			}

			if (maxLength <= grfx.MeasureString(EAT_TRAILER, stringFont).Width)
			{
				return "";
			}

			// Maybe the input string is already of necessary length.
			if (grfx.MeasureString(stringToTrim, stringFont).Width < maxLength)
			{
				return stringToTrim;
			}
			else
			{
				stringToTrim += EAT_TRAILER;
				return this.EaterSubRoutine(stringToTrim, stringFont, maxLength, grfx);
			}
		}

		/*
		 * GetContentUntilCRLF
		 */

		/// <summary>
		/// Returns the first line in the specified text.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="textToExtractContentFrom"/> is <see langword="null"/>.
		/// </exception>
		public string GetContentUntilCRLF(string textToExtractContentFrom)
		{
			if (textToExtractContentFrom == null)
			{
				throw new ArgumentNullException("textToExtractContentFrom");
			}

			int crlfIndex = textToExtractContentFrom.IndexOf(System.Environment.NewLine);

			if (crlfIndex > -1)
			{
				return textToExtractContentFrom.Substring(0, crlfIndex);
			}

			return textToExtractContentFrom;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenStringProcessor"/> class.
		/// </summary>
		public NuGenStringProcessor()
		{

		}

		#endregion
	}
}
