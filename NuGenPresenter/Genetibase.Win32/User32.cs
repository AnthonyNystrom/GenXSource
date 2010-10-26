/* -----------------------------------------------
 * User32.cs
 * Copyright © 2005-2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Imports User32.dll functions.
	/// </summary>
	public static class User32
	{
		private const String DLL = "User32.dll";

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
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean AdjustWindowRectEx(ref RECT rect, Int32 dwStyle, [MarshalAs(UnmanagedType.Bool)] Boolean bMenu, Int32 dwExStyle);

		/// <summary>
		/// Passes the hook information to the next hook procedure in the current hook chain.
		/// A hook procedure can call this function either before or after processing the hook information.
		/// </summary>
		/// <param name="hhk">Ignored.</param>
		/// <param name="nCode">Specifies the hook code passed to the current hook procedure. The next hook procedure uses this code to determine how to process the hook information.</param>
		/// <param name="wParam">Specifies the wParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain.</param>
		/// <param name="lParam">Specifies the lParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain.</param>
		/// <returns>This value is returned by the next hook procedure in the chain. The current hook procedure must also return this value. The meaning of the return value depends on the hook type. For more information, see the descriptions of the individual hook procedures.</returns>
		[DllImport(DLL, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr CallNextHookEx(NuGenHookHandle hhk, Int32 nCode, IntPtr wParam, IntPtr lParam);

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
		[DllImport(DLL)]
		[CLSCompliant(false)]
		public static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, POINT pt, uint flags);

        /// <summary>
        /// Converts the client-area coordinates of a specified point to screen coordinates. 
        /// </summary>
        /// <param name="hWnd">Handle to the window whose client area is used for the conversion.</param>
        /// <param name="point">Pointer to a <see cref="POINT"/> structure that contains the client coordinates to be converted. The new screen coordinates are copied into this structure if the function succeeds.</param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.
        /// If the function fails, the return value is <see langword="false"/>. 
        /// </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DLL)]
        public static extern Boolean ClientToScreen(IntPtr hWnd, ref POINT point);

		/// <summary>
		/// Destroys an icon and frees any memory the icon occupied.
		/// </summary>
		/// <param name="hIcon">Handle to the icon to be destroyed. The icon must not be in use. </param>
		/// <returns>
		/// If the function succeeds, the return value is <see langword="true"/>.
		/// If the function fails, the return value is <see langword="false"/>.
		/// To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean DestroyIcon(IntPtr hIcon);

		/// <summary>
		/// Fills a rectangle by using the specified brush. This function includes the left and top borders,
		/// but excludes the right and bottom borders of the rectangle.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="lprc">Pointer to a RECT structure that contains the logical coordinates of the rectangle to be filled.</param>
		/// <param name="hbr">Handle to the brush used to fill the rectangle.</param>
		/// <returns></returns>
		[DllImport(DLL)]
		public static extern Int32 FillRect(IntPtr hdc, [In] ref RECT lprc, IntPtr hbr);

		/// <summary>
		/// Fills a rectangle by using a brush of the specified color. This function includes the left and
		/// top borders, but excludes the right and bottom borders of the rectangle.
		/// </summary>
		/// <param name="hdc">Handle to the device context.</param>
		/// <param name="lprc">Pointer to a RECT structure that contains the logical coordinates of the rectangle to be filled.</param>
		/// <param name="crColor">Specifies the color to fill the rectangle with.</param>
		/// <returns></returns>
		public static Int32 FillSolidRect(IntPtr hdc, [In] ref RECT lprc, Color crColor)
		{
			return FillRect(hdc, ref lprc, Gdi32.CreateSolidBrush(crColor));
		}

		/// <summary>
		/// Returns the time required to invert the caret's pixels. The user can set this value. 
		/// </summary>
		[DllImport(DLL)]
		public static extern Int32 GetCaretBlinkTime();

		/// <summary>
		/// Copies the caret's position, in client coordinates, to the specified POINT structure.
		/// </summary>
		/// <param name="lpPoint">Points to the POINT structure that is to receive the client coordinates of the caret.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>.
		/// If the function fails, the return value is <see langword="false"/>.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean GetCaretPos(out POINT lpPoint);

		/// <summary>
		/// Retrieves a handle of a display device context (DC) for the client area of the specified window. 
		/// The display device context can be used in subsequent GDI functions to draw in the client area 
		/// of the window.
		/// </summary>
		/// <param name="hWnd">Identifies the window whose device context is to be retrieved.</param>
		/// <returns>If the function succeeds, the return value identifies the device context for the 
		/// given window's client area; otherwise, the return value is <c>null</c>.</returns>
		[DllImport(DLL)]
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
		[DllImport(DLL)]
		[CLSCompliant(false)]
		public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, uint flags);

		/// <summary>
		/// Returns a handle to the desktop window.
		/// </summary>
		[DllImport(DLL)]
		public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// Retrieves the active input locale identifier (formerly called the keyboard layout) for the specified thread. If the idThread parameter is zero, the input locale identifier for the active thread is returned.
        /// </summary>
        /// <param name="dwThread">Identifies the thread to query or is zero for the current thread.</param>
        /// <returns>The return value is the input locale identifier for the thread. The low word contains a Language Identifier for the input language and the high word contains a device handle to the physical layout of the keyboard.</returns>
        [DllImport(DLL)]
        public static extern IntPtr GetKeyboardLayout(Int32 dwThread);

		/// <summary>
		/// Copies the status of the 256 virtual keys to the specified buffer. 
		/// </summary>
		/// <param name="pbKeyState">Pointer to the 256-Byte array that receives the status data for each virtual key.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(DLL)]
		public static extern Int32 GetKeyboardState(Byte[] pbKeyState);

        /// <summary>
        /// Retrieves information about a display monitor. 
        /// </summary>
        /// <param name="hMonitor">Handle to the display monitor of interest.</param>
        /// <param name="lpmi">
        /// Pointer to a <see cref="MONITORINFO"/> or MONITORINFOEX structure that receives information about the specified display monitor. 
        /// You must set the cbSize member of the structure to sizeof(MONITORINFO) or sizeof(MONITORINFOEX) before calling the
        /// GetMonitorInfo function. Doing so lets the function determine the type of structure you are passing to it. 
        /// The MONITORINFOEX structure is a superset of the <see cref="MONITORINFO"/> structure. It has one additional member: a string
        /// that contains a name for the display monitor. Most applications have no use for a display monitor name, and so can save some
        /// bytes by using a <see cref="MONITORINFO"/> structure. 
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.
        /// If the function fails, the return value is <see langword="false"/>. 
        /// </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DLL)]
        public static extern Boolean GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

		/// <summary>
		/// Retrieves the handle of the specified child window's parent window. 
		/// </summary>
		/// <param name="hWnd">Identifies the window whose parent window handle is to be retrieved.</param>
		/// <returns>If the function succeeds, the return value is the handle of the parent window. If the window has no parent window, the return value is <c>IntPtr.Zero</c>. To get extended error information, call GetLastError.</returns>
		[DllImport(DLL)]
		public static extern IntPtr GetParent(IntPtr hWnd);

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
		[DllImport(DLL)]
		public static extern Int32 GetScrollPos(IntPtr hWnd, Int32 nBar);

		/// <summary>
		/// Retrieves various system metrics (widths and heights of display elements) and system configuration settings. All dimensions retrieved by GetSystemMetrics are in pixels.
		/// </summary>
		/// <param name="nIndex">
		/// <para>System metric or configuration setting to retrieve.</para>
		/// <para>See MSDN for more info.</para>
		/// </param>
		/// <returns></returns>
		[DllImport(DLL)]
		public static extern Int32 GetSystemMetrics(Int32 nIndex);

		/// <summary>
		/// Retrieves the device context (DC) for the entire window, including title bar, menus, and scroll
		/// bars. A window device context permits painting anywhere in a window, because the origin of the
		/// device context is the upper-left corner of the window instead of the client area.
		/// </summary>
		/// <param name="hWnd">Identifies the window with a device context that is to be retrieved.</param>
		/// <returns>If the function succeeds, the return value is the handle of a device context for the
		/// specified window; otherwise, the return value is <c>IntPtr.Zero</c>, indicating an error or an
		/// invalid hWnd parameter.</returns>
		[DllImport(DLL)]
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
		[DllImport(DLL)]
		public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

		/// <summary>
		/// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions 
		/// are given in screen coordinates that are relative to the upper-left corner of the screen.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="lpRect">Pointer to a structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>. 
		/// To get extended error information, call GetLastError.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean GetWindowRect(IntPtr hWnd, ref RECT lpRect);

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
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean InvalidateRect(IntPtr hWnd, ref RECT lpRect, [MarshalAs(UnmanagedType.Bool)] Boolean bErase);

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
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean InvalidateRect(IntPtr hWnd, IntPtr lpRect, [MarshalAs(UnmanagedType.Bool)] Boolean bErase);

		/// <summary>
		/// Determines whether the specified window handle identifies an existing window.
		/// </summary>
		/// <param name="hWnd">Handle to the window to test.</param>
		/// <returns>If the window handle identifies an existing window, the return value is <c>true</c>;
		/// otherwise, <c>false</c>.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean IsWindow(IntPtr hWnd);

		/// <summary>
		/// Changes the position and dimensions of the specified window. For a top-level window, the 
		/// position and dimensions are relative to the upper-left corner of the screen. For a child 
		/// window, they are relative to the upper-left corner of the parent window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="x">Specifies the new position of the left side of the window.</param>
		/// <param name="y">Specifies the new position of the top of the window.</param>
		/// <param name="nWidth">Specifies the new width of the window.</param>
		/// <param name="nHeight">Specifies the new height of the window.</param>
		/// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is 
		/// <c>true</c>, the window receives a message; otherwise, no repainting 
		/// of any kind occurs. This applies to the client area, the nonclient area (including the title 
		/// bar and scroll bars), and any part of the parent window uncovered as a result of moving a 
		/// child window.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>. 
		/// To get extended error information, call GetLastError.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean MoveWindow(IntPtr hWnd, Int32 x, Int32 y, Int32 nWidth, Int32 nHeight, [MarshalAs(UnmanagedType.Bool)] Boolean bRepaint);

		/// <summary>
		/// Updates the specified rectangle or region in a window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be redrawn. If this parameter is <c>IntPtr.Zero</c>, the desktop window is updated.</param>
		/// <param name="lprcUpdate">Pointer to a <see cref="RECT"/> structure containing the coordinates, in device units, of the update rectangle. This parameter is ignored if the hrgnUpdate parameter identifies a region.</param>
		/// <param name="hrgnUpdate">Handle to the update region. If both the <paramref name="hrgnUpdate"/> and <paramref name="lprcUpdate"/> parameters are <c>IntPtr.Zero</c>, the entire client area is added to the update region.</param>
		/// <param name="flags">Specifies one or more redraw flags. This parameter can be used to invalidate or validate a window, control repainting, and control which windows are affected by RedrawWindow. See MSDN for more info.</param>
		/// <returns>
		/// If the function succeeds, the return value is <see langword="true"/>. If the function fails, the return value is <see langword="false"/>. 
		/// </returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, Int32 flags);

		/// <summary>
		/// Updates the specified rectangle or region in a window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window to be redrawn. If this parameter is <c>IntPtr.Zero</c>, the desktop window is updated.</param>
		/// <param name="lprcUpdate">Pointer to a <see cref="RECT"/> structure containing the coordinates, in device units, of the update rectangle. This parameter is ignored if the hrgnUpdate parameter identifies a region.</param>
		/// <param name="hrgnUpdate">Handle to the update region. If both the <paramref name="hrgnUpdate"/> and <paramref name="lprcUpdate"/> parameters are <c>IntPtr.Zero</c>, the entire client area is added to the update region.</param>
		/// <param name="flags">Specifies one or more redraw flags. This parameter can be used to invalidate or validate a window, control repainting, and control which windows are affected by RedrawWindow. See MSDN for more info.</param>
		/// <returns>
		/// If the function succeeds, the return value is <see langword="true"/>. If the function fails, the return value is <see langword="false"/>. 
		/// </returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean RedrawWindow(IntPtr hWnd, ref RECT lprcUpdate, IntPtr hrgnUpdate, Int32 flags);

		/// <summary>
		/// Releases the mouse capture from a window in the current thread and restores normal 
		/// mouse input processing. A window that has captured the mouse receives all mouse input, 
		/// regardless of the position of the cursor, except when a mouse button is clicked while the 
		/// cursor hot spot is in the window of another thread.
		/// </summary>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, <c>false</c>. 
		/// To get extended error information, call <c>GetLastError</c>.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean ReleaseCapture();

		/// <summary>
		/// Releases a device context (DC), freeing it for use by other applications. The effect of the ReleaseDC function depends on the type of DC. It frees only common and window DCs. It has no effect on class or private DCs.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose DC is to be released.</param>
		/// <param name="hDC">Handle to the DC to be released.</param>
		/// <returns>The return value indicates whether the DC was released. If the DC was released, the return value is 1. If the DC was not released, the return value is zero.</returns>
		[DllImport(DLL)]
		public static extern Int32 ReleaseDC(IntPtr hWnd, IntPtr hDC);

		/// <summary>
		/// Converts the screen coordinates of a specified point on the screen to client-area coordinates.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose client area will be used for the conversion.</param>
		/// <param name="lpPoint">Pointer to a <see cref="POINT"/> structure that specifies the screen coordinates to be converted.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

		/// <summary>
		/// Converts the screen coordinates of a specified point on the screen to client-area coordinates.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose client area will be used for the conversion.</param>
		/// <param name="lpPoint">Pointer to a <see cref="POINT"/> structure that specifies the screen coordinates to be converted.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean ScreenToClient(IntPtr hWnd, ref Point lpPoint);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message. 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific information.</param>
		/// <param name="lParam">Specifies additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(DLL)]
		public static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message. 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific information.</param>
		/// <param name="lParam">Specifies TVITEM structure that contains additional message information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(DLL)]
		[CLSCompliant(false)]
		public static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, IntPtr wParam, ref TVITEM lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message. 
		/// </summary>
		/// <param name="hWnd">Handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">Specifies the message to be sent.</param>
		/// <param name="wParam">Specifies additional message-specific information.</param>
		/// <param name="lParam">Specifies RECT structure that contains additional message information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(DLL)]
		public static extern IntPtr SendMessage(IntPtr hWnd, Int32 msg, IntPtr wParam, ref RECT lParam);

		/// <summary>
		/// Changes the parent window of the specified child window.
		/// </summary>
		/// <param name="hWndChild">Handle to the child window.</param>
		/// <param name="hWndNewParent">Handle to the new parent window. If this parameter is <c>null</c>, 
		/// the desktop window becomes the new parent window. Windows 2000/XP: If this parameter is 
		/// HWND_MESSAGE, the child window becomes a message-only window.</param>
		/// <returns>If the function succeeds, the return value is a handle to the previous parent window;
		/// otherwise, <c>null</c>. To get extended error information, call GetLastError.</returns>
		[DllImport(DLL)]
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
		[DllImport(DLL)]
		public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

		/// <summary>
		/// Changes the size, position, and Z order of a child, pop-up, or top-level window. 
		/// Child, pop-up, and top-level windows are ordered according to their appearance on the screen. 
		/// The topmost window receives the highest rank and is the first window in the Z order.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="hWndInsertAfter">Handle to the window to precede the positioned window in the Z order.</param>
		/// <param name="x">Specifies the new position of the left side of the window, in client coordinates.</param>
		/// <param name="y">Specifies the new position of the top of the window, in client coordinates.</param>
		/// <param name="cx">Specifies the new width of the window, in pixels.</param>
		/// <param name="cy">Specifies the new height of the window, in pixels.</param>
		/// <param name="uFlags">Specifies the window sizing and positioning flags.</param>
		/// <returns>If the function succeeds, the return value is <c>true</c>; otherwise, the return 
		/// value is <c>false</c>. To get extended error information, call GetLastError.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[CLSCompliant(false)]
		public static extern Boolean SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Int32 x, Int32 y, Int32 cx, Int32 cy, uint uFlags);

		/// <summary>
		/// Installs an application-defined hook procedure into a hook chain. You would install a hook
		/// procedure to monitor the system for certain types of events. These events are associated
		/// either with a specific thread or with all threads in the same desktop as the calling thread.
		/// </summary>
		/// <param name="idHook">
		/// <para>Specifies the type of hook procedure to be installed.</para>
		/// <para>See MSDN for more info.</para>
		/// </param>
		/// <param name="lpfn">Pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a thread created by a different process, the lpfn parameter must point to a hook procedure in a dynamic-link library (DLL). Otherwise, lpfn can point to a hook procedure in the code associated with the current process.</param>
		/// <param name="hInstance">Handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by the current process and if the hook procedure is within the code associated with the current process.</param>
		/// <param name="threadId">Specifies the identifier of the thread with which the hook procedure is to be associated. If this parameter is zero, the hook procedure is associated with all existing threads running in the same desktop as the calling thread.</param>
		/// <returns>
		/// If the function succeeds, the return value is the handle to the hook procedure. If the function
		/// fails, the return value is <c>IntPtr.Zero</c>. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(DLL, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr SetWindowsHookEx(Int32 idHook, HOOKPROC lpfn, IntPtr hInstance, Int32 threadId);

		/// <summary>
		/// Shows or hides the specified scroll bar.
		/// </summary>
		/// <param name="hWnd">
		/// Identifies a scroll bar control or a window with a standard scroll bar, depending on the value of the wBar parameter.
		/// </param>
		/// <param name="wBar">
		/// <para>Specifies the scroll bar(s) to be shown or hidden. This parameter can be one of the following values:</para> 
		/// <para>SB_BOTH - Shows or hides a window's standard horizontal and vertical scroll bars.</para>
		/// <para>SB_CTL - Shows or hides a scroll bar control. The hWnd parameter must be the handle of the scroll bar control.</para>
		/// <para>SB_HORZ - Shows or hides a window's standard horizontal scroll bars.</para>
		/// <para>SB_VERT - Shows or hides a window's standard vertical scroll bar.</para>
		/// </param>
		/// <param name="bShow">
		/// Specifies whether the scroll bar is shown or hidden. If this parameter is <see langword="true"/>, the scroll bar is shown; otherwise, it is hidden.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is <see langword="true"/>.
		/// If the function fails, the return value is <see langword="false"/>. To get extended error information, call GetLastError. 
		/// </returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean ShowScrollBar(IntPtr hWnd, Int32 wBar, Boolean bShow);

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
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

		/// <summary>
		/// Translates the specified virtual-key code and keyboard state to the corresponding character or
		/// characters. The function translates the code using the input language and physical keyboard
		/// layout identified by the keyboard layout handle.
		/// </summary>
		/// <param name="uVirtKey">Specifies the virtual-key code to be translated.</param>
		/// <param name="uScanCode">Specifies the hardware scan code of the key to be translated. The high-order bit of this value is set if the key is up (not pressed).</param>
		/// <param name="lpbKeyState">Pointer to a 256-Byte array that contains the current keyboard state. Each element (Byte) in the array contains the state of one key. If the high-order bit of a Byte is set, the key is down (pressed). The low bit, if set, indicates that the key is toggled on. In this function, only the toggle bit of the CAPS LOCK key is relevant. The toggle state of the NUM LOCK and SCROLL LOCK keys is ignored.</param>
		/// <param name="lpwTransKey">Pointer to the buffer that receives the translated character or characters.</param>
		/// <param name="fuState">Specifies whether a menu is active.</param>
		/// <returns>
		/// <para>If the specified key is a dead key, the return value is negative. Otherwise, it is one of the following values.</para>
		/// <para>0 - The specified virtual key has no translation for the current state of the keyboard.</para>
		/// <para>1 - One character was copied to the buffer.</para> 
		/// <para>2 - Two characters were copied to the buffer. This usually happens when a dead-key character (accent or diacritic) stored in the keyboard layout cannot be composed with the specified virtual key to form a single character.</para>
		/// </returns>
		[DllImport(DLL)]
		public static extern Int32 ToAscii(Int32 uVirtKey, Int32 uScanCode, Byte[] lpbKeyState, Byte[] lpwTransKey, Boolean fuState);

		/// <summary>
		/// Posts messages when the mouse pointer leaves a window or hovers over a window for a specified amount of time.
		/// </summary>
		/// <param name="eventTrack"></param>
		/// <returns></returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[CLSCompliant(false)]
		public static extern Boolean TrackMouseEvent(ref TRACKMOUSEEVENT eventTrack);

		/// <summary>
		/// Removes a hook procedure installed in a hook chain by the <see cref="SetWindowsHookEx"/> function.
		/// </summary>
		/// <param name="hhk">Handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx.</param>
		/// <returns>
		/// If the function succeeds, the return value is <see langword="true"/>.
		/// If the function fails, the return value is <see langword="false"/>.
		/// To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(DLL, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean UnhookWindowsHookEx(IntPtr hhk);

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
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean UpdateLayeredWindow(
			IntPtr hWnd,
			IntPtr hdcDst,
			ref POINT pptDst,
			ref SIZE psize,
			IntPtr hdcSrc,
			ref POINT pptSrc,
			Int32 crKey,
			ref BLENDFUNCTION pblend,
			Int32 dwFlags
			);

		/// <summary>
		/// Retrieves the handle of the window that contains the specified <see cref="T:POINT"/>.
		/// </summary>
		/// <param name="p">Specifies the point to be checked.</param>
		/// <returns></returns>
		[DllImport(DLL)]
		public static extern IntPtr WindowFromPoint(POINT p);
	}
}
