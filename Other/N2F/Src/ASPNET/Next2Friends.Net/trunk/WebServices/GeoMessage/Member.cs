/* ------------------------------------------------
 * FriendDescriptor.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;

namespace Next2Friends.WebServices.GeoMessage
{
    /// <summary>
    /// Encapsulates member identifier and human-readable member name.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Creates a new instance of the <c>Member</c> class.
        /// </summary>
        public Member()
        {
        }

        /// <summary>
        /// Creates a new instance of the <c>Member</c> class.
        /// </summary>
        /// <param name="id">Specifies the identifier for this member, that is <c>WebMemberID</c> field value in Next2Friends database.</param>
        /// <param name="name">Specifies the name for this member, that is <c>NickName</c> field value in Next2Friends database.</param>
        public Member(String id, String name)
        {
            ID = id;
            Name = name;
        }

        /// <summary>
        /// Gets or sets this member identifier.
        /// </summary>
        public String ID { get; set; }

        /// <summary>
        /// Gets or sets the human-readable member name.
        /// </summary>
        public String Name { get; set; }
    }
}
