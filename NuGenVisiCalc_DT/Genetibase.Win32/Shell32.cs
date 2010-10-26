/* -----------------------------------------------
 * Shell32.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Imports Shell32.dll functions.
	/// </summary>
	[CLSCompliant(false)]
	public class Shell32
	{
		/// <summary>
		/// Retrieves the <see cref="IShellFolder"/> interface for the desktop folder, which is the root of the Shell's namespace.
		/// </summary>
		[DllImport(DLL)]
		public static extern Int32 SHGetDesktopFolder(ref IShellFolder ppshf);

		/// <summary>
		/// Retrieves information about an Object in the file system, such as a file, a folder, a directory, or a drive root.
		/// </summary>
		[DllImport(DLL)]
		public static extern IntPtr SHGetFileInfo(String pszPath, uint dwFileAttribs, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

		/// <summary>
		/// Retrieves information about an Object in the file system, such as a file, a folder, a directory, or a drive root.
		/// </summary>
		[DllImport(DLL)]
		public static extern IntPtr SHGetFileInfo(IntPtr pIDL, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

		/// <summary>
		/// Retrieves a pointer to the ITEMIDLIST structure of a special folder.
		/// </summary>
		[DllImport(DLL)]
		public static extern Int32 SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl);

		/// <summary>
		/// Combines two ITEMIDLIST structures.
		/// </summary>
		[DllImport(DLL)]
		public static extern IntPtr ILCombine(IntPtr pIDLParent, IntPtr pIDLChild);

		/// <summary>
		/// Converts an item identifier list to a file system path.
		/// </summary>
		[DllImport(DLL)]
		public static extern Int32 SHGetPathFromIDList(IntPtr pIDL, StringBuilder strPath);

		/// <summary>
		/// Defines the values used with the <see cref="IShellFolder.GetDisplayNameOf"/> and <see cref="IShellFolder.SetNameOf"/> methods to specify the type of file or folder names used by those methods.
		/// </summary>
		[Flags]
		public enum SHGNO : uint
		{
			/// <summary>
			/// Default (display purpose)
			/// </summary>
			SHGDN_NORMAL = 0x0000,

			/// <summary>
			/// Displayed under a folder (relative)
			/// </summary>
			SHGDN_INFOLDER = 0x0001,

			/// <summary>
			/// For in-place editing
			/// </summary>
			SHGDN_FOREDITING = 0x1000,

			/// <summary>
			/// UI friendly parsing name (remove ugly stuff)
			/// </summary>
			SHGDN_FORADDRESSBAR = 0x4000,

			/// <summary>
			/// Parsing name for ParseDisplayName()
			/// </summary>
			SHGDN_FORPARSING = 0x8000,
		}

		/// <summary>
		/// Determines the type of items included in an enumeration. These values are used with the <see cref="IShellFolder.EnumObjects"/> method.
		/// </summary>
		[Flags]
		public enum SHCONTF : uint
		{
			/// <summary>
			/// Only want folders enumerated (SFGAO_FOLDER)
			/// </summary>
			SHCONTF_FOLDERS = 0x0020,

			/// <summary>
			/// Include non folders
			/// </summary>
			SHCONTF_NONFOLDERS = 0x0040,

			/// <summary>
			/// Show items normally hidden
			/// </summary>
			SHCONTF_INCLUDEHIDDEN = 0x0080,

			/// <summary>
			/// Allow EnumObject() to return before validating enum
			/// </summary>
			SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,

			/// <summary>
			/// Hint that client is looking for printers
			/// </summary>
			SHCONTF_NETPRINTERSRCH = 0x0200,

			/// <summary>
			/// Hint that client is looking sharable resources (remote shares)
			/// </summary>
			SHCONTF_SHAREABLE = 0x0400,

			/// <summary>
			/// Include all items with accessible storage and their ancestors
			/// </summary>
			SHCONTF_STORAGE = 0x0800,
		}

		/// <summary>
		/// </summary>
		[Flags]
		public enum SFGAOF : uint
		{
			/// <summary>
			/// Objects can be copied  (DROPEFFECT_COPY)
			/// </summary>
			SFGAO_CANCOPY = 0x1,

			/// <summary>
			/// Objects can be moved   (DROPEFFECT_MOVE)
			/// </summary>
			SFGAO_CANMOVE = 0x2,

			/// <summary>
			/// Objects can be linked  (DROPEFFECT_LINK)
			/// </summary>
			SFGAO_CANLINK = 0x4,

			/// <summary>
			/// Supports BindToObject(IID_IStorage)
			/// </summary>
			SFGAO_STORAGE = 0x00000008,

			/// <summary>
			/// Objects can be renamed
			/// </summary>
			SFGAO_CANRENAME = 0x00000010,

			/// <summary>
			/// Objects can be deleted
			/// </summary>
			SFGAO_CANDELETE = 0x00000020,

			/// <summary>
			/// Objects have property sheets
			/// </summary>
			SFGAO_HASPROPSHEET = 0x00000040,

			/// <summary>
			/// Objects are drop target
			/// </summary>
			SFGAO_DROPTARGET = 0x00000100,

			/// <summary>
			/// </summary>
			SFGAO_CAPABILITYMASK = 0x00000177,

			/// <summary>
			/// Object is encrypted (use alt color)
			/// </summary>
			SFGAO_ENCRYPTED = 0x00002000,

			/// <summary>
			/// 'Slow' Object
			/// </summary>
			SFGAO_ISSLOW = 0x00004000,

			/// <summary>
			/// Ghosted icon
			/// </summary>
			SFGAO_GHOSTED = 0x00008000,

			/// <summary>
			/// Shortcut (link)
			/// </summary>
			SFGAO_LINK = 0x00010000,

			/// <summary>
			/// Shared
			/// </summary>
			SFGAO_SHARE = 0x00020000,

			/// <summary>
			/// Read-only
			/// </summary>
			SFGAO_READONLY = 0x00040000,

			/// <summary>
			/// Hidden Object
			/// </summary>
			SFGAO_HIDDEN = 0x00080000,

			/// <summary> 
			/// </summary>
			SFGAO_DISPLAYATTRMASK = 0x000FC000,

			/// <summary>
			/// May contain children with SFGAO_FILESYSTEM
			/// </summary>
			SFGAO_FILESYSANCESTOR = 0x10000000,

			/// <summary>
			/// Support BindToObject(IID_IShellFolder)
			/// </summary>
			SFGAO_FOLDER = 0x20000000,

			/// <summary>
			/// Is a win32 file system Object (file/folder/root)
			/// </summary>
			SFGAO_FILESYSTEM = 0x40000000,

			/// <summary>
			/// May contain children with SFGAO_FOLDER
			/// </summary>
			SFGAO_HASSUBFOLDER = 0x80000000,

			/// <summary>
			/// </summary>
			SFGAO_CONTENTSMASK = 0x80000000,

			/// <summary>
			/// Invalidate cached information
			/// </summary>
			SFGAO_VALIDATE = 0x01000000,

			/// <summary>
			/// Is this removeable media?
			/// </summary>
			SFGAO_REMOVABLE = 0x02000000,

			/// <summary>
			/// Object is compressed (use alt color)
			/// </summary>
			SFGAO_COMPRESSED = 0x04000000,

			/// <summary>
			/// Supports IShellFolder, but only implements CreateViewObject() (non-folder view)
			/// </summary>
			SFGAO_BROWSABLE = 0x08000000,

			/// <summary>
			/// Is a non-enumerated Object
			/// </summary>
			SFGAO_NONENUMERATED = 0x00100000,

			/// <summary>
			/// Should show bold in explorer tree
			/// </summary>
			SFGAO_NEWCONTENT = 0x00200000,

			/// <summary>
			/// Defunct
			/// </summary>
			SFGAO_CANMONIKER = 0x00400000,

			/// <summary>
			/// Defunct
			/// </summary>
			SFGAO_HASSTORAGE = 0x00400000,

			/// <summary>
			/// Supports BindToObject(IID_IStream)
			/// </summary>
			SFGAO_STREAM = 0x00400000,

			/// <summary>
			/// May contain children with SFGAO_STORAGE or SFGAO_STREAM
			/// </summary>
			SFGAO_STORAGEANCESTOR = 0x00800000,

			/// <summary>
			/// For determining storage capabilities, ie for open/save semantics
			/// </summary>
			SFGAO_STORAGECAPMASK = 0x70C50008,
		}

		/// <summary>
		/// Contains strings returned from the <see cref="IShellFolder"/> interface methods.
		/// </summary>
		[Flags]
		public enum STRRET : uint
		{
			/// <summary>
			/// </summary>
			STRRET_WSTR = 0,

			/// <summary>
			/// </summary>
			STRRET_OFFSET = 0x1,

			/// <summary>
			/// </summary>
			STRRET_CSTR = 0x2,
		}

		/// <summary>
		/// </summary>
		[Flags]
		public enum SHGFI
		{
			/// <summary>
			/// </summary>
			SHGFI_ICON = 0x000000100,

			/// <summary>
			/// </summary>
			SHGFI_DISPLAYNAME = 0x000000200,

			/// <summary>
			/// </summary>
			SHGFI_TYPENAME = 0x000000400,

			/// <summary>
			/// </summary>
			SHGFI_ATTRIBUTES = 0x000000800,

			/// <summary>
			/// </summary>
			SHGFI_ICONLOCATION = 0x000001000,

			/// <summary>
			/// </summary>
			SHGFI_EXETYPE = 0x000002000,

			/// <summary>
			/// </summary>
			SHGFI_SYSICONINDEX = 0x000004000,

			/// <summary>
			/// </summary>
			SHGFI_LINKOVERLAY = 0x000008000,

			/// <summary>
			/// </summary>
			SHGFI_SELECTED = 0x000010000,

			/// <summary>
			/// </summary>
			SHGFI_ATTR_SPECIFIED = 0x000020000,

			/// <summary>
			/// </summary>
			SHGFI_LARGEICON = 0x000000000,

			/// <summary>
			/// </summary>
			SHGFI_SMALLICON = 0x000000001,

			/// <summary>
			/// </summary>
			SHGFI_OPENICON = 0x000000002,

			/// <summary>
			/// </summary>
			SHGFI_SHELLICONSIZE = 0x000000004,

			/// <summary>
			/// </summary>
			SHGFI_PIDL = 0x000000008,

			/// <summary>
			/// </summary>
			SHGFI_USEFILEATTRIBUTES = 0x000000010,

			/// <summary>
			/// </summary>
			SHGFI_ADDOVERLAYS = 0x000000020,

			/// <summary>
			/// </summary>
			SHGFI_OVERLAYINDEX = 0x000000040
		}

		/// <summary>
		/// CSIDL values provide a unique system-independent way to identify special folders used frequently by applications, but which may not have the same name or location on any given system. For example, the system folder may be "C:\Windows" on one system and "C:\Winnt" on another. These constants are defined in Shlobj.h and Shfolder.h.
		/// </summary>
		[Flags]
		public enum CSIDL : uint
		{
			/// <summary>
			/// </summary>
			CSIDL_DESKTOP = 0x0000,

			/// <summary>
			/// </summary>
			CSIDL_WINDOWS = 0x0024
		}

		/// <summary>
		/// Contains information about a file Object.
		/// </summary>
		public struct SHFILEINFO
		{
			/// <summary>
			/// </summary>
			public IntPtr hIcon;

			/// <summary>
			/// </summary>
			public Int32 iIcon;

			/// <summary>
			/// </summary>
			public uint dwAttributes;

			/// <summary>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public String szDisplayName;

			/// <summary>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public String szTypeName;
		}

		/// <summary>
		/// Used to manage folders. It is exposed by all Shell namespace folder objects.
		/// </summary>
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000214E6-0000-0000-C000-000000000046")]
		public interface IShellFolder
		{
			/// <summary>
			/// Translates a file Object's or folder's display name into an item identifier list.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwnd">Optional window handle</param>
			/// <param name="pbc">Optional bind context that controls the parsing operation. This parameter is normally set to NULL.</param>
			/// <param name="pszDisplayName">Null-terminated UNICODE String with the display name.</param>
			/// <param name="pchEaten">Pointer to a ULONG value that receives the number of characters of the display name that was parsed.</param>
			/// <param name="ppidl">Pointer to an ITEMIDLIST pointer that receives the item identifier list for the Object.</param>
			/// <param name="pdwAttributes">Optional parameter that can be used to query for file attributes. This can be values from the SFGAO enum</param>
			[PreserveSig()]
			uint ParseDisplayName(
				IntPtr hwnd,
				IntPtr pbc,
				[In(), MarshalAs(UnmanagedType.LPWStr)] 
                String pszDisplayName,
				out uint pchEaten,
				out IntPtr ppidl,
				ref uint pdwAttributes);

			/// <summary>
			/// Allows a client to determine the contents of a folder by creating an item identifier enumeration Object and returning its IEnumIDList interface.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwnd">If user input is required to perform the enumeration, this window handle should be used by the enumeration Object as the parent window to take user input.</param>
			/// <param name="grfFlags">Flags indicating which items to include in the enumeration. For a list of possible values, see the SHCONTF enum. </param>
			/// <param name="ppenumIDList">Address that receives a pointer to the IEnumIDList interface of the enumeration Object created by this method. </param>
			[PreserveSig()]
			uint EnumObjects(
				IntPtr hwnd,
				SHCONTF grfFlags,
				out IEnumIDList ppenumIDList);

			/// <summary>
			/// Retrieves an IShellFolder Object for a subfolder.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.</param>
			/// <param name="pbc">Optional address of an IBindCtx interface on a bind context Object to be used during this operation.</param>
			/// <param name="riid">Identifier of the interface to return. </param>
			/// <param name="ppv">Address that receives the interface pointer.</param>
			[PreserveSig()]
			uint BindToObject(
				IntPtr pidl,
				IntPtr pbc,
				[In()]
                ref Guid riid,
				out IShellFolder ppv);

			/// <summary>
			/// Requests a pointer to an Object's storage interface. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure that identifies the subfolder relative to its parent folder.</param>
			/// <param name="pbc">Optional address of an IBindCtx interface on a bind context Object to be used during this operation.</param>
			/// <param name="riid">Interface identifier (IID) of the requested storage interface.</param>
			/// <param name="ppv">Address that receives the interface pointer specified by riid.</param>
			[PreserveSig()]
			uint BindToStorage(
				IntPtr pidl,
				IntPtr pbc,
				[In()]
                ref Guid riid,
				[MarshalAs(UnmanagedType.Interface)]
                out Object ppv);

			/// <summary>
			/// Determines the relative order of two file objects or folders, given their item identifier lists. 
			/// Return value: If this method is successful, the CODE field of the HRESULT contains one of the following values (the code can be retrived using the helper function GetHResultCode)...
			/// A negative return value indicates that the first item should precede the second (pidl1 &lt; pidl2). 
			/// A positive return value indicates that the first item should follow the second (pidl1 &gt; pidl2).  Zero A return value of zero indicates that the two items are the same (pidl1 = pidl2). 
			/// </summary>
			/// <param name="lParam">Value that specifies how the comparison should be performed. The lower sixteen bits of lParam define the sorting rule. The upper sixteen bits of lParam are used for flags that modify the sorting rule. values can be from the SHCIDS enum</param>
			/// <param name="pidl1">Pointer to the first item's ITEMIDLIST structure.</param>
			/// <param name="pidl2">Pointer to the second item's ITEMIDLIST structure.</param>
			[PreserveSig()]
			Int32 CompareIDs(
				Int32 lParam,
				IntPtr pidl1,
				IntPtr pidl2);

			/// <summary>
			/// Requests an Object that can be used to obtain information from or interact with a folder Object.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwndOwner">Handle to the owner window.</param>
			/// <param name="riid">Identifier of the requested interface.</param>
			/// <param name="ppv">Address of a pointer to the requested interface. </param>
			[PreserveSig()]
			uint CreateViewObject(
				IntPtr hwndOwner,       
				[In()]
                ref Guid riid,         
				[MarshalAs(UnmanagedType.Interface)]
                out Object ppv);      

			/// <summary>
			/// Retrieves the attributes of one or more file objects or subfolders. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="cidl">Number of file objects from which to retrieve attributes. </param>
			/// <param name="apidl">Address of an array of pointers to ITEMIDLIST structures, each of which uniquely identifies a file Object relative to the parent folder.</param>
			/// <param name="rgfInOut">Address of a single ULONG value that, on entry, contains the attributes that the caller is requesting. On exit, this value contains the requested attributes that are common to all of the specified objects. this value can be from the SFGAO enum</param>
			[PreserveSig()]
			uint GetAttributesOf(
				Int32 cidl,               
				out IntPtr apidl,          
				out SFGAOF rgfInOut);      

			/// <summary>
			/// Retrieves an OLE interface that can be used to carry out actions on the specified file objects or folders. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwndOwner">Handle to the owner window that the client should specify if it displays a dialog box or message box.</param>
			/// <param name="cidl">Number of file objects or subfolders specified in the apidl parameter. </param>
			/// <param name="apidl">Address of an array of pointers to ITEMIDLIST structures, each of which uniquely identifies a file Object or subfolder relative to the parent folder.</param>
			/// <param name="riid">Identifier of the COM interface Object to return.</param>
			/// <param name="rgfReserved">Reserved. </param>
			/// <param name="ppv">Pointer to the requested interface.</param>
			[PreserveSig()]
			uint GetUIObjectOf(
				IntPtr hwndOwner,
				Int32 cidl,
				[In(), MarshalAs(UnmanagedType.LPArray)] IntPtr[]
				apidl,
				[In()]
                ref Guid riid,
				IntPtr rgfReserved,
				[MarshalAs(UnmanagedType.Interface)]
                out Object ppv);

			/// <summary>
			/// Retrieves the display name for the specified file Object or subfolder. 
			/// Return value: error code, if any
			/// </summary>
			/// <param name="pidl">Address of an ITEMIDLIST structure (PIDL) that uniquely identifies the file Object or subfolder relative to the parent folder. </param>
			/// <param name="uFlags">Flags used to request the type of display name to return. For a list of possible values. </param>
			/// <param name="pName">Address of a STRRET structure in which to return the display name.</param>
			[PreserveSig()]
			uint GetDisplayNameOf(
				IntPtr pidl,
				SHGNO uFlags,
				out STRRET pName);

			/// <summary>
			/// Sets the display name of a file Object or subfolder, changing the item identifier in the process.
			/// Return value: error code, if any
			/// </summary>
			/// <param name="hwnd">Handle to the owner window of any dialog or message boxes that the client displays.</param>
			/// <param name="pidl">Pointer to an ITEMIDLIST structure that uniquely identifies the file Object or subfolder relative to the parent folder.</param>
			/// <param name="pszName">Pointer to a null-terminated String that specifies the new display name. </param>
			/// <param name="uFlags">Flags indicating the type of name specified by the lpszName parameter. For a list of possible values, see the description of the SHGNO enum. </param>
			/// <param name="ppidlOut">Address of a pointer to an ITEMIDLIST structure which receives the new ITEMIDLIST. </param>
			[PreserveSig()]
			uint SetNameOf(
				IntPtr hwnd,
				IntPtr pidl,
				[In(), MarshalAs(UnmanagedType.LPWStr)] 
                String pszName,
				SHGNO uFlags,
				out IntPtr ppidlOut);
		}

		/// <summary>
		/// Provides a standard set of methods that can be used to enumerate the pointers to item identifier lists (PIDLs) of the items in a Shell folder. When a folder's IShellFolder::EnumObjects method is called, it creates an enumeration Object and passes a pointer to the Object's IEnumIDList interface back to the caller.
		/// </summary>
		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000214F2-0000-0000-C000-000000000046")]
		public interface IEnumIDList
		{
			/// <summary>
			/// Retrieves the specified number of item identifiers in the enumeration sequence and advances the current position by the number of items retrieved.
			/// </summary>
			/// <param name="celt">Number of elements in the array pointed to by the rgelt parameter.</param>
			/// <param name="rgelt">Address of an array of ITEMIDLIST pointers that receives the item identifiers. The implementation must allocate these item identifiers using the Shell's allocator (retrieved by the SHGetMalloc function). The calling application is responsible for freeing the item identifiers using the Shell's allocator.</param>
			/// <param name="pceltFetched">Address of a value that receives a count of the item identifiers actually returned in rgelt. The count can be smaller than the value specified in the celt parameter. This parameter can be NULL only if celt is one.</param>
			/// <returns></returns>
			[PreserveSig()]
			uint Next(
				uint celt,
				out IntPtr rgelt,
				out Int32 pceltFetched
				);

			/// <summary>
			/// Skips over the specified number of elements in the enumeration sequence. 
			/// </summary>
			/// <param name="celt">Number of item identifiers to skip.</param>
			[PreserveSig()]
			uint Skip(
				uint celt
				);

			/// <summary>
			/// Returns to the beginning of the enumeration sequence. 
			/// </summary>
			[PreserveSig()]
			uint Reset();

			/// <summary>
			/// Creates a new item enumeration Object with the same contents and state as the current one. 
			/// </summary>
			/// <param name="ppenum">Address of a pointer to the new enumeration Object. The calling application must eventually free the new Object by calling its Release member function.</param>
			[PreserveSig()]
			uint Clone(
				out IEnumIDList ppenum
				);
		}

		/// <summary>
		/// </summary>
		public const Int32 FILE_ATTRIBUTE_NORMAL = 0x80;

		/// <summary>
		/// </summary>
		public static Guid IID_IShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");
		private const String DLL = "Shell32.dll";

		private Shell32()
		{
		}
	}
}
