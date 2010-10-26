/* -----------------------------------------------
 * Msctr.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Genetibase.WinApi
{
    /// <summary>
    /// Imports MSCTF.dll functions.
    /// </summary>
    public static class Msctr
    {
        private const String DLL = "Msctf.dll";

        /// <summary>
        /// Creates a thread manager object without having to initialize COM. Usage of this method is not recommended, because the calling process must maintain a proper reference count on an object that is owned by Msctf.dll.
        /// </summary>
        /// <param name="threadMgr"></param>
        /// <returns></returns>
        [DllImport(DLL)]
        public static extern Int32 TF_CreateThreadMgr(out ITfThreadMgr threadMgr);
    }
}
