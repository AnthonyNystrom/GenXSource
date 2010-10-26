/* -----------------------------------------------
 * GlobalHook.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Properties;
using Genetibase.WinApi;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	internal class GlobalHook : IDisposable
	{
		#region Methods.Public

		public void InstallKeyboardHook()
		{
			m_keyboardHookProcedure = new HOOKPROC(KeyboardHookProc);
			m_keyboardHook = new NuGenHookHandle(WinUser.WH_KEYBOARD_LL, m_keyboardHookProcedure);

			if (m_keyboardHook.IsInvalid)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), Resources.Win32_InvalidKbdLLHookHandle);
			}
		}

		public void RemoveKeyboardHook()
		{
			if (m_keyboardHook != null && !m_keyboardHook.IsInvalid)
			{
				m_keyboardHook.Close();
			}
		}

		#endregion

		#region Methods.Private

		private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			// it was ok and someone listens to events
			if ((nCode >= 0) && (KeyDown != null || KeyUp != null || KeyPress != null))
			{
				KBDLLHOOKSTRUCT MyKeyboardHookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
				// KeyDown
				if ((KeyDown != null) && (wParam == (IntPtr)WinUser.WM_KEYDOWN || wParam == (IntPtr)WinUser.WM_SYSKEYDOWN))
				{
					Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
					KeyEventArgs e = new KeyEventArgs(keyData);
					this.KeyDown(this, e);
				}

				// KeyPress
				if ((KeyPress != null) && (wParam == (IntPtr)WinUser.WM_KEYDOWN))
				{
					byte[] keyState = new byte[256];
					User32.GetKeyboardState(keyState);

					byte[] inBuffer = new byte[2];

					if (User32.ToAscii(MyKeyboardHookStruct.vkCode,
						MyKeyboardHookStruct.scanCode,
						keyState,
						inBuffer,
						MyKeyboardHookStruct.flags == 1) == 1)
					{
						KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
						KeyPress(this, e);
					}
				}

				// KeyUp
				if ((KeyUp != null) && (wParam == (IntPtr)WinUser.WM_KEYUP || wParam == (IntPtr)WinUser.WM_SYSKEYUP))
				{
					Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
					KeyEventArgs e = new KeyEventArgs(keyData);
					KeyUp(this, e);
				}
			}
			return User32.CallNextHookEx(m_keyboardHook, nCode, wParam, lParam);
		}

		#endregion

		private bool disposed;
		private HOOKPROC m_keyboardHookProcedure;
		private NuGenHookHandle m_keyboardHook;

		public event KeyEventHandler KeyUp;
		public event KeyEventHandler KeyDown;
		public event KeyPressEventHandler KeyPress;

		public GlobalHook()
		{
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					RemoveKeyboardHook();
				}
				// shared cleanup logic
				disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
