/* -----------------------------------------------
 * MONITORINFO.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Genetibase.WinApi
{
    /// <summary>
    /// Contains information about a display monitor.
    /// </summary>
    public struct MONITORINFO
    {
        /// <summary>
        /// The size of the structure, in bytes. 
        /// Set the cbSize member to sizeof(MONITORINFO) before calling the GetMonitorInfo function. Doing so lets the function determine the type of structure you are passing to it.
        /// </summary>
        public Int32 cbSize;
        
        /// <summary>
        /// A <see cref="RECT"/> structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary>
        public RECT rcMonitor;
        
        /// <summary>
        /// A <see cref="RECT"/> structure that specifies the work area rectangle of the display monitor, expressed in virtual-screen coordinates. Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values. 
        /// </summary>
        public RECT rcWork;
        
        /// <summary>
        /// A set of flags that represent attributes of the display monitor. The following flag is defined. 
        /// <para>MONITORINFOF_PRIMARY - This is the primary display monitor.</para>
        /// </summary>
        public Int32 dwFlags;
    }
}
