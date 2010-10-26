/* -----------------------------------------------
 * Gdi32.cs
 * Copyright © 2005-2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Imports Gdi32.dll functions.
	/// </summary>
	public static class Gdi32
	{
		/// <summary>
		/// Background remains untouched.
		/// </summary>
		public const Int32 TRANSPARENT = 1;

		/// <summary>
		/// Background is filled with the current background color before the text, hatched brush, or pen is drawn.
		/// </summary>
		public const Int32 OPAQUE = 2;

		/// <summary>
		/// Displays bitmaps that have transparent or semitransparent pixels.
		/// </summary>
		/// <param name="hdcDest">Handle to the destination device context.</param>
		/// <param name="nXOriginDest">Specifies the x-coordinate, in logical units, of the upper-left
		/// corner of the destination rectangle.</param>
		/// <param name="nYOriginDest">Specifies the y-coordinate, in logical units, of the upper-left
		/// corner of the destination rectangle.</param>
		/// <param name="nWdithDest">Specifies the width, in logical units, of the destination rectangle.</param>
		/// <param name="nHeightDest">Specifies the height, in logical units, of the destination rectangle.</param>
		/// <param name="hdcSrc">Handle to the source device context.</param>
		/// <param name="nXOriginSrc">Specifies the x-coordinate, in logical units, of the upper-left corner
		/// of the source rectangle.</param>
		/// <param name="nYOriginSrc">Specifies the y-coordinate, in logical units, of the upper-left corner
		/// of the source rectangle.</param>
		/// <param name="nWidthSrc">Specifies the width, in logical units, of the source rectangle.</param>
		/// <param name="nHeightSrc">Specifies the height, in logical units, of the source rectangle.</param>
		/// <param name="blendFunction">Specifies the alpha-blending function for source and destination
		/// bitmaps, a global alpha value to be applied to the entire source bitmap, and format information
		/// for the source bitmap. The source and destination blend functions are currently limited to
		/// AC_SRC_OVER.</param>
		/// <returns></returns>
		[DllImport("Gdi32.dll", EntryPoint = "GdiAlphaBlend")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean AlphaBlend(
			IntPtr hdcDest,
			Int32 nXOriginDest,
			Int32 nYOriginDest,
			Int32 nWdithDest,
			Int32 nHeightDest,
			IntPtr hdcSrc,
			Int32 nXOriginSrc,
			Int32 nYOriginSrc,
			Int32 nWidthSrc,
			Int32 nHeightSrc,
			BLENDFUNCTION blendFunction
			);

		/// <summary>
		/// Performs a bit-block transfer of the color data corresponding to a rectangle of pixels from the specified source device context into a destination device context. 
		/// </summary>
		/// <param name="hdcDest">dentifies the destination device context.</param>
		/// <param name="nXDest">Specifies the logical x-coordinate of the upper-left corner of the destination rectangle.</param>
		/// <param name="nYDest">Specifies the logical y-coordinate of the upper-left corner of the destination rectangle.</param>
		/// <param name="nWidth">Specifies the logical width of the source and destination rectangles.</param>
		/// <param name="nHeight">Specifies the logical height of the source and the destination rectangles.</param>
		/// <param name="hdcSrc">Identifies the source device context.</param>
		/// <param name="nXSrc">Specifies the logical x-coordinate of the upper-left corner of the source rectangle.</param>
		/// <param name="nYSrc">Specifies the logical y-coordinate of the upper-left corner of the source rectangle.</param>
		/// <param name="dwRop">
		/// <para>Specifies a raster-operation code. These codes define how the color data for the source rectangle is to be combined with the color data for the destination rectangle to achieve the final color.</para>
		/// <para>BLACKNESS - Fills the destination rectangle using the color associated with index 0 in the physical palette. (This color is black for the default physical palette.)</para>
		/// <para>DSTINVERT - Inverts the destination rectangle.</para>
		/// <para>MERGECOPY - Merges the colors of the source rectangle with the specified pattern by using the Boolean AND operator.</para>
		/// <para>MERGEPAINT - Merges the colors of the inverted source rectangle with the colors of the destination rectangle by using the Boolean OR operator.</para>
		/// <para>NOTSRCCOPY - Copies the inverted source rectangle to the destination.</para>
		/// <para>NOTSRCERASE - Combines the colors of the source and destination rectangles by using the Boolean OR operator and then inverts the resultant color.</para>
		/// <para>PATCOPY - Copies the specified pattern into the destination bitmap.</para>
		/// <para>PATINVERT - Combines the colors of the specified pattern with the colors of the destination rectangle by using the Boolean XOR operator.</para>
		/// <para>PATPAINT - Combines the colors of the pattern with the colors of the inverted source rectangle by using the Boolean OR operator. The result of this operation is combined with the colors of the destination rectangle by using the Boolean OR operator.</para>
		/// <para>SRCAND - Combines the colors of the source and destination rectangles by using the Boolean AND operator.</para>
		/// <para>SRCCOPY - Copies the source rectangle directly to the destination rectangle.</para>
		/// <para>SRCERASE - Combines the inverted colors of the destination rectangle with the colors of the source rectangle by using the Boolean AND operator.</para>
		/// <para>SRCINVERT - Combines the colors of the source and destination rectangles by using the Boolean XOR operator.</para>
		/// <para>SRCPAINT - Combines the colors of the source and destination rectangles by using the Boolean OR operator.</para>
		/// <para>WHITENESS - Fills the destination rectangle using the color associated with index 1 in the physical palette. (This color is white for the default physical palette.)</para>
		/// </param>
		/// <returns>If the function succeeds, the return value is <c>true</c>, otherwise - false.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean BitBlt(IntPtr hdcDest, Int32 nXDest, Int32 nYDest, Int32 nWidth, Int32 nHeight, IntPtr hdcSrc, Int32 nXSrc, Int32 nYSrc, Int32 dwRop);

		/// <summary>
		/// Creates a logical brush that has the specified solid color.
		/// </summary>
		/// <param name="crColor">Specifies the color of the brush. To create a COLORREF color value, use <see cref="T:System.Drawing.ColorTranslator"/>.</param>
		/// <returns>If the function succeeds, the return value identifies a logical brush; otherwise, IntPtr.Zero.</returns>
		[DllImport("Gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(Int32 crColor);

		/// <summary>
		/// Creates a logical brush that has the specified solid color.
		/// </summary>
		/// <param name="crColor">Specifies the color of the brush.</param>
		/// <returns>If the function succeeds, the return value identifies a logical brush; otherwise, IntPtr.Zero.</returns>
		public static IntPtr CreateSolidBrush(Color crColor)
		{
			return CreateSolidBrush(ColorTranslator.ToWin32(crColor));
		}

		/// <summary>
		/// Creates a memory device context (DC) compatible with the specified device.
		/// </summary>
		/// <param name="hdc">Identifies the device context. If this handle is <c>IntPtr.Zero</c>, 
		/// the function creates a memory device context compatible with the application's current screen.</param>
		/// <returns>If the function succeeds, the return value is the handle to a memory device context, 
		/// otherwise - <c>IntPtr.Zero</c>.</returns>
		[DllImport("Gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		/// <summary>
		/// Creates a bitmap compatible with the device that is associated with the specified device context.
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="nWidth"></param>
		/// <param name="nHeight"></param>
		/// <returns>If the function succeeds, the return value is a handle to the bitmap,
		/// otherwise - <c>IntPtr.Zero</c>.</returns>
		[DllImport("Gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, Int32 nWidth, Int32 nHeight);

		/// <summary>
		/// Creates a logical pen that has the specified style, width, and color. The pen can subsequently 
		/// be selected into a device context and used to draw lines and curves.
		/// </summary>
		/// <param name="fnPenStyle">Specifies the pen style.</param>
		/// <param name="nWidth">Specifies the width of the pen, in logical units. If nWidth is zero, the 
		/// pen is a single pixel wide, regardless of the current transformation. 
		/// CreatePen returns a pen with the specified width bit with the PS_SOLID style if you specify a 
		/// width greater than one for the following styles: PS_DASH, PS_DOT, PS_DASHDOT, PS_DASHDOTDOT.</param>
		/// <param name="crColor">Specifies a color reference for the pen color.</param>
		/// <returns>If the function succeeds, the return value is a handle that identifies a logical pen.
		/// If the function fails, the return value is <c>IntPtr.Zero</c>. Windows NT/2000/XP: To get 
		/// extended error information, call GetLastError.</returns>
		[DllImport("Gdi32.dll")]
		public static extern IntPtr CreatePen(Int32 fnPenStyle, Int32 nWidth, Int32 crColor);

		/// <summary>
		/// Creates a logical pen from the specified <see cref="T:System.Drawing.Pen"/>.
		/// </summary>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen"/> to convert.</param>
		/// <returns>If the function succeeds, the return value is a handle that identifies a logical pen.
		/// If the function fails, the return value is <c>IntPtr.Zero</c>.</returns>
		/// <exception cref="ArgumentNullException"><para><paramref name="pen"/> is <see langword="null"/>.</para></exception>
		public static IntPtr CreatePen(Pen pen)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}

			Int32 penStyle;

			switch (pen.DashStyle)
			{
				case DashStyle.Dash:
				penStyle = WinGdi.PS_DASH;
				break;
				case DashStyle.DashDot:
				penStyle = WinGdi.PS_DASHDOT;
				break;
				case DashStyle.DashDotDot:
				penStyle = WinGdi.PS_DASHDOTDOT;
				break;
				case DashStyle.Dot:
				penStyle = WinGdi.PS_DOT;
				break;
				default:
				penStyle = WinGdi.PS_SOLID;
				break;
			}

			Int32 width = (Int32)pen.Width;
			Int32 color = ColorTranslator.ToWin32(pen.Color);

			return Gdi32.CreatePen(penStyle, width, color);
		}

		/// <summary>
		/// Deletes the specified device context (DC). 
		/// </summary>
		/// <param name="hdc">Identifies the device context.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean DeleteDC(IntPtr hdc);

		/// <summary>
		/// Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the Object. After the Object is deleted, the specified handle is no longer valid. 
		/// </summary>
		/// <param name="hObject">Identifies a logical pen, brush, font, bitmap, region, or palette.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>, otherwise - <c>false</c>.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean DeleteObject(IntPtr hObject);

		/// <summary>
		/// Creates a logical cosmetic or geometric pen that has the specified style, width, and brush 
		/// attributes.
		/// </summary>
		/// <param name="dwPenStyle">Specifies a combination of type, style, end cap, and join attributes. 
		/// The values from each category are combined by using the bitwise OR operator (|).</param>
		/// <param name="dwWidth">Specifies the width of the pen. If the dwPenStyle parameter is 
		/// PS_GEOMETRIC, the width is given in logical units.
		/// If dwPenStyle is PS_COSMETIC, the width must be set to 1.</param>
		/// <param name="lplb">Pointer to a LOGBRUSH structure. If dwPenStyle is PS_COSMETIC, the lbColor 
		/// member specifies the color of the pen and the lbStyle member must be set to BS_SOLID. 
		/// If dwPenStyle is PS_GEOMETRIC, all members must be used to specify the brush attributes of 
		/// the pen.</param>
		/// <param name="dwStyleCount">Specifies the length, in DWORD units, of the lpStyle array. 
		/// This value must be zero if dwPenStyle is not PS_USERSTYLE.</param>
		/// <param name="lpStyle">Pointer to an array. The first value specifies the length of the first 
		/// dash in a user-defined style, the second value specifies the length of the first space, and so 
		/// on. This pointer must be <c>null</c> if dwPenStyle is not PS_USERSTYLE.</param>
		/// <returns>If the function succeeds, the return value is a handle that identifies a logical pen;
		/// otherwise, the return value is <c>IntPtr.Zero</c>. Windows NT/2000/XP: To get extended error 
		/// information, call GetLastError.</returns>
		[DllImport("Gdi32.dll")]
		[CLSCompliant(false)]
		public static extern IntPtr ExtCreatePen(
			uint dwPenStyle,
			uint dwWidth,
			ref LOGBRUSH lplb,
			uint dwStyleCount,
			uint[] lpStyle
			);

		/// <summary>
		/// Creates a logical cosmetic or geometric pen that has the specified style, width, and brush 
		/// attributes.
		/// </summary>
		/// <param name="pen">The <see cref="T:System.Drawing.Pen"/> to convert.</param>
		/// <returns>If the function succeeds, the return value is a handle that identifies a logical pen;
		/// otherwise, the return value is <c>IntPtr.Zero</c>.</returns>
		/// <exception cref="ArgumentNullException"><para><paramref name="pen"/> is <see langword="null"/>.</para></exception>
		public static IntPtr ExtCreatePen(Pen pen)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}

			/*
			 * Determine the pen style.
			 */

			uint penStyle = WinGdi.PS_GEOMETRIC | WinGdi.PS_ENDCAP_SQUARE | WinGdi.PS_JOIN_ROUND;

			switch (pen.DashStyle)
			{
				case DashStyle.Dash:
				penStyle |= WinGdi.PS_DASH;
				break;
				case DashStyle.DashDot:
				penStyle |= WinGdi.PS_DASHDOT;
				break;
				case DashStyle.DashDotDot:
				penStyle |= WinGdi.PS_DASHDOTDOT;
				break;
				case DashStyle.Dot:
				penStyle |= WinGdi.PS_DOT;
				break;
				default:
				penStyle |= WinGdi.PS_SOLID;
				break;
			}

			/*
			 * Determine the pen width.
			 */

			uint penWidth = (uint)pen.Width;

			/*
			 * Initialize LOGBRUSH.
			 */

			LOGBRUSH logBrush = new LOGBRUSH();

			logBrush.lbStyle = WinGdi.BS_SOLID;
			logBrush.lbColor = ColorTranslator.ToWin32(pen.Color);

			/*
			 * Call native method.
			 */

			return Gdi32.ExtCreatePen(penStyle, penWidth, ref logBrush, 0, null);
		}

		/// <summary>
		/// Retrieves a handle to one of the stock pens, brushes, fonts, or palettes.
		/// </summary>
		/// <param name="fnObject">Specifies the type of stock Object.</param>
		/// <returns>If the function succeeds, the return value is a handle to the requested logical Object; 
		/// otherwise, <c>IntPtr.Zero</c>.</returns>
		[DllImport("Gdi32.dll")]
		public static extern IntPtr GetStockObject(Int32 fnObject);

		/// <summary>
		/// Retrieves the current stretching mode. The stretching mode defines how color data is
		/// added to or removed from bitmaps that are stretched or compressed when the StretchBlt
		/// function is called.
		/// </summary>
		/// <param name="hdc">Identifies the device context.</param>
		/// <returns>
		/// If the function succeeds, the return value is the current stretching mode.
		/// If the function fails, the return value is zero.
		/// </returns>
		[DllImport("Gdi32.dll")]
		public static extern Int32 GetStretchBltMode(IntPtr hdc);

		/// <summary>
		/// Fills the specified buffer with the metrics for the currently selected font.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="tm"><see cref="T:Genetibase.WinApi.TEXTMETRIC"/> structure that receives the text metrics.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean GetTextMetrics(IntPtr hdc, TEXTMETRIC tm);

		/// <summary>
		/// Creates a new clipping region from the intersection of the current clipping region and the specified rectangle. 
		/// </summary>
		/// <remarks>The lower and right-most edges of the given rectangle are excluded from the clipping region.</remarks>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="nLeftRect">Specifies the x-coordinate, in logical units, of the upper-left corner of the rectangle.</param>
		/// <param name="nTopRect">Specifies the y-coordinate, in logical units, of the upper-left corner of the rectangle.</param>
		/// <param name="nRightRect">Specifies the x-coordinate, in logical units, of the lower-right corner of the rectangle.</param>
		/// <param name="nBottomRect">Specifies the y-coordinate, in logical units, of the lower-right corner of the rectangle.</param>
		/// <returns>The return value specifies the new clipping region's type and can be one of the following
		/// values: NULLREGION: Region is empty. -or- SIMPLEREGION: Region is a single rectangle. -or- COMPLEXREGION: 
		/// Region is more than one rectangle. -or- ERROR: An error occured. (The current clipping region is unaffected.)</returns>
		[DllImport("Gdi32.dll")]
		public static extern Int32 IntersectClipRect(
			IntPtr hdc,
			Int32 nLeftRect,
			Int32 nTopRect,
			Int32 nRightRect,
			Int32 nBottomRect
		);

		/// <summary>
		/// Creates a new clipping region from the intersection of the current clipping region and the specified rectangle. 
		/// </summary>
		/// <remarks>The lower and right-most edges of the given rectangle are excluded from the clipping region.</remarks>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="rect">Specifies the <see cref="T:System.Drawing.Rectangle"/> to use for intersection.</param>
		/// <returns>The return value specifies the new clipping region's type and can be one of the following
		/// values: NULLREGION: Region is empty. -or- SIMPLEREGION: Region is a single rectangle. -or- COMPLEXREGION: 
		/// Region is more than one rectangle. -or- ERROR: An error occured. (The current clipping region is unaffected.)</returns>
		public static Int32 IntersectClipRect(IntPtr hdc, Rectangle rect)
		{
			return IntersectClipRect(hdc, rect.Left, rect.Top, rect.Right, rect.Bottom);
		}

		/// <summary>
		/// Draws a line from the current position up to, but not including, the specified point.
		/// </summary>
		/// <param name="hdc">Identifies a device context.</param>
		/// <param name="nXEnd">Specifies the x-coordinate of the line's ending point.</param>
		/// <param name="nYEnd">Specifies the y-coordinate of the line's ending point.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>, otherwise - <c>false</c>.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean LineTo(IntPtr hdc, Int32 nXEnd, Int32 nYEnd);

		/// <summary>
		/// Updates the current position to the specified point.
		/// </summary>
		/// <param name="hdc">Identifies a device context.</param>
		/// <param name="x">Specifies the x-coordinate of the new position, in logical units.</param>
		/// <param name="y">Specifies the y-coordinate of the new position, in logical units.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>, otherwise - <c>false</c>.</returns>
		public static Boolean MoveTo(IntPtr hdc, Int32 x, Int32 y)
		{
			return MoveToEx(hdc, x, y, IntPtr.Zero);
		}

		/// <summary>
		/// Updates the current position to the specified point and optionally returns the previous position. 
		/// </summary>
		/// <param name="hdc">Identifies a device context.</param>
		/// <param name="x">Specifies the x-coordinate of the new position, in logical units.</param>
		/// <param name="y">Specifies the y-coordinate of the new position, in logical units.</param>
		/// <param name="lpPoint">Points to a POINT structure in which the previous current position is stored. If this parameter is a <c>null</c> pointer, the previous position is not returned.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>, otherwise - <c>false</c>.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean MoveToEx(IntPtr hdc, Int32 x, Int32 y, IntPtr lpPoint);

		/// <summary>
		/// Draws a rectangle. The rectangle is outlined by using the current pen and filled by using the current brush.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="nLeftRect">Specifies the x-coordinate, in logical coordinates, of the upper-left corner of the
		/// rectangle.</param>
		/// <param name="nTopRect">Specifies the y-coordinate, in logical coordinates, of the upper-left corner of the rectangle.</param>
		/// <param name="nRightRect">Specifies the x-coordinate, in logical coordinates, of the lower-right corner of the rectangle.</param>
		/// <param name="nBottomRect">Specifies the y-coordinate, in logical coordinates, of the lower-right corner of the rectangle.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>;otherwise,
		/// the return value is <see langword="false"/>.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean Rectangle(IntPtr hdc, Int32 nLeftRect, Int32 nTopRect, Int32 nRightRect, Int32 nBottomRect);

		/// <summary>
		/// Draws a rectangle with rounded corners. The rectangle is outlined by using the current pen 
		/// and filled by using the current brush.
		/// </summary>
		/// <param name="hdc">Identifies the device context.</param>
		/// <param name="nLeftRect">Specifies the x-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="nTopRect">Specifies the y-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="nRightRect">Specifies the x-coordinate of the lower-right corner of the rectangle.</param>
		/// <param name="nBottomRect">Specifies the y-coordinate of the lower-right corner of the rectangle.</param>
		/// <param name="nWidth">Specifies the width of the ellipse used to draw the rounded corners.</param>
		/// <param name="nHeight">Specifies the height of the ellipse used to draw the rounded corners.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>.
		/// To get extended error information, call GetLastError.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean RoundRect(
			IntPtr hdc,
			Int32 nLeftRect,
			Int32 nTopRect,
			Int32 nRightRect,
			Int32 nBottomRect,
			Int32 nWidth,
			Int32 nHeight
			);

		/// <summary>
		/// Selects an Object into the specified device context. The new Object replaces the previous Object of the same type. 
		/// </summary>
		/// <param name="hdc">Identifies the device context.</param>
		/// <param name="hgdiobj">Identifies the Object to be selected.</param>
		/// <returns>If the selected Object is not a region and the function succeeds, the return value 
		/// is the handle of the Object being replaced. If the selected Object is a region and the 
		/// function succeeds, the return value is one of the following values: SIMPLEREGION
		/// (region consists of a single rectangle), COMPLEXREGION (region consists of more than one 
		/// rectangle), NULLREGION (region is empty). If an error occurs and the selected Object is not 
		/// a region, the return value is <c>IntPtr.Zero</c>. Otherwise, it is GDI_ERROR.</returns>
		[DllImport("Gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		/// <summary>
		/// Sets the current background color to the specified color value, or to the nearest physical color 
		/// if the device cannot represent the specified color value.
		/// </summary>
		/// <param name="hdc">Identifies the device context.</param>
		/// <param name="crColor">Specifies the new background color.</param>
		/// <returns>If the function succeeds, the return value specifies the previous background color 
		/// as a COLORREF value. If the function fails, the return value is CLR_INVALID.</returns>
		[DllImport("Gdi32.dll")]
		public static extern Int32 SetBkColor(IntPtr hdc, Int32 crColor);

		/// <summary>
		/// Sets the background mix mode of the specified device context. The background mix mode is used with text, hatched brushes, and pen styles that are not solid lines. 
		/// </summary>
		/// <param name="hdc">Handle to the device context. </param>
		/// <param name="iBkMode">Specifies the background mode. This parameter can be one of the following values.
		/// OPAQUE: Background is filled with the current background color before the text, hatched brush, or pen is drawn.
		/// -or-
		/// TRANSPARENT: Background remains untouched.</param>
		/// <returns>If the function succeeds, the return value specifies the previous background mode; otherwise, zero.</returns>
		[DllImport("Gdi32.dll")]
		public static extern Int32 SetBkMode(IntPtr hdc, Int32 iBkMode);

		/// <summary>
		/// Sets the current foreground mix mode. GDI uses the foreground mix mode 
		/// to combine pens and interiors of filled objects with the colors already 
		/// on the screen. The foreground mix mode defines how colors from the brush 
		/// or pen and the colors in the existing image are to be combined.
		/// </summary>
		/// <param name="hdc">Identifies the device context.</param>
		/// <param name="fnDrawMode">Specifies the new mix mode.</param>
		/// <returns>If the function succeeds, the return value specifies the previous mix mode,
		/// otherwise the return value is zero.</returns>
		[DllImport("Gdi32.dll")]
		public static extern Int32 SetROP2(IntPtr hdc, Int32 fnDrawMode);

		/// <summary>
		/// Sets the pixel at the specified coordinates to the specified color. 
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="x">Specifies the x-coordinate, in logical units, of the point to be set.</param>
		/// <param name="y">Specifies the y-coordinate, in logical units, of the point to be set.</param>
		/// <param name="crColor">Specifies the color to be used to paint the point.
		/// Use <see cref="M:System.Drawing.ColorTranslator.ToWin32"/> method to create a COLORREF color value.
		/// </param>
		/// <returns>If the function succeeds, the return value is the RGB value that the function sets
		/// the pixel to. This value may differ from the color specified by crColor; that occurs when an
		/// exact match for the specified color cannot be found. If the function fails, the return value is –1.</returns>
		[DllImport("Gdi32.dll")]
		public static extern Int32 SetPixel(IntPtr hdc, Int32 x, Int32 y, Int32 crColor);

		/// <summary>
		/// Sets the pixel at the specified coordinates to the specified color.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="x">Specifies the x-coordinate, in logical units, of the point to be set.</param>
		/// <param name="y">Specifies the y-coordinate, in logical units, of the point to be set.</param>
		/// <param name="color">Specifies the color to be used to paint the point.</param>
		/// <returns>If the function succeeds, the return value is the <see cref="T:Color"/> that the function
		/// sets the pixel to. This value may differ from the color specified by color; that occurs when an
		/// exact match for the specified color cannot be found. If the function fails, the return value is -1.</returns>
		public static Color SetPixel(IntPtr hdc, Int32 x, Int32 y, Color color)
		{
			return ColorTranslator.FromWin32(Gdi32.SetPixel(hdc, x, y, ColorTranslator.ToWin32(color)));
		}

		/// <summary>
		/// Sets the pixel at the specified coordinates to the closest approximation of the specified color.
		/// The point must be in the clipping region and the visible part of the device surface.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="x">Specifies the x-coordinate, in logical units, of the point to be set.</param>
		/// <param name="y">Specifies the y-coordinate, in logical units, of the point to be set.</param>
		/// <param name="crColor">Specifies the color to be used to paint the point.
		/// Use <see cref="M:System.Drawing.ColorTranslator.ToWin32"/> method to create a COLORREF color value.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport("Gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean SetPixelV(IntPtr hdc, Int32 x, Int32 y, Int32 crColor);

		/// <summary>
		/// Sets the pixel at the specified coordinates to the closest approximation of the specified color.
		/// The point must be in the clipping region and the visible part of the device surface.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="x">Specifies the x-coordinate, in logical units, of the point to be set.</param>
		/// <param name="y">Specifies the y-coordinate, in logical units, of the point to be set.</param>
		/// <param name="color">Specifies the color to be used to paint the point.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		public static Boolean SetPixelV(IntPtr hdc, Int32 x, Int32 y, Color color)
		{
			return Gdi32.SetPixelV(hdc, x, y, ColorTranslator.ToWin32(color));
		}

		/// <summary>
		/// </summary>
		/// <param name="hdc">Identifies the device context.</param>
		/// <param name="iStretchMode">
		/// <para>Specifies the stretching mode.  It can be one of the following values:</para>
		/// <para>BLACKONWHITE - Performs a Boolean AND operation using the color values for the eliminated and existing pixels. If the bitmap is a monochrome bitmap, this mode preserves black pixels at the expense of white pixels.</para>
		/// <para>COLORONCOLOR - Deletes the pixels. This mode deletes all eliminated lines of pixels without trying to preserve their information.</para>
		/// <para>HALFTONE - Maps pixels from the source rectangle into blocks of pixels in the destination rectangle. The average color over the destination block of pixels approximates the color of the source pixels. After setting the HALFTONE stretching mode, an application must call the SetBrushOrgEx function to set the brush origin. If it fails to do so, brush misalignment occurs.</para>
		/// <para>STRETCH_ANDSCANS - Same as BLACKONWHITE.</para>
		/// <para>STRETCH_DELETESCANS - Same as COLORONCOLOR.</para>
		/// <para>STRETCH_HALFTONE - Same as HALFTONE.</para>
		/// <para>STRETCH_ORSCANS - Same as WHITEONBLACK.</para>
		/// <para>WHITEONBLACK - Performs a Boolean OR operation using the color values for the eliminated and existing pixels. If the bitmap is a monochrome bitmap, this mode preserves white pixels at the expense of black pixels.</para>
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is the previous stretching mode. If the function fails, the return value is zero. 
		/// </returns>
		[DllImport("Gdi32.dll")]
		public static extern Int32 SetStretchBltMode(IntPtr hdc, Int32 iStretchMode);

		/// <summary>
		/// Copies a bitmap from a source rectangle into a destination rectangle, stretching or compressing the bitmap to fit the dimensions of the destination rectangle, if necessary. Windows stretches or compresses the bitmap according to the stretching mode currently set in the destination device context.
		/// </summary>
		/// <param name="hdcDest">Identifies the destination device context.</param>
		/// <param name="nXOriginDest">Specifies the x-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
		/// <param name="nYOriginDest">Specifies the y-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
		/// <param name="nWidthDest">Specifies the width, in logical units, of the destination rectangle.</param>
		/// <param name="nHeightDest">Specifies the height, in logical units, of the destination rectangle.</param>
		/// <param name="hdcSrc">Identifies the source device context.</param>
		/// <param name="nXOriginSrc">Specifies the x-coordinate, in logical units, of the upper-left corner of the source rectangle.</param>
		/// <param name="nYOriginSrc">Specifies the y-coordinate, in logical units, of the upper-left corner of the source rectangle.</param>
		/// <param name="nWidthSrc">Specifies the width, in logical units, of the source rectangle.</param>
		/// <param name="nHeightSrc">Specifies the height, in logical units, of the source rectangle.</param>
		/// <param name="dwRop">
		/// <para>Specifies a raster-operation code. These codes define how the color data for the source rectangle is to be combined with the color data for the destination rectangle to achieve the final color.</para>
		/// <para>BLACKNESS - Fills the destination rectangle using the color associated with index 0 in the physical palette. (This color is black for the default physical palette.)</para>
		/// <para>DSTINVERT - Inverts the destination rectangle.</para>
		/// <para>MERGECOPY - Merges the colors of the source rectangle with the specified pattern by using the Boolean AND operator.</para>
		/// <para>MERGEPAINT - Merges the colors of the inverted source rectangle with the colors of the destination rectangle by using the Boolean OR operator.</para>
		/// <para>NOTSRCCOPY - Copies the inverted source rectangle to the destination.</para>
		/// <para>NOTSRCERASE - Combines the colors of the source and destination rectangles by using the Boolean OR operator and then inverts the resultant color.</para>
		/// <para>PATCOPY - Copies the specified pattern into the destination bitmap.</para>
		/// <para>PATINVERT - Combines the colors of the specified pattern with the colors of the destination rectangle by using the Boolean XOR operator.</para>
		/// <para>PATPAINT - Combines the colors of the pattern with the colors of the inverted source rectangle by using the Boolean OR operator. The result of this operation is combined with the colors of the destination rectangle by using the Boolean OR operator.</para>
		/// <para>SRCAND - Combines the colors of the source and destination rectangles by using the Boolean AND operator.</para>
		/// <para>SRCCOPY - Copies the source rectangle directly to the destination rectangle.</para>
		/// <para>SRCERASE - Combines the inverted colors of the destination rectangle with the colors of the source rectangle by using the Boolean AND operator.</para>
		/// <para>SRCINVERT - Combines the colors of the source and destination rectangles by using the Boolean XOR operator.</para>
		/// <para>SRCPAINT - Combines the colors of the source and destination rectangles by using the Boolean OR operator.</para>
		/// <para>WHITENESS - Fills the destination rectangle using the color associated with index 1 in the physical palette. (This color is white for the default physical palette.)</para>
		/// </param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>. If the function fails, the return value is <see langword="false"/>. To get extended error information, call GetLastError.</returns>
		[DllImport("Gdi32.dll")]
		public static extern Boolean StretchBlt(
			IntPtr hdcDest,
			Int32 nXOriginDest,
			Int32 nYOriginDest,
			Int32 nWidthDest,
			Int32 nHeightDest,
			IntPtr hdcSrc,
			Int32 nXOriginSrc,
			Int32 nYOriginSrc,
			Int32 nWidthSrc,
			Int32 nHeightSrc,
			Int32 dwRop
		);
	}
}
