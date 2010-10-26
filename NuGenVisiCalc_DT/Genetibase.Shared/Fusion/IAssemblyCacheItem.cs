/* -----------------------------------------------
 * IAssemblyCacheItem.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Genetibase.Shared.Fusion
{
	/// <summary>
	/// </summary>
	[ComImport(), Guid("9E3AAEB4-D1CD-11D2-BAB9-00C04F8ECEAE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	public interface IAssemblyCacheItem
	{
		/// <summary>
		/// </summary>
		/// <param name="pszName"></param>
		/// <param name="dwFormat"></param>
		/// <param name="dwFlags"></param>
		/// <param name="dwMaxSize"></param>
		/// <param name="ppStream"></param>
		void CreateStream(
			[MarshalAs(UnmanagedType.LPWStr)] string pszName,
			uint dwFormat,
			uint dwFlags,
			uint dwMaxSize,
			out IStream ppStream
			);
		
		/// <summary>
		/// </summary>
		/// <param name="pName"></param>
		void IsNameEqual(
			IAssemblyName pName
			);
		
		/// <summary>
		/// </summary>
		/// <param name="dwFlags"></param>
		void Commit(
			uint dwFlags
			);
		
		/// <summary>
		/// </summary>
		/// <param name="dwFlags"></param>
		void MarkAssemblyVisible(
			uint dwFlags
			);
	}
}
