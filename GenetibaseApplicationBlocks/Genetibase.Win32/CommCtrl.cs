/* -----------------------------------------------
 * CommCtrl.cs
 * Copyright © 2005-2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines constants declared in CommCtrl.h.
	/// </summary>
	public static class CommCtrl
	{
		#region Custom Draw

		/// <summary>
		/// The control will draw itself. It will not send any additional NM_CUSTOMDRAW messages for this
		/// paint cycle. This occurs when dwDrawState equals CDDS_PREPAINT.
		/// </summary>
		public const int CDRF_DODEFAULT = 0x00000000;
		
		/// <summary>
		/// The application specified a new font for the item; the control will use the new font. For more
		/// information about changing fonts, see Changing fonts and colors. This occurs when dwDrawState
		/// equals CDDS_ITEMPREPAINT.
		/// </summary>
		public const int CDRF_NEWFONT = 0x00000002;
		
		/// <summary>
		/// The application drew the item manually. The control will not draw the item. This occurs when
		/// dwDrawState equals CDDS_ITEMPREPAINT.
		/// </summary>
		public const int CDRF_SKIPDEFAULT = 0x00000004;
		
		/// <summary>
		/// The control will notify the parent after painting an item.
		/// This occurs when dwDrawState equals CDDS_PREPAINT.
		/// </summary>
		public const int CDRF_NOTIFYPOSTPAINT = 0x00000010;
		
		/// <summary>
		/// The control will notify the parent of any item-related drawing operations. It will send
		/// NM_CUSTOMDRAW notification messages before and after drawing items. This occurs when dwDrawState
		/// equals CDDS_PREPAINT.
		/// </summary>
		public const int CDRF_NOTIFYITEMDRAW = 0x00000020;
		
		/// <summary>
		/// Your application will receive an NM_CUSTOMDRAW message with dwDrawState set to
		/// CDDS_ITEMPREPAINT | CDDS_SUBITEM before each list-view subitem is drawn. You can then specify
		/// font and color for each subitem separately or return CDRF_DODEFAULT for default processing.
		/// This occurs when dwDrawState equals CDDS_ITEMPREPAINT.
		/// </summary>
		public const int CDRF_NOTIFYSUBITEMDRAW = 0x00000020;
		
		/// <summary>
		/// The control will notify the parent after erasing an item. This occurs when dwDrawState equals CDDS_PREPAINT.
		/// </summary>
		public const int CDRF_NOTIFYPOSTERASE = 0x00000040;
		
		/// <summary>
		/// Before the paint cycle begins.
		/// </summary>
		public const int CDDS_PREPAINT = 0x00000001;
		
		/// <summary>
		/// After the paint cycle is complete.
		/// </summary>
		public const int CDDS_POSTPAINT = 0x00000002;
		
		/// <summary>
		/// Before the erase cycle begins. 
		/// </summary>
		public const int CDDS_PREERASE = 0x00000003;
		
		/// <summary>
		/// After the erase cycle is complete.
		/// </summary>
		public const int CDDS_POSTERASE = 0x00000004;
		
		/// <summary>
		/// Indicates that the dwItemSpec, uItemState, and lItemlParam members are valid.
		/// </summary>
		public const int CDDS_ITEM = 0x00010000;
		
		/// <summary>
		/// Before an item is drawn.
		/// </summary>
		public const int CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT);
		
		/// <summary>
		/// After an item has been drawn.
		/// </summary>
		public const int CDDS_ITEMPOSTPAINT = (CDDS_ITEM | CDDS_POSTPAINT);
		
		/// <summary>
		/// Before an item is erased.
		/// </summary>
		public const int CDDS_ITEMPREERASE = (CDDS_ITEM | CDDS_PREERASE);
		
		/// <summary>
		/// After an item has been erased.
		/// </summary>
		public const int CDDS_ITEMPOSTERASE = (CDDS_ITEM | CDDS_POSTERASE);
		
		/// <summary>
		/// Flag combined with CDDS_ITEMPREPAINT or CDDS_ITEMPOSTPAINT if a subitem is being drawn.
		/// This will only be set if CDRF_NOTIFYITEMDRAW is returned from CDDS_PREPAINT.
		/// </summary>
		public const int CDDS_SUBITEM = 0x00020000;
		
		/// <summary>
		/// The item is selected.
		/// </summary>
		public const int CDIS_SELECTED = 0x0001;
		
		/// <summary>
		/// The item is grayed.
		/// </summary>
		public const int CDIS_GRAYED = 0x0002;
		
		/// <summary>
		/// The item is disabled.
		/// </summary>
		public const int CDIS_DISABLED = 0x0004;
		
		/// <summary>
		/// The item is checked.
		/// </summary>
		public const int CDIS_CHECKED = 0x0008;
		
		/// <summary>
		/// The item is in focus.
		/// </summary>
		public const int CDIS_FOCUS = 0x0010;
		
		/// <summary>
		/// The item is in its default state.
		/// </summary>
		public const int CDIS_DEFAULT = 0x0020;
		
		/// <summary>
		/// The item is currently under the pointer ("hot").
		/// </summary>
		public const int CDIS_HOT = 0x0040;
		
		/// <summary>
		/// The item is marked. The meaning of this is up to the implementation.
		/// </summary>
		public const int CDIS_MARKED = 0x0080;
		
		/// <summary>
		/// The item is in an indeterminate state.
		/// </summary>
		public const int CDIS_INDETERMINATE = 0x0100;
		
		/// <summary>
		/// The item is a keyboard cue.
		/// </summary>
		public const int CDIS_SHOWKEYBOARDCUES = 0x0200;

		#endregion

		#region Generic WM_NOTIFY notification codes

		/// <summary>
		/// Notifies a control's parent window that the control could not complete an operation because
		/// there was not enough memory available. NM_OUTOFMEMORY is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_OUTOFMEMORY = (NM_FIRST - 1);
		
		/// <summary>
		/// Notifies a control's parent window that the user has clicked the left mouse button within the
		/// control. NM_CLICK is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_CLICK = (NM_FIRST - 2); 
		
		/// <summary>
		/// Notifies a control's parent window that the user has double-clicked the left mouse button within
		/// the control. NM_DBLCLK is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_DBLCLK = (NM_FIRST - 3);
		
		/// <summary>
		/// Notifies a control's parent window that the control has the input focus and that the user has
		/// pressed the ENTER key. NM_RETURN is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_RETURN = (NM_FIRST - 4);
		
		/// <summary>
		/// Notifies a control's parent window that the user has clicked the right mouse button within the
		/// control. NM_RCLICK is sent in the form of a WM_NOTIFY message. 
		/// </summary>
		public const int NM_RCLICK = (NM_FIRST - 5); 
		
		/// <summary>
		/// Notifies a control's parent window that the user has double-clicked the right mouse button
		/// within the control. NM_RDBLCLK is sent in the form of a WM_NOTIFY message. 
		/// </summary>
		public const int NM_RDBLCLK = (NM_FIRST - 6);
		
		/// <summary>
		/// Notifies a control's parent window that the control has received the input focus. NM_SETFOCUS
		/// is sent in the form of a WM_NOTIFY message. 
		/// </summary>
		public const int NM_SETFOCUS = (NM_FIRST - 7);
		
		/// <summary>
		/// Notifies a control's parent window that the control has lost the input focus. NM_KILLFOCUS is
		/// sent in the form of a WM_NOTIFY message. 
		/// </summary>
		public const int NM_KILLFOCUS = (NM_FIRST - 8);
		
		/// <summary>
		/// Sent by some common controls to notify their parent windows about drawing operations. This
		/// notification is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_CUSTOMDRAW = (NM_FIRST - 12);
		
		/// <summary>
		/// Sent by a control when the mouse hovers over an item. This notification message is sent in
		/// the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_HOVER = (NM_FIRST - 13);
		
		/// <summary>
		/// Sent by a control when the control receives a WM_NCHITTEST message. This notification message
		/// is sent in the form of a WM_NOTIFY message. 
		/// </summary>
		public const int NM_NCHITTEST = (NM_FIRST - 14); 
		
		/// <summary>
		/// Sent by a control when the control has the keyboard focus and the user presses a key. This
		/// notification message is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_KEYDOWN = (NM_FIRST - 15);
		
		/// <summary>
		/// Notifies a control's parent window that the control is releasing mouse capture. This
		/// notification is sent in the form of a WM_NOTIFY message. 
		/// </summary>
		public const int NM_RELEASEDCAPTURE = (NM_FIRST - 16);
		
		/// <summary>
		/// Notifies a control's parent window that the control is setting the cursor in response to a
		/// NM_SETCURSOR message. This notification is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_SETCURSOR = (NM_FIRST - 17);
		
		/// <summary>
		/// The NM_CHAR notification message is sent by a control when a character key is processed.
		/// This notification message is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_CHAR = (NM_FIRST - 18);
		
		/// <summary>
		/// Notifies a control's parent window that the control has created a ToolTip control.
		/// This notification is sent in the form of a WM_NOTIFY message.
		/// </summary>
		public const int NM_TOOLTIPSCREATED = (NM_FIRST - 19);
		
		/// <summary>
		/// Notifies a toolbar's parent window that the left mouse button has been pressed.
		/// </summary>
		public const int NM_LDOWN = (NM_FIRST - 20);
		
		/// <summary>
		/// Notifies a toolbar's parent window that the right mouse button has been pressed.
		/// </summary>
		public const int NM_RDOWN = (NM_FIRST - 21);
		
		/// <summary>
		/// No documentation available.
		/// </summary>
		public const int NM_THEMECHANGED = (NM_FIRST - 22);

		#endregion

		#region TabControl

		/// <summary>
		/// Retrieves the image list associated with a tab control.
		/// </summary>
		public const int TCM_GETIMAGELIST = TCM_FIRST + 2;
		
		/// <summary>
		/// Assigns an image list to a tab control.
		/// </summary>
		public const int TCM_SETIMAGELIST = TCM_FIRST + 3;
		
		/// <summary>
		/// Retrieves the number of tabs in the tab control.
		/// </summary>
		public const int TCM_GETITEMCOUNT = TCM_FIRST + 4;
		
		/// <summary>
		/// Retrieves information about a tab in a tab control.
		/// </summary>
		public const int TCM_GETITEM = TCM_FIRST + 5;
		
		/// <summary>
		/// Sets some or all of a tab's attributes.
		/// </summary>
		public const int TCM_SETITEM = TCM_FIRST + 6;
		
		/// <summary>
		/// Inserts a new tab in a tab control.
		/// </summary>
		public const int TCM_INSERTITEM = TCM_FIRST + 7;
		
		/// <summary>
		/// Removes an item from a tab control.
		/// </summary>
		public const int TCM_DELETEITEM = TCM_FIRST + 8;
		
		/// <summary>
		/// Removes all items from a tab control.
		/// </summary>
		public const int TCM_DELETEALLITEMS = TCM_FIRST + 9;
		
		/// <summary>
		/// Retrieves the bounding rectangle for a tab in a tab control.
		/// </summary>
		public const int TCM_GETITEMRECT = TCM_FIRST + 10;
		
		/// <summary>
		/// Determines the currently selected tab in a tab control.
		/// </summary>
		public const int TCM_GETCURSEL = TCM_FIRST + 11;
		
		/// <summary>
		/// Selects a tab in a tab control.
		/// </summary>
		public const int TCM_SETCURSEL = TCM_FIRST + 12;
		
		/// <summary>
		/// Determines which tab, if any, is at a specified screen position.
		/// </summary>
		public const int TCM_HITTEST = TCM_FIRST + 13;
		
		/// <summary>
		/// Sets the number of bytes per tab reserved for application-defined data in a tab control.
		/// </summary>
		public const int TCM_SETITEMEXTRA = TCM_FIRST + 14;
		
		/// <summary>
		/// Calculates a tab control's display area given a window rectangle, or calculates the
		/// window rectangle that would correspond to a specified display area.
		/// </summary>
		public const int TCM_ADJUSTRECT = TCM_FIRST + 40;
		
		/// <summary>
		/// Sets the width and height of tabs in a fixed-width or owner-drawn tab control.
		/// </summary>
		public const int TCM_SETITEMSIZE = TCM_FIRST + 41;
		
		/// <summary>
		/// Removes an image from a tab control's image list.
		/// </summary>
		public const int TCM_REMOVEIMAGE = TCM_FIRST + 42;
		
		/// <summary>
		/// Sets the amount of space (padding) around each tab's icon and label in a tab control.
		/// </summary>
		public const int TCM_SETPADDING = TCM_FIRST + 43;
		
		/// <summary>
		/// Retrieves the current number of rows of tabs in a tab control.
		/// </summary>
		public const int TCM_GETROWCOUNT = TCM_FIRST + 44;
		
		/// <summary>
		/// Retrieves the handle to the ToolTip control associated with a tab control.
		/// </summary>
		public const int TCM_GETTOOLTIPS = TCM_FIRST + 45;
		
		/// <summary>
		/// Assigns a ToolTip control to a tab control.
		/// </summary>
		public const int TCM_SETTOOLTIPS = TCM_FIRST + 46;
		
		/// <summary>
		/// Returns the index of the item that has the focus in a tab control.
		/// </summary>
		public const int TCM_GETCURFOCUS = TCM_FIRST + 47;
		
		/// <summary>
		/// Sets the focus to a specified tab in a tab control.
		/// </summary>
		public const int TCM_SETCURFOCUS = TCM_FIRST + 48;
		
		/// <summary>
		/// Sets the minimum width of items in a tab control.
		/// </summary>
		public const int TCM_SETMINTABWIDTH = TCM_FIRST + 49;
		
		/// <summary>
		/// Resets items in a tab control, clearing any that were set to the TCIS_BUTTONPRESSED state.
		/// </summary>
		public const int TCM_DESELECTALL = TCM_FIRST + 50;
		
		/// <summary>
		/// Sets the highlight state of a tab item.
		/// </summary>
		public const int TCM_HIGHLIGHTITEM = TCM_FIRST + 51;
		
		/// <summary>
		/// Sets the extended styles that the tab control will use.
		/// </summary>
		public const int TCM_SETEXTENDEDSTYLE = TCM_FIRST + 52;
		
		/// <summary>
		/// Retrieves the extended styles that are currently in use for the tab control.
		/// </summary>
		public const int TCM_GETEXTENDEDSTYLE = TCM_FIRST + 53;

		#endregion

		#region TreeView
	
		/// <summary>
		/// The pszText and cchTextMax members are valid.
		/// </summary>
		public const int TVIF_TEXT = 0x0001;
		
		/// <summary>
		/// The iImage member is valid
		/// </summary>
		public const int TVIF_IMAGE = 0x0002;
		
		/// <summary>
		/// The lParam member is valid.
		/// </summary>
		public const int TVIF_PARAM = 0x0004;
		
		/// <summary>
		/// The state and stateMask members are valid.
		/// </summary>
		public const int TVIF_STATE = 0x0008;
		
		/// <summary>
		/// The hItem member is valid.
		/// </summary>
		public const int TVIF_HANDLE = 0x0010;
		
		/// <summary>
		/// The iSelectedImage member is valid.
		/// </summary>
		public const int TVIF_SELECTEDIMAGE = 0x0020;
		
		/// <summary>
		/// The cChildren member is valid.
		/// </summary>
		public const int TVIF_CHILDREN = 0x0040;
		
		/// <summary>
		/// The item is selected. Its appearance depends on whether it has the focus.
		/// The item will be drawn using the system colors for selection.
		/// </summary>
		public const int TVIS_SELECTED = 0x0002;
		
		/// <summary>
		/// The item is selected as part of a cut-and-paste operation.
		/// </summary>
		public const int TVIS_CUT = 0x0004;
		
		/// <summary>
		/// The item is selected as a drag-and-drop target.
		/// </summary>
		public const int TVIS_DROPHILITED = 0x0008;
		
		/// <summary>
		/// The item is bold.
		/// </summary>
		public const int TVIS_BOLD = 0x0010;
		
		/// <summary>
		/// The item's list of child items is currently expanded; that is, the child items are visible.
		/// This value applies only to parent items.
		/// </summary>
		public const int TVIS_EXPANDED = 0x0020;
		
		/// <summary>
		/// The item's list of child items has been expanded at least once. The TVN_ITEMEXPANDING and
		/// TVN_ITEMEXPANDED notification messages are not generated for parent items that have this state
		/// set in response to a TVM_EXPAND message. Using TVE_COLLAPSE and TVE_COLLAPSERESET with
		/// TVM_EXPAND will cause this state to be reset. This value applies only to parent items.
		/// </summary>
		public const int TVIS_EXPANDEDONCE = 0x0040;
		
		/// <summary>
		/// A partially expanded tree-view item. In this state, some, but not all, of the child items are
		/// visible and the parent item's plus symbol is displayed.
		/// </summary>
		public const int TVIS_EXPANDPARTIAL = 0x0080;
		
		/// <summary>
		/// Mask for the bits used to specify the item's overlay image index.
		/// </summary>
		public const int TVIS_OVERLAYMASK = 0x0F00;
		
		/// <summary>
		/// Mask for the bits used to specify the item's state image index.
		/// </summary>
		public const int TVIS_STATEIMAGEMASK = 0xF000;
		
		/// <summary>
		/// Same as TVIS_STATEIMAGEMASK.
		/// </summary>
		public const int TVIS_USERMASK = 0xF000;

		/// <summary>
		/// This message inserts a new item in a tree view control.
		/// </summary>
		public const int TVM_INSERTITEM = TV_FIRST + 0;

		/// <summary>
		/// This message removes an item from a tree view control.
		/// </summary>
		public const int TVM_DELETEITEM = TV_FIRST + 1;

		/// <summary>
		/// This message expands or collapses the list of child items, if any, associated with the specified
		/// parent item.
		/// </summary>
		public const int TVM_EXPAND = TV_FIRST + 2;

		/// <summary>
		/// This message retrieves the bounding rectangle for a tree view item and indicates whether the item is visible.
		/// </summary>
		public const int TVM_GETITEMRECT = TV_FIRST + 4;

		/// <summary>
		/// This message retrieves a count of the items in a tree view control.
		/// </summary>
		public const int TVM_GETCOUNT = TV_FIRST + 5;

		/// <summary>
		/// This message retrieves the amount, in pixels, that child items are indented relative to their parent items.
		/// </summary>
		public const int TVM_GETINDENT = TV_FIRST + 6;

		/// <summary>
		/// This message sets the width of indentation for a tree view control and redraws the control to reflect the new width.
		/// </summary>
		public const int TVM_SETINDENT = TV_FIRST + 7;

		/// <summary>
		/// This message retrieves the handle of the normal or state image list associated with a tree view control.
		/// </summary>
		public const int TVM_GETITEMAGELIST = TV_FIRST + 8;

		/// <summary>
		/// This message sets the normal or state image list for a tree view
		/// control and redraws the control using the new images.
		/// </summary>
		public const int TVM_SETITEMAGELIST = TV_FIRST + 9;

		/// <summary>
		/// This message retrieves the tree view item that bears the specified relationship to a specified item.
		/// </summary>
		public const int TVM_GETNEXTITEM = TV_FIRST + 10;

		/// <summary>
		/// Selects the specified tree-view item, scrolls the item into view, or redraws the item in the
		/// style used to indicate the target of a drag-and-drop operation.
		/// </summary>
		public const int TVM_SELECTITEM = TV_FIRST + 11;

		/// <summary>
		/// This message retrieves some or all of the attributes for a tree view item.
		/// </summary>
		public const int TVM_GETITEM = TV_FIRST + 12;

		/// <summary>
		/// This message sets some or all of the attributes of a tree view item.
		/// </summary>
		public const int TVM_SETITEM = TV_FIRST + 13;

		/// <summary>
		/// This message begins in-place editing of the text of the specified item, replacing the text of
		/// the item with a single-line edit control containing the text. This message implicitly selects
		/// and focuses the specified item.
		/// </summary>
		public const int TVM_EDITLABEL = TV_FIRST + 14;

		/// <summary>
		/// This message retrieves the handle of the edit control being used to edit the text of a tree view item.
		/// </summary>
		public const int TVM_GETEDITCONTROL = TV_FIRST + 15;

		/// <summary>
		/// This message retrieves the number of items that can be fully visible in the client window of the tree-view control.
		/// </summary>
		public const int TVM_GETVISIBLECOUNT = TV_FIRST + 16;

		/// <summary>
		/// This message determines the location of the specified point relative to the client area of a tree view control.
		/// </summary>
		public const int TVM_HITTEST = TV_FIRST + 17;

		/// <summary>
		/// This message creates a dragging bitmap for the specified item in a tree view control, creates
		/// an image list for the bitmap, and adds the bitmap to the image list. An application can display
		/// the image when dragging the item by using the image list functions.
		/// </summary>
		public const int TVM_CREATEDRAGIMAGE = TV_FIRST + 18;

		/// <summary>
		/// This message sorts the child items of the specified parent item in a tree view control.
		/// </summary>
		public const int TVM_SORTCHILDREN = TV_FIRST + 19;

		/// <summary>
		/// This message ensures that a tree view item is visible, expanding the parent item or scrolling
		/// the tree view control, if necessary.
		/// </summary>
		public const int TVM_ENSUREVISIBLE = TV_FIRST + 20;

		/// <summary>
		/// This message sorts tree view items using an application-defined callback function that compares the items.
		/// </summary>
		public const int TVM_SORTCHILDRENCB = TV_FIRST + 21;

		/// <summary>
		/// This message ends the editing of a label for a tree view item.
		/// </summary>
		public const int TVM_ENDEDITLABLENOW = TV_FIRST + 22;

		/// <summary>
		/// This message retrieves the incremental search string for a tree view control. The tree view
		/// control uses the incremental search string to select an item based on characters entered by the
		/// user.
		/// </summary>
		public const int TVM_GETISEARCHSTRING = TV_FIRST + 23;

		/// <summary>
		/// Sets a tree-view control's child ToolTip control.
		/// </summary>
		public const int TVM_SETTOOLTIPS = TV_FIRST + 24;

		/// <summary>
		/// Retrieves the handle to the child ToolTip control used by a tree-view control.
		/// </summary>
		public const int TVM_GETTOOLTIPS = TV_FIRST + 25;

		/// <summary>
		/// Sets the insertion mark in a tree-view control.
		/// </summary>
		public const int TVM_SETINSERTMARK = TV_FIRST + 26;

		/// <summary>
		/// Sets the height of the tree-view items.
		/// </summary>
		public const int TVM_SETITEMHEIGHT = TV_FIRST + 27;

		/// <summary>
		/// Retrieves the current height of the each tree-view item.
		/// </summary>
		public const int TVM_GETITEMHEIGHT = TV_FIRST + 28;

		/// <summary>
		/// Sets the background color of the control.
		/// </summary>
		public const int TVM_SETBKCOLOR = TV_FIRST + 29;

		/// <summary>
		/// Sets the text color of the control.
		/// </summary>
		public const int TVM_SETTEXTCOLOR = TV_FIRST + 30;

		/// <summary>
		/// Retrieves the current background color of the control.
		/// </summary>
		public const int TVM_GETBKCOLOR = TV_FIRST + 31;

		/// <summary>
		/// Retrieves the current text color of the control.
		/// </summary>
		public const int TVM_GETTEXTCOLOR = TV_FIRST + 32;

		/// <summary>
		/// Sets the maximum scroll time for the tree-view control.
		/// </summary>
		public const int TVM_SETSCROLLTIME = TV_FIRST + 33;

		/// <summary>
		/// Retrieves the maximum scroll time for the tree-view control.
		/// </summary>
		public const int TVM_GETSCROLLTIME = TV_FIRST + 34;

		/// <summary>
		/// Sets the color used to draw the insertion mark for the tree view.
		/// </summary>
		public const int TVM_SETINSERTMARKCOLOR = TV_FIRST + 37;

		/// <summary>
		/// Retrieves the color used to draw the insertion mark for the tree view.
		/// </summary>
		public const int TVM_GETINSERTMARKCOLOR = TV_FIRST + 38;

		/// <summary>
		/// Retrieves some or all of a tree-view item's state attributes.
		/// </summary>
		public const int TVM_GETITEMSTATE = TV_FIRST + 39;

		/// <summary>
		/// Sets the current line color. 
		/// </summary>
		public const int TVM_SETLINECOLOR = TV_FIRST + 40;

		/// <summary>
		/// The TVM_GETLINECOLOR message gets the current line color. 
		/// </summary>
		public const int TVM_GETLINECOLOR = TV_FIRST + 41;

		/// <summary>
		/// Maps an accessibility identifier (ID) to an HTREEITEM.
		/// </summary>
		public const int TVM_MAPACCIDTOHTREEITEM = TV_FIRST + 42;

		/// <summary>
		/// Maps an HTREEITEM to an accessibility identifier (ID).
		/// </summary>
		public const int TVM_MAPHTREEITEMTOACCID = TV_FIRST + 43;

		#endregion

		#region Ranges for control message IDs

		/*
		 * ListView messages.
		 */
		
		private const int LVM_FIRST = 0x1000;      
		
		/*
		 * TreeView messages.
		 */
		
		private const int TV_FIRST = 0x1100;      
		
		/*
		 * Header messages.
		 */
		
		private const int HDM_FIRST = 0x1200;     
		
		/*
		 * TabControl messages.
		 */
		
		private const int TCM_FIRST = 0x1300;     
		
		/*
		 * PageControl messages.
		 */
		
		private const int PGM_FIRST = 0x1400;     
		
		/*
		 * EditControl messages.
		 */
		
		private const int ECM_FIRST = 0x1500;      
		
		/*
		 * Button messages.
		 */

		private const int BCM_FIRST = 0x1600;      
		
		/*
		 * ComboBox messages.
		 */
		
		private const int CBM_FIRST = 0x1700;      
		
		/*
		 * CommonControl shared messages.
		 */
		
		private const int CCM_FIRST = 0x2000;      
		private const int CCM_LAST = (CCM_FIRST + 0x200);

		#endregion

		#region WM_NOTIFY codes (NMHDR.code values)

		/*
		 * Generic to all cotrols.
		 */

		private const int NM_FIRST = (0 - 0);
		private const int NM_LAST = (0 - 99);

		/*
		 * ListView
		 */

		private const int LVN_FIRST = (0 - 100);       
		private const int LVN_LAST = (0 - 199);

		/* 
		 * Header
		 */

		private const int HDN_FIRST = (0 - 300);       
		private const int HDN_LAST = (0 - 399);

		/*
		 * TreeView
		 */

		private const int TVN_FIRST = (0 - 400);      
		private const int TVN_LAST = (0 - 499);

		/*
		 * ToolTips
		 */

		private const int TTN_FIRST = (0 - 520);      
		private const int TTN_LAST = (0 - 549);

		/*
		 * TabControl
		 */

		private const int TCN_FIRST = (0 - 550);      
		private const int TCN_LAST = (0 - 580);

		/*
		 * Common Dialog (New)
		 */

		private const int CDN_FIRST = (0 - 601);      
		private const int CDN_LAST = (0 - 699);

		/*
		 * ToolBar
		 */

		private const int TBN_FIRST = (0 - 700);      
		private const int TBN_LAST = (0 - 720);

		/*
		 * UpDown
		 */

		private const int UDN_FIRST = (0 - 721);      
		private const int UDN_LAST = (0 - 740);

		/*
		 * MonthCalendar
		 */

		private const int MCN_FIRST = (0 - 750);      
		private const int MCN_LAST = (0 - 759);

		/*
		 * DateTimePicker
		 */

		private const int DTN_FIRST = (0 - 760);      
		private const int DTN_LAST = (0 - 799);

		/*
		 * ComboBoxEx
		 */

		private const int CBEN_FIRST = (0 - 800);     
		private const int CBEN_LAST = (0 - 830);

		/*
		 * ReBar
		 */

		private const int RBN_FIRST = (0 - 831);      
		private const int RBN_LAST = (0 - 859);

		/*
		 * Internet Address
		 */

		private const int IPN_FIRST = (0 - 860); 
		private const int IPN_LAST = (0 - 879);  

		/*
		 * StatusBar
		 */

		private const int SBN_FIRST = (0 - 880); 
		private const int SBN_LAST = (0 - 899);

		/*
		 * PagerControl
		 */

		private const int PGN_FIRST = (0 - 900); 
		private const int PGN_LAST = (0 - 950);

		private const int WMN_FIRST = (0 - 1000);
		private const int WMN_LAST = (0 - 1200);

		private const int BCN_FIRST = (0 - 1250);
		private const int BCN_LAST = (0 - 1350);

		private const int MSGF_COMMCTRL_BEGINDRAG = 0x4200;
		private const int MSGF_COMMCTRL_SIZEHEADER = 0x4201;
		private const int MSGF_COMMCTRL_DRAGSELECT = 0x4202;
		private const int MSGF_COMMCTRL_TOOLBARCUST = 0x4203;

		#endregion
	}
}
