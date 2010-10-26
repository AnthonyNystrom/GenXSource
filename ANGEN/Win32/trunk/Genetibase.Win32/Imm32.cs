/* -----------------------------------------------
 * Imm32.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Genetibase.WinApi
{
    /// <summary>
    /// Imports Imm32.dll functions.
    /// </summary>
    public static class Imm32
    {
        private const String DLL = "Imm32.dll";

        /// <summary>
        /// Associates the specified input context with the specified window. By default, the system associates the default input context
        /// with each window as it is created. To specify a type of association, use the ImmAssociateContextEx function.
        /// </summary>
        /// <param name="hWnd">Handle to the window to be associated with the input context.</param>
        /// <param name="hIMC">Handle to the input context. If hIMC is <c>IntPtr.Zero</c>, the function removes any association the window may have with an input context and thus IME cannot be used in the window.</param>
        /// <returns>Returns the handle to the input context previously associated with the window.</returns>
        [DllImport(DLL)]
        public static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        /// <summary>
        /// Creates a new input context, allocating memory for the context and initializing it. An application calls this function to prepare its own input context.
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the new input context. 
        /// If the function fails, the return value is <c>IntPtr.Zero</c>.
        /// </returns>
        [DllImport(DLL)]
        public static extern IntPtr ImmCreateContext();

        /// <summary>
        /// Releases the input context and frees any memory associated with it.
        /// </summary>
        /// <param name="immContext">Handle to the input context to free.</param>
        /// <returns>
        /// If the function succeeds, the return value is a <see langword="true"/> value.
        /// If the function fails, the return value is <see langword="false"/>.
        /// </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DLL)]
        public static extern Boolean ImmDestroyContext(IntPtr immContext);

        /// <summary>
        /// Accesses capabilities of particular IMEs that are not available through other IMM APIs. and is used mainly for country-specific operations.
        /// </summary>
        /// <param name="hkl">Input locale identifier.</param>
        /// <param name="himc">Handle to the input context.</param>
        /// <param name="esc">Index of the operations. For more information, see IME Escapes.</param>
        /// <param name="lpBuf">Pointer to the data required for the escape specified in uEscape. Depending on the escape, when the function returns lpBuf may contain the result of the escape. For more information, see IME Escapes.</param>
        /// <returns>Returns <c>IntPtr.Zero</c> on error; otherwise, returns an operation-specific value.</returns>
        [DllImport(DLL, CharSet = CharSet.Unicode)]
        public static extern IntPtr ImmEscapeW(IntPtr hkl, IntPtr himc, Int32 esc, IntPtr lpBuf);

        /// <summary>
        /// Retrieves information about the composition string.
        /// </summary>
        /// <param name="hIMC"></param>
        /// <param name="dwIndex"></param>
        /// <param name="lpBuf"></param>
        /// <param name="dwBufLen"></param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.I4)]
        [DllImport(DLL, CharSet = CharSet.Unicode)]
        public static extern Int32 ImmGetCompositionStringW(IntPtr hIMC, Int32 dwIndex, StringBuilder lpBuf, Int32 dwBufLen);

        /// <summary>
        /// Retrieves the input context associated with the specified window.
        /// </summary>
        /// <param name="hWnd">Handle to the window for which to retrieve the input context.</param>
        /// <returns>Returns the handle to the input context.</returns>
        [DllImport(DLL)]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        /// <summary>
        /// Notifies the IME about changes to the status of the input context.
        /// </summary>
        /// <param name="immContext">Handle to the input context.</param>
        /// <param name="dwAction">Specifies the notification code. See MSDN for the list of available values.</param>
        /// <param name="dwIndex">Specifies the index of a candidate list or, if dwAction is NI_COMPOSITIONSTR. See MSDN for the list of available values.</param>
        /// <param name="dwValue">Specifies the index of a candidate string or not used, depending on the value of the dwAction parameter.</param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.
        /// If the function fails, the return value is <see langword="false"/>.
        /// </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DLL)]
        public static extern Boolean ImmNotifyIME(IntPtr immContext, Int32 dwAction, Int32 dwIndex, Int32 dwValue);

        /// <summary>
        /// Releases the input context and unlocks the memory associated in the input context. An application must call this function for each call to the <see cref="ImmGetContext"/> function. 
        /// </summary>
        /// <param name="hWnd">Handle to the window for which the input context was previously retrieved.</param>
        /// <param name="hIMC">Handle to the input context.</param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.
        /// If the function fails, the return value is <see langword="false"/>.
        /// </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DLL)]
        public static extern Boolean ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        /// <summary>
        /// Sets the logical font to be used to display characters in the composition window.
        /// </summary>
        /// <param name="hIMC">Handle to the input context.</param>
        /// <param name="lplf">Pointer to a <see cref="LOGFONT"/> structure containing the font information to set.</param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.
        /// If the function fails, the return value is <see langword="false"/>.
        /// </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DLL)]
        public static extern Boolean ImmSetCompositionFontW(IntPtr hIMC, IntPtr lplf);

        /// <summary>
        /// Sets the characters, attributes, and clauses of the composition and reading strings.
        /// </summary>
        /// <param name="hIMC">Handle to the input context.</param>
        /// <param name="dwIndex">Specifies the type of information to set. See MSDN for the list of available values.</param>
        /// <param name="lpComp">Pointer to a buffer containing the information to set for the composition string. The information is as specified by the dwIndex value.</param>
        /// <param name="dwCompLen">Specifies the size, in bytes, of the information buffer for the composition string. This is true even if SCS_SETSTR is specified and the buffer contains a Unicode string.</param>
        /// <param name="lpBuf">Pointer to a buffer containing the information to set for the reading string. The information is as specified by the dwIndex value.</param>
        /// <param name="dwBufLen">Specifies the size, in bytes, of the information buffer for the reading string. The size is in bytes even if SCS_SETSTR is specified and the buffer contains a Unicode string.</param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.
        /// If the function fails, the return value is <see langword="false"/>.
        /// </returns>
        [return: MarshalAs(UnmanagedType.I4)]
        [DllImport(DLL, CharSet = CharSet.Unicode)]
        public static extern Int32 ImmSetCompositionStringW(IntPtr hIMC, Int32 dwIndex, StringBuilder lpComp, Int32 dwCompLen, StringBuilder lpBuf, Int32 dwBufLen);

        /// <summary>
        /// Sets the position of the composition window.
        /// </summary>
        /// <param name="hIMC">Handle to the input context.</param>
        /// <param name="ptr">Pointer to a <see cref="COMPOSITIONFORM"/> structure that contains the new position and other related information about the composition window.</param>
        /// <returns>
        /// If the function succeeds, the return value is <see langword="true"/>.
        /// If the function fails, the return value is <see langword="false"/>.
        /// </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DLL)]
        public static extern Boolean ImmSetCompositionWindow(IntPtr hIMC, IntPtr ptr);
    }
}
