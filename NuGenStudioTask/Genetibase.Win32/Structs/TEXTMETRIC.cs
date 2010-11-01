/* -----------------------------------------------
 * TEXTMETRIC.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains basic information about a physical font. All sizes are specified in logical units; that is,
	/// they depend on the current mapping mode of the display context.
	/// </summary>
	public struct TEXTMETRIC
	{
		/// <summary>
		/// Specifies the height (ascent + descent) of characters.
		/// </summary>
		public int tmHeight;

		/// <summary>
		/// Specifies the ascent (units above the base line) of characters.
		/// </summary>
		public int tmAscent;

		/// <summary>
		/// Specifies the descent (units below the base line) of characters.
		/// </summary>
		public int tmDescent;

		/// <summary>
		/// Specifies the amount of leading (space) inside the bounds set by the tmHeight member. Accent
		/// marks and other diacritical characters may occur in this area. The designer may set this member
		/// to zero.
		/// </summary>
		public int tmInternalLeading;

		/// <summary>
		/// Specifies the amount of extra leading (space) that the application adds between rows. Since this
		/// area is outside the font, it contains no marks and is not altered by text output calls in either
		/// OPAQUE or TRANSPARENT mode. The designer may set this member to zero. 
		/// </summary>
		public int tmExternalLeading;

		/// <summary>
		/// Specifies the average width of characters in the font (generally defined as the width of the
		/// letter x). This value does not include the overhang required for bold or italic characters.
		/// </summary>
		public int tmAveCharWidth;

		/// <summary>
		/// Specifies the width of the widest character in the font.
		/// </summary>
		public int tmMaxCharWidth;

		/// <summary>
		/// Specifies the weight of the font.
		/// </summary>
		public int tmWeight;

		/// <summary>
		/// Specifies the extra width per string that may be added to some synthesized fonts. When
		/// synthesizing some attributes, such as bold or italic, graphics device interface (GDI) or a device
		/// may have to add width to a string on both a per-character and per-string basis. For example, GDI
		/// makes a string bold by expanding the spacing of each character and overstriking by an offset
		/// value; it italicizes a font by shearing the string. In either case, there is an overhang past the
		/// basic string. For bold strings, the overhang is the distance by which the overstrike is offset.
		/// For italic strings, the overhang is the amount the top of the font is sheared past the bottom of
		/// the font. 
		/// The tmOverhang member enables the application to determine how much of the character width
		/// returned by a GetTextExtentPoint32 function call on a single character is the actual character
		/// width and how much is the per-string extra width. The actual width is the extent minus the
		/// overhang.
		/// </summary>
		public int tmOverhang;

		/// <summary>
		/// Specifies the horizontal aspect of the device for which the font was designed.
		/// </summary>
		public int tmDigitizedAspectX;

		/// <summary>
		/// Specifies the vertical aspect of the device for which the font was designed. The ratio of the
		/// tmDigitizedAspectX and tmDigitizedAspectY members is the aspect ratio of the device for which the
		/// font was designed.
		/// </summary>
		public int tmDigitizedAspectY;

		/// <summary>
		/// Specifies the value of the first character defined in the font.
		/// </summary>
		public char tmFirstChar;

		/// <summary>
		/// Specifies the value of the last character defined in the font.
		/// </summary>
		public char tmLastChar;

		/// <summary>
		/// Specifies the value of the character to be substituted for characters not in the font.
		/// </summary>
		public char tmDefaultChar;

		/// <summary>
		/// Specifies the value of the character that will be used to define word breaks for text
		/// justification.
		/// </summary>
		public char tmBreakChar;

		/// <summary>
		/// Specifies an italic font if it is nonzero.
		/// </summary>
		public byte tmItalic;

		/// <summary>
		/// Specifies an underlined font if it is nonzero.
		/// </summary>
		public byte tmUnderlined;

		/// <summary>
		/// Specifies a strikeout font if it is nonzero.
		/// </summary>
		public byte tmStruckOut;

		/// <summary>
		/// Specifies information about the pitch, the technology, and the family of a physical font. 
		/// The four low-order bits of this member specify information about the pitch and the technology of
		/// the font. A constant is defined for each of the four bits.
		/// TMPF_FIXED_PITCH - If this bit is set the font is a variable pitch font. If this bit is clear the font is a fixed pitch font. Note very carefully that those meanings are the opposite of what the constant name implies. 
		/// -or-
		/// TMPF_VECTOR - If this bit is set the font is a vector font. 
		/// -or-
		/// TMPF_TRUETYPE - If this bit is set the font is a TrueType font. 
		/// -or-
		/// TMPF_DEVICE - If this bit is set the font is a device font.
		/// An application should carefully test for qualities encoded in these low-order bits, making no
		/// arbitrary assumptions. For example, besides having their own bits set, TrueType and PostScript
		/// fonts set the TMPF_VECTOR bit. A monospace bitmap font has all of these low-order bits clear; a
		/// proportional bitmap font sets the TMPF_FIXED_PITCH bit. A Postscript printer device font sets the
		/// TMPF_DEVICE, TMPF_VECTOR, and TMPF_FIXED_PITCH bits. 
		/// The four high-order bits of tmPitchAndFamily designate the font's font family. An application can
		/// use the value 0xF0 and the bitwise AND operator to mask out the four low-order bits of
		/// <paramref name="tmPitchAndFamily"/>, thus obtaining a value that can be directly compared with
		/// font family names to find an identical match. For information about font families, see the
		/// description of the LOGFONT structure.
		/// </summary>
		public byte tmPitchAndFamily;

		/// <summary>
		/// Specifies the character set of the font. The character set can be one of the following values. 
		/// ANSI_CHARSET
		/// -or-
		/// BALTIC_CHARSET
		/// -or-
		/// CHINESEBIG5_CHARSET
		/// -or-
		/// DEFAULT_CHARSET
		/// -or-
		/// EASTEUROPE_CHARSET
		/// -or-
		/// GB2312_CHARSET
		/// -or-
		/// GREEK_CHARSET
		/// -or-
		/// HANGUL_CHARSET
		/// -or-
		/// MAC_CHARSET
		/// -or-
		/// OEM_CHARSET
		/// -or-
		/// RUSSIAN_CHARSET
		/// -or-
		/// SHIFTJIS_CHARSET
		/// -or-
		/// SYMBOL_CHARSET
		/// -or-
		/// TURKISH_CHARSET
		/// -or-
		/// VIETNAMESE_CHARSET 
		/// -Korean language edition of Windows-
		/// JOHAB_CHARSET 
		/// -Middle East language edition of Windows-
		/// ARABIC_CHARSET
		/// -or-
		/// HEBREW_CHARSET 
		/// -Thai language edition of Windows- 
		/// THAI_CHARSET 
		/// </summary>
		public byte tmCharSet;
	}
}
