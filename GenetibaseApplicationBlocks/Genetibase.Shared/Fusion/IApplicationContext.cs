/* -----------------------------------------------
 * IApplicationContext.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Runtime.InteropServices;

namespace Genetibase.Shared.Fusion
{
	/// <summary>
	/// </summary>
	[ComImport(), Guid("7C23FF90-33AF-11D3-95DA-00A024A85B51"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IApplicationContext
	{
		/// <summary>
		/// </summary>
		/// <param name="pName"></param>
		void SetContextNameObject(
			IAssemblyName pName
			);
		
		/// <summary>
		/// </summary>
		/// <param name="ppName"></param>
		void GetContextNameObject(
			out IAssemblyName ppName
			);
		
		/// <summary>
		/// </summary>
		/// <param name="szName"></param>
		/// <param name="pvValue"></param>
		/// <param name="cbValue"></param>
		/// <param name="dwFlags"></param>
		void Set(
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			int pvValue,
			uint cbValue,
			uint dwFlags
			);
		
		/// <summary>
		/// </summary>
		/// <param name="szName"></param>
		/// <param name="pvValue"></param>
		/// <param name="pcbValue"></param>
		/// <param name="dwFlags"></param>
		void Get(
			[MarshalAs(UnmanagedType.LPWStr)] string szName,
			out int pvValue,
			ref uint pcbValue,
			uint dwFlags
			);
		
		/// <summary>
		/// </summary>
		/// <param name="wzDynamicDir"></param>
		/// <param name="pdwSize"></param>
		void GetDynamicDirectory(
			out int wzDynamicDir,
			ref uint pdwSize
			);
	}
}
