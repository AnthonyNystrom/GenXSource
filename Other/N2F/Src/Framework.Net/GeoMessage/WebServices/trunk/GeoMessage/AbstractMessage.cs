/* ------------------------------------------------
 * AbstractMessage.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.GeoMessage
{
    /// <summary>
    /// Represents a base class for all message descriptors.
    /// </summary>
    public abstract class AbstractMessage
    {
        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        public String MessageText { get; set; }

        /// <summary>
        /// Gets or sets an array of members that will receive this message.
        /// </summary>
        public Member[] Receivers { get; set; }
    }
}
