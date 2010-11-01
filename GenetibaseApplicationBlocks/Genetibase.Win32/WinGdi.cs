/* -----------------------------------------------
 * WinGdi.cs
 * Copyright © 2005-2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines constants declared in WinGDI.h.
	/// </summary>
	public static class WinGdi
	{
		#region Alpha Format

		/// <summary>
		/// Currently defined blend function.
		/// </summary>
		public const int AC_SRC_OVER = 0x00;

		/// <summary>
		/// Alpha format flags.
		/// </summary>
		public const int AC_SRC_ALPHA = 0x01;

		#endregion

		#region Brush Styles

		/// <summary>
		/// Solid brush.
		/// </summary>
		public const int BS_SOLID = 0;
		
		/// <summary>
		/// Same as BS_HOLLOW.
		/// </summary>
		public const int BS_NULL = 1;
		
		/// <summary>
		/// Hollow brush.
		/// </summary>
		public const int BS_HOLLOW = BS_NULL;
		
		/// <summary>
		/// Hatched brush.
		/// </summary>
		public const int BS_HATCHED = 2;
		
		/// <summary>
		/// Pattern brush defined by a memory bitmap.
		/// </summary>
		public const int BS_PATTERN = 3;
		
		/// <summary>
		/// A pattern brush defined by a device-independent bitmap (DIB) specification. If lbStyle is 
		/// BS_DIBPATTERN, the lbHatch member contains a handle to a packed DIB.
		/// </summary>
		public const int BS_DIBPATTERN = 5;
		
		/// <summary>
		/// A pattern brush defined by a device-independent bitmap (DIB) specification. If lbStyle is 
		/// BS_DIBPATTERNPT, the lbHatch member contains a pointer to a packed DIB.
		/// </summary>
		public const int BS_DIBPATTERNPT = 6;
		
		/// <summary>
		/// Same as BS_PATTERN.
		/// </summary>
		public const int BS_PATTERN8X8 = 7;
		
		/// <summary>
		/// Same as BS_DIBPATTERN.
		/// </summary>
		public const int BS_DIBPATTERN8X8 = 8;

		#endregion

		#region Hatch Styles

		/// <summary>
		/// Horizontal hatch.
		/// </summary>
		/// <remarks>-----</remarks>
		public const int HS_HORIZONTAL = 0;      
		
		/// <summary>
		/// Vertical hatch.
		/// </summary>
		/// <remarks>|||||</remarks>
		public const int HS_VERTICAL = 1;
		
		/// <summary>
		/// A 45-degree downward, left-to-right hatch.
		/// </summary>
		/// <remarks>\\\\\</remarks>
		public const int HS_FDIAGONAL = 2;       
		
		/// <summary>
		/// A 45-degree upward, left-to-right hatch.
		/// </summary>
		/// <remarks>/////</remarks>
		public const int HS_BDIAGONAL = 3;       
		
		/// <summary>
		/// Horizontal and vertical cross-hatch.
		/// </summary>
		/// <remarks>+++++</remarks>
		public const int HS_CROSS = 4;
		
		/// <summary>
		/// 45-degree crosshatch.
		/// </summary>
		/// <remarks>xxxxx</remarks>
		public const int HS_DIAGCROSS = 5;       

		#endregion

		#region Pen Styles

		/// <summary>
		/// The pen is solid.
		/// </summary>
		public const int PS_SOLID = 0;
		
		/// <summary>
		/// The pen is dashed. This style is valid only when the pen width is one or less in device units.
		/// </summary>
		/// <remarks>-------</remarks>
		public const int PS_DASH = 1;       
		
		/// <summary>
		/// The pen is dotted. This style is valid only when the pen width is one or less in device units.
		/// </summary>
		/// <remarks>.......</remarks>
		public const int PS_DOT = 2;        
		
		/// <summary>
		/// The pen has alternating dashes and dots. This style is valid only when the pen width is one or 
		/// less in device units.
		/// </summary>
		/// <remarks>_._._._</remarks>
		public const int PS_DASHDOT = 3;    
		
		/// <summary>
		/// The pen has alternating dashes and double dots. This style is valid only when the pen width is one 
		/// or less in device units.
		/// </summary>
		/// <remarks>_.._.._</remarks>
		public const int PS_DASHDOTDOT = 4;
		
		/// <summary>
		/// The pen is invisible.
		/// </summary>
		public const int PS_NULL = 5;
		
		/// <summary>
		/// The pen is solid. When this pen is used in any GDI drawing function that takes a bounding 
		/// rectangle, the dimensions of the figure are shrunk so that it fits entirely in the bounding 
		/// rectangle, taking into account the width of the pen. This applies only to geometric pens.
		/// </summary>
		public const int PS_INSIDEFRAME = 6;

		/// <summary>
		/// Windows NT/2000/XP: The pen uses a styling array supplied by the user.
		/// </summary>
		public const int PS_USERSTYLE = 7;
		
		/// <summary>
		/// Windows NT/2000/XP: The pen sets every other pixel.
		/// (This style is applicable only for cosmetic pens.)
		/// </summary>
		public const int PS_ALTERNATE = 8;
				
		/// <summary>
		/// End caps are round.
		/// </summary>
		public const int PS_ENDCAP_ROUND = 0x00000000;
		
		/// <summary>
		/// End caps are square.
		/// </summary>
		public const int PS_ENDCAP_SQUARE = 0x00000100;
		
		/// <summary>
		/// End caps are flat.
		/// </summary>
		public const int PS_ENDCAP_FLAT = 0x00000200;
				
		/// <summary>
		/// Joins are round.
		/// </summary>
		public const int PS_JOIN_ROUND = 0x00000000;
		
		/// <summary>
		/// Joins are beveled.
		/// </summary>
		public const int PS_JOIN_BEVEL = 0x00001000;
		
		/// <summary>
		/// Joins are mitered when they are within the current limit set by the SetMiterLimit function. 
		/// If it exceeds this limit, the join is beveled.
		/// </summary>
		public const int PS_JOIN_MITER = 0x00002000;
				
		/// <summary>
		/// The pen is cosmetic.
		/// </summary>
		public const int PS_COSMETIC = 0x00000000;
		
		/// <summary>
		/// The pen is geometric.
		/// </summary>
		public const int PS_GEOMETRIC = 0x00010000;
		
		#endregion

		#region Raster Operation Codes
		
		/// <summary>
		/// Pixel is always 0.
		/// </summary>
		public const int R2_BLACK = 1;
		
		/// <summary>
		/// Pixel is the inverse of the R2_MERGEPEN color.
		/// </summary>
		public const int R2_NOTMERGEPEN = 2;
		
		/// <summary>
		/// Pixel is a combination of the colors common to both the screen and the inverse of the pen.
		/// </summary>
		public const int R2_MASKNOTPEN = 3;
		
		/// <summary>
		/// Pixel is the inverse of the pen color.
		/// </summary>
		public const int R2_NOTCOPYPEN = 4;
		
		/// <summary>
		/// Pixel is a combination of the colors common to both the pen and the inverse of the screen.
		/// </summary>
		public const int R2_MASKPENNOT = 5;
		
		/// <summary>
		/// Pixel is the inverse of the screen color.
		/// </summary>
		public const int R2_NOT = 6;
		
		/// <summary>
		/// Pixel is a combination of the colors in the pen and in the screen, but not in both.
		/// </summary>
		public const int R2_XORPEN = 7;
		
		/// <summary>
		/// Pixel is the inverse of the R2_MASKPEN color.
		/// </summary>
		public const int R2_NOTMASKPEN = 8;
		
		/// <summary>
		/// Pixel is a combination of the colors common to both the pen and the screen.
		/// </summary>
		public const int R2_MASKPEN = 9;
		
		/// <summary>
		/// Pixel is the inverse of the R2_XORPEN color.
		/// </summary>
		public const int R2_NOTXORPEN = 10; 
		
		/// <summary>
		/// Pixel remains unchanged.
		/// </summary>
		public const int R2_NOP = 11; 
		
		/// <summary>
		/// Pixel is a combination of the screen color and the inverse of the pen color.
		/// </summary>
		public const int R2_MERGENOTPEN = 12; 
		
		/// <summary>
		/// Pixel is the pen color.
		/// </summary>
		public const int R2_COPYPEN = 13; 
		
		/// <summary>
		/// Pixel is a combination of the pen color and the inverse of the screen color.
		/// </summary>
		public const int R2_MERGEPENNOT = 14; 
		
		/// <summary>
		/// Pixel is a combination of the pen color and the screen color.
		/// </summary>
		public const int R2_MERGEPEN = 15; 
		
		/// <summary>
		/// Pixel is always 1.
		/// </summary>
		public const int R2_WHITE = 16; 
		
		/// <summary>
		/// Copies the source rectangle directly to the destination rectangle.
		/// </summary>
		public const int SRCCOPY = 0x00CC0020; 
		
		/// <summary>
		/// Combines the colors of the source and destination rectangles by using the Boolean OR operator.
		/// </summary>
		public const int SRCPAINT = 0x00EE0086;
		
		/// <summary>
		/// Combines the colors of the source and destination rectangles by using the Boolean AND operator.
		/// </summary>
		public const int SRCAND = 0x008800C6;
		
		/// <summary>
		/// Combines the colors of the source and destination rectangles by using the Boolean XOR operator.
		/// </summary>
		public const int SRCINVERT = 0x00660046;
		
		/// <summary>
		/// Combines the inverted colors of the destination rectangle with the colors of the source rectangle by using the Boolean AND operator.
		/// </summary>
		public const int SRCERASE = 0x00440328;
		
		/// <summary>
		/// Copies the inverted source rectangle to the destination.
		/// </summary>
		public const int NOTSRCCOPY = 0x00330008;
		
		/// <summary>
		/// Combines the colors of the source and destination rectangles by using the Boolean OR operator and then inverts the resultant color.
		/// </summary>
		public const int NOTSRCERASE = 0x001100A6;
		
		/// <summary>
		/// Merges the colors of the source rectangle with the specified pattern by using the Boolean AND operator.
		/// </summary>
		public const int MERGECOPY = 0x00C000CA;
		
		/// <summary>
		/// Merges the colors of the inverted source rectangle with the colors of the destination rectangle by using the Boolean OR operator.
		/// </summary>
		public const int MERGEPAINT = 0x00BB0226;
		
		/// <summary>
		/// Copies the specified pattern into the destination bitmap.
		/// </summary>
		public const int PATCOPY = 0x00F00021;
		
		/// <summary>
		/// Combines the colors of the pattern with the colors of the inverted source rectangle by using the Boolean OR operator. The result of this operation is combined with the colors of the destination rectangle by using the Boolean OR operator.
		/// </summary>
		public const int PATPAINT = 0x00FB0A09;
		
		/// <summary>
		/// Combines the colors of the specified pattern with the colors of the destination rectangle by using the Boolean XOR operator.
		/// </summary>
		public const int PATINVERT = 0x005A0049;
		
		/// <summary>
		/// Inverts the destination rectangle.
		/// </summary>
		public const int DSTINVERT = 0x00550009;
		
		/// <summary>
		/// Fills the destination rectangle using the color associated with index 0 in the physical palette. (This color is black for the default physical palette.)
		/// </summary>
		public const int BLACKNESS = 0x00000042;
		
		/// <summary>
		/// Fills the destination rectangle using the color associated with index 1 in the physical palette. (This color is white for the default physical palette.)
		/// </summary>
		public const int WHITENESS = 0x00FF0062;

		#endregion

		#region Stock Logical Objects

		/// <summary>
		/// White brush.
		/// </summary>
		public const int WHITE_BRUSH = 0;
		
		/// <summary>
		/// Light gray brush.
		/// </summary>
		public const int LTGRAY_BRUSH = 1;
		
		/// <summary>
		/// Gray brush.
		/// </summary>
		public const int GRAY_BRUSH = 2;
		
		/// <summary>
		/// Dark gray brush.
		/// </summary>
		public const int DKGRAY_BRUSH = 3;
		
		/// <summary>
		/// Black brush.
		/// </summary>
		public const int BLACK_BRUSH = 4;
		
		/// <summary>
		/// Null brush (equivalent to HOLLOW_BRUSH).
		/// </summary>
		public const int NULL_BRUSH = 5;
		
		/// <summary>
		/// Hollow brush (equivalent to NULL_BRUSH).
		/// </summary>
		public const int HOLLOW_BRUSH = NULL_BRUSH;
		
		/// <summary>
		/// White pen.
		/// </summary>
		public const int WHITE_PEN = 6;
		
		/// <summary>
		/// Black pen.
		/// </summary>
		public const int BLACK_PEN = 7;
		
		/// <summary>
		/// Null pen.
		/// </summary>
		public const int NULL_PEN = 8;
		
		/// <summary>
		/// Original equipment manufacturer (OEM) dependent fixed-pitch (monospace) font.
		/// </summary>
		public const int OEM_FIXED_FONT = 10;
		
		/// <summary>
		/// Windows fixed-pitch (monospace) system font.
		/// </summary>
		public const int ANSI_FIXED_FONT = 11;
		
		/// <summary>
		/// Windows variable-pitch (proportional space) system font.
		/// </summary>
		public const int ANSI_VAR_FONT = 12;
		
		/// <summary>
		/// System font. By default, the system uses the system font to 
		/// draw menus, dialog box controls, and text.
		/// </summary>
		/// <remarks>Windows 95/98 and Windows NT: The system font is MS Sans Serif.
		/// Windows 2000/XP: The system font is Tahoma.</remarks>
		public const int SYSTEM_FONT = 13;
		
		/// <summary>
		/// Windows NT/2000/XP: Device-dependent font.
		/// </summary>
		public const int DEVICE_DEFAULT_FONT = 14;
		
		/// <summary>
		/// Default palette. This palette consists of the static colors in the system palette.
		/// </summary>
		public const int DEFAULT_PALETTE = 15;
		
		/// <summary>
		/// Fixed-pitch (monospace) system font. This stock object is provided only for 
		/// compatibility with 16-bit Windows versions earlier than 3.0. 
		/// </summary>
		public const int SYSTEM_FIXED_FONT = 16;
		
		/// <summary>
		/// Default font for user interface objects such as menus and dialog boxes. 
		/// This is MS Sans Serif. Compare this with SYSTEM_FONT.
		/// </summary>
		public const int DEFAULT_GUI_FONT = 17;
		
		/// <summary>
		/// Windows 2000/XP: Solid color brush. The default color is white. The color can be 
		/// changed by using the SetDCBrushColor function.
		/// </summary>
		public const int DC_BRUSH = 18;
		
		/// <summary>
		/// Windows 2000/XP: Solid pen color. The default color is white. The color can be 
		/// changed by using the SetDCPenColor function.
		/// </summary>
		public const int DC_PEN = 19;

		#endregion
	}
}
