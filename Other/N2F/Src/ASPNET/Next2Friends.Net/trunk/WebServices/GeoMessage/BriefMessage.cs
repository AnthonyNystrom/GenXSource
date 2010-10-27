/* ------------------------------------------------
 * BriefMessageDescriptor.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.WebServices.GeoMessage
{
    /// <summary>
    /// Represents brief message description that is used just to list messages and make available further operations such
    /// as removal or edition providing message identifier.
    /// </summary>
    public sealed class BriefMessage : AbstractMessage
    {
        /// <summary>
        /// Creates a new instance of the <c>BriefMessageDescriptor</c> class.
        /// </summary>
        public BriefMessage()
        {
        }

        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        public Int32 MessageID { get; set; }
    }
}
