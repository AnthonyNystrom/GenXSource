/* -----------------------------------------------
 * IAssemblyName.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Genetibase.Shared.Fusion
{
	/// <summary>
	/// </summary>
	[ComImport(), Guid("CD193BC0-B4BC-11D2-9833-00C04FC31D2E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAssemblyName
	{
		/// <summary>
		/// </summary>
		/// <param name="PropertyId"></param>
		/// <param name="pvProperty"></param>
		/// <param name="cbProperty"></param>
		/// <returns></returns>
		[PreserveSig()]
		int Set(
			uint PropertyId,
			IntPtr pvProperty,
			uint cbProperty
			);
		
		/// <summary>
		/// </summary>
		/// <param name="PropertyId"></param>
		/// <param name="pvProperty"></param>
		/// <param name="pcbProperty"></param>
		/// <returns></returns>
		[PreserveSig()]
		int Get(
			uint PropertyId,
			IntPtr pvProperty,
			ref uint pcbProperty
			);
		
		/// <summary>
		/// </summary>
		/// <returns></returns>
		[PreserveSig()]
		int Finalize();
		
		/// <summary>
		/// </summary>
		/// <param name="szDisplayName"></param>
		/// <param name="pccDisplayName"></param>
		/// <param name="dwDisplayFlags"></param>
		/// <returns></returns>
		[PreserveSig()]
		int GetDisplayName(
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder szDisplayName,
			ref uint pccDisplayName,
			uint dwDisplayFlags
			);
		
		/// <summary>
		/// </summary>
		/// <param name="refIID"></param>
		/// <param name="pAsmBindSink"></param>
		/// <param name="pApplicationContext"></param>
		/// <param name="szCodeBase"></param>
		/// <param name="llFlags"></param>
		/// <param name="pvReserved"></param>
		/// <param name="cbReserved"></param>
		/// <param name="ppv"></param>
		/// <returns></returns>
		[PreserveSig()]
		int BindToObject(
			object refIID,
			object pAsmBindSink,
			IApplicationContext pApplicationContext,
			[MarshalAs(UnmanagedType.LPWStr)] string szCodeBase,
			long llFlags,
			int pvReserved,
			uint cbReserved,
			out int ppv
			);
		
		/// <summary>
		/// </summary>
		/// <param name="lpcwBuffer"></param>
		/// <param name="pwzName"></param>
		/// <returns></returns>
		[PreserveSig()]
		int GetName(
			ref uint lpcwBuffer,
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwzName
			);
		
		/// <summary>
		/// </summary>
		/// <param name="pdwVersionHi"></param>
		/// <param name="pdwVersionLow"></param>
		/// <returns></returns>
		[PreserveSig()]
		int GetVersion(
			out uint pdwVersionHi,
			out uint pdwVersionLow
			);
		
		/// <summary>
		/// </summary>
		/// <param name="pName"></param>
		/// <param name="dwCmpFlags"></param>
		/// <returns></returns>
		[PreserveSig()]
		int IsEqual(
			IAssemblyName pName,
			uint dwCmpFlags
			);
		
		/// <summary>
		/// </summary>
		/// <param name="pName"></param>
		/// <returns></returns>
		[PreserveSig()]
		int Clone(
			out IAssemblyName pName
			);
	}
}
