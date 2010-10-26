/* -----------------------------------------------
 * DefaultLineGutterTextSource.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.TextFormatting;
using System.Globalization;

namespace Genetibase.Windows.Controls.Editor
{
	internal class DefaultLineGutterTextSource : TextSource
	{
		private String _text;
		private TextFormattingRunProperties _textFormattingRunProperties;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultLineGutterTextSource"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="text"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="properties"/> is <see langword="null"/>.</para>
		/// </exception>
		public DefaultLineGutterTextSource(String text, TextFormattingRunProperties properties)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			_text = text;
			_textFormattingRunProperties = properties;
		}

		/// <summary>
		/// Retrieves the text span immediately before the specified <see cref="T:System.Windows.Media.TextFormatting.TextSource"/> position.
		/// </summary>
		/// <param name="textSourceCharacterIndexLimit">An <see cref="T:System.Int32"/> value that specifies the character index position where text retrieval stops.</param>
		/// <returns>
		/// A <see cref="T:System.Windows.Media.TextFormatting.CultureSpecificCharacterBufferRange"/> value that represents the text span immediately before <paramref name="textSourceCharacterIndexLimit"/>.
		/// </returns>
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(Int32 textSourceCharacterIndexLimit)
		{
			return new TextSpan<CultureSpecificCharacterBufferRange>(0, new CultureSpecificCharacterBufferRange(CultureInfo.CurrentUICulture, new CharacterBufferRange("", 0, 0)));
		}

		/// <summary>
		/// Retrieves a value that maps a <see cref="T:System.Windows.Media.TextFormatting.TextSource"/> character index to a <see cref="T:System.Windows.Media.TextEffect"/> character index.
		/// </summary>
		/// <param name="textSourceCharacterIndex">An <see cref="T:System.Int32"/> value that specifies the <see cref="T:System.Windows.Media.TextFormatting.TextSource"/> character index to map.</param>
		/// <returns>
		/// An <see cref="T:System.Int32"/> value that represents the <see cref="T:System.Windows.Media.TextEffect"/> character index.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="textSourceCharacterIndex"/> is less than zero or is greater or equal to _text.Length.</para>
		/// </exception>
		public override Int32 GetTextEffectCharacterIndexFromTextSourceCharacterIndex(Int32 textSourceCharacterIndex)
		{
			if ((textSourceCharacterIndex < 0) || (textSourceCharacterIndex >= _text.Length))
			{
				throw new ArgumentOutOfRangeException("textSourceCharacterIndex");
			}
			return textSourceCharacterIndex;
		}

		/// <summary>
		/// Retrieves a <see cref="T:System.Windows.Media.TextFormatting.TextRun"/> starting at a specified <see cref="T:System.Windows.Media.TextFormatting.TextSource"/> position.
		/// </summary>
		/// <param name="textSourceCharacterIndex">Specifies the character index position in the <see cref="T:System.Windows.Media.TextFormatting.TextSource"/> where the <see cref="T:System.Windows.Media.TextFormatting.TextRun"/> is retrieved.</param>
		/// <returns>
		/// A value that represents a <see cref="T:System.Windows.Media.TextFormatting.TextRun"/>, or an Object derived from <see cref="T:System.Windows.Media.TextFormatting.TextRun"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="textSourceCharacterIndex"/> is less than zero or is greater than _text.Length.</para>
		/// </exception>
		public override TextRun GetTextRun(Int32 textSourceCharacterIndex)
		{
			if ((textSourceCharacterIndex < 0) || (textSourceCharacterIndex > _text.Length))
			{
				throw new ArgumentOutOfRangeException("textSourceCharacterIndex");
			}
			if (textSourceCharacterIndex == _text.Length)
			{
				return new TextEndOfLine(1);
			}
			return new TextCharacters(_text.Substring(textSourceCharacterIndex), _textFormattingRunProperties);
		}
	}
}
