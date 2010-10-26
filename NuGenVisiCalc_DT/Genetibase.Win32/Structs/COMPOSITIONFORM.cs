/* -----------------------------------------------
 * COMPOSITIONFORM.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
    /// <summary>
    /// Contains position information for a composition window.
    /// </summary>
    public struct COMPOSITIONFORM
    {
        /// <summary>
        /// <para>Contains the position style. This can be one of the following values:</para>
        /// <para>
        /// CFS_DEFAULT - Move the composition window to the default position.
        /// The IME window can display the composition window outside the client area, such as in a floating window.
        /// </para>
        /// -or-
        /// <para>
        /// CFS_FORCE_POSITION - Display the upper-left corner of the composition window at exactly
        /// the position given by ptCurrentPos. The coordinates are relative to the upper-left corner of
        /// the window containing the composition window and are not subject to adjustment by the IME. 
        /// </para>
        /// -or-
        /// <para>
        /// CFS_POINT - Display the upper-left corner of the composition window at the position given by
        /// ptCurrentPos. The coordinates are relative to the upper-left corner of the window containing
        /// the composition window and are subject to adjustment by the IME. 
        /// </para>
        /// -or-
        /// <para>
        /// CFS_RECT - Display the composition window at the position given by rcArea. The coordinates
        /// are relative to the upper-left corner of the window containing the composition window. 
        /// </para>
        /// </summary>
        public Int32 dwStyle;

        /// <summary>
        /// Contains the coordinates of the upper-left corner of the composition window.
        /// </summary>
        public POINT ptCurrentPos;

        /// <summary>
        /// Contains the coordinates of the upper-left and lower-right corners of the composition window.
        /// </summary>
        public RECT rcArea;
    }
}
