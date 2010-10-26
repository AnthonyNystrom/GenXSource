/* -----------------------------------------------
 * IHistoryReader.cs
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
	[ComImport(), Guid("1D23DF4D-A1E2-4B8B-93D6-6EA3DC285A54"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	public interface IHistoryReader
	{
		/// <summary>
		/// </summary>
		/// <param name="wzFilePath"></param>
		/// <param name="pdwSize"></param>
		/// <returns></returns>
		[PreserveSig()]
		int GetFilePath(
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder wzFilePath,
			ref uint pdwSize
			);
		
		/// <summary>
		/// </summary>
		/// <param name="wzAppName"></param>
		/// <param name="pdwSize"></param>
		/// <returns></returns>
		[PreserveSig()]
		int GetApplicationName(
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder wzAppName,
			ref uint pdwSize
			);
		
		/// <summary>
		/// </summary>
		/// <param name="wzExePath"></param>
		/// <param name="pdwSize"></param>
		/// <returns></returns>
		[PreserveSig()]
		int GetEXEModulePath(
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder wzExePath,
			ref uint pdwSize
			);
		
		/// <summary>
		/// </summary>
		/// <param name="pdwNumActivations"></param>
		void GetNumActivations(
			out uint pdwNumActivations
			);
		
		/// <summary>
		/// </summary>
		/// <param name="dwIdx"></param>
		/// <param name="pftDate"></param>
		void GetActivationDate(
			uint dwIdx, 
			out long pftDate
			);
		
		/// <summary>
		/// </summary>
		/// <param name="pftActivationDate"></param>
		/// <param name="wzRunTimeVersion"></param>
		/// <param name="pdwSize"></param>
		/// <returns></returns>
		[PreserveSig()]
		int GetRunTimeVersion(
			ref long pftActivationDate,
			[MarshalAs(UnmanagedType.LPWStr)] StringBuilder wzRunTimeVersion,
			ref uint pdwSize
			);
		
		/// <summary>
		/// </summary>
		/// <param name="pftActivationDate"></param>
		/// <param name="pdwNumAsms"></param>
		void GetNumAssemblies(
			ref long pftActivationDate,
			out uint pdwNumAsms
			);
		
		/// <summary>
		/// </summary>
		/// <param name="pftActivationDate"></param>
		/// <param name="dwIdx"></param>
		/// <param name="ppHistAsm"></param>
		void GetHistoryAssembly(
			ref long pftActivationDate,
			uint dwIdx,
			[MarshalAs(UnmanagedType.IUnknown)] out object ppHistAsm
			);
	}
}
