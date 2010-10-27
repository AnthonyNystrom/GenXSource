/* ------------------------------------------------
 * MessageDescriptor.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.WebServices.GeoMessage
{
    /// <summary>
    /// Represents the message descriptor that is used to create, edit messages
    /// </summary>
    public sealed class Message : AbstractMessage
    {
        /// <summary>
        /// Creates a new instance of the <c>Message</c> instance.
        /// </summary>
        public Message()
        {
        }

        /// <summary>
        /// Gets or sets the type of the message.
        /// The following values are allowed and can be combined:
        /// 1 - for the message that will be sent when the target enters the area.
        /// 2 - for the message that will be sent when the target leaves the area.
        /// </summary>
        public Int32 MessageType { get; set; }

        /// <summary>
        /// Gets or sets the measure units for <c>spot</c>.
        /// The following values are allowed:
        /// 1 - Feets
        /// 2 - Meters
        /// </summary>
        public Int32 MeasureUnits { get; set; }

        /// <summary>
        /// Gets or sets the radius in feets or meters to indicate the area the message will be valid within.
        /// </summary>
        public Single Spot { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the message will be repeated or initiated only once.
        /// </summary>
        public Boolean ShouldRepeat { get; set; }

        /// <summary>
        /// Gets or sets the number of times the message will be repeated.
        /// Ignored if <c>shouldRepeat</c> is <c>false</c>.
        /// Specify <c>0</c> to always repeat this message.
        /// </summary>
        public Int32 RepeatTimes { get; set; }

        /// <summary>
        /// Gets or sets the current user location.
        /// </summary>
        public Location CurrentLocation { get; set; }
    }
}
