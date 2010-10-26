/* -----------------------------------------------
 * TVITEMEX.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Specifies or receives attributes of a tree-view item. This structure is an enhancement to the
	/// <see cref="TVITEM"/> structure. New applications should use this structure where appropriate.
	/// </summary>
	public struct TVITEMEX
	{
		/// <summary>
		/// <para>Array of flags that indicate which of the other structure members contain valid data.
		/// When this structure is used with the TVM_GETITEM message, the mask member indicates the item
		/// attributes to retrieve. This member can be one or more of the following values.</para>
		/// <para><see cref="CommCtrl.TVIF_CHILDREN"/> - The cChildren member is valid.</para>
		/// <para><see cref="CommCtrl.TVIF_DI_SETITEM"/> - The tree-view control will retain the supplied information and will not request it again.
		/// This flag is valid only when processing the TVN_GETDISPINFO notification.</para>
		/// <para><see cref="CommCtrl.TVIF_HANDLE"/> - The hItem member is valid.</para>
		/// <para><see cref="CommCtrl.TVIF_IMAGE"/> - The iImage member is valid.</para>
		/// <para><see cref="CommCtrl.TVIF_INTEGRAL"/> - The iIntegral member is valid.</para>
		/// <para><see cref="CommCtrl.TVIF_PARAM"/> - The lParam member is valid.</para>
		/// <para><see cref="CommCtrl.TVIF_SELECTEDIMAGE"/> - The iSelectedImage member is valid.</para>
		/// <para><see cref="CommCtrl.TVIF_STATE"/> - The state and stateMask members are valid.</para>
		/// <para><see cref="CommCtrl.TVIF_TEXT"/> - The pszText and cchTextMax members are valid.</para>
		/// </summary>
		public Int32 mask;

		/// <summary>
		/// Item to which this structure refers.
		/// </summary>
		public IntPtr hItem;

		/// <summary>
		/// <para>Set of bit flags and image list indexes that indicate the item's state. When setting the state
		/// of an item, the stateMask member indicates the bits of this member that are valid.
		/// When retrieving the state of an item, this member returns the current state for the bits
		/// indicated in the stateMask member.</para>
		/// <para>Bits 0 through 7 of this member contain the item state flags. Possible item state flags are the following.</para>
		/// <para><see cref="CommCtrl.TVIS_BOLD"/> - The item is bold.</para>
		/// <para><see cref="CommCtrl.TVIS_CUT"/> - The item is selected as part of a cut-and-paste operation.</para> 
		/// <para><see cref="CommCtrl.TVIS_DROPHILITED"/> - The item is selected as a drag-and-drop target.</para>
		/// <para><see cref="CommCtrl.TVIS_EXPANDED"/> - The item's list of child items is currently expanded; that is, the child items
		/// are visible. This value applies only to parent items.</para>
		/// <para><see cref="CommCtrl.TVIS_EXPANDEDONCE"/> - The item's list of child items has been expanded at least once.
		/// The TVN_ITEMEXPANDING and TVN_ITEMEXPANDED notification messages are not generated for parent
		/// items that have this state set in response to a TVM_EXPAND message. Using TVE_COLLAPSE and
		/// TVE_COLLAPSERESET with TVM_EXPAND will cause this state to be reset. This value applies only
		/// to parent items.</para>
		/// <para><see cref="CommCtrl.TVIS_EXPANDPARTIAL"/> - A partially expanded tree-view item. In this state, some, but not all,
		/// of the child items are visible and the parent item's plus symbol is displayed.</para>
		/// <para><see cref="CommCtrl.TVIS_SELECTED"/> - The item is selected. Its appearance depends on whether it has the focus.
		/// The item will be drawn using the system colors for selection. Note: When you set or retrieve
		/// an item's overlay image index or state image index, you must specify the following masks in the
		/// stateMask member of the TVITEM structure. These values can also be used to mask off the state
		/// bits that are not of interest.</para>
		/// <para><see cref="CommCtrl.TVIS_OVERLAYMASK"/> - Mask for the bits used to specify the item's overlay image index.</para>
		/// <para><see cref="CommCtrl.TVIS_STATEIMAGEMASK"/> - Mask for the bits used to specify the item's state image index.</para>
		/// <para><see cref="CommCtrl.TVIS_USERMASK"/> - Same as <see cref="CommCtrl.TVIS_STATEIMAGEMASK"/>.</para>
		/// <para></para>
		/// <para>See TVITEMEX structure info on MSDN for more details.</para>
		/// </summary>
		public Int32 state;

		/// <summary>
		/// Bits of the state member that are valid. If you are retrieving an item's state, set the bits
		/// of the stateMask member to indicate the bits to be returned in the state member. If you are
		/// setting an item's state, set the bits of the stateMask member to indicate the bits of the state
		/// member that you want to set. To set or retrieve an item's overlay image index, set the
		/// <see cref="CommCtrl.TVIS_OVERLAYMASK"/> bits. To set or retrieve an item's state image index, set the
		/// <see cref="CommCtrl.TVIS_STATEIMAGEMASK"/> bits. 
		/// </summary>
		public Int32 stateMask;

		/// <summary>
		/// Pointer to a null-terminated String that contains the item text if the structure specifies item
		/// attributes. If this member is the LPSTR_TEXTCALLBACK value, the parent window is responsible
		/// for storing the name. In this case, the tree-view control sends the parent window a
		/// TVN_GETDISPINFO notification message when it needs the item text for displaying, sorting, or
		/// editing and a TVN_SETDISPINFO notification message when the item text changes. If the structure
		/// is receiving item attributes, this member is the address of the buffer that receives the item
		/// text. Note that although the tree-view control allows any length String to be stored as item
		/// text, only the first 260 characters are displayed.
		/// </summary>
		public String pszText;

		/// <summary>
		/// Size of the buffer pointed to by the pszText member, in characters. If this structure is being
		/// used to set item attributes, this member is ignored. 
		/// </summary>
		public Int32 cchTextMax;

		/// <summary>
		/// Index in the tree-view control's image list of the icon image to use when the item is in the
		/// nonselected state. If this member is the I_IMAGECALLBACK value, the parent window is
		/// responsible for storing the index. In this case, the tree-view control sends the parent a
		/// TVN_GETDISPINFO notification message to retrieve the index when it needs to display the image.
		/// </summary>
		public Int32 iImage;

		/// <summary>
		/// Index in the tree-view control's image list of the icon image to use when the item is in the
		/// selected state. If this member is the I_IMAGECALLBACK value, the parent window is responsible
		/// for storing the index. In this case, the tree-view control sends the parent a TVN_GETDISPINFO
		/// notification message to retrieve the index when it needs to display the image.
		/// </summary>
		public Int32 iSelectedImage;

		/// <summary>
		/// Flag that indicates whether the item has associated child items.
		/// <para></para>
		/// <para>See TVITEMEX structure info on MSDN for more details.</para>
		/// </summary>
		public Int32 cChildren;

		/// <summary>
		/// A value to associate with the item.
		/// </summary>
		public IntPtr lParam;

		/// <summary>
		/// Height of the item. This height is in increments of the standard item height (see TVM_SETITEMHEIGHT). By default, each item gets one increment of item height. Setting this member to 2 will give the item twice the standard height; setting this member to 3 will give the item three times the standard height; and so on. The tree-view control does not draw in this extra area. This extra space can be used by the application for drawing when using custom draw. 
		/// </summary>
		public Int32 iIntegral; 
	}
}
