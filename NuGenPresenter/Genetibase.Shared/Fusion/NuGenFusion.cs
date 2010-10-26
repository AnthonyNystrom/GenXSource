/* -----------------------------------------------
 * NuGenFusion.cs
 * Copyright � 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Fusion;

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Genetibase.Shared.Fusion
{
	/// <summary>
	/// Provides methods to work with the GAC.
	/// </summary>
	[CLSCompliant(false)]
	public static class NuGenFusion
	{
		/*
		 * CreateApplicationContext
		 */

		/// <summary>
		/// No documentation available.
		/// </summary>
		/// <param name="ppAppContext"></param>
		/// <param name="dw"></param>
		/// <returns></returns>
		[DllImport("fusion.dll")]
		public static extern int CreateApplicationContext(
			out IApplicationContext ppAppContext,
			uint dw
			);
		
		/*
		 * CreateAssemblyCache
		 */
			
		/// <summary>
		/// Gets a pointer to a new <see cref="IAssemblyCache"/> instance representing the Global Assembly Cache.
		/// </summary>
		/// <param name="ppAsmCache">The returned <see cref="IAssemblyCache"/> pointer.</param>
		/// <param name="dwReserved">Reserved for future extensibility. Must be zero.</param>
		/// <returns></returns>
		[DllImport("fusion.dll", CharSet=CharSet.Auto)]
		public static extern int CreateAssemblyCache(
			out IAssemblyCache ppAsmCache,
			uint dwReserved
			);
		
		/*
		 * CreateAssemblyEnum
		 */

		/// <summary>
		/// Gets a pointer to an <see cref="IAssemblyEnum"/> for the assembly with the specified
		/// <see cref="IAssemblyName"/>.
		/// </summary>
		/// <param name="ppEnum">The requested <see cref="IAssemblyEnum"/> pointer.</param>
		/// <param name="pAppCtx">No documentation available.</param>
		/// <param name="pName">The <see cref="IAssemblyName"/> of the requested assembly.</param>
		/// <param name="dwFlags">Flags for modification of the enumerator's behavior.</param>
		/// <param name="pvReserved">Reserved for future extensibility. Must be a <see langword="null"/> reference.</param>
		[DllImport("fusion.dll", CharSet=CharSet.Auto)]
		public static extern int CreateAssemblyEnum(
			out IAssemblyEnum ppEnum,
			IApplicationContext pAppCtx,
			IAssemblyName pName,
			uint dwFlags,
			int pvReserved
			);
		
		/*
		 * CreateAssemblyNameObject
		 */

		/// <summary>
		/// Gets an interface pointer to an <see cref="IAssemblyName"/>
		/// instance that represents the unique identity of the assembly with the specified name.
		/// </summary>
		/// <param name="ppName">The returned <see cref="IAssemblyName"/>.</param>
		/// <param name="szAssemblyName">The name of the assembly for which to create the new <see cref="IAssemblyName"/> instance.</param>
		/// <param name="dwFlags">Flags to pass to the object constructor.</param>
		/// <param name="pvReserved">Reserved for future extensibility. Must be a <see langword="null"/> reference.</param>
		/// <returns></returns>
		[DllImport("fusion.dll", CharSet=CharSet.Auto)]
		public static extern int CreateAssemblyNameObject(
			out IAssemblyName ppName,
			string szAssemblyName,
			uint dwFlags,
			int pvReserved
			);
		
		/*
		 * CreateHistoryReader
		 */

		/// <summary>
		/// No documentation available.
		/// </summary>
		/// <param name="wzFilePath"></param>
		/// <param name="ppHistReader"></param>
		/// <returns></returns>
		[DllImport("fusion.dll", CharSet=CharSet.Auto)]
		public static extern int CreateHistoryReader(
			string wzFilePath,
			out IHistoryReader ppHistReader
			);

		/*
		 * GetCachePath
		 */

		/// <summary>
		/// Gets the path to the cached assembly, using the specified flags.
		/// </summary>
		/// <param name="wzDir">The returned pointer to the path.</param>
		/// <param name="pdwSize">No documentation available.</param>
		/// <param name="dwReserved">Reserved for future extensibility. Must be a <see langword="null"/> reference.</param>
		/// <returns></returns>
		[DllImport("fusion.dll")]
		public static extern int GetCachePath(
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder wzDir,
			ref uint pdwSize,
			uint dwReserved
			);

		/*
		 * GetHistoryFileDirectory
		 */

		/// <summary>
		/// Retrieves the path of the ApplicationHistory folder, typically
		/// Documents and Settings\&lt;User&gt;\Local Settings\Application Data\ApplicationHistory
		/// containing *.ini files that can be read with <see cref="IHistoryReader"/>.
		/// </summary>
		/// <param name="wzDir">The returned pointer to the path.</param>
		/// <param name="pdwSize">The offset of the last backslash in the returned string after the call.</param>
		/// <returns>If the function succeeded, the return value is S_OK; otherwise, error HRESULT.</returns>
		[DllImport("fusion.dll", CharSet=CharSet.Unicode)]
		public static extern int GetHistoryFileDirectory(
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder wzDir,
			ref uint pdwSize
			);
		
		/*
		 * NukeDownloadedCache
		 */

		/// <summary>
		/// No documentation available.
		/// </summary>
		/// <returns></returns>
		[DllImport("fusion.dll")]
		public static extern int NukeDownloadedCache();
		
		/*
		 * PolicyManager
		 */

		/// <summary>
		/// No documentation available.
		/// </summary>
		/// <param name="hWndParent"></param>
		/// <param name="pwzFullyQualifiedAppPath"></param>
		/// <param name="pwzAppName"></param>
		/// <param name="dwFlags"></param>
		/// <returns></returns>
		[DllImport("shfusion.dll", CharSet=CharSet.Unicode)]
		public static extern uint PolicyManager(
			IntPtr hWndParent,
			string pwzFullyQualifiedAppPath,
			string pwzAppName,
			int dwFlags
			);		
	}
}
