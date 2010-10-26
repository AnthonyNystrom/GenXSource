/* -----------------------------------------------
 * IAssemblyEnum.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Runtime.InteropServices;

namespace Genetibase.Shared.Fusion
{
	/// <summary>
	/// </summary>
	[ComImport(), Guid("21B8916C-F28E-11D2-A473-00C04F8EF448"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	public interface IAssemblyEnum
	{
		/// <summary>
		/// </summary>
		/// <param name="ppAppCtx"></param>
		/// <param name="ppName"></param>
		/// <param name="dwFlags"></param>
		/// <returns></returns>
		[PreserveSig()]
		int GetNextAssembly(
			out IApplicationContext ppAppCtx,
			out IAssemblyName ppName,
			uint dwFlags
			);
		
		/// <summary>
		/// </summary>
		/// <returns></returns>
		[PreserveSig()]
		int Reset();
		
		/// <summary>
		/// </summary>
		/// <param name="ppEnum"></param>
		/// <returns></returns>
		[PreserveSig()]
		int Clone(
			out IAssemblyEnum ppEnum
			);
	}
}
