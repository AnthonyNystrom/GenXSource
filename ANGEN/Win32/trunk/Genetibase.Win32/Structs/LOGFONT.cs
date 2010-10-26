/* -----------------------------------------------
 * LOGFONT.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Genetibase.WinApi
{
    /// <summary>
    /// Defines the attributes of a font. See MSDN for more info.
    /// </summary>
    public class LOGFONT
    {
        /// <summary>
        /// </summary>
        public Int32 lfHeight;

        /// <summary>
        /// </summary>
        public Int32 lfWidth;

        /// <summary>
        /// </summary>
        public Int32 lfEscapement;

        /// <summary>
        /// </summary>
        public Int32 lfOrientation;

        /// <summary>
        /// </summary>
        public Int32 lfWeight;

        /// <summary>
        /// </summary>
        public Byte lfItalic;

        /// <summary>
        /// </summary>
        public Byte lfUnderline;

        /// <summary>
        /// </summary>
        public Byte lfStrikeOut;

        /// <summary>
        /// </summary>
        public Byte lfCharSet;

        /// <summary>
        /// </summary>
        public Byte lfOutPrecision;

        /// <summary>
        /// </summary>
        public Byte lfClipPrecision;

        /// <summary>
        /// </summary>
        public Byte lfQuality;

        /// <summary>
        /// </summary>
        public Byte lfPitchAndFamily;

        /// <summary>
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public String lfFaceName;
    }
}
