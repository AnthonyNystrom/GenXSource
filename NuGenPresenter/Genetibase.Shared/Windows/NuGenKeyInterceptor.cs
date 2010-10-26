/* -----------------------------------------------
 * NuGenKeyInterceptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Genetibase.Shared.Properties;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Installs a low-level keyboard hook and filters the user input looking for the specified keyboard combinations.
	/// </summary>
	public class NuGenKeyInterceptor : IDisposable
	{
		/// <summary>
		/// </summary>
		public IList<NuGenHotKeyOperation> Operations
		{
			get
			{
				return _hotKeys.Operations;
			}
		}

		private Keys GetKey(int keyCode)
		{
			Keys keys = (Keys)keyCode;

			switch (keys)
			{
				case Keys.RControlKey:
				case Keys.LControlKey:
				{
					return Keys.Control;
				}
				case Keys.RShiftKey:
				case Keys.LShiftKey:
				{
					return Keys.Shift;
				}
				case Keys.RMenu:
				case Keys.LMenu:
				{
					return Keys.Alt;
				}
				default:
				{
					return keys;
				}
			}
		}

		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0)
			{
				if (wParam == (IntPtr)WinUser.WM_KEYDOWN)
				{
					_hotKeys.KeyDown(this.GetKey(Marshal.ReadInt32(lParam)));

				}
				else if (wParam == (IntPtr)WinUser.WM_KEYUP)
				{
					_hotKeys.KeyUp(this.GetKey(Marshal.ReadInt32(lParam)));
				}
			}

			return User32.CallNextHookEx(_handle, nCode, wParam, lParam);
		}

		private NuGenHotKeysLL _hotKeys;
		private NuGenHookHandle _handle;
		private HOOKPROC _hookCallBack;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenKeyInterceptor"/> class.
		/// </summary>
		public NuGenKeyInterceptor()
		{
			_hotKeys = new NuGenHotKeysLL();
			_hookCallBack = new HOOKPROC(this.HookCallback);
			_handle = new NuGenHookHandle(WinUser.WH_KEYBOARD_LL, _hookCallBack);

			if (_handle.IsInvalid)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), Resources.Win32_InvalidKbdLLHookHandle);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		protected virtual void Dispose(bool disposing)
		{
			if (_handle != null && !_handle.IsInvalid)
			{
				_handle.Dispose();
			}
		}
	}
}
