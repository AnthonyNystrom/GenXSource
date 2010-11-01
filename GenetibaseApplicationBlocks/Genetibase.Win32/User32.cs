/* -----------------------------------------------
 * User32.cs
 * Copyright © 2005-2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Imports User32.dll functions.
	/// </summary>
	public static class User32
	{
		/// <summary>
		/// Calculates the required size of the window rectangle, based on the desired size of the client rectangle.
		/// </summary>
		/// <param name="rect">Pointer to a <see cref="T:Genetibase.WinApi.RECT"/> structure that contains the coordinates of the top-left and
		/// bottom-right corners of the desired client area. When the function returns, the structure contains
		/// the coordinates of the top-left and bottom-right corners of the window to accommodate the desired
		/// client area.</param>
		/// <param name="dwStyle">Specifies the window style of the window whose required size is to be calculated. Note that you cannot specify the WS_OVERLAPPED style.</param>
		/// <param name="bMenu">Specifies whether the window has a menu.</param>
		/// <param name="dwExStyle">Specifies the extended window style of the window whose required size is to be calculated.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport("User32.dll")]
		public static extern bool AdjustWindowRectEx(ref RECT rect, int dwStyle, bool bMenu, int dwExStyle);

		/// <summary>
		/// Determines which, if any, of the child windows belonging to the specified parent window
		/// contains the specified point. The function can ignore invisible, disabled, and transparent
		/// child windows.
		/// </summary>
		/// <param name="hwndParent">Identifies the parent window.</param>
		/// <param name="pt">Specifies a POINT structure that defines the client coordinates of the point to be checked.</param>
		/// <param name="flags">Specifies which child windows to skip.</param>
		/// <returns>If the function succeeds, the return value is the handle to the first child window that
		/// contains the point and meets the criteria specified by uFlags. If the point is within the parent
		/// window but not within any child window that meets the criteria, the return value is the handle to
		/// the parent window. If the point lies outside the parent window or if the function fails, the
		/// return value is <c>IntPtr.Zero</c>.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, POINT pt, uint flags);

		/// <summary>
		/// Fills a rectangle by using the specified brush. This function includes the left and top borders,
		/// but excludes the right and bottom borders of the rectangle.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="lprc">Pointer to a RECT structure that contains the logical coordinates of the rectangle to be filled.</param>
		/// <param name="hbr">Handle to the brush used to fill the rectangle.</param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern int FillRect(IntPtr hdc, [In] ref RECT lprc, IntPtr hbr);

		/// <summary>
		/// Fills a rectangle by using a brush of the specified color. This function includes the left and
		/// top borders, but excludes the right and bottom borders of the rectangle.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="lprc">Pointer to a RECT structure that contains the logical coordinates of the rectangle to be filled.</param>
		/// <param name="crColor">Specifies the color to fill the rectangle with.</param>
		/// <returns></returns>
		public static int FillSolidRect(IntPtr hdc, [In] ref RECT lprc, Color crColor)
		{
			return FillRect(hdc, ref lprc, Gdi32.CreateSolidBrush(crColor));
		}

		/// <summary>
		/// Copies the caret's position, in client coordinates, to the specified POINT structure.
		/// </summary>
		/// <param name="lpPoint">Points to the POINT structure that is to receive the client coordinates of the caret.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>.
		/// If the function fails, the return value is <see langword="false"/>.</returns>
		[DllImport("User32.dll")]
		public static extern bool GetCaretPos(out POINT lpPoint);

		/// <summary>
		/// Retrieves a handle of a display device context (DC) for the client area of the specified window. 
		/// The display device context can be used in subsequent GDI functions to draw in the client area 
		/// of the window.
		/// </summary>
		/// <param name="hWnd">Identifies the window whose device context is to be retrieved.</param>
		/// <returns>If the function succeeds, the return value identifies the device context for the 
		/// given window's client area; otherwise, the return value is <c>null</c>.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);

		/// <summary>
		/// Retrieves the handle of a display device (DC) context for the specified window. The display 
		/// device context can be used in subsequent GDI functions to draw in the client area.
		/// </summary>
		/// <param name="hWnd">Identifies the window where drawing will occur.</param>
		/// <param name="hrgnClip">Specifies a clipping region that may be combined with the visible region 
		/// of the client window.</param>
		/// <param name="flags">Specifies how the device context is created.</param>
		/// <returns>If the function succeeds, the return value is the handle of the device context for the 
		/// given window; otherwise, the return value is <c>IntPtr.Zero</c>. An invalid value for the 
		/// hWnd parameter will cause the function to fail.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, uint flags);

		/// <summary>
		/// Retrieves the current position of the scroll box (thumb) in the specified scroll bar.
		/// The current position is a relative value that depends on the current scrolling range.
		/// For example, if the scrolling range is 0 through 100 and the scroll box is in the middle 
		/// of the bar, the current position is 50.
		/// </summary>
		/// <param name="hWnd">Handle to a scroll bar control or a window with a standard scroll bar,
		/// depending on the value of the nBar parameter.</param>
		/// <param name="nBar">Specifies the scroll bar to be examined.</param>
		/// <returns>If the function succeeds, the return value is the current position of the scroll box;
		/// otherwise, the return value is zero.</returns>
		[DllImport("User32.dll")]
		public static extern int GetScrollPos(IntPtr hWnd, int nBar);

		/// <summary>
		/// Retrieves the device context (DC) for the entire window, including title bar, menus, and scroll
		/// bars. A window device context permits painting anywhere in a window, because the origin of the
		/// device context is the upper-left corner of the window instead of the client area.
		/// </summary>
		/// <param name="hWnd">Identifies the window with a device context that is to be retrieved.</param>
		/// <returns>If the function succeeds, the return value is the handle of a device context for the
		/// specified window; otherwise, the return value is <c>IntPtr.Zero</c>, indicating an error or an
		/// invalid hWnd parameter.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);

		/// <summary>
		/// Retrieves information about the specified window. The function also retrieves the 32-bit (long) value at the specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="nIndex">Specifies the zero-based offset to the value to be retrieved. Valid values 
		/// are in the range zero through the number of bytes of extra window memory, minus four; for 
		/// example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to 
		/// the third 32-bit integer.</param>
		/// <returns>If the function succeeds, the return value is the requested 32-bit value. Otherwise, 
		/// the return value is zero. To get extended error information, call GetLastError. If SetWindowLong 
		/// has not been called previously, GetWindowLong returns zero for values in the extra window or 
		/// class memory.</returns>
		[DllImport("User32.dll")]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		/// <summary>
		/// Calls <see cref="M:Genetibase.WinApi.User32.GetWindowLong"/> or <see cref="M:Genetibase.WinApi.User32.GetWindowLongPtr"/>
		/// depending on the <see cref="P:System.IntPtr.Size"/>.
		/// </summary>
		/// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="nIndex">Specifies the zero-based offset to the value to be retrieved. Valid values
		/// are in the range zero through the number of bytes of extra window memory, minus the size of an
		/// integer.</param>
		/// <returns>If the function succeeds, the return value is the requested value. Otherwise, 
		/// the return value is zero. To get extended error information, call GetLastError. If SetWindowLong 
		/// has not been called previously, GetWindowLong returns zero for values in the extra window or 
		/// class memory.</returns>
		public static IntPtr GetWindowLongEx(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 8)
			{
				return User32.GetWindowLongPtr(hWnd, nIndex);
			}

			return new IntPtr(User32.GetWindowLong(hWnd, nIndex));
		}

		/// <summary>
		/// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.
		/// If you are retrieving a pointer or a handle, this function supersedes the GetWindowLong function.
		/// (Pointers and handles are 32 bits on 32-bit Windows and 64 bits on 64-bit Windows.) To write code
		/// that is compatible with both 32-bit and 64-bit versions of Windows, use GetWindowLongPtr.</param>
		/// <param name="nIndex">Specifies the zero-based offset to the value to be retrieved. Valid values
		/// are in the range zero through the number of bytes of extra window memory, minus the size of an
		/// integer.</param>
		/// <returns>If the function succeeds, the return value is the requested value. Otherwise, 
		/// the return value is zero. To get extended error information, call GetLastError. If SetWindowLong 
		/// has not been called previously, GetWindowLong returns zero for values in the extra window or 
		/// class memory.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

		/// <summary>
		/// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions 
		/// are given in screen coordinates that are relative to the upper-left corner of the screen.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="lpRect">Pointer to a structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>. 
		/// To get extended error information, call GetLastError.</returns>
		[DllImport("User32.dll")]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

		/// <summary>
		/// Adds a rectangle to the specified window's update region. The update region 
		/// represents the portion of the window's client area that must be redrawn. 
		/// </summary>
		/// <param name="hWnd">
		/// Identifies the window whose update region has changed. 
		/// If this parameter is <c>null</c>, Windows invalidates and redraws all windows, 
		/// and sends the WM_ERASEBKGND and WM_NCPAINT messages to the window procedure 
		/// before the function returns.
		/// </param>
		/// <param name="lpRect">
		/// Points to a RECT structure that contains the client coordinates 
		/// of the rectangle to be added to the update region. If this parameter is <c>null</c>,
		/// the entire client area is added to the update region.
		/// </param>
		/// <param name="bErase">
		/// Specifies whether the background within the update region is to 
		/// be erased when the update region is processed. If this parameter is <c>true</c>, 
		/// the background is erased when the BeginPaint function is called. If this parameter 
		/// is <c>false</c>, the background remains unchanged.
		/// </param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>.</returns>
		[DllImport("User32.dll")]
		public static extern bool InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);

		/// <summary>
		/// Adds a rectangle to the specified window's update region. The update region 
		/// represents the portion of the window's client area that must be redrawn. 
		/// </summary>
		/// <param name="hWnd">
		/// Identifies the window whose update region has changed. 
		/// If this parameter is <c>null</c>, Windows invalidates and redraws all windows, 
		/// and sends the WM_ERASEBKGND and WM_NCPAINT messages to the window procedure 
		/// before the function returns.
		/// </param>
		/// <param name="lpRect">
		/// Points to a RECT structure that contains the client coordinates 
		/// of the rectangle to be added to the update region. If this parameter is <c>null</c>,
		/// the entire client area is added to the update region.
		/// </param>
		/// <param name="bErase">
		/// Specifies whether the background within the update region is to 
		/// be erased when the update region is processed. If this parameter is <c>true</c>, 
		/// the background is erased when the BeginPaint function is called. If this parameter 
		/// is <c>false</c>, the background remains unchanged.
		/// </param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>.</returns>
		[DllImport("User32.dll")]
		public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

		/// <summary>
		/// Determines whether the specified window handle identifies an existing window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to test.</param>
		/// <returns>If the window handle identifies an existing window, the return value is <c>true</c>;
		/// otherwise, <c>false</c>.</returns>
		[DllImport("User32.dll")]
		public static extern bool IsWindow(IntPtr hWnd);

		/// <summary>
		/// Changes the position and dimensions of the specified window. For a top-level window, the 
		/// position and dimensions are relative to the upper-left corner of the screen. For a child 
		/// window, they are relative to the upper-left corner of the parent window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="X">Specifies the new position of the left side of the window.</param>
		/// <param name="Y">Specifies the new position of the top of the window.</param>
		/// <param name="nWidth">Specifies the new width of the window.</param>
		/// <param name="nHeight">Specifies the new height of the window.</param>
		/// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is 
		/// <c>true</c>, the window receives a message; otherwise, no repainting 
		/// of any kind occurs. This applies to the client area, the nonclient area (including the title 
		/// bar and scroll bars), and any part of the parent window uncovered as a result of moving a 
		/// child window.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>. 
		/// To get extended error information, call GetLastError.</returns>
		[DllImport("User32.dll")]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		/// <summary>
		/// Releases the mouse capture from a window in the current thread and restores normal 
		/// mouse input processing. A window that has captured the mouse receives all mouse input, 
		/// regardless of the position of the cursor, except when a mouse button is clicked while the 
		/// cursor hot spot is in the window of another thread.
		/// </summary>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>. 
		/// To get extended error information, call <c>GetLastError</c>.</returns>
		[DllImport("User32.dll")]
		public static extern bool ReleaseCapture();

		/// <summary>
		/// Releases a device context (DC), freeing it for use by other applications. The effect of the ReleaseDC function depends on the type of DC. It frees only common and window DCs. It has no effect on class or private DCs.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose DC is to be released.</param>
		/// <param name="hDC">Handle to the DC to be released.</param>
		/// <returns>The return value indicates whether the DC was released. If the DC was released, the return value is 1. If the DC was not released, the return value is zero.</returns>
		[DllImport("User32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		/// <summary>
		/// Converts the screen coordinates of a specified point on the screen to client-area coordinates.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose client area will be used for the conversion.</param>
		/// <param name="lpPoint">Pointer to a <see cref="POINT"/> structure that specifies the screen coordinates to be converted.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport("User32.dll")]
		public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

		/// <summary>
		/// Converts the screen coordinates of a specified point on the screen to client-area coordinates.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose client area will be used for the conversion.</param>
		/// <param name="lpPoint">Pointer to a <see cref="POINT"/> structure that specifies the screen coordinates to be converted.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport("User32.dll")]
		public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message. 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific information.</param>
		/// <param name="lParam">Specifies additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message. 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific information.</param>
		/// <param name="lParam">Specifies TVITEM structure that contains additional message information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message. 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific information.</param>
		/// <param name="lParam">Specifies RECT structure that contains additional message information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref RECT lParam);

		/// <summary>
		/// Changes the parent window of the specified child window.
		/// </summary>
		/// <param name="hWndChild">Handle to the child window.</param>
		/// <param name="hWndNewParent">Handle to the new parent window. If this parameter is <c>null</c>, 
		/// the desktop window becomes the new parent window. Windows 2000/XP: If this parameter is 
		/// HWND_MESSAGE, the child window becomes a message-only window.</param>
		/// <returns>If the function succeeds, the return value is a handle to the previous parent window;
		/// otherwise, <c>null</c>. To get extended error information, call GetLastError.</returns>
		[DllImport("User32.dll")]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		/// <summary>
		/// Changes an attribute of the specified window. The function also sets the 32-bit (long) value 
		/// at the specified offset into the extra window memory.
		/// </summary>
		/// <remarks>This function has been superseded by the SetWindowLongPtr function. To write code
		/// that is compatible with both 32-bit and 64-bit versions of Microsoft Windows, use the 
		/// SetWindowLongPtr function.</remarks>
		/// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.
		/// Windows 95/98/Me: The SetWindowLong function may fail if the window specified by the hWnd parameter 
		/// does not belong to the same process as the calling thread.</param>
		/// <param name="nIndex">Specifies the zero-based offset to the value to be set. Valid values are 
		/// in the range zero through the number of bytes of extra window memory, minus the size of an 
		/// integer.</param>
		/// <param name="dwNewLong">Specifies the replacement value.</param>
		/// <returns>If the function succeeds, the return value is the previous value of the specified 
		/// 32-bit integer; otherwise, the return value is zero. To get extended error information, 
		/// call GetLastError.</returns>
		[DllImport("User32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		/// <summary>
		/// Changes the size, position, and Z order of a child, pop-up, or top-level window. 
		/// Child, pop-up, and top-level windows are ordered according to their appearance on the screen. 
		/// The topmost window receives the highest rank and is the first window in the Z order.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="hWndInsertAfter">Handle to the window to precede the positioned window in the Z order.</param>
		/// <param name="X">Specifies the new position of the left side of the window, in client coordinates.</param>
		/// <param name="Y">Specifies the new position of the top of the window, in client coordinates.</param>
		/// <param name="cx">Specifies the new width of the window, in pixels.</param>
		/// <param name="cy">Specifies the new height of the window, in pixels.</param>
		/// <param name="uFlags">Specifies the window sizing and positioning flags.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, the return 
		/// value is <c>false</c>. To get extended error information, call GetLastError.</returns>
		[DllImport("User32.dll")]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		/// <summary>
		/// Sets the specified window's show state.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="nCmdShow">Specifies how the window is to be shown. This parameter is ignored 
		/// the first time an application calls ShowWindow, if the program that launched the application 
		/// provides a STARTUPINFO structure. Otherwise, the first time ShowWindow is called, the value 
		/// should be the value obtained by the WinMain function in its nCmdShow parameter.</param>
		/// <returns>If the window was previously visible, the return value is <c>true</c>.
		/// If the window was previously hidden, the return value is <c>false</c>.</returns>
		[DllImport("User32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		/// <summary>
		/// Posts messages when the mouse pointer leaves a window or hovers over a window for a specified amount of time.
		/// </summary>
		/// <param name="eventTrack"></param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT eventTrack);

		/// <summary>
		/// Updates the position, size, shape, content, and translucency of a layered window.
		/// </summary>
		/// <param name="hWnd">Handle to a layered window. A layered window is created by specifying 
		/// WS_EX_LAYERED when creating the window with the CreateWindowEx function.</param>
		/// <param name="hdcDst">Handle to a device context (DC) for the screen. This handle is obtained 
		/// by specifying <c>IntPtr.Zero</c> when calling the function. It is used for palette color matching 
		/// when the window contents are updated. If hdcDst is <c>IntPtr.Zero</c>, the default palette will 
		/// be used. If hdcSrc is <c>IntPtr.Zero</c>, hdcDst must be <c>IntPtr.Zero</c>.</param>
		/// <param name="pptDst">Pointer to a POINT structure that specifies the new screen position of the 
		/// layered window. If the current position is not changing, pptDst can be <c>null</c>.</param>
		/// <param name="psize">Pointer to a SIZE structure that specifies the new size of the layered 
		/// window. If the size of the window is not changing, psize can be <c>null</c>. If hdcSrc is 
		/// <c>null</c>, psize must be <c>null</c>.</param>
		/// <param name="hdcSrc">Handle to a DC for the surface that defines the layered window. This handle 
		/// can be obtained by calling the CreateCompatibleDC function. If the shape and visual context of 
		/// the window are not changing, hdcSrc can be <c>null</c>.</param>
		/// <param name="pptSrc">Pointer to a POINT structure that specifies the location of the layer in 
		/// the device context. If hdcSrc is <c>null</c>, pptSrc should be <c>null</c>.</param>
		/// <param name="crKey">Specifies the color key to be used when composing the layered window.</param>
		/// <param name="pblend">Pointer to a BLENDFUNCTION structure that specifies the transparency value to be used when composing the layered window.</param>
		/// <param name="dwFlags">If hdcSrc is <c>null</c>, dwFlags should be zero.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, the return 
		/// value is <c>false</c>. To get extended error information, call GetLastError.</returns>
		[DllImport("User32.dll")]
		public static extern bool UpdateLayeredWindow(
			IntPtr hWnd,
			IntPtr hdcDst,
			ref POINT pptDst,
			ref SIZE psize,
			IntPtr hdcSrc,
			ref POINT pptSrc,
			int crKey,
			ref BLENDFUNCTION pblend,
			int dwFlags
			);

		/// <summary>
		/// Retrieves the handle of the window that contains the specified <see cref="T:POINT"/>.
		/// </summary>
		/// <param name="p">Specifies the point to be checked.</param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern IntPtr WindowFromPoint(POINT p);
	}
}
