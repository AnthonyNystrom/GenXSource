/* -----------------------------------------------
 * WinUser.cs
 * Copyright © 2005-2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System.ComponentModel;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines constants declared in WinUser.h.
	/// </summary>
	public static class WinUser
	{
		#region Button Control Styles

		/// <summary>
		/// Creates an owner-drawn button. The owner window receives a WM_MEASUREITEM message when the button is created and a WM_DRAWITEM message when a visual aspect of the button has changed. Do not combine the BS_OWNERDRAW style with any other button styles.
		/// </summary>
		public const int BS_OWNERDRAW = 0x0000000B;

		#endregion

		#region Class styles

		/// <summary>
		/// The CS_VREDRAW style causes a window to be completely redrawn whenever its vertical size (height) 
		/// changes.
		/// </summary>
		public const int CS_VREDRAW = 0x0001;

		/// <summary>
		/// The CS_HREDRAW style causes a window to be completely redrawn whenever its horizontal size (width) 
		/// changes.
		/// </summary>
		public const int CS_HREDRAW = 0x0002;

		/// <summary>
		/// The CS_DBLCLKS style causes Windows to detect a double-click (the user clicking the left mouse 
		/// button twice in quick succession) for an application.
		/// </summary>
		public const int CS_DBLCLKS = 0x0008;

		/// <summary>
		/// Allocates a unique device context for each window in the class.
		/// </summary>
		public const int CS_OWNDC = 0x0020;

		/// <summary>
		/// Allocates one device context to be shared by all windows in the class. Because window classes 
		/// are process specific, it is possible for multiple threads of an application to create a window 
		/// of the same class. It is also possible for the threads to attempt to use the device context 
		/// simultaneously. When this happens, the system allows only one thread to successfully finish its 
		/// drawing operation.
		/// </summary>
		public const int CS_CLASSDC = 0x0040;

		/// <summary>
		/// Sets the clipping rectangle of the child window to that of the parent window so that the child 
		/// can draw on the parent. A window with the CS_PARENTDC style bit receives a regular device 
		/// context from the system's cache of device contexts. It does not give the child the parent's 
		/// device context or device context settings. Specifying CS_PARENTDC enhances an application's 
		/// performance.
		/// </summary>
		public const int CS_PARENTDC = 0x0080;

		/// <summary>
		/// A window that belongs to a class with the CS_NOCLOSE style does not have the Close command in its 
		/// System menu.
		/// </summary>
		public const int CS_NOCLOSE = 0x0200;

		/// <summary>
		/// Menus, dialog boxes, and combo list boxes have the CS_SAVEBITS style. When you use this style for 
		/// a window, Windows saves a bitmap copy of the screen image that the window obscures.
		/// </summary>
		public const int CS_SAVEBITS = 0x0800;

		/// <summary>
		/// Aligns the window's client area on a byte boundary (in the x direction).
		/// This style affects the width of the window and its horizontal placement on the display.
		/// </summary>
		public const int CS_BYTEALIGNCLIENT = 0x1000;

		/// <summary>
		/// Aligns the window on a byte boundary (in the x direction). This style affects the width 
		/// of the window and its horizontal placement on the display.
		/// </summary>
		public const int CS_BYTEALIGNWINDOW = 0x2000;

		/// <summary>
		/// Specifies that the window class is an application global class.
		/// </summary>
		public const int CS_GLOBALCLASS = 0x4000;

		/// <summary>
		/// The UI class should be registered with CS_IME specified in the style field so every
		/// application can use it through the IME class.
		/// </summary>
		public const int CS_IME = 0x00010000;

		/// <summary>
		/// Enables the drop shadow effect on a window. The effect is turned on and off through 
		/// SPI_SETDROPSHADOW. Typically, this is enabled for small, short-lived windows such as menus to 
		/// emphasize their Z order relationship to other windows.
		/// </summary>
		public const int CS_DROPSHADOW = 0x00020000;

		#endregion

		#region ChildFromPointEx Flags

		/// <summary>
		/// Does not skip any child windows.
		/// </summary>
		public const int CWP_ALL = 0x0000;

		/// <summary>
		/// Skips invisible child windows.
		/// </summary>
		public const int CWP_SKIPINVISIBLE = 0x0001;

		/// <summary>
		/// Skips disabled child windows.
		/// </summary>
		public const int CWP_SKIPDISABLED = 0x0002;

		/// <summary>
		/// Skips transparent child windows.
		/// </summary>
		public const int CWP_SKIPTRANSPARENT = 0x0004;

		#endregion

		#region Edit Control Messages

		/// <summary>
		/// Retrieves the starting and ending character positions of the current selection in an edit control. 
		/// You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_GETSEL = 0x00B0;

		/// <summary>
		/// Selects a range of characters in an edit control. You can send this message to either an edit 
		/// control or a rich edit control.
		/// </summary>
		public const int EM_SETSEL = 0x00B1;

		/// <summary>
		/// Retrieves the formatting rectangle of an edit control. The formatting rectangle is the limiting 
		/// rectangle into which the control draws the text. The limiting rectangle is independent of the 
		/// size of the edit-control window. You can send this message to either an edit control or a rich 
		/// edit control.
		/// </summary>
		public const int EM_GETRECT = 0x00B2;

		/// <summary>
		/// Sets the formatting rectangle of a multiline edit control. The formatting rectangle is the 
		/// limiting rectangle into which the control draws the text. The limiting rectangle is independent 
		/// of the size of the edit control window. This message is processed only by multiline edit controls. You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_SETRECT = 0x00B3;

		/// <summary>
		/// Sets the formatting rectangle of a multiline edit control. The EM_SETRECTNP message is identical
		/// to the EM_SETRECT message, except that EM_SETRECTNP does not redraw the edit control window.
		/// The formatting rectangle is the limiting rectangle into which the control draws the text. The 
		/// limiting rectangle is independent of the size of the edit control window. This message is 
		/// processed only by multiline edit controls. You can send this message to either an edit control 
		/// or a rich edit control.
		/// </summary>
		public const int EM_SETRECTNP = 0x00B4;

		/// <summary>
		/// Scrolls the text vertically in a multiline edit control. This message is equivalent to sending a 
		/// WM_VSCROLL message to the edit control. You can send this message to either an edit control or a 
		/// rich edit control.
		/// </summary>
		public const int EM_SCROLL = 0x00B5;

		/// <summary>
		/// Scrolls the text in a multiline edit control.
		/// </summary>
		public const int EM_LINESCROLL = 0x00B6;

		/// <summary>
		/// Scrolls the caret into view in an edit control. You can send this message to either an edit
		/// control or a rich edit control.
		/// </summary>
		public const int EM_SCROLLCARET = 0x00B7;

		/// <summary>
		/// Retrieves the state of an edit control's modification flag. The flag indicates whether the 
		/// contents of the edit control have been modified. You can send this message to either an edit 
		/// control or a rich edit control.
		/// </summary>
		public const int EM_GETMODIFY = 0x00B8;

		/// <summary>
		/// Sets or clears the modification flag for an edit control. The modification flag indicates 
		/// whether the text within the edit control has been modified. You can send this message to 
		/// either an edit control or a rich edit control.
		/// </summary>
		public const int EM_SETMODIFY = 0x00B9;

		/// <summary>
		/// Retrieves the number of lines in a multiline edit control. You can send this message to either 
		/// an edit control or a rich edit control.
		/// </summary>
		public const int EM_GETLINECOUNT = 0x00BA;

		/// <summary>
		/// Retrieves the character index of the first character of a specified line in a multiline edit 
		/// control. A character index is the zero-based index of the character from the beginning of the 
		/// edit control. You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_LINEINDEX = 0x00BB;

		/// <summary>
		/// Sets the handle of the memory that will be used by a multiline edit control.
		/// </summary>
		public const int EM_SETHANDLE = 0x00BC;

		/// <summary>
		/// Retrieves a handle of the memory currently allocated for a multiline edit control's text.
		/// </summary>
		public const int EM_GETHANDLE = 0x00BD;

		/// <summary>
		/// Retrieves the position of the scroll box (thumb) in the vertical scroll bar of a multiline edit 
		/// control. You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_GETTHUMB = 0x00BE;

		/// <summary>
		/// Retrieves the length, in characters, of a line in an edit control. You can send this message to 
		/// either an edit control or a rich edit control.
		/// </summary>
		public const int EM_LINELENGTH = 0x00C1;

		/// <summary>
		/// Replaces the current selection in an edit control with the specified text. You can send this 
		/// message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_REPLACESEL = 0x00C2;

		/// <summary>
		/// Copies a line of text from an edit control and places it in a specified buffer. You can send 
		/// this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_GETLINE = 0x00C4;

		/// <summary>
		/// Sets the text limit of an edit control. The text limit is the maximum amount of text, in TCHARs, 
		/// that the user can type into the edit control. You can send this message to either an edit control 
		/// or a rich edit control.
		/// </summary>
		public const int EM_LIMITTEXT = 0x00C5;

		/// <summary>
		/// Determines whether there are any actions in an edit control's undo queue. You can send this 
		/// message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_CANUNDO = 0x00C6;

		/// <summary>
		/// Undoes the last edit control operation in the control's undo queue. You can send this message 
		/// to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_UNDO = 0x00C7;

		/// <summary>
		/// Sets a flag that determines whether a multiline edit control includes soft line-break 
		/// characters. A soft line break consists of two carriage returns and a line feed and is inserted 
		/// at the end of a line that is broken because of wordwrapping.
		/// </summary>
		public const int EM_FMTLINES = 0x00C8;

		/// <summary>
		/// Retrieves the index of the line that contains the specified character index in a multiline edit 
		/// control. A character index is the zero-based index of the character from the beginning of the 
		/// edit control. You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_LINEFROMCHAR = 0x00C9;

		/// <summary>
		/// Sets the tab stops in a multiline edit control. When text is copied to the control, any tab 
		/// character in the text causes space to be generated up to the next tab stop.
		/// </summary>
		public const int EM_SETTABSTOPS = 0x00CB;

		/// <summary>
		/// Sets or removes the password character for an edit control. When a password character is set,
		/// that character is displayed in place of the characters typed by the user. You can send this
		/// message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_SETPASSWORDCHAR = 0x00CC;

		/// <summary>
		/// Resets the undo flag of an edit control. The undo flag is set whenever an operation within 
		/// the edit control can be undone. You can send this message to either an edit control or a rich 
		/// edit control.
		/// </summary>
		public const int EM_EMPTYUNDOBUFFER = 0x00CD;

		/// <summary>
		/// Retrieves the zero-based index of the uppermost visible line in a multiline edit control. 
		/// You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_GETFIRSTVISIBLELINE = 0x00CE;

		/// <summary>
		/// Sets or removes the read-only style (ES_READONLY) of an edit control. You can send this 
		/// message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_SETREADONLY = 0x00CF;

		/// <summary>
		/// Replaces an edit control's default Wordwrap function with an application-defined Wordwrap 
		/// function. You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_SETWORDBREAKPROC = 0x00D0;

		/// <summary>
		/// Retrieves the address of the current Wordwrap function. You can send this message to either 
		/// an edit control or a rich edit control.
		/// </summary>
		public const int EM_GETWORDBREAKPROC = 0x00D1;

		/// <summary>
		/// Retrieves the password character that an edit control displays when the user enters text. 
		/// You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_GETPASSWORDCHAR = 0x00D2;

		/// <summary>
		/// Sets the widths of the left and right margins for an edit control. The message redraws the 
		/// control to reflect the new margins. You can send this message to either an edit control or a 
		/// rich edit control.
		/// </summary>
		public const int EM_SETMARGINS = 0x00D3;

		/// <summary>
		/// Retrieves the widths of the left and right margins for an edit control.
		/// </summary>
		public const int EM_GETMARGINS = 0x00D4;

		/// <summary>
		/// sets the text limit of an edit control. The text limit is the maximum amount of text, in 
		/// TCHARs, that the user can type into the edit control. You can send this message to either an 
		/// edit control or a rich edit control.
		/// </summary>
		public const int EM_SETLIMITTEXT = EM_LIMITTEXT;

		/// <summary>
		/// Retrieves the current text limit for an edit control. You can send this message to either an 
		/// edit control or a rich edit control.
		/// </summary>
		public const int EM_GETLIMITTEXT = 0x00D5;

		/// <summary>
		/// Retrieves the client area coordinates of a specified character in an edit control. You can 
		/// send this message to either an edit control or a rich edit control. 
		/// </summary>
		public const int EM_POSFROMCHAR = 0x00D6;

		/// <summary>
		/// Retrieves information about the character closest to a specified point in the client area of 
		/// an edit control. You can send this message to either an edit control or a rich edit control.
		/// </summary>
		public const int EM_CHARFROMPOS = 0x00D7;

		/// <summary>
		/// Sets the status flags that determine how an edit control interacts with the IME.
		/// </summary>
		public const int EM_SETIMESTATUS = 0x00D8;

		/// <summary>
		/// Retrieves a set of status flags that indicate how the edit control interacts with the Input 
		/// Method Editor (IME).
		/// </summary>
		public const int EM_GETIMESTATUS = 0x00D9;

		#endregion

		#region Extended Window Styles

		/// <summary>
		/// Windows that can accept dragged objects must be created with this style so that Windows can determine
		/// that the window will accept objects and can change the drag/drop cursor as the user drags an
		/// object over the window.
		/// </summary>
		public const int WS_EX_ACCEPTFILES = 0x00000010;

		/// <summary>
		/// Forces a top-level window onto the taskbar when the window is visible.
		/// </summary>
		public const int WS_EX_APPWINDOW = 0x00040000;

		/// <summary>
		/// Specifies that a window has a border with a sunken edge.
		/// </summary>
		public const int WS_EX_CLIENTEDGE = 0x00000200;

		/// <summary>
		/// Windows XP: Paints all descendants of a window in bottom-to-top painting order using
		/// double-buffering.
		/// This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. 
		/// </summary>
		public const int WS_EX_COMPOSITED = 0x02000000;

		/// <summary>
		/// Includes a question mark in the title bar of the window. When the user clicks the question mark,
		/// the cursor changes to a question mark with a pointer. If the user then clicks a child window, the
		/// child receives a WM_HELP message. The child window should pass the message to the parent window
		/// procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help
		/// application displays a pop-up window that typically contains help for the child window.
		/// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
		/// </summary>
		public const int WS_EX_CONTEXTHELP = 0x00000400;

		/// <summary>
		/// The window itself contains child windows that should take part in dialog box navigation.
		/// If this style is specified, the dialog manager recurses into children of this window when
		/// performing navigation operations such as handling the TAB key, an arrow key, or a keyboard
		/// mnemonic.
		/// </summary>
		public const int WS_EX_CONTROLPARENT = 0x00010000;

		/// <summary>
		/// When this style is enabled, Windows uses a dialog border on a window that has a caption.
		/// When used, the WS_EX_DLGMODALFRAME style overrides the WS_BORDER and WS_THICKFRAME styles,
		/// producing a window with a dialog frame. This extended style is normally used on dialog boxes,
		/// but it can be used for any window to get a dialog frame.
		/// </summary>
		public const int WS_EX_DLGMODALFRAME = 0x00000001;

		/// <summary>
		/// Windows 2000/XP: Creates a layered window. Note that this cannot be used for child windows.
		/// Also, this cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
		/// </summary>
		public const int WS_EX_LAYERED = 0x00080000;

		/// <summary>
		/// Arabic and Hebrew versions of Windows 98/Me, Windows 2000/XP: Creates a window whose horizontal
		/// origin is on the right edge. Increasing horizontal values advance to the left.
		/// </summary>
		public const int WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring.

		/// <summary>
		/// Creates a window that has generic left-aligned properties. This is the default.
		/// </summary>
		public const int WS_EX_LEFT = 0x00000000;

		/// <summary>
		/// If the shell language is Hebrew, Arabic, or another language that supports reading order
		/// alignment, the vertical scroll bar (if present) is to the left of the client area. For other
		/// languages, the style is ignored.
		/// </summary>
		public const int WS_EX_LEFTSCROLLBAR = 0x00004000;

		/// <summary>
		/// The window text is displayed using left-to-right reading-order properties. This is the default.
		/// </summary>
		public const int WS_EX_LTRREADING = 0x00000000;

		/// <summary>
		/// Creates a multiple-document interface (MDI) child window.
		/// </summary>
		public const int WS_EX_MDICHILD = 0x00000040;

		/// <summary>
		/// Windows 2000/XP: A top-level window created with this style does not become the foreground window
		/// when the user clicks it. The system does not bring this window to the foreground when the user
		/// minimizes or closes the foreground window. 
		/// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
		/// The window does not appear on the taskbar by default. To force the window to appear on the
		/// taskbar, use the WS_EX_APPWINDOW style.
		/// </summary>
		public const int WS_EX_NOACTIVATE = 0x08000000;

		/// <summary>
		/// Windows 2000/XP: A window created with this style does not pass its window layout to its
		/// child windows.
		/// </summary>
		public const int WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children.

		/// <summary>
		/// This style is used for child windows. When this style is enabled, Windows does not send
		/// WM_NOTIFY messages to the child window's parent. By default, Windows sends WM_NOTIFY messages
		/// to a child window's parent when a child window is created or destroyed.
		/// </summary>
		public const int WS_EX_NOPARENTNOTIFY = 0x00000004;

		/// <summary>
		/// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles.
		/// </summary>
		public const int WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE;

		/// <summary>
		/// Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles.
		/// </summary>
		public const int WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST;

		/// <summary>
		/// The window has generic "right-aligned" properties. This depends on the window class. This style
		/// has an effect only if the shell language is Hebrew, Arabic, or another language that supports
		/// reading-order alignment; otherwise, the style is ignored.
		/// Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT
		/// or ES_RIGHT style, respectively. Using this style with button controls has the same effect as
		/// using BS_RIGHT and BS_RIGHTBUTTON styles.
		/// </summary>
		public const int WS_EX_RIGHT = 0x00001000;

		/// <summary>
		/// Vertical scroll bar (if present) is to the right of the client area. This is the default.
		/// </summary>
		public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;

		/// <summary>
		/// If the shell language is Hebrew, Arabic, or another language that supports reading-order
		/// alignment, the window text is displayed using right-to-left reading-order properties. For other
		/// languages, the style is ignored.
		/// </summary>
		public const int WS_EX_RTLREADING = 0x00002000;

		/// <summary>
		/// Creates a window with a three-dimensional border style intended to be used for items that
		/// do not accept user input.
		/// </summary>
		public const int WS_EX_STATICEDGE = 0x00020000;

		/// <summary>
		/// Creates a tool window; that is, a window intended to be used as a floating toolbar. A tool
		/// window has a title bar that is shorter than a normal title bar, and the window title is drawn
		/// using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears
		/// when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on
		/// the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
		/// </summary>
		public const int WS_EX_TOOLWINDOW = 0x00000080;

		/// <summary>
		/// This style applies only to top-level windows; it is ignored for child windows. When this style
		/// is enabled, Windows places the window above any windows that do not have the WS_EX_TOPMOST style.
		/// Beginning with Windows version 3.1, there are two classes of top-level windows: topmost top-level
		/// windows and top-level windows. Topmost top-level windows always appear above top-level windows in
		/// the Z order. Top-level windows can be made topmost top-level windows by calling the SetWindowPos
		/// function with the handle to the window and –1 for the hwndInsertAfter parameter.
		/// Topmost top-level windows can become regular top-level windows by calling SetWindowPos with the
		/// window handle and 1 for the hwndInsertAfter parameter.
		/// </summary>
		public const int WS_EX_TOPMOST = 0x00000008;

		/// <summary>
		/// The WS_EX_TRANSPARENT style makes a window transparent; that is, the window can be seen through,
		/// and anything under the window is still visible. Transparent windows are not transparent to mouse
		/// or keyboard events. A transparent window receives paint messages when anything under it changes.
		/// Transparent windows are useful for drawing drag handles on top of other windows or for
		/// implementing "hot-spot" areas without having to hit test because the transparent window receives
		/// click messages.
		/// </summary>
		public const int WS_EX_TRANSPARENT = 0x00000020;

		/// <summary>
		/// Specifies that a window has a border with a raised edge.
		/// </summary>
		public const int WS_EX_WINDOWEDGE = 0x00000100;

		#endregion

		#region GetDCEx() flags

		/// <summary>
		/// Returns a device context corresponding to the window rectangle rather than the client rectangle.
		/// </summary>
		public const int DCX_WINDOW = 0x00000001;

		/// <summary>
		/// Returns a device context from the cache, rather than the OWNDC or CLASSDC window. 
		/// Essentially overrides CS_OWNDC and CS_CLASSDC.
		/// </summary>
		public const int DCX_CACHE = 0x00000002;

		/// <summary>
		/// Does not reset the attributes of this device context to the default attributes when 
		/// this device context is released.
		/// </summary>
		public const int DCX_NORESETATTRS = 0x00000004;

		/// <summary>
		/// Excludes the visible regions of all child windows below the window identified by hWnd.
		/// </summary>
		public const int DCX_CLIPCHILDREN = 0x00000008;

		/// <summary>
		/// Excludes the visible regions of all sibling windows above the window identified by hWnd.
		/// </summary>
		public const int DCX_CLIPSIBLINGS = 0x00000010;

		/// <summary>
		/// Uses the visible region of the parent window. The parent's WS_CLIPCHILDREN and CS_PARENTDC 
		/// style bits are ignored. The device context origin is set to the upper-left corner of the window
		/// identified by hWnd.
		/// </summary>
		public const int DCX_PARENTCLIP = 0x00000020;

		/// <summary>
		/// When specified with DCX_INTERSECTUPDATE, causes the device context to be completely validated. 
		/// Using this function with both DCX_INTERSECTUPDATE and DCX_VALIDATE is identical to using the 
		/// BeginPaint function.
		/// </summary>
		public const int DCX_VALIDATE = 0x00200000;

		/// <summary>
		/// The clipping region identified by hrgnClip is excluded from the visible region of the 
		/// returned device context.
		/// </summary>
		public const int DCX_EXCLUDERGN = 0x00000040;

		/// <summary>
		/// The clipping region identified by hrgnClip is intersected with the visible region of the 
		/// returned device context.
		/// </summary>
		public const int DCX_INTERSECTRGN = 0x00000080;

		/// <summary>
		/// Returns a region that excludes the window's update region.
		/// </summary>
		public const int DCX_EXCLUDEUPDATE = 0x00000100;

		/// <summary>
		/// Returns a region that includes the window's update region.
		/// </summary>
		public const int DCX_INTERSECTUPDATE = 0x00000200;

		/// <summary>
		/// Allows drawing even if there is a LockWindowUpdate call in effect that would otherwise exclude 
		/// this window. Used for drawing during tracking.
		/// </summary>
		public const int DCX_LOCKWINDOWUPDATE = 0x00000400;

		#endregion

		#region Key State Masks for Mouse Messages

		/// <summary>
		/// The left mouse button is down.
		/// </summary>
		public const int MK_LBUTTON = 0x0001;

		/// <summary>
		/// The right mouse button is down.
		/// </summary>
		public const int MK_RBUTTON = 0x0002;

		/// <summary>
		/// The SHIFT key is down.
		/// </summary>
		public const int MK_SHIFT = 0x0004;

		/// <summary>
		/// The CTRL key is down.
		/// </summary>
		public const int MK_CONTROL = 0x0008;

		/// <summary>
		/// The middle mouse button is down.
		/// </summary>
		public const int MK_MBUTTON = 0x0010;

		/// <summary>
		/// Windows 2000/Windows XP: the first X button is down.
		/// </summary>
		public const int MK_XBUTTON1 = 0x0020;

		/// <summary>
		/// Windows 2000/Windows XP: the second X button is down.
		/// </summary>
		public const int MK_XBUTTON2 = 0x0040;

		#endregion

		#region Layered Window Attributes

		/// <summary>
		/// Use crKey as the transparency color.
		/// </summary>
		public const int ULW_COLORKEY = 0x00000001;

		/// <summary>
		/// Use pblend as the blend function. If the display mode is 256 colors or less, the effect of 
		/// this value is the same as the effect of ULW_OPAQUE.
		/// </summary>
		public const int ULW_ALPHA = 0x00000002;

		/// <summary>
		/// Draw an opaque layered window.
		/// </summary>
		public const int ULW_OPAQUE = 0x00000004;

		#endregion

		#region Mouse Position Codes

		/// <summary>
		/// Location of the mouse:
		/// On the screen background or on a dividing line between windows (same as HTNOWHERE except that
		/// the DefWndProc Windows function produces a system beep to indicate an error).
		/// </summary>
		public const int HTERROR = (-2);

		/// <summary>
		/// Location of the mouse:
		/// In a window currently covered by another window.
		/// </summary>
		public const int HTTRANSPARENT = (-1);

		/// <summary>
		/// Location of the mouse:
		/// On the screen background or on a dividing line between windows.
		/// </summary>
		public const int HTNOWHERE = 0;

		/// <summary>
		/// Location of the mouse:
		/// In a client area.
		/// </summary>
		public const int HTCLIENT = 1;

		/// <summary>
		/// Location of the mouse:
		/// In a title-bar area.
		/// </summary>
		public const int HTCAPTION = 2;

		/// <summary>
		/// Location of the mouse:
		/// In a Control menu or in a Close button in a child window.
		/// </summary>
		public const int HTSYSMENU = 3;

		/// <summary>
		/// Location of the mouse:
		/// In a size box.
		/// </summary>
		public const int HTGROWBOX = 4;

		/// <summary>
		/// Location of the mouse:
		/// In a size box (same as HTGROWBOX).
		/// </summary>
		public const int HTSIZE = HTGROWBOX;

		/// <summary>
		/// Location of the mouse:
		/// In a menu area.
		/// </summary>
		public const int HTMENU = 5;

		/// <summary>
		/// Location of the mouse:
		/// In the horizontal scroll bar.
		/// </summary>
		public const int HTHSCROLL = 6;

		/// <summary>
		/// Location of the mouse:
		/// In the vertical scroll bar
		/// </summary>
		public const int HTVSCROLL = 7;

		/// <summary>
		/// Location of the mouse:
		/// In a Minimize button.
		/// </summary>
		public const int HTMINBUTTON = 8;

		/// <summary>
		/// Location of the mouse:
		/// In a Maximize button.
		/// </summary>
		public const int HTMAXBUTTON = 9;

		/// <summary>
		/// Location of the mouse:
		/// In the left border of the window.
		/// </summary>
		public const int HTLEFT = 10;

		/// <summary>
		/// Location of the mouse:
		/// In the right border of the window.
		/// </summary>
		public const int HTRIGHT = 11;

		/// <summary>
		/// Location of the mouse:
		/// In the upper horizontal border of the window.
		/// </summary>
		public const int HTTOP = 12;

		/// <summary>
		/// Location of the mouse:
		/// In the upper-left corner of the window border.
		/// </summary>
		public const int HTTOPLEFT = 13;

		/// <summary>
		/// Location of the mouse:
		/// In the upper-right corner of the window border.
		/// </summary>
		public const int HTTOPRIGHT = 14;

		/// <summary>
		/// Location of the mouse:
		/// In the lower horizontal border of the window.
		/// </summary>
		public const int HTBOTTOM = 15;

		/// <summary>
		/// Location of the mouse:
		/// In the lower-left corner of the window border.
		/// </summary>
		public const int HTBOTTOMLEFT = 16;

		/// <summary>
		/// Location of the mouse:
		/// In the lower-right corner of the window border.
		/// </summary>
		public const int HTBOTTOMRIGHT = 17;

		/// <summary>
		/// Location of the mouse:
		/// In the border of a window that does not have a sizing border. Use to disallow moving or sizing.
		/// Can still be used to activate app.
		/// </summary>
		public const int HTBORDER = 18;

		/// <summary>
		/// Location of the mouse:
		/// In a Minimize button.
		/// </summary>
		public const int HTREDUCE = HTMINBUTTON;

		/// <summary>
		/// Location of the mouse:
		/// In a Maximize button.
		/// </summary>
		public const int HTZOOM = HTMAXBUTTON;

		/// <summary>
		/// Location of the mouse:
		/// In the left border of the window.
		/// </summary>
		public const int HTSIZEFIRST = HTLEFT;

		/// <summary>
		/// Location of the mouse:
		/// In the lower-right corner of the window border.
		/// </summary>
		public const int HTSIZELAST = HTBOTTOMRIGHT;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int HTOBJECT = 19;

		/// <summary>
		/// Location of the mouse:
		/// In a Close button.
		/// </summary>
		public const int HTCLOSE = 20;

		/// <summary>
		/// Location of the mouse:
		/// In a Help button.
		/// </summary>
		public const int HTHELP = 21;

		#endregion

		#region Scroll Bar Constants

		/// <summary>
		/// Horizontal scroll bar.
		/// </summary>
		public const int SB_HORZ = 0;

		/// <summary>
		/// Vertical scroll bar.
		/// </summary>
		public const int SB_VERT = 1;

		/// <summary>
		/// Scroll bar control.
		/// </summary>
		public const int SB_CTL = 2;

		/// <summary>
		/// Horizontal and vertical scroll bars.
		/// </summary>
		public const int SB_BOTH = 3;

		#endregion

		#region Scroll Bar Commands

		/// <summary>
		/// The user clicks the top scroll arrow. Decrements the scroll box position; scrolls toward the 
		/// top of the data by one unit.
		/// </summary>
		public const int SB_LINEUP = 0;

		/// <summary>
		/// The user clicks the left scroll arrow. Decrements the scroll box position;
		/// scrolls toward the left end of the data by one unit.
		/// </summary>
		public const int SB_LINELEFT = 0;

		/// <summary>
		/// The user clicks the bottom scroll arrow. Increments the scroll box position;
		/// scrolls toward the bottom of the data by one unit.
		/// </summary>
		public const int SB_LINEDOWN = 1;

		/// <summary>
		/// The user clicks the right scroll arrow. Increments the scroll box position;
		/// scrolls toward the right end of the data by one unit.
		/// </summary>
		public const int SB_LINERIGHT = 1;

		/// <summary>
		/// The user clicks the scroll bar shaft above the scroll box. Decrements the scroll
		/// box position by the number of data units in the window; scrolls toward the top of
		/// the data by the same number of units.
		/// </summary>
		public const int SB_PAGEUP = 2;

		/// <summary>
		/// The user clicks the scroll bar shaft to the left of the scroll box. Decrements the
		/// scroll box position by the number of data units in the window; scrolls toward the left
		/// end of the data by the same number of units.
		/// </summary>
		public const int SB_PAGELEFT = 2;

		/// <summary>
		/// The user clicks the scroll bar shaft below the scroll box. Increments the scroll box
		/// position by the number of data units in the window; scrolls toward the bottom of the data
		/// by the same number of units.
		/// </summary>
		public const int SB_PAGEDOWN = 3;

		/// <summary>
		/// The user clicks the scroll bar shaft to the right of the scroll box. Increments the scroll
		/// box position by the number of data units in the window; scrolls toward the right end of the
		/// data by the same number of units.
		/// </summary>
		public const int SB_PAGERIGHT = 3;

		/// <summary>
		/// The user releases the scroll box after dragging it. Sets the scroll box to the position specified
		/// in the message; scrolls the data by the same number of units the scroll box has moved.
		/// </summary>
		public const int SB_THUMBPOSITION = 4;

		/// <summary>
		/// The user drags the scroll box. Sets the scroll box to the position specified in the message and
		/// scrolls the data by the same number of units the scroll box has moved for applications that draw
		/// data quickly. Applications that cannot draw data quickly must wait for the SB_THUMBPOSITION
		/// request code before moving the scroll box and scrolling the data.
		/// </summary>
		public const int SB_THUMBTRACK = 5;

		/// <summary>
		/// Scrolls to the upper left.
		/// </summary>
		public const int SB_TOP = 6;

		/// <summary>
		/// Scroll to far left.
		/// </summary>
		public const int SB_LEFT = 6;

		/// <summary>
		/// Scrolls to the lower right.
		/// </summary>
		public const int SB_BOTTOM = 7;

		/// <summary>
		/// Scroll to far right.
		/// </summary>
		public const int SB_RIGHT = 7;

		/// <summary>
		/// The user releases the mouse after holding it on an arrow or in the scroll bar shaft.
		/// No response is needed.
		/// </summary>
		public const int SB_ENDSCROLL = 8;

		#endregion

		#region SetWindowPos Flags

		/// <summary>
		/// Retains the current size (ignores the cx and cy parameters).
		/// </summary>
		public const int SWP_NOSIZE = 0x0001;

		/// <summary>
		/// Retains the current position (ignores X and Y parameters).
		/// </summary>
		public const int SWP_NOMOVE = 0x0002;

		/// <summary>
		/// Retains the current Z order (ignores the hWndInsertAfter parameter).
		/// </summary>
		public const int SWP_NOZORDER = 0x0004;

		/// <summary>
		/// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
		/// the client area, the nonclient area (including the title bar and scroll bars), and any part of 
		/// the parent window uncovered as a result of the window being moved. When this flag is set, the 
		/// application must explicitly invalidate or redraw any parts of the window and parent window that 
		/// need redrawing.
		/// </summary>
		public const int SWP_NOREDRAW = 0x0008;

		/// <summary>
		/// Does not activate the window. If this flag is not set, the window is activated and moved to 
		/// the top of either the topmost or non-topmost group (depending on the setting of the 
		/// hWndInsertAfter parameter).
		/// </summary>
		public const int SWP_NOACTIVATE = 0x0010;

		/// <summary>
		/// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message 
		/// to the window, even if the window's size is not being changed. If this flag is not specified, 
		/// WM_NCCALCSIZE is sent only when the window's size is being changed.
		/// </summary>
		public const int SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */

		/// <summary>
		/// Displays the window.
		/// </summary>
		public const int SWP_SHOWWINDOW = 0x0040;

		/// <summary>
		/// Hides the window.
		/// </summary>
		public const int SWP_HIDEWINDOW = 0x0080;

		/// <summary>
		/// Discards the entire contents of the client area. If this flag is not specified, the valid 
		/// contents of the client area are saved and copied back into the client area after the window is 
		/// sized or repositioned.
		/// </summary>
		public const int SWP_NOCOPYBITS = 0x0100;

		/// <summary>
		/// Does not change the owner window's position in the Z order.
		/// </summary>
		public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */

		/// <summary>
		/// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
		/// </summary>
		public const int SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

		/// <summary>
		/// Draws a frame (defined in the window's class description) around the window.
		/// </summary>
		public const int SWP_DRAWFRAME = SWP_FRAMECHANGED;

		/// <summary>
		/// Same as the SWP_NOOWNERZORDER flag.
		/// </summary>
		public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;

		/// <summary>
		/// Prevents generation of the WM_SYNCPAINT message.
		/// </summary>
		public const int SWP_DEFERERASE = 0x2000;

		/// <summary>
		/// If the calling thread and the thread that owns the window are attached to different input 
		/// queues, the system posts the request to the thread that owns the window. This prevents the 
		/// calling thread from blocking its execution while other threads process the request.
		/// </summary>
		public const int SWP_ASYNCWINDOWPOS = 0x4000;

		/// <summary>
		/// Places the window at the top of the Z order.
		/// </summary>
		public const int HWND_TOP = 0;

		/// <summary>
		/// Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost 
		/// window, the window loses its topmost status and is placed at the bottom of all other windows.
		/// </summary>
		public const int HWND_BOTTOM = 1;

		/// <summary>
		/// Places the window above all non-topmost windows. The window maintains its topmost position 
		/// even when it is deactivated.
		/// </summary>
		public const int HWND_TOPMOST = -1;

		/// <summary>
		/// Places the window above all non-topmost windows (that is, behind all topmost windows). This 
		/// flag has no effect if the window is already a non-topmost window.
		/// </summary>
		public const int HWND_NOTOPMOST = -2;

		#endregion

		#region ShowWindow() Commands

		/// <summary>
		/// Hides the window and activates another window.
		/// </summary>
		public const int SW_HIDE = 0;

		/// <summary>
		/// Activates and displays a window. If the window is minimized or maximized, the system restores
		/// it to its original size and position. An application should specify this flag when displaying
		/// the window for the first time.
		/// </summary>
		public const int SW_SHOWNORMAL = 1;

		/// <summary>
		/// Activates the window and displays it as a minimized window.
		/// </summary>
		public const int SW_SHOWMINIMIZED = 2;

		/// <summary>
		/// Activates the window and displays it as a maximized window.
		/// </summary>
		public const int SW_SHOWMAXIMIZED = 3;

		/// <summary>
		/// Maximizes the specified window.
		/// </summary>
		public const int SW_MAXIMIZE = 3;

		/// <summary>
		/// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, 
		/// except the window is not actived.
		/// </summary>
		public const int SW_SHOWNOACTIVATE = 4;

		/// <summary>
		/// Activates the window and displays it in its current size and position.
		/// </summary>
		public const int SW_SHOW = 5;

		/// <summary>
		/// Minimizes the specified window and activates the next top-level window in the Z order.
		/// </summary>
		public const int SW_MINIMIZE = 6;

		/// <summary>
		/// Displays the window as a minimized window. 
		/// This value is similar to SW_SHOWMINIMIZED, except the window is not activated.
		/// </summary>
		public const int SW_SHOWMINNOACTIVE = 7;

		/// <summary>
		/// Displays the window in its current size and position. 
		/// This value is similar to SW_SHOW, except the window is not activated.
		/// </summary>
		public const int SW_SHOWNA = 8;

		/// <summary>
		/// Activates and displays the window. If the window is minimized or maximized, the system 
		/// restores it to its original size and position. An application should specify this flag 
		/// when restoring a minimized window.
		/// </summary>
		public const int SW_RESTORE = 9;

		/// <summary>
		/// Sets the show state based on the SW_ value specified in the STARTUPINFO structure 
		/// passed to the CreateProcess function by the program that started the application.
		/// </summary>
		public const int SW_SHOWDEFAULT = 10;

		/// <summary>
		/// Windows 2000/XP: Minimizes a window, even if the thread that owns the window is hung. 
		/// This flag should only be used when minimizing windows from a different thread.
		/// </summary>
		public const int SW_FORCEMINIMIZE = 11;

		#endregion

		#region Window field offsets for GetWindowLong()

		/// <summary>
		/// Sets a new address for the window procedure. Windows NT/2000/XP: You cannot change this 
		/// attribute if the window does not belong to the same process as the calling thread.
		/// </summary>
		public const int GWL_WNDPROC = -4;

		/// <summary>
		/// Sets a new application instance handle.
		/// </summary>
		public const int GWL_HINSTANCE = -6;

		/// <summary>
		/// Retrieves a handle to the parent window, if any.
		/// </summary>
		public const int GWL_HWNDPARENT = -8;

		/// <summary>
		/// Sets a new window style.
		/// </summary>
		public const int GWL_STYLE = -16;

		/// <summary>
		/// Sets a new extended window style. For more information, see CreateWindowEx.
		/// </summary>
		public const int GWL_EXSTYLE = -20;

		/// <summary>
		/// Sets the user data associated with the window. This data is intended for use by the application 
		/// that created the window. Its value is initially zero.
		/// </summary>
		public const int GWL_USERDATA = -21;

		/// <summary>
		/// Sets a new identifier of the window.
		/// </summary>
		public const int GWL_ID = -12;

		/// <summary>
		/// Retrieves the pointer to the window procedure, or a handle representing the pointer to the window
		/// procedure. You must use the CallWindowProc function to call the window procedure.
		/// </summary>
		public const int GWLP_WNDPROC = -4;

		/// <summary>
		/// Retrieves a handle to the application instance.
		/// </summary>
		public const int GWLP_HINSTANCE = -6;

		/// <summary>
		/// Retrieves a handle to the parent window, if there is one.
		/// </summary>
		public const int GWLP_HWNDPARENT = -8;

		/// <summary>
		/// Retrieves the user data associated with the window. This data is intended for use by the
		/// application that created the window. Its value is initially zero.
		/// </summary>
		public const int GWLP_USERDATA = -21;

		/// <summary>
		/// Retrieves the identifier of the window.
		/// </summary>
		public const int GWLP_ID = -12;

		#endregion

		#region Window Messages

		/// <summary>
		/// The WM_NULL message performs no operation. An application sends the WM_NULL message if it
		/// wants to post a message that the recipient window will ignore.
		/// </summary>
		public const int WM_NULL = 0x0000;

		/// <summary>
		/// The WM_THEMECHANGED message is broadcast to every window following a theme change event.
		/// Examples of theme change events are the activation of a theme, the deactivation of a theme,
		/// or a transition from one theme to another.
		/// </summary>
		public const int WM_THEMECHANGED = 0x031A;

		/// <summary>
		/// The WM_CREATE message is sent when an application requests that a window be created by calling the
		/// CreateWindowEx or CreateWindow function. (The message is sent before the function returns.)
		/// The window procedure of the new window receives this message after the window is created,
		/// but before the window becomes visible. 
		/// </summary>
		public const int WM_CREATE = 0x0001;

		/// <summary>
		/// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window
		/// procedure of the window being destroyed after the window is removed from the screen.
		/// </summary>
		public const int WM_DESTROY = 0x0002;

		/// <summary>
		/// The WM_MOVE message is sent after a window has been moved.
		/// </summary>
		public const int WM_MOVE = 0x0003;

		/// <summary>
		/// The WM_SIZE message is sent to a window after its size has changed.
		/// </summary>
		public const int WM_SIZE = 0x0005;

		/// <summary>
		/// The WM_ACTIVATE message is sent to both the window being activated and the window being
		/// deactivated. If the windows use the same input queue, the message is sent synchronously, first
		/// to the window procedure of the top-level window being deactivated, then to the window procedure
		/// of the top-level window being activated. If the windows use different input queues, the message
		/// is sent asynchronously, so the window is activated immediately.
		/// </summary>
		public const int WM_ACTIVATE = 0x0006;

		/// <summary>
		/// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus.
		/// </summary>
		public const int WM_SETFOCUS = 0x0007;

		/// <summary>
		/// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus.
		/// </summary>
		public const int WM_KILLFOCUS = 0x0008;

		/// <summary>
		/// The WM_ENABLE message is sent when an application changes the enabled state of a window.
		/// It is sent to the window whose enabled state is changing. This message is sent before the
		/// EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the
		/// window has changed.
		/// </summary>
		public const int WM_ENABLE = 0x000A;

		/// <summary>
		/// An application sends the WM_SETREDRAW message to a window to allow changes in that window to
		/// be redrawn or to prevent changes in that window from being redrawn.
		/// </summary>
		public const int WM_SETREDRAW = 0x000B;

		/// <summary>
		/// An application sends a WM_SETTEXT message to set the text of a window.
		/// </summary>
		public const int WM_SETTEXT = 0x000C;

		/// <summary>
		/// An application sends a WM_GETTEXT message to copy the text that
		/// corresponds to a window into a buffer provided by the caller.
		/// </summary>
		public const int WM_GETTEXT = 0x000D;

		/// <summary>
		/// An application sends a WM_GETTEXTLENGTH message to determine the length,
		/// in characters, of the text associated with a window.
		/// </summary>
		public const int WM_GETTEXTLENGTH = 0x000E;

		/// <summary>
		/// The WM_PAINT message is sent when the system or another application makes a request to
		/// paint a portion of an application's window. The message is sent when the UpdateWindow or
		/// RedrawWindow function is called, or by the DispatchMessage function when the application
		/// obtains a WM_PAINT message by using the GetMessage or PeekMessage function.
		/// </summary>
		public const int WM_PAINT = 0x000F;

		/// <summary>
		/// The WM_CLOSE message is sent as a signal that a window or an application should terminate.
		/// </summary>
		public const int WM_CLOSE = 0x0010;

		/// <summary>
		/// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an
		/// application calls the ExitWindows function. If any application returns zero, the session is not
		/// ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns
		/// zero.
		/// After processing this message, the system sends the WM_ENDSESSION message with the wParam
		/// parameter set to the results of the WM_QUERYENDSESSION message.
		/// </summary>
		public const int WM_QUERYENDSESSION = 0x0011;

		/// <summary>
		/// The WM_QUIT message indicates a request to terminate an application and is generated when
		/// the application calls the PostQuitMessage function. It causes the GetMessage function to
		/// return zero.
		/// </summary>
		public const int WM_QUIT = 0x0012;

		/// <summary>
		/// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be
		/// restored to its previous size and position.
		/// </summary>
		public const int WM_QUERYOPEN = 0x0013;

		/// <summary>
		/// The WM_ERASEBKGND message is sent when the window background must be erased (for example,
		/// when a window is resized). The message is sent to prepare an invalidated portion of a window
		/// for painting.
		/// </summary>
		public const int WM_ERASEBKGND = 0x0014;

		/// <summary>
		/// The WM_SYSCOLORCHANGE message is sent to all top-level windows when
		/// a change is made to a system color setting.
		/// </summary>
		public const int WM_SYSCOLORCHANGE = 0x0015;

		/// <summary>
		/// The WM_ENDSESSION message is sent to an application after the system processes the results
		/// of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether
		/// the session is ending.
		/// </summary>
		public const int WM_ENDSESSION = 0x0016;

		/// <summary>
		/// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
		/// </summary>
		public const int WM_SHOWWINDOW = 0x0018;

		/// <summary>
		/// An application sends the WM_WININICHANGE message to all top-level windows after making a change
		/// to the WIN.INI file. The SystemParametersInfo function sends this message after an application
		/// uses the function to change a setting in WIN.INI.
		/// <remarks>The WM_WININICHANGE message is provided only for compatibility with earlier versions
		/// of the system. Applications should use the WM_SETTINGCHANGE message.</remarks>
		/// </summary>
		public const int WM_WININICHANGE = 0x001A;

		/// <summary>
		/// The system sends the WM_SETTINGCHANGE message to all top-level windows when the
		/// SystemParametersInfo function changes a system-wide setting or when policy settings have changed.
		/// Applications should send WM_SETTINGCHANGE to all top-level windows when they make changes to
		/// system parameters. (This message cannot be sent directly to a window.) To send the
		/// WM_SETTINGCHANGE message to all top-level windows, use the SendMessageTimeout function with
		/// the hwnd parameter set to HWND_BROADCAST.
		/// </summary>
		public const int WM_SETTINGCHANGE = 0x001A;

		/// <summary>
		/// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the
		/// user changes device-mode settings.
		/// </summary>
		public const int WM_DEVMODECHANGE = 0x001B;

		/// <summary>
		/// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than
		/// the active window is about to be activated. The message is sent to the application whose
		/// window is being activated and to the application whose window is being deactivated.
		/// </summary>
		public const int WM_ACTIVATEAPP = 0x001C;

		/// <summary>
		/// An application sends the WM_FONTCHANGE message to all top-level windows in the
		/// system after changing the pool of font resources.
		/// </summary>
		public const int WM_FONTCHANGE = 0x001D;

		/// <summary>
		/// An application sends the WM_TIMECHANGE message whenever it updates the system time.
		/// </summary>
		public const int WM_TIMECHANGE = 0x001E;

		/// <summary>
		/// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For
		/// example, the system sends this message to the active window when a dialog box or message box
		/// is displayed. Certain functions also send this message explicitly to the specified window
		/// regardless of whether it is the active window. For example, the EnableWindow function sends
		/// this message when disabling the specified window.
		/// </summary>
		public const int WM_CANCELMODE = 0x001F;

		/// <summary>
		/// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a
		/// window and mouse input is not captured.
		/// </summary>
		public const int WM_SETCURSOR = 0x0020;

		/// <summary>
		/// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button.
		/// The parent window receives this message only if the child window passes it to the DefWindowProc function.
		/// </summary>
		public const int WM_MOUSEACTIVATE = 0x0021;

		/// <summary>
		/// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the
		/// window's title bar or when the window is activated, moved, or sized.
		/// </summary>
		public const int WM_CHILDACTIVATE = 0x0022;

		/// <summary>
		/// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate
		/// user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure.
		/// </summary>
		public const int WM_QUEUESYNC = 0x0023;

		/// <summary>
		/// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window
		/// is about to change. An application can use this message to override the window's default
		/// maximized size and position, or its default minimum or maximum tracking size. 
		/// </summary>
		public const int WM_GETMINMAXINFO = 0x0024;

		/// <summary>
		/// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the
		/// icon is to be painted. This message is not sent by newer versions of Microsoft Windows,
		/// except in unusual circumstances explained in the Remarks.
		/// <remarks>Windows 95/98/Me, Windows NT 4.0 and later: This message is sent only to 16-bit
		/// windows (note that this is with a lowercase "W"), and only for compatibility reasons. Under
		/// such conditions, the value of wParam is TRUE (the value carries no significance). 
		/// On Microsoft Windows NT 3.51 and earlier, or on newer versions of Windows when the unusual
		/// circumstances above are met, the window receives this message only if a class icon is defined
		/// for the window; otherwise, WM_PAINT is sent instead.
		/// Windows NT 3.51 and earlier: The DefWindowProc function draws the class icon. On newer versions
		/// of Windows, the DefWindowProc function ignores the message.
		/// </remarks>
		/// </summary>
		public const int WM_PAINTICON = 0x0026;

		/// <summary>
		/// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when
		/// the background of the icon must be filled before painting the icon. A window receives this
		/// message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent.
		/// This message is not sent by newer versions of Windows.
		/// <remarks>Windows NT 3.51 and earlier: The DefWindowProc function fills the icon background with
		/// the class background brush of the parent window. On newer versions of Windows, the DefWindowProc
		/// function ignores the message.</remarks>
		/// </summary>
		public const int WM_ICONERASEBKGND = 0x0027;

		/// <summary>
		/// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a
		/// different control in the dialog box. 
		/// </summary>
		public const int WM_NEXTDLGCTL = 0x0028;

		/// <summary>
		/// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed
		/// from the Print Manager queue.
		/// </summary>
		public const int WM_SPOOLERSTATUS = 0x002A;

		/// <summary>
		/// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box,
		/// list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.
		/// </summary>
		public const int WM_DRAWITEM = 0x002B;

		/// <summary>
		/// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control,
		/// or menu item when the control or menu is created.
		/// </summary>
		public const int WM_MEASUREITEM = 0x002C;

		/// <summary>
		/// The WM_DELETEITEM message is sent to the owner of a list box or combo box when the list box
		/// or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT,
		/// CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each
		/// deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box
		/// item with nonzero item data.
		/// </summary>
		public const int WM_DELETEITEM = 0x002D;

		/// <summary>
		/// The WM_VKEYTOITEM message is sent by a list box with the LBS_WANTKEYBOARDINPUT style
		/// to its owner in response to a WM_KEYDOWN message. 
		/// </summary>
		public const int WM_VKEYTOITEM = 0x002E;

		/// <summary>
		/// The WM_CHARTOITEM message is sent by a list box with the LBS_WANTKEYBOARDINPUT style to
		/// its owner in response to a WM_CHAR message.
		/// </summary>
		public const int WM_CHARTOITEM = 0x002F;

		/// <summary>
		/// An application sends a WM_SETFONT message to specify the font that a control
		/// is to use when drawing text.
		/// </summary>
		public const int WM_SETFONT = 0x0030;

		/// <summary>
		/// An application sends a WM_GETFONT message to a control to retrieve the font with which
		/// the control is currently drawing its text.
		/// </summary>
		public const int WM_GETFONT = 0x0031;

		/// <summary>
		/// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window.
		/// When the user presses the hot key, the system activates the window.
		/// </summary>
		public const int WM_SETHOTKEY = 0x0032;

		/// <summary>
		/// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window.
		/// </summary>
		public const int WM_GETHOTKEY = 0x0033;

		/// <summary>
		/// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to
		/// be dragged by the user but does not have an icon defined for its class. An application can
		/// return a handle to an icon or cursor. The system displays this cursor or icon while the user
		/// drags the icon.
		/// </summary>
		public const int WM_QUERYDRAGICON = 0x0037;

		/// <summary>
		/// The system sends the WM_COMPAREITEM message to determine the relative position of a new item
		/// in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a
		/// new item, the system sends this message to the owner of a combo box or list box created with
		/// the CBS_SORT or LBS_SORT style.
		/// </summary>
		public const int WM_COMPAREITEM = 0x0039;

		/// <summary>
		/// Active Accessibility sends the WM_GETOBJECT message to obtain information about an
		/// accessible object contained in a server application.
		/// </summary>
		public const int WM_GETOBJECT = 0x003D;

		/// <summary>
		/// The WM_COMPACTING message is sent to all top-level windows when the system detects more
		/// than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting
		/// memory. This indicates that system memory is low.
		/// </summary>
		public const int WM_COMPACTING = 0x0041;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_COMMNOTIFY = 0x0044;

		/// <summary>
		/// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z
		/// order is about to change as a result of a call to the SetWindowPos function or another
		/// window-management function.
		/// </summary>
		public const int WM_WINDOWPOSCHANGING = 0x0046;

		/// <summary>
		/// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the
		/// Z order has changed as a result of a call to the SetWindowPos function or another
		/// window-management function.
		/// </summary>
		public const int WM_WINDOWPOSCHANGED = 0x0047;

		/// <summary>
		/// The WM_POWER message is broadcast when the system, typically a battery-powered
		/// personal computer, is about to enter suspended mode.
		/// </summary>
		public const int WM_POWER = 0x0048;

		/// <summary>
		/// An application sends the WM_COPYDATA message to pass data to another application.
		/// </summary>
		public const int WM_COPYDATA = 0x004A;

		/// <summary>
		/// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's
		/// journaling activities. The message is posted with a NULL window handle. 
		/// </summary>
		public const int WM_CANCELJOURNAL = 0x004B;

		/// <summary>
		/// The WM_NOTIFY message is sent by a common control to its parent window when an event has occurred
		/// or the control requires some information.
		/// </summary>
		public const int WM_NOTIFY = 0x004E;

		/// <summary>
		/// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user
		/// chooses a new input language, either with the hotkey (specified in the Keyboard control panel
		/// application) or from the indicator on the system taskbar. An application can accept the change
		/// by passing the message to the DefWindowProc function or reject the change (and prevent it from
		/// taking place) by returning immediately.
		/// </summary>
		public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;

		/// <summary>
		/// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's
		/// input language has been changed. You should make any application-specific settings and pass the
		/// message to the DefWindowProc function, which passes the message to all first-level child windows.
		/// These child windows can pass the message to DefWindowProc to have it pass the message to their
		/// child windows, and so on.
		/// </summary>
		public const int WM_INPUTLANGCHANGE = 0x0051;

		/// <summary>
		/// Sent to an application that has initiated a training card with Microsoft Windows Help.
		/// The message informs the application when the user clicks an authorable button. An application
		/// initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.
		/// </summary>
		public const int WM_TCARD = 0x0052;

		/// <summary>
		/// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP
		/// is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that
		/// has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently
		/// active window.
		/// </summary>
		public const int WM_HELP = 0x0053;

		/// <summary>
		/// The WM_USERCHANGED message is sent to all windows after the user has logged on or off.
		/// When the user logs on or off, the system updates the user-specific settings. The system sends
		/// this message immediately after updating the settings.
		/// </summary>
		public const int WM_USERCHANGED = 0x0054;

		/// <summary>
		/// Used to determine if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification
		/// message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and
		/// from the parent window to the common control.
		/// </summary>
		public const int WM_NOTIFYFORMAT = 0x0055;

		/// <summary>
		/// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button
		/// (right-clicked) in the window.
		/// </summary>
		public const int WM_CONTEXTMENU = 0x007B;

		/// <summary>
		/// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is
		/// about to change one or more of the window's styles.
		/// </summary>
		public const int WM_STYLECHANGING = 0x007C;

		/// <summary>
		/// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has
		/// changed one or more of the window's styles.
		/// </summary>
		public const int WM_STYLECHANGED = 0x007D;

		/// <summary>
		/// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.
		/// </summary>
		public const int WM_DISPLAYCHANGE = 0x007E;

		/// <summary>
		/// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon
		/// associated with a window. The system displays the large icon in the ALT+TAB dialog, and the
		/// small icon in the window caption.
		/// </summary>
		public const int WM_GETICON = 0x007F;

		/// <summary>
		/// An application sends the WM_SETICON message to associate a new large or small icon with a window.
		/// The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window
		/// caption.
		/// </summary>
		public const int WM_SETICON = 0x0080;

		/// <summary>
		/// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.
		/// </summary>
		public const int WM_NCCREATE = 0x0081;

		/// <summary>
		/// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed.
		/// The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY
		/// message. WM_DESTROY is used to free the allocated memory object associated with the window. 
		/// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast,
		/// WM_DESTROY is sent before the child windows are destroyed.
		/// </summary>
		public const int WM_NCDESTROY = 0x0082;

		/// <summary>
		/// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be
		/// calculated. By processing this message, an application can control the content of the window's
		/// client area when the size or position of the window changes.
		/// </summary>
		public const int WM_NCCALCSIZE = 0x0083;

		/// <summary>
		/// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is
		/// pressed or released. If the mouse is not captured, the message is sent to the window beneath
		/// the cursor. Otherwise, the message is sent to the window that has captured the mouse.
		/// </summary>
		public const int WM_NCHITTEST = 0x0084;

		/// <summary>
		/// The WM_NCPAINT message is sent to a window when its frame must be painted.
		/// </summary>
		public const int WM_NCPAINT = 0x0085;

		/// <summary>
		/// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to
		/// indicate an active or inactive state.
		/// </summary>
		public const int WM_NCACTIVATE = 0x0086;

		/// <summary>
		/// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default,
		/// the system handles all keyboard input to the control; the system interprets certain types of
		/// keyboard input as dialog box navigation keys. To override this default behavior, the control
		/// can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process
		/// itself.
		/// </summary>
		public const int WM_GETDLGCODE = 0x0087;

		/// <summary>
		/// The WM_SYNCPAINT message is used to synchronize painting while
		/// avoiding linking independent GUI threads.
		/// </summary>
		public const int WM_SYNCPAINT = 0x0088;

		/// <summary>
		/// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient
		/// area of the window. This message is posted to the window that contains the cursor. If a window
		/// has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCMOUSEMOVE = 0x00A0;

		/// <summary>
		/// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the
		/// cursor is within the nonclient area of a window. This message is posted to the window that
		/// contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCLBUTTONDOWN = 0x00A1;

		/// <summary>
		/// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the
		/// cursor is within the nonclient area of a window. This message is posted to the window that
		/// contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCLBUTTONUP = 0x00A2;

		/// <summary>
		/// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button
		/// while the cursor is within the nonclient area of a window. This message is posted to the window
		/// that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCLBUTTONDBLCLK = 0x00A3;

		/// <summary>
		/// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the
		/// cursor is within the nonclient area of a window. This message is posted to the window that
		/// contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCRBUTTONDOWN = 0x00A4;

		/// <summary>
		/// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the
		/// cursor is within the nonclient area of a window. This message is posted to the window that
		/// contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCRBUTTONUP = 0x00A5;

		/// <summary>
		/// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button
		/// while the cursor is within the nonclient area of a window. This message is posted to the window
		/// that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCRBUTTONDBLCLK = 0x00A6;

		/// <summary>
		/// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the
		/// cursor is within the nonclient area of a window. This message is posted to the window that
		/// contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCMBUTTONDOWN = 0x00A7;

		/// <summary>
		/// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the
		/// cursor is within the nonclient area of a window. This message is posted to the window that
		/// contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCMBUTTONUP = 0x00A8;

		/// <summary>
		/// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button
		/// while the cursor is within the nonclient area of a window. This message is posted to the window
		/// that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCMBUTTONDBLCLK = 0x00A9;

		/// <summary>
		/// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button
		/// while the cursor is in the nonclient area of a window. This message is posted to the window
		/// that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCXBUTTONDOWN = 0x00AB;

		/// <summary>
		/// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while
		/// the cursor is in the nonclient area of a window. This message is posted to the window that
		/// contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		public const int WM_NCXBUTTONUP = 0x00AC;

		/// <summary>
		/// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is
		/// pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed. 
		/// </summary>
		public const int WM_KEYDOWN = 0x0100;

		/// <summary>
		/// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is
		/// released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a
		/// keyboard key that is pressed when a window has the keyboard focus.
		/// </summary>
		public const int WM_KEYUP = 0x0101;

		/// <summary>
		/// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is
		/// translated by the TranslateMessage function. The WM_CHAR message contains the character code of
		/// the key that was pressed.
		/// </summary>
		public const int WM_CHAR = 0x0102;

		/// <summary>
		/// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message
		/// is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated
		/// by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot),
		/// that is combined with another character to form a composite character. For example, the umlaut-O
		/// character (O) is generated by typing the dead key for the umlaut character, and then typing the
		/// O key.
		/// </summary>
		public const int WM_DEADCHAR = 0x0103;

		/// <summary>
		/// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses
		/// the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key.
		/// It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN
		/// message is sent to the active window. The window that receives the message can distinguish
		/// between these two contexts by checking the context code in the lParam parameter.
		/// </summary>
		public const int WM_SYSKEYDOWN = 0x0104;

		/// <summary>
		/// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases
		/// a key that was pressed while the ALT key was held down. It also occurs when no window currently
		/// has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window.
		/// The window that receives the message can distinguish between these two contexts by checking
		/// the context code in the lParam parameter. 
		/// </summary>
		public const int WM_SYSKEYUP = 0x0105;

		/// <summary>
		/// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN
		/// message is translated by the TranslateMessage function. It specifies the character code of a
		/// system character key — that is, a character key that is pressed while the ALT key is down. 
		/// </summary>
		public const int WM_SYSCHAR = 0x0106;

		/// <summary>
		/// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN
		/// message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character
		/// code of a system dead key — that is, a dead key that is pressed while holding down the ALT key.
		/// </summary>
		public const int WM_SYSDEADCHAR = 0x0107;

		/// <summary>
		/// This message filters for keyboard messages.
		/// </summary>
		public const int WM_KEYLAST = 0x0108;

		/// <summary>
		/// The WM_IME_STARTCOMPOSITION message is sent immediately before the IME generates the
		/// composition string as a result of a keystroke. The message is a notification to an IME window
		/// to open its composition window. An application should process this message if it displays
		/// composition characters itself.
		/// </summary>
		public const int WM_IME_STARTCOMPOSITION = 0x010D;

		/// <summary>
		/// The WM_IME_ENDCOMPOSITION message is sent to an application when the IME ends composition.
		/// An application should process this message if it displays composition characters itself.
		/// </summary>
		public const int WM_IME_ENDCOMPOSITION = 0x010E;

		/// <summary>
		/// The WM_IME_COMPOSITION message is sent to an application when the IME changes composition
		/// status as a result of a keystroke. An application should process this message if it displays
		/// composition characters itself. Otherwise, it should send the message to the IME window.
		/// </summary>
		public const int WM_IME_COMPOSITION = 0x010F;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_IME_KEYLAST = 0x010F;

		/// <summary>
		/// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box
		/// is displayed. Dialog box procedures typically use this message to initialize controls and carry
		/// out any other initialization tasks that affect the appearance of the dialog box.
		/// </summary>
		public const int WM_INITDIALOG = 0x0110;

		/// <summary>
		/// The WM_COMMAND message is sent when the user selects a command item from a menu, when a
		/// control sends a notification message to its parent window, or when an accelerator keystroke
		/// is translated.
		/// </summary>
		public const int WM_COMMAND = 0x0111;

		/// <summary>
		/// A window receives this message when the user chooses a command from the Window menu
		/// (formerly known as the system or control menu) or when the user chooses the maximize button,
		/// minimize button, restore button, or close button.
		/// </summary>
		public const int WM_SYSCOMMAND = 0x0112;

		/// <summary>
		/// The WM_TIMER message is posted to the installing thread's message queue when a timer expires.
		/// The message is posted by the GetMessage or PeekMessage function.
		/// </summary>
		public const int WM_TIMER = 0x0113;

		/// <summary>
		/// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard
		/// horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control
		/// when a scroll event occurs in the control.
		/// </summary>
		public const int WM_HSCROLL = 0x0114;

		/// <summary>
		/// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard
		/// vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control
		/// when a scroll event occurs in the control.
		/// </summary>
		public const int WM_VSCROLL = 0x0115;

		/// <summary>
		/// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user
		/// clicks an item on the menu bar or presses a menu key. This allows the application to modify the
		/// menu before it is displayed.
		/// </summary>
		public const int WM_INITMENU = 0x0116;

		/// <summary>
		/// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active.
		/// This allows an application to modify the menu before it is displayed, without changing the
		/// entire menu. 
		/// </summary>
		public const int WM_INITMENUPOPUP = 0x0117;

		/// <summary>
		/// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item.
		/// </summary>
		public const int WM_MENUSELECT = 0x011F;

		/// <summary>
		/// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that
		/// does not correspond to any mnemonic or accelerator key. This message is sent to the window
		/// that owns the menu.
		/// </summary>
		public const int WM_MENUCHAR = 0x0120;

		/// <summary>
		/// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is
		/// entering an idle state. A modal dialog box or menu enters an idle state when no messages are
		/// waiting in its queue after it has processed one or more previous messages.
		/// </summary>
		public const int WM_ENTERIDLE = 0x0121;

		/// <summary>
		/// The WM_MENURBUTTONUP message is sent when the user releases the right mouse
		/// button while the cursor is on a menu item.
		/// </summary>
		public const int WM_MENURBUTTONUP = 0x0122;

		/// <summary>
		/// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the
		/// user drags a menu item. 
		/// </summary>
		public const int WM_MENUDRAG = 0x0123;

		/// <summary>
		/// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse
		/// cursor enters a menu item or moves from the center of the item to the top or bottom of the item.
		/// </summary>
		public const int WM_MENUGETOBJECT = 0x0124;

		/// <summary>
		/// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed.
		/// </summary>
		public const int WM_UNINITMENUPOPUP = 0x0125;

		/// <summary>
		/// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu.
		/// </summary>
		public const int WM_MENUCOMMAND = 0x0126;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_CTLCOLORMSGBOX = 0x0132;

		/// <summary>
		/// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its
		/// parent window when the control is about to be drawn. By responding to this message, the parent
		/// window can use the specified device context handle to set the text and background colors of
		/// the edit control. 
		/// </summary>
		public const int WM_CTLCOLOREDIT = 0x0133;

		/// <summary>
		/// The WM_CTLCOLORLISTBOX message is sent to the parent window of a list box before the system
		/// draws the list box. By responding to this message, the parent window can set the text and
		/// background colors of the list box by using the specified display device context handle.
		/// </summary>
		public const int WM_CTLCOLORLISTBOX = 0x0134;

		/// <summary>
		/// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button.
		/// The parent window can change the button's text and background colors. However, only owner-drawn
		/// buttons respond to the parent window processing this message.
		/// </summary>
		public const int WM_CTLCOLORBTN = 0x0135;

		/// <summary>
		/// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box.
		/// By responding to this message, the dialog box can set its text and background colors using
		/// the specified display device context handle. 
		/// </summary>
		public const int WM_CTLCOLORDLG = 0x0136;

		/// <summary>
		/// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when
		/// the control is about to be drawn. By responding to this message, the parent window can use
		/// the display context handle to set the background color of the scroll bar control.
		/// </summary>
		public const int WM_CTLCOLORSCROLLBAR = 0x0137;

		/// <summary>
		/// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC
		/// message to its parent window when the control is about to be drawn. By responding to this
		/// message, the parent window can use the specified device context handle to set the text and
		/// background colors of the static control. 
		/// </summary>
		public const int WM_CTLCOLORSTATIC = 0x0138;

		/// <summary>
		/// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not
		/// captured, the message is posted to the window that contains the cursor. Otherwise, the message
		/// is posted to the window that has captured the mouse.
		/// </summary>
		public const int WM_MOUSEMOVE = 0x0200;

		/// <summary>
		/// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the
		/// cursor is in the client area of a window. If the mouse is not captured, the message is posted
		/// to the window beneath the cursor. Otherwise, the message is posted to the window that has
		/// captured the mouse.
		/// </summary>
		public const int WM_LBUTTONDOWN = 0x0201;

		/// <summary>
		/// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the
		/// cursor is in the client area of a window. If the mouse is not captured, the message is posted
		/// to the window beneath the cursor. Otherwise, the message is posted to the window that has
		/// captured the mouse.
		/// </summary>
		public const int WM_LBUTTONUP = 0x0202;

		/// <summary>
		/// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button
		/// while the cursor is in the client area of a window. If the mouse is not captured, the message
		/// is posted to the window beneath the cursor. Otherwise, the message is posted to the window
		/// that has captured the mouse.
		/// </summary>
		public const int WM_LBUTTONDBLCLK = 0x0203;

		/// <summary>
		/// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the
		/// cursor is in the client area of a window. If the mouse is not captured, the message is posted
		/// to the window beneath the cursor. Otherwise, the message is posted to the window that has
		/// captured the mouse.
		/// </summary>
		public const int WM_RBUTTONDOWN = 0x0204;

		/// <summary>
		/// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the
		/// cursor is in the client area of a window. If the mouse is not captured, the message is posted
		/// to the window beneath the cursor. Otherwise, the message is posted to the window that has
		/// captured the mouse.
		/// </summary>
		public const int WM_RBUTTONUP = 0x0205;

		/// <summary>
		/// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button
		/// while the cursor is in the client area of a window. If the mouse is not captured, the message
		/// is posted to the window beneath the cursor. Otherwise, the message is posted to the window that
		/// has captured the mouse.
		/// </summary>
		public const int WM_RBUTTONDBLCLK = 0x0206;

		/// <summary>
		/// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the
		/// cursor is in the client area of a window. If the mouse is not captured, the message is posted to
		/// the window beneath the cursor. Otherwise, the message is posted to the window that has captured
		/// the mouse.
		/// </summary>
		public const int WM_MBUTTONDOWN = 0x0207;

		/// <summary>
		/// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the
		/// cursor is in the client area of a window. If the mouse is not captured, the message is posted
		/// to the window beneath the cursor. Otherwise, the message is posted to the window that has
		/// captured the mouse.
		/// </summary>
		public const int WM_MBUTTONUP = 0x0208;

		/// <summary>
		/// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button
		/// while the cursor is in the client area of a window. If the mouse is not captured, the message
		/// is posted to the window beneath the cursor. Otherwise, the message is posted to the window
		/// that has captured the mouse.
		/// </summary>
		public const int WM_MBUTTONDBLCLK = 0x0209;

		/// <summary>
		/// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The
		/// DefWindowProc function propagates the message to the window's parent. There should be no
		/// internal forwarding of the message, since DefWindowProc propagates it up the parent chainuntil
		/// it finds a window that processes it.
		/// </summary>
		public const int WM_MOUSEWHEEL = 0x020A;

		/// <summary>
		/// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button
		/// while the cursor is in the client area of a window. If the mouse is not captured, the message
		/// is posted to the window beneath the cursor. Otherwise, the message is posted to the window that
		/// has captured the mouse.
		/// </summary>
		public const int WM_XBUTTONDOWN = 0x020B;

		/// <summary>
		/// The WM_XBUTTONUP message is posted when the user releases the first or second X button while
		/// the cursor is in the client area of a window. If the mouse is not captured, the message is
		/// posted to the window beneath the cursor. Otherwise, the message is posted to the window that
		/// has captured the mouse.
		/// </summary>
		public const int WM_XBUTTONUP = 0x020C;

		/// <summary>
		/// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button
		/// while the cursor is in the client area of a window. If the mouse is not captured, the message
		/// is posted to the window beneath the cursor. Otherwise, the message is posted to the window that
		/// has captured the mouse.
		/// </summary>
		public const int WM_XBUTTONDBLCLK = 0x020D;

		/// <summary>
		/// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window
		/// is created or destroyed, or when the user clicks a mouse button while the cursor is over the
		/// child window. When the child window is being created, the system sends WM_PARENTNOTIFY just
		/// before the CreateWindow or CreateWindowEx function that creates the window returns. When the
		/// child window is being destroyed, the system sends the message before any processing to destroy
		/// the window takes place.
		/// </summary>
		public const int WM_PARENTNOTIFY = 0x0210;

		/// <summary>
		/// The WM_ENTERMENULOOP message informs an application's main window
		/// procedure that a menu modal loop has been entered.
		/// </summary>
		public const int WM_ENTERMENULOOP = 0x0211;

		/// <summary>
		/// The WM_EXITMENULOOP message informs an application's main window procedure
		/// that a menu modal loop has been exited.
		/// </summary>
		public const int WM_EXITMENULOOP = 0x0212;

		/// <summary>
		/// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used 
		/// to switch between the menu bar and the system menu.
		/// </summary>
		public const int WM_NEXTMENU = 0x0213;

		/// <summary>
		/// The WM_SIZING message is sent to a window that the user is resizing. By processing this
		/// message, an application can monitor the size and position of the drag rectangle and, if needed,
		/// change its size or position.
		/// </summary>
		public const int WM_SIZING = 0x0214;

		/// <summary>
		/// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.
		/// </summary>
		public const int WM_CAPTURECHANGED = 0x0215;

		/// <summary>
		/// The WM_MOVING message is sent to a window that the user is moving. By processing this message,
		/// an application can monitor the position of the drag rectangle and, if needed, change its position.
		/// </summary>
		public const int WM_MOVING = 0x0216;

		/// <summary>
		/// The WM_DEVICECHANGE device message notifies an application of a change to
		/// the hardware configuration of a device or the computer.
		/// </summary>
		public const int WM_DEVICECHANGE = 0x0219;

		/// <summary>
		/// An application sends the WM_MDICREATE message to a multiple-document interface (MDI)
		/// client window to create an MDI child window.
		/// </summary>
		public const int WM_MDICREATE = 0x0220;

		/// <summary>
		/// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client
		/// window to close an MDI child window.
		/// </summary>
		public const int WM_MDIDESTROY = 0x0221;

		/// <summary>
		/// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window
		/// to instruct the client window to activate a different MDI child window.
		/// </summary>
		public const int WM_MDIACTIVATE = 0x0222;

		/// <summary>
		/// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window
		/// to restore an MDI child window from maximized or minimized size.
		/// </summary>
		public const int WM_MDIRESTORE = 0x0223;

		/// <summary>
		/// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client
		/// window to activate the next or previous child window. 
		/// </summary>
		public const int WM_MDINEXT = 0x0224;

		/// <summary>
		/// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client
		/// window to maximize an MDI child window. The system resizes the child window to make its client
		/// area fill the client window. The system places the child window's window menu icon in the
		/// rightmost position of the frame window's menu bar, and places the child window's restore icon
		/// in the leftmost position. The system also appends the title bar text of the child window to that
		/// of the frame window.
		/// </summary>
		public const int WM_MDIMAXIMIZE = 0x0225;

		/// <summary>
		/// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client
		/// window to arrange all of its MDI child windows in a tile format.
		/// </summary>
		public const int WM_MDITILE = 0x0226;

		/// <summary>
		/// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI)
		/// client window to arrange all its child windows in a cascade format.
		/// </summary>
		public const int WM_MDICASCADE = 0x0227;

		/// <summary>
		/// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI)
		/// client window to arrange all minimized MDI child windows. It does not affect child windows
		/// that are not minimized.
		/// </summary>
		public const int WM_MDIICONARRANGE = 0x0228;

		/// <summary>
		/// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI)
		/// client window to retrieve the handle to the active MDI child window.
		/// </summary>
		public const int WM_MDIGETACTIVE = 0x0229;

		/// <summary>
		/// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client
		/// window to replace the entire menu of an MDI frame window, to replace the window menu of the
		/// frame window, or both.
		/// </summary>
		public const int WM_MDISETMENU = 0x0230;

		/// <summary>
		/// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or
		/// sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the
		/// window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the
		/// DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE
		/// value. The operation is complete when DefWindowProc returns.
		/// </summary>
		public const int WM_ENTERSIZEMOVE = 0x0231;

		/// <summary>
		/// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or
		/// sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the
		/// window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the
		/// DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE
		/// value. The operation is complete when DefWindowProc returns.
		/// </summary>
		public const int WM_EXITSIZEMOVE = 0x0232;

		/// <summary>
		/// Sent when the user drops a file on the window of an application that has registered
		/// itself as a recipient of dropped files.
		/// </summary>
		public const int WM_DROPFILES = 0x0233;

		/// <summary>
		/// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI)
		/// client window to refresh the window menu of the MDI frame window.
		/// </summary>
		public const int WM_MDIREFRESHMENU = 0x0234;

		/// <summary>
		/// The WM_IME_SETCONTEXT message is sent to an application when a window of the application is
		/// activated. If the application has created an IME window, it should call the ImmIsUIMessage
		/// function. Otherwise, it should pass this message to the DefWindowProc function.
		/// </summary>
		public const int WM_IME_SETCONTEXT = 0x0281;

		/// <summary>
		/// The WM_IME_NOTIFY message is sent to an application to notify it of changes to the IME window.
		/// An application processes this message if it is responsible for managing the IME window.
		/// </summary>
		public const int WM_IME_NOTIFY = 0x0282;

		/// <summary>
		/// The WM_IME_CONTROL message directs the IME window to carry out the requested command.
		/// An application uses this message to control the IME window created by the application.
		/// </summary>
		public const int WM_IME_CONTROL = 0x0283;

		/// <summary>
		/// The WM_IME_COMPOSITIONFULL message is sent to an application when the IME window finds no
		/// space to extend the area for the composition window. The application should use the
		/// IMC_SETCOMPOSITIONWINDOW command to specify how the window should be displayed.
		/// </summary>
		public const int WM_IME_COMPOSITIONFULL = 0x0284;

		/// <summary>
		/// The WM_IME_SELECT message is sent to an application when the system is about to change the
		/// current IME. An application that has created an IME window should pass this message to that
		/// window so that it can retrieve the keyboard layout handle for the newly selected IME.
		/// </summary>
		public const int WM_IME_SELECT = 0x0285;

		/// <summary>
		/// The WM_IME_CHAR message is sent to an application when the IME gets a character of the
		/// conversion result. Unlike the WM_CHAR message for a non-Unicode window, this message can include
		/// double-byte as well as single-byte character values. For a Unicode window, this message is the
		/// same as WM_CHAR.
		/// </summary>
		public const int WM_IME_CHAR = 0x0286;

		/// <summary>
		/// The WM_IME_REQUEST message provides a group of commands to request information
		/// from an application.
		/// </summary>
		public const int WM_IME_REQUEST = 0x0288;

		/// <summary>
		/// The WM_IME_KEYDOWN message is sent to an application by the IME to notify the application of
		/// a key press. An application can process this message or pass it to the DefWindowProc function
		/// to generate a matching WM_KEYDOWN message. This message is usually generated by the IME to keep
		/// message order.
		/// </summary>
		public const int WM_IME_KEYDOWN = 0x0290;

		/// <summary>
		/// The WM_IME_KEYUP message is sent to an application by the IME to notify the application of a key
		/// release. An application can process this message or pass it to the DefWindowProc function to
		/// generate a matching WM_KEYUP message. This message is usually generated by the IME to keep
		/// message order.
		/// </summary>
		public const int WM_IME_KEYUP = 0x0291;

		/// <summary>
		/// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client
		/// area of the window for the period of time specified in a prior call to TrackMouseEvent.
		/// </summary>
		public const int WM_MOUSEHOVER = 0x02A1;

		/// <summary>
		/// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of
		/// the window specified in a prior call to TrackMouseEvent.
		/// </summary>
		public const int WM_MOUSELEAVE = 0x02A3;

		/// <summary>
		/// An application sends a WM_CUT message to an edit control or combo box to delete (cut) the
		/// current selection, if any, in the edit control and copy the deleted text to the clipboard in
		/// CF_TEXT format.
		/// </summary>
		public const int WM_CUT = 0x0300;

		/// <summary>
		/// An application sends the WM_COPY message to an edit control or combo box to copy the current
		/// selection to the clipboard in CF_TEXT format. 
		/// </summary>
		public const int WM_COPY = 0x0301;

		/// <summary>
		/// An application sends a WM_PASTE message to an edit control or combo box to copy the current
		/// content of the clipboard to the edit control at the current caret position. Data is inserted
		/// only if the clipboard contains data in CF_TEXT format. 
		/// </summary>
		public const int WM_PASTE = 0x0302;

		/// <summary>
		/// An application sends a WM_CLEAR message to an edit control or combo box to delete (clear)
		/// the current selection, if any, from the edit control.
		/// </summary>
		public const int WM_CLEAR = 0x0303;

		/// <summary>
		/// An application sends a WM_UNDO message to an edit control to undo the last operation.
		/// When this message is sent to an edit control, the previously deleted text is restored or the
		/// previously added text is deleted. 
		/// </summary>
		public const int WM_UNDO = 0x0304;

		/// <summary>
		/// The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific
		/// clipboard format and if an application has requested data in that format. The clipboard owner
		/// must render data in the specified format and place it on the clipboard by calling the
		/// SetClipboardData function.
		/// </summary>
		public const int WM_RENDERFORMAT = 0x0305;

		/// <summary>
		/// The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed, if the
		/// clipboard owner has delayed rendering one or more clipboard formats. For the content of the
		/// clipboard to remain available to other applications, the clipboard owner must render data in
		/// all the formats it is capable of generating, and place the data on the clipboard by calling the
		/// SetClipboardData function. 
		/// </summary>
		public const int WM_RENDERALLFORMATS = 0x0306;

		/// <summary>
		/// The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the
		/// EmptyClipboard function empties the clipboard.
		/// </summary>
		public const int WM_DESTROYCLIPBOARD = 0x0307;

		/// <summary>
		/// The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the
		/// content of the clipboard changes. This enables a clipboard viewer window to display the new
		/// content of the clipboard.
		/// </summary>
		public const int WM_DRAWCLIPBOARD = 0x0308;

		/// <summary>
		/// The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when
		/// the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area
		/// needs repainting.
		/// </summary>
		public const int WM_PAINTCLIPBOARD = 0x0309;

		/// <summary>
		/// The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window
		/// when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the
		/// clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update
		/// the scroll bar values.
		/// </summary>
		public const int WM_VSCROLLCLIPBOARD = 0x030A;

		/// <summary>
		/// The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window
		/// when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's
		/// client area has changed size.
		/// </summary>
		public const int WM_SIZECLIPBOARD = 0x030B;

		/// <summary>
		/// The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window
		/// to request the name of a CF_OWNERDISPLAY clipboard format.
		/// </summary>
		public const int WM_ASKCBFORMATNAME = 0x030C;

		/// <summary>
		/// The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when
		/// a window is being removed from the chain. 
		/// </summary>
		public const int WM_CHANGECBCHAIN = 0x030D;

		/// <summary>
		/// The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window.
		/// This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs
		/// in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image
		/// and update the scroll bar values.
		/// </summary>
		public const int WM_HSCROLLCLIPBOARD = 0x030E;

		/// <summary>
		/// The WM_QUERYNEWPALETTE message informs a window that it is about to receive the keyboard focus,
		/// giving the window the opportunity to realize its logical palette when it receives the focus.
		/// </summary>
		public const int WM_QUERYNEWPALETTE = 0x030F;

		/// <summary>
		/// The WM_PALETTEISCHANGING message informs applications that an application is going to realize
		/// its logical palette.
		/// </summary>
		public const int WM_PALETTEISCHANGING = 0x0310;

		/// <summary>
		/// The WM_PALETTECHANGED message is sent to all top-level and overlapped windows after the window
		/// with the keyboard focus has realized its logical palette, thereby changing the system palette.
		/// This message enables a window that uses a color palette but does not have the keyboard focus to
		/// realize its logical palette and update its client area.
		/// </summary>
		public const int WM_PALETTECHANGED = 0x0311;

		/// <summary>
		/// The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey
		/// function. The message is placed at the top of the message queue associated with the thread that
		/// registered the hot key.
		/// </summary>
		public const int WM_HOTKEY = 0x0312;

		/// <summary>
		/// The WM_PRINT message is sent to a window to request that it draw itself in the specified device
		/// context, most commonly in a printer device context.
		/// </summary>
		public const int WM_PRINT = 0x0317;

		/// <summary>
		/// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the
		/// specified device context, most commonly in a printer device context.
		/// </summary>
		public const int WM_PRINTCLIENT = 0x0318;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_HANDHELDFIRST = 0x0358;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_HANDHELDLAST = 0x035F;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_AFXFIRST = 0x0360;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_AFXLAST = 0x037F;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_PENWINFIRST = 0x0380;

		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int WM_PENWINLAST = 0x038F;

		/// <summary>
		/// The WM_APP constant is used by applications to help define private messages, usually of the
		/// form WM_APP+X, where X is an integer value.
		/// </summary>
		public const int WM_APP = 0x8000;

		/// <summary>
		/// The WM_USER constant is used by applications to help define private messages for use by private
		/// window classes, usually of the form WM_USER+X, where X is an integer value.
		/// </summary>
		public const int WM_USER = 0x0400;

		/// <summary>
		/// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the
		/// window specified in a prior call to TrackMouseEvent.
		/// <paramref name="wParam"/> Not used, must be zero.
		/// <paramref name="lParam"/> Not used, must be zero.
		/// </summary>
		public const int WM_NCMOUSELEAVE = 0x02A2;

		/// <summary>
		/// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the
		/// window for the period of time specified in a prior call to TrackMouseEvent.
		/// <paramref name="wParam"/> Specifies the hit-test value returned by the DefWindowProc function as a result
		/// of processing the WM_NCHITTEST message. For a list of hit-test values, see WM_NCHITTEST.
		/// <paramref name="lParam"/> Specifies a POINTS structure that contains the x- and y-coordinates of the cursor.
		/// The coordinates are relative to the upper-left corner of the screen.
		/// </summary>
		public const int WM_NCMOUSEHOVER = 0x02A0;

		#endregion

		#region Window Styles

		/// <summary>
		/// Creates a window that has a thin-line border.
		/// </summary>
		public const int WS_BORDER = 0x00800000;

		/// <summary>
		/// Creates a window that has a title bar (includes the WS_BORDER style).
		/// </summary>
		public const int WS_CAPTION = 0x00C00000;    /* WS_BORDER | WS_DLGFRAME  */

		/// <summary>
		/// Creates a child window. A window with this style cannot have a menu bar.
		/// This style cannot be used with the WS_POPUP style.
		/// </summary>
		public const int WS_CHILD = 0x40000000;

		/// <summary>
		/// Same as the WS_CHILD style.
		/// </summary>
		public const int WS_CHILDWINDOW = WS_CHILD;

		/// <summary>
		/// Excludes the area occupied by child windows when drawing occurs
		/// within the parent window. This style is used when creating the parent window.
		/// </summary>
		public const int WS_CLIPCHILDREN = 0x02000000;

		/// <summary>
		/// Clips child windows relative to each other; that is, when a particular child window receives a
		/// WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the
		/// region of the child window to be updated. If WS_CLIPSIBLINGS is not specified and child windows
		/// overlap, it is possible, when drawing within the client area of a child window, to draw within
		/// the client area of a neighboring child window.
		/// </summary>
		public const int WS_CLIPSIBLINGS = 0x04000000;

		/// <summary>
		/// Creates a window that is initially disabled. A disabled window cannot receive input from the user.
		/// To change this after a window has been created, use EnableWindow.
		/// </summary>
		public const int WS_DISABLED = 0x08000000;

		/// <summary>
		/// Creates a window that has a border of a style typically used with dialog boxes.
		/// A window with this style cannot have a title bar.
		/// </summary>
		public const int WS_DLGFRAME = 0x00400000;

		/// <summary>
		/// Specifies the first control of a group of controls. The group consists of this first control and
		/// all controls defined after it, up to the next control with the WS_GROUP style. The first control
		/// in each group usually has the WS_TABSTOP style so that the user can move from group to group.
		/// The user can subsequently change the keyboard focus from one control in the group to the next
		/// control in the group by using the direction keys.
		/// </summary>
		public const int WS_GROUP = 0x00020000;

		/// <summary>
		/// Creates a window that has a horizontal scroll bar.
		/// </summary>
		public const int WS_HSCROLL = 0x00100000;

		/// <summary>
		/// Creates a window that is initially minimized. Same as the WS_MINIMIZE style.
		/// </summary>
		public const int WS_ICONIC = WS_MINIMIZE;

		/// <summary>
		/// Creates a window that is initially maximized.
		/// </summary>
		public const int WS_MAXIMIZE = 0x01000000;

		/// <summary>
		/// Creates a window that has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP
		/// style. The WS_SYSMENU style must also be specified.
		/// </summary>
		public const int WS_MAXIMIZEBOX = 0x00010000;

		/// <summary>
		/// Creates a window that is initially minimized. Same as the WS_ICONIC style.
		/// </summary>
		public const int WS_MINIMIZE = 0x20000000;

		/// <summary>
		/// Creates a window that has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP
		/// style. The WS_SYSMENU style must also be specified.
		/// </summary>
		public const int WS_MINIMIZEBOX = 0x00020000;

		/// <summary>
		/// Creates an overlapped window. An overlapped window has a title bar and a border.
		/// Same as the WS_TILED style.
		/// </summary>
		public const int WS_OVERLAPPED = 0x00000000;

		/// <summary>
		/// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU, WS_THICKFRAME,
		/// WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the WS_TILEDWINDOW style.
		/// </summary>
		public const int WS_OVERLAPPEDWINDOW = WS_OVERLAPPED |
			WS_CAPTION |
			WS_SYSMENU |
			WS_THICKFRAME |
			WS_MINIMIZEBOX |
			WS_MAXIMIZEBOX;

		/// <summary>
		/// Creates a pop-up window. This style cannot be used with the WS_CHILD style.
		/// </summary>
		public const uint WS_POPUP = 0x80000000;

		/// <summary>
		/// Creates a pop-up window with WS_BORDER, WS_POPUP, and WS_SYSMENU styles. The WS_CAPTION and
		/// WS_POPUPWINDOW styles must be combined to make the window menu visible.
		/// </summary>
		public const uint WS_POPUPWINDOW = WS_POPUP |
			WS_BORDER |
			WS_SYSMENU;

		/// <summary>
		/// Creates a window that has a sizing border. Same as the WS_THICKFRAME style.
		/// </summary>
		public const int WS_SIZEBOX = WS_THICKFRAME;

		/// <summary>
		/// Creates a window that has a window menu on its title bar.
		/// The WS_CAPTION style must also be specified.
		/// </summary>
		public const int WS_SYSMENU = 0x00080000;

		/// <summary>
		/// Specifies a control that can receive the keyboard focus when the user presses the TAB key.
		/// Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
		/// </summary>
		public const int WS_TABSTOP = 0x00010000;

		/// <summary>
		/// Creates a window that has a sizing border. Same as the WS_SIZEBOX style.
		/// </summary>
		public const int WS_THICKFRAME = 0x00040000;

		/// <summary>
		/// Creates an overlapped window. An overlapped window has a title bar and a border.
		/// Same as the WS_OVERLAPPED style.
		/// </summary>
		public const int WS_TILED = WS_OVERLAPPED;

		/// <summary>
		/// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU, WS_THICKFRAME,
		/// WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the WS_OVERLAPPEDWINDOW style.
		/// </summary>
		public const int WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;

		/// <summary>
		/// Creates a window that is initially visible.
		/// </summary>
		public const int WS_VISIBLE = 0x10000000;

		/// <summary>
		/// Creates a window that has a vertical scroll bar.
		/// </summary>
		public const int WS_VSCROLL = 0x00200000;

		#endregion

		#region WM_MOUSEACTIVATE Return Codes

		/// <summary>
		/// Activates the window, and does not discard the mouse message.
		/// </summary>
		public const int MA_ACTIVATE = 1;

		/// <summary>
		/// Activates the window, and discards the mouse message.
		/// </summary>
		public const int MA_ACTIVATEANDEAT = 2;

		/// <summary>
		/// Does not activate the window, and does not discard the mouse message.
		/// </summary>
		public const int MA_NOACTIVATE = 3;

		/// <summary>
		/// Does not activate the window, but discards the mouse message.
		/// </summary>
		public const int MA_NOACTIVATEANDEAT = 4;

		#endregion

		#region WM_NCCALCSIZE "window valid rect" return values

		/// <summary>
		/// Specifies that the client area of the window is to be preserved and aligned with the top of the 
		/// new position of the window. For example, to align the client area to the upper-left corner, 
		/// return the WVR_ALIGNTOP and WVR_ALIGNLEFT values.
		/// </summary>
		public const int WVR_ALIGNTOP = 0x0010;

		/// <summary>
		/// Specifies that the client area of the window is to be preserved and aligned with the left side 
		/// of the new position of the window. For example, to align the client area to the lower-left 
		/// corner, return the WVR_ALIGNLEFT and WVR_ALIGNBOTTOM values.
		/// </summary>
		public const int WVR_ALIGNLEFT = 0x0020;

		/// <summary>
		/// Specifies that the client area of the window is to be preserved and aligned with the bottom of 
		/// the new position of the window. For example, to align the client area to the top-left corner, 
		/// return the WVR_ALIGNTOP and WVR_ALIGNLEFT values.
		/// </summary>
		public const int WVR_ALIGNBOTTOM = 0x0040;

		/// <summary>
		/// Specifies that the client area of the window is to be preserved and aligned with the right side
		/// of the new position of the window. For example, to align the client area to the lower-right
		/// corner, return the WVR_ALIGNRIGHT and WVR_ALIGNBOTTOM values.
		/// </summary>
		public const int WVR_ALIGNRIGHT = 0x0080;

		/// <summary>
		/// Used in combination with any other values, causes the window to be completely redrawn if the 
		/// client rectangle changes size horizontally. This value is similar to CS_HREDRAW class style.
		/// </summary>
		public const int WVR_HREDRAW = 0x0100;

		/// <summary>
		/// Used in combination with any other values, causes the window to be completely redrawn if the 
		/// client rectangle changes size vertically. This value is similar to CS_VREDRAW class style.
		/// </summary>
		public const int WVR_VREDRAW = 0x0200;

		/// <summary>
		/// This value causes the entire window to be redrawn. 
		/// It is a combination of WVR_HREDRAW and WVR_VREDRAW values.
		/// </summary>
		public const int WVR_REDRAW = WVR_HREDRAW | WVR_VREDRAW;

		/// <summary>
		/// This value indicates that, upon return from WM_NCCALCSIZE, the rectangles specified by the 
		/// rgrc[1] and rgrc[2] members of the NCCALCSIZE_PARAMS structure contain valid destination and 
		/// source area rectangles, respectively. The system combines these rectangles to calculate the
		/// area of the window to be preserved. The system copies any part of the window image that is 
		/// within the source rectangle and clips the image to the destination rectangle. Both rectangles 
		/// are in parent-relative or screen-relative coordinates. This return value allows an application 
		/// to implement more elaborate client-area preservation strategies, such as centering or preserving
		/// a subset of the client area.
		/// </summary>
		public const int WVR_VALIDRECTS = 0x0400;

		#endregion

		#region WM_PRINT flags

		/// <summary>
		/// Draws the window only if it is visible.
		/// </summary>
		public const int PRF_CHECKVISIBLE = 0x00000001;

		/// <summary>
		/// Draws the nonclient area of the window.
		/// </summary>
		public const int PRF_NONCLIENT = 0x00000002;

		/// <summary>
		/// Draws the client area of the window.
		/// </summary>
		public const int PRF_CLIENT = 0x00000004;

		/// <summary>
		/// Erases the background before drawing the window.
		/// </summary>
		public const int PRF_ERASEBKGND = 0x00000008;

		/// <summary>
		/// Draws all visible children windows.
		/// </summary>
		public const int PRF_CHILDREN = 0x00000010;

		/// <summary>
		/// Draws all owned windows.
		/// </summary>
		public const int PRF_OWNED = 0x00000020;

		#endregion

		#region TrackMouseEvent Flags
		
		/// <summary>
		/// The caller wants hover notification. Notification is delivered as a WM_MOUSEHOVER message.
		/// If the caller requests hover tracking while hover tracking is already active, the hover timer will be reset.
		/// This flag is ignored if the mouse pointer is not over the specified window or area.
		/// </summary>
		public const uint TME_HOVER = 0x00000001;
		
		/// <summary>
		/// The caller wants leave notification. Notification is delivered as a WM_MOUSELEAVE message. If the mouse is not over the specified window or area, a leave notification is generated immediately and no further tracking is performed.
		/// </summary>
		public const uint TME_LEAVE = 0x00000002;
		
		/// <summary>
		/// The caller wants hover and leave notification for the nonclient areas. Notification is delivered as WM_NCMOUSEHOVER and WM_NCMOUSELEAVE messages.
		/// </summary>
		public const uint TME_NONCLIENT = 0x00000010;
		
		/// <summary>
		/// The function fills in the structure instead of treating it as a tracking request. The structure is filled such that had that structure been passed to TrackMouseEvent, it would generate the current tracking. The only anomaly is that the hover time-out returned is always the actual time-out and not HOVER_DEFAULT, if HOVER_DEFAULT was specified during the original TrackMouseEvent request.
		/// </summary>
		public const uint TME_QUERY = 0x40000000;
		
		/// <summary>
		/// The caller wants to cancel a prior tracking request. The caller should also specify the type of tracking that it wants to cancel. For example, to cancel hover tracking, the caller must pass the TME_CANCEL and TME_HOVER flags.
		/// </summary>
		public const uint TME_CANCEL = 0x80000000;

		#endregion
	}
}
