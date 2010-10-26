/* -----------------------------------------------
 * Common.cs
 * Copyright © 2005-2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines helper methods and constants to interact with Win32 API.
	/// </summary>
	public static class Common
	{
		/// <summary>
		/// Zero value.
		/// </summary>
		public static readonly IntPtr FALSE = new IntPtr(0);
		
		/// <summary>
		/// Non-zero value.
		/// </summary>
		public static readonly IntPtr TRUE = new IntPtr(1);

		/// <summary>
		/// Creates an unsigned 32-bit value.
		/// </summary>
		/// <param name="wLow">Specifies the low-order word of the new long value.</param>
		/// <param name="wHigh">Specifies the high-order word of the new long value.</param>
		public static Int32 MakeLong(Int32 wLow, Int32 wHigh)
		{
			return (wHigh << 16) | (wLow & 0xffff);
		}
 
		/// <summary>
		/// Creates an unsigned 32-bit value for use as an lParam parameter in a message.  
		/// </summary>
		/// <param name="wLow">Specifies the low-order word of the new long value.</param>
		/// <param name="wHigh">Specifies the high-order word of the new long value.</param>
		public static IntPtr MakeLParam(Int32 wLow, Int32 wHigh) 
		{ 
			return (IntPtr)((wHigh << 16) | (wLow & 0xffff));
		}
 
		/// <summary>
		/// Retrieves the high-order word from the given 32-bit value.
		/// </summary>
		/// <param name="dwValue">Specifies the value to be converted.</param>
		public static Int32 HiWord(Int32 dwValue) 
		{ 
			return (dwValue >> 16) & 0xffff;
		} 

		/// <summary>
		/// Retrieves the low-order word from the given 32-bit value.
		/// </summary>
		/// <param name="dwValue">Specifies the value to be converted.</param>
		public static Int32 LoWord(Int32 dwValue) 
		{ 
			return dwValue & 0xffff;
		}
	}
}
