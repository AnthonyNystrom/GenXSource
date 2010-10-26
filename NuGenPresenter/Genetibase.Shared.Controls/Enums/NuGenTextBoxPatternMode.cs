/* -----------------------------------------------
 * NuGenTextBoxPatternMode.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Defines the patterns that the text is checked against.
	/// </summary>
	[Flags]
	public enum NuGenTextBoxPatternMode : int
	{
		/// <summary>
		/// An empty string.
		/// </summary>
		None = 0,

		/// <summary>
		/// A string consisting of small letters (<c>[a-z]</c>).
		/// </summary>
		SmallLetters = 1,

		/// <summary>
		/// A string consisting of capital letters (<c>[A-Z]</c>).
		/// </summary>
		CapitalLetters = 2,

		/// <summary>
		/// A string consisting of digits (<c>[0-9]</c>).
		/// </summary>
		Digits = 4,

		/// <summary>
		/// A string consisting of characters other than letters or digits (. , ? ! / etc).
		/// </summary>
		NonAlphaNumericCharacters = 8,

		/// <summary>
		/// A string of characters from a user-defined collection.
		/// </summary>
		CharacterCollection = 16,

		/// <summary>
		/// A string specified with a DOS-like wildcard pattern
		/// (* represents a collection of one or more characters; ? represents a single character).
		/// </summary>
		WildcardPattern = 32,

		/// <summary>
		/// A string specified with a regular expression pattern.
		/// </summary>
		RegexPattern = 64,

		/// <summary>
		/// A string consisting of any characters 
		/// (<c>SmallLetters | CapitalLetters | Digits | NonAlphaNumericCharacters</c>)
		/// </summary>
		All = SmallLetters |
			CapitalLetters |
			Digits |
			NonAlphaNumericCharacters
	}
}
