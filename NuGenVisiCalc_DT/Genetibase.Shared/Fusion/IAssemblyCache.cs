/* -----------------------------------------------
 * IAssemblyCache.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Runtime.InteropServices;

namespace Genetibase.Shared.Fusion
{
	/// <summary>
	/// </summary>
	[ComImport(), Guid("E707DCDE-D1CD-11D2-BAB9-00C04F8ECEAE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	public interface IAssemblyCache
	{
		/// <summary>
		/// </summary>
		/// <param name="dwFlags"></param>
		/// <param name="pszAssemblyName"></param>
		/// <param name="pvReserved"></param>
		/// <param name="pulDisposition"></param>
		/// <returns></returns>
		[PreserveSig()]
		int UninstallAssembly(
			uint dwFlags,
			[MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName,
			IntPtr pvReserved,
			out uint pulDisposition
			);
		
		/// <summary>
		/// </summary>
		/// <param name="dwFlags"></param>
		/// <param name="pszAssemblyName"></param>
		/// <param name="pAsmInfo"></param>
		/// <returns></returns>
		[PreserveSig()]
		int QueryAssemblyInfo(
			uint dwFlags,
			[MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName,
			IntPtr pAsmInfo
			);
		
		/// <summary>
		/// </summary>
		/// <param name="dwFlags"></param>
		/// <param name="pvReserved"></param>
		/// <param name="ppAsmItem"></param>
		/// <param name="pszAssemblyName"></param>
		/// <returns></returns>
		[PreserveSig()]
		int CreateAssemblyCacheItem(
			uint dwFlags,
			IntPtr pvReserved,
			out IAssemblyCacheItem ppAsmItem,
			[MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName
			);
		
		/// <summary>
		/// </summary>
		/// <param name="ppAsmScavenger"></param>
		/// <returns></returns>
		[PreserveSig()]
		int CreateAssemblyScavenger(
			out object ppAsmScavenger
			);
		
		/// <summary>
		/// </summary>
		/// <param name="dwFlags"></param>
		/// <param name="pszManifestFilePath"></param>
		/// <param name="pvReserved"></param>
		/// <returns></returns>
		[PreserveSig()]
		int InstallAssembly(
			uint dwFlags,
			[MarshalAs(UnmanagedType.LPWStr)] string pszManifestFilePath,
			IntPtr pvReserved
			);
	}
}
