/* ------------------------------------------------
 * MessageType.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.WebServices.GeoMessage
{
    /// <summary>
    /// Defines possible types of geographically dependent messages.
    /// </summary>
    public static class MessageType
    {
        /// <summary>
        /// Default value used to check whether the type of the message was initialized.
        /// </summary>
        public const Int32 None = 0;

        /// <summary>
        /// The message will be sent when the target enters the specified area.
        /// </summary>
        public const Int32 EntersTheArea = 1;

        /// <summary>
        /// The message will be sent when the target leaves the specified area.
        /// </summary>
        public const Int32 LeavesTheArea = 2;
    }
}
