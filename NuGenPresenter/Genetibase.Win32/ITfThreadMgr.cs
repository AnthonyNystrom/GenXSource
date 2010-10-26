/* -----------------------------------------------
 * ITfThreadMgr.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Genetibase.WinApi
{
    /// <summary>
    /// Defines the primary object implemented by the TSF manager. ITfThreadMgr is used by applications and text services to activate and deactivate text services, create document managers, and maintain the document context focus.
    /// </summary>
    [ComImport, Guid("aa80e801-2021-11d2-93e0-0060b067b86e"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITfThreadMgr
    {
        /// <summary>
        /// Activates TSF for the calling thread.
        /// </summary>
        /// <param name="clientId"></param>
        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        void Activate(out Int32 clientId);

        /// <summary>
        /// Deactivates TSF for the calling thread.
        /// </summary>
        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        void Deactivate();

        /// <summary>
        /// Creates a document manager object.
        /// </summary>
        /// <param name="docMgr"></param>
        [SuppressUnmanagedCodeSecurity, SecurityCritical]
        void CreateDocumentMgr(out Object docMgr);

        /// <summary>
        /// Returns an enumerator for all the document managers within the calling thread.
        /// </summary>
        /// <param name="enumDocMgrs"></param>
        void EnumDocumentMgrs(out Object enumDocMgrs);

        /// <summary>
        /// Returns the document manager that has the input focus.
        /// </summary>
        /// <param name="docMgr"></param>
        void GetFocus(out IntPtr docMgr);

        /// <summary>
        /// Sets the input focus to the specified document manager.
        /// </summary>
        /// <param name="docMgr"></param>
        [SecurityCritical, SuppressUnmanagedCodeSecurity]
        void SetFocus(IntPtr docMgr);

        /// <summary>
        /// Associates the focus for a window with a document manager object.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="newDocMgr"></param>
        /// <param name="prevDocMgr"></param>
        void AssociateFocus(IntPtr hwnd, Object newDocMgr, out Object prevDocMgr);

        /// <summary>
        /// Determines if the calling thread has the TSF input focus.
        /// </summary>
        /// <param name="isFocus"></param>
        void IsThreadFocus([MarshalAs(UnmanagedType.Bool)] out Boolean isFocus);

        /// <summary>
        /// Obtains the specified function provider object.
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="funcProvider"></param>
        /// <returns></returns>
        [PreserveSig, SecurityCritical, SuppressUnmanagedCodeSecurity]
        Int32 GetFunctionProvider(ref Guid classId, out Object funcProvider);

        /// <summary>
        /// Obtains an enumerator for all of the function providers registered for the calling thread.
        /// </summary>
        /// <param name="enumProviders"></param>
        void EnumFunctionProviders(out Object enumProviders);

        /// <summary>
        /// Obtains the global compartment manager object.
        /// </summary>
        /// <param name="compartmentMgr"></param>
        [SuppressUnmanagedCodeSecurity, SecurityCritical]
        void GetGlobalCompartment(out Object compartmentMgr);
    }
}
