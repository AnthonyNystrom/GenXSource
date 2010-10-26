/* -----------------------------------------------
 * TVITEM.cs
 * Copyright � 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Specifies or receives attributes of a tree-view item. This structure is identical to the TV_ITEM structure, but it has been renamed to follow current naming conventions.
	/// New applications should use this structure.
	/// </summary>
	[CLSCompliant(false)]
	public struct TVITEM
	{
		/// <summary>
		/// Array of flags that indicate which of the other structure members contain valid data.
		/// When this structure is used with the TVM_GETITEM message, the mask member indicates the item
		/// attributes to retrieve.
		/// </summary>
		public uint mask;

		/// <summary>
		/// Item to which this structure refers.
		/// </summary>
		public IntPtr hItem;

		/// <summary>
		/// Set of bit flags and image list indexes that indicate the item's state. When setting the state
		/// of an item, the stateMask member indicates the bits of this member that are valid.
		/// When retrieving the state of an item, this member returns the current state for the bits
		/// indicated in the stateMask member. 
		/// </summary>
		public uint state;

		/// <summary>
		/// Bits of the state member that are valid. If you are retrieving an item's state, set the bits
		/// of the stateMask member to indicate the bits to be returned in the state member. If you are
		/// setting an item's state, set the bits of the stateMask member to indicate the bits of the state
		/// member that you want to set. To set or retrieve an item's overlay image index, set the
		/// TVIS_OVERLAYMASK bits. To set or retrieve an item's state image index, set the
		/// TVIS_STATEIMAGEMASK bits.
		/// </summary>
		public uint stateMask;

		/// <summary>
		/// Pointer to a null-terminated String that contains the item text if the structure specifies item
		/// attributes. If this member is the LPSTR_TEXTCALLBACK value, the parent window is responsible for
		/// storing the name. In this case, the tree-view control sends the parent window a TVN_GETDISPINFO
		/// notification message when it needs the item text for displaying, sorting, or editing and a
		/// TVN_SETDISPINFO notification message when the item text changes. If the structure is receiving
		/// item attributes, this member is the address of the buffer that receives the item text. Note that
		/// although the tree-view control allows any length String to be stored as item text, only the
		/// first 260 characters are displayed.
		/// </summary>
		public IntPtr pszText;

		/// <summary>
		/// Size of the buffer pointed to by the pszText member, in characters.
		/// If this structure is being used to set item attributes, this member is ignored.
		/// </summary>
		public Int32 cchTextMax;

		/// <summary>
		/// Index in the tree-view control's image list of the icon image to use when the item is in the
		/// nonselected state. If this member is the I_IMAGECALLBACK value, the parent window is responsible
		/// for storing the index. In this case, the tree-view control sends the parent a TVN_GETDISPINFO
		/// notification message to retrieve the index when it needs to display the image.
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
		/// </summary>
		public Int32 cChildren;

		/// <summary>
		/// A value to associate with the item.
		/// </summary>
		public IntPtr lParam;
	}
}
