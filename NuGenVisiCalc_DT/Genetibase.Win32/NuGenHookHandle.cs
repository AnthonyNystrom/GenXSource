/* -----------------------------------------------
 * NuGenHookHandle.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Microsoft.Win32.SafeHandles;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Represents a <see cref="SafeHandle"/> useful for hook operations.
	/// </summary>
	public class NuGenHookHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>
		/// Invokes the <see cref="User32.UnhookWindowsHookEx"/> method to free the handle.
		/// </summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a ReleaseHandleFailed Managed Debugging Assistant.
		/// </returns>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override Boolean ReleaseHandle()
		{
			return User32.UnhookWindowsHookEx(handle);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHookHandle"/> class.
		/// </summary>
		/// <param name="idHook">Specifies the type of hook procedure to be installed. See the SetWindowsHookEx Function article on MSDN for more info.</param>
		/// <param name="callbackProc">Specifies the method to be installed into a hook chain.</param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="callbackProc"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenHookHandle(Int32 idHook, HOOKPROC callbackProc)
			: base(true)
		{
			if (callbackProc == null)
			{
				throw new ArgumentNullException("proc");
			}

			using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
			{
				this.SetHandle(
					User32.SetWindowsHookEx(
						idHook
						, callbackProc
						, Kernel32.GetModuleHandle(curModule.ModuleName)
						, 0
					)
				);
			}
		}
	}
}
